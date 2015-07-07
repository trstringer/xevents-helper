CREATE PROCEDURE [dbo].[EventColumnSearchByEvent]
    @ReleaseName varchar(32),
    @EventName nvarchar(128)
AS
    set nocount on;

    select 
	    oc.name,
	    oc.type_name,
	    is_optional = 
		    case
			    when oc.capabilities_desc like '%optional%' then 1
			    else 0
		    end,
	    oc.description
    from dbo.XeObjects o
    inner join dbo.XeObjectColumns oc
    on o.name = oc.object_name
    inner join dbo.Release r
    on o.ReleaseId = r.Id
    and oc.ReleaseId = r.Id
    where r.FriendlyName = @ReleaseName
    and o.object_type = 'event'
    and oc.column_type = 'data'
    and o.name = @EventName
    order by column_id;
go
