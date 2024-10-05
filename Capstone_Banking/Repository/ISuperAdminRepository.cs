﻿using Capstone_Banking.Dto;
using Capstone_Banking.Model;

namespace Capstone_Banking.Repository
{
    public interface ISuperAdminRepository
    {
        public Task<ICollection<Client>> GetAllClientsPaged(int page, int pageSize);
        public Task<ICollection<User>> GetAllClientsPagedPending(int page, int pageSize);

        public ICollection<Client> GetAllClients();
        public  Task<int> GetClientCount(string status);

        public Task<Client> GetClientById(int id);
        public ICollection<User> GetClientName(string companyName, string status);


        public Task<ICollection<Documents>> GetDocuments(int clientId);

        public Task<ICollection<SalaryDisbursementResponseDto>> GetSalaryDisbursementClient(int clientId);

        public void ClientStatus(int clientId, string status);

        public void UpdatePaymentStatus(int clientId,int beneficiaryId, int payementId, string status);

        public void UpdateSalaryDisbursementStatus(int clientId,int salaryDisId, string status);



    }
}
