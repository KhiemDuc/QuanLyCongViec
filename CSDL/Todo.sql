/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2019 (15.0.2000)
    Source Database Engine Edition : Microsoft SQL Server Express Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2019
    Target Database Engine Edition : Microsoft SQL Server Express Edition
    Target Database Engine Type : Standalone SQL Server
*/
USE [Todo]
GO
/****** Object:  Table [dbo].[tGroup]    Script Date: 5/17/2023 1:08:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tGroup](
	[group_id] [int] IDENTITY(1,1) NOT NULL,
	[group_name] [nvarchar](50) NOT NULL,
	[user_id] [int] NOT NULL,
 CONSTRAINT [PK_tGroup_1] PRIMARY KEY CLUSTERED 
(
	[group_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tInfoUser]    Script Date: 5/17/2023 1:08:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tInfoUser](
	[user_id] [int] NOT NULL,
	[fullname] [nvarchar](30) NOT NULL,
	[date_of_birth] [date] NOT NULL,
	[sex] [bit] NOT NULL,
	[nickname] [nchar](10) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tRoles]    Script Date: 5/17/2023 1:08:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tRoles](
	[id_role] [int] IDENTITY(1,1) NOT NULL,
	[roll_name] [nchar](10) NOT NULL,
 CONSTRAINT [PK_tRolls] PRIMARY KEY CLUSTERED 
(
	[id_role] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tTasks]    Script Date: 5/17/2023 1:08:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tTasks](
	[task_id] [int] IDENTITY(1,1) NOT NULL,
	[group_id] [int] NOT NULL,
	[task_name] [nvarchar](20) NOT NULL,
	[task_des] [nvarchar](50) NOT NULL,
	[start_date] [datetime] NOT NULL,
	[end_date] [datetime] NOT NULL,
	[task_status] [nvarchar](15) NOT NULL,
	[user_id] [int] NOT NULL,
	[date_done] [datetime] NULL,
 CONSTRAINT [PK_tTasks] PRIMARY KEY CLUSTERED 
(
	[task_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tUser]    Script Date: 5/17/2023 1:08:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tUser](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](20) NOT NULL,
	[password] [varchar](30) NOT NULL,
	[email] [varchar](30) NOT NULL,
 CONSTRAINT [PK_tUser_1] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tGroup]  WITH CHECK ADD  CONSTRAINT [FK_tGroup_tUser] FOREIGN KEY([user_id])
REFERENCES [dbo].[tUser] ([user_id])
GO
ALTER TABLE [dbo].[tGroup] CHECK CONSTRAINT [FK_tGroup_tUser]
GO
ALTER TABLE [dbo].[tTasks]  WITH CHECK ADD  CONSTRAINT [FK_tTasks_tGroup] FOREIGN KEY([group_id])
REFERENCES [dbo].[tGroup] ([group_id])
GO
ALTER TABLE [dbo].[tTasks] CHECK CONSTRAINT [FK_tTasks_tGroup]
GO
ALTER TABLE [dbo].[tTasks]  WITH CHECK ADD  CONSTRAINT [FK_tTasks_tUser] FOREIGN KEY([user_id])
REFERENCES [dbo].[tUser] ([user_id])
GO
ALTER TABLE [dbo].[tTasks] CHECK CONSTRAINT [FK_tTasks_tUser]
GO
