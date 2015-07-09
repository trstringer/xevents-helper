CREATE PROCEDURE [dbo].[TargetParameterGet]
    @ReleaseName varchar(32),
    @TargetName nvarchar(128)
AS
    set nocount on;

    select
	    oc.name,
	    oc.type_name,
	    is_mandatory = 
		    case
			    when oc.capabilities_desc = 'mandatory' then 1
			    else 0
		    end,
	    oc.description
    from dbo.XeObjects o
    inner join dbo.Release r
    on o.ReleaseId = r.Id
    inner join dbo.XePackages p
    on o.package_guid = p.guid
    and o.ReleaseId = p.ReleaseId
    inner join dbo.XeObjectColumns oc
    on o.name = oc.object_name
    and o.ReleaseId = oc.ReleaseId
    where o.object_type = 'target'
    and r.FriendlyName = @ReleaseName
    and
    (
	    o.capabilities_desc not like '%private%'
	    or o.capabilities_desc is null
    )
    and
    (
	    oc.column_type = 'customizable'
	    or oc.column_type is null
    )
    and o.name = @TargetName
    order by o.name;
go
