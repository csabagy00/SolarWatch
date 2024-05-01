namespace SolarWatch.Services.Authentication;

public interface IAuthService
{
    public Task<AuthResult> RegisterAsync(string email, string username, string password);
}