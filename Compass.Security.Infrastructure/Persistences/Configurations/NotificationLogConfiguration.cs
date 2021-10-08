using Compass.Security.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compass.Security.Infrastructure.Persistences.Configurations
{
    public class NotificationLogConfiguration : IEntityTypeConfiguration<NotificationLog>
    {
        public void Configure(EntityTypeBuilder<NotificationLog> builder)
        {
            builder.ToTable("notification_logs");

            builder.Ignore(x => x.Status);
            builder.Ignore(x => x.CreatedBy);
            builder.Ignore(x => x.UpdatedBy);
            builder.Ignore(x => x.UpdatedAt);
            
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
                .IsRequired()
                .HasMaxLength(10);
            
            builder.Property(x => x.Identifier)
                .HasColumnName("identifier")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();
        }
    }
}