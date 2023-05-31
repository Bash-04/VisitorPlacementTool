using System.Text.RegularExpressions;

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
        public int VisitorCount { get; private set; }
        public int UnsortedVisitors { get; private set; }
        public bool Full { get; set; }

        // Constructors
        public Happening()
        {
            Id = Guid.NewGuid().ToString();
            Random random = new Random();
            int daysToSignup = random.Next(1, 31);
            SignupDeadline = DateOnly.FromDateTime(DateTime.Now.AddDays(-daysToSignup));
            Sectors = new List<Sector>();
            Registrations = new List<Group>();
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

            OrderSectors();
            CountMaxVisitors();

            return sectorsHaveBeenCreated;
        }

        public bool CreateRandomVisitors()
        {
            bool visitorsAreCreated = false;

            int minVisitors = Convert.ToInt32(MaxVisitors * .8);
            int maxVisitors = Convert.ToInt32(MaxVisitors * 1.3);

            Random random = new Random();
            VisitorCount = random.Next(minVisitors, maxVisitors);
            int groupedVisitors = 0;

            while (groupedVisitors != VisitorCount)
            {
                int groupSize = 0;
                Group group = new Group();

                if (VisitorCount - groupedVisitors >= 20)
                {
                    groupSize = random.Next(1, 21);
                }
                else
                {
                    groupSize = random.Next(1, VisitorCount - groupedVisitors);
                }

                for (int i = 0; i < groupSize; i++)
                {
                    Visitor visitor = new Visitor();
                    group.Visitors.Add(visitor);
                }
                group.DefaultCheckAndCount();

                if (!ExecuteChecks(group))
                {
                    continue;
                }
                groupedVisitors += group.Visitors.Count();
            }

            return visitorsAreCreated;
        }
        #endregion

        #region Sort
        public void PlaceVisitors()
        {
            CountAvailableSeats();
            OrderGroups();
            PlaceGroups();
            UnsortedVisitors = Registrations.Sum(group => group.UnsortedGroupMembers);
        }

        private void PlaceGroups()
        {
            foreach (var group in Registrations)
            {
                if (AvailableSeats > group.Visitors.Count())
                {
                    group.OrderGroupByAge();
                    group.DefaultCheckAndCount();
                    PlaceInSector(group);
                    CountAvailableSeats();
                }
                else
                {
                    continue;
                }
            }
        }

        private void PlaceInSector(Group group)
        {
            foreach (var sector in Sectors)
            {
                if (!sector.CheckIfFull())
                {
                    sector.PlaceInRow(group);
                    if (!group.IsPlaced)
                    {
                        continue;
                    }
                    break;
                }
            }
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
        public bool ExecuteChecks(Group group)
        {
            bool groupIsValid = false;
            // If the group contains an adult, add it to the list of groups
            if (group.ChildrenCount > 9 && group.AdultCount < 2)
            {
            }
            else if (!group.ContainsAdult)
            {
            }
            else
            {
                group.DefaultCheckAndCount();
                Registrations.Add(group);
                groupIsValid = true;
            }
            return groupIsValid;
        }

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
        private void OrderGroups()
        {
            OrderRegistrationsBySignupDate();
            OrderRegistrationsBySize();
        }
        private void OrderRegistrationsBySignupDate()
        {
            var orderedGroupsOnSignupDate = Registrations.OrderBy(x => x.EarliestSignupDate.Date);
            Registrations = orderedGroupsOnSignupDate.ToList();
        }
        private void OrderRegistrationsBySize()
        {
            var orderedGroupsOnSize = Registrations.OrderByDescending(x => x.Visitors.Count());
            Registrations = orderedGroupsOnSize.ToList();
        }

        private void OrderSectors()
        {
            OrderSectorsByRowCount();
            OrderSectorsByLenght();
        }
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