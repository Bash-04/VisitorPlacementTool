using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisitorPlacementToolLibrary
{
    public class Seat
    {
        // Properties
        public int FollowNumber { get; private set; }
        // Sector row code + "-"follownumber = Code
        public string Code { get; private set; }
        public bool Occupied { get; private set; }

        // Constructors
        public Seat(int seatNumber, string rowCode)
        {
            FollowNumber = seatNumber;
            Code = rowCode + "-" + FollowNumber.ToString();
        }

        // Methods

    }
}