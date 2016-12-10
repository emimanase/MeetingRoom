-----------------------------------------------------
/*
		Retrieve employees
*/
-----------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[GetAllEmployees]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetAllEmployees]
GO

CREATE PROCEDURE [dbo].[GetAllEmployees]
AS

	Select * from [MeetingRoomDB].dbo.Employee