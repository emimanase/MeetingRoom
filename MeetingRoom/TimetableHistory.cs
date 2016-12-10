using System;

namespace EmployeeContext
{
    public class TimetableHistory
    {
        public int RoomId { get; private set; }
        public string Type { get; private set; }

        public string FieldName { get; private set; }

        public string OldValue { get; private set; }

        public string NewValue { get; private set; }

        public DateTime UpdatedDate{ get; private set; }

        public CNP EmployeeCNP { get; private set; }

        public TimetableHistory(int roomId, string type, string fieldName, string oldValue, string newValue, DateTime updatedDate, CNP employeeCNP)
        {
            RoomId = roomId;
            this.Type = type;
            FieldName = fieldName;
            OldValue = oldValue;
            NewValue = newValue;
            UpdatedDate = updatedDate;
            EmployeeCNP = employeeCNP;
        }
    }
}
