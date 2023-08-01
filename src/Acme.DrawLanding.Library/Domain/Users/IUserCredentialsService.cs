namespace Acme.DrawLanding.Library.Domain.Users;

public interface IUserCredentialsService
{
    byte[] CreateRandomSalt();

    byte[] HashPassword(string password, byte[] salt);
}
