using Capstone_Banking.CommonFunction;
using Capstone_Banking.Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Capstone_Banking.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {

        // POST api/<ClientController>
        [HttpPost]
        public void Post(ICollection<IFormFile> fileList)
        {
            BankingDbContext bankingDbContext = new BankingDbContext();
            string id = "2";

            if (fileList != null)
            {
                (new UploadHandler(bankingDbContext)).Upload(int.Parse(id),fileList);
            }
        }
    }
}
