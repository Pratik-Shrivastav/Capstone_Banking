﻿using Capstone_Banking.CommonFunction;
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

        public ICollection<Client> GetAllClients()
        {
            var clients=  _db.ClientTable.Include(c => c.AccountDetailsObject)
                .Include(d => d.EmployeeList).ThenInclude(y => y.AccountDetailsObject)
                .Include(e => e.BeneficiaryList).ThenInclude(x => x.PaymentsList).ThenInclude(e => e.Transactions)
                .Include(a => a.SalaryDisbursementList)
                .ThenInclude(p => p.TransactionList)
                .ToList();
            return clients;
        }
        public async Task<ICollection<Client>> GetAllClientsPaged(int page, int pageSize)
        {
            return await _db.ClientTable.Include(c => c.AccountDetailsObject)
                .Include(d => d.EmployeeList).ThenInclude(y => y.AccountDetailsObject)
                .Include(e => e.BeneficiaryList).ThenInclude(x => x.PaymentsList).ThenInclude(e => e.Transactions)
                .Include(a => a.SalaryDisbursementList)
                .ThenInclude(p => p.TransactionList)
                .Where(s=>s.Status=="Success")
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<ICollection<User>> GetAllClientsPagedPending(int page, int pageSize)
        {
            var users = await _db.UserTable.Include(cd=>cd.ClientObject).ThenInclude(c => c.AccountDetailsObject)
                .Include(cd => cd.ClientObject)
                .ThenInclude(d => d.EmployeeList).ThenInclude(y => y.AccountDetailsObject)
               .Include(cd => cd.ClientObject).ThenInclude(e => e.BeneficiaryList).ThenInclude(x => x.PaymentsList).ThenInclude(e => e.Transactions)
               .Include(cd => cd.ClientObject).ThenInclude(a => a.SalaryDisbursementList)
                .ThenInclude(p => p.TransactionList)
                .Where(cd=>cd.ClientObject.Status == "Pending")
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return users;
        }

        public async Task<int> GetClientCount(string status)
        {
            var clients = await _db.ClientTable.Where(s=>s.Status==status).CountAsync();
            return clients;
        }

        public async Task<Client> GetClientById(int id)
        {
            return await _db.ClientTable.Include(c => c.AccountDetailsObject).Include(d=>d.DocumentList)
                .Include(t => t.BeneficiaryList).ThenInclude(q => q.AccountDetailsObject)
                .Include(e => e.BeneficiaryList).ThenInclude(x => x.PaymentsList).ThenInclude(e => e.Transactions)
                .FirstOrDefaultAsync(z => z.Id == id);
        }
        public (ICollection<User>, int count) GetClientName(string companyName, string status, int page, int pageSize)
        {

           ICollection<User> users= _db.UserTable.Include(cd=>cd.ClientObject).ThenInclude(c => c.AccountDetailsObject)
                .Include(cd => cd.ClientObject).ThenInclude(d => d.EmployeeList).ThenInclude(y => y.AccountDetailsObject)
                .Include(cd => cd.ClientObject).ThenInclude(t => t.BeneficiaryList).ThenInclude(q => q.AccountDetailsObject)
                .Include(cd => cd.ClientObject).ThenInclude(e => e.BeneficiaryList).ThenInclude(x => x.PaymentsList).ThenInclude(e => e.Transactions)
               .Include(cd => cd.ClientObject).ThenInclude(a => a.SalaryDisbursementList).ThenInclude(p => p.TransactionList)
                .Where(cd => cd.ClientObject.CompanyName.StartsWith(companyName)&& cd.ClientObject.Status==status)
                .ToList();

            var count = users.Count();
            var paginatedUser = users.Skip((page-1)*pageSize).Take(pageSize).ToList();
            return (paginatedUser, count);
        }

        public async Task<ICollection<Documents>> GetDocuments(int clientId)
        {
            Client client= await _db.ClientTable.Include(c => c.DocumentList).FirstOrDefaultAsync(x=>x.Id==clientId);
            return client.DocumentList.Skip(Math.Max(0, client.DocumentList.Count - 3))
                    .Take(3) 
                    .ToList();
        }

        public async Task<(ICollection<SalaryDisbursementResponseDto>, int count)> GetSalaryDisbursementClient(int clientId, int page, int pageSize)
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
            var count = salaryDisbursementResponseDtoList.Count();
            var paginatedSalary = salaryDisbursementResponseDtoList.OrderBy(s => s.Status).Skip((page-1)*pageSize).Take(pageSize).ToList();
            return (paginatedSalary,count);
        }


        public void ClientStatus(int clientId, string status)
        {
            Client client = _db.ClientTable.Find(clientId);
            client.Status = status;
            _db.SaveChanges();
        }

        public bool UpdatePaymentStatus(int clientId, int beneficiaryId, int paymentId, string status)
        {
            try
            {
                // Retrieve the payment with related transactions
                Payment payment =  _db.PaymentTable
                    .Include(x => x.Transactions)
                    .FirstOrDefault(o => o.Id == paymentId);
                Beneficiary beneficiary = _db.BeneficiaryTable.Include(c=>c.AccountDetailsObject).FirstOrDefault(x=>x.Id==beneficiaryId);

                if (payment == null)
                {
                    throw new Exception("Payment not found");
                }

                Client client = _db.ClientTable.Include(x => x.AccountDetailsObject).FirstOrDefault(o => o.Id == clientId);
                if (payment == null)
                {
                    throw new Exception("Client not found");
                }

                // Update the payment details
                payment.Status = status;
                payment.ApprovedAt = DateTime.Now;
                payment.ApprovedBy = 4;  // Assumes an admin or approver ID

                // If the status is "Success", add a new transaction
                if (status == "Success")
                {
                    if (client.AccountDetailsObject.AccountBalance > payment.Amount) 
                    {
                        Transactions transaction = new Transactions
                        {
                            TransactionDate = DateTime.Now,
                            TransactionAmount = payment.Amount,
                            TransactionStatus = status,
                        };
                        payment.Transactions.Add(transaction);
                        
                        if (beneficiary.AccountDetailsObject == null)
                        {
                            Client inboundClient = _db.ClientTable.Include(ac=>ac.AccountDetailsObject).FirstOrDefault(f=>f.Id==beneficiary.InbounClientId);
                            inboundClient.AccountDetailsObject.AccountBalance += payment.Amount;
                            string subject = "Account Credited";
                            string body = $"Amount {payment.Amount} had been credited to the AccountNo: {inboundClient.AccountDetailsObject.AccountNumber}";
                            EmailHandler.SendEmail(beneficiary.Email, subject, body);
                            client.AccountDetailsObject.AccountBalance = client.AccountDetailsObject.AccountBalance - payment.Amount;
                            _db.SaveChanges();
                        }
                        else
                        {
                            string subject = "Account Credited";
                            string body = $"Amount {payment.Amount} had been credited to the AccountNo: {beneficiary.AccountDetailsObject.AccountNumber}";
                            EmailHandler.SendEmail(beneficiary.Email, subject, body);

                            client.AccountDetailsObject.AccountBalance = client.AccountDetailsObject.AccountBalance - payment.Amount;
                            _db.SaveChanges();

                        }


                    }
                    else if ((client.AccountDetailsObject.AccountBalance*client.ForPayment*0.01) < payment.Amount)
                    {
                        throw new Exception("Insufficient Balance For Payments");
                        
                    }
                }
                // Save changes to the database
                _db.SaveChanges();
                Console.WriteLine("Changes saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex}");
                throw;  // Rethrow the exception to handle it outside if necessary
            }
        }

        public void UpdateSalaryDisbursementStatus(int clientId,int salaryDisId, string status)
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
                    Employee employee = _db.EmployeeTable.Include(c=>c.AccountDetailsObject).FirstOrDefault(o=>o.EmployeeId==salaryFor.EmployeeId);
                    Client client = _db.ClientTable.Include(x => x.AccountDetailsObject).FirstOrDefault(o => o.Id == clientId);
                    if ((client.AccountDetailsObject.AccountBalance * client.ForSalary*0.01) > salaryDisbursement.Amount)
                    {
                        Transactions transaction = new Transactions
                        {
                            TransactionDate = DateTime.Now,
                            TransactionAmount = employee.Salary,
                            TransactionStatus = status,
                            EmployeePaidId = salaryFor.EmployeeId
                        };
                        salaryDisbursement.TransactionList.Add(transaction);

                        client.AccountDetailsObject.AccountBalance = client.AccountDetailsObject.AccountBalance - employee.Salary;
                        _db.SaveChanges();
                        string subject = "Salary Credited";
                        string body = $"Amount of {employee.Salary} has been credited to your account from {client.CompanyName}" +
                            $"to your AccountNo: {employee.AccountDetailsObject.AccountNumber}";
                        EmailHandler.SendEmail(employee.Email, subject, body);
                    }
                }
            }
            _db.SaveChanges();

        }

        public (ICollection<Beneficiary>, int count) BeneficiartyOption(int clientId, int page, int pageSize)
        {
            Client client =  _db.ClientTable.Include(c => c.BeneficiaryList.Where(b=>b.IsActive))
                            .ThenInclude(ac => ac.AccountDetailsObject)
                            .FirstOrDefault(f=>f.Id==clientId);
                                        
            var inboundCLientsIncluded = new List<Beneficiary>();
            foreach(Beneficiary beneficiary in client.BeneficiaryList)
            {
                if (beneficiary.AccountDetailsObject == null)
                {
                    Client inbounCLient = _db.ClientTable.Include(ac => ac.AccountDetailsObject).FirstOrDefault(f => f.Id == beneficiary.InbounClientId);
                    beneficiary.AccountDetailsObject = inbounCLient.AccountDetailsObject;
                    inboundCLientsIncluded.Add(beneficiary);
                }
                else
                {
                    inboundCLientsIncluded.Add(beneficiary);
                }
            }
            int count =  client.BeneficiaryList.Count();
            var paginatedBenificiary =  client.BeneficiaryList.OrderBy(s=>s.BenificiaryName).
                Skip((page-1) * pageSize).Take(pageSize).ToList();

            return (paginatedBenificiary, count);

        }

        public (ICollection<Payment>, int count) PaymentsOfBeneficiary(int beneficiaryId, int page, int pageSize)
        {
            Beneficiary beneficiary =  _db.BeneficiaryTable.Include(ac => ac.PaymentsList).ThenInclude(c=>c.Transactions)
                            .FirstOrDefault(f => f.Id == beneficiaryId);

            int count = beneficiary.PaymentsList.Count();
            var paginatedPayments = beneficiary.PaymentsList.OrderBy(s => s.Status).
                Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return (paginatedPayments, count);

        }

        public (ICollection<Beneficiary>, int count) GetBeneficiaryByName(int clientId, string beneficiaryName, int page, int pageSize)
        {
            Client client = _db.ClientTable.Include(c => c.BeneficiaryList.Where(b => b.IsActive))
                            .ThenInclude(ac => ac.AccountDetailsObject)
                            .FirstOrDefault(f => f.Id == clientId);

           var paginatedBeneficiarySearch = client.BeneficiaryList.Where(c=>c.BenificiaryName.StartsWith(beneficiaryName))
                                            .Where(b=>b.IsActive).Skip((page-1)*pageSize).Take(pageSize).ToList();
            int count = paginatedBeneficiarySearch.Count();
            return (paginatedBeneficiarySearch, count);
        }


        public (ICollection<Payment>, int count) GetPaymentByName(int beneficiaryId, string paymentName, int page, int pageSize)
        {
            Beneficiary beneficiary = _db.BeneficiaryTable.Include(c=>c.PaymentsList).ThenInclude(x=>x.Transactions)
                                      .FirstOrDefault(z=>z.Id == beneficiaryId);

            var payments = beneficiary.PaymentsList.Where(c=>c.PaymentType.StartsWith(paymentName)).
                                           OrderBy(c=>c.PaymentType).ToList();
            var paginatedPaymentSearch = payments.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var count = payments.Count();

            return (paginatedPaymentSearch, count);


        }



    }
}
