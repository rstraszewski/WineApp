CREATE PROCEDURE [dbo].[GetWineList]
	-- Add the parameters for the stored procedure here
	@page INT,
	@pageSize INT,
	@filterCriteria VARCHAR(MAX) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SELECT * FROM Wines
	WHERE @filterCriteria IS NULL OR [Wines].[Search] LIKE '%' + @filterCriteria + '%'
	ORDER BY [Id]
	OFFSET     @pageSize * @page ROWS       -- skip 10 rows
	FETCH NEXT @pageSize ROWS ONLY; -- take 10 rows
END