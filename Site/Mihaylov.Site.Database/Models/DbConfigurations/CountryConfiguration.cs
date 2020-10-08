using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mihaylov.Site.Database.Models.DbConfigurations
{
    public class CountryConfiguration : LookupTableConfiguration<Country>
    {
        public CountryConfiguration()
            : base(50)
        {
        }

        public override void Configure(EntityTypeBuilder<Country> builder)
        {
            base.Configure(builder);

            builder.Property(s => s.CountryCode).HasMaxLength(2);
        }
    }
}
