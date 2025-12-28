using Notredame.Domain.DTOs;

namespace Notredame.Infra.Services.Response;

public record LocationResponse
{
    public Coordinates Coordinates { get; set; } = new();
};

public record Coordinates
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}