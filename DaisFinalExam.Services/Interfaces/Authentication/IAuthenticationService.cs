using DaisFinalExam.Services.DTOs.Authentication;

namespace DaisFinalExam.Services.Interfaces.Authentication
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}
