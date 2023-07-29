namespace Acme.DrawLanding.Library.Domain.SerialNumbers;

public sealed class SerialNumberRecord
{
    public int Id { get; set; }

    public Guid Content { get; set; }

    public int Uses { get; set; }
}
