SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FlowBills](
	[Id] [uniqueidentifier] NOT NULL,
	[ParentId] [uniqueidentifier] NULL,
	[MainId] [uniqueidentifier] NULL,
	[StepId] [uniqueidentifier] NULL,
	[BillsType] [nvarchar](500) NULL,
	[WorkOrderCode] [nvarchar](500) NULL,
	[WorkOrderName] [nvarchar](2000) NULL,
	[BillsCode] [nvarchar](255) NULL,
	[Sort] [int] NOT NULL,
	[State] [nvarchar](50) NULL,
	[SubmitUser] [uniqueidentifier] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateTime] [datetime] NULL,
	[Description] [nvarchar](1024) NULL,
	[Version] [int] NULL,
 CONSTRAINT [PK_dbo.FlowBills] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FlowBillsRecord](
	[Id] [uniqueidentifier] NOT NULL,
	[BillsId] [uniqueidentifier] NOT NULL,
	[PrevStepId] [uniqueidentifier] NULL,
	[NextStepId] [uniqueidentifier] NULL,
	[UserId] [uniqueidentifier] NULL,
	[PrevBillsRecordId] [uniqueidentifier] NULL,
	[AuditDate] [datetime] NULL,
	[Sort] [int] NOT NULL,
	[Condition] [int] NOT NULL,
	[State] [nvarchar](4000) NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[Choice] [nvarchar](4000) NULL,
	[Type] [int] NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[Batch] [int] NULL,
 CONSTRAINT [PK_dbo.FlowBillsRecord] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FlowBillsRecordUser](
	[Id] [uniqueidentifier] NOT NULL,
	[StepId] [uniqueidentifier] NULL,
	[BillsRecordId] [uniqueidentifier] NOT NULL,
	[BillsRecordOutId] [uniqueidentifier] NULL,
	[UserId] [uniqueidentifier] NULL,
	[CreateTime] [datetime] NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[Type] [int] NOT NULL,
	[State] [nvarchar](4000) NULL,
	[Condition] [nvarchar](4000) NULL,
	[Choice] [nvarchar](4000) NULL,
	[Opinion] [nvarchar](4000) NULL,
	[DisplayState] [nvarchar](4000) NULL,
	[RunTime] [datetime] NULL,
 CONSTRAINT [PK_dbo.FlowBillsRecordUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FlowMain](
	[Id] [uniqueidentifier] NOT NULL,
	[Code] [nvarchar](1024) NULL,
	[Name] [nvarchar](256) NULL,
	[Description] [nvarchar](1024) NULL,
 CONSTRAINT [PK_dbo.FlowMain] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FlowOrderCodes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Pinyin] [nvarchar](64) NOT NULL,
	[Day] [nvarchar](64) NOT NULL,
	[Index] [int] NOT NULL,
	[Code] [nvarchar](512) NOT NULL,
 CONSTRAINT [PK_FlowOrderCodes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FlowStep](
	[StepId] [uniqueidentifier] NOT NULL,
	[MainId] [uniqueidentifier] NULL,
	[StepName] [nvarchar](64) NULL,
	[StepStatus] [int] NOT NULL,
	[Agency] [nvarchar](32) NULL,
	[Circularize] [nvarchar](32) NULL,
	[RunMode] [nvarchar](4000) NULL,
	[LinkCode] [nvarchar](128) NULL,
	[ShowTabIndex] [nvarchar](4000) NULL,
	[ReminderTimeout] [bigint] NULL,
	[AuditNorm] [nvarchar](128) NULL,
	[AuditId] [nvarchar](256) NULL,
	[AuditNormRead] [nvarchar](128) NULL,
	[AuditIdRead] [nvarchar](256) NULL,
	[SmsTemplateToDo] [nvarchar](4000) NULL,
	[SmsTemplateRead] [nvarchar](4000) NULL,
	[Description] [nvarchar](4000) NULL,
	[Style] [nvarchar](512) NULL,
 CONSTRAINT [PK_dbo.FlowStep] PRIMARY KEY CLUSTERED 
(
	[StepId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FlowStepPath](
	[Id] [uniqueidentifier] NOT NULL,
	[MainId] [uniqueidentifier] NULL,
	[StartStepId] [uniqueidentifier] NULL,
	[StopStepId] [uniqueidentifier] NULL,
	[Nikename] [nvarchar](512) NULL,
	[Condition] [int] NOT NULL,
	[Expression] [nvarchar](512) NULL,
	[Description] [nvarchar](1024) NULL,
 CONSTRAINT [PK_dbo.FlowStepPath] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


ALTER TABLE [dbo].[FlowBillsRecord]  WITH CHECK ADD  CONSTRAINT [FK_FlowBillsRecord_FlowBills] FOREIGN KEY([BillsId])
REFERENCES [dbo].[FlowBills] ([Id])
GO
ALTER TABLE [dbo].[FlowBillsRecord] CHECK CONSTRAINT [FK_FlowBillsRecord_FlowBills]
GO
ALTER TABLE [dbo].[FlowBillsRecordUser]  WITH CHECK ADD  CONSTRAINT [FK_FlowBillsRecordUser_FlowBillsRecord] FOREIGN KEY([BillsRecordId])
REFERENCES [dbo].[FlowBillsRecord] ([Id])
GO
ALTER TABLE [dbo].[FlowBillsRecordUser] CHECK CONSTRAINT [FK_FlowBillsRecordUser_FlowBillsRecord]
GO