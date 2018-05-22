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

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GeneralExample](
	[exampleId] [uniqueidentifier] NOT NULL,
	[exampleName] [nvarchar](64) NULL,
	[plainText] [nvarchar](64) NULL,
	[requiredText] [nvarchar](64) NULL,
	[repeatText] [nvarchar](64) NULL,
	[mobileText] [nvarchar](64) NULL,
	[emailText] [nvarchar](64) NULL,
	[plainInt] [int] NULL,
	[plainDecimal] [decimal](18, 2) NULL,
	[dateTimePicker] [datetime] NULL,
	[dateTimePickerDate] [datetime] NULL,
	[dateTimePickerYYMM] [datetime] NULL,
	[dateTimePickerFormatter] [datetime] NULL,
	[userPicker] [nvarchar](64) NULL,
	[ouPicker] [nvarchar](64) NULL,
	[exampleText] [nvarchar](256) NULL,
	[exampleRadio] [nvarchar](256) NULL,
	[exampleCheckbox] [nvarchar](256) NULL,
	[exampleSelect] [nvarchar](256) NULL,
	[examplePhone] [varchar](16) NULL,
	[exampleTime] [datetime] NULL,
	[exampleUser] [nvarchar](4000) NULL,
	[department] [nvarchar](4000) NULL,
	[autoComplete] [nvarchar](4000) NULL,
	[autoComSelectText] [nvarchar](4000) NULL,
	[autoComSelect] [nvarchar](4000) NULL,
	[linkageSelectA] [nvarchar](64) NULL,
	[linkageSelectB] [nvarchar](64) NULL,
	[linkageSelectC] [nvarchar](64) NULL,
	[createTime] [datetime] NULL,
	[linkageSelectD] [nvarchar](64) NULL,
	[linkageSelectE] [nvarchar](64) NULL,
	[linkageSelectF] [nvarchar](64) NULL,
	[parentId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_GeneralExample] PRIMARY KEY CLUSTERED 
(
	[exampleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysAttachment](
	[id] [uniqueidentifier] NOT NULL,
	[relationID] [nvarchar](64) NULL,
	[name] [nvarchar](512) NULL,
	[url] [nvarchar](max) NULL,
	[type] [int] NULL,
	[suffix] [nvarchar](32) NULL,
	[path] [nvarchar](max) NULL,
	[names] [nvarchar](max) NULL,
	[status] [int] NULL,
	[size] [bigint] NULL,
	[createBy] [nvarchar](64) NULL,
	[createByName] [nvarchar](64) NULL,
	[createTime] [datetime] NULL,
 CONSTRAINT [PK__SysAttachment] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysDepartment](
	[departmentId] [uniqueidentifier] NOT NULL,
	[parentId] [uniqueidentifier] NULL,
	[departmentCode] [nvarchar](64) NULL,
	[departmentName] [nvarchar](64) NULL,
	[departmentFullName] [nvarchar](512) NULL,
	[orderNo] [int] NULL,
	[createTime] [datetime] NULL,
	[createBy] [uniqueidentifier] NULL,
	[updateTime] [datetime] NULL,
	[pdateBy] [uniqueidentifier] NULL,
 CONSTRAINT [PK_SysDepartment] PRIMARY KEY CLUSTERED 
(
	[departmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysDictionary](
	[type] [nvarchar](64) NOT NULL,
	[member] [nvarchar](64) NOT NULL,
	[memberName] [nvarchar](64) NOT NULL,
	[description] [nvarchar](2048) NULL,
	[orderNo] [int] NULL,
 CONSTRAINT [PK_Dictionary] PRIMARY KEY NONCLUSTERED 
(
	[type] ASC,
	[member] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysLeader](
	[leaderId] [uniqueidentifier] NOT NULL,
	[departmentCode] [nvarchar](512) NULL,
	[userCode] [nvarchar](512) NULL,
	[pos] [nvarchar](2048) NULL,
	[sequence] [nvarchar](2048) NULL,
 CONSTRAINT [PK_Leader] PRIMARY KEY CLUSTERED 
(
	[leaderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysLog](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userName] [varchar](50) NULL,
	[userId] [uniqueidentifier] NULL,
	[userCode] [varchar](32) NULL,
	[ipAddress] [varchar](50) NULL,
	[departmentName] [nvarchar](500) NULL,
	[title] [varchar](50) NULL,
	[type] [varchar](50) NULL,
	[createTime] [datetime] NULL,
	[description] [nvarchar](2048) NULL,
 CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysModule](
	[moduleId] [uniqueidentifier] NOT NULL,
	[parentId] [uniqueidentifier] NULL,
	[moduleName] [nvarchar](64) NULL,
	[moduleIcon] [nvarchar](512) NULL,
	[description] [nvarchar](2048) NULL,
	[orderNo] [int] NULL,
 CONSTRAINT [PK_SysModule] PRIMARY KEY CLUSTERED 
(
	[moduleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysModulePage](
	[id] [uniqueidentifier] NOT NULL,
	[moduleId] [uniqueidentifier] NULL,
	[pageSign] [nvarchar](64) NOT NULL,
	[pageName] [nvarchar](64) NOT NULL,
	[pageIcon] [nvarchar](512) NULL,
	[pageUrl] [nvarchar](512) NULL,
	[description] [nvarchar](2048) NULL,
	[orderNo] [int] NULL,
 CONSTRAINT [PK_SysModulePage] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysOperation](
	[id] [uniqueidentifier] NOT NULL,
	[operationSign] [nvarchar](64) NULL,
	[operationName] [nvarchar](64) NULL,
	[createBy] [nvarchar](64) NULL,
	[createTime] [datetime] NULL,
	[orderNo] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysPageOperation](
	[id] [uniqueidentifier] NOT NULL,
	[pageID] [uniqueidentifier] NULL,
	[operationSign] [varchar](64) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysRole](
	[id] [uniqueidentifier] NOT NULL,
	[roleName] [nvarchar](64) NULL,
 CONSTRAINT [PK_SysRole] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysRoleOperatePower](
	[id] [uniqueidentifier] NOT NULL,
	[roleId] [uniqueidentifier] NULL,
	[modulePageId] [uniqueidentifier] NULL,
	[moduleParentId] [uniqueidentifier] NULL,
	[operationSign] [nvarchar](512) NULL,
 CONSTRAINT [PK_SysRoleM_] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysRoleUser](
	[id] [uniqueidentifier] NOT NULL,
	[roleId] [uniqueidentifier] NULL,
	[userId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_SysRolesUser] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysUser](
	[userId] [uniqueidentifier] NOT NULL,
	[departmentId] [uniqueidentifier] NULL,
	[userCode] [nvarchar](32) NULL,
	[userName] [nvarchar](32) NULL,
	[userPassword] [nvarchar](128) NULL,
	[idCard] [nvarchar](32) NULL,
	[mobile] [nvarchar](32) NULL,
	[email] [nvarchar](128) NULL,
	[post] [nvarchar](32) NULL,
	[gender] [nvarchar](32) NULL,
	[birthday] [datetime] NULL,
	[extendId] [nvarchar](64) NULL,
	[userStatus] [nvarchar](32) NULL,
	[orderNo] [int] NULL,
	[createBy] [uniqueidentifier] NULL,
	[createTime] [datetime] NULL,
	[updateBy] [uniqueidentifier] NULL,
	[updateTime] [datetime] NULL,
 CONSTRAINT [PK_SysUser] PRIMARY KEY CLUSTERED 
(
	[userId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysUserOpenId](
	[openId] [nvarchar](64) NOT NULL,
	[userId] [uniqueidentifier] NULL,
	[createTime] [datetime] NULL,
	[bindTime] [datetime] NULL,
 CONSTRAINT [PK_SysUserOpenId] PRIMARY KEY CLUSTERED 
(
	[openId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysUserClientId](
	[clientId] [nvarchar](64) NOT NULL,
	[userId] [uniqueidentifier] NULL,
	[updateTime] [datetime] NULL,
 CONSTRAINT [PK_SysUserClientId] PRIMARY KEY CLUSTERED 
(
	[clientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SysServer](
	[serverIP] [nvarchar](128) NOT NULL,
	[createTime] [datetime] NULL,
	[updateTime] [datetime] NULL,
 CONSTRAINT [PK_SysServer] PRIMARY KEY CLUSTERED 
(
	[serverIP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SysSmsCode](
	[id] [uniqueidentifier] NOT NULL,
	[mobile] [nvarchar](32) NULL,
	[smsSign] [nvarchar](32) NULL,
	[smsCode] [nvarchar](32) NULL,
	[isVerify] [int] NULL,
	[createTime] [datetime] NULL,
	[overTime] [datetime] NULL,
	[verifyTime] [datetime] NULL,
 CONSTRAINT [PK_SysSMSCode] PRIMARY KEY CLUSTERED 
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


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		黎泽金
-- Create date: 2016/12/22
-- Description:	计算时间差
-- =============================================
Create FUNCTION [dbo].[fn_ComputationTimeDifference]
(
	@StartDate datetime,
	@EndDate datetime,
	@Format nvarchar(50)='mm:ss'
)
RETURNS nvarchar(255)
AS
BEGIN
	DECLARE @result nvarchar(255)
	
	set @Format=LOWER(@Format)
	DECLARE @val bigint
	SET @val=datediff(SS, @StartDate, @EndDate)
	
	set @result=
		(case @Format
			when 'hh:mm:ss' then ltrim(@val/3600)+':'+right(REPLICATE('0',2) + ltrim(@val%3600/60),2)+':'+right(REPLICATE('0',2) + ltrim(@val%60),2)
			when 'mm:ss' then ltrim(@val/60)+':'+right(REPLICATE('0',2) + ltrim(@val%60),2)
			else ''
		end)
	RETURN @result

END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

