using System.ComponentModel.DataAnnotations;

namespace Acme.DrawLanding.Website.Domain.Users;

public sealed class UserLoginRequest
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }
}
