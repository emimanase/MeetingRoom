-----------------------------------------------------
/*
		Retrieve employee by cnp
*/
-----------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[GetEmployeeByCNP]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetEmployeeByCNP]
GO

CREATE PROCEDURE [dbo].[GetEmployeeByCNP]

@CNP varchar(13)

AS

	Select * from [MeetingRoomDB].dbo.Employee where CNP = @CNP