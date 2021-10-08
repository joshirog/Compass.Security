using Compass.Security.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compass.Security.Infrastructure.Persistences.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("user_roles");

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(x => x.RoleId)
                .HasColumnName("role_id")
                .IsRequired();
        }
    }
}