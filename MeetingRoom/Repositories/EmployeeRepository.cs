using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EmployeeContext.Repositories
{
    public class EmployeeRepository : AbstractRepository
    {
        private RoomRepository _repo = new RoomRepository();

        public IEnumerable<EmployeeAggregate> GetAllEmployees()
        {
            List<EmployeeAggregate> result = new List<EmployeeAggregate>();

            var table = Dal.GetAllEmployees();
            foreach (DataRow row in table.Rows)
            {
                EmployeeAggregate emp;
                CNP cnp = new CNP(row["CNP"].ToString());
                Text firstName = new Text(row["FirstName"].ToString());
                Text lastName = new Text(row["LastName"].ToString());
                Role role = (Role)Enum.Parse(typeof (Role), row["Role"].ToString());
                if(!string.IsNullOrEmpty(row["AvailableRooms"].ToString()))
                {
                    IEnumerable<MeetingRoom> availableRooms = row["AvailableRooms"].ToString()
                                                               .Split(',')
                                                               .Where(t => !string.IsNullOrEmpty(t))
                                                               .Select(r => _repo.GetMeetingRoomById(Int32.Parse(r)));
                    emp = new EmployeeAggregate(cnp, firstName, lastName, role, availableRooms);
                }
                else
                {
                    emp = new EmployeeAggregate(cnp, firstName, lastName, role);
                }
                result.Add(emp);
            }

            return result;
        }
        
        public EmployeeAggregate GetEmployeeByCNP(CNP employeeCNP)
        {
            var table = Dal.GetEmployeeByCnp(employeeCNP.ToString());
            var row = table.Rows[0];
            if(row != null)
            {
                EmployeeAggregate employee;
                CNP cnp = new CNP(row["CNP"].ToString());
                Text firstName = new Text(row["FirstName"].ToString());
                Text lastName = new Text(row["LastName"].ToString());
                Role role = (Role)Enum.Parse(typeof (Role), row["Role"].ToString());
                if (!string.IsNullOrEmpty(row["AvailableRooms"].ToString()))
                {
                    IEnumerable<MeetingRoom> availableRooms = row["AvailableRooms"].ToString()
                                                               .Split(',')
                                                               .Where(t => !string.IsNullOrEmpty(t))
                                                               .Select(r => _repo.GetMeetingRoomById(Int32.Parse(r)));
                    employee = new EmployeeAggregate(cnp, firstName, lastName, role, availableRooms);
                }
                else
                {
                    employee = new EmployeeAggregate(cnp, firstName, lastName, role);
                }
                return employee;
            }

            return null;
        }

        public void UpdateEmployee(EmployeeAggregate employee)
        {
            var rooms = employee.AvailableRooms.Aggregate("", (current, room) => current + room.Id.ToString() + ',');
            Dal.UpdateEmployee(employee.Cnp.ToString(), employee.FirstName.ToString(), employee.LastName.ToString(), employee.Role.ToString(), rooms);
        }

        public void RemoveEmployee(EmployeeAggregate employee)
        {
            Dal.RemoveEmployee(employee.Cnp.ToString());
        }

        public void SaveNewEmployee(EmployeeAggregate employee)
        {
            var rooms = employee.AvailableRooms.Aggregate("", (current, room) => current + room.Id.ToString() + ',');
            Dal.InsertEmployee(employee.Cnp.ToString(), employee.FirstName.ToString(), employee.LastName.ToString(), employee.Role.ToString(), rooms);
        }
    }
}
