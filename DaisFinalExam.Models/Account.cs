using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaisFinalExam.Models;

namespace DaisFinalExam.Models
{
    public class Account
    {
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Account name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Account name should be between 3 and 100 characters long")]
        public string AccountName { get; set; }
        [Required]
        [StringLength(22, MinimumLength = 22, ErrorMessage = "Account number should be exactly 22 characters long")]
        public string AccountNumber { get; set; }
    }
}



