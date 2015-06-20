CREATE PROCEDURE [dbo].[ActionGetByRelease]
    @ReleaseName varchar(32)
AS
    select name
    from dbo.XeObjects o
    inner join dbo.Release r
    on o.ReleaseId = r.Id
    where r.FriendlyName = @ReleaseName
    and o.object_type = 'action'
    order by name;
go