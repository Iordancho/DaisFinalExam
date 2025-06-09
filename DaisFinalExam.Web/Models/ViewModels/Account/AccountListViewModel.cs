namespace DaisFinalExam.Web.Models.ViewModels.Account
{
    public class AccountListViewModel
    {
        public ICollection<AccountViewModel> Accounts { get; set; } = new List<AccountViewModel>();
    }
}
