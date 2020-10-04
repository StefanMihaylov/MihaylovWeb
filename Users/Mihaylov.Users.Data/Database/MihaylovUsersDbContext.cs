using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Common.Databases.Interfaces;
using Mihaylov.Users.Data.Database.Models;
using Mihaylov.Users.Models.Enums;

namespace Mihaylov.Users.Data.Database
{
    public class MihaylovUsersDbContext : IdentityDbContext<User>
    {
        private readonly IAuditService _auditService;

        public MihaylovUsersDbContext(DbContextOptions<MihaylovUsersDbContext> options,
            IAuditService auditService)
            : base(options)
        {
            this._auditService = auditService;
        }

        public DbSet<Gender> Genders { get; set; }


        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAuditInformation();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            this.ApplyAuditInformation();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                   .OwnsOne<UserProfile>(u => u.Profile, up =>
                   {
                       up.Property(a => a.FirstName).HasColumnName(nameof(UserProfile.FirstName)).HasMaxLength(25);
                       up.Property(a => a.LastName).HasColumnName(nameof(UserProfile.LastName)).HasMaxLength(25);
                       up.Property(a => a.Gender).HasColumnName(nameof(UserProfile.Gender)).HasConversion<int>();
                       up.HasOne(u => u.GenderModel).WithMany().HasForeignKey(a => a.Gender);
                   });

            builder
                .Entity<Gender>(g =>
                {
                    g.HasKey(e => e.Id);
                    g.Property(e => e.Id).HasConversion<int>();
                    g.Property(e => e.Name).HasMaxLength(25).IsRequired();
                    g.HasData(Enum.GetValues(typeof(GenderType)).Cast<GenderType>()
                                  .Select(e => new Gender() { Id = e, Name = e.ToString() }));
                });

            base.OnModelCreating(builder);
        }

        private void ApplyAuditInformation()
        {
            var entities = this.ChangeTracker.Entries().ToList();

            this._auditService.ApplyAuditInformation(entities);
        }
    }
}
