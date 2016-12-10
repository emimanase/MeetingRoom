using System;
using System.Collections.Generic;
using System.Linq;
using EmployeeContext.Exceptions;
using EmployeeContext.Repositories;
using Logger;

namespace EmployeeContext
{
    public class EmployeeAggregate
    {
        private EmployeeRepository _employeeRepo = new EmployeeRepository();
        private List<MeetingRoom> _availableRooms = new List<MeetingRoom>(); 
        public CNP Cnp { get; private set; }

        public Text FirstName { get; private set; }

        public Text LastName { get; private set; }

        public Role Role { get; private set; }

        public List<MeetingRoom> AvailableRooms
        {
            get
            {
                foreach (var availableRoom in _availableRooms)
                {
                    availableRoom.GetTimetableFromRepositoryByRoom();
                }
                return _availableRooms;
            }
            private set { _availableRooms = value; }
        }

        internal EmployeeAggregate(CNP c, Text fn, Text ln, Role r)
        {
            Cnp = c;
            FirstName = fn;
            LastName = ln;
            Role = r;
            AvailableRooms = new List<MeetingRoom>();
        }

        internal EmployeeAggregate(CNP c, Text fn, Text ln, Role r, IEnumerable<MeetingRoom> rooms)
        {
            Cnp = c;
            FirstName = fn;
            LastName = ln;
            Role = r;
            AvailableRooms = new List<MeetingRoom>();
            AvailableRooms.AddRange(rooms);
        }

        public void ChangeName(Text fn, Text ln)
        {
            if (!(string.IsNullOrEmpty(fn.Continut) && string.IsNullOrEmpty(ln.Continut)))
            {
                FirstName = fn;
                LastName = ln;
                
                _employeeRepo.UpdateEmployee(this);
            }
            else
            {
                throw new InvalidInputException("Values for last name and first name should be provided");
            }
        }

        public void ChangeRole(Role r)
        {
            Role = r;
            _employeeRepo.UpdateEmployee(this);
        }

        public void AddRooms(IEnumerable<MeetingRoom> rooms)
        {
            foreach (var room in rooms)
            {
                if(!AvailableRooms.Exists(r => r.Id == room.Id))
                    AvailableRooms.Add(room);
            }
            _employeeRepo.UpdateEmployee(this);
        }

        public void AddRoom(MeetingRoom room)
        {
            if (!AvailableRooms.Exists(r => r.Id == room.Id))
            {
                AvailableRooms.Add(room);
            }

            _employeeRepo.UpdateEmployee(this);
        }

        public void DeleteRoom(MeetingRoom room)
        {
            var specificRoom = AvailableRooms.FirstOrDefault(r => r.Id == room.Id);
            if (specificRoom != null)
            {
                AvailableRooms.Remove(specificRoom);
                _employeeRepo.UpdateEmployee(this);
            }
        }

        public bool CanBookRoom(MeetingRoom room)
        {
            return AvailableRooms.Exists(r => r.Id == room.Id);
        }
        
        public bool CreateBookingOnRoom(MeetingRoom room, DateTime from, DateTime to)
        {
            try
            {
                if (CanBookRoom(room))
                {
                    room.CreateTimetableEntry(from, to, this);
                }
            }
            catch (Exception ex)
            {
                EventLogger.Error(ex);
                return false;
            }

            return true;
        }

        public bool UpdateBookingOnRoom(MeetingRoom room, TimetableEntry entry)
        {
            try
            {
                var specificEntry = room.GetTimetableEntryById(entry.Id);
                if (specificEntry != null)
                   room.UpdateTimetableEntry(entry);
                 
            }
            catch (Exception ex)
            {
                EventLogger.Error(ex);
                return false;
            }

            return true;
        }

        public bool RemoveBookingOnRoom(MeetingRoom room, DateTime from, DateTime to)
        {
            try
            {
                var entry = room.GetTimetableEntry(from, to, Cnp);
                if (entry != null)
                    room.RemoveTimetableEntry(entry);
            }
            catch (Exception ex)
            {
                EventLogger.Error(ex);
                return false;
            }
            return true;
        }
    }
}
