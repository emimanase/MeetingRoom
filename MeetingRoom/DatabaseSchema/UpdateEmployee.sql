-----------------------------------------------------
/*
		Update employee
*/
-----------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[UpdateEmployee]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdateEmployee]
GO

CREATE PROCEDURE [dbo].[UpdateEmployee]

@CNP varchar(13),
@FirstName varchar(50),
@LastName varchar(50),
@Role varchar(25),
@Rooms varchar(100)

AS
	update dbo.Employee set FirstName = @FirstName, LastName = @LastName, [Role] = @Role, AvailableRooms = @Rooms
	where CNP = @CNP