SET IDENTITY_INSERT [dbo].[Roles] ON
MERGE INTO [dbo].[Roles] AS Target 
USING (
VALUES	(1, 'Administrator'),
		(2, 'Reviewer'),
		(3, 'WineManager'),
		(4, 'UserManager'),
		(5, 'Viewer')
) 
AS Source (Id, Name) 
ON Target.Id = Source.Id 

-- update matched rows 

WHEN MATCHED THEN 
UPDATE SET Name = Source.Name 

-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (Id, Name) 
VALUES (Id, Name) 

-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN DELETE;
SET IDENTITY_INSERT [dbo].[Roles] OFF;
