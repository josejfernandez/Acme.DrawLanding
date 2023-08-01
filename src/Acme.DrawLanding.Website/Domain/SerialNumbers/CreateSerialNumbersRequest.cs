using System.ComponentModel.DataAnnotations;

namespace Acme.DrawLanding.Website.Domain.SerialNumbers;

public sealed class CreateSerialNumbersRequest
{
    [Required]
    [Range(1, 100)]
    public int HowMany { get; set; }
}
