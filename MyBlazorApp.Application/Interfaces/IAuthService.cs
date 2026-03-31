namespace MyBlazorApp.Application.Interfaces;

public interface IAuthService
{
    Task<string?> AuthenticateAsync(string username, string password);
    Task<bool> RegisterUserAsync(string username, string email, string password);
}
