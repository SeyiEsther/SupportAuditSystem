using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupportAuditSystem.Models;
using SupportAuditSystem.Services;

namespace SupportAuditSystem.Pages
{
    public class ChecklistModel : PageModel
    {
        private readonly ChecklistService _checklists;
        private readonly RosterService _roster;
        public ChecklistModel(ChecklistService checklists, RosterService roster)
        {
            _checklists = checklists;
            _roster = roster;
        }

        public record GridSection(string Category, List<TaskItem> Items);

        public ChecklistSubmission? Submission { get; set; }
        public List<TaskItem> Items { get; set; } = new();
        public Dictionary<int, TaskResponse> ResponsesByItem { get; set; } = new();
        public List<RosterPerson> HodRoster { get; set; } = new();

        // Time-boxed items that carry a Category (Dispatch's Warehouse Audit)
        // render as one combined hourly grid table, grouped by category, with a
        // Y/N answer per hour — matching the Production Audit System's hourly
        // check grid exactly. Everything else (Stores' items, and any
        // non-categorised time-boxed item) keeps the single-column card layout.
        public List<GridSection> GridSections { get; set; } = new();
        public List<TaskCheckpoint> GridCheckpoints { get; set; } = new();
        public List<TaskItem> CardItems { get; set; } = new();

        public int AnsweredCount { get; set; }
        public int TotalCount { get; set; }

        // The HOD sign-off block only appears when this task list actually
        // carries escalation-to-HOD accountability (Dispatch's Warehouse Audit) —
        // Stores' checklists don't have this data, so they see nothing new.
        public bool NeedsHodSignOff => Items.Any(i =>
            string.Equals(i.EscalateToRole, "HOD", StringComparison.OrdinalIgnoreCase));

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Submission = await _checklists.GetForEntryAsync(id);
            if (Submission == null) return NotFound();

            Items = (Submission.TaskList?.Items ?? new()).OrderBy(i => i.SortOrder).ToList();
            ResponsesByItem = Submission.Responses.ToDictionary(r => r.TaskItemId);
            TotalCount = Items.Count;
            AnsweredCount = Items.Count(i =>
                ResponsesByItem.TryGetValue(i.Id, out var r) &&
                (!string.IsNullOrEmpty(r.Status) || r.CheckpointResponses.Any(c => c.Ticked)));

            var gridItems = Items.Where(i => i.IsTimeBoxed && !string.IsNullOrEmpty(i.Category)).ToList();
            CardItems = Items.Except(gridItems).ToList();
            var categoryOrder = gridItems.Select(i => i.Category!).Distinct().ToList();
            GridSections = categoryOrder
                .Select(c => new GridSection(c, gridItems.Where(i => i.Category == c).OrderBy(i => i.SortOrder).ToList()))
                .ToList();
            GridCheckpoints = gridItems.FirstOrDefault()?.Checkpoints.OrderBy(c => c.SortOrder).ToList() ?? new();

            if (NeedsHodSignOff)
                HodRoster = await _roster.GetAsync(RosterKinds.Hod);

            return Page();
        }

        // Maps a category label to the Production Audit System's fixed section
        // colour classes (safety/quality/perf/morale) — a presentation lookup
        // only; an unrecognised category still renders, just without a themed
        // colour, so admin can name categories freely with no code change.
        public static string SectionClass(string category) => category.Trim().ToLowerInvariant() switch
        {
            "h&s" or "health & safety" or "health and safety" or "safety" => "safety",
            "quality" => "quality",
            "performance" or "perf" => "perf",
            "morale" => "morale",
            _ => "quality",
        };
    }
}
