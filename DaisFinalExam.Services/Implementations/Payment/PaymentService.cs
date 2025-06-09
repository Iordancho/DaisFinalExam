using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaisFinalExam.Models;
using DaisFinalExam.Repository.Interfaces.Account;
using DaisFinalExam.Repository.Interfaces.Payment;
using DaisFinalExam.Repository.Interfaces.UserAccount;
using DaisFinalExam.Services.DTOs.Payment;
using DaisFinalExam.Services.Interfaces.Payment;

namespace DaisFinalExam.Services.Implementations.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository paymentRepository;
        private readonly IUserAccountRepository userAccountRepository;
        private readonly IAccountRepository accountRepository;

        public const string PendingStatus = "ИЗЧАКВА";
        public const string FinishedStatus = "ОБРАБОТЕН";
        public const string CanceledStatus = "ОТКАЗАН";


        public PaymentService(
            IPaymentRepository _paymentRepository,
            IUserAccountRepository _userAccountRepository,
            IAccountRepository _accountRepository)
        {
            paymentRepository = _paymentRepository;
            userAccountRepository = _userAccountRepository;
            accountRepository = _accountRepository;
        }
        public async Task<CancelPaymentResponse> CancelPaymentAsync(CancelPaymentRequest request)
        {
            var payment = await paymentRepository.RetrieveAsync(request.PaymentId);
            if (payment == null)
            {
                return new CancelPaymentResponse
                {
                    Success = false,
                    ErrorMessage = "Не съществува такова плащане",
                };
            }

            var account = await accountRepository.RetrieveAsync(payment.FromAccountId);
            if (account == null)
            {
                return new CancelPaymentResponse
                {
                    Success = false,
                    ErrorMessage = "Сметката, от която трябва да се прекрати плащането, не съществува.",
                };
            }
            if (payment.CreatorId != request.UserId)
            {
                return new CancelPaymentResponse
                {
                    Success = false,
                    ErrorMessage = "Нямате право да прекратите това плащане",
                };
            }
            if (payment.Status != PendingStatus)
            {
                return new CancelPaymentResponse
                {
                    Success = false,
                    ErrorMessage = $"Имате право да прекратявате само плащания със статус: {PendingStatus}",
                };
            }

            var paymentUpdate = new PaymentUpdate
            {
                Status = CanceledStatus
            };
            var paymentUpdateResponse = await paymentRepository.UpdateAsync(payment.PaymentId, paymentUpdate);
            return new CancelPaymentResponse
            {
                Success = true,
            };
        }
        public async Task<FinishPaymentResponse> FinishPaymentAsync(FinishPaymentRequest request)
        {
            var payment = await paymentRepository.RetrieveAsync(request.PaymentId);
            if (payment == null)
            {
                return new FinishPaymentResponse
                {
                    Success = false,
                    ErrorMessage = "Не съществува такова плащане",
                };
            }

            var account = await accountRepository.RetrieveAsync(payment.FromAccountId);

            var userAccount = await userAccountRepository.RetrieveCollectionAsync(
                new UserAccountFilter
                {
                    UserId = request.UserId,
                    AccountId = account.AccountId
                }).FirstOrDefaultAsync();


            if (account == null)
            {
                return new FinishPaymentResponse
                {
                    Success = false,
                    ErrorMessage = "Сметката, от която трябва да се извърши плащането, не съществува.",
                };
            }

            if(userAccount == null)
            {
                return new FinishPaymentResponse
                {
                    Success = false,
                    ErrorMessage = "Нямате такава сметка",
                };
            }

            if (payment.CreatorId != request.UserId)
            {
                return new FinishPaymentResponse
                {
                    Success = false,
                    ErrorMessage = "Нямате право да извършите това плащане",
                };
            }
            if(payment.Amount > userAccount.Balance)
            {
                return new FinishPaymentResponse
                {
                    Success = false,
                    ErrorMessage = "Нямате достатъчно баланс в сметката си за да извършите плащането",
                };
            }
            if(payment.Status != PendingStatus)
            {
                return new FinishPaymentResponse
                {
                    Success = false,
                    ErrorMessage = $"Имате право да извършвате само плащания със статус: {PendingStatus}",
                };
            }

            var accountUpdate = new UserAccountUpdate
            {
                Balance = userAccount.Balance - payment.Amount,
                AccountId = account.AccountId
            };
            var accountUpdateResponse = await userAccountRepository.UpdateAsync(request.UserId, accountUpdate);

            var paymentUpdate = new PaymentUpdate
            {
                Status = FinishedStatus
            };
            var paymentUpdateResponse = await paymentRepository.UpdateAsync(payment.PaymentId, paymentUpdate);

            return new FinishPaymentResponse
            {
                Success = true,
            };
        }

        public async Task<GetAllPaymentsResponse> GetAllPaymentsByUserIdAsync(GetAllPaymentsRequest request)
        {
            var payments = paymentRepository.RetrieveCollectionAsync(
                new PaymentFilter { CreatorId = request.UserId });

            var accounts = accountRepository.RetrieveCollectionAsync(
                new AccountFilter { });

            var accountDict = await accounts
                    .ToDictionaryAsync(a => a.AccountId, a => a.AccountName);


            if (!await payments.AnyAsync())
            {
                return new GetAllPaymentsResponse
                {
                    Success = false,
                    ErrorMessage = "Нямате налични плащания"
                };
            }

            var paymentsList = await payments.ToListAsync();

            switch (request.SortingParameter?.ToLower())
            {
                case "date":
                    paymentsList = paymentsList.OrderByDescending(p => p.CreatedAt).ToList();
                    break;
                case "status":
                    paymentsList = paymentsList.OrderBy(p => p.Status != PendingStatus).ThenByDescending(p => p.CreatedAt).ToList();
                    break;
                default:
                    // Default to date sort if no valid param
                    paymentsList = paymentsList.OrderByDescending(p => p.CreatedAt).ToList();
                    break;
            }

            foreach(var payment in paymentsList)
            {

            }

            var response = new GetAllPaymentsResponse
            {
                Payments = paymentsList.Select(p => new GetPaymentInfo
                {
                    PaymentId = p.PaymentId,
                    FromAccountId = p.FromAccountId,
                    FromAccountName = accountDict.ContainsKey(p.FromAccountId) ? accountDict[p.FromAccountId] : "Unknown",
                    ToAccountNumber = p.ToAccountNumber,
                    Status = p.Status,
                    Amount = p.Amount,
                    CreatedAt = p.CreatedAt,
                    Reason = p.Reason,
                }).ToList(),
                Success = true,
            };
            return response;
        }
        public async Task<CreatePaymentResponse> CreatePaymentAsync(CreatePaymentRequest request)
        {
            var userAccount = await userAccountRepository.RetrieveCollectionAsync(
                new UserAccountFilter 
                {
                    UserId = request.UserId ,
                    AccountId = request.FromAccountId,
                }).FirstOrDefaultAsync();

            bool hasAccessToFromAccount = false;
            if (userAccount == null)
            {
                return new CreatePaymentResponse
                {
                    Success = false,
                    ErrorMessage = "Нямате достъп до избраната сметка"
                };
            }

            //await foreach (var ua in userAccounts)
            //{
            //    if (ua.AccountId == request.FromAccountId)
            //    {
            //        hasAccessToFromAccount = true;
            //        break;
            //    }
            //}

            //if (!hasAccessToFromAccount)
            //{
            //    return new CreatePaymentResponse
            //    {
            //        Success = false,
            //        ErrorMessage = "Нямате достъп до избраната сметка"
            //    };
            //}

            var result = await paymentRepository.CreateAsync(new Models.Payment
            {
                FromAccountId = request.FromAccountId,
                CreatorId = request.UserId,
                ToAccountNumber = request.ToAccountNumber,
                Status = PendingStatus,
                Amount = request.Amount,
                CreatedAt = DateTime.UtcNow,
                Reason = request.Reason,
            });

            return new CreatePaymentResponse
            {
                Success = true,
            };
        }
    }
}
