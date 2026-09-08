using Microsoft.EntityFrameworkCore;
using SupportAuditSystem.Data;
using SupportAuditSystem.Models;

namespace SupportAuditSystem.Services
{
    // One edited task line coming back from the admin editor.
    public class EditedTaskItem
    {
        public string Text { get; set; } = "";
        public bool IsTimeBoxed { get; set; }
        // Checkpoint labels in order, e.g. ["9am","11am","1pm","3pm"].
        public List<string> Checkpoints { get; set; } = new();

        // Optional structured accountability — admin-typed free text, never a
        // hard-coded set (see TaskItem.Category etc.).
        public string? Category { get; set; }
        public string? Cadence { get; set; }
        public string? ResponsibleRole { get; set; }
        public string? EscalateToRole { get; set; }
        public string? EscalationWindow { get; set; }
    }

    // Owns task-list versioning. A task list belongs to an Area + Shift. Editing a
    // list that has already been filled against forks a new version and marks it
    // current; the old version stays untouched so every completed checklist that
    // pinned it still reprints exactly as signed.
    public class TaskListService
    {
        private readonly AppDbContext _db;

        public TaskListService(AppDbContext db) => _db = db;

        public async Task<TaskList?> GetCurrentAsync(int areaId, int shiftId)
        {
            return await _db.TaskLists
                .Include(t => t.Items.OrderBy(i => i.SortOrder)).ThenInclude(i => i.Checkpoints.OrderBy(c => c.SortOrder))
                .Where(t => t.AreaId == areaId && t.ShiftId == shiftId && t.IsCurrent)
                .OrderByDescending(t => t.Version)
                .FirstOrDefaultAsync();
        }

        // Creates the first version of a (previously empty) task list, or replaces
        // the content. If the current version has any pinned submission it is
        // forked to a new version; otherwise it is edited in place.
        public async Task SaveAsync(int areaId, int shiftId, IEnumerable<EditedTaskItem> items, string? reminder, AppUser user)
        {
            var current = await GetCurrentAsync(areaId, shiftId);
            var clean = items
                .Where(i => !string.IsNullOrWhiteSpace(i.Text))
                .ToList();

            bool referenced = current != null &&
                await _db.ChecklistSubmissions.AnyAsync(s => s.TaskListId == current.Id);

            if (current == null || referenced)
            {
                if (current != null)
                    current.IsCurrent = false;

                var nextVersion = current == null
                    ? 1
                    : await _db.TaskLists.Where(t => t.AreaId == areaId && t.ShiftId == shiftId).MaxAsync(t => t.Version) + 1;

                var fresh = new TaskList
                {
                    AreaId = areaId,
                    ShiftId = shiftId,
                    Version = nextVersion,
                    IsCurrent = true,
                    HealthRepsReminder = reminder,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = user.DisplayName ?? user.Username,
                };
                FillItems(fresh, clean);
                _db.TaskLists.Add(fresh);
            }
            else
            {
                // Safe to edit in place — nothing has pinned this version yet.
                current.HealthRepsReminder = reminder;
                _db.TaskItems.RemoveRange(current.Items);
                current.Items.Clear();
                FillItems(current, clean);
            }

            await _db.SaveChangesAsync();
        }

        private static void FillItems(TaskList list, List<EditedTaskItem> items)
        {
            int order = 0;
            foreach (var edit in items)
            {
                static string? Clean(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
                var item = new TaskItem
                {
                    Text = edit.Text.Trim(),
                    SortOrder = order++,
                    IsTimeBoxed = edit.IsTimeBoxed && edit.Checkpoints.Any(c => !string.IsNullOrWhiteSpace(c)),
                    Category = Clean(edit.Category),
                    Cadence = Clean(edit.Cadence),
                    ResponsibleRole = Clean(edit.ResponsibleRole),
                    EscalateToRole = Clean(edit.EscalateToRole),
                    EscalationWindow = Clean(edit.EscalationWindow),
                };
                if (item.IsTimeBoxed)
                {
                    int cpOrder = 0;
                    foreach (var label in edit.Checkpoints.Where(c => !string.IsNullOrWhiteSpace(c)))
                        item.Checkpoints.Add(new TaskCheckpoint { Label = label.Trim(), SortOrder = cpOrder++ });
                }
                list.Items.Add(item);
            }
        }
    }
}
