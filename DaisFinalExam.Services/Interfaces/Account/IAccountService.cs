using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaisFinalExam.Services.DTOs.Account;

namespace DaisFinalExam.Services.Interfaces.Account
{
    public interface IAccountService
    {
        Task<GetAccountsByUserResponse> GetAccountsByUserIdAsync(int userId);

    }
}
