using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaisFinalExam.Services.DTOs.Payment;

namespace DaisFinalExam.Services.Interfaces.Payment
{
    public interface IPaymentService
    {
        Task<CancelPaymentResponse> CancelPaymentAsync(CancelPaymentRequest request);
        Task<FinishPaymentResponse> FinishPaymentAsync(FinishPaymentRequest request);
        Task<GetAllPaymentsResponse> GetAllPaymentsByUserIdAsync(GetAllPaymentsRequest request);
        Task<CreatePaymentResponse> CreatePaymentAsync(CreatePaymentRequest request);
    }
}
