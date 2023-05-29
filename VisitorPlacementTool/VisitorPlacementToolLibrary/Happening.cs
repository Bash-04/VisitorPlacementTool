using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisitorPlacementToolLibrary
{
    public class Happening
    {
        // Properties
        public string Id { get; private set; }
        public int MaxVisitors { get; private set; }
        public DateOnly SignupDeadline { get; private set; }
        public List<Sector> Sectors { get; private set; }
        public List<Group> Registrations { get; private set; }
        public int AvailableSeats { get; private set; }
        public bool Full { get; set; }

        // Constructors
        public Happening()
        {
            Id = Guid.NewGuid().ToString();
            Random random = new Random();
            int daysToSignup = random.Next(1, 31);
            SignupDeadline = DateOnly.FromDateTime(DateTime.Now.AddDays(-daysToSignup));
            Sectors = new List<Sector>();
        }

        // Methods
        #region Create
        public bool CreateSectors()
        {
            bool sectorsHaveBeenCreated = false;
            Random random = new Random();

            int sectorCount = random.Next(3, 27);

            for (int i = 0; i < sectorCount; i++)
            {
                Char sectorLetter = (Char)((true ? 65 : 97) + (Sectors.Count()));
                int RowsCount = random.Next(1, 4);
                int RowLength = random.Next(3, 11);

                Sector sector = new Sector(sectorLetter, RowsCount, RowLength);
                sector.CreateRows();
                Sectors.Add(sector);
            }

            if (Sectors.Count() == sectorCount)
            {
                sectorsHaveBeenCreated = true;
            }

            OrderSectorsByRowCount();
            OrderSectorsByLenght();
            CountMaxVisitors();

            return sectorsHaveBeenCreated;
        }
        #endregion

        #region Count
        public int CountMaxVisitors()
        {
            MaxVisitors = 0;

            foreach (var sector in Sectors)
            {
                MaxVisitors += sector.TotalSeats;
            }

            return MaxVisitors;
        }

        public int CountAvailableSeats()
        {
            AvailableSeats = 0;
            foreach (var sector in Sectors)
            {
                sector.CountAvailableSeats();
                AvailableSeats += sector.AvailableSeats;
            }
            return AvailableSeats;
        }
        #endregion

        #region Check
        public bool CheckIfFull()
        {
            Full = true;
            foreach (var sector in Sectors)
            {
                if (!sector.Full)
                {
                    Full = false;
                    break;
                }
            }
            return Full;
        }
        #endregion

        #region Order by
        private void OrderSectorsByRowCount()
        {
            var orderedSectionsOnRowCount = Sectors.OrderByDescending(x => x.RowCount);
            Sectors = orderedSectionsOnRowCount.ToList();
        }

        private void OrderSectorsByLenght()
        {
            var orderedSectionsOnLenght = Sectors.OrderByDescending(x => x.RowLength);
            Sectors = orderedSectionsOnLenght.ToList();
        }
        #endregion
    }
}