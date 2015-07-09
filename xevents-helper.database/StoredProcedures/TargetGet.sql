CREATE PROCEDURE [dbo].[TargetGet]
    @ReleaseName varchar(32)
AS
    set nocount on;

    select
	    o.name,
	    package_name = p.name
    from dbo.XeObjects o
    inner join dbo.Release r
    on o.ReleaseId = r.Id
    inner join dbo.XePackages p
    on o.package_guid = p.guid
    and o.ReleaseId = p.ReleaseId
    where o.object_type = 'target'
    and r.FriendlyName = @ReleaseName
    and
    (
	    o.capabilities_desc not like '%private%'
	    or o.capabilities_desc is null
    )
    order by o.name;
go