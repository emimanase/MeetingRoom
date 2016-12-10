-----------------------------------------------------
/*
		Retrieve meeting room by id
*/
-----------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[GetMeetingRoomById]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetMeetingRoomById]
GO

CREATE PROCEDURE [dbo].[GetMeetingRoomById]

@Id int

AS

	Select * from [MeetingRoomDB].dbo.Room r
	Inner join dbo.[Floor] f On f.RoomId = r.Id
	where Id = @Id
	
