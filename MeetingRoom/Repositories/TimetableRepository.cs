using System;
using System.Collections.Generic;
using System.Data;

namespace EmployeeContext.Repositories
{
    public class TimetableRepository : AbstractRepository
    {
        public IEnumerable<TimetableEntry> GetEntireTimetable()
        {
            return null;
        }

        public IEnumerable<TimetableEntry> GetEntireTimetableByRoom(int roomId)
        {
            List<TimetableEntry> result = new List<TimetableEntry>();
            var table = Dal.GetTimetableForMeetingRoom(roomId);
            foreach (DataRow row in table.Rows)
            {
                var id = Int32.Parse(row["Id"].ToString());
                var fromDate = Convert.ToDateTime(row["From"].ToString());
                var toDate = Convert.ToDateTime(row["To"].ToString());
                var cnp = new CNP(row["EmployeeCNP"].ToString());
                var timetableEntry = new TimetableEntry(id, fromDate, toDate, cnp);
                result.Add(timetableEntry);
            }

            return result;
        }

        public void UpdateTimetable(int roomId, TimetableEntry entry)
        {
            Dal.UpdateEntryInTimetable(entry.Id, entry.From, entry.To, roomId, entry.OwnerCNP.ToString());
        }

        public void RemoveTimetableEntry(TimetableEntry entry)
        {
            Dal.RemoveEntryInTimetable(entry.Id);
        }

        public void SaveNewTimetableEntry(int roomId, TimetableEntry entry)
        {
            Dal.InsertEntryInTimetable(entry.From, entry.To, roomId, entry.OwnerCNP.ToString());
        }

        public IEnumerable<TimetableHistory> GetHistoryByRoomId(int roomId)
        {
            List<TimetableHistory> results = new List<TimetableHistory>();
            var table = Dal.GetTimetableHistoryForMeetingRoom(roomId);
            foreach (DataRow row in table.Rows)
            {
                var room = Int32.Parse(row["RoomId"].ToString());
                var type = row["Type"].ToString();
                var fieldName = row["FieldName"].ToString();
                var oldValue = row["OldValue"].ToString();
                var newValue = row["NewValue"].ToString();
                var updatedDate = Convert.ToDateTime(row["UpdateDate"].ToString());
                var employeeCnp = new CNP(row["Employee"].ToString());

                var historyEntry = new TimetableHistory(room, type, fieldName, oldValue, newValue, updatedDate, employeeCnp);

                results.Add(historyEntry);
            }

            return results;
        }
    }
}
