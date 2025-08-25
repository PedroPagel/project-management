using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Project.Management.Infrastructure.Data.Mappings
{
    public class ProjectMapping : BaseMapping<Domain.Entities.Project>
    {
        public override void Mapping(EntityTypeBuilder<Domain.Entities.Project> builder)
        {
            builder.ToTable("TL_PROJECT");

            builder.Property(d => d.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(p => p.Name)
                .HasColumnName("ds_name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasColumnName("ds_description")
                .HasMaxLength(500);

            builder.Property(p => p.Status)
                .HasColumnName("ds_status")
                .HasConversion<string>()
                .IsRequired();

            builder.Property(p => p.StartDate)
                .HasColumnName("dt_start");

            builder.Property(p => p.EndDate)
                .HasColumnName("dt_end");
        }
    }

}
