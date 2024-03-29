/****** Object:  StoredProcedure [dbo].[ReporteEstadistico]    Script Date: 11/17/2009 11:11:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReporteEstadistico]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ReporteEstadistico]
GO
/****** Object:  StoredProcedure [dbo].[ReporteEstadistico]    Script Date: 11/17/2009 11:11:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReporteEstadistico]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'







CREATE procedure [dbo].[ReporteEstadistico]
	@fechaInicio datetime = ''1900-01-01'',
	@fechaFin datetime = ''9999-12-31'', 
	@title VarChar(512),
	@urlRequested VarChar(1024),
	@id uniqueIdentifier,
	@type VarChar(512), 
	@username VarChar(50),
	@applicationName VarChar(512),
	@firstIndex int, 
	@lastIndex int,
	@totalCount int output
as

set nocount on;

-- Begin the transaction
begin transaction

if @fechaInicio is null
	set @fechaInicio = getdate() - getdate()

if @fechaFin is null
	set @fechaFin = getdate()

create table #reporteEstadistico(rowNumber int, fecha datetime, title VarChar(512), urlRequested VarChar(1024),
	id uniqueIdentifier, [type] VarChar(512), username VarChar(512), hits int)

declare @rowNumber int, @_fecha datetime, @_title VarChar(512), @_urlRequested VarChar(1024), @_id uniqueIdentifier, 
	@_type Varchar(512), @_username VarChar(512), @_hits int

set @rowNumber = 0

declare reporteEstadistico cursor fast_forward for
select fecha, title, urlRequested, id, [type], username, count(*) hits
from (
	select cast(cast(year(s.[timestamp]) as varchar) + ''-'' + cast(month(s.[timestamp]) as varchar) +
		''-'' + cast(day(s.[timestamp]) as varchar) as datetime) fecha, title, urlRequested, id, [type], username
	from [statistics] s
	where s.[timestamp] >= @fechaInicio and s.[timestamp] < @fechaFin
		and s.title = isnull(@title, s.title)
		and s.urlRequested = isnull(@urlRequested, s.urlRequested)
		and s.id = isnull(@id, s.id)
		and s.[type] = isnull(@type, s.[type])
		and s.username = isnull(@username, s.username)
		and s.applicationName = @applicationName
) grouped
group by fecha, title, urlRequested, id, [type], username
order by fecha, title, urlRequested, id, [type], username;

open reporteEstadistico;

fetch next from reporteEstadistico
into @_fecha, @_title, @_urlRequested, @_id, @_type, @_username, @_hits;

while @@fetch_status = 0
begin
	set @rowNumber = @rowNumber + 1

	insert into #reporteEstadistico values (@rowNumber, @_fecha, @_title, @_urlRequested, @_id, @_type, @_username, @_hits);

	fetch next from reporteEstadistico
	into @_fecha, @_title, @_urlRequested, @_id, @_type, @_username, @_hits;
end

close reporteEstadistico;
deallocate reporteEstadistico;

declare @statementCount nvarchar(1024), @statement nvarchar(1024),
	@parameterDefinition nvarchar(512), @parameterDefinitionCount nvarchar(512)

set @statement =	''select fecha, title, urlRequested, id, [type], username, sum(hits) hits
					from #reporteEstadistico 
					where 1 = 1 
						and title = isnull(@title, title)
						and urlRequested = isnull(@urlRequested, urlRequested)
						and id = isnull(@id, id)
						and [type] = isnull(@type, [type])
						and username = isnull(@username, username)'';

set @statementCount = ''select @totalCount = count(*) from ('' + @statement;
set @statementCount = @statementCount + ''group by fecha, title, urlRequested, id, [type], username'';
set @statementCount = @statementCount  + '') total'';

set @statement = @statement + ''and rowNumber >= @firstIndex and rowNumber <= @lastIndex '';
set @statement = @statement + ''group by fecha, title, urlRequested, id, [type], username'';

set @parameterDefinition = ''@title VarChar(512), @urlRequested VarChar(1024), @id uniqueIdentifier, @type VarChar(512), @username VarChar(512), @firstIndex int, @lastIndex int'';
set @parameterDefinitionCount = @parameterDefinition + '', @totalCount int output'';

execute sp_executesql @statementCount, @parameterDefinitionCount,
	@title = @title, @urlRequested = @urlRequested, @id = @id, @type = @type, @username = @username, 
	@firstIndex = @firstIndex, @lastIndex = @lastIndex,	@totalCount = @totalCount output;

execute sp_executesql @statement, @parameterDefinition,
	@title = @title, @urlRequested = @urlRequested, @id = @id, @type = @type, @username = @username, 
	@firstIndex = @firstIndex, @lastIndex = @lastIndex;

drop table #reporteEstadistico

-- Commit the Transaction
commit transaction

set nocount off;















' 
END
GO
