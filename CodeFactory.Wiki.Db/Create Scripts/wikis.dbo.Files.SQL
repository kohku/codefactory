/****** Object:  Table [dbo].[Files]    Script Date: 11/17/2009 11:11:12 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Files]') AND type in (N'U'))
DROP TABLE [dbo].[Files]
GO
/****** Object:  Table [dbo].[Files]    Script Date: 11/17/2009 11:11:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Files]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Files](
	[id] [uniqueidentifier] NOT NULL,
	[applicationName] [varchar](512) NOT NULL,
	[foreignId] [uniqueidentifier] NOT NULL,
	[fileName] [varchar](1024) NOT NULL,
	[contentType] [varchar](50) NOT NULL,
	[contentLength] [int] NOT NULL,
	[data] [varbinary](max) NOT NULL,
	[dateCreated] [datetime] NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
