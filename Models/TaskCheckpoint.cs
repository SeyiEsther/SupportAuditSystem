namespace SupportAuditSystem.Models
{
    // A named checkpoint within a time-boxed task — "9am", "11am", "1pm", "3pm",
    // or "Batch 1", etc. Each is ticked and timestamped separately at entry.
    public class TaskCheckpoint
    {
        public int Id { get; set; }
        public int TaskItemId { get; set; }
        public TaskItem? TaskItem { get; set; }

        public string Label { get; set; } = "";
        public int SortOrder { get; set; }
    }
}
