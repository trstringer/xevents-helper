CREATE PROCEDURE [dbo].[EventSearchByDescription]
    @ReleaseName varchar(32),
    @SearchString varchar(512)
AS
    select o.name, o.description
    from dbo.XeObjects o
    inner join dbo.Release r
    on o.ReleaseId = r.Id
    where r.FriendlyName= @ReleaseName
    and o.object_type = 'event'
    and o.description like '%' + @SearchString + '%'
    order by o.name;
go