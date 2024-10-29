using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Dictionary.Database.Models;

namespace Mihaylov.Api.Dictionary.Database.DbConfigurations
{
    internal class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(t => t.CourseId).HasName("PK_Courses_CourseId");
            builder.Property(t => t.CourseId).ValueGeneratedOnAdd().IsRequired();

            builder.Property(e => e.StartDate).IsRequired(false).HasColumnType("Date");
            builder.Property(e => e.EndDate).IsRequired(false).HasColumnType("Date");
            builder.Property(e => e.ModulesStartNumber).IsRequired(false);
            builder.Property(e => e.ModulesEndNumber).IsRequired(false);

            builder.Property(t => t.LevelId).IsRequired();
            builder.HasOne(t => t.Level).WithMany().IsRequired().HasForeignKey(c => c.LevelId).OnDelete(DeleteBehavior.NoAction);

            builder.Property(t => t.LearningSystemId).IsRequired();
            builder.HasOne(t => t.LearningSystem).WithMany().IsRequired().HasForeignKey(c => c.LearningSystemId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
