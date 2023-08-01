namespace Acme.DrawLanding.Library.Domain.SerialNumbers;

public sealed class SerialNumber
{
    public Guid Number { get; set; }

    public int Uses { get; set; }
}
