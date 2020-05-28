CREATE PROCEDURE [dbo].[Sp_GetAllUsersByRole]
	@RoleType VARCHAR(150),
	@PageIndex [INT] = 1,
	@PageSize [INT] = 10
AS

BEGIN
	WITH
		UserList
		AS
		(
			SELECT u.Id, u.LastName, u.FirstName, u.MiddleName, u.UserName, u.Email, u.CreatedOnUtc,
				u.Activated, u.UserType, 
				R.Name as RoleName, R.Id as RoleId,
				COUNT(*) OVER () TotalCount,
				ROW_NUMBER() OVER(ORDER BY U.Id DESC) as RowNo
			FROM [dbo].[DafmisUser] u 
			LEFT JOIN [dbo].[DafmisUserRole] ur
			ON  U.Id = Ur.UserId
			LEFT JOIN [dbo].[DafmisRole] R 
			on UR.RoleId = R.Id
			WHERE (@RoleType IS NULL OR (r.Name  like '%'+ @RoleType + '%'))
		),
		Counts
		AS
		(
			SELECT Count(*) TotalCount
			FROM UserList
		)
	SELECT u.*,
		ROW_NUMBER() OVER(ORDER BY u.Id DESC) as RowNo,
		c.TotalCount
	FROM UserList u, Counts c
	ORDER BY u.CreatedOnUtc DESC
    OFFSET @PageIndex ROWS FETCH NEXT @PageSize ROWS ONLY
END

