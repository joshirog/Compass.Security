using Compass.Security.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compass.Security.Infrastructure.Persistences.Configurations
{
    public class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
    {
        public void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.ToTable("role_claims");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(x => x.RoleId)
                .HasColumnName("role_id")
                .IsRequired();

            builder.Property(x => x.ClaimType)
                .HasColumnName("claim_type")
                .IsRequired(false);

            builder.Property(x => x.ClaimValue)
                .HasColumnName("claim_value")
                .IsRequired(false);
        }
    }
}