using Compass.Security.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compass.Security.Infrastructure.Persistences.Configurations
{
    public class ApplicationAuthorizationConfiguration : IEntityTypeConfiguration<ApplicationAuthorization>
    {
        public void Configure(EntityTypeBuilder<ApplicationAuthorization> builder)
        {
            builder.ToTable("application_authorizations");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(x => x.ApplicationId)
                .HasColumnName("application_id")
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(x => x.ConcurrencyToken)
                .HasColumnName("concurrency_token")
                .IsRequired(false);

            builder.Property(x => x.CreationDate)
                .HasColumnName("creation_date")
                .IsRequired(false);

            builder.Property(x => x.Properties)
                .HasColumnName("properties")
                .IsRequired(false);

            builder.Property(x => x.Scopes)
                .HasColumnName("scopes")
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