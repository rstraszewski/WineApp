CREATE TABLE [dbo].[Reviews] (
    [Id]       INT             PRIMARY KEY		IDENTITY (1, 1) NOT NULL,
    [WineId]   INT             NOT NULL,
    [Body]     NVARCHAR (2048) NOT NULL,
    [UserName] NVARCHAR (128)  NOT NULL,
    [Created]  DATETIME        NOT NULL
    CONSTRAINT [FK_dbo.Reviews_dbo.Wines_WineId] FOREIGN KEY ([WineId]) REFERENCES [dbo].[Wines] ([Id]) ON DELETE CASCADE
);

