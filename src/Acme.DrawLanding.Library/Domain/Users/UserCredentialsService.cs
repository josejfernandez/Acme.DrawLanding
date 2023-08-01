using System.Security.Cryptography;
using System.Text;

namespace Acme.DrawLanding.Library.Domain.Users;

public sealed class UserCredentialsService : IUserCredentialsService
{
    private const int PasswordSaltLengthInBytes = 256;
    private const int PasswordHashLengthInBytes = 128;

    public byte[] CreateRandomSalt()
    {
        return RandomNumberGenerator.GetBytes(PasswordSaltLengthInBytes / 8);
    }

    public byte[] HashPassword(string password, byte[] salt)
    {
        ValidatePasswordSalt(salt);

        using var hasher = new Rfc2898DeriveBytes(
            Encoding.UTF8.GetBytes(password),
            salt,
            100000,
            HashAlgorithmName.SHA512);

        return hasher.GetBytes(PasswordHashLengthInBytes);
    }

    private static void ValidatePasswordSalt(byte[] bytes)
    {
        if (bytes == null)
        {
            throw new ArgumentNullException(nameof(bytes));
        }

        if (bytes.Length != PasswordSaltLengthInBytes / 8)
        {
            throw new ArgumentOutOfRangeException(
                nameof(bytes),
                Messages.InvalidKeySize.With(bytes.Length, PasswordSaltLengthInBytes / 8));
        }
    }
}
