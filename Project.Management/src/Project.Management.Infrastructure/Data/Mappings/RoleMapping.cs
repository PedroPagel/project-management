using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Management.Domain.Entities;

namespace Project.Management.Infrastructure.Data.Mappings
{
    public class RoleMapping : BaseMapping<Role>
    {
        public override void Mapping(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("TL_ROLE");

            builder.Property(d => d.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(r => r.Name)
                .HasColumnName("ds_name")
                .HasMaxLength(100)
                .IsRequired();
        }
    }

}
