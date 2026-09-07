namespace SupportAuditSystem.Models
{
    // An area within a department, e.g. "Stores DP1 & DP3" (one combined area,
    // since the paper form covers both) or "Consumables". A default Location
    // string can be pre-filled onto the printed checklist header.
    public class Area
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        public string Name { get; set; } = "";
        // Free text pre-filled into the checklist "Location" line (e.g. "DP1 & DP3").
        public string? DefaultLocation { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
