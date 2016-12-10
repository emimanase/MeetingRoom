-----------------------------------------------------
/*
		Retrieve meeting rooms
*/
-----------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[GetAllMeetingRooms]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetAllMeetingRooms]
GO

CREATE PROCEDURE [dbo].[GetAllMeetingRooms]
AS

	Select *, f.Number from [MeetingRoomDB].dbo.Room r
	Inner join dbo.[Floor] f On f.RoomId = r.Id