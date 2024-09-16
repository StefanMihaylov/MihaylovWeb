using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Site.Database.Models;
using DTO = Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Database.DbConfigurations
{
    internal class QuizPhraseConfiguration : IEntityTypeConfiguration<QuizPhrase>
    {
        public void Configure(EntityTypeBuilder<QuizPhrase> builder)
        {
            builder.HasKey(t => t.PhraseId);

            builder.Property(t => t.PhraseId).ValueGeneratedOnAdd().IsRequired();
            builder.Property(t => t.Text).IsRequired().HasMaxLength(DTO.QuizPhrase.TextMaxLength);
            builder.Property(t => t.OrderId).IsRequired();

            builder.HasIndex(t => t.OrderId);
        }    
    }
}
