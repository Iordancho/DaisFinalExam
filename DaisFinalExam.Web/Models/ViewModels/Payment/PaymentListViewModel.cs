namespace DaisFinalExam.Web.Models.ViewModels.Payment
{
    public class PaymentListViewModel
    {
        public ICollection<PaymentViewModel> Payments { get; set; } = new List<PaymentViewModel>();
        public string ErrorMessage { get; set; }
    }
}
