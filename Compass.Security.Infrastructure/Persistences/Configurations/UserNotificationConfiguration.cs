using Compass.Security.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compass.Security.Infrastructure.Persistences.Configurations
{
    public class UserNotificationConfiguration : IEntityTypeConfiguration<UserNotification>
    {
        public void Configure(EntityTypeBuilder<UserNotification> builder)
        {
            builder.ToTable("user_notifications");

            builder.Ignore(x => x.Status);
            builder.Ignore(x => x.CreatedBy);
            builder.Ignore(x => x.UpdatedBy);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasMaxLength(36)
                .IsRequired();
            
            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();
            
            builder.Property(x => x.Type)
                .HasColumnName("type")
                .IsRequired();
            
            builder.Property(x => x.Counter)
                .HasColumnName("counter")
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();
            
            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);
        }
    }
}