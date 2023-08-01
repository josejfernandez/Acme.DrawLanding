namespace Acme.DrawLanding.Library.Domain.Users;

public interface IUserService
{
    public Task CreateUserAsync(string username, string password);

    public Task<bool> ValidateCredentialsAsync(string username, string password);
}
