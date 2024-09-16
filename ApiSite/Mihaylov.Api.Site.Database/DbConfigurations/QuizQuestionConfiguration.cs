using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Site.Database.Models;
using DTO = Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Database.DbConfigurations
{
    internal class QuizQuestionConfiguration : IEntityTypeConfiguration<QuizQuestion>
    {
        public void Configure(EntityTypeBuilder<QuizQuestion> builder)
        {
            builder.HasKey(b => b.QuestionId).HasName("PK_QuestionId");

            builder.Property(t => t.QuestionId).ValueGeneratedOnAdd().IsRequired();
            builder.Property(c => c.Value).IsRequired().HasMaxLength(DTO.QuizQuestion.ValueMaxLength);
        }
    }
}
