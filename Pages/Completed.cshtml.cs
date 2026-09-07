using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SupportAuditSystem.Data;
using SupportAuditSystem.Models;

namespace SupportAuditSystem.Pages
{
    public class CompletedModel : PageModel
    {
        private readonly AppDbContext _db;
        public CompletedModel(AppDbContext db) => _db = db;

        [BindProperty(SupportsGet = true)] public int? DepartmentId { get; set; }
        [BindProperty(SupportsGet = true)] public int? AreaId { get; set; }
        [BindProperty(SupportsGet = true)] public int? ShiftId { get; set; }
        [BindProperty(SupportsGet = true)] public DateOnly? From { get; set; }
        [BindProperty(SupportsGet = true)] public DateOnly? To { get; set; }

        public List<Department> Departments { get; set; } = new();
        public List<Row> Rows { get; set; } = new();

        public record Row(int Id, DateOnly Date, string Department, string Area, string Shift,
            string? Auditors, int IssueCount, int Answered, int Total, DateTime? CompletedAt);

        public async Task OnGetAsync()
        {
            Departments = await _db.Departments
                .Include(d => d.Areas).Include(d => d.Shifts)
                .OrderBy(d => d.SortOrder).ThenBy(d => d.Name)
                .ToListAsync();

            var q = _db.ChecklistSubmissions
                .Include(s => s.Area).ThenInclude(a => a!.Department)
                .Include(s => s.Shift)
                .Include(s => s.Responses)
                .Include(s => s.TaskList).ThenInclude(t => t!.Items)
                .Where(s => s.CompletedAt != null);

            if (DepartmentId.HasValue) q = q.Where(s => s.Area!.DepartmentId == DepartmentId);
            if (AreaId.HasValue) q = q.Where(s => s.AreaId == AreaId);
            if (ShiftId.HasValue) q = q.Where(s => s.ShiftId == ShiftId);
            if (From.HasValue) q = q.Where(s => s.ChecklistDate >= From);
            if (To.HasValue) q = q.Where(s => s.ChecklistDate <= To);

            var subs = await q
                .OrderByDescending(s => s.ChecklistDate).ThenByDescending(s => s.CompletedAt)
                .AsSplitQuery()
                .ToListAsync();

            Rows = subs.Select(s => new Row(
                s.Id,
                s.ChecklistDate,
                s.Area?.Department?.Name ?? "",
                s.Area?.Name ?? "",
                s.Shift?.Name ?? "",
                s.AuditorNames,
                s.Responses.Count(r => r.Status == Models.TaskStatus.Issue),
                s.Responses.Count(r => !string.IsNullOrEmpty(r.Status)),
                s.TaskList?.Items.Count ?? 0,
                s.CompletedAt
            )).ToList();
        }
    }
}
