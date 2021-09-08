using Compass.Security.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compass.Security.Infrastructure.Persistences.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(x => x.UserName)
                .HasColumnName("username")
                .IsRequired();

            builder.Property(x => x.NormalizedUserName)
                .HasColumnName("normalized_username")
                .IsRequired();

            builder.Property(x => x.Email)
                .HasColumnName("email")
                .IsRequired(false);

            builder.Property(x => x.NormalizedEmail)
                .HasColumnName("normalized_email")
                .IsRequired(false);

            builder.Property(x => x.EmailConfirmed)
                .HasColumnName("email_confirmed")
                .IsRequired();

            builder.Property(x => x.PasswordHash)
                .HasColumnName("password_hash")
                .IsRequired(false);

            builder.Property(x => x.SecurityStamp)
                .HasColumnName("security_stamp")
                .IsRequired(false);

            builder.Property(x => x.ConcurrencyStamp)
                .HasColumnName("concurrency_stamp")
                .IsRequired(false);

            builder.Property(x => x.PhoneNumber)
                .HasColumnName("phone_number")
                .IsRequired(false);

            builder.Property(x => x.PhoneNumberConfirmed)
                .HasColumnName("phone_number_confirmed")
                .IsRequired();

            builder.Property(x => x.TwoFactorEnabled)
                .HasColumnName("two_factor_enabled")
                .IsRequired();

            builder.Property(x => x.LockoutEnd)
                .HasColumnName("lockout_end")
                .IsRequired(false);

            builder.Property(x => x.LockoutEnabled)
                .HasColumnName("lockout_enabled")
                .IsRequired();

            builder.Property(x => x.AccessFailedCount)
                .HasColumnName("access_failed_count")
                .IsRequired();

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(x => x.CreatedBy)
                .HasColumnName("created_by")
                .HasMaxLength(120)
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
               .HasColumnName("updated_at")
               .IsRequired(false);

            builder.Property(x => x.UpdatedBy)
                .HasColumnName("updated_by")
                .HasMaxLength(120)
                .IsRequired(false);
        }
    }
}