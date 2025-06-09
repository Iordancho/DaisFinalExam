using System.ComponentModel.DataAnnotations;

namespace DaisFinalExam.Web.Models.ViewModels.Payment
{
    public class PaymentViewModel
    {
        public int PaymentId { get; set; }
        public int CreatorId { get; set; }
        public int FromAccountId { get; set; }
        public string FromAccountName { get; set; }
        public string ToAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
