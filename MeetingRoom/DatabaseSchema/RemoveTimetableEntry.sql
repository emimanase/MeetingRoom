-----------------------------------------------------
/*
		Insert new entry in Timetable
*/
-----------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[RemoveTimetableEntry]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[RemoveTimetableEntry]
GO

CREATE PROCEDURE [dbo].[RemoveTimetableEntry]

@Id int

AS
	delete from dbo.Timetable where Id = @Id