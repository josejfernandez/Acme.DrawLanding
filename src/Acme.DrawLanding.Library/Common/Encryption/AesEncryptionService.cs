using System.Security.Cryptography;

namespace Acme.DrawLanding.Library.Common.Encryption;

public sealed class AesEncryptionService : IEncryptionService
{
    public const int AesKeySize = 256;
    public const int AesBlockSize = 128;

    public async Task<byte[]> EncryptAsync(byte[] key, byte[] data)
    {
        ValidateEncryptionKey(key);

        using var memory = new MemoryStream();
        using var aes = Aes.Create();

        aes.Key = key;

        await memory.WriteAsync(aes.IV.AsMemory(0, aes.IV.Length));

        using var encryptor = aes.CreateEncryptor();

        await using (var crypto = new CryptoStream(memory, encryptor, CryptoStreamMode.Write))
        {
            await crypto.WriteAsync(data);
        }

        return memory.ToArray();
    }

    public async Task<string> DecryptToStringAsync(byte[] key, byte[] data)
    {
        ValidateEncryptionKey(key);

        using var memory = new MemoryStream(data);
        using var aes = Aes.Create();

        var iv = new byte[AesBlockSize / 8];
        var numBytesToRead = AesBlockSize / 8;
        var numBytesRead = 0;

        while (numBytesToRead > 0)
        {
            var n = await memory.ReadAsync(iv.AsMemory(numBytesRead, numBytesToRead));

            if (n == 0)
            {
                break;
            }

            numBytesRead += n;
            numBytesToRead -= n;
        }

        using var encryptor = aes.CreateDecryptor(key, iv);

        await using var cryptoStream = new CryptoStream(memory, encryptor, CryptoStreamMode.Read);
        using var decryptReader = new StreamReader(cryptoStream);

        return await decryptReader.ReadToEndAsync();
    }

    public async Task<byte[]> DecryptToBytesAsync(byte[] key, byte[] data)
    {
        ValidateEncryptionKey(key);

        using var memory = new MemoryStream(data);
        using var aes = Aes.Create();

        var iv = new byte[AesBlockSize / 8];
        var numBytesToRead = AesBlockSize / 8;
        var numBytesRead = 0;

        while (numBytesToRead > 0)
        {
            var n = await memory.ReadAsync(iv.AsMemory(numBytesRead, numBytesToRead));

            if (n == 0)
            {
                break;
            }

            numBytesRead += n;
            numBytesToRead -= n;
        }

        using var encryptor = aes.CreateDecryptor(key, iv);

        using var result = new MemoryStream();
        await using var cryptoStream = new CryptoStream(memory, encryptor, CryptoStreamMode.Read);

        await cryptoStream.CopyToAsync(result);
        return result.ToArray();
    }

    private static void ValidateEncryptionKey(byte[] bytes)
    {
        if (bytes == null)
        {
            throw new ArgumentNullException(nameof(bytes));
        }

        if (bytes.Length != AesKeySize / 8)
        {
            throw new ArgumentOutOfRangeException(
                nameof(bytes),
                Messages.InvalidKeySize.With(bytes.Length, AesKeySize / 8));
        }
    }
}
