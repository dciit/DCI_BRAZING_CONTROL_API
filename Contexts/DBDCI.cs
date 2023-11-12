using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using BrazingControlAPI.Models;

namespace BrazingControlAPI.Contexts
{
    public partial class DBDCI : DbContext
    {
        public DBDCI()
        {
        }

        public DBDCI(DbContextOptions<DBDCI> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<TrCourse> TrCourses { get; set; } = null!;
        public virtual DbSet<TrSchedule> TrSchedules { get; set; } = null!;
        public virtual DbSet<TrTraineeDatum> TrTraineeData { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=192.168.226.145;Database=dbDci;TrustServerCertificate=True;uid=sa;password=decjapan");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Thai_CI_AS");

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("Employee");

                entity.HasIndex(e => e.Resign, "IX_Employee");

                entity.HasIndex(e => new { e.Costcenter, e.Resign }, "IX_Employee_1");

                entity.HasIndex(e => new { e.LineCode, e.Dvcd, e.Resign }, "IX_Employee_2");

                entity.HasIndex(e => new { e.Resign, e.Posit, e.Costcenter, e.Budgettype, e.LineCode, e.Dvcd }, "IX_Employee_3");

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .HasColumnName("CODE");

                entity.Property(e => e.Andon)
                    .HasMaxLength(500)
                    .HasColumnName("ANDON");

                entity.Property(e => e.AnnualcalDt)
                    .HasColumnType("date")
                    .HasColumnName("ANNUALCAL_DT");

                entity.Property(e => e.Birth)
                    .HasColumnType("date")
                    .HasColumnName("BIRTH");

                entity.Property(e => e.Budgettype)
                    .HasMaxLength(10)
                    .HasColumnName("BUDGETTYPE");

                entity.Property(e => e.Bus)
                    .HasMaxLength(50)
                    .HasColumnName("BUS");

                entity.Property(e => e.Company)
                    .HasMaxLength(50)
                    .HasColumnName("COMPANY");

                entity.Property(e => e.Costcenter)
                    .HasMaxLength(50)
                    .HasColumnName("COSTCENTER");

                entity.Property(e => e.Dlrate)
                    .HasMaxLength(50)
                    .HasColumnName("DLRATE");

                entity.Property(e => e.Dvcd)
                    .HasMaxLength(10)
                    .HasColumnName("DVCD");

                entity.Property(e => e.Grpot)
                    .HasMaxLength(50)
                    .HasColumnName("GRPOT");

                entity.Property(e => e.Join)
                    .HasColumnType("date")
                    .HasColumnName("JOIN");

                entity.Property(e => e.LineCode).HasMaxLength(100);

                entity.Property(e => e.Location)
                    .HasMaxLength(500)
                    .HasColumnName("LOCATION");

                entity.Property(e => e.Mail)
                    .HasMaxLength(50)
                    .HasColumnName("MAIL");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .HasColumnName("NAME");

                entity.Property(e => e.Nickname)
                    .HasMaxLength(50)
                    .HasColumnName("NICKNAME");

                entity.Property(e => e.Otype)
                    .HasMaxLength(50)
                    .HasColumnName("OTYPE");

                entity.Property(e => e.PGrade)
                    .HasMaxLength(10)
                    .HasColumnName("P_GRADE");

                entity.Property(e => e.PRank)
                    .HasMaxLength(10)
                    .HasColumnName("P_RANK");

                entity.Property(e => e.Posit)
                    .HasMaxLength(50)
                    .HasColumnName("POSIT");

                entity.Property(e => e.Pren)
                    .HasMaxLength(50)
                    .HasColumnName("PREN");

                entity.Property(e => e.Religion)
                    .HasMaxLength(50)
                    .HasColumnName("RELIGION");

                entity.Property(e => e.Resign)
                    .HasColumnType("date")
                    .HasColumnName("RESIGN");

                entity.Property(e => e.Rstype)
                    .HasMaxLength(50)
                    .HasColumnName("RSTYPE");

                entity.Property(e => e.Sex)
                    .HasMaxLength(20)
                    .HasColumnName("SEX");

                entity.Property(e => e.Stop)
                    .HasMaxLength(50)
                    .HasColumnName("STOP");

                entity.Property(e => e.Surn)
                    .HasMaxLength(200)
                    .HasColumnName("SURN");

                entity.Property(e => e.Tcaddr3)
                    .HasMaxLength(50)
                    .HasColumnName("TCADDR3");

                entity.Property(e => e.Tcaddr4)
                    .HasMaxLength(50)
                    .HasColumnName("TCADDR4");

                entity.Property(e => e.Tctel)
                    .HasMaxLength(50)
                    .HasColumnName("TCTEL");

                entity.Property(e => e.Telephone)
                    .HasMaxLength(50)
                    .HasColumnName("TELEPHONE");

                entity.Property(e => e.Tname)
                    .HasMaxLength(200)
                    .HasColumnName("TNAME");

                entity.Property(e => e.TposijoinDt)
                    .HasColumnType("date")
                    .HasColumnName("TPOSIJOIN_DT");

                entity.Property(e => e.Tposiname)
                    .HasMaxLength(150)
                    .HasColumnName("TPOSINAME");

                entity.Property(e => e.Tpren)
                    .HasMaxLength(20)
                    .HasColumnName("TPREN");

                entity.Property(e => e.Tsex)
                    .HasMaxLength(20)
                    .HasColumnName("TSEX");

                entity.Property(e => e.Tsurn)
                    .HasMaxLength(200)
                    .HasColumnName("TSURN");

                entity.Property(e => e.Workcenter)
                    .HasMaxLength(10)
                    .HasColumnName("WORKCENTER");

                entity.Property(e => e.Wsts)
                    .HasMaxLength(50)
                    .HasColumnName("WSTS");

                entity.Property(e => e.Wtype)
                    .HasMaxLength(50)
                    .HasColumnName("WTYPE");
            });

            modelBuilder.Entity<TrCourse>(entity =>
            {
                entity.ToTable("TR_COURSE");

                entity.HasIndex(e => new { e.CourseCode, e.CourseStatus }, "IX_TR_COURSE");

                entity.HasIndex(e => new { e.CourseName, e.CourseStatus }, "IX_TR_COURSE_1");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cby)
                    .HasMaxLength(50)
                    .HasColumnName("CBY");

                entity.Property(e => e.Cdate)
                    .HasColumnType("datetime")
                    .HasColumnName("CDATE");

                entity.Property(e => e.CourseCode)
                    .HasMaxLength(50)
                    .HasColumnName("COURSE_CODE");

                entity.Property(e => e.CourseExamSet)
                    .HasMaxLength(50)
                    .HasColumnName("COURSE_EXAM_SET");

                entity.Property(e => e.CourseName)
                    .HasMaxLength(250)
                    .HasColumnName("COURSE_NAME");

                entity.Property(e => e.CourseNameEn)
                    .HasMaxLength(250)
                    .HasColumnName("COURSE_NAME_EN");

                entity.Property(e => e.CoursePerPerson)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("COURSE_PER_PERSON");

                entity.Property(e => e.CourseStatus)
                    .HasMaxLength(50)
                    .HasColumnName("COURSE_STATUS");

                entity.Property(e => e.CourseType)
                    .HasMaxLength(10)
                    .HasColumnName("COURSE_TYPE");

                entity.Property(e => e.CourseVdoPath)
                    .HasMaxLength(500)
                    .HasColumnName("COURSE_VDO_PATH");

                entity.Property(e => e.ExpireStatus)
                    .HasColumnName("EXPIRE_STATUS")
                    .HasDefaultValueSql("('FALSE')");

                entity.Property(e => e.Group)
                    .HasMaxLength(50)
                    .HasColumnName("GROUP");

                entity.Property(e => e.Line).HasMaxLength(50);

                entity.Property(e => e.Uby)
                    .HasMaxLength(50)
                    .HasColumnName("UBY");

                entity.Property(e => e.Udate)
                    .HasColumnType("datetime")
                    .HasColumnName("UDATE");
            });

            modelBuilder.Entity<TrSchedule>(entity =>
            {
                entity.HasKey(e => e.ScheduleCode);

                entity.ToTable("TR_Schedule");

                entity.HasIndex(e => new { e.ScheduleStart, e.ScheduleEnd }, "IX_TR_Schedule");

                entity.Property(e => e.ScheduleCode).HasColumnName("SCHEDULE_CODE");

                entity.Property(e => e.Cby)
                    .HasMaxLength(50)
                    .HasColumnName("CBY");

                entity.Property(e => e.Cdate)
                    .HasColumnType("datetime")
                    .HasColumnName("CDATE");

                entity.Property(e => e.Company)
                    .HasMaxLength(200)
                    .HasColumnName("COMPANY");

                entity.Property(e => e.CourseExamSet)
                    .HasMaxLength(50)
                    .HasColumnName("COURSE_EXAM_SET");

                entity.Property(e => e.CourseId)
                    .HasMaxLength(50)
                    .HasColumnName("COURSE_ID");

                entity.Property(e => e.FileName).HasMaxLength(200);

                entity.Property(e => e.Indicator).HasMaxLength(50);

                entity.Property(e => e.InstructionMedia).HasMaxLength(50);

                entity.Property(e => e.Location)
                    .HasMaxLength(500)
                    .HasColumnName("LOCATION");

                entity.Property(e => e.LocationType)
                    .HasMaxLength(50)
                    .HasColumnName("LOCATION_TYPE");

                entity.Property(e => e.Mark).HasColumnName("MARK");

                entity.Property(e => e.ScheduleDetail)
                    .HasMaxLength(500)
                    .HasColumnName("SCHEDULE_Detail");

                entity.Property(e => e.ScheduleEnd)
                    .HasColumnType("datetime")
                    .HasColumnName("SCHEDULE_END");

                entity.Property(e => e.ScheduleStart)
                    .HasColumnType("datetime")
                    .HasColumnName("SCHEDULE_START");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("STATUS");

                entity.Property(e => e.TrainDay)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("TRAIN_DAY")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Trainer)
                    .HasMaxLength(200)
                    .HasColumnName("TRAINER");

                entity.Property(e => e.TrainerCompany).HasMaxLength(200);

                entity.Property(e => e.TrainerType).HasMaxLength(50);

                entity.Property(e => e.Uby)
                    .HasMaxLength(50)
                    .HasColumnName("UBY");

                entity.Property(e => e.Udate)
                    .HasColumnType("datetime")
                    .HasColumnName("UDATE");
            });

            modelBuilder.Entity<TrTraineeDatum>(entity =>
            {
                entity.HasKey(e => e.ScheduleDetCode)
                    .HasName("PK_TR_Trainee_Data_bkup_230918");

                entity.ToTable("TR_Trainee_Data");

                entity.Property(e => e.ScheduleDetCode).HasColumnName("SCHEDULE_DET_CODE");

                entity.Property(e => e.Cby)
                    .HasMaxLength(50)
                    .HasColumnName("CBY");

                entity.Property(e => e.Cdate)
                    .HasColumnType("datetime")
                    .HasColumnName("CDATE");

                entity.Property(e => e.CourseCode)
                    .HasMaxLength(50)
                    .HasColumnName("COURSE_CODE");

                entity.Property(e => e.Empcode)
                    .HasMaxLength(10)
                    .HasColumnName("EMPCODE")
                    .IsFixedLength();

                entity.Property(e => e.EvaluateResult)
                    .HasMaxLength(50)
                    .HasColumnName("EVALUATE_RESULT");

                entity.Property(e => e.ExamSetCode)
                    .HasMaxLength(50)
                    .HasColumnName("EXAM_SET_CODE");

                entity.Property(e => e.Expire).HasColumnType("datetime");

                entity.Property(e => e.PostTestResult)
                    .HasMaxLength(50)
                    .HasColumnName("POST_TEST_RESULT");

                entity.Property(e => e.PreTestResult)
                    .HasMaxLength(50)
                    .HasColumnName("PRE_TEST_RESULT");

                entity.Property(e => e.ScheduleCode)
                    .HasMaxLength(50)
                    .HasColumnName("SCHEDULE_CODE");

                entity.Property(e => e.Uby)
                    .HasMaxLength(50)
                    .HasColumnName("UBY");

                entity.Property(e => e.Udate)
                    .HasColumnType("datetime")
                    .HasColumnName("UDATE");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
