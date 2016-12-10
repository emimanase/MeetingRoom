-----------------------------------------------------
/*
		Remove employee
*/
-----------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[RemoveEmployee]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[RemoveEmployee]
GO

CREATE PROCEDURE [dbo].[RemoveEmployee]

@CNP varchar(13)

AS
	delete from dbo.Employee where CNP = @CNP