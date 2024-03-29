/****** Object:  Table [dbo].[Pages]    Script Date: 02/17/2010 23:30:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Pages]') AND type in (N'U'))
DROP TABLE [dbo].[Pages]
GO
/****** Object:  Table [dbo].[Pages]    Script Date: 02/17/2010 23:30:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
	[ParentID] [uniqueidentifier] NULL,
	[IsVisible] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
	[Author] [nvarchar](512) NOT NULL,
	[LastUpdatedBy] [nvarchar](512) NULL,
 CONSTRAINT [PK_Pages] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Trigger [dbo].[Pages_Version]    Script Date: 02/17/2010 23:30:58 ******/
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
