using Compass.Security.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compass.Security.Infrastructure.Persistences.Configurations
{
    public class ApplicationTokenConfiguration : IEntityTypeConfiguration<ApplicationToken>
    {
        public void Configure(EntityTypeBuilder<ApplicationToken> builder)
        {
            builder.ToTable("application_tokens");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(x => x.ApplicationId)
                .HasColumnName("application_id")
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(x => x.AuthorizationId)
                .HasColumnName("authorization_id")
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(x => x.ConcurrencyToken)
                .HasColumnName("concurrency_token")
                .IsRequired(false);

            builder.Property(x => x.CreationDate)
                .HasColumnName("creation_date")
                .IsRequired(false);

            builder.Property(x => x.ExpirationDate)
                .HasColumnName("expiration_date")
                .IsRequired(false);

            builder.Property(x => x.Payload)
                .HasColumnName("payload")
                .IsRequired(false);

            builder.Property(x => x.Properties)
                .HasColumnName("properties")
                .IsRequired(false);

            builder.Property(x => x.RedemptionDate)
                .HasColumnName("redemption_date")
                .IsRequired(false);

            builder.Property(x => x.ReferenceId)
                .HasColumnName("reference_id")
                .IsRequired(false);

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .IsRequired(false);

            builder.Property(x => x.Subject)
                .HasColumnName("subject")
                .IsRequired(false);

            builder.Property(x => x.Type)
                .HasColumnName("type")
                .IsRequired(false);
        }
    }
}