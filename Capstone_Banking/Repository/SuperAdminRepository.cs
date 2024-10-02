using Capstone_Banking.Data;
using Capstone_Banking.Dto;
using Capstone_Banking.Model;
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
                .Include(t => t.BeneficiaryList).ThenInclude(q => q.AccountDetailsObject)
                .Include(e => e.BeneficiaryList).ThenInclude(x => x.PaymentsList).ThenInclude(e => e.Transactions)
                .Include(a => a.SalaryDisbursementList).ThenInclude(p => p.TransactionList)
                .FirstOrDefaultAsync(z => z.Id == id);
        }

        public async Task<ICollection<Documents>> GetDocuments(int clientId)
        {
            Client client= await _db.ClientTable.Include(c => c.DocumentList).FirstOrDefaultAsync(x=>x.Id==clientId);
            return client.DocumentList;
        }

        public async Task<ICollection<SalaryDisbursementResponseDto>> GetSalaryDisbursementClient(int clientId)
        {
            Client client = await _db.ClientTable
                .Include(c => c.SalaryDisbursementList).ThenInclude(x => x.SalaryForList)
                .Include(c => c.SalaryDisbursementList).ThenInclude(x => x.TransactionList)
                .FirstOrDefaultAsync(z => z.Id == clientId);

            List<SalaryDisbursementResponseDto> salaryDisbursementResponseDtoList = new List<SalaryDisbursementResponseDto>();
            foreach(SalaryDisbursement salaryDisbursement in client.SalaryDisbursementList)
            {
                SalaryDisbursementResponseDto responseDto = new SalaryDisbursementResponseDto();
                responseDto.Id = salaryDisbursement.Id;
                responseDto.Amount = salaryDisbursement.Amount;
                responseDto.Status = salaryDisbursement.Status;
                responseDto.ProcessedAt = salaryDisbursement.ProcessedAt;
                responseDto.TransactionList = salaryDisbursement.TransactionList;
                responseDto.EmployeeList = new List<Employee>();
                foreach(SalaryFor salaryFor in salaryDisbursement.SalaryForList)
                {
                    Employee employee = _db.EmployeeTable.Find(salaryFor.EmployeeId);
                    responseDto.EmployeeList.Add(employee); 
                }
                salaryDisbursementResponseDtoList.Add(responseDto);

            }
            return salaryDisbursementResponseDtoList;
        }


        public void ClientStatus(int clientId, string status)
        {
            Client client = _db.ClientTable.Find(clientId);
            client.Status = status;
            _db.SaveChanges();
        }

        public void UpdatePaymentStatus(int paymentId, string status)
        {
            try
            {
                // Retrieve the payment with related transactions
                Payment payment =  _db.PaymentTable
                    .Include(x => x.Transactions)
                    .FirstOrDefault(o => o.Id == paymentId);

                if (payment == null)
                {
                    throw new Exception("Payment not found");
                }

                // Update the payment details
                payment.Status = status;
                payment.ApprovedAt = DateTime.Now;
                payment.ApprovedBy = 4;  // Assumes an admin or approver ID

                // If the status is "Success", add a new transaction
                if (status == "Success")
                {
                    Transactions transaction = new Transactions
                    {
                        TransactionDate = DateTime.Now,
                        TransactionAmount = payment.Amount,
                        TransactionStatus = status,
                    };
                    payment.Transactions.Add(transaction);
                }

                // Save changes to the database
                _db.SaveChanges();
                Console.WriteLine("Changes saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex}");
                throw;  // Rethrow the exception to handle it outside if necessary
            }
        }

        public void UpdateSalaryDisbursementStatus(int salaryDisId, string status)
        {
            SalaryDisbursement salaryDisbursement = _db.SalaryDisbursementTable
                .Include(c => c.TransactionList).Include(x => x.SalaryForList)
                .FirstOrDefault(z => z.Id == salaryDisId);

            salaryDisbursement.Status = status;
            salaryDisbursement.ProcessedAt = DateTime.Now;

            if (status == "Success")
            {
                foreach (SalaryFor salaryFor in salaryDisbursement.SalaryForList)
                {
                    Employee employee = _db.EmployeeTable.Find(salaryFor.EmployeeId);
                    Transactions transaction = new Transactions
                    {
                        TransactionDate = DateTime.Now,
                        TransactionAmount = employee.Salary,
                        TransactionStatus = status,
                        EmployeePaidId = salaryFor.EmployeeId
                    };
                    salaryDisbursement.TransactionList.Add(transaction);
                }
            }
            _db.SaveChanges();






        }

    }
}
