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
            
            builder.Property(x => x.SmsCounter)
                .HasColumnName("sms_counter")
                .IsRequired()
                .HasMaxLength(10);
            
            builder.Property(x => x.EmailCounter)
                .HasColumnName("email_counter")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();
            
            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);
        }
    }
}