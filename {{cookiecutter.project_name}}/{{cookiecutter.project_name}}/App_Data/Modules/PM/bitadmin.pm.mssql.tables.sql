--项目管理模块：项目导航、新建项目、项目计划、项目执行、项目流程、项目交流

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PmProject](
	[ProjectId] [uniqueidentifier] NOT NULL,
	[ProjectCode] [nvarchar](32) NULL,
	[ProjectType] [nvarchar](16) NULL,
	[ProjectProperty] [nvarchar](16) NULL,
	[ProjectName] [nvarchar](64) NULL,
	[ProjectContent] [text] NULL,
	[ProjectManager] [uniqueidentifier] NULL,
	[ProjectManagerMobile] [nvarchar](16) NULL,
	[ProjectStatus] [nvarchar](16) NULL,
	[PlanStartDate] [datetime] NULL,
	[PlanEndDate] [datetime] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Remark] [text] NULL,
	[CreateBy] [uniqueidentifier] NULL,
	[CreateTime] [datetime] NULL,
	[UpdateBy] [uniqueidentifier] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_PmProject] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PmProjectUser](
	[ProjectId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[UserRole] [nvarchar](16) NULL,
 CONSTRAINT [PK_PmProjectUser] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PmTask](
	[TaskId] [uniqueidentifier] NOT NULL,
	[ProjectId] [uniqueidentifier] NULL,
	[ParentId] [uniqueidentifier] NULL,
	[TaskType] [nvarchar](16) NULL,
	[TaskName] [nvarchar](64) NULL,
	[TaskContent] [text] NULL,
	[TaskManager] [nvarchar](64) NULL,
	[TaskStatus] [nvarchar](16) NULL,
	[PlanStartDate] [datetime] NULL,
	[PlanEndDate] [datetime] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Remark] [text] NULL,
	[CreateBy] [uniqueidentifier] NULL,
	[CreateTime] [datetime] NULL,
	[UpdateBy] [uniqueidentifier] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_PmTask] PRIMARY KEY CLUSTERED 
(
	[TaskId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PmTaskUser](
	[TaskId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[UserRole] [nvarchar](16) NULL,
 CONSTRAINT [PK_TaskUser] PRIMARY KEY CLUSTERED 
(
	[TaskId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
