namespace Acme.DrawLanding.Library.Common.Encryption;

public sealed class EncryptionKey
{
    public byte[] Value { get; }

    public EncryptionKey(byte[] value)
    {
        if (value == null || value.Length == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Encryption key must not be empty.");
        }

        Value = value;
    }
}
