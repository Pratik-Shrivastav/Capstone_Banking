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
            return await _db.ClientTable.Include(c=>c.AccountDetailsObject)
                .Include(d=>d.EmployeeList).ThenInclude(y=>y.AccountDetailsObject)
                .Include(e=>e.Payments).ThenInclude(x=>x.Transactions).ToListAsync();
        }

        public async Task<Client> GetClientById(int id)
        {
            return await _db.ClientTable.Include(c => c.AccountDetailsObject)
                .Include(d => d.EmployeeList).ThenInclude(y => y.AccountDetailsObject)
                .Include(e => e.Payments).ThenInclude(x => x.Transactions).FirstOrDefaultAsync(z => z.Id == id);
        }



    }
}
