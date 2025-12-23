using System.ComponentModel.DataAnnotations;

namespace Notredame.Shared.Options;

public record BrasilCepOptionNotredame
{
    public const string SectionName = "BrasilCep";
    
    [Required(AllowEmptyStrings=false,ErrorMessage = $"{SectionName}__BaseUrl not specified")]
    public string BaseUrl { get; set; } = string.Empty;
}