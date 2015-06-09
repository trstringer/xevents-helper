CREATE PROCEDURE [dbo].[EventGetDescription]
    @ReleaseName varchar(32),
    @EventName nvarchar(128)
AS
    select o.description
    from dbo.XeObjects o
    inner join dbo.Release r
    on o.ReleaseId = r.Id
    where r.FriendlyName = @ReleaseName
    and o.object_type = 'event'
    and o.name = @EventName;
go