
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Example](
	[id] [uniqueidentifier] NOT NULL,
	[exampleName] [varchar](255) NULL,
	[exampleText] [varchar](255) NULL,
	[exampleRadio] [varchar](255) NULL,
	[exampleCheckbox] [varchar](255) NULL,
	[exampleSelect] [varchar](255) NULL,
	[examplePhone] [varchar](13) NULL,
	[exampleTime] [datetime] NULL,
	[exampleUser] [nvarchar](4000) NULL,
	[department] [nvarchar](4000) NULL,
	[autoComplete] [nvarchar](4000) NULL,
	[autoComSelectText] [nvarchar](4000) NULL,
	[autoComSelect] [nvarchar](4000) NULL,
	[linkageSelectA] [nvarchar](50) NULL,
	[linkageSelectB] [nvarchar](50) NULL,
	[linkageSelectC] [nvarchar](50) NULL,
	[createTime] [datetime] NULL,
	[linkageSelectD] [nvarchar](50) NULL,
	[linkageSelectE] [nvarchar](50) NULL,
	[linkageSelectF] [nvarchar](50) NULL,
	[parentId] [uniqueidentifier] NULL,
 CONSTRAINT [PK__Example__3214EC27E0ACAC72] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FlowBills](
	[id] [uniqueidentifier] NOT NULL,
	[parentId] [uniqueidentifier] NULL,
	[workOrderName] [nvarchar](2000) NULL,
	[billsCode] [nvarchar](255) NULL,
	[mainId] [nvarchar](50) NULL,
	[stepId] [nvarchar](50) NULL,
	[sort] [int] NOT NULL,
	[state] [nvarchar](50) NULL,
	[submitUser] [uniqueidentifier] NOT NULL,
	[createTime] [datetime] NOT NULL,
	[updateTime] [datetime] NULL,
	[Description] [nvarchar](1000) NULL,
	[Version] [int] NULL,
	[BillsType] [nvarchar](500) NULL,
	[WorkOrderCode] [nvarchar](500) NULL,
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
	[upStep] [nvarchar](50) NULL,
	[nextStep] [nvarchar](50) NULL,
	[userId] [nvarchar](50) NULL,
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
	[upBillsRecordId] [nvarchar](50) NULL,
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
	[billsRecordId] [uniqueidentifier] NOT NULL,
	[billsRecordOutId] [nvarchar](50) NULL,
	[userId] [nvarchar](50) NULL,
	[createTime] [datetime] NULL,
	[startTime] [datetime] NULL,
	[endTime] [datetime] NULL,
	[type] [int] NOT NULL,
	[state] [nvarchar](4000) NULL,
	[condition] [nvarchar](4000) NULL,
	[stepId] [nvarchar](4000) NULL,
	[choice] [nvarchar](4000) NULL,
	[opinion] [nvarchar](4000) NULL,
	[displayState] [nvarchar](4000) NULL,
	[runTime] [datetime] NULL,
	[portalToDoId] [int] NULL,
	[smsId] [nvarchar](4000) NULL,
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
	[code] [nvarchar](1023) NULL,
	[name] [nvarchar](200) NULL,
	[description] [nvarchar](1000) NULL,
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
	[id] [uniqueidentifier] NOT NULL,
	[mainId] [nvarchar](64) NULL,
	[stepName] [nvarchar](256) NULL,
	[auditNorm] [nvarchar](128) NULL,
	[auditId] [nvarchar](256) NULL,
	[auditors] [int] NOT NULL,
	[stepStatus] [int] NOT NULL,
	[description] [nvarchar](4000) NULL,
	[deployInfo] [nvarchar](4000) NULL,
	[openChoices] [nvarchar](4000) NULL,
	[power] [nvarchar](4000) NULL,
	[runMode] [nvarchar](4000) NULL,
	[joinMode] [nvarchar](4000) NULL,
	[examineMode] [nvarchar](4000) NULL,
	[relationStepKey] [nvarchar](4000) NULL,
	[percentage] [int] NOT NULL,
	[function] [nvarchar](1000) NULL,
	[linkCode] [nvarchar](128) NULL,
	[auditLinkCode] [nvarchar](128) NULL,
	[showTabIndex] [nvarchar](4000) NULL,
	[circularize] [nvarchar](4000) NULL,
	[reminderTimeout] [bigint] NULL,
	[smsTemplateToDo] [nvarchar](4000) NULL,
	[smsTemplateRead] [nvarchar](4000) NULL,
	[auditNormRead] [nvarchar](128) NULL,
	[auditIdRead] [nvarchar](256) NULL,
 CONSTRAINT [PK_dbo.FlowStep] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FlowStepPath](
	[id] [uniqueidentifier] NOT NULL,
	[mainId] [nvarchar](64) NULL,
	[upStep] [nvarchar](64) NULL,
	[nextStep] [nvarchar](64) NULL,
	[condition] [int] NOT NULL,
	[expression] [nvarchar](1024) NULL,
	[description] [nvarchar](1024) NULL,
	[btnName] [nvarchar](4000) NULL,
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
 CONSTRAINT [PK_dbo.DGLeader] PRIMARY KEY CLUSTERED 
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
 CONSTRAINT [PK__SysModulePage] PRIMARY KEY CLUSTERED 
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
 CONSTRAINT [PK__SysRoleM__3214EC2729E23816] PRIMARY KEY CLUSTERED 
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
	[ServerIP] [nvarchar](128) NOT NULL,
	[CreateTime] [datetime] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_SysServer] PRIMARY KEY CLUSTERED 
(
	[ServerIP] ASC
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

ALTER TABLE [dbo].[FlowBills]  WITH CHECK ADD  CONSTRAINT [FK_dbo.FlowBills_dbo.FlowBills_ParentId] FOREIGN KEY([parentId])
REFERENCES [dbo].[FlowBills] ([id])
GO
ALTER TABLE [dbo].[FlowBills] CHECK CONSTRAINT [FK_dbo.FlowBills_dbo.FlowBills_ParentId]
GO
ALTER TABLE [dbo].[FlowBillsRecord]  WITH CHECK ADD  CONSTRAINT [FK_dbo.FlowBillsRecord_dbo.FlowBills_BillsId] FOREIGN KEY([billsId])
REFERENCES [dbo].[FlowBills] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FlowBillsRecord] CHECK CONSTRAINT [FK_dbo.FlowBillsRecord_dbo.FlowBills_BillsId]
GO
ALTER TABLE [dbo].[FlowBillsRecordUser]  WITH CHECK ADD  CONSTRAINT [FK_dbo.FlowBillsRecordUser_dbo.FlowBillsRecord_BillsRecordId] FOREIGN KEY([billsRecordId])
REFERENCES [dbo].[FlowBillsRecord] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FlowBillsRecordUser] CHECK CONSTRAINT [FK_dbo.FlowBillsRecordUser_dbo.FlowBillsRecord_BillsRecordId]
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


-- =============================================
-- Author:		陈银鑫
-- Create date: 2016-12-09 10:53:30 
-- Description:	获取部门全称
-- =============================================
CREATE FUNCTION [dbo].[fn_GetDepartmentFullName]
(
	-- Add the parameters for the function here
	@DepartmentID uniqueidentifier
)
RETURNS nvarchar(1000)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @fullName nvarchar(1000);

	-- Add the T-SQL statements to compute the return value here
	with OUInfo
	as
	(
	select DepartmentID, cast( DepartmentName as nvarchar(1024)) as OUFullName,ParentID from SysDepartment where  DepartmentID=@DepartmentID
	union all
	select Data.DepartmentID, cast( ( Data.DepartmentName + '→' + tmp.OUFullName ) as nvarchar(1024) ) as OUFullName ,Data.ParentID
	from SysDepartment Data
	inner join OUInfo tmp on Data.DepartmentID = tmp.ParentID
	)
	select top 1 @fullName=OUFullName from OUInfo where ParentID is null;

	-- Return the result of the function
	RETURN @fullName

END


GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[fn_GetDepartmentsByID]
(	
	-- Add the parameters for the function here
	@DepartmentID uniqueidentifier
)
RETURNS TABLE 
AS

RETURN 
(
    with Departments
	as
	(
	select DepartmentID,ParentID from SysDepartment where  DepartmentID=@DepartmentID
	union all
	select Data.DepartmentID,Data.ParentID
	from SysDepartment Data
	inner join Departments tmp on Data.ParentID = tmp.DepartmentID
	)
	-- Add the SELECT statement with parameter references here
	SELECT DepartmentID from Departments 
)


GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取所有父级模块ID
-- =============================================
CREATE FUNCTION [dbo].[fn_GetModuleParentID]
(	
	-- Add the parameters for the function here
	@ModulePageID uniqueidentifier
)
RETURNS TABLE 
AS
RETURN 
(	
    with Departments
	as
	(
	select ModuleID,ParentID from SysModule where  ModuleID=@ModulePageID
	union all
	select Data.ModuleID,Data.ParentID
	from SysModule Data
	inner join Departments tmp on Data.ModuleID = tmp.ParentID
	)
	-- Add the SELECT statement with parameter references here
	SELECT ModuleID from Departments 
)


GO
