namespace InventorySystem.Application.Features.Auth;

public class RegisterRequest
{
    public string CompanyName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}