namespace SupportAuditSystem.Models
{
    // A shift within a department — "1st", "2nd", "3rd", "Continental nights".
    // Shifts belong to a department so Dispatch can carry an entirely different
    // set once its documents arrive, all through admin.
    public class Shift
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        public string Name { get; set; } = "";
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
