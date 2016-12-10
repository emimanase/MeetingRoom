-------------------------------------------------
/*
		Create tables in database
*/
-------------------------------------------------

use MeetingRoomDB



IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Asset')
	create table Asset(
		Id int primary key,
		Name varchar(25)
	)
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Room')
	create table Room(
		Id int Primary key,
		Capacity int,
		Assets varchar(100)
	)
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Employee')	
	create table Employee(
		CNP varchar(13) Primary key,
		FirstName varchar(50),
		LastName varchar(50),
		Role varchar(25),
		AvailableRooms varchar(100)
	)
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Floor')
	create table [Floor](
		Number int,
		RoomId int Foreign key references Room(Id) Primary Key,
	)
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Timetable')
	create table Timetable(
		Id int primary key identity(1,1),
		RoomId int Foreign key references Room(Id) ON DELETE CASCADE,
		[From] dateTime,
		[To] dateTime,
		EmployeeCNP varchar(13) Foreign key references Employee(CNP) ON DELETE CASCADE
	)
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME= 'TimetableHistory')
	create table TimetableHistory(
		AuditId [int] IDENTITY(1,1) NOT NULL,
		RoomId int Foreign key references Room(Id) ON DELETE CASCADE,
		[Type] char(1), 
		FieldName varchar(128), 
		OldValue sql_variant, 
		NewValue sql_variant, 
		UpdateDate datetime DEFAULT (GetDate()), 
		Employee varchar(128)
	)
GO


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[Timetable_Audit]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
DROP TRIGGER [dbo].[Timetable_Audit]
GO

Create Trigger dbo.Timetable_Audit on dbo.Timetable
for insert, update, delete

as 

declare @bit int,
@field int, 
@maxfield int,
@char int ,
@fieldname varchar(128) ,
@sql nvarchar(1000), 
@UpdateDate varchar(21) ,
@Employee varchar(128) ,
@Type char(1) ,
@RoomId int,
@ParamDefinition varchar(1000),
@deletedColumnName varchar(100),
@insertedColumnName varchar(100),
@deletedValue sql_variant,
@insertedValue sql_variant

	-- date
	set @UpdateDate = GETDATE()

	-- Action
	IF EXISTS(select * from inserted)
		IF EXISTS (select * from deleted)
		Begin
			select @Type = 'U'
			set @RoomId = (select RoomId from inserted)
			set @Employee = (select EmployeeCNP from inserted)
		End
		else
			select @Type = 'I'
	else
	Begin
		select @Type = 'D'
		set @RoomId = (select RoomId from deleted)
		set @Employee = (select EmployeeCNP from deleted)
	End

		set @deletedValue = (select RoomId from deleted)
		set @insertedValue = (select RoomId from inserted)
	
		if(@deletedValue <> @insertedValue or @deletedValue is null or @insertedValue is null)
		Begin
			set @Employee = (select EmployeeCNP from inserted)
			insert TimetableHistory ([Type], FieldName, OldValue, NewValue, UpdateDate, Employee, RoomId)
						values(@Type, 'RoomId', @deletedValue, @insertedValue, @UpdateDate, @Employee,@RoomId)
		End
		
		set @deletedValue = (select [From] from deleted)
		set @insertedValue = (select [From] from inserted)
		if(@deletedValue <> @insertedValue or @deletedValue is null or @insertedValue is null)
		Begin
			
			set @Employee = (select EmployeeCNP from inserted)
			insert TimetableHistory ([Type], FieldName, OldValue, NewValue, UpdateDate, Employee, RoomId)
						values(@Type, 'From', @deletedValue, @insertedValue, @UpdateDate, @Employee,@RoomId)
		End
		
		set @deletedValue = (select [To] from deleted)
		set @insertedValue = (select [To] from inserted)
		if(@deletedValue <> @insertedValue or @deletedValue is null or @insertedValue is null)
		Begin
			set @Employee = (select EmployeeCNP from inserted)
			insert TimetableHistory ([Type], FieldName, OldValue, NewValue, UpdateDate, Employee, RoomId)
						values(@Type, 'To', @deletedValue, @insertedValue, @UpdateDate, @Employee,@RoomId)
		End

		set @deletedValue = (select [EmployeeCNP] from deleted)
		set @insertedValue = (select [EmployeeCNP] from inserted)
		if(@deletedValue <> @insertedValue or @deletedValue is null or @insertedValue is null)
		Begin
			set @Employee = (select EmployeeCNP from inserted)
			insert TimetableHistory ([Type], FieldName, OldValue, NewValue, UpdateDate, Employee, RoomId)
						values(@Type, 'EmployeeCNP', @deletedValue, @insertedValue, @UpdateDate, @Employee,@RoomId)
		End
	