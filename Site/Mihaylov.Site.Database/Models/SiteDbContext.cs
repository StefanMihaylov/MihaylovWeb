using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Mihaylov.Site.Database.Models
{
    public partial class SiteDbContext : DbContext
    {
        public SiteDbContext()
        {
        }

        public SiteDbContext(DbContextOptions<SiteDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AnswerType> AnswerTypes { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<EthnicityType> EthnicityTypes { get; set; }
        public virtual DbSet<OrientationType> OrientationTypes { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<Phrase> Phrases { get; set; }
        public virtual DbSet<UnitType> UnitTypes { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=.;Database=MihaylovDb_copy;Trusted_Connection=True;");
//            }
//        }       
    }
}
