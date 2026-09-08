namespace SupportAuditSystem.Models
{
    // A named individual available for sign-off pickers (e.g. HOD). Grouped by
    // ListKind so more rosters (e.g. a Senior Operator list) can be added later
    // purely through admin — the kind itself is data, not a hard-coded set.
    public class RosterPerson
    {
        public int Id { get; set; }
        public string ListKind { get; set; } = "";
        public string Name { get; set; } = "";
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
    }

    // Fixed roster kinds the app itself understands structurally (drives which
    // picker a screen queries) — not user-visible business content, so this
    // doesn't conflict with the "nothing hard-coded" rule any more than
    // TaskStatus.Done/Issue does. The actual people in each roster are data.
    public static class RosterKinds
    {
        public const string Hod = "Hod";
    }
}
