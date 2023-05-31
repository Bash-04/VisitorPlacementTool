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
        public int AvailableSeats { get; private set; }
        public bool Full { get; set; }

        // Constructors
        public Row(int rowNumber, char sectorLetter)
        {
            RowNumber = rowNumber;
            Code = sectorLetter.ToString() + RowNumber.ToString();
            Seats = new List<Seat>();
        }

        // Methods
        #region Create
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
        #endregion

        #region Count
        public int CountAvailableSeats()
        {
            AvailableSeats = 0;
            foreach (var seat in Seats)
            {
                if (!seat.Occupied)
                {
                    AvailableSeats++;
                }
            }
            return AvailableSeats;
        }
        #endregion

        #region Check
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
        #endregion
    }
}