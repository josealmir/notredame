using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Notredame.Domain;

namespace Notredame.Infra.Data.Configurations;

public class CepsMapEf : IEntityTypeConfiguration<Cep>
{
    public void Configure(EntityTypeBuilder<Cep> builder)
    {
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Id)
            .UseAutoincrement();
        
        builder.Property(c => c.ExternalId)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.ZipCode);

        builder.Property(c => c.City);

        builder.Property(c => c.State);

        builder.Property(c => c.Ibge);
        
        builder.Property(c => c.Provider)
            .HasConversion<string>();

        builder.Property(x=> x.CreatedAt)
            .ValueGeneratedOnAdd();
        
        builder.Property(x=> x.ModifiedAt)
            .ValueGeneratedOnUpdate();  
        
        builder.HasOne(x=>x.Location)
            .WithOne()
            .HasForeignKey<Location>(x=>x.CepId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}