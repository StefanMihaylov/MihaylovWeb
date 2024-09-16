using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Site.Database.Models;
using DTO = Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Database.DbConfigurations
{
    internal class QuizAnswerConfiguration : IEntityTypeConfiguration<QuizAnswer>
    {
        public void Configure(EntityTypeBuilder<QuizAnswer> builder)
        {
            builder.HasKey(b => b.QuizAnswerId).HasName("PK_QuizAnswers_QuizAnswerId");

            builder.Property(t => t.QuizAnswerId).ValueGeneratedOnAdd().IsRequired();
            builder.Property(c => c.Details).IsRequired().HasMaxLength(DTO.QuizAnswer.DetailsMaxLength);
            builder.Property(t => t.AskDate).IsRequired();
            builder.Property(t => t.Value).IsRequired(false).HasPrecision(18, 2);

            builder.Property(t => t.PersonId).IsRequired();
            builder.HasOne(t => t.Person).WithMany(p => p.Answers).IsRequired().HasForeignKey(t => t.PersonId).OnDelete(DeleteBehavior.NoAction);

            builder.Property(t => t.QuestionId).IsRequired();
            builder.HasOne(t => t.Question).WithMany().IsRequired().HasForeignKey(t => t.QuestionId).OnDelete(DeleteBehavior.NoAction);

            builder.Property(t => t.UnitId).IsRequired(false);
            builder.HasOne(t => t.Unit).WithMany().IsRequired(false).HasForeignKey(t => t.UnitId).OnDelete(DeleteBehavior.NoAction);

            builder.Property(t => t.HalfTypeId).IsRequired(false);
            builder.HasOne(t => t.HalfType).WithMany().IsRequired(false).HasForeignKey(t => t.HalfTypeId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
