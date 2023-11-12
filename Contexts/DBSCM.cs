using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using BrazingControlAPI.Models;

namespace BrazingControlAPI.Contexts
{
    public partial class DBSCM : DbContext
    {
        public DBSCM()
        {
        }

        public DBSCM(DbContextOptions<DBSCM> options)
            : base(options)
        {
        }

        public virtual DbSet<SkcDictMstr> SkcDictMstrs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=192.168.226.86;Database=dbSCM;TrustServerCertificate=True;uid=sa;password=decjapan");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Thai_CI_AS");

            modelBuilder.Entity<SkcDictMstr>(entity =>
            {
                entity.HasKey(e => e.DictId);

                entity.ToTable("SKC_DictMstr");

                entity.HasIndex(e => e.DictType, "IX_SKC_DictMstr");

                entity.HasIndex(e => new { e.DictType, e.Code }, "IX_SKC_DictMstr_1");

                entity.HasIndex(e => new { e.DictType, e.Code, e.RefCode }, "IX_SKC_DictMstr_2");

                entity.Property(e => e.DictId).HasColumnName("DICT_ID");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CODE");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATE_DATE");

                entity.Property(e => e.DictDesc)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DICT_DESC");

                entity.Property(e => e.DictStatus).HasColumnName("DICT_STATUS");

                entity.Property(e => e.DictType)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DICT_TYPE");

                entity.Property(e => e.Note)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOTE");

                entity.Property(e => e.RefCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("REF_CODE");

                entity.Property(e => e.RefItem)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("REF_ITEM");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("UPDATE_DATE");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
