using System.ComponentModel.DataAnnotations;

namespace Acme.DrawLanding.Website.Domain.Users;

public sealed class CreateUserRequest
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
