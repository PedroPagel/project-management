using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Management.Domain.Entities;

namespace Project.Management.Infrastructure.Data.Mappings
{
    public class ProjectMemberMapping : IEntityTypeConfiguration<ProjectMember>
    {
        public void Configure(EntityTypeBuilder<ProjectMember> builder)
        {
            builder.ToTable("TL_PROJECT_MEMBER");

            builder.Property(d => d.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.HasKey(pm => new { pm.UserId, pm.ProjectId });

            builder.HasOne(pm => pm.User)
                .WithMany(u => u.ProjectMemberships)
                .HasForeignKey(pm => pm.UserId);

            builder.HasOne(pm => pm.Project)
                .WithMany(p => p.Members)
                .HasForeignKey(pm => pm.ProjectId);

            builder.HasOne(pm => pm.Role)
                .WithMany(r => r.ProjectMembers)
                .HasForeignKey(pm => pm.RoleId);

            builder.Property(pm => pm.Active)
               .HasColumnName("bl_no_sales")
               .IsRequired();
        }
    }

}
