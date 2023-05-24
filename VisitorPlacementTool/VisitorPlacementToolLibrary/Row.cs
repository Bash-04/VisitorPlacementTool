using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisitorPlacementToolLibrary
{
    public class Row
    {
        // Properties
        public int RowNumber { get; private set; }
        // Code = Sector letter + RowNumber 
        public string Code { get; private set;}
        public List<Seat> Seats { get; private set; }
        public bool Full { get; set; }

        // Constructors
        public Row(int rowNumber, char sectorLetter)
        {
            RowNumber = rowNumber;
            Code = sectorLetter.ToString() + RowNumber.ToString();
            Seats = new List<Seat>();
        }

        // Methods
        public bool CreateSeats(int length)
        {
            bool seatsHaveBeenCreated = false;

            for (int i = 0; i < length; i++)
            {
                Seat seat = new Seat(Seats.Count+1, Code);
                Seats.Add(seat);
            }

            return seatsHaveBeenCreated;
        }

        public bool CheckIfFull()
        {
            Full = true;
            foreach (var seat in Seats)
            {
                if (!seat.Occupied)
                {
                    Full = false;
                    break;
                }
            }
            return Full;
        }
    }
}