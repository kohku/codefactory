/****** Object:  StoredProcedure [dbo].[UploadStorage_GetFiles]    Script Date: 11/03/2009 20:34:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UploadStorage_GetFiles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UploadStorage_GetFiles]
GO
/****** Object:  StoredProcedure [dbo].[UploadStorage_GetFiles]    Script Date: 11/03/2009 20:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UploadStorage_GetFiles]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
Create Procedure [dbo].[UploadStorage_GetFiles]

@ApplicationName NVarChar(512),
@ID UniqueIdentifier = NULL,
@ContentType NVarChar(50) = NULL,
@InitialDateCreated DateTime = NULL,
@FinalDateCreated DateTime = NULL,
@FileName NVarChar(1024) = NULL,
@InitialLastUpdated DateTime = NULL,
@FinalLastUpdated DateTime = NULL,
@ParentId UniqueIdentifier = NULL,
@FirstIndex Int,
@LastIndex Int,
@TotalCount Int Output

AS

SELECT @TotalCount = COUNT(id)
FROM UploadedFiles
WHERE ApplicationName = @ApplicationName
	AND ID = ISNULL(@ID, ID)
	AND ContentType = ISNULL(@ContentType, ContentType)
	AND DateCreated BETWEEN ISNULL(@InitialDateCreated, DateCreated) AND ISNULL(@FinalDateCreated, DateCreated)
	AND FileName = ISNULL(@FileName, FileName)
	AND LastUpdated BETWEEN ISNULL(@InitialLastUpdated, LastUpdated) AND ISNULL(@FinalLastUpdated, LastUpdated)
	AND ParentId = ISNULL(@ParentId, ParentId);

WITH Ordered(ID, RowNumber) AS (
	SELECT ID, ROW_NUMBER() OVER (ORDER BY fileName ASC) RowNumber
	FROM UploadedFiles
	WHERE ApplicationName = @ApplicationName
		AND ID = ISNULL(@ID, ID)
		AND ContentType = ISNULL(@ContentType, ContentType)
		AND DateCreated BETWEEN ISNULL(@InitialDateCreated, DateCreated) AND ISNULL(@FinalDateCreated, DateCreated)
		AND FileName = ISNULL(@FileName, FileName)
		AND LastUpdated BETWEEN ISNULL(@InitialLastUpdated, LastUpdated) AND ISNULL(@FinalLastUpdated, LastUpdated)
		AND ParentId = ISNULL(@ParentId, ParentId)
)
SELECT ID
FROM Ordered
WHERE RowNumber BETWEEN @FirstIndex AND @LastIndex;	

' 
END
GO
