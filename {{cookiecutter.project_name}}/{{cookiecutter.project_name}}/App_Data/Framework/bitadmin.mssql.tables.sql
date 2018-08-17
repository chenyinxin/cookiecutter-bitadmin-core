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
	[operationSign] [varchar](512) NULL,
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

