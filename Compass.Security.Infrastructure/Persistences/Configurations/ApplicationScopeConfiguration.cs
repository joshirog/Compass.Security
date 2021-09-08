using Compass.Security.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compass.Security.Infrastructure.Persistences.Configurations
{
    public class ApplicationScopeConfiguration : IEntityTypeConfiguration<ApplicationScope>
    {
        public void Configure(EntityTypeBuilder<ApplicationScope> builder)
        {
            builder.ToTable("application_scopes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(x => x.ConcurrencyToken)
                .HasColumnName("concurrency_token")
                .IsRequired(false);

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .IsRequired(false);

            builder.Property(x => x.Descriptions)
                .HasColumnName("descriptions")
                .IsRequired(false);

            builder.Property(x => x.DisplayName)
                .HasColumnName("display_name")
                .IsRequired(false);

            builder.Property(x => x.DisplayNames)
                .HasColumnName("display_names")
                .IsRequired(false);

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired(false);

            builder.Property(x => x.Properties)
                .HasColumnName("properties")
                .IsRequired(false);

            builder.Property(x => x.Resources)
                .HasColumnName("resources")
                .IsRequired(false);
        }
    }
}