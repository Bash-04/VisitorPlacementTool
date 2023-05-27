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
        public int RowCount { get; private set; }
        public int RowLength { get; private set; }
        public bool Full { get; set; }
        public bool FrontSeatsTaken { get; private set; }
        public int TotalSeats { get; private set; }
        public int AvailableSeats { get; private set; }

        // Constructors
        public Sector(char sectorLetter, int rowCount, int rowLength)
        {
            Rows = new List<Row>();
            SectorLetter = sectorLetter;
            RowCount = rowCount;
            RowLength = rowLength;
        }

        // Methods
        public bool CreateRows()
        {
            bool rowsHaveBeenCreated = false;

            for (int i = 0; i < RowCount; i++)
            {
                Row row = new Row(i+1, SectorLetter);
                row.CreateSeats(RowLength);
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

        public bool CheckIfFull()
        {
            Full = true;
            foreach (var row in Rows)
            {
                if (!row.Full)
                {
                    Full = false;
                    break;
                }
            }
            return Full;
        }
    }
}