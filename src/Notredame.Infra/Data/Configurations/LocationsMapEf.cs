using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Notredame.Domain;

namespace Notredame.Infra.Data.Configurations;

public class LocationsMapEf : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ExternalId)
            .UseAutoincrement();
        
        builder.Property(x => x.ExternalId)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Lat)
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(x => x.Lon)
            .HasDefaultValue(0)
            .IsRequired();
        
        builder.Property(x=> x.CreatedAt)
            .ValueGeneratedOnAdd();
        
        builder.Property(x=> x.ModifiedAt)
            .ValueGeneratedOnUpdate();

        builder.Metadata.FindNavigation(nameof(Cep))
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}