CREATE TABLE [dbo].[Release]
(
    [Id] INT identity(1, 1) NOT NULL 
        constraint PK_Release_Id PRIMARY KEY clustered,
    [Major] int not null,
    [Minor] int not null,
    [FriendlyName] varchar(32) not null,
    constraint UK_Release_MajorMinor unique (Major, Minor)
)
