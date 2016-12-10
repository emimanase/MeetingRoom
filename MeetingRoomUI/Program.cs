using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeContext;
using EmployeeContext.Repositories;

namespace MeetingRoomUI
{
    /// <summary>
    /// This is the user access point
    /// </summary>
    class Program
    {

        private static EmployeeRepository _employeeRepo = new EmployeeRepository();
        private static RoomRepository _roomRepo = new RoomRepository();
        static void Main(string[] args)
        {
            while (true)
            {
                int option;
                InitializeApp();
                option = DoMainMenu();

                switch (option)
                {
                    case 1:
                        CreateEmployeeMenu();
                        break;
                    case 2:
                    {
                        var emp = DoEmployeesMenu();
                        _employeeRepo.RemoveEmployee(emp);
                        break;
                    }
                    case 3:
                    {
                        var emp = DoEmployeesMenu();
                        DoEmployeeMenu(emp);
                        break;
                    }
                    case 4:
                    {
                        var room = DoMeetingRoomsMenu();
                        DoMeetingRoomMenu(room);
                        break;
                    }
                }
                Console.Clear();
            }
        }

        #region Main menu
        private static void InitializeApp()
        {
            Console.WriteLine("==========================================");
            Console.WriteLine("=========Meeting Room application=========");
            Console.WriteLine("==========================================");
            Console.WriteLine();
        }

        private static int DoMainMenu()
        {
            Console.WriteLine("1. Create Employee");
            Console.WriteLine("2. Remove Employee");
            Console.WriteLine("3. Show All Employees");
            Console.WriteLine("4. Show All Meeting Rooms");
            Console.WriteLine();
            Console.WriteLine("Choose an option!");
            
            return Int32.Parse(Console.ReadLine());
        }

        #endregion Main menu

        #region Employees
        private static EmployeeAggregate DoEmployeesMenu()
        {
            var employees = _employeeRepo.GetAllEmployees();
            DisplayEmployees(employees);
            Console.WriteLine("Pick an employee (index)");
            
            return employees.ToArray()[Int32.Parse(Console.ReadLine())];
        }

        private static void DoEmployeeMenu(EmployeeAggregate employee)
        {
            Console.WriteLine("1. Change name");
            Console.WriteLine("2. Change role");
            Console.WriteLine("3. Add available rooms");
            Console.WriteLine("4. Remove available rooms");
            Console.WriteLine("5. Create booking");
            Console.WriteLine("6. Update booking");
            Console.WriteLine("7. Remove booking");
            Console.WriteLine("8. Check booking history");
            Console.WriteLine();
            Console.WriteLine("Choose an option!");
            
            var option = Int32.Parse(Console.ReadLine());
            switch (option)
            {
                case 1:
                {
                    Console.WriteLine("First Name: ");
                    var fn = new Text(Console.ReadLine());
                    Console.WriteLine("Last Name: ");
                    var ln = new Text(Console.ReadLine());
                    employee.ChangeName(fn, ln);
                    break;
                }
                case 2:
                {
                    var roleId = DoRoleMenu();
                    employee.ChangeRole((Role)roleId);
                    break;
                }
                case 3:
                {
                    AddAvailableRoom(employee);
                    break;
                }
                case 4:
                {
                    DeleteAvailableRoom(employee);
                    break;
                }
                case 5:
                {
                    BookRoom(employee);
                    break;
                }
                case 6:
                {
                    UpdateBooking(employee);
                    break;
                }
                case 7:
                {
                    RemoveBooking(employee);
                    break;
                }
                case 8:
                    CheckBookingHistory(employee);
                    break;
            }
        }

        private static void CheckBookingHistory(EmployeeAggregate employee)
        {
            var lowerRoleEmployees = _employeeRepo.GetAllEmployees().Where(e => e.Role <= employee.Role);
            var rooms = new List<MeetingRoom>();
            foreach (var emp in lowerRoleEmployees)
            {
                foreach (var room in emp.AvailableRooms)
                {
                    if (!rooms.Exists(r => r.Id == room.Id))
                        rooms.Add(room);
                }
            }

            DisplayRooms(rooms);
            Console.WriteLine("These are rooms you can view. Pick a room (index) to see its history!");
            var pickedRoom = rooms[Int32.Parse(Console.ReadLine())];
            var history = pickedRoom.CheckHistory();
            DisplayHistory(history);
            Console.WriteLine("Do you want to check other rooms' history? Y/N");
            if (Console.ReadLine().ToLower() == "y")
            {
                CheckBookingHistory(employee);
            }
        }

        private static void DisplayHistory(IEnumerable<TimetableHistory> historyEntries)
        {
            var header = string.Format("|{0,8}|{1,8}|{2,8}|{3,8}|{4,8}|{5,8}|{6,8}|{7,8}|", "Index", "Room Id", "Type", "Field Name",
                "Old Value", "New Value", "Updated Date", "Employee CNP");
            Console.WriteLine(header);
            foreach (var entry in historyEntries)
            {
                var displayedEntry = string.Format("|{0,8}|{1,8}|{2,8}|{3,8}|{4,8}|{5,8}|{6,8}|{7,8}|", 
                                                                                      historyEntries.ToList().IndexOf(entry),
                                                                                      entry.RoomId,
                                                                                      entry.Type,
                                                                                      entry.FieldName,
                                                                                      entry.OldValue,
                                                                                      entry.NewValue,
                                                                                      entry.UpdatedDate,
                                                                                      entry.EmployeeCNP);
                Console.WriteLine(displayedEntry);
            }
        }

        private static void AddAvailableRoom(EmployeeAggregate employee)
        {
            var room = DoMeetingRoomsMenu();
            employee.AddRoom(room);
            Console.WriteLine("Do you want to add another room? Y/N");
            if(Console.ReadLine().ToLower() == "y")
                AddAvailableRoom(employee);
        }

        private static void DeleteAvailableRoom(EmployeeAggregate employee)
        {
            DisplayRooms(employee.AvailableRooms);
            Console.WriteLine("Pick a room (index) to delete it!");

            var room = employee.AvailableRooms[Int32.Parse(Console.ReadLine())];
            employee.DeleteRoom(room);
            Console.WriteLine("Do you want to delete another available room? Y/N");
            if (Console.ReadLine().ToLower() == "y")
                DeleteAvailableRoom(employee);
        }

        private static void CreateEmployeeMenu()
        {
            Console.WriteLine("CNP: ");
            var cnp = new CNP(Console.ReadLine());
            Console.WriteLine("First Name: ");
            var firstName = new Text(Console.ReadLine());
            Console.WriteLine("Last Name: ");
            var lastName = new Text(Console.ReadLine());
            var roleId = DoRoleMenu();
            var role = (Role)roleId;
            
            EmployeeFactory factory = new EmployeeFactory();
            var employee = factory.GetEmployeeAggregate(cnp, firstName, lastName, role);

            DisplayEmployee(employee);
        }

        private static void DisplayEmployee(EmployeeAggregate employee)
        {
            var header = string.Format("|{0,5}|{1,5}|{2,5}|{3,5}|{4,5}|", "Employee CNP", "First Name", "Last Name", "Role", "Available Rooms Id" );
            Console.WriteLine(header);
            var rooms = employee.AvailableRooms.Aggregate("", (a, x) => a + x.Id + ",");
            var displayedEmployee = string.Format("|{0,5}|{1,5}|{2,5}|{3,5}|{4,5}|", employee.Cnp,
                                                                                      employee.FirstName,
                                                                                      employee.LastName,
                                                                                      employee.Role,
                                                                                      rooms);
            Console.WriteLine(displayedEmployee);
            Console.ReadLine();
        }

        private static void DisplayEmployees(IEnumerable<EmployeeAggregate> employees)
        {
            var header = string.Format("|{0,5}|{1,5}|{2,5}|{3,5}|{4,5}|{5,5}|", "Index", "CNP", "First Name", "Last Name", "Role","Available Rooms Id");
            Console.WriteLine(header);
            foreach (var employee in employees)
            {
                var rooms = employee.AvailableRooms.Aggregate("", (current, room) => current + room.Id.ToString() + ',');
                var displayedEmployee = string.Format("|{0,5}|{1,5}|{2,5}|{3,5}|{4,5}|{5,5}|",employees.ToList().IndexOf(employee),
                                                                                      employee.Cnp,
                                                                                      employee.FirstName,
                                                                                      employee.LastName,
                                                                                      employee.Role,
                                                                                      rooms);
                Console.WriteLine(displayedEmployee);    
            }
            
        }

        private static int DoRoleMenu()
        {
            int i = 1;
            foreach (var value in Enum.GetValues(typeof(Role)))
            {
                Console.WriteLine("{0}. {1}",i,value);
                i++;
            }
            Console.WriteLine("Pick a role (number)!");
            return Int32.Parse(Console.ReadLine());
        }
        #endregion Employees

        #region MeetingRooms
        private static MeetingRoom DoMeetingRoomsMenu()
        {
            var rooms = _roomRepo.GetAllMeetingRooms();
            DisplayRooms(rooms);
            Console.WriteLine("Pick a room (index)!");

            return rooms.ToArray()[Int32.Parse(Console.ReadLine())];
        }

        private static void DoMeetingRoomMenu(MeetingRoom room)
        {
            Console.WriteLine("1. Add assets");
            Console.WriteLine("2. Remove assets");
            Console.WriteLine();
            Console.WriteLine("Choose an option!");

            var option = Int32.Parse(Console.ReadLine());
            switch (option)
            {
                case 1:
                {
                    AddAsset(room);
                    break;
                }
                case 2:
                {
                    RemoveAsset(room);
                    break;
                }
            }
        }

        private static void AddAsset(MeetingRoom room)
        {
            var assetId = DoAssetsMenu();
            Asset asset = (Asset)assetId;
            room.AddAsset(asset);
            Console.WriteLine("Do you want to add another asset? Y/N");
            if (Console.ReadLine().ToLower() == "y")
            {
                AddAsset(room);
            }
        }

        private static void RemoveAsset(MeetingRoom room)
        {
            var assetId = DoAssetsMenu();
            Asset asset = (Asset)assetId;
            room.RemoveAsset(asset);
            Console.WriteLine("Do you want to remove another asset? Y/N");
            if (Console.ReadLine().ToLower() == "y")
            {
                RemoveAsset(room);
            }
        }

        private static int DoAssetsMenu()
        {
            int i = 1;
            foreach (var asset in Enum.GetValues(typeof(Asset)))
            {
                Console.WriteLine("{0}. {1}",i, asset);
                i++;
            }
            Console.WriteLine("Pick an asset (number)!");
            return Int32.Parse(Console.ReadLine());
        }

        private static void DisplayRooms(IEnumerable<MeetingRoom> rooms)
        {
            var header = string.Format("|{0,6}|{1,6}|{2,6}|{3,6}|{4,6}|", "Index", "Room Id", "Capacity", "Floor",
                "Assets Id");
            Console.WriteLine(header);                          
            foreach (var room in rooms)
            {
                var assets = room.Assets.Aggregate("", (current, asset) => current + (int)asset + ',');
                var displayedRoom = string.Format("|{0,6}|{1,6}|{2,6}|{3,6}|{4,6}|", rooms.ToList().IndexOf(room),
                                                                                      room.Id,
                                                                                      room.Capacity,
                                                                                      room.Floor,
                                                                                      assets);
                Console.WriteLine(displayedRoom);
            }

        }

        private static void BookRoom(EmployeeAggregate employee)
        {
            DisplayRooms(employee.AvailableRooms);
            Console.WriteLine("Pick a room (index) to book!");
            var room = employee.AvailableRooms[Int32.Parse(Console.ReadLine())];
            Console.WriteLine("Please provide date and time in this supported format: dd/mm/yyyy hh:mm");
            Console.WriteLine("From date and time:");
            var fromDate = Console.ReadLine();
            var from = Convert.ToDateTime(fromDate + ":00.00");
            Console.WriteLine("To date and time:");
            var toDate = Console.ReadLine();
            var to = Convert.ToDateTime(toDate + ":00.00");
            var booked = employee.CreateBookingOnRoom(room, from, to);
            if (booked)
                Console.WriteLine("Booked!");
            else
            {
                Console.WriteLine("Cannot book that room for that period. Try again? Y/N");
                if(Console.ReadLine().ToLower() == "y")
                    BookRoom(employee);
            }
        }

        private static void UpdateBooking(EmployeeAggregate employee)
        {
            var lowerRoleEmployees = _employeeRepo.GetAllEmployees().Where(e => e.Role <= employee.Role);
            var rooms = new List<MeetingRoom>();
            foreach (var emp in lowerRoleEmployees)
            {
                foreach (var room in emp.AvailableRooms)
                {
                    if(!rooms.Exists(r => r.Id == room.Id))
                        rooms.Add(room);
                }
            }

            DisplayRooms(rooms);
            Console.WriteLine("These are rooms you can update. Pick a room (index) to update timetable!");
            var pickedRoom = rooms[Int32.Parse(Console.ReadLine())];
            DisplayTimetable(pickedRoom.Timetable);
            Console.WriteLine("Pick an entry (index) to update it!");
            var pickedEntry = pickedRoom.Timetable[Int32.Parse(Console.ReadLine())];
            Console.WriteLine("Please provide date and time in this supported format: dd/mm/yyyy hh:mm");
            Console.WriteLine("From date and time:");
            var from = Convert.ToDateTime(Console.ReadLine());
            Console.WriteLine("To date and time:");
            var to = Convert.ToDateTime(Console.ReadLine());
            pickedEntry.SetFrom(from);
            pickedEntry.SetTo(to);
            pickedEntry.SetOwnerCNP(employee.Cnp);
            var updated = employee.UpdateBookingOnRoom(pickedRoom, pickedEntry);
            if (updated)
                Console.WriteLine("Updated!");
            else
            {
                Console.WriteLine("Cannot update that room for that period. Try again? Y/N");
                if (Console.ReadLine().ToLower() == "y")
                    UpdateBooking(employee);
            }
        }

        private static void RemoveBooking(EmployeeAggregate employee)
        {
            var lowerRoleEmployees = _employeeRepo.GetAllEmployees().Where(e => e.Role <= employee.Role);
            var rooms = new List<MeetingRoom>();
            foreach (var emp in lowerRoleEmployees)
            {
                foreach (var room in emp.AvailableRooms)
                {
                    if (!rooms.Exists(r => r.Id == room.Id))
                        rooms.Add(room);
                }
            }

            DisplayRooms(rooms);
            Console.WriteLine("These are rooms you can update. Pick a room (index) to update timetable!");
            var pickedRoom = rooms[Int32.Parse(Console.ReadLine())];
            DisplayTimetable(pickedRoom.Timetable);
            Console.WriteLine("Pick an entry (index) to delete it!");
            var pickedEntry = pickedRoom.Timetable[Int32.Parse(Console.ReadLine())];
            var deleted = employee.RemoveBookingOnRoom(pickedRoom, pickedEntry.From, pickedEntry.To);
            if (deleted)
            {
                Console.WriteLine("Deleted!");
            }
            else
            {
                Console.WriteLine("Cannot delete that period. Try again? Y/N");
                if (Console.ReadLine().ToLower() == "y")
                    RemoveBooking(employee);
            }
        }

        #endregion MeetingRooms

        #region Timetable

        private static void DisplayTimetable(IEnumerable<TimetableEntry> timetable)
        {
            var header = string.Format("|{0,5}|{1,5}|{2,5}|{3,5}|{4,5}|","Index", "Id","From Date", "To Date", "Owner CNP" );
            Console.WriteLine(header);
            foreach (var entry in timetable)
            {
                var displayedEntry = string.Format("|{0,5}|{1,5}|{2,5}|{3,5}|{4,5}|", timetable.ToList().IndexOf(entry),
                                                                                      entry.Id,
                                                                                      entry.From,
                                                                                      entry.To,
                                                                                      entry.OwnerCNP);
                Console.WriteLine(displayedEntry);
            }
        }

        #endregion Timetable
    }
}
