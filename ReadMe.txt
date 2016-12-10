1. Run scripts from: MeetingRoom\MeetingRoom\DatabaseSchema in this order 
	1. CreateDatabase.sql
	2. CreateTables.sql
	3. PopulateDatabaseWithMochValues
	4. All the others in any order 

2. Update MeetingRoom\MeetingRoomUI\App.config  to point to the installed instance of SQL (database should not be changed)
3. Update MeetingRoom\MeetingRoomUI\bin\Debug\MeetingRoomUI.exe.config  to point to the installed instance of SQL (database should not be changed)

4. Run it MeetingRoom\MeetingRoomUI\bin\Debug\MeetingRoomUI.exe