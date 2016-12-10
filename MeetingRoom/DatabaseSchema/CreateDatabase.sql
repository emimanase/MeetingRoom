-------------------------------------------------
/*
		Create database with name MeetingRoomDB
*/
-------------------------------------------------

IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = 'MeetingRoomDB' OR name = 'MeetingRoomDB'))	
	create database MeetingRoomDB
GO