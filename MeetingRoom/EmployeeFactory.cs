using System.Collections.Generic;
using System.Linq;
using EmployeeContext.Repositories;

namespace EmployeeContext
{
    public class EmployeeFactory
    {
        private EmployeeRepository _repo = new EmployeeRepository();
        public EmployeeAggregate GetEmployeeAggregate(CNP cnp, Text firstName, Text lastName, Role role, IEnumerable<MeetingRoom> availableRooms)
        {
            EmployeeAggregate employee;
            if(availableRooms.Any())
                employee = new EmployeeAggregate(cnp, firstName, lastName, role, availableRooms);
            else
            {
                employee = new EmployeeAggregate(cnp, firstName, lastName, role);
            }
            
            _repo.SaveNewEmployee(employee);

            return employee;
        }

        public EmployeeAggregate GetEmployeeAggregate(CNP cnp, Text firstName, Text lastName, Role role)
        {
            var employee = new EmployeeAggregate(cnp, firstName, lastName, role);
            _repo.SaveNewEmployee(employee);

            return employee;
        }
    }
}
