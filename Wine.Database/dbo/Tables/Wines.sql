CREATE TABLE [dbo].[Wines] (
    [Id]          INT             PRIMARY KEY		IDENTITY (1, 1) NOT NULL,
    [Region]      NVARCHAR (128)  NOT NULL,
    [Thumbnail]   VARBINARY (MAX) NOT NULL,
    [Name]        NVARCHAR (256)  NOT NULL,
    [Category]    TINYINT         NOT NULL,
    [Varietal]    TINYINT         NOT NULL,
    [Description] NVARCHAR (2048) NOT NULL,
    [Vintage]     INT             NOT NULL,
	[Search] AS [Name] + ' ' +
	  CASE 
		WHEN [Category] = 0 THEN 'Dry'
		WHEN [Category] = 1 THEN 'Semi Dry'
		WHEN [Category] = 2 THEN 'Sweet'
	  END + ' ' +
	  CASE 
		WHEN [Varietal] = 0 THEN 'Red'
		WHEN [Varietal] = 1 THEN 'White'
		WHEN [Varietal] = 2 THEN 'Sparkling'
		WHEN [Varietal] = 3 THEN 'Rose'
	  END + ' ' + CAST([Vintage] AS VARCHAR)
);

