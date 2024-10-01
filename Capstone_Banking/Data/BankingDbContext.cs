using Capstone_Banking.Model;
using Microsoft.EntityFrameworkCore;

namespace Capstone_Banking.Data
{
    public class BankingDbContext : DbContext
    {
        private string _connectionString;
        public DbSet<AccountDetails> AccountDetailsTable { get; set; }  
        public DbSet<AuditLog> AuditLogTable { get; set; }
        public DbSet<Bank> BankTable { get; set; }
        public DbSet<Beneficiary> BeneficiaryTable { get; set; }
        public DbSet<Client> ClientTable { get; set; }
        public DbSet<Employee> EmployeeTable { get; set; }
        public DbSet<Documents> DocumentsTable { get; set; }
        public DbSet<Payment> PaymentTable { get; set; }
        public DbSet<SalaryDisbursement> SalaryDisbursementTable { get; set; }
        public DbSet<Transactions> MoneyTransactionsTable { get; set; }
        public DbSet<User> UserTable { get; set; }


        public BankingDbContext()
        {
            _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BankProjectDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

    }
}
