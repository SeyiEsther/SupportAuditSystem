namespace SupportAuditSystem.Models
{
    // The signed-in Windows user, resolved from the identity IIS forwards.
    public class AppUser
    {
        public string Username { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public bool IsManager { get; set; }
    }
}
