using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisitorPlacementToolLibrary
{
    public class Sector
    {
        // Properties
        public char SectorLetter { get; private set; }
        public bool Opened { get; private set; }
        public List<Row> Rows { get; private set; }
        public int Length { get; private set; }
        public bool Full { get; private set; }
        public bool FrontSeatsTaken { get; private set; }
        public int TotalSeats { get; private set; }
        public int AvailableSeats { get; private set; }

        // Constructors
        public Sector()
        {
            Rows = new List<Row>();
        }

        // Methods
        public bool CreateRows(char sectorLetter)
        {
            bool rowsHaveBeenCreated = false;

            SectorLetter = sectorLetter;

            Random random = new Random();
            int rowsCount = random.Next(1, 4);
            Length = random.Next(1, 11);

            for (int i = 0; i < rowsCount; i++)
            {
                Row row = new Row(Rows.Count+1, SectorLetter);
                row.CreateSeats(Length);
                Rows.Add(row);
            }

            CountTotalSeats();

            return rowsHaveBeenCreated;
        }

        public int CountTotalSeats()
        {
            TotalSeats = 0;

            foreach (var row in Rows)
            {
                TotalSeats += row.Seats.Count();
            }

            return TotalSeats;
        }
    }
}