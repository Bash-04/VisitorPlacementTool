﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorPlacementToolLibrary
{
    public class Sector
    {
        // Properties
        public char SectorLetter { get; private set; }
        public bool Opened { get; private set; }
        public List<Row> Rows { get; private set; }
        private int RowCount { get; set; }
        private int RowLength { get; set; }
        public bool Full { get; private set; }
        public bool FrontSeatsTaken { get; private set; }
        private bool BackSeatsTaken { get; set; }
        public int TotalSeats { get; private set; }
        public int AvailableSeats { get; private set; }

        // Constructors
        public Sector(char sectorLetter, int rowCount, int rowLength)
        {
            Rows = new List<Row>();
            SectorLetter = sectorLetter;
            RowCount = rowCount;
            RowLength = rowLength;
            Opened = true;
            CreateRows();
            CountAvailableSeats();
            CountTotalSeats();
        }

        // Methods
        #region Create
        public bool CreateRows()
        {
            bool rowsHaveBeenCreated = false;

            for (int i = 0; i < RowCount; i++)
            {
                Row row = new Row(i + 1, SectorLetter, RowLength);
                Rows.Add(row);
            }

            CountTotalSeats();

            return rowsHaveBeenCreated;
        }
        #endregion

        #region Sort

        public void PlaceVisitors(Sector sector, Group group)
        {
            // if group contains children and the sector has front seats available, place children in front seats
            if (group.ContainsChildren && !sector.FrontSeatsTaken && !group.ChildrenArePlaced)
            {
                PlaceChildrenInSector(group);
            }
            // if group does not contain children place group in sector
            else if (!group.ContainsChildren || group.ChildrenArePlaced)
            {
                PlaceInBackRows(group);
            }
            // if back seats are taken and front seats are available, place group in front seats
            if (CheckIfBackRowSeatsAreTaken() && !sector.FrontSeatsTaken)
            {
                PlaceInFirstRow(group);
            }
        }

        private void PlaceChildrenInSector(Group group)
        {
            // Make sure there are enough seats available to put a parent with the children in the front row
            if (Rows[0].AvailableSeats > group.ChildrenCount)
            {
                PlaceInFirstRow(group);
                group.DefaultCheckAndCount();
                // if children are placed and not all group members are placed, place the rest of the group in the back rows
                if (group.ChildrenArePlaced && !group.IsPlaced)
                {
                    PlaceInRow(group);
                }
            }
        }

        private void PlaceInRow(Group group)
        {
            foreach (var row in Rows)
            {
                if (!row.CheckIfFull())
                {
                    row.PlaceVisitors(group);
                }
                group.DefaultCheckAndCount();
                if (group.IsPlaced)
                {
                    break;
                }
            }
            CheckIfBackRowSeatsAreTaken();
            CheckIfFrontSeatsAreTaken();
        }

        private void PlaceInFirstRow(Group group)
        {
            Rows[0].PlaceVisitors(group);
            CheckIfFrontSeatsAreTaken();
        }

        private void PlaceInBackRows(Group group)
        {
            for (int i = 1; i < RowCount; i++)
            {
                Rows[i].PlaceVisitors(group);
                CheckIfBackRowSeatsAreTaken();
            }
        }
        #endregion

        #region Count
        private int CountTotalSeats()
        {
            TotalSeats = 0;

            foreach (var row in Rows)
            {
                TotalSeats += row.Seats.Count();
            }

            return TotalSeats;
        }

        public int CountAvailableSeats()
        {
            AvailableSeats = 0;

            foreach (var row in Rows)
            {
                row.CountAvailableSeats();
                AvailableSeats += row.AvailableSeats;
            }

            return AvailableSeats;
        }
        #endregion

        #region Check
        private bool CheckIfFrontSeatsAreTaken()
        {
            FrontSeatsTaken = false;
            if (Rows[0].CheckIfFull())
            {
                FrontSeatsTaken = true;
            }
            return FrontSeatsTaken;
        }

        public bool CheckIfBackRowSeatsAreTaken()
        {
            BackSeatsTaken = true;
            if (Rows.Count() > 1)
            {
                for (int i = 1; i < RowCount; i++)
                {
                    if (!Rows[i].CheckIfFull())
                    {
                        BackSeatsTaken = false;
                        break;
                    }
                }
            }
            else
            {
                BackSeatsTaken = true;
            }
            return BackSeatsTaken;
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

        public void Close()
        {
            Opened = false;
        }
        #endregion
    }
}
