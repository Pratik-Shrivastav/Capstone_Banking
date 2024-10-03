using Capstone_Banking.Data;
using Capstone_Banking.Model;
using Microsoft.EntityFrameworkCore;

namespace Capstone_Banking.CommonFunction
{
    public class AddAuditLogs
    {
        private static BankingDbContext _db = new BankingDbContext();
        public static void AddLog(int userId, string action, string details)
        {
            User user = _db.UserTable.Include(c=>c.AuditLogList).FirstOrDefault(x=>x.Id == userId);
            AuditLog auditLog = new AuditLog()
            {
                Action = action,
                Timestamp = DateTime.Now,
                Details = $"UserName: {user.Name} {details}"
            };
            user.AuditLogList.Add(auditLog);
            _db.SaveChanges();

        }
    }
}
