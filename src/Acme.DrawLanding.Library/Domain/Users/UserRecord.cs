namespace Acme.DrawLanding.Library.Domain.Users;

public sealed class UserRecord
{
    public int Id { get; set; }

    public string Identity { get; set; } = string.Empty;

    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();

    public byte[] Salt { get; set; } = Array.Empty<byte>();
}
