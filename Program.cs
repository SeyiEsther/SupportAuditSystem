using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using SupportAuditSystem.Data;
using SupportAuditSystem.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();

// Persist Data Protection keys outside the app folder so an app-pool recycle or
// a redeploy can't regenerate the keys and invalidate in-flight antiforgery
// tokens (which would turn a save into an HTTP 400). Pick the first writable
// candidate folder.
static string? ResolveWritableKeyDir(params string?[] candidates)
{
    foreach (var c in candidates)
    {
        if (string.IsNullOrWhiteSpace(c)) continue;
        try
        {
            Directory.CreateDirectory(c);
            var probe = Path.Combine(c, ".writetest");
            File.WriteAllText(probe, "ok");
            File.Delete(probe);
            return c;
        }
        catch { /* try the next candidate */ }
    }
    return null;
}

var keyDir = ResolveWritableKeyDir(
    builder.Configuration["DataProtection:KeyPath"],
    Path.Combine(builder.Environment.ContentRootPath, "..", "SupportAudit-dataprotection-keys"),
    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "SupportAudit", "dataprotection-keys"));

var keysPersisted = keyDir != null;
if (keysPersisted)
{
    builder.Services.AddDataProtection()
        .SetApplicationName("SupportAudit-Portal")
        .PersistKeysToFileSystem(new DirectoryInfo(keyDir!));
}

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<ChecklistService>();
builder.Services.AddScoped<TaskListService>();
builder.Services.AddScoped<PdfExportService>();
builder.Services.AddScoped<RosterService>();

builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.ValueCountLimit = 20_000;
    options.ValueLengthLimit = 1024 * 1024;
    options.MultipartBodyLengthLimit = 32 * 1024 * 1024;
});

builder.Services.AddControllers().AddJsonOptions(o =>
    o.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    options.Cookie.MaxAge = TimeSpan.FromHours(12);
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
});

builder.Services.AddRazorPages();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");

    if (keysPersisted)
        logger.LogInformation("Data Protection keys persisted to {Path}.", keyDir);
    else
        logger.LogError("Data Protection keys are NOT being persisted — restarts will invalidate open forms.");

    if (db.Database.IsRelational())
    {
        try
        {
            db.Database.Migrate();
            logger.LogInformation("Database migrations applied successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Database migration failed — run dotnet ef database update on the server.");
            throw;
        }
    }
}

// Turn the empty-bodied HTTP 400 from an expired antiforgery token into a
// friendly page for HTML page POSTs; genuine JSON 400s keep their body.
app.UseStatusCodePages(async ctx =>
{
    var res = ctx.HttpContext.Response;
    var req = ctx.HttpContext.Request;
    var wantsHtml = req.Headers.Accept.Any(h => h != null && h.Contains("text/html", StringComparison.OrdinalIgnoreCase));
    if (res.StatusCode == 400
        && HttpMethods.IsPost(req.Method)
        && wantsHtml
        && !req.Path.StartsWithSegments("/api"))
    {
        res.ContentType = "text/html; charset=utf-8";
        await res.WriteAsync(StartupHtml.SessionRefreshedHtml);
    }
});

app.UseStaticFiles();
app.MapControllers();
app.MapRazorPages();

app.Run();

static class StartupHtml
{
    public const string SessionRefreshedHtml = """
<!doctype html><html lang="en"><head><meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1">
<title>Session refreshed</title>
<style>
  body{font-family:'Segoe UI',system-ui,sans-serif;background:#eef0f3;margin:0;
       display:flex;min-height:100vh;align-items:center;justify-content:center;color:#1a1a1a}
  .card{background:#fff;max-width:460px;margin:20px;padding:28px 30px;border-radius:14px;
        box-shadow:0 6px 24px rgba(0,0,0,.12);border-top:5px solid #CC1F2C}
  h1{font-size:20px;margin:0 0 8px}
  p{font-size:14px;line-height:1.55;color:#374151;margin:0 0 12px}
  .btn{display:inline-block;background:#CC1F2C;color:#fff;border:0;border-radius:9px;
       padding:10px 18px;font-size:14px;font-weight:700;cursor:pointer;text-decoration:none}
  .muted{font-size:12px;color:#6b7280;margin-top:14px}
</style></head><body>
  <div class="card">
    <h1>Your session refreshed</h1>
    <p>The app updated while this form was open, so this save was declined for security.
       <strong>Nothing was lost.</strong></p>
    <p>Reload the page and submit again. Anything you typed is saved in this browser
       and should reappear when the page loads.</p>
    <button class="btn" onclick="history.back()">Go back &amp; retry</button>
    <div class="muted">If it keeps happening, close the tab, reopen the form from the menu, and try once more.</div>
  </div>
</body></html>
""";
}

public partial class Program { }
