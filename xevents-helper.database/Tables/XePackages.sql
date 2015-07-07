CREATE TABLE [dbo].[XePackages]
(
    ReleaseId int not null 
        constraint FK_XePackages_Release_Id foreign key references dbo.Release(Id), 
    [name] NVARCHAR(60) NOT NULL, 
    [guid] UNIQUEIDENTIFIER NOT NULL, 
    [description] NVARCHAR(256) NOT NULL, 
    [capabilities] INT NULL, 
    [capabilities_desc] NVARCHAR(256) NULL, 
    [module_guid] UNIQUEIDENTIFIER NOT NULL, 
    [module_address] VARBINARY(8) NOT NULL,
    constraint PK_XePackages_NameReleaseId primary key (name, ReleaseId, [guid])
);
