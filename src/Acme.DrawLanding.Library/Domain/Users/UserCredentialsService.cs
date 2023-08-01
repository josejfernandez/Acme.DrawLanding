using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

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

        var passwordBytes = Encoding.UTF8.GetBytes(password);

        // https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html#argon2id
        using var hasher = new Argon2d(passwordBytes)
        {
            DegreeOfParallelism = 1,
            MemorySize = 32768,
            Iterations = 10,
            Salt = salt,
        };

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
