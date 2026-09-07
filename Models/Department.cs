namespace SupportAuditSystem.Models
{
    // A support department — Stores, Dispatch, and later Logistics. Every
    // department, area, shift and task line lives in the database (never in
    // code) so the whole of a department can be deleted and re-entered through
    // the admin screens with no redeployment.
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;

        public List<Area> Areas { get; set; } = new();
        public List<Shift> Shifts { get; set; } = new();
    }
}
