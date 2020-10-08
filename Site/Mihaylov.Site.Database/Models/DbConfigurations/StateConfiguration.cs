using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mihaylov.Site.Database.Models.DbConfigurations
{
    public class StateConfiguration : LookupTableConfiguration<State>
    {
        public StateConfiguration()
            : base(50)
        {
        }

        public override void Configure(EntityTypeBuilder<State> builder)
        {
            base.Configure(builder);

            builder.Property(s => s.StateCode).HasMaxLength(2);
            builder.HasOne(t => t.Country).WithMany().HasForeignKey(t => t.CountryId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
