using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaisFinalExam.Repository.Implementations.UserAccount;
using DaisFinalExam.Repository.Interfaces.Account;
using DaisFinalExam.Repository.Interfaces.UserAccount;

using DaisFinalExam.Services.DTOs.Account;
using DaisFinalExam.Services.Interfaces.Account;

namespace DaisFinalExam.Services.Implementations.Account
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository accountRepository;
        private readonly IUserAccountRepository userAccountRepository;

        public AccountService(
            IAccountRepository _accountRepository,
            IUserAccountRepository _userAccountRepository)
        {
            accountRepository = _accountRepository;
            userAccountRepository = _userAccountRepository;
        }

        public async Task<GetAccountsByUserResponse> GetAccountsByUserIdAsync(int userId)
        {
            var accountsByUserIdResponse = new GetAccountsByUserResponse();
            var accountsByUserId = userAccountRepository.RetrieveCollectionAsync(
                new UserAccountFilter { UserId = userId });


            if(!await accountsByUserId.AnyAsync())
            {
                return new GetAccountsByUserResponse
                {
                    Success = false,
                    ErrorMessage = "Нямате налични сметки"
                };
            }

            await foreach(var accountByUserId in accountsByUserId)
            {
                var account = await accountRepository.RetrieveAsync(accountByUserId.AccountId);
                if (account == null)
                    continue;

                accountsByUserIdResponse.Accounts.Add(new GetAccountResponse
                {
                    AccountName = account.AccountName,
                    AccountId = account.AccountId,
                    AccountNumber = account.AccountNumber,
                    Balance = accountByUserId.Balance,
                });
            }

            accountsByUserIdResponse.Success = true;
            return accountsByUserIdResponse;
        }
    }
}
