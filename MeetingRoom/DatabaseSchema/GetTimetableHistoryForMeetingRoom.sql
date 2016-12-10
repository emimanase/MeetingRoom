-----------------------------------------------------
/*
		Retrieve timetable history for meeting room
*/
-----------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[GetTimetableHistoryForMeetingRoom]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetTimetableHistoryForMeetingRoom]
GO

CREATE PROCEDURE [dbo].[GetTimetableHistoryForMeetingRoom]

@RoomId int

AS

	Select * from dbo.TimetableHistory where RoomId = @RoomId