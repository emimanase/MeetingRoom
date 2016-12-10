-----------------------------------------------------
/*
		Insert meeting a room
*/
-----------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[InsertRoom]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[InsertRoom]
GO

CREATE PROCEDURE [dbo].[InsertRoom]

@Id int,
@Capacity int,
@Assets varchar(100)

AS
	insert into dbo.Room (Id, Capacity, Assets) values (@Id, @Capacity, @Assets);