using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SupportAuditSystem.Data;
using SupportAuditSystem.Models;
using SupportAuditSystem.Services;

namespace SupportAuditSystem.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly ChecklistService _checklists;
        private readonly UserService _users;

        public IndexModel(AppDbContext db, ChecklistService checklists, UserService users)
        {
            _db = db;
            _checklists = checklists;
            _users = users;
        }

        public List<Department> Departments { get; set; } = new();
        public string? Error { get; set; }

        [BindProperty] public int AreaId { get; set; }
        [BindProperty] public int ShiftId { get; set; }
        [BindProperty] public DateOnly ChecklistDate { get; set; }
        [BindProperty] public string? AuditorNames { get; set; }

        public async Task OnGetAsync()
        {
            await LoadAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadAsync();

            if (AreaId == 0 || ShiftId == 0)
            {
                Error = "Choose a department, area and shift to begin.";
                return Page();
            }
            if (ChecklistDate == default)
                ChecklistDate = DateOnly.FromDateTime(DateTime.Today);

            var user = _users.GetCurrentUser();
            var result = await _checklists.GetOrCreateAsync(AreaId, ShiftId, ChecklistDate, user);
            if (!result.Ok)
            {
                Error = result.Error;
                return Page();
            }

            if (!string.IsNullOrWhiteSpace(AuditorNames))
                await _checklists.SaveHeaderAsync(result.SubmissionId, AuditorNames, null, user);

            return RedirectToPage("/Checklist", new { id = result.SubmissionId });
        }

        private async Task LoadAsync()
        {
            Departments = await _db.Departments
                .Where(d => d.IsActive)
                .Include(d => d.Areas.Where(a => a.IsActive))
                .Include(d => d.Shifts.Where(s => s.IsActive))
                .OrderBy(d => d.SortOrder).ThenBy(d => d.Name)
                .ToListAsync();
            if (ChecklistDate == default)
                ChecklistDate = DateOnly.FromDateTime(DateTime.Today);
        }
    }
}
