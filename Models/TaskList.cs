namespace SupportAuditSystem.Models
{
    // The set of task lines for one Area + Shift, versioned. Editing a live list
    // creates a new version and marks it current; completed checklists stay
    // pinned to the exact version they were filled against, so a reprint always
    // matches what was signed.
    public class TaskList
    {
        public int Id { get; set; }
        public int AreaId { get; set; }
        public Area? Area { get; set; }
        public int ShiftId { get; set; }
        public Shift? Shift { get; set; }

        public int Version { get; set; } = 1;
        public bool IsCurrent { get; set; } = true;

        // The health & reps reminder line printed on the paper layout. Held on the
        // version so a reprint reproduces the wording in force when it was signed.
        public string? HealthRepsReminder { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }

        public List<TaskItem> Items { get; set; } = new();
    }
}
