using System.ComponentModel.DataAnnotations;

namespace DaisFinalExam.Web.Models.ViewModels.Payment
{
    public class CreatePaymentViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Sender is required")]
        public int FromAccountId { get; set; }

        [Required(ErrorMessage = "Receiver is required")]
        [StringLength(22, MinimumLength = 22, ErrorMessage = "Account number should be exactly 22 characters long")]
        public string ToAccountNumber { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Reason is required")]
        [StringLength(32, ErrorMessage = "Reason cannot be more than 32 characters long")]
        public string Reason { get; set; }
    }
}

