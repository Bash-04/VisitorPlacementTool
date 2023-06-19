using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorPlacementToolLibrary
{
    public class Seat
    {
        // Properties
        private int FollowNumber { get; set; }
        // Sector row code + "-"follownumber = Code
        public string Code { get; private set; }
        public bool Occupied { get; private set; }
        public Visitor Visitor { get; private set; }

        // Constructors
        public Seat(int seatNumber, string rowCode)
        {
            FollowNumber = seatNumber;
            Code = rowCode + "-" + FollowNumber.ToString();
            Visitor = new Visitor(""); 
            Occupied = false;
        }

        // Methods
        public void AssignVisitorToSeat(Visitor visitor)
        {
            if (visitor.AssignedSeat == "")
            {
                Visitor = visitor;
                Occupied = true;
            }
        }
    }
}
