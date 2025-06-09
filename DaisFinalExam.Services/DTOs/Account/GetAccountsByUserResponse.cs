using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaisFinalExam.Services.DTOs.Account
{
    public class GetAccountsByUserResponse
    {
        public ICollection<GetAccountResponse> Accounts { get; set; } = new List<GetAccountResponse>();
        public string ErrorMessage { get; set; }
        public bool Success { get; set; }
    }
}
