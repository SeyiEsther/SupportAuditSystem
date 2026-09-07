using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SupportAuditSystem.Data;
using SupportAuditSystem.Models;
using SupportAuditSystem.Services;

namespace SupportAuditSystem.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly AdminService _admin;

        public IndexModel(AppDbContext db, AdminService admin)
        {
            _db = db;
            _admin = admin;
        }

        public List<Department> Departments { get; set; } = new();
        // Which area+shift pairs already have a current task list, and its size.
        public Dictionary<(int AreaId, int ShiftId), (int Version, int Items)> TaskListInfo { get; set; } = new();
        public string? Notice { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!_admin.IsAdmin()) return RedirectToPage("/Index");
            await LoadAsync();
            return Page();
        }

        // ── Departments ─────────────────────────────────────────────
        public async Task<IActionResult> OnPostAddDepartmentAsync(string name)
        {
            if (!_admin.IsAdmin()) return Forbid();
            if (!string.IsNullOrWhiteSpace(name))
            {
                var max = await _db.Departments.MaxAsync(d => (int?)d.SortOrder) ?? 0;
                _db.Departments.Add(new Department { Name = name.Trim(), SortOrder = max + 1 });
                await SaveGuardedAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditDepartmentAsync(int id, string name, bool isActive)
        {
            if (!_admin.IsAdmin()) return Forbid();
            var d = await _db.Departments.FindAsync(id);
            if (d != null)
            {
                d.Name = name.Trim();
                d.IsActive = isActive;
                await SaveGuardedAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteDepartmentAsync(int id)
        {
            if (!_admin.IsAdmin()) return Forbid();
            var d = await _db.Departments.FindAsync(id);
            if (d != null) { _db.Departments.Remove(d); await SaveGuardedAsync(); }
            return RedirectToPage();
        }

        // ── Areas ───────────────────────────────────────────────────
        public async Task<IActionResult> OnPostAddAreaAsync(int departmentId, string name, string? defaultLocation)
        {
            if (!_admin.IsAdmin()) return Forbid();
            if (!string.IsNullOrWhiteSpace(name))
            {
                var max = await _db.Areas.Where(a => a.DepartmentId == departmentId).MaxAsync(a => (int?)a.SortOrder) ?? 0;
                _db.Areas.Add(new Area { DepartmentId = departmentId, Name = name.Trim(), DefaultLocation = defaultLocation?.Trim(), SortOrder = max + 1 });
                await SaveGuardedAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAreaAsync(int id, string name, string? defaultLocation, bool isActive)
        {
            if (!_admin.IsAdmin()) return Forbid();
            var a = await _db.Areas.FindAsync(id);
            if (a != null)
            {
                a.Name = name.Trim();
                a.DefaultLocation = string.IsNullOrWhiteSpace(defaultLocation) ? null : defaultLocation.Trim();
                a.IsActive = isActive;
                await SaveGuardedAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAreaAsync(int id)
        {
            if (!_admin.IsAdmin()) return Forbid();
            var a = await _db.Areas.FindAsync(id);
            if (a != null) { _db.Areas.Remove(a); await SaveGuardedAsync(); }
            return RedirectToPage();
        }

        // ── Shifts ──────────────────────────────────────────────────
        public async Task<IActionResult> OnPostAddShiftAsync(int departmentId, string name)
        {
            if (!_admin.IsAdmin()) return Forbid();
            if (!string.IsNullOrWhiteSpace(name))
            {
                var max = await _db.Shifts.Where(s => s.DepartmentId == departmentId).MaxAsync(s => (int?)s.SortOrder) ?? 0;
                _db.Shifts.Add(new Shift { DepartmentId = departmentId, Name = name.Trim(), SortOrder = max + 1 });
                await SaveGuardedAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditShiftAsync(int id, string name, bool isActive)
        {
            if (!_admin.IsAdmin()) return Forbid();
            var s = await _db.Shifts.FindAsync(id);
            if (s != null)
            {
                s.Name = name.Trim();
                s.IsActive = isActive;
                await SaveGuardedAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteShiftAsync(int id)
        {
            if (!_admin.IsAdmin()) return Forbid();
            var s = await _db.Shifts.FindAsync(id);
            if (s != null) { _db.Shifts.Remove(s); await SaveGuardedAsync(); }
            return RedirectToPage();
        }

        private async Task SaveGuardedAsync()
        {
            try { await _db.SaveChangesAsync(); }
            catch (DbUpdateException) { TempData["Notice"] = "That change conflicts with an existing name — names must be unique within a department."; }
        }

        private async Task LoadAsync()
        {
            Departments = await _db.Departments
                .Include(d => d.Areas.OrderBy(a => a.SortOrder))
                .Include(d => d.Shifts.OrderBy(s => s.SortOrder))
                .OrderBy(d => d.SortOrder).ThenBy(d => d.Name)
                .ToListAsync();

            var lists = await _db.TaskLists
                .Where(t => t.IsCurrent)
                .Select(t => new { t.AreaId, t.ShiftId, t.Version, Items = t.Items.Count })
                .ToListAsync();
            TaskListInfo = lists.ToDictionary(x => (x.AreaId, x.ShiftId), x => (x.Version, x.Items));
            Notice = TempData["Notice"] as string;
        }
    }
}
