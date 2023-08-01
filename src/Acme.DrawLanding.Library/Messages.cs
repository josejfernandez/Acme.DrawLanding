using System.Globalization;

namespace Acme.DrawLanding.Library;

public static class Messages
{
    public const string InvalidKeySize = "Invalid key size. Current size is '{0}', but must be '{1}'.";

    public static string With(this string message, params object[] args)
    {
        return string.Format(CultureInfo.InvariantCulture, message, args);
    }
}
