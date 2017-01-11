CREATE TABLE [dbo].[Credentials] (
    [Id]           INT            PRIMARY KEY	IDENTITY (1, 1) NOT NULL,
    [PasswordHash] NVARCHAR (128) NOT NULL,
    [Salt]         NVARCHAR (32)  NOT NULL,
);

