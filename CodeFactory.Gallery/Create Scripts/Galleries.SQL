/****** Object:  Table [dbo].[Galleries]    Script Date: 11/03/2009 19:53:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Galleries]') AND type in (N'U'))
DROP TABLE [dbo].[Galleries]
GO
/****** Object:  Table [dbo].[Galleries]    Script Date: 11/03/2009 19:53:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Galleries]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Galleries](
	[ApplicationName] [nvarchar](512) NOT NULL,
	[ID] [uniqueidentifier] NOT NULL,
	[Author] [nvarchar](512) NOT NULL,
	[Content] [nvarchar](1024) NULL,
	[DateCreated] [datetime] NOT NULL,
	[Description] [nvarchar](512) NULL,
	[IsVisible] [bit] NOT NULL,
	[Keywords] [nvarchar](1024) NULL,
	[LastUpdated] [datetime] NULL,
	[LastUpdatedBy] [nvarchar](512) NULL,
	[Slug] [nvarchar](512) NULL,
	[Title] [nvarchar](512) NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Galleries] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Trigger [dbo].[Galleries_Version]    Script Date: 11/03/2009 19:53:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[Galleries_Version]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER Galleries_Version
ON dbo.Galleries
AFTER UPDATE
AS
BEGIN
	UPDATE dbo.Galleries
	SET LastUpdated = GETDATE()
	WHERE ID in (
		SELECT ID
		FROM inserted
	);
END;
' 
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Galleries_GalleryID]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Galleries] ADD  CONSTRAINT [DF_Galleries_GalleryID]  DEFAULT (newid()) FOR [ID]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Galleries_DateCreated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Galleries] ADD  CONSTRAINT [DF_Galleries_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Galleries_IsVisible]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Galleries] ADD  CONSTRAINT [DF_Galleries_IsVisible]  DEFAULT ((0)) FOR [IsVisible]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Galleries_LastUpdate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Galleries] ADD  CONSTRAINT [DF_Galleries_LastUpdate]  DEFAULT (getdate()) FOR [LastUpdated]
END

GO
