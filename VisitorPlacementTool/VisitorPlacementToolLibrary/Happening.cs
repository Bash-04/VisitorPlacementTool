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

        // Constructors
        public Happening()
        {
            Id = Guid.NewGuid().ToString();
            Random random = new Random();
            int daysToSignup = random.Next(1, 200);
            SignupDeadline = DateOnly.FromDateTime(DateTime.Now.AddMilliseconds(daysToSignup));
            Sectors = new List<Sector>();
        }

        // Methods
        public bool CreateSectors()
        {
            bool sectorsHaveBeenCreated = false;
            Random random = new Random();

            int sectorCount = random.Next(3, 27);

            for (int i = 0; i < sectorCount; i++)
            {
                Char sectorLetter = (Char)((true ? 65 : 97) + (Sectors.Count()));

                Sector sector = new Sector();
                sector.CreateRows(sectorLetter);
                Sectors.Add(sector);
            }

            if (Sectors.Count() == sectorCount)
            {
                sectorsHaveBeenCreated = true;
            }

            CountMaxVisitors();

            return sectorsHaveBeenCreated;
        }

        public int CountMaxVisitors()
        {
            MaxVisitors = 0;

            foreach (var sector in Sectors)
            {
                MaxVisitors += sector.TotalSeats;
            }

            return MaxVisitors;
        }
    }
}