using DaisFinalExam.Services.Interfaces.Account;
using DaisFinalExam.Web.Attributes;
using DaisFinalExam.Web.Models.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;

namespace DaisFinalExam.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService _accountService)
        {
            accountService = _accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = HttpContext.Session.GetInt32("UserId").Value;

            var allAccounts = await accountService.GetAccountsByUserIdAsync(userId);

            var model = new AccountListViewModel
            {
                Accounts = allAccounts.Accounts.Select(a => new AccountViewModel
                {
                    AccountId = a.AccountId,
                    AccountName = a.AccountName,
                    AccountNumber = a.AccountNumber,
                    Balance = a.Balance,
                }).ToList()
            };

            return View(model);
            
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
