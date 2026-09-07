using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupportAuditSystem.Models;
using SupportAuditSystem.Services;

namespace SupportAuditSystem.Pages
{
    public class ChecklistModel : PageModel
    {
        private readonly ChecklistService _checklists;
        public ChecklistModel(ChecklistService checklists) => _checklists = checklists;

        public ChecklistSubmission? Submission { get; set; }
        public List<TaskItem> Items { get; set; } = new();
        public Dictionary<int, TaskResponse> ResponsesByItem { get; set; } = new();

        public int AnsweredCount { get; set; }
        public int TotalCount { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Submission = await _checklists.GetForEntryAsync(id);
            if (Submission == null) return NotFound();

            Items = (Submission.TaskList?.Items ?? new()).OrderBy(i => i.SortOrder).ToList();
            ResponsesByItem = Submission.Responses.ToDictionary(r => r.TaskItemId);
            TotalCount = Items.Count;
            AnsweredCount = Items.Count(i =>
                ResponsesByItem.TryGetValue(i.Id, out var r) && !string.IsNullOrEmpty(r.Status));
            return Page();
        }
    }
}
