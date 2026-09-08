namespace SupportAuditSystem.Models
{
    // A Safe Start checklist filled for one Area + Shift + Date. Resolved by
    // find-or-create on (Area, Shift, Date) only — never on the auditor's name,
    // so two people on the same shift work the same form and nobody's entries
    // look lost. Pinned to the TaskList version it was started against.
    public class ChecklistSubmission
    {
        public int Id { get; set; }

        public int AreaId { get; set; }
        public Area? Area { get; set; }
        public int ShiftId { get; set; }
        public Shift? Shift { get; set; }

        // The exact task-list version this checklist was filled against.
        public int TaskListId { get; set; }
        public TaskList? TaskList { get; set; }

        public DateOnly ChecklistDate { get; set; }

        // Free text — agency workers fill these in, so never a dropdown.
        public string? AuditorNames { get; set; }
        public string? Location { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? LastEditedAt { get; set; }
        public string? LastEditedBy { get; set; }

        // Set when the checklist is confirmed complete. Completion is blocked
        // while any Issue response has no note.
        public DateTime? CompletedAt { get; set; }
        public string? CompletedBy { get; set; }

        // HOD sign-off. Picked from the admin-managed HOD roster (RosterKinds.Hod)
        // — a real named individual, not free text, since these are permanent
        // staff, not agency workers. Not a hard gate on Complete; present and
        // functional so it can be tightened into one later if wanted.
        public string? HodSignOffName { get; set; }
        public DateTime? HodSignOffAt { get; set; }

        public List<TaskResponse> Responses { get; set; } = new();
    }
}
