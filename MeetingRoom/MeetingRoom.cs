using System;
using System.Collections.Generic;
using System.Linq;
using EmployeeContext.Exceptions;
using EmployeeContext.Repositories;
using Logger;

namespace EmployeeContext
{
    public class MeetingRoom
    {

        private RoomRepository _roomRepo = new RoomRepository();
        private TimetableRepository _timetableRepo = new TimetableRepository();

        private List<Asset> _assets = new List<Asset>();
        private List<TimetableEntry> _timetableEntries = new List<TimetableEntry>();

        public int Id { get; private set; }

        public Floor Floor { get; private set; }

        public List<Asset> Assets
        {
            get { return _assets; }
        }

        public List<TimetableEntry> Timetable
        {
            get { return _timetableEntries; }
        }

        public Capacity Capacity { get; private set; }

        internal MeetingRoom(int id, Floor floorNumber, Capacity capacity)
        {
            Id = id;
            Floor = floorNumber;
            Capacity = capacity;
            _assets = new List<Asset>();
            _timetableEntries = new List<TimetableEntry>();

        }

        internal MeetingRoom(int id, Floor floorNumber, Capacity capacity, IEnumerable<Asset> assets)
        {
            Id = id;
            Floor = floorNumber;
            Capacity = capacity;
            _assets = new List<Asset>();
            _timetableEntries = new List<TimetableEntry>();

            _assets.AddRange(assets);

        }


        public void AddAsset(Asset a)
        {
            //The case when you already have an asset is not treated
            // I considered a meeting room has only one asset per type
            if (!_assets.Contains(a))
            {
                _assets.Add(a);
                _roomRepo.UpdateMeetingRoom(this);
            }
        }

        public void RemoveAsset(Asset a)
        {
            if (_assets.Contains(a))
            {
                _assets.Remove(a);
                _roomRepo.UpdateMeetingRoom(this);
            }
        }

        public IEnumerable<TimetableHistory> CheckHistory()
        {
            return _timetableRepo.GetHistoryByRoomId(Id);
        }

        internal void CreateTimetableEntry(DateTime from, DateTime to, EmployeeAggregate employee)
        {
            if (IsTimetableAvailableInInterval(from, to))
            {
                try
                {
                    var entry = new TimetableEntry(_timetableEntries.Count + 1, from, to, employee.Cnp);
                    _timetableEntries.Add(entry);
                    _timetableRepo.SaveNewTimetableEntry(Id, entry);
                }
                catch (InvalidInputException ex)
                {
                    EventLogger.Error(ex);
                    throw;
                }
            }
        }

        internal void GetTimetableFromRepositoryByRoom()
        {
            _timetableEntries = _timetableRepo.GetEntireTimetableByRoom(Id).ToList();
        }

        internal TimetableEntry GetTimetableEntryById(int id)
        {
            return _timetableEntries.FirstOrDefault(e => e.Id == id);
        }

        internal TimetableEntry GetTimetableEntry(DateTime from, DateTime to, CNP ownerCnp)
        {
            return Timetable.FirstOrDefault(e => e.From == from && e.To == to && e.OwnerCNP == ownerCnp);
        }

        internal bool DoesTimetableEntryExists(int id)
        {
            return GetTimetableEntryById(id) != null;
        }

        internal void UpdateTimetableEntry(TimetableEntry entry)
        {
            if (DoesTimetableEntryExists(entry.Id))
            {
                _timetableEntries[_timetableEntries.IndexOf(entry)] = entry;
                _timetableRepo.UpdateTimetable(Id, entry);
            }
        }

        internal void RemoveTimetableEntry(TimetableEntry entry)
        {
            if (DoesTimetableEntryExists(entry.Id))
            {
                _timetableEntries.Remove(entry);
                _timetableRepo.RemoveTimetableEntry(entry);
            }
        }

        private bool IsTimetableAvailableInInterval(DateTime from, DateTime to)
        {
            return !_timetableEntries.Exists(entry => entry.From < to && from < entry.To);
        }
    }
}
