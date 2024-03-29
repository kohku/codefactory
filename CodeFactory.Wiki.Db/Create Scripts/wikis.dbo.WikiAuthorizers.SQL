/****** Object:  Table [dbo].[WikiAuthorizers]    Script Date: 11/17/2009 11:10:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WikiAuthorizers]') AND type in (N'U'))
DROP TABLE [dbo].[WikiAuthorizers]
GO
/****** Object:  Table [dbo].[WikiAuthorizers]    Script Date: 11/17/2009 11:10:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WikiAuthorizers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[WikiAuthorizers](
	[Category] [varchar](50) NOT NULL,
	[UserName] [nvarchar](512) NOT NULL,
	[applicationName] [varchar](512) NULL,
 CONSTRAINT [PK_WikiAuthorizers] PRIMARY KEY CLUSTERED 
(
	[Category] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
