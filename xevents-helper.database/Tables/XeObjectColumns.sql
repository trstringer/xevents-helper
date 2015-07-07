CREATE TABLE [dbo].[XeObjectColumns]
(
    ReleaseId int not null 
        constraint FK_XeObjectColumns_Release_Id foreign key references dbo.Release(Id), 
    [name] NVARCHAR(60) NOT NULL, 
    [column_id] INT NOT NULL, 
    [object_name] NVARCHAR(60) NOT NULL, 
    [object_package_guid] UNIQUEIDENTIFIER NOT NULL, 
    [type_name] NVARCHAR(60) NOT NULL, 
    [type_package_guid] UNIQUEIDENTIFIER NOT NULL, 
    [column_type] NVARCHAR(60) NOT NULL, 
    [column_value] NVARCHAR(256) NULL, 
    [capabilities] INT NULL, 
    [capabilities_desc] NVARCHAR(256) NULL, 
    [description] NVARCHAR(384) NULL,
    constraint PK_XeObjectColumns_NameReleaseIdObjectNameType primary key (name, ReleaseId, [object_name], column_type)
);
