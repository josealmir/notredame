using System.ComponentModel.DataAnnotations;

namespace Notredame.Shared.Options;

public class ViaCepOptionNotredame
{
    public const string SectionName = "ViaCep";
    
    [Required(AllowEmptyStrings=false,ErrorMessage = $"{SectionName}__BaseUrl not specified")]
    public string BaseUrl { get; set; } = string.Empty;
}