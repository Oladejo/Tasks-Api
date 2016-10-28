using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace TaskWebApi.Services.Models.Mapping
{
    public class TaskUserMap : EntityTypeConfiguration<TaskUser>
    {
        public TaskUserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.UserId)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("TaskUser");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.TaskId).HasColumnName("TaskId");

            // Relationships
            this.HasRequired(t => t.AspNetUser)
                .WithMany(t => t.TaskUsers)
                .HasForeignKey(d => d.UserId);
            this.HasRequired(t => t.Task)
                .WithMany(t => t.TaskUsers)
                .HasForeignKey(d => d.TaskId);

        }
    }
}
