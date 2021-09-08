using Compass.Security.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compass.Security.Infrastructure.Persistences.Configurations
{
    public class ApplicationConfiguration : IEntityTypeConfiguration<Domain.Entities.Application>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Application> builder)
        {
            builder.ToTable("applications");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(x => x.ClientId)
                .HasColumnName("client_id")
                .IsRequired(false);

            builder.Property(x => x.ClientSecret)
                .HasColumnName("client_secret")
                .IsRequired(false);

            builder.Property(x => x.ConcurrencyToken)
                .HasColumnName("concurrency_token")
                .IsRequired(false);

            builder.Property(x => x.ConsentType)
                .HasColumnName("consent_type")
                .IsRequired(false);

            builder.Property(x => x.DisplayName)
                .HasColumnName("display_name")
                .IsRequired(false);

            builder.Property(x => x.DisplayNames)
                .HasColumnName("display_names")
                .IsRequired(false);

            builder.Property(x => x.Permissions)
                .HasColumnName("permissions")
                .IsRequired(false);

            builder.Property(x => x.PostLogoutRedirectUris)
                .HasColumnName("post_logout_redirect_uris")
                .IsRequired(false);

            builder.Property(x => x.Properties)
                .HasColumnName("properties")
                .IsRequired(false);

            builder.Property(x => x.RedirectUris)
                .HasColumnName("redirect_uris")
                .IsRequired(false);

            builder.Property(x => x.Requirements)
                .HasColumnName("requirements")
                .IsRequired(false);

            builder.Property(x => x.Type)
                .HasColumnName("type")
                .IsRequired(false);
        }
    }
}