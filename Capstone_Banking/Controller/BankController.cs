﻿using System.Reflection.Metadata;
using Capstone_Banking.CommonFunction;
using Capstone_Banking.Data;
using Capstone_Banking.Dto;
using Capstone_Banking.Model;
using Capstone_Banking.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Capstone_Banking.Controller
{
    [Authorize(Roles = "Bank")]
    [Route("api/[controller]")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private IBankService _bankService;
        private UploadHandler _uploadHandler;

        public BankController(IBankService bankService, UploadHandler uploadHandler)
        {
            _bankService = bankService;
            _uploadHandler = uploadHandler;
        }
        [HttpPost]
        public async Task Register([FromBody] User user)
        {
            await _bankService.AddBank(user);
        }

        [HttpGet]
        public Task<ICollection<Client>> Get()
        {
            return _bankService.GetAllClients();
        }

        [HttpGet("{clientId}")]
        public async Task<Client> GetById(int clientId)
        {

            return await _bankService.GetClientsById(clientId);
        }

        [HttpGet("Document/{clientId}")]
        public async Task<ICollection<Documents>> GetDocumentsNameOfClient(int clientId)
        {

            return await _bankService.GetDocuments(clientId);
        }
        [HttpGet("Download/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            
            BankingDbContext bankingDbContext = new BankingDbContext();
            string fileResult = await _uploadHandler.DownloadFile(fileName);
            if (fileResult.Length==0)
            {
                return NotFound("File not found");
            }
            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            AddAuditLogs.AddLog(int.Parse(userId), "File Download", "File Downloaded");

            return Ok(new { documentUrl = fileResult }); ;
        }

        [HttpGet("SalaryDisbursement/{clientId}")]
        public async Task<ICollection<SalaryDisbursementResponseDto>> GetSalaryDisbursementClient(int clientId)
        {

            return await _bankService.GetSalaryDisbursementClient(clientId);
        }
    }
}
