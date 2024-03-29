/****** Object:  Table [dbo].[Sections]    Script Date: 02/17/2010 23:31:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sections]') AND type in (N'U'))
DROP TABLE [dbo].[Sections]
GO
/****** Object:  Table [dbo].[Sections]    Script Date: 02/17/2010 23:31:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sections]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Sections](
	[ID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](512) NOT NULL,
	[Slug] [nvarchar](512) NOT NULL,
	[ApplicationName] [nvarchar](512) NOT NULL,
	[ParentID] [uniqueidentifier] NULL,
	[DateCreated] [datetime] NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
	[Index] [int] NOT NULL,
	[IsVisible] [bit] NOT NULL,
 CONSTRAINT [PK_Sections] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Trigger [dbo].[Sections_Version]    Script Date: 02/17/2010 23:31:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[Sections_Version]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER Sections_Version
ON dbo.Sections
AFTER UPDATE
AS
BEGIN
	UPDATE dbo.Sections
	SET LastUpdated = GETDATE()
	WHERE ID in (
		SELECT ID
		FROM inserted
	);
END;
' 
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Sections_DateCreated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Sections] ADD  CONSTRAINT [DF_Sections_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Sections_LastUpdated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Sections] ADD  CONSTRAINT [DF_Sections_LastUpdated]  DEFAULT (getdate()) FOR [LastUpdated]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Sections_Index]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Sections] ADD  CONSTRAINT [DF_Sections_Index]  DEFAULT ((0)) FOR [Index]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Sections_IsVisivle]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Sections] ADD  CONSTRAINT [DF_Sections_IsVisivle]  DEFAULT ((0)) FOR [IsVisible]
END

GO
