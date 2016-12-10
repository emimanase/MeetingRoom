-----------------------------------------------------
/*
		Update an entry in Timetable
*/
-----------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[UpdateTimetableEntry]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdateTimetableEntry]
GO

CREATE PROCEDURE [dbo].[UpdateTimetableEntry]

@Id int,
@From datetime,
@To datetime,
@RoomId int,
@CNP varchar(13)

AS
	update dbo.Timetable set [From] = @From, [To] = @To, RoomId = @RoomId, EmployeeCNP = @CNP 
	where Id = @Id