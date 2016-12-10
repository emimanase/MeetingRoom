-----------------------------------------------------
/*
		Update meeting a room
*/
-----------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[UpdateRoom]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdateRoom]
GO

CREATE PROCEDURE [dbo].[UpdateRoom]

@Id int,
@Capacity int,
@Assets varchar(100)

AS
	update dbo.Room set Capacity = @Capacity, Assets = @Assets
	where Id = @Id