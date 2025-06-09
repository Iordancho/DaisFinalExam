using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaisFinalExam.Services.DTOs.Payment
{
    public class GetAllPaymentsResponse
    {
        public ICollection<GetPaymentInfo> Payments { get; set; } = new List<GetPaymentInfo>();
        public string ErrorMessage { get; set; }
        public bool Success { get; set; }
    }
}
