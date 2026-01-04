using Notredame.Domain.Commons;
using Notredame.Domain.DTOs;

namespace Notredame.Domain;

public class Location : Entity
{
    public double Lat { get; set; }
    
    public double Lon { get; set; }

    public LocationDTO Map() => new LocationDTO() { Lat = Lat, Lon = Lon, };
}