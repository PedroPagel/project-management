using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Management.Domain.Entities;

namespace Project.Management.Infrastructure.Data.Mappings
{
    public class UserMapping : BaseMapping<User>
    {
        public override void Mapping(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("TL_USER");

            builder.Property(d => d.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(u => u.FullName)
                .HasColumnName("ds_full_name")
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(u => u.Email)
                .HasColumnName("ds_email")
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(u => u.PasswordHash)
                .HasColumnName("ds_password_hash")
                .IsRequired();
        }
    }
}
