using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaisFinalExam.Services.DTOs.Account;

namespace DaisFinalExam.Services.DTOs.Payment
{
    public class CreatePaymentRequest
    {
        public int UserId { get; set; }
        public int FromAccountId { get; set; }
        public string ToAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
    }
}
