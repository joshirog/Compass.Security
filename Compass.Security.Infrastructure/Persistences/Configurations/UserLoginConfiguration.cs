using Compass.Security.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compass.Security.Infrastructure.Persistences.Configurations
{
    public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.ToTable("user_logins");

            //builder.HasNoKey();

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(x => x.LoginProvider)
                .HasColumnName("login_provider")
                .IsRequired();

            builder.Property(x => x.ProviderKey)
                .HasColumnName("provider_key")
                .IsRequired();

            builder.Property(x => x.ProviderDisplayName)
                .HasColumnName("provider_display_name")
                .IsRequired(false);
        }
    }
}