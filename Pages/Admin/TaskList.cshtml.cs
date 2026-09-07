using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SupportAuditSystem.Data;
using SupportAuditSystem.Models;
using SupportAuditSystem.Services;

namespace SupportAuditSystem.Pages.Admin
{
    public class TaskListModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly AdminService _admin;
        private readonly TaskListService _lists;
        private readonly UserService _users;

        public TaskListModel(AppDbContext db, AdminService admin, TaskListService lists, UserService users)
        {
            _db = db;
            _admin = admin;
            _lists = lists;
            _users = users;
        }

        public Area? Area { get; set; }
        public Shift? Shift { get; set; }
        public Models.TaskList? Current { get; set; }
        public string DefaultReminder => PdfExportService.DefaultReminder;

        [BindProperty] public int AreaId { get; set; }
        [BindProperty] public int ShiftId { get; set; }
        [BindProperty] public string? ItemsJson { get; set; }
        [BindProperty] public string? Reminder { get; set; }

        public async Task<IActionResult> OnGetAsync(int areaId, int shiftId)
        {
            if (!_admin.IsAdmin()) return RedirectToPage("/Index");
            AreaId = areaId;
            ShiftId = shiftId;
            await LoadAsync();
            if (Area == null || Shift == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!_admin.IsAdmin()) return Forbid();

            var items = new List<EditedTaskItem>();
            if (!string.IsNullOrWhiteSpace(ItemsJson))
            {
                try
                {
                    var parsed = JsonSerializer.Deserialize<List<EditedTaskItem>>(ItemsJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (parsed != null) items = parsed;
                }
                catch { /* fall through with empty list */ }
            }

            var reminder = string.IsNullOrWhiteSpace(Reminder) ? null : Reminder.Trim();
            await _lists.SaveAsync(AreaId, ShiftId, items, reminder, _users.GetCurrentUser());
            TempData["Saved"] = "Task list saved.";
            return RedirectToPage(new { areaId = AreaId, shiftId = ShiftId });
        }

        private async Task LoadAsync()
        {
            Area = await _db.Areas.Include(a => a.Department).FirstOrDefaultAsync(a => a.Id == AreaId);
            Shift = await _db.Shifts.FirstOrDefaultAsync(s => s.Id == ShiftId);
            Current = await _lists.GetCurrentAsync(AreaId, ShiftId);
        }
    }
}
