using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Dictionary.Database.Models;
using DTO = Mihaylov.Api.Dictionary.Contracts.Models;

namespace Mihaylov.Api.Dictionary.Database.DbConfigurations
{
    internal class RecordConfiguration : IEntityTypeConfiguration<Record>
    {
        public void Configure(EntityTypeBuilder<Record> builder)
        {
            builder.HasKey(t => t.RecordId).HasName("PK_Records_RecordId");
            builder.Property(t => t.RecordId).ValueGeneratedOnAdd().IsRequired();

            builder.Property(e => e.ModuleNumber).IsRequired(false);
            builder.Property(e => e.Original).IsRequired().HasMaxLength(DTO.Record.OriginalMaxLength);
            builder.Property(e => e.Translation).IsRequired(false).HasMaxLength(DTO.Record.TranslationMaxLength);
            builder.Property(e => e.Comment).IsRequired(false).HasMaxLength(DTO.Record.CommentMaxLength);

            builder.Property(t => t.CourseId).IsRequired();
            builder.HasOne(t => t.Course).WithMany().IsRequired().HasForeignKey(c => c.CourseId).OnDelete(DeleteBehavior.NoAction);

            builder.Property(t => t.RecordTypeId).IsRequired();
            builder.HasOne(t => t.RecordType).WithMany().IsRequired().HasForeignKey(c => c.RecordTypeId).OnDelete(DeleteBehavior.NoAction);

            builder.Property(t => t.PrepositionId).IsRequired(false);
            builder.HasOne(t => t.Preposition).WithMany().IsRequired(false).HasForeignKey(c => c.PrepositionId).OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(e => new { e.CourseId, e.Original }).IsUnique();
        }
    }
}
