using System.ComponentModel.DataAnnotations;

namespace Notredame.Shared.Options;

public record NotredameApmOption
{
    public const string SectionName = "NotredameApm"; 
    
    [Required(AllowEmptyStrings=false,ErrorMessage = $"{SectionName}__TraceUri not specified")]
    public string TraceUri { get; set; } = string.Empty;
    
    [Required(AllowEmptyStrings=false,ErrorMessage = $"{SectionName}__LogUri not specified")]
    public string LogUri { get; set; } = string.Empty;
    
    [Required(AllowEmptyStrings=false,ErrorMessage = $"{SectionName}__MetricsUri not specified")]
    public string MetricsUri { get; set; } = string.Empty;
}