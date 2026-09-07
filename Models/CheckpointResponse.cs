namespace SupportAuditSystem.Models
{
    // A single ticked checkpoint on a time-boxed task. TickedAt records when it
    // was genuinely ticked (not a scheduled time), so the printed form shows the
    // real moment each check happened.
    public class CheckpointResponse
    {
        public int Id { get; set; }
        public int TaskResponseId { get; set; }
        public TaskResponse? TaskResponse { get; set; }

        public int TaskCheckpointId { get; set; }
        public TaskCheckpoint? TaskCheckpoint { get; set; }

        public bool Ticked { get; set; }
        public DateTime? TickedAt { get; set; }
    }
}
