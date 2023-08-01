using Acme.DrawLanding.Library.Common.Encryption;
using Acme.DrawLanding.Library.Data;
using Microsoft.EntityFrameworkCore;

namespace Acme.DrawLanding.Library.Domain.Users;

public sealed class UserService : IUserService
{
    private readonly IEncryptionService _encryptionService;
    private readonly IUserCredentialsService _userCredentialsService;
    private readonly AppDbContext _context;
    private readonly EncryptionKey _encryptionKey;

    public UserService(
        IEncryptionService encryptionService,
        IUserCredentialsService userCredentialsService,
        AppDbContext context,
        EncryptionKey encryptionKey)
    {
        _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
        _userCredentialsService = userCredentialsService ?? throw new ArgumentNullException(nameof(userCredentialsService));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _encryptionKey = encryptionKey ?? throw new ArgumentNullException(nameof(encryptionKey));
    }

    public async Task<bool> ValidateCredentialsAsync(string username, string password)
    {
        var user = await _context.Users
            .Where(x => x.Identity == username)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return false;
        }

        var inputPasswordHash = _userCredentialsService.HashPassword(password, user.Salt);

        var decryptedExistingPasswordHash = await _encryptionService
            .DecryptToBytesAsync(_encryptionKey.Value, user.PasswordHash);

        if (!inputPasswordHash.SequenceEqual(decryptedExistingPasswordHash))
        {
            return false;
        }

        return true;
    }

    public async Task CreateUserAsync(string username, string password)
    {
        var salt = _userCredentialsService.CreateRandomSalt();

        var passwordHash = _userCredentialsService.HashPassword(password, salt);

        var encryptedPasswordHash = await _encryptionService.EncryptAsync(_encryptionKey.Value, passwordHash);

        var entity = new UserRecord()
        {
            Identity = username,
            PasswordHash = encryptedPasswordHash,
            Salt = salt,
        };

        _context.Add(entity);

        await _context.SaveChangesAsync();
    }
}