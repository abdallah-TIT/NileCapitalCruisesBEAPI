using NileCapitalCruisesBEAPI.DTOs.Step_Authentication;

namespace NileCapitalCruisesBEAPI.Services
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);
        Task<AuthModel> GetTokenAsync(TokenRequestModel model);
        Task<AuthModel> RefreshTokenAsync(string token);
    }
}
