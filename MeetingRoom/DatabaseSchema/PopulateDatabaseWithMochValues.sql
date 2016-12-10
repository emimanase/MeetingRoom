-----------------------------------------------------
/*
		Populate tables with mock values
*/
-----------------------------------------------------

USE MeetingRoomDB

IF NOT EXISTS(Select * from dbo.Employee)
Begin
	insert into dbo.Employee(CNP, FirstName, LastName, [Role], AvailableRooms) values ('1930704350001', 'Jon', 'Bridge', 'Developer', N'1,2,3')

	insert into dbo.Employee(CNP, FirstName, LastName, [Role], AvailableRooms) values ('1930704350002', 'Mark', 'Cox', 'Developer', N'1,2')

	insert into dbo.Employee(CNP, FirstName, LastName, [Role], AvailableRooms) values ('1930704350003', 'Mike', 'Desouza', 'TeamLeader', N'1,4')
																																		   
	insert into dbo.Employee(CNP, FirstName, LastName, [Role], AvailableRooms) values ('1930704350004', 'Bill', 'Norman', 'Manager', N'5,6')

	insert into dbo.Employee(CNP, FirstName, LastName, [Role], AvailableRooms) values ('1930704350005', 'Rob', 'Turner', 'Ceo', N'2,5,6')
End
GO

IF NOT Exists(Select * from dbo.Asset)
Begin
	insert into dbo.Asset(Id, Name) values (1, 'Projector')
	insert into dbo.Asset(Id, Name) values (2, 'FlipBoard')
	insert into dbo.Asset(Id, Name) values (3, 'TV')
	insert into dbo.Asset(Id, Name) values (4, 'WhiteBoard')
	insert into dbo.Asset(Id, Name) values (5, 'VideoConferenceEq')
End

IF NOT EXISTS(Select * from dbo.[Room])
Begin
	insert into dbo.Room (Id, Capacity, Assets) values (1, 10, N'1,2')
																 
	insert into dbo.Room (Id, Capacity, Assets) values (2, 12, N'1,2,5')

	insert into dbo.Room (Id, Capacity, Assets) values (3, 20, N'1,2,5')
																 
	insert into dbo.Room (Id, Capacity, Assets) values (4, 23, N'1,2,4,5')

	insert into dbo.Room (Id, Capacity, Assets) values (5, 30, N'1,3,4,5')

	insert into dbo.Room (Id, Capacity, Assets) values (6, 50, N'1,2,3,4,5')
	
End
GO

IF NOT EXISTS(Select * from dbo.[Floor])
Begin
	insert into dbo.[Floor](Number, RoomId) values (1, 1)
	insert into dbo.[Floor](Number, RoomId) values (1, 2)
	insert into dbo.[Floor](Number, RoomId) values (1, 3)
	insert into dbo.[Floor](Number, RoomId) values (2, 4)
	insert into dbo.[Floor](Number, RoomId) values (2, 5)
	insert into dbo.[Floor](Number, RoomId) values (3, 6)
End
GO

IF NOT EXISTS(Select * from dbo.Timetable)
Begin
	insert into dbo.Timetable (RoomId, EmployeeCNP, [From], [To]) values (1, '1930704350001', '8/12/2016 16:00:000', '8/12/2016 18:00:000')
	insert into dbo.Timetable (RoomId, EmployeeCNP, [From], [To]) values (1, '1930704350002', '7/12/2016 9:00:000', '7/12/2016 11:00:000')
	insert into dbo.Timetable (RoomId, EmployeeCNP, [From], [To]) values (4, '1930704350003', '7/12/2016 9:00:000', '7/12/2016 11:00:000')
End
Go