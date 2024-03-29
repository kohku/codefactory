IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Modules_DateCreated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Modules] DROP CONSTRAINT [DF_Modules_DateCreated]
END

GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Modules_LastUpdated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Modules] DROP CONSTRAINT [DF_Modules_LastUpdated]
END

GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Pages_IsVisible]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Pages] DROP CONSTRAINT [DF_Pages_IsVisible]
END

GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Pages_DateCreated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Pages] DROP CONSTRAINT [DF_Pages_DateCreated]
END

GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Pages_LastUpdate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Pages] DROP CONSTRAINT [DF_Pages_LastUpdate]
END

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PagesInRoles_Pages]') AND parent_object_id = OBJECT_ID(N'[dbo].[PagesInRoles]'))
ALTER TABLE [dbo].[PagesInRoles] DROP CONSTRAINT [FK_PagesInRoles_Pages]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Publications_DateCreated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Publications] DROP CONSTRAINT [DF_Publications_DateCreated]
END

GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Publications_LastUpdated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Publications] DROP CONSTRAINT [DF_Publications_LastUpdated]
END

GO
/****** Object:  Table [dbo].[Modules]    Script Date: 02/06/2010 13:11:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Modules]') AND type in (N'U'))
DROP TABLE [dbo].[Modules]
GO
/****** Object:  Table [dbo].[Pages]    Script Date: 02/06/2010 13:11:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Pages]') AND type in (N'U'))
DROP TABLE [dbo].[Pages]
GO
/****** Object:  Table [dbo].[PagesInRoles]    Script Date: 02/06/2010 13:11:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PagesInRoles]') AND type in (N'U'))
DROP TABLE [dbo].[PagesInRoles]
GO
/****** Object:  Table [dbo].[Publications]    Script Date: 02/06/2010 13:11:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Publications]') AND type in (N'U'))
DROP TABLE [dbo].[Publications]
GO
/****** Object:  Table [dbo].[Modules]    Script Date: 02/06/2010 13:11:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Modules]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Modules](
	[ID] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](512) NOT NULL,
	[Content] [varbinary](max) NOT NULL,
	[ApplicationName] [nvarchar](512) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
 CONSTRAINT [PK_Modules] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

/****** Object:  Trigger [dbo].[Modules_Version]    Script Date: 02/06/2010 13:11:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[Modules_Version]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER Modules_Version
ON dbo.Modules
AFTER UPDATE
AS
BEGIN
	UPDATE dbo.Modules
	SET LastUpdated = GETDATE()
	WHERE ID in (
		SELECT ID
		FROM inserted
	);
END;
' 
GO
/****** Object:  Table [dbo].[Pages]    Script Date: 02/06/2010 13:11:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Pages]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Pages](
	[ID] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](512) NOT NULL,
	[Slug] [nvarchar](512) NULL,
	[Description] [nvarchar](512) NULL,
	[Keywords] [nvarchar](1024) NULL,
	[Layout] [nvarchar](512) NOT NULL,
	[ApplicationName] [nvarchar](512) NOT NULL,
	[BackgroundColor] [char](7) NULL,
	[BackgroundImage] [nvarchar](1024) NULL,
	[ParentID] [uniqueidentifier] NULL,
	[IsVisible] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
	[Author] [nvarchar](512) NOT NULL,
 CONSTRAINT [PK_Pages] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

/****** Object:  Trigger [dbo].[Pages_Version]    Script Date: 02/06/2010 13:11:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[Pages_Version]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER Pages_Version
ON dbo.Pages
AFTER UPDATE
AS
BEGIN
	UPDATE dbo.Pages
	SET LastUpdated = GETDATE()
	WHERE ID in (
		SELECT ID
		FROM inserted
	);
END;
' 
GO
/****** Object:  Table [dbo].[PagesInRoles]    Script Date: 02/06/2010 13:11:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PagesInRoles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PagesInRoles](
	[pageId] [uniqueidentifier] NOT NULL,
	[roleName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_PagesInRoles] PRIMARY KEY CLUSTERED 
(
	[pageId] ASC,
	[roleName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Publications]    Script Date: 02/06/2010 13:11:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Publications]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Publications](
	[ID] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](512) NOT NULL,
	[Description] [nvarchar](1024) NULL,
	[Content] [nvarchar](max) NOT NULL,
	[Category] [nvarchar](512) NULL,
	[DateCreated] [datetime] NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
	[Author] [nvarchar](512) NOT NULL,
	[ApplicationName] [nvarchar](512) NOT NULL,
	[IsVisible] [bit] NOT NULL,
	[RelativeLink] [nvarchar](1024) NOT NULL,
 CONSTRAINT [PK_Publications] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Modules_DateCreated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Modules] ADD  CONSTRAINT [DF_Modules_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Modules_LastUpdated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Modules] ADD  CONSTRAINT [DF_Modules_LastUpdated]  DEFAULT (getdate()) FOR [LastUpdated]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Pages_IsVisible]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Pages] ADD  CONSTRAINT [DF_Pages_IsVisible]  DEFAULT ((0)) FOR [IsVisible]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Pages_DateCreated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Pages] ADD  CONSTRAINT [DF_Pages_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Pages_LastUpdate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Pages] ADD  CONSTRAINT [DF_Pages_LastUpdate]  DEFAULT (getdate()) FOR [LastUpdated]
END

GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PagesInRoles_Pages]') AND parent_object_id = OBJECT_ID(N'[dbo].[PagesInRoles]'))
ALTER TABLE [dbo].[PagesInRoles]  WITH CHECK ADD  CONSTRAINT [FK_PagesInRoles_Pages] FOREIGN KEY([pageId])
REFERENCES [dbo].[Pages] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PagesInRoles_Pages]') AND parent_object_id = OBJECT_ID(N'[dbo].[PagesInRoles]'))
ALTER TABLE [dbo].[PagesInRoles] CHECK CONSTRAINT [FK_PagesInRoles_Pages]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Publications_DateCreated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Publications] ADD  CONSTRAINT [DF_Publications_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Publications_LastUpdated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Publications] ADD  CONSTRAINT [DF_Publications_LastUpdated]  DEFAULT (getdate()) FOR [LastUpdated]
END

GO
