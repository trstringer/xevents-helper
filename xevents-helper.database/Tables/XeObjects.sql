CREATE TABLE [dbo].[XeObjects]
(
    ReleaseId int not null 
        constraint FK_XeObjects_Release_Id foreign key references dbo.Release(Id),
    name nvarchar(128) NOT NULL,
    object_type nvarchar(128) not null,
    package_guid uniqueidentifier not null,
    description nvarchar(512) not null,
    capabilities int null,
    capabilities_desc nvarchar(512) null,
    type_name nvarchar(128) null,
    type_package_guid uniqueidentifier null,
    type_size int null
);
