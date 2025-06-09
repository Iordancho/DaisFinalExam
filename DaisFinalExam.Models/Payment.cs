using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaisFinalExam.Models;

namespace DaisFinalExam.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        [Required(ErrorMessage = "Creator is required")]
        public int CreatorId { get; set; }

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

        [Required(ErrorMessage = "Status is required")]
        [StringLength(15, ErrorMessage = "Status cannot be more then 15 characters long")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
    }
}


