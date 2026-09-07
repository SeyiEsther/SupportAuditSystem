using Microsoft.EntityFrameworkCore;
using SupportAuditSystem.Data;
using SupportAuditSystem.Models;

namespace SupportAuditSystem.Services
{
    public record StartResult(bool Ok, int SubmissionId, string? Error);
    public record SaveResult(bool Ok, string? Error);
    public record CompleteResult(bool Ok, IReadOnlyList<string> MissingNotes, string? Error);

    // Owns filling in and saving a checklist. Every tap on the entry screen saves
    // immediately through here; the caller shows success only once the save is
    // confirmed.
    public class ChecklistService
    {
        private readonly AppDbContext _db;
        private readonly ILogger<ChecklistService> _log;

        public ChecklistService(AppDbContext db, ILogger<ChecklistService> log)
        {
            _db = db;
            _log = log;
        }

        // Find-or-create on (Area, Shift, Date) only — never on the person's name,
        // or two people on a shift get separate forms and the first person's work
        // looks lost. A new submission is pinned to the area+shift's current task
        // list version and seeded with a response row per task line (and a
        // checkpoint-response row per checkpoint of a time-boxed line).
        public async Task<StartResult> GetOrCreateAsync(int areaId, int shiftId, DateOnly date, AppUser user)
        {
            var existing = await _db.ChecklistSubmissions
                .Where(s => s.AreaId == areaId && s.ShiftId == shiftId && s.ChecklistDate == date)
                .OrderBy(s => s.Id)
                .FirstOrDefaultAsync();
            if (existing != null)
                return new StartResult(true, existing.Id, null);

            var area = await _db.Areas.FirstOrDefaultAsync(a => a.Id == areaId);
            if (area == null)
                return new StartResult(false, 0, "That area no longer exists.");

            var taskList = await _db.TaskLists
                .Include(t => t.Items).ThenInclude(i => i.Checkpoints)
                .Where(t => t.AreaId == areaId && t.ShiftId == shiftId && t.IsCurrent)
                .OrderByDescending(t => t.Version)
                .FirstOrDefaultAsync();
            if (taskList == null)
                return new StartResult(false, 0, "No task list is configured for that area and shift yet.");

            var submission = new ChecklistSubmission
            {
                AreaId = areaId,
                ShiftId = shiftId,
                TaskListId = taskList.Id,
                ChecklistDate = date,
                Location = area.DefaultLocation,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user.DisplayName ?? user.Username,
            };
            foreach (var item in taskList.Items.OrderBy(i => i.SortOrder))
            {
                var resp = new TaskResponse { TaskItemId = item.Id };
                foreach (var cp in item.Checkpoints.OrderBy(c => c.SortOrder))
                    resp.CheckpointResponses.Add(new CheckpointResponse { TaskCheckpointId = cp.Id });
                submission.Responses.Add(resp);
            }

            _db.ChecklistSubmissions.Add(submission);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // A concurrent insert may have created the same natural key first
                // (the index is deliberately non-unique). Fall back to the winner.
                _log.LogWarning(ex, "Concurrent checklist create for area {Area} shift {Shift} {Date}; reloading.", areaId, shiftId, date);
                var winner = await _db.ChecklistSubmissions
                    .Where(s => s.AreaId == areaId && s.ShiftId == shiftId && s.ChecklistDate == date)
                    .OrderBy(s => s.Id)
                    .FirstOrDefaultAsync();
                if (winner != null) return new StartResult(true, winner.Id, null);
                return new StartResult(false, 0, "Could not open the checklist. Please try again.");
            }
            return new StartResult(true, submission.Id, null);
        }

        public async Task<ChecklistSubmission?> GetForEntryAsync(int id)
        {
            return await _db.ChecklistSubmissions
                .Include(s => s.Area).ThenInclude(a => a!.Department)
                .Include(s => s.Shift)
                .Include(s => s.TaskList).ThenInclude(t => t!.Items).ThenInclude(i => i.Checkpoints)
                .Include(s => s.Responses).ThenInclude(r => r.CheckpointResponses)
                .AsSplitQuery()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        // Saves a single task line's status and note. Returns only once the write
        // is confirmed.
        public async Task<SaveResult> SaveResponseAsync(int submissionId, int taskItemId, string? status, string? notes, AppUser user)
        {
            var resp = await _db.TaskResponses
                .FirstOrDefaultAsync(r => r.ChecklistSubmissionId == submissionId && r.TaskItemId == taskItemId);
            if (resp == null)
                return new SaveResult(false, "That task line is not part of this checklist.");

            status = status switch
            {
                Models.TaskStatus.Done => Models.TaskStatus.Done,
                Models.TaskStatus.Issue => Models.TaskStatus.Issue,
                _ => null
            };
            resp.Status = status;
            resp.Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();
            resp.UpdatedAt = DateTime.UtcNow;

            await TouchAsync(submissionId, user);
            try { await _db.SaveChangesAsync(); }
            catch (Exception ex)
            {
                _log.LogError(ex, "Save response failed sub {Sub} item {Item}", submissionId, taskItemId);
                return new SaveResult(false, "Could not save. Your entry is kept — try again.");
            }
            return new SaveResult(true, null);
        }

        public async Task<SaveResult> SaveCheckpointAsync(int submissionId, int checkpointResponseId, bool ticked, AppUser user)
        {
            var cp = await _db.CheckpointResponses
                .Include(c => c.TaskResponse)
                .FirstOrDefaultAsync(c => c.Id == checkpointResponseId && c.TaskResponse!.ChecklistSubmissionId == submissionId);
            if (cp == null)
                return new SaveResult(false, "That checkpoint is not part of this checklist.");

            cp.Ticked = ticked;
            cp.TickedAt = ticked ? DateTime.UtcNow : null;
            if (cp.TaskResponse != null) cp.TaskResponse.UpdatedAt = DateTime.UtcNow;

            await TouchAsync(submissionId, user);
            try { await _db.SaveChangesAsync(); }
            catch (Exception ex)
            {
                _log.LogError(ex, "Save checkpoint failed sub {Sub} cp {Cp}", submissionId, checkpointResponseId);
                return new SaveResult(false, "Could not save. Try again.");
            }
            return new SaveResult(true, null);
        }

        public async Task<SaveResult> SaveHeaderAsync(int submissionId, string? auditorNames, string? location, AppUser user)
        {
            var sub = await _db.ChecklistSubmissions.FirstOrDefaultAsync(s => s.Id == submissionId);
            if (sub == null) return new SaveResult(false, "Checklist not found.");
            sub.AuditorNames = string.IsNullOrWhiteSpace(auditorNames) ? null : auditorNames.Trim();
            sub.Location = string.IsNullOrWhiteSpace(location) ? null : location.Trim();
            sub.LastEditedAt = DateTime.UtcNow;
            sub.LastEditedBy = user.DisplayName ?? user.Username;
            try { await _db.SaveChangesAsync(); }
            catch (Exception ex)
            {
                _log.LogError(ex, "Save header failed sub {Sub}", submissionId);
                return new SaveResult(false, "Could not save. Try again.");
            }
            return new SaveResult(true, null);
        }

        // Completion is blocked while any Issue response has no note.
        public async Task<CompleteResult> CompleteAsync(int submissionId, AppUser user)
        {
            var sub = await _db.ChecklistSubmissions
                .Include(s => s.TaskList).ThenInclude(t => t!.Items)
                .Include(s => s.Responses)
                .FirstOrDefaultAsync(s => s.Id == submissionId);
            if (sub == null)
                return new CompleteResult(false, Array.Empty<string>(), "Checklist not found.");

            var itemsById = sub.TaskList!.Items.ToDictionary(i => i.Id);
            var missing = sub.Responses
                .Where(r => r.Status == Models.TaskStatus.Issue && string.IsNullOrWhiteSpace(r.Notes))
                .Select(r => itemsById.TryGetValue(r.TaskItemId, out var it) ? it.Text : "(task)")
                .ToList();
            if (missing.Count > 0)
                return new CompleteResult(false, missing, "Every issue needs a note before you can complete.");

            sub.CompletedAt = DateTime.UtcNow;
            sub.CompletedBy = user.DisplayName ?? user.Username;
            sub.LastEditedAt = DateTime.UtcNow;
            sub.LastEditedBy = user.DisplayName ?? user.Username;
            try { await _db.SaveChangesAsync(); }
            catch (Exception ex)
            {
                _log.LogError(ex, "Complete failed sub {Sub}", submissionId);
                return new CompleteResult(false, Array.Empty<string>(), "Could not complete. Try again.");
            }
            return new CompleteResult(true, Array.Empty<string>(), null);
        }

        private async Task TouchAsync(int submissionId, AppUser user)
        {
            var sub = await _db.ChecklistSubmissions.FirstOrDefaultAsync(s => s.Id == submissionId);
            if (sub == null) return;
            sub.LastEditedAt = DateTime.UtcNow;
            sub.LastEditedBy = user.DisplayName ?? user.Username;
        }
    }
}
