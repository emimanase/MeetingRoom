using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EmployeeContext.Repositories
{
    public class RoomRepository : AbstractRepository
    {
        public IEnumerable<EmployeeContext.MeetingRoom> GetAllMeetingRooms()
        {
            List<EmployeeContext.MeetingRoom> result = new List<EmployeeContext.MeetingRoom>();

            var table = Dal.GetAllMeetingRooms();
            foreach (DataRow row in table.Rows)
            {
                MeetingRoom room;
                var id = Int32.Parse(row["Id"].ToString());
                var capacity = new Capacity(Int32.Parse(row["Capacity"].ToString()));
                var floorNumber = new Floor(Int32.Parse(row["Number"].ToString()));
                var retrievedAssets = row["Assets"].ToString();
                if (!string.IsNullOrEmpty(retrievedAssets))
                {
                    List<Asset> assets = retrievedAssets.Split(',')
                        .Where(t => !string.IsNullOrEmpty(t))
                        .Select(t => (Asset) Enum.Parse(typeof (Asset), t.ToString()))
                        .ToList();

                    room = new EmployeeContext.MeetingRoom(id, floorNumber, capacity, assets);
                }
                else
                {
                    room = new EmployeeContext.MeetingRoom(id, floorNumber, capacity);
                }
                result.Add(room);
            }

            return result;
        }

        public EmployeeContext.MeetingRoom GetMeetingRoomById(int roomId)
        {
            var table = Dal.GetMeetingRoomById(roomId);
            var row = table.Rows[0];
            if (row != null)
            {
                MeetingRoom room;
                var id = Int32.Parse(row["Id"].ToString());
                var capacity = new Capacity(Int32.Parse(row["Capacity"].ToString()));
                var floorNumber = new Floor(Int32.Parse(row["Number"].ToString()));
                var retrievedAssets = row["Assets"].ToString();
                if (!string.IsNullOrEmpty(retrievedAssets))
                {
                    List<Asset> assets = retrievedAssets.Split(',')
                        .Where(t => !string.IsNullOrEmpty(t))
                        .Select(t => (Asset)Enum.Parse(typeof(Asset), t.ToString()))
                        .ToList();

                    room = new EmployeeContext.MeetingRoom(id, floorNumber, capacity, assets);
                }
                else
                {
                    room = new EmployeeContext.MeetingRoom(id, floorNumber, capacity);
                }
                return room;
            }
            return null;
        }

        public void UpdateMeetingRoom(EmployeeContext.MeetingRoom room)
        {
            string assets = room.Assets.Aggregate("", (current, asset) => current + ((int)asset) + ',');
            Dal.UpdateRoom(room.Id, room.Capacity.Number, assets);
        }

        public void RemoveMeetingRoom(EmployeeContext.MeetingRoom room)
        {
            Dal.RemoveRoom(room.Id);
        }

        public void SaveNewMeetingRoom(EmployeeContext.MeetingRoom room)
        {
            string assets = room.Assets.Aggregate("", (current, asset) => current + ((int)asset) + ',');
            Dal.InsertRoom(room.Id, room.Capacity.Number, assets);
        }
    }
}
