using MyBlazorApp.Application.Interfaces;
using MyBlazorApp.Domain.Entities;
using MyBlazorApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;

namespace MyBlazorApp.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<string?> AuthenticateAsync(string username, string password)
    {
        // Find user by username
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        
        if (user == null)
            return null; // User not found

        // Verify password hash
        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return null; // Password incorrect

        // Generate JWT token
        return GenerateJwtToken(user);
    }

    public async Task<bool> RegisterUserAsync(string username, string email, string password)
    {
        // Check if user already exists
        if (await _context.Users.AnyAsync(u => u.Username == username))
            return false;

        // Hash the password
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        // Create new user
        var newUser = new User
        {
            Username = username,
            Email = email,
            PasswordHash = hashedPassword
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return true;
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSecret = _configuration["Jwt:Secret"];
        var jwtExpiration = int.Parse(_configuration["Jwt:ExpirationInMinutes"] ?? "60");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var token = new JwtSecurityToken(
            issuer: "PlaygroundAPI",
            audience: "PlaygroundUI",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtExpiration),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
