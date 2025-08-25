using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Management.Domain.Entities;

namespace Project.Management.Infrastructure.Data.Mappings
{
    public class TaskItemMapping : BaseMapping<TaskItem>
    {
        public override void Mapping(EntityTypeBuilder<TaskItem> builder)
        {
            builder.ToTable("TL_TASK");

            builder.Property(d => d.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(t => t.Title)
                .HasColumnName("ds_title")
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(t => t.Description)
                .HasColumnName("ds_description");

            builder.Property(t => t.Status)
                .HasColumnName("ds_status")
                .HasConversion<string>();

            builder.Property(t => t.DueDate)
                .HasColumnName("dt_due");

            builder.HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId);

            builder.HasOne(t => t.AssignedUser)
                .WithMany()
                .HasForeignKey(t => t.AssignedUserId);
        }
    }

}
