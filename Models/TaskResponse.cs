namespace SupportAuditSystem.Models
{
    public static class TaskStatus
    {
        public const string Done = "Done";
        public const string Issue = "Issue";
    }

    // The answer to one task line on one checklist. Status is Done/Issue (or null
    // until answered); Notes is required when Status is Issue.
    public class TaskResponse
    {
        public int Id { get; set; }
        public int ChecklistSubmissionId { get; set; }
        public ChecklistSubmission? ChecklistSubmission { get; set; }

        public int TaskItemId { get; set; }
        public TaskItem? TaskItem { get; set; }

        public string? Status { get; set; }
        public string? Notes { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public List<CheckpointResponse> CheckpointResponses { get; set; } = new();
    }
}
