CREATE TABLE [dbo].[Wines] (
    [Id]          INT             PRIMARY KEY	IDENTITY (1, 1) NOT NULL,
    [Region]      NVARCHAR (128)  NOT NULL,
    [Thumbnail]   VARBINARY (MAX) NOT NULL,
    [Name]        NVARCHAR (256)  NOT NULL,
    [Category]    TINYINT         NOT NULL,
    [Varietal]    TINYINT         NOT NULL,
    [Description] NVARCHAR (2048) NOT NULL,
    [Vintage]     INT             NOT NULL
);

