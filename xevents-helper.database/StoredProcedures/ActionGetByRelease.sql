CREATE PROCEDURE [dbo].[ActionGetByRelease]
    @ReleaseName varchar(32)
AS
    select 
		o.name,
		package_name = p.name,
		o.type_name
    from dbo.XeObjects o
    inner join dbo.Release r
    on o.ReleaseId = r.Id
	inner join dbo.XePackages p
	on o.package_guid = p.guid
	and o.ReleaseId = p.ReleaseId
    where r.FriendlyName = @ReleaseName
    and o.object_type = 'action'
    order by o.name;
go