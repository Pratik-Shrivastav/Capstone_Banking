﻿using Capstone_Banking.Dto;

namespace Capstone_Banking.Repository
{
    public interface ISuperAdminRepository
    {
        public Task<ICollection<Client>> GetAllClients();

        public Task<Client> GetClientById(int id);

        public Task<ICollection<Documents>> GetDocuments(int clientId);

        public Task<ICollection<SalaryDisbursementResponseDto>> GetSalaryDisbursementClient(int clientId);

        public void ClientStatus(int clientId, string status);

        public void UpdatePaymentStatus(int clientId, int payementId, string status);

        public void UpdateSalaryDisbursementStatus(int clientId,int salaryDisId, string status);



    }
}
