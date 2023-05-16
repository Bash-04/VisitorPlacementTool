using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisitorPlacementToolLibrary
{
    public class Seat
    {
        public int FollowNumber { get; private set; }
        // Section row code + follownumber = Code
        public string Code { get; private set; }
    }
}