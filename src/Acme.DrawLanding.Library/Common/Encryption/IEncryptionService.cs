namespace Acme.DrawLanding.Library.Common.Encryption;

public interface IEncryptionService
{
    Task<byte[]> EncryptAsync(byte[] key, byte[] data);

    Task<byte[]> DecryptToBytesAsync(byte[] key, byte[] data);
}
