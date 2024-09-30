namespace Capstone_Banking.Model
{
    public class AuditLog
    {
        public int LogId { get; set; }
        public int UserId { get; set; }  // Foreign key to User
        public string Action { get; set; }  // e.g., "Created Client", "Approved Payment"
        public DateTime Timestamp { get; set; }
        public string Details { get; set; }  // Additional info about the action
    }
}
