CREATE PROCEDURE [dbo].[ReleaseGet]
    @FriendlyName varchar(32) = null
AS
    select FriendlyName
    from dbo.Release
    where
    (
        @FriendlyName is null
        or FriendlyName = @FriendlyName
    );
go