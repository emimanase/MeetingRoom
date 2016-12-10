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
		OldValue varchar(1000), 
		NewValue varchar(1000), 
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
@deletedValue varchar(100),
@insertedValue varchar(100)

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
	-- get list of columns
	select * into #ins from inserted
	select * into #del from deleted

	select @field = 0, @maxfield = max(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'Timetable'
	while @field < @maxfield
	begin
		select @field = min(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'Timetable' and ORDINAL_POSITION > @field
		select @bit = (@field - 1 )% 8 + 1
		select @bit = power(2,@bit - 1)
		select @char = ((@field - 1) / 8) + 1
		
		if substring(COLUMNS_UPDATED(),@char, 1) & @bit > 0 or @Type in ('I','D')
		begin
			set @deletedColumnName = 'd.'+@fieldName
			set @insertedColumnName = 'i.'+@fieldName

			select @fieldname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'Timetable' and ORDINAL_POSITION = @field
			set @deletedValue = (select @deletedColumnName from deleted d)
			set @insertedValue = (select @insertedColumnName from inserted i)
			insert TimetableHistory ([Type], FieldName, OldValue, NewValue, UpdateDate, Employee, RoomId)
						select  @Type, @fieldName, @deletedValue, @insertedValue, @UpdateDate, @Employee,@RoomId
						from inserted i 
						full outer join deleted d on d.Id = i.Id
						where @insertedValue <> @deletedValue
							or (@insertedValue is null and  @deletedValue is not null)
							or (@insertedValue is not null and  @deletedValue is null)
		end
	end