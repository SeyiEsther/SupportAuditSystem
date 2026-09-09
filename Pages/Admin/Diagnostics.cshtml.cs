using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SupportAuditSystem.Data;
using SupportAuditSystem.Services;

namespace SupportAuditSystem.Pages.Admin
{
    // Answers two questions the app is otherwise silent about: is the running
    // binary the current build, and did the data migrate? Both have caused
    // "the screen hasn't changed" reports that looked like rendering bugs.
    public class DiagnosticsModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly AdminService _admin;

        public DiagnosticsModel(AppDbContext db, AdminService admin)
        {
            _db = db;
            _admin = admin;
        }

        public record TaskListRow(
            string Department, string Area, string Shift, int Version,
            int Items, int TimeBoxed, int Categorised, int GridRows, int Cards);

        public string BuildTime { get; set; } = "";
        public string AssemblyVersion { get; set; } = "";
        public List<string> Applied { get; set; } = new();
        public List<string> Pending { get; set; } = new();
        public List<TaskListRow> TaskLists { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (!_admin.IsAdmin()) return RedirectToPage("/Index");

            var asm = Assembly.GetExecutingAssembly();
            AssemblyVersion = asm.GetName().Version?.ToString() ?? "unknown";
            try
            {
                BuildTime = System.IO.File.GetLastWriteTime(asm.Location).ToString("dd MMM yyyy HH:mm:ss");
            }
            catch { BuildTime = "unknown"; }

            Applied = (await _db.Database.GetAppliedMigrationsAsync()).OrderBy(m => m).ToList();
            Pending = (await _db.Database.GetPendingMigrationsAsync()).OrderBy(m => m).ToList();

            var lists = await _db.TaskLists
                .Where(t => t.IsCurrent)
                .Include(t => t.Items)
                .Include(t => t.Area).ThenInclude(a => a!.Department)
                .Include(t => t.Shift)
                .ToListAsync();

            // Mirrors the split in ChecklistModel exactly — a time-boxed item that
            // carries a category goes in the grid, everything else is a card.
            TaskLists = lists
                .Select(t =>
                {
                    var grid = t.Items.Count(i => i.IsTimeBoxed && !string.IsNullOrEmpty(i.Category));
                    return new TaskListRow(
                        t.Area?.Department?.Name ?? "—",
                        t.Area?.Name ?? "—",
                        t.Shift?.Name ?? "—",
                        t.Version,
                        t.Items.Count,
                        t.Items.Count(i => i.IsTimeBoxed),
                        t.Items.Count(i => !string.IsNullOrEmpty(i.Category)),
                        grid,
                        t.Items.Count - grid);
                })
                .OrderBy(r => r.Department).ThenBy(r => r.Area).ThenBy(r => r.Shift)
                .ToList();

            return Page();
        }
    }
}
