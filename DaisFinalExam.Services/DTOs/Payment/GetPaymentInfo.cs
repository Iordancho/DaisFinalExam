using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaisFinalExam.Services.DTOs.Payment
{
    public class GetPaymentInfo
    {
        public int PaymentId { get; set; }
        public int FromAccountId { get; set; }
        public string FromAccountName { get; set; }
        public string ToAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
