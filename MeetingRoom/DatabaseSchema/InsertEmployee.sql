-----------------------------------------------------
/*
		Insert employee
*/
-----------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[InsertEmployee]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[InsertEmployee]
GO

CREATE PROCEDURE [dbo].[InsertEmployee]

@CNP varchar(13),
@FirstName varchar(50),
@LastName varchar(50),
@Role varchar(25),
@Rooms varchar(100)

AS
	insert into dbo.Employee(CNP, FirstName, LastName, [Role], AvailableRooms) values (@CNP, @FirstName, @LastName, @Role, @Rooms);