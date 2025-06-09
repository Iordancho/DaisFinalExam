using System.ComponentModel.DataAnnotations;

namespace DaisFinalExam.Web.Models.ViewModels.Account
{
    public class AccountViewModel
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
    }
}
