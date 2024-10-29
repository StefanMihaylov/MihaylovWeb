using System;
using System.Collections.Generic;

namespace Mihaylov.Api.Dictionary.Database.Models
{
    public class _Test
    {
        public int TestId { get; set; }

        public int? PreviousTestId { get; set; }

        public string UserId { get; set; }

        public int CourseId { get; set; }

        public int? ModuleStartNumber { get; set; }

        public int? ModuleEndNumber { get; set; }

        public DateTime StartDate { get; set; }

        public int? CorrectAnswers { get; set; }

        public int? TotalRecords { get; set; }


        public Course Course { get; set; }

        public _Test PreviousTest { get; set; }


        public ICollection<_IncorrectAnswer> IncorrectAnswers { get; set; }

        public ICollection<_Test> InversePreviousTest { get; set; }


        public _Test()
        {
            IncorrectAnswers = new List<_IncorrectAnswer>();
            InversePreviousTest = new List<_Test>();
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<IncorrectAnswer>(entity =>
        //    {
        //        entity.HasKey(e => new { e.TestId, e.RecordId })
        //            .HasName("PK_IncorrectAnswers_1");

        //        entity.ToTable("IncorrectAnswers", "dict");

        //        entity.HasOne(d => d.Record)
        //            .WithMany(p => p.IncorrectAnswers)
        //            .HasForeignKey(d => d.RecordId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("FK_IncorrectAnswers_Records");

        //        entity.HasOne(d => d.Test)
        //            .WithMany(p => p.IncorrectAnswers)
        //            .HasForeignKey(d => d.TestId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("FK_IncorrectAnswers_Tests");
        //    });


        //    modelBuilder.Entity<Test>(entity =>
        //    {
        //        entity.HasKey(e => e.TestId);

        //        entity.ToTable("Tests", "dict");

        //        entity.Property(e => e.StartDate).HasColumnType("datetime");

        //        entity.Property(e => e.UserId)
        //            .IsRequired()
        //            .HasMaxLength(128);

        //        entity.HasOne(d => d.Course)
        //            .WithMany(p => p.Tests)
        //            .HasForeignKey(d => d.CourseId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("FK_Tests_Courses");

        //        entity.HasOne(d => d.PreviousTest)
        //            .WithMany(p => p.InversePreviousTest)
        //            .HasForeignKey(d => d.PreviousTestId)
        //            .HasConstraintName("FK_Tests_Tests");

        //        entity.HasOne(d => d.User)
        //            .WithMany(p => p.Tests)
        //            .HasForeignKey(d => d.UserId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("FK_Tests_AspNetUsers");
        //    });
    }
}
