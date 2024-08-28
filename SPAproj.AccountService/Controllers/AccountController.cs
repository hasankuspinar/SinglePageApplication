using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SPAproj.Models;

namespace SPAproj.AccountService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpGet("getall")]
        public ActionResult<List<Account>> GetAll()
        {
            var accounts = new List<Account>
            {
                new Account { AccountNumber = "123456789", Balance = 1000.00M },
                new Account { AccountNumber = "987654321", Balance = 2500.50M },
                new Account { AccountNumber = "112233445", Balance = 300.75M }
            };

            return Ok(accounts);
        }
    }
}
