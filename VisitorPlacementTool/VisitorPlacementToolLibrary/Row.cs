using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace VisitorPlacementToolLibrary
{
    public class Row
    {
        // Properties
        private int RowNumber { get; set; }
        // Code = Sector letter + RowNumber 
        private string Code { get; set; }
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
                Seat seat = new Seat(Seats.Count + 1, Code);
                Seats.Add(seat);
            }

            return seatsHaveBeenCreated;
        }
        #endregion

        #region Sort
        public void PlaceVisitors(Group group)
        {
            foreach (var visitor in group.Visitors)
            {
                if (!visitor.Seated)
                {
                    if (!visitor.Adult && RowNumber == 1)
                    {
                        PlaceInSeat(visitor, group);
                    }
                    else if (visitor.Adult)
                    {
                        PlaceInSeat(visitor, group);
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
            }
        }

        private void PlaceInSeat(Visitor visitor, Group group)
        {
            foreach (var seat in Seats)
            {
                if (!seat.Occupied)
                {
                    seat.AssignVisitorToSeat(visitor);
                    visitor.SeatVisitor(seat.Code);
                    group.UnseatedGroupMembers--;
                    break;
                }
            }
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
