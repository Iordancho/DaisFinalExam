using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaisFinalExam.Services.DTOs.Account
{
    public class GetAccountResponse
    {
        public string AccountName { get; set; }
        public int AccountId { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
    }
}
