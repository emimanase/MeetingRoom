-----------------------------------------------------
/*
		Retrieve timetable for meeting room
*/
-----------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[GetTimetableForMeetingRoom]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetTimetableForMeetingRoom]
GO

CREATE PROCEDURE [dbo].[GetTimetableForMeetingRoom]

@RoomId int

AS

	Select * from [MeetingRoomDB].dbo.Timetable where RoomId = @RoomId