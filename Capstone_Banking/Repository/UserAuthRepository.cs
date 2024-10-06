using Capstone_Banking.Data;
using Capstone_Banking.Model;
using Microsoft.EntityFrameworkCore;

namespace Capstone_Banking.Repository
{
    public class UserAuthRepository:IUserAuthRepository
    {
        private BankingDbContext _bankingDbContext;
        public UserAuthRepository(BankingDbContext bankingDbContext) 
        {
            _bankingDbContext = bankingDbContext;
        }
        public async Task<User> AddUser(User newUser)
        {
            await _bankingDbContext.UserTable.AddAsync(newUser);
            await _bankingDbContext.SaveChangesAsync();
            return newUser;
        }

        public async Task<User> GetUserByUserName(string userName)
        {
            User user = await _bankingDbContext.UserTable.Include(X=>X.ClientObject).ThenInclude(o=>o.AccountDetailsObject).FirstOrDefaultAsync(c=>c.UserName == userName);
            return user;
        }

        public async Task<User> GetUserByIdDocumnet(int id)
        {
            User user = await _bankingDbContext.UserTable.Include(X => X.ClientObject).ThenInclude(o => o.DocumentList).FirstOrDefaultAsync(c => c.Id == id);
            return user;
        }

        public async Task<ICollection<User>> GetAllUser()
        {
            return await _bankingDbContext.UserTable.Include(c=>c.ClientObject).ThenInclude(x=>x.AccountDetailsObject).ToListAsync();
        }


    }
    

}
