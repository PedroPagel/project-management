using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Management.Domain.Entities;

namespace Project.Management.Infrastructure.Data.Mappings
{
    public abstract class BaseMapping<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : Entity
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(i => i.CreatedDate)
                .HasColumnName("dh_created")
                .IsRequired();

            builder.Property(i => i.UpdatedDate)
                .HasColumnName("dh_updated");

            builder.Property(i => i.UserUpdated)
                .HasColumnName("ds_id_user_updated")
                .HasColumnType("varchar(256)")
                .IsRequired();
        }

        public abstract void Mapping(EntityTypeBuilder<TEntity> builder);
    }
}
