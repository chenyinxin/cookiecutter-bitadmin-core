using System;
using {{cookiecutter.project_name}}.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace {{cookiecutter.project_name}}.Models
{
    public partial class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<FlowBills> FlowBills { get; set; }
        public virtual DbSet<FlowBillsRecord> FlowBillsRecord { get; set; }
        public virtual DbSet<FlowBillsRecordUser> FlowBillsRecordUser { get; set; }
        public virtual DbSet<FlowMain> FlowMain { get; set; }
        public virtual DbSet<FlowOrderCodes> FlowOrderCodes { get; set; }
        public virtual DbSet<FlowStep> FlowStep { get; set; }
        public virtual DbSet<FlowStepPath> FlowStepPath { get; set; }
        public virtual DbSet<GeneralExample> GeneralExample { get; set; }
        public virtual DbSet<SysAttachment> SysAttachment { get; set; }
        public virtual DbSet<SysDepartment> SysDepartment { get; set; }
        public virtual DbSet<SysDictionary> SysDictionary { get; set; }
        public virtual DbSet<SysLeader> SysLeader { get; set; }
        public virtual DbSet<SysLog> SysLog { get; set; }
        public virtual DbSet<SysModule> SysModule { get; set; }
        public virtual DbSet<SysModulePage> SysModulePage { get; set; }
        public virtual DbSet<SysOperation> SysOperation { get; set; }
        public virtual DbSet<SysPageOperation> SysPageOperation { get; set; }
        public virtual DbSet<SysRole> SysRole { get; set; }
        public virtual DbSet<SysRoleOperatePower> SysRoleOperatePower { get; set; }
        public virtual DbSet<SysRoleUser> SysRoleUser { get; set; }
        public virtual DbSet<SysServer> SysServer { get; set; }
        public virtual DbSet<SysSmsCode> SysSmsCode { get; set; }
        public virtual DbSet<SysUser> SysUser { get; set; }
        public virtual DbSet<SysUserClientId> SysUserClientId { get; set; }
        public virtual DbSet<SysUserOpenId> SysUserOpenId { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(SqlHelper.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FlowBills>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.BillsCode).HasMaxLength(255);

                entity.Property(e => e.BillsType).HasMaxLength(500);

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(1024);

                entity.Property(e => e.State).HasMaxLength(50);

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");

                entity.Property(e => e.WorkOrderCode).HasMaxLength(500);

                entity.Property(e => e.WorkOrderName).HasMaxLength(2000);
            });

            modelBuilder.Entity<FlowBillsRecord>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AuditDate).HasColumnType("datetime");

                entity.Property(e => e.Choice).HasMaxLength(4000);

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.State).HasMaxLength(4000);

                entity.HasOne(d => d.Bills)
                    .WithMany(p => p.FlowBillsRecord)
                    .HasForeignKey(d => d.BillsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FlowBillsRecord_FlowBills");
            });

            modelBuilder.Entity<FlowBillsRecordUser>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Choice).HasMaxLength(4000);

                entity.Property(e => e.Condition).HasMaxLength(4000);

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.DisplayState).HasMaxLength(4000);

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.Opinion).HasMaxLength(4000);

                entity.Property(e => e.RunTime).HasColumnType("datetime");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.State).HasMaxLength(4000);

                entity.HasOne(d => d.BillsRecord)
                    .WithMany(p => p.FlowBillsRecordUser)
                    .HasForeignKey(d => d.BillsRecordId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FlowBillsRecordUser_FlowBillsRecord");
            });

            modelBuilder.Entity<FlowMain>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(1024);

                entity.Property(e => e.Description).HasMaxLength(1024);

                entity.Property(e => e.Name).HasMaxLength(256);
            });

            modelBuilder.Entity<FlowOrderCodes>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.Day)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Pinyin)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<FlowStep>(entity =>
            {
                entity.HasKey(e => e.StepId);

                entity.Property(e => e.StepId).ValueGeneratedNever();

                entity.Property(e => e.Agency).HasMaxLength(32);

                entity.Property(e => e.AuditId).HasMaxLength(256);

                entity.Property(e => e.AuditIdRead).HasMaxLength(256);

                entity.Property(e => e.AuditNorm).HasMaxLength(128);

                entity.Property(e => e.AuditNormRead).HasMaxLength(128);

                entity.Property(e => e.Circularize).HasMaxLength(32);

                entity.Property(e => e.Description).HasMaxLength(4000);

                entity.Property(e => e.LinkCode).HasMaxLength(128);

                entity.Property(e => e.RunMode).HasMaxLength(4000);

                entity.Property(e => e.ShowTabIndex).HasMaxLength(4000);

                entity.Property(e => e.SmsTemplateRead).HasMaxLength(4000);

                entity.Property(e => e.SmsTemplateToDo).HasMaxLength(4000);

                entity.Property(e => e.StepName).HasMaxLength(64);

                entity.Property(e => e.Style).HasMaxLength(512);
            });

            modelBuilder.Entity<FlowStepPath>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(1024);

                entity.Property(e => e.Expression).HasMaxLength(512);

                entity.Property(e => e.Nikename).HasMaxLength(512);
            });

            modelBuilder.Entity<GeneralExample>(entity =>
            {
                entity.HasKey(e => e.ExampleId);

                entity.Property(e => e.ExampleId).ValueGeneratedNever();

                entity.Property(e => e.AutoComSelect).HasMaxLength(4000);

                entity.Property(e => e.AutoComSelectText).HasMaxLength(4000);

                entity.Property(e => e.AutoComplete).HasMaxLength(4000);

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.DateTimePicker).HasColumnType("datetime");

                entity.Property(e => e.DateTimePickerDate).HasColumnType("datetime");

                entity.Property(e => e.DateTimePickerFormatter).HasColumnType("datetime");

                entity.Property(e => e.DateTimePickerYymm)
                    .HasColumnName("DateTimePickerYYMM")
                    .HasColumnType("datetime");

                entity.Property(e => e.Department).HasMaxLength(4000);

                entity.Property(e => e.EmailText).HasMaxLength(64);

                entity.Property(e => e.ExampleCheckbox).HasMaxLength(256);

                entity.Property(e => e.ExampleName).HasMaxLength(64);

                entity.Property(e => e.ExamplePhone)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.ExampleRadio).HasMaxLength(256);

                entity.Property(e => e.ExampleSelect).HasMaxLength(256);

                entity.Property(e => e.ExampleText).HasMaxLength(256);

                entity.Property(e => e.ExampleTime).HasColumnType("datetime");

                entity.Property(e => e.ExampleUser).HasMaxLength(4000);

                entity.Property(e => e.LinkageSelectA).HasMaxLength(64);

                entity.Property(e => e.LinkageSelectB).HasMaxLength(64);

                entity.Property(e => e.LinkageSelectC).HasMaxLength(64);

                entity.Property(e => e.LinkageSelectD).HasMaxLength(64);

                entity.Property(e => e.LinkageSelectE).HasMaxLength(64);

                entity.Property(e => e.LinkageSelectF).HasMaxLength(64);

                entity.Property(e => e.MobileText).HasMaxLength(64);

                entity.Property(e => e.Oupicker)
                    .HasColumnName("OUPicker")
                    .HasMaxLength(64);

                entity.Property(e => e.PlainText).HasMaxLength(64);

                entity.Property(e => e.RepeatText).HasMaxLength(64);

                entity.Property(e => e.RequiredText).HasMaxLength(64);

                entity.Property(e => e.UserPicker).HasMaxLength(64);
            });

            modelBuilder.Entity<SysAttachment>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateBy).HasMaxLength(64);

                entity.Property(e => e.CreateByName).HasMaxLength(64);

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(512);

                entity.Property(e => e.RelationId)
                    .HasColumnName("RelationID")
                    .HasMaxLength(64);

                entity.Property(e => e.Suffix).HasMaxLength(32);
            });

            modelBuilder.Entity<SysDepartment>(entity =>
            {
                entity.HasKey(e => e.DepartmentId);

                entity.Property(e => e.DepartmentId).ValueGeneratedNever();

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.DepartmentCode).HasMaxLength(64);

                entity.Property(e => e.DepartmentFullName).HasMaxLength(512);

                entity.Property(e => e.DepartmentName).HasMaxLength(64);

                entity.Property(e => e.ExtendId).HasMaxLength(64);

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<SysDictionary>(entity =>
            {
                entity.HasKey(e => new { e.Type, e.Member })
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.Type).HasMaxLength(64);

                entity.Property(e => e.Member).HasMaxLength(64);

                entity.Property(e => e.Description).HasMaxLength(2048);

                entity.Property(e => e.MemberName)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<SysLeader>(entity =>
            {
                entity.HasKey(e => e.LeaderId);

                entity.Property(e => e.LeaderId).ValueGeneratedNever();

                entity.Property(e => e.DepartmentCode).HasMaxLength(32);

                entity.Property(e => e.Pos).HasMaxLength(32);

                entity.Property(e => e.Sequence).HasMaxLength(32);

                entity.Property(e => e.UserCode).HasMaxLength(32);
            });

            modelBuilder.Entity<SysLog>(entity =>
            {
                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.DepartmentName).HasMaxLength(128);

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.IpAddress)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.UserAgent)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.UserCode)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(64)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SysModule>(entity =>
            {
                entity.HasKey(e => e.ModuleId);

                entity.Property(e => e.ModuleId).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(2048);

                entity.Property(e => e.ModuleIcon).HasMaxLength(512);

                entity.Property(e => e.ModuleName).HasMaxLength(64);
            });

            modelBuilder.Entity<SysModulePage>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(2048);

                entity.Property(e => e.PageIcon).HasMaxLength(512);

                entity.Property(e => e.PageName)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.PageSign)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.PageUrl).HasMaxLength(512);
            });

            modelBuilder.Entity<SysOperation>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateBy).HasMaxLength(64);

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.OperationName).HasMaxLength(64);

                entity.Property(e => e.OperationSign).HasMaxLength(64);
            });

            modelBuilder.Entity<SysPageOperation>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.OperationSign)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.PageId).HasColumnName("PageID");
            });

            modelBuilder.Entity<SysRole>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.RoleName).HasMaxLength(64);
            });

            modelBuilder.Entity<SysRoleOperatePower>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.OperationSign).HasMaxLength(512);
            });

            modelBuilder.Entity<SysRoleUser>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<SysServer>(entity =>
            {
                entity.HasKey(e => e.ServerIp);

                entity.Property(e => e.ServerIp)
                    .HasColumnName("ServerIP")
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<SysSmsCode>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Mobile).HasMaxLength(32);

                entity.Property(e => e.OverTime).HasColumnType("datetime");

                entity.Property(e => e.SmsCode).HasMaxLength(32);

                entity.Property(e => e.SmsSign).HasMaxLength(32);

                entity.Property(e => e.VerifyTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<SysUser>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Birthday).HasColumnType("datetime");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(128);

                entity.Property(e => e.ExtendId).HasMaxLength(64);

                entity.Property(e => e.Gender).HasMaxLength(32);

                entity.Property(e => e.IdCard).HasMaxLength(32);

                entity.Property(e => e.Mobile).HasMaxLength(32);

                entity.Property(e => e.Post).HasMaxLength(32);

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");

                entity.Property(e => e.UserCode).HasMaxLength(32);

                entity.Property(e => e.UserName).HasMaxLength(32);

                entity.Property(e => e.UserPassword).HasMaxLength(128);

                entity.Property(e => e.UserStatus).HasMaxLength(32);
            });

            modelBuilder.Entity<SysUserClientId>(entity =>
            {
                entity.HasKey(e => e.ClientId);

                entity.Property(e => e.ClientId)
                    .HasMaxLength(64)
                    .ValueGeneratedNever();

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<SysUserOpenId>(entity =>
            {
                entity.HasKey(e => e.OpenId);

                entity.Property(e => e.OpenId)
                    .HasMaxLength(64)
                    .ValueGeneratedNever();

                entity.Property(e => e.BindTime).HasColumnType("datetime");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");
            });
        }
    }
}
