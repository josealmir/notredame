namespace Notredame.Domain.DTOs;

public record LocationDTO
{
    public double Lat { get; set; }
    
    public double Lon { get; set; }
}