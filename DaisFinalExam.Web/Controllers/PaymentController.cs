using Azure;
using DaisFinalExam.Services.DTOs.Payment;
using DaisFinalExam.Services.Interfaces.Payment;
using DaisFinalExam.Web.Attributes;
using DaisFinalExam.Web.Models.ViewModels.Payment;
using Microsoft.AspNetCore.Mvc;

namespace DaisFinalExam.Web.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly IPaymentService paymentService;

        public PaymentController(IPaymentService _paymentService)
        {
            paymentService = _paymentService;
        }

        [HttpGet]
        public async Task<IActionResult> Cancel(int id)
        {
            var model = new CancelPaymentViewModel
            {
                PaymentId = id
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Cancel(CancelPaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userId = HttpContext.Session.GetInt32("UserId").Value;

            var response = await paymentService.CancelPaymentAsync(new CancelPaymentRequest
            {
                PaymentId = model.PaymentId,
                UserId = userId
            });

            if (!response.Success)
            {
                TempData["ErrorMessage"] = response.ErrorMessage;
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("GetAll");
        }

        [HttpGet]
        public async Task<IActionResult> Finish(int id)
        {
            var model = new FinishPaymentViewModel
            {
                PaymentId = id
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Finish(FinishPaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userId = HttpContext.Session.GetInt32("UserId").Value;

            var response = await paymentService.FinishPaymentAsync(new FinishPaymentRequest
            {
                PaymentId = model.PaymentId,
                UserId = userId
            });

            if (!response.Success)
            {
                TempData["ErrorMessage"] = response.ErrorMessage;
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("GetAll");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string sortingParameter)
        {
            var userId = HttpContext.Session.GetInt32("UserId").Value;

            var request = new GetAllPaymentsRequest
            {
                UserId = userId,
                SortingParameter = sortingParameter,
            };
            var payments = await paymentService.GetAllPaymentsByUserIdAsync(request);

            var model = new PaymentListViewModel
            {
                Payments = payments.Payments.Select(p => new PaymentViewModel
                {
                    PaymentId = p.PaymentId,
                    FromAccountName = p.FromAccountName,
                    Status = p.Status,
                    Amount = p.Amount,
                    CreatedAt = p.CreatedAt,
                    FromAccountId = p.FromAccountId,
                    Reason = p.Reason,
                    ToAccountNumber = p.ToAccountNumber,
                }).ToList(),
                ErrorMessage = payments.Success ? null : payments.ErrorMessage
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int FromAccountId)
        {
            var model = new CreatePaymentViewModel
            {
                FromAccountId = FromAccountId,
            };
            return View(model);
        }

        public async Task<IActionResult> Create(CreatePaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = HttpContext.Session.GetInt32("UserId").Value;

            var response = await paymentService.CreatePaymentAsync(new CreatePaymentRequest
            {
                UserId = userId,
                FromAccountId = model.FromAccountId,
                ToAccountNumber = model.ToAccountNumber,
                Amount = model.Amount,
                Reason = model.Reason,
            });

            if (!response.Success)
            {
                TempData["ErrorMessage"] = response.ErrorMessage;
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("GetAll", "Account");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
