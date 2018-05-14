using System;
using {{cookiecutter.project_name}}.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace {{cookiecutter.project_name}}.Models
{
    public partial class DataContext : DbContext
    {
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
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.BillsCode)
                    .HasColumnName("billsCode")
                    .HasMaxLength(255);

                entity.Property(e => e.BillsType)
                    .HasColumnName("billsType")
                    .HasMaxLength(500);

                entity.Property(e => e.CreateTime)
                    .HasColumnName("createTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(1000);

                entity.Property(e => e.MainId)
                    .HasColumnName("mainId")
                    .HasMaxLength(50);

                entity.Property(e => e.ParentId).HasColumnName("parentId");

                entity.Property(e => e.Sort).HasColumnName("sort");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasMaxLength(50);

                entity.Property(e => e.StepId)
                    .HasColumnName("stepId")
                    .HasMaxLength(50);

                entity.Property(e => e.SubmitUser).HasColumnName("submitUser");

                entity.Property(e => e.UpdateTime)
                    .HasColumnName("updateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Version).HasColumnName("version");

                entity.Property(e => e.WorkOrderCode)
                    .HasColumnName("workOrderCode")
                    .HasMaxLength(500);

                entity.Property(e => e.WorkOrderName)
                    .HasColumnName("workOrderName")
                    .HasMaxLength(2000);
            });

            modelBuilder.Entity<FlowBillsRecord>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AuditDate)
                    .HasColumnName("auditDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Batch).HasColumnName("batch");

                entity.Property(e => e.BillsId).HasColumnName("billsId");

                entity.Property(e => e.Choice)
                    .HasColumnName("choice")
                    .HasMaxLength(4000);

                entity.Property(e => e.Condition).HasColumnName("condition");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(1000);

                entity.Property(e => e.EndTime)
                    .HasColumnName("endTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.NextStepId)
                    .HasColumnName("nextStepId")
                    .HasMaxLength(50);

                entity.Property(e => e.PrevBillsRecordId)
                    .HasColumnName("prevBillsRecordId")
                    .HasMaxLength(50);

                entity.Property(e => e.PrevStepId)
                    .HasColumnName("prevStepId")
                    .HasMaxLength(50);

                entity.Property(e => e.Sort).HasColumnName("sort");

                entity.Property(e => e.StartTime)
                    .HasColumnName("startTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasMaxLength(4000);

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.UserId)
                    .HasColumnName("userId")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Bills)
                    .WithMany(p => p.FlowBillsRecord)
                    .HasForeignKey(d => d.BillsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FlowBillsRecord_FlowBills");
            });

            modelBuilder.Entity<FlowBillsRecordUser>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.BillsRecordId).HasColumnName("billsRecordId");

                entity.Property(e => e.BillsRecordOutId)
                    .HasColumnName("billsRecordOutId")
                    .HasMaxLength(50);

                entity.Property(e => e.Choice)
                    .HasColumnName("choice")
                    .HasMaxLength(4000);

                entity.Property(e => e.Condition)
                    .HasColumnName("condition")
                    .HasMaxLength(4000);

                entity.Property(e => e.CreateTime)
                    .HasColumnName("createTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.DisplayState)
                    .HasColumnName("displayState")
                    .HasMaxLength(4000);

                entity.Property(e => e.EndTime)
                    .HasColumnName("endTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Opinion)
                    .HasColumnName("opinion")
                    .HasMaxLength(4000);

                entity.Property(e => e.PortalToDoId).HasColumnName("portalToDoId");

                entity.Property(e => e.RunTime)
                    .HasColumnName("runTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.SmsId)
                    .HasColumnName("smsId")
                    .HasMaxLength(4000);

                entity.Property(e => e.StartTime)
                    .HasColumnName("startTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasMaxLength(4000);

                entity.Property(e => e.StepId)
                    .HasColumnName("stepId")
                    .HasMaxLength(4000);

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.UserId)
                    .HasColumnName("userId")
                    .HasMaxLength(50);

                entity.HasOne(d => d.BillsRecord)
                    .WithMany(p => p.FlowBillsRecordUser)
                    .HasForeignKey(d => d.BillsRecordId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FlowBillsRecordUser_FlowBillsRecord");
            });

            modelBuilder.Entity<FlowMain>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(1024);

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(1024);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<FlowOrderCodes>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasColumnName("code")
                    .HasMaxLength(512);

                entity.Property(e => e.Day)
                    .IsRequired()
                    .HasColumnName("day")
                    .HasMaxLength(64);

                entity.Property(e => e.Index).HasColumnName("index");

                entity.Property(e => e.Pinyin)
                    .IsRequired()
                    .HasColumnName("pinyin")
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<FlowStep>(entity =>
            {
                entity.HasKey(e => e.StepId);

                entity.Property(e => e.StepId)
                    .HasColumnName("stepId")
                    .ValueGeneratedNever();

                entity.Property(e => e.Agency)
                    .HasColumnName("agency")
                    .HasMaxLength(32);

                entity.Property(e => e.AuditId)
                    .HasColumnName("auditId")
                    .HasMaxLength(256);

                entity.Property(e => e.AuditIdRead)
                    .HasColumnName("auditIdRead")
                    .HasMaxLength(256);

                entity.Property(e => e.AuditNorm)
                    .HasColumnName("auditNorm")
                    .HasMaxLength(128);

                entity.Property(e => e.AuditNormRead)
                    .HasColumnName("auditNormRead")
                    .HasMaxLength(128);

                entity.Property(e => e.Circularize)
                    .HasColumnName("circularize")
                    .HasMaxLength(32);

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(4000);

                entity.Property(e => e.LinkCode)
                    .HasColumnName("linkCode")
                    .HasMaxLength(128);

                entity.Property(e => e.MainId)
                    .HasColumnName("mainId")
                    .HasMaxLength(64);

                entity.Property(e => e.ReminderTimeout).HasColumnName("reminderTimeout");

                entity.Property(e => e.RunMode)
                    .HasColumnName("runMode")
                    .HasMaxLength(4000);

                entity.Property(e => e.ShowTabIndex)
                    .HasColumnName("showTabIndex")
                    .HasMaxLength(4000);

                entity.Property(e => e.SmsTemplateRead)
                    .HasColumnName("smsTemplateRead")
                    .HasMaxLength(4000);

                entity.Property(e => e.SmsTemplateToDo)
                    .HasColumnName("smsTemplateToDo")
                    .HasMaxLength(4000);

                entity.Property(e => e.StepName)
                    .HasColumnName("stepName")
                    .HasMaxLength(64);

                entity.Property(e => e.StepStatus).HasColumnName("stepStatus");

                entity.Property(e => e.Style)
                    .HasColumnName("style")
                    .HasMaxLength(512);
            });

            modelBuilder.Entity<FlowStepPath>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Condition).HasColumnName("condition");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(1024);

                entity.Property(e => e.Expression)
                    .HasColumnName("expression")
                    .HasMaxLength(512);

                entity.Property(e => e.MainId)
                    .HasColumnName("mainId")
                    .HasMaxLength(64);

                entity.Property(e => e.Nikename)
                    .HasColumnName("nikename")
                    .HasMaxLength(512);

                entity.Property(e => e.StartStepId)
                    .HasColumnName("startStepId")
                    .HasMaxLength(64);

                entity.Property(e => e.StopStepId)
                    .HasColumnName("stopStepId")
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<GeneralExample>(entity =>
            {
                entity.HasKey(e => e.ExampleId);

                entity.Property(e => e.ExampleId)
                    .HasColumnName("exampleId")
                    .ValueGeneratedNever();

                entity.Property(e => e.AutoComSelect)
                    .HasColumnName("autoComSelect")
                    .HasMaxLength(4000);

                entity.Property(e => e.AutoComSelectText)
                    .HasColumnName("autoComSelectText")
                    .HasMaxLength(4000);

                entity.Property(e => e.AutoComplete)
                    .HasColumnName("autoComplete")
                    .HasMaxLength(4000);

                entity.Property(e => e.CreateTime)
                    .HasColumnName("createTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateTimePicker)
                    .HasColumnName("dateTimePicker")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateTimePickerDate)
                    .HasColumnName("dateTimePickerDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateTimePickerFormatter)
                    .HasColumnName("dateTimePickerFormatter")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateTimePickerYymm)
                    .HasColumnName("dateTimePickerYYMM")
                    .HasColumnType("datetime");

                entity.Property(e => e.Department)
                    .HasColumnName("department")
                    .HasMaxLength(4000);

                entity.Property(e => e.EmailText)
                    .HasColumnName("emailText")
                    .HasMaxLength(64);

                entity.Property(e => e.ExampleCheckbox)
                    .HasColumnName("exampleCheckbox")
                    .HasMaxLength(256);

                entity.Property(e => e.ExampleName)
                    .HasColumnName("exampleName")
                    .HasMaxLength(64);

                entity.Property(e => e.ExamplePhone)
                    .HasColumnName("examplePhone")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.ExampleRadio)
                    .HasColumnName("exampleRadio")
                    .HasMaxLength(256);

                entity.Property(e => e.ExampleSelect)
                    .HasColumnName("exampleSelect")
                    .HasMaxLength(256);

                entity.Property(e => e.ExampleText)
                    .HasColumnName("exampleText")
                    .HasMaxLength(256);

                entity.Property(e => e.ExampleTime)
                    .HasColumnName("exampleTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.ExampleUser)
                    .HasColumnName("exampleUser")
                    .HasMaxLength(4000);

                entity.Property(e => e.LinkageSelectA)
                    .HasColumnName("linkageSelectA")
                    .HasMaxLength(64);

                entity.Property(e => e.LinkageSelectB)
                    .HasColumnName("linkageSelectB")
                    .HasMaxLength(64);

                entity.Property(e => e.LinkageSelectC)
                    .HasColumnName("linkageSelectC")
                    .HasMaxLength(64);

                entity.Property(e => e.LinkageSelectD)
                    .HasColumnName("linkageSelectD")
                    .HasMaxLength(64);

                entity.Property(e => e.LinkageSelectE)
                    .HasColumnName("linkageSelectE")
                    .HasMaxLength(64);

                entity.Property(e => e.LinkageSelectF)
                    .HasColumnName("linkageSelectF")
                    .HasMaxLength(64);

                entity.Property(e => e.MobileText)
                    .HasColumnName("mobileText")
                    .HasMaxLength(64);

                entity.Property(e => e.OuPicker)
                    .HasColumnName("ouPicker")
                    .HasMaxLength(64);

                entity.Property(e => e.ParentId).HasColumnName("parentId");

                entity.Property(e => e.PlainDecimal).HasColumnName("plainDecimal");

                entity.Property(e => e.PlainInt).HasColumnName("plainInt");

                entity.Property(e => e.PlainText)
                    .HasColumnName("plainText")
                    .HasMaxLength(64);

                entity.Property(e => e.RepeatText)
                    .HasColumnName("repeatText")
                    .HasMaxLength(64);

                entity.Property(e => e.RequiredText)
                    .HasColumnName("requiredText")
                    .HasMaxLength(64);

                entity.Property(e => e.UserPicker)
                    .HasColumnName("userPicker")
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<SysAttachment>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateBy)
                    .HasColumnName("createBy")
                    .HasMaxLength(64);

                entity.Property(e => e.CreateByName)
                    .HasColumnName("createByName")
                    .HasMaxLength(64);

                entity.Property(e => e.CreateTime)
                    .HasColumnName("createTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(512);

                entity.Property(e => e.Names).HasColumnName("names");

                entity.Property(e => e.Path).HasColumnName("path");

                entity.Property(e => e.RelationId)
                    .HasColumnName("relationID")
                    .HasMaxLength(64);

                entity.Property(e => e.Size).HasColumnName("size");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Suffix)
                    .HasColumnName("suffix")
                    .HasMaxLength(32);

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.Url).HasColumnName("url");
            });

            modelBuilder.Entity<SysDepartment>(entity =>
            {
                entity.HasKey(e => e.DepartmentId);

                entity.Property(e => e.DepartmentId)
                    .HasColumnName("departmentId")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateBy).HasColumnName("createBy");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("createTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.DepartmentCode)
                    .HasColumnName("departmentCode")
                    .HasMaxLength(64);

                entity.Property(e => e.DepartmentFullName)
                    .HasColumnName("departmentFullName")
                    .HasMaxLength(512);

                entity.Property(e => e.DepartmentName)
                    .HasColumnName("departmentName")
                    .HasMaxLength(64);

                entity.Property(e => e.OrderNo).HasColumnName("orderNo");

                entity.Property(e => e.ParentId).HasColumnName("parentId");

                entity.Property(e => e.PdateBy).HasColumnName("pdateBy");

                entity.Property(e => e.UpdateTime)
                    .HasColumnName("updateTime")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<SysDictionary>(entity =>
            {
                entity.HasKey(e => new { e.Type, e.Member })
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(64);

                entity.Property(e => e.Member)
                    .HasColumnName("member")
                    .HasMaxLength(64);

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(2048);

                entity.Property(e => e.MemberName)
                    .IsRequired()
                    .HasColumnName("memberName")
                    .HasMaxLength(64);

                entity.Property(e => e.OrderNo).HasColumnName("orderNo");
            });

            modelBuilder.Entity<SysLeader>(entity =>
            {
                entity.HasKey(e => e.LeaderId);

                entity.Property(e => e.LeaderId)
                    .HasColumnName("leaderId")
                    .ValueGeneratedNever();

                entity.Property(e => e.DepartmentCode)
                    .HasColumnName("departmentCode")
                    .HasMaxLength(512);

                entity.Property(e => e.Pos)
                    .HasColumnName("pos")
                    .HasMaxLength(2048);

                entity.Property(e => e.Sequence)
                    .HasColumnName("sequence")
                    .HasMaxLength(2048);

                entity.Property(e => e.UserCode)
                    .HasColumnName("userCode")
                    .HasMaxLength(512);
            });

            modelBuilder.Entity<SysLog>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("createTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.DepartmentName)
                    .HasColumnName("departmentName")
                    .HasMaxLength(500);

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(2048);

                entity.Property(e => e.IpAddress)
                    .HasColumnName("ipAddress")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserCode)
                    .HasColumnName("userCode")
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.UserName)
                    .HasColumnName("userName")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SysModule>(entity =>
            {
                entity.HasKey(e => e.ModuleId);

                entity.Property(e => e.ModuleId)
                    .HasColumnName("moduleId")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(2048);

                entity.Property(e => e.ModuleIcon)
                    .HasColumnName("moduleIcon")
                    .HasMaxLength(512);

                entity.Property(e => e.ModuleName)
                    .HasColumnName("moduleName")
                    .HasMaxLength(64);

                entity.Property(e => e.OrderNo).HasColumnName("orderNo");

                entity.Property(e => e.ParentId).HasColumnName("parentId");
            });

            modelBuilder.Entity<SysModulePage>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(2048);

                entity.Property(e => e.ModuleId).HasColumnName("moduleId");

                entity.Property(e => e.OrderNo).HasColumnName("orderNo");

                entity.Property(e => e.PageIcon)
                    .HasColumnName("pageIcon")
                    .HasMaxLength(512);

                entity.Property(e => e.PageName)
                    .IsRequired()
                    .HasColumnName("pageName")
                    .HasMaxLength(64);

                entity.Property(e => e.PageSign)
                    .IsRequired()
                    .HasColumnName("pageSign")
                    .HasMaxLength(64);

                entity.Property(e => e.PageUrl)
                    .HasColumnName("pageUrl")
                    .HasMaxLength(512);
            });

            modelBuilder.Entity<SysOperation>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateBy)
                    .HasColumnName("createBy")
                    .HasMaxLength(64);

                entity.Property(e => e.CreateTime)
                    .HasColumnName("createTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.OperationName)
                    .HasColumnName("operationName")
                    .HasMaxLength(64);

                entity.Property(e => e.OperationSign)
                    .HasColumnName("operationSign")
                    .HasMaxLength(64);

                entity.Property(e => e.OrderNo).HasColumnName("orderNo");
            });

            modelBuilder.Entity<SysPageOperation>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.OperationSign)
                    .HasColumnName("operationSign")
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.PageId).HasColumnName("pageID");
            });

            modelBuilder.Entity<SysRole>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.RoleName)
                    .HasColumnName("roleName")
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<SysRoleOperatePower>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.ModulePageId).HasColumnName("modulePageId");

                entity.Property(e => e.ModuleParentId).HasColumnName("moduleParentId");

                entity.Property(e => e.OperationSign)
                    .HasColumnName("operationSign")
                    .HasMaxLength(512);

                entity.Property(e => e.RoleId).HasColumnName("roleId");
            });

            modelBuilder.Entity<SysRoleUser>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.Property(e => e.UserId).HasColumnName("userId");
            });

            modelBuilder.Entity<SysServer>(entity =>
            {
                entity.HasKey(e => e.ServerIp);

                entity.Property(e => e.ServerIp)
                    .HasColumnName("serverIP")
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateTime)
                    .HasColumnName("createTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdateTime)
                    .HasColumnName("updateTime")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<SysSmsCode>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateTime)
                    .HasColumnName("createTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.IsVerify).HasColumnName("isVerify");

                entity.Property(e => e.Mobile)
                    .HasColumnName("mobile")
                    .HasMaxLength(32);

                entity.Property(e => e.OverTime)
                    .HasColumnName("overTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.SmsCode)
                    .HasColumnName("smsCode")
                    .HasMaxLength(32);

                entity.Property(e => e.SmsSign)
                    .HasColumnName("smsSign")
                    .HasMaxLength(32);

                entity.Property(e => e.VerifyTime)
                    .HasColumnName("verifyTime")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<SysUser>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId)
                    .HasColumnName("userId")
                    .ValueGeneratedNever();

                entity.Property(e => e.Birthday)
                    .HasColumnName("birthday")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreateBy).HasColumnName("createBy");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("createTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.DepartmentId).HasColumnName("departmentId");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(128);

                entity.Property(e => e.ExtendId)
                    .HasColumnName("extendId")
                    .HasMaxLength(64);

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasMaxLength(32);

                entity.Property(e => e.IdCard)
                    .HasColumnName("idCard")
                    .HasMaxLength(32);

                entity.Property(e => e.Mobile)
                    .HasColumnName("mobile")
                    .HasMaxLength(32);

                entity.Property(e => e.OrderNo).HasColumnName("orderNo");

                entity.Property(e => e.Post)
                    .HasColumnName("post")
                    .HasMaxLength(32);

                entity.Property(e => e.UpdateBy).HasColumnName("updateBy");

                entity.Property(e => e.UpdateTime)
                    .HasColumnName("updateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserCode)
                    .HasColumnName("userCode")
                    .HasMaxLength(32);

                entity.Property(e => e.UserName)
                    .HasColumnName("userName")
                    .HasMaxLength(32);

                entity.Property(e => e.UserPassword)
                    .HasColumnName("userPassword")
                    .HasMaxLength(128);

                entity.Property(e => e.UserStatus)
                    .HasColumnName("userStatus")
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<SysUserClientId>(entity =>
            {
                entity.HasKey(e => e.ClientId);

                entity.Property(e => e.ClientId)
                    .HasColumnName("clientId")
                    .HasMaxLength(64)
                    .ValueGeneratedNever();

                entity.Property(e => e.UpdateTime)
                    .HasColumnName("updateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("userId");
            });

            modelBuilder.Entity<SysUserOpenId>(entity =>
            {
                entity.HasKey(e => e.OpenId);

                entity.Property(e => e.OpenId)
                    .HasColumnName("openId")
                    .HasMaxLength(64)
                    .ValueGeneratedNever();

                entity.Property(e => e.BindTime)
                    .HasColumnName("bindTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("createTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("userId");
            });
        }
    }
}
