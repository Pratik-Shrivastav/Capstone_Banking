namespace Capstone_Banking.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role {  get; set; }
        public string UserName { get; set; }
        public DateTime CreatedOn { get; set; }
        public Bank? BankObject { get; set; } 
        public Client? ClientObject { get; set; }
        public ICollection<AuditLog>? AuditLogList { get; set; }


    }
}
