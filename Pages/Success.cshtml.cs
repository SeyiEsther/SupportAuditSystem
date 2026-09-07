using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupportAuditSystem.Models;
using SupportAuditSystem.Services;

namespace SupportAuditSystem.Pages
{
    public class SuccessModel : PageModel
    {
        private readonly ChecklistService _checklists;
        public SuccessModel(ChecklistService checklists) => _checklists = checklists;

        public ChecklistSubmission? Submission { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Submission = await _checklists.GetForEntryAsync(id);
            if (Submission == null) return NotFound();
            return Page();
        }
    }
}
