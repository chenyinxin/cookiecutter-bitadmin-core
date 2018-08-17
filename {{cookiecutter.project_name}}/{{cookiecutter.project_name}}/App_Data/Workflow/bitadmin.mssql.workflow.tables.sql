SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FlowBills](
	[id] [uniqueidentifier] NOT NULL,
	[parentId] [uniqueidentifier] NULL,
	[mainId] [uniqueidentifier] NULL,
	[stepId] [uniqueidentifier] NULL,
	[billsType] [nvarchar](500) NULL,
	[workOrderCode] [nvarchar](500) NULL,
	[workOrderName] [nvarchar](2000) NULL,
	[billsCode] [nvarchar](255) NULL,
	[sort] [int] NOT NULL,
	[state] [nvarchar](50) NULL,
	[submitUser] [uniqueidentifier] NOT NULL,
	[createTime] [datetime] NOT NULL,
	[updateTime] [datetime] NULL,
	[description] [nvarchar](1024) NULL,
	[version] [int] NULL,
 CONSTRAINT [PK_dbo.FlowBills] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FlowBillsRecord](
	[id] [uniqueidentifier] NOT NULL,
	[billsId] [uniqueidentifier] NOT NULL,
	[prevStepId] [uniqueidentifier] NULL,
	[nextStepId] [uniqueidentifier] NULL,
	[userId] [uniqueidentifier] NULL,
	[prevBillsRecordId] [uniqueidentifier] NULL,
	[auditDate] [datetime] NULL,
	[sort] [int] NOT NULL,
	[condition] [int] NOT NULL,
	[state] [nvarchar](4000) NULL,
	[startTime] [datetime] NULL,
	[endTime] [datetime] NULL,
	[choice] [nvarchar](4000) NULL,
	[type] [int] NOT NULL,
	[description] [nvarchar](1000) NULL,
	[batch] [int] NULL,
 CONSTRAINT [PK_dbo.FlowBillsRecord] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FlowBillsRecordUser](
	[id] [uniqueidentifier] NOT NULL,
	[stepId] [uniqueidentifier] NULL,
	[billsRecordId] [uniqueidentifier] NOT NULL,
	[billsRecordOutId] [uniqueidentifier] NULL,
	[userId] [uniqueidentifier] NULL,
	[createTime] [datetime] NULL,
	[startTime] [datetime] NULL,
	[endTime] [datetime] NULL,
	[type] [int] NOT NULL,
	[state] [nvarchar](4000) NULL,
	[condition] [nvarchar](4000) NULL,
	[choice] [nvarchar](4000) NULL,
	[opinion] [nvarchar](4000) NULL,
	[displayState] [nvarchar](4000) NULL,
	[runTime] [datetime] NULL,
 CONSTRAINT [PK_dbo.FlowBillsRecordUser] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FlowMain](
	[id] [uniqueidentifier] NOT NULL,
	[code] [nvarchar](1024) NULL,
	[name] [nvarchar](256) NULL,
	[description] [nvarchar](1024) NULL,
 CONSTRAINT [PK_dbo.FlowMain] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FlowOrderCodes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[pinyin] [nvarchar](64) NOT NULL,
	[day] [nvarchar](64) NOT NULL,
	[index] [int] NOT NULL,
	[code] [nvarchar](512) NOT NULL,
 CONSTRAINT [PK_FlowOrderCodes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FlowStep](
	[stepId] [uniqueidentifier] NOT NULL,
	[mainId] [uniqueidentifier] NULL,
	[stepName] [nvarchar](64) NULL,
	[stepStatus] [int] NOT NULL,
	[agency] [nvarchar](32) NULL,
	[circularize] [nvarchar](32) NULL,
	[runMode] [nvarchar](4000) NULL,
	[linkCode] [nvarchar](128) NULL,
	[showTabIndex] [nvarchar](4000) NULL,
	[reminderTimeout] [bigint] NULL,
	[auditNorm] [nvarchar](128) NULL,
	[auditId] [nvarchar](256) NULL,
	[auditNormRead] [nvarchar](128) NULL,
	[auditIdRead] [nvarchar](256) NULL,
	[smsTemplateToDo] [nvarchar](4000) NULL,
	[smsTemplateRead] [nvarchar](4000) NULL,
	[description] [nvarchar](4000) NULL,
	[style] [nvarchar](512) NULL,
 CONSTRAINT [PK_dbo.FlowStep] PRIMARY KEY CLUSTERED 
(
	[stepId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FlowStepPath](
	[id] [uniqueidentifier] NOT NULL,
	[mainId] [uniqueidentifier] NULL,
	[startStepId] [uniqueidentifier] NULL,
	[stopStepId] [uniqueidentifier] NULL,
	[nikename] [nvarchar](512) NULL,
	[condition] [int] NOT NULL,
	[expression] [nvarchar](512) NULL,
	[description] [nvarchar](1024) NULL,
 CONSTRAINT [PK_dbo.FlowStepPath] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


ALTER TABLE [dbo].[FlowBillsRecord]  WITH CHECK ADD  CONSTRAINT [FK_FlowBillsRecord_FlowBills] FOREIGN KEY([billsId])
REFERENCES [dbo].[FlowBills] ([id])
GO
ALTER TABLE [dbo].[FlowBillsRecord] CHECK CONSTRAINT [FK_FlowBillsRecord_FlowBills]
GO
ALTER TABLE [dbo].[FlowBillsRecordUser]  WITH CHECK ADD  CONSTRAINT [FK_FlowBillsRecordUser_FlowBillsRecord] FOREIGN KEY([billsRecordId])
REFERENCES [dbo].[FlowBillsRecord] ([id])
GO
ALTER TABLE [dbo].[FlowBillsRecordUser] CHECK CONSTRAINT [FK_FlowBillsRecordUser_FlowBillsRecord]
GO