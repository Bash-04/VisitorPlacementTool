using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisitorPlacementToolLibrary
{
    public class Section
    {
        public char SectionLetter { get; private set; }
        public bool Opened { get; private set; }
        public List<Row> Rows { get; private set; }
        public int Length { get; private set; }
        public bool Full { get; private set; }
        public bool FrontSeatsTaken { get; private set; }
        public int TotalSeats { get; private set; }
        public int AvailableSeats { get; private set; }
    }
}