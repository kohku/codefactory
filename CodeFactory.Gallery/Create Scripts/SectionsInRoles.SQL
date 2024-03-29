/****** Object:  Table [dbo].[SectionsInRoles]    Script Date: 02/17/2010 23:31:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SectionsInRoles]') AND type in (N'U'))
DROP TABLE [dbo].[SectionsInRoles]
GO
/****** Object:  Table [dbo].[SectionsInRoles]    Script Date: 02/17/2010 23:31:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SectionsInRoles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SectionsInRoles](
	[sectionId] [uniqueidentifier] NOT NULL,
	[roleName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_SectionInRoles] PRIMARY KEY CLUSTERED 
(
	[sectionId] ASC,
	[roleName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SectionInRoles_Section]') AND parent_object_id = OBJECT_ID(N'[dbo].[SectionsInRoles]'))
ALTER TABLE [dbo].[SectionsInRoles]  WITH CHECK ADD  CONSTRAINT [FK_SectionInRoles_Section] FOREIGN KEY([sectionId])
REFERENCES [dbo].[Sections] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SectionInRoles_Section]') AND parent_object_id = OBJECT_ID(N'[dbo].[SectionsInRoles]'))
ALTER TABLE [dbo].[SectionsInRoles] CHECK CONSTRAINT [FK_SectionInRoles_Section]
GO
