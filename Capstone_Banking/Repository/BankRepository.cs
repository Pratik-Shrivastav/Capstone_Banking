using Capstone_Banking.Data;
using Capstone_Banking.Dto;
using Capstone_Banking.Model;
using Microsoft.EntityFrameworkCore;

namespace Capstone_Banking.Repository
{
    public class BankRepository : IBankRepository
    {
        private BankingDbContext _bankingDbContext;
        public BankRepository(BankingDbContext bankingDbContext) 
        {
            _bankingDbContext = bankingDbContext;
        }
        public void AddBank(User user)
        {
            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(user.Password);
             _bankingDbContext.UserTable.Add(user);
             _bankingDbContext.SaveChanges();
        }

        public async Task<ICollection<Client>> GetAllClients()
        {
            return await _bankingDbContext.ClientTable.Include(c => c.AccountDetailsObject)
                .Include(e => e.BeneficiaryList).ThenInclude(x => x.PaymentsList).ThenInclude(p => p.Transactions)
                .ToListAsync();
        }

        public async Task<Client> GetClientsById(int clientId)
        {
            return await _bankingDbContext.ClientTable.Include(c => c.AccountDetailsObject)
                .Include(e => e.BeneficiaryList).ThenInclude(w=>w.AccountDetailsObject)
                .Include(e => e.BeneficiaryList)
                .ThenInclude(x => x.PaymentsList).ThenInclude(p => p.Transactions)
                .FirstOrDefaultAsync(z=>z.Id==clientId);
        }

        public async Task<ICollection<Documents>> GetDocuments(int clientId)
        {
            Client client = await _bankingDbContext.ClientTable.Include(c => c.DocumentList).FirstOrDefaultAsync(x => x.Id == clientId);
            return client.DocumentList;
        }

        public async Task<ICollection<SalaryDisbursementResponseDto>> GetSalaryDisbursementClient(int clientId)
        {
            Client client = await _bankingDbContext.ClientTable
                .Include(c => c.SalaryDisbursementList).ThenInclude(x => x.SalaryForList)
                .Include(c => c.SalaryDisbursementList).ThenInclude(x => x.TransactionList)
                .FirstOrDefaultAsync(z => z.Id == clientId);

            List<SalaryDisbursementResponseDto> salaryDisbursementResponseDtoList = new List<SalaryDisbursementResponseDto>();
            foreach (SalaryDisbursement salaryDisbursement in client.SalaryDisbursementList)
            {
                SalaryDisbursementResponseDto responseDto = new SalaryDisbursementResponseDto();
                responseDto.Id = salaryDisbursement.Id;
                responseDto.Amount = salaryDisbursement.Amount;
                responseDto.Status = salaryDisbursement.Status;
                responseDto.ProcessedAt = salaryDisbursement.ProcessedAt;
                responseDto.TransactionList = salaryDisbursement.TransactionList;
                responseDto.EmployeeList = new List<Employee>();
                foreach (SalaryFor salaryFor in salaryDisbursement.SalaryForList)
                {
                    Employee employee = _bankingDbContext.EmployeeTable.Find(salaryFor.EmployeeId);
                    responseDto.EmployeeList.Add(employee);
                }
                salaryDisbursementResponseDtoList.Add(responseDto);

            }
            return salaryDisbursementResponseDtoList;
        }



    }
}
