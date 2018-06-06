using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Timetabling.DB;
using Timetabling.Resources;

namespace Timetabling.Objects

{
    /// <summary>
    /// Rooms list.
    /// </summary>
    public class RoomsList : AbstractList
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Timetabling.Objects.RoomsList"/> class.
        /// </summary>
        /// <param name="_db">Db.</param>
        public RoomsList(DataModel _db) : base(_db) => SetListElement("Rooms_List");

        /// <summary>
        /// Creates the Room elements from the datamodel
        /// </summary>
        public override void Create()
        {
            var query = dB.School_BuildingsUnits.Where(room => room.IsActive == true)
                          .Select(room => room.ID);

            foreach (var room in query)
            {
                List.Add(new XElement("Room", new XElement("Name", room)));
            }
        }

        public Dictionary<int, Room> GetRooms()
        {

            var rooms = new Dictionary<int, Room>();

            var query = dB.School_BuildingsUnits.Where(room => room.IsActive == true);

            // Loop over all rooms
            foreach (var room in query)
            {
                rooms.Add(room.ID, new Room
                {
                    Id = room.ID,
                    Name = room.UnitName
                });
            }

            return rooms;
        }

    }
}
