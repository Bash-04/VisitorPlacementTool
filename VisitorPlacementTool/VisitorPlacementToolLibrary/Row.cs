using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisitorPlacementToolLibrary
{
    public class Row
    {
        public int RowNumber { get; private set; }
        // Section Section letter + RowNumber = Code
        public string Code { get; private set;}
        public List<Seat> Seats { get; private set; }
    }
}