using Capstone_Banking.Data;
using Microsoft.EntityFrameworkCore;

namespace Capstone_Banking.Repository
{
    public class SuperAdminRepository : ISuperAdminRepository
    {
        private BankingDbContext _db;

        public SuperAdminRepository(BankingDbContext bankingDbContext) 
        {
            _db = bankingDbContext;
        }

        public async Task<ICollection<Client>> GetAllClients()
        {
            return await _db.ClientTable.Include(c => c.AccountDetailsObject)
                .Include(d => d.EmployeeList).ThenInclude(y => y.AccountDetailsObject)
                .Include(e => e.BeneficiaryList).ThenInclude(x => x.PaymentsList).ThenInclude(e => e.Transactions)
                .Include(a => a.SalaryDisbursementList)
                .ThenInclude(p => p.TransactionList).ToListAsync();
        }

        public async Task<Client> GetClientById(int id)
        {
            return await _db.ClientTable.Include(c => c.AccountDetailsObject)
                .Include(d => d.EmployeeList).ThenInclude(y => y.AccountDetailsObject)
                .Include(e => e.BeneficiaryList).ThenInclude(x => x.PaymentsList).ThenInclude(e => e.Transactions)
                .Include(a => a.SalaryDisbursementList)
                .ThenInclude(p => p.TransactionList).FirstOrDefaultAsync(z => z.Id == id);
        }



    }
}
