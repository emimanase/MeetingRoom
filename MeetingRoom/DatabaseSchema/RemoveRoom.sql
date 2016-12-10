-----------------------------------------------------
/*
		Remove room
*/
-----------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[RemoveRoom]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[RemoveRoom]
GO

CREATE PROCEDURE [dbo].[RemoveRoom]

@Id int

AS
	delete from dbo.Room where Id = @Id