SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GeneralExample](
	[ExampleId] [uniqueidentifier] NOT NULL,
	[ExampleName] [nvarchar](64) NULL,
	[ParentId] [uniqueidentifier] NULL,
	[PlainText] [nvarchar](64) NULL,
	[RequiredText] [nvarchar](64) NULL,
	[RepeatText] [nvarchar](64) NULL,
	[MobileText] [nvarchar](64) NULL,
	[EmailText] [nvarchar](64) NULL,
	[PlainInt] [int] NULL,
	[PlainDecimal] [decimal](18, 2) NULL,
	[DateTimePicker] [datetime] NULL,
	[DateTimePickerDate] [datetime] NULL,
	[DateTimePickerYYMM] [datetime] NULL,
	[DateTimePickerFormatter] [datetime] NULL,
	[UserPicker] [nvarchar](64) NULL,
	[OUPicker] [nvarchar](64) NULL,
	[ExampleText] [nvarchar](256) NULL,
	[ExampleRadio] [nvarchar](256) NULL,
	[ExampleCheckbox] [nvarchar](256) NULL,
	[ExampleSelect] [nvarchar](256) NULL,
	[ExamplePhone] [varchar](16) NULL,
	[ExampleTime] [datetime] NULL,
	[ExampleUser] [nvarchar](4000) NULL,
	[Department] [nvarchar](4000) NULL,
	[AutoComplete] [nvarchar](4000) NULL,
	[AutoComSelectText] [nvarchar](4000) NULL,
	[AutoComSelect] [nvarchar](4000) NULL,
	[LinkageSelectA] [nvarchar](64) NULL,
	[LinkageSelectB] [nvarchar](64) NULL,
	[LinkageSelectC] [nvarchar](64) NULL,
	[LinkageSelectD] [nvarchar](64) NULL,
	[LinkageSelectE] [nvarchar](64) NULL,
	[LinkageSelectF] [nvarchar](64) NULL,
	[CreateBy] [uniqueidentifier] NULL,
	[CreateTime] [datetime] NULL,
	[UpdateBy] [uniqueidentifier] NULL,
	[UpdateTime] [datetime] NULL,
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
	[Id] [uniqueidentifier] NOT NULL,
	[RelationID] [nvarchar](64) NULL,
	[Name] [nvarchar](512) NULL,
	[Url] [nvarchar](max) NULL,
	[Type] [int] NULL,
	[Suffix] [nvarchar](32) NULL,
	[Path] [nvarchar](max) NULL,
	[Names] [nvarchar](max) NULL,
	[Status] [int] NULL,
	[Size] [bigint] NULL,
	[CreateBy] [nvarchar](64) NULL,
	[CreateByName] [nvarchar](64) NULL,
	[CreateTime] [datetime] NULL,
 CONSTRAINT [PK_SysAttachment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysDepartment](
	[DepartmentId] [uniqueidentifier] NOT NULL,
	[ParentId] [uniqueidentifier] NULL,
	[DepartmentCode] [nvarchar](64) NULL,
	[DepartmentName] [nvarchar](64) NULL,
	[DepartmentFullName] [nvarchar](512) NULL,
	[WeixinWorkId] [bigint] NULL,
	[ExtendId] [nvarchar](64) NULL,
	[OrderNo] [int] NULL,
	[CreateBy] [uniqueidentifier] NULL,
	[CreateTime] [datetime] NULL,
	[UpdateBy] [uniqueidentifier] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_SysDepartment] PRIMARY KEY CLUSTERED 
(
	[DepartmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysDictionary](
	[Type] [nvarchar](64) NOT NULL,
	[Member] [nvarchar](64) NOT NULL,
	[MemberName] [nvarchar](64) NOT NULL,
	[Description] [nvarchar](2048) NULL,
	[OrderNo] [int] NULL,
	[CreateBy] [uniqueidentifier] NULL,
	[CreateTime] [datetime] NULL,
	[UpdateBy] [uniqueidentifier] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_Dictionary] PRIMARY KEY NONCLUSTERED 
(
	[Type] ASC,
	[Member] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysLeader](
	[LeaderId] [uniqueidentifier] NOT NULL,
	[DepartmentCode] [nvarchar](32) NULL,
	[UserCode] [nvarchar](32) NULL,
	[Pos] [nvarchar](32) NULL,
	[Sequence] [nvarchar](32) NULL,
 CONSTRAINT [PK_Leader] PRIMARY KEY CLUSTERED 
(
	[LeaderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [uniqueidentifier] NULL,
	[UserCode] [varchar](64) NULL,
	[UserName] [varchar](64) NULL,
	[DepartmentName] [nvarchar](128) NULL,
	[IpAddress] [varchar](64) NULL,
	[UserAgent] [varchar](512) NULL,
	[Title] [varchar](64) NULL,
	[Type] [varchar](64) NULL,
	[CreateTime] [datetime] NULL,
	[Description] [text] NULL,
 CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysModule](
	[ModuleId] [uniqueidentifier] NOT NULL,
	[ParentId] [uniqueidentifier] NULL,
	[ModuleName] [nvarchar](64) NULL,
	[ModuleIcon] [nvarchar](512) NULL,
	[Description] [nvarchar](2048) NULL,
	[OrderNo] [int] NULL,
	[CreateBy] [uniqueidentifier] NULL,
	[CreateTime] [datetime] NULL,
	[UpdateBy] [uniqueidentifier] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_SysModule] PRIMARY KEY CLUSTERED 
(
	[ModuleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysModulePage](
	[Id] [uniqueidentifier] NOT NULL,
	[ModuleId] [uniqueidentifier] NULL,
	[PageSign] [nvarchar](64) NOT NULL,
	[PageName] [nvarchar](64) NOT NULL,
	[PageIcon] [nvarchar](512) NULL,
	[PageUrl] [nvarchar](512) NULL,
	[Description] [nvarchar](2048) NULL,
	[OrderNo] [int] NULL,
	[CreateBy] [uniqueidentifier] NULL,
	[CreateTime] [datetime] NULL,
	[UpdateBy] [uniqueidentifier] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_SysModulePage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysOperation](
	[Id] [uniqueidentifier] NOT NULL,
	[OperationSign] [nvarchar](64) NULL,
	[OperationName] [nvarchar](64) NULL,
	[OrderNo] [int] NULL,
	[CreateBy] [uniqueidentifier] NULL,
	[CreateTime] [datetime] NULL,
	[UpdateBy] [uniqueidentifier] NULL,
	[UpdateTime] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysPageOperation](
	[Id] [uniqueidentifier] NOT NULL,
	[PageID] [uniqueidentifier] NULL,
	[OperationSign] [varchar](512) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysRole](
	[Id] [uniqueidentifier] NOT NULL,
	[RoleName] [nvarchar](64) NULL,
	[OrderNo] [int] NULL,
	[CreateBy] [uniqueidentifier] NULL,
	[CreateTime] [datetime] NULL,
	[UpdateBy] [uniqueidentifier] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_SysRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysRoleOperatePower](
	[Id] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NULL,
	[ModulePageId] [uniqueidentifier] NULL,
	[ModuleParentId] [uniqueidentifier] NULL,
	[OperationSign] [nvarchar](512) NULL,
 CONSTRAINT [PK_SysRoleM_] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysRoleUser](
	[Id] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NULL,
	[UserId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_SysRolesUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysUser](
	[UserId] [uniqueidentifier] NOT NULL,
	[DepartmentId] [uniqueidentifier] NULL,
	[UserCode] [nvarchar](32) NULL,
	[UserName] [nvarchar](32) NULL,
	[UserPassword] [nvarchar](128) NULL,
	[IdCard] [nvarchar](32) NULL,
	[Mobile] [nvarchar](32) NULL,
	[Email] [nvarchar](128) NULL,
	[Post] [nvarchar](32) NULL,
	[Gender] [nvarchar](32) NULL,
	[Birthday] [datetime] NULL,
	[ExtendId] [nvarchar](64) NULL,
	[UserImage] [nvarchar](128) NULL,
	[UserStatus] [nvarchar](32) NULL,
	[OrderNo] [int] NULL,
	[CreateBy] [uniqueidentifier] NULL,
	[CreateTime] [datetime] NULL,
	[UpdateBy] [uniqueidentifier] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_SysUser] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysUserOpenId](
	[OpenId] [nvarchar](64) NOT NULL,
	[UserId] [uniqueidentifier] NULL,
	[CreateTime] [datetime] NULL,
	[BindTime] [datetime] NULL,
 CONSTRAINT [PK_SysUserOpenId] PRIMARY KEY CLUSTERED 
(
	[OpenId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysUserClientId](
	[ClientId] [nvarchar](64) NOT NULL,
	[UserId] [uniqueidentifier] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_SysUserClientId] PRIMARY KEY CLUSTERED 
(
	[ClientId] ASC
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
	[Id] [uniqueidentifier] NOT NULL,
	[Mobile] [nvarchar](32) NULL,
	[SmsSign] [nvarchar](32) NULL,
	[SmsCode] [nvarchar](32) NULL,
	[IsVerify] [int] NULL,
	[CreateTime] [datetime] NULL,
	[OverTime] [datetime] NULL,
	[VerifyTime] [datetime] NULL,
 CONSTRAINT [PK_SysSmsCode] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

