using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaisFinalExam.Services.DTOs.Payment
{
    public class CancelPaymentRequest
    {
        public int UserId { get; set; }
        public int PaymentId { get; set; }
    }
}
