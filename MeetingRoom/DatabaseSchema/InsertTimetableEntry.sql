-----------------------------------------------------
/*
		Insert new entry in Timetable
*/
-----------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[InsertTimetableEntry]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[InsertTimetableEntry]
GO

CREATE PROCEDURE [dbo].[InsertTimetableEntry]

@From datetime,
@To datetime,
@RoomId int,
@CNP varchar(13)

AS
	insert into dbo.Timetable ([From], [To], RoomId, EmployeeCNP)
	values ( @From, @To, @RoomId, @CNP)