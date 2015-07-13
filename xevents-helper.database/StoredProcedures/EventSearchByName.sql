CREATE PROCEDURE [dbo].[EventSearchByName]
    @ReleaseName varchar(32),
    @SearchString varchar(512) = null
AS
    select 
		o.name, 
		package_name = p.name,
		o.description
    from dbo.XeObjects o
    inner join dbo.Release r
    on o.ReleaseId = r.Id
	inner join dbo.XePackages p
	on o.package_guid = p.guid
	and o.ReleaseId = p.ReleaseId
    where r.FriendlyName= @ReleaseName
    and o.object_type = 'event'
    and 
    (
        @SearchString is null
        or o.name like '%' + @SearchString + '%'
    )
    order by o.name;
go