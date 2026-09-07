namespace SupportAuditSystem.Models
{
    // One line on a task list. Standardised on Done/Issue plus Notes across every
    // shift (the 1st-shift paper form's Done/Issue/Notes wins; the Y/N forms are
    // mapped onto it, and the 2nd-shift Y/N reversal on two rows is corrected).
    //
    // A time-boxed line (e.g. "floors and fire exits" at 9am/11am/1pm/3pm, or
    // "parcels to consumables every two hours") is modelled as ONE task with
    // named checkpoints, each ticked and timestamped separately. All checkpoints
    // are shown from the start of shift rather than locking future ones; the
    // timestamp records when each was genuinely ticked.
    public class TaskItem
    {
        public int Id { get; set; }
        public int TaskListId { get; set; }
        public TaskList? TaskList { get; set; }

        public string Text { get; set; } = "";
        public int SortOrder { get; set; }

        public bool IsTimeBoxed { get; set; }
        public List<TaskCheckpoint> Checkpoints { get; set; } = new();
    }
}
