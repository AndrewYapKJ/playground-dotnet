using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace MyBlazorApp.UI.Auth;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;

    public CustomAuthStateProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            // Check if token exists in localStorage
            var token = await GetTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            // Parse JWT token to get claims
            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }
        catch
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    public async Task LoginAsync(string token)
    {
        // Store token
        await SetTokenAsync(token);
        
        // Notify that auth state has changed
        var claims = ParseClaimsFromJwt(token);
        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public async Task LogoutAsync()
    {
        // Clear token
        await ClearTokenAsync();
        
        // Notify that auth state has changed
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
    }

    private static List<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = System.Text.Json.JsonDocument.Parse(jsonBytes).RootElement;

        foreach (var kvp in keyValuePairs.EnumerateObject())
        {
            var claimValue = kvp.Value.ToString();
            var claimType = kvp.Name;
            claims.Add(new Claim(claimType, claimValue));
        }

        return claims;
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }

    private async Task<string> GetTokenAsync()
    {
        // In a real app, use sessionStorage/localStorage via JS interop
        // For now, return empty (we'll enhance this later)
        return await Task.FromResult(string.Empty);
    }

    private async Task SetTokenAsync(string token)
    {
        // Store token (would use JS interop for localStorage)
        await Task.CompletedTask;
    }

    private async Task ClearTokenAsync()
    {
        // Clear token
        await Task.CompletedTask;
    }
}
