using System.Security.Cryptography.X509Certificates;
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
        public bool Full { get; private set; }
        public bool FrontSeatsTaken { get; private set; }

        // Constructors
        public Happening()
        {
            Sectors = new List<Sector>();
            Registrations = new List<Group>();

            Id = Guid.NewGuid().ToString();
            Random random = new Random();
            int daysToSignup = random.Next(1, 31);
            SignupDeadline = DateOnly.FromDateTime(DateTime.Now.AddDays(-daysToSignup));


            CreateSectors();
            CreateRandomVisitors();
        }

        // Methods
        #region Create
        private bool CreateSectors()
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

        private bool CreateRandomVisitors()
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

                if (!ExecuteCreateGroupChecks(group))
                {
                    continue;
                }
                groupedVisitors += group.Visitors.Count();
            }

            return visitorsAreCreated;
        }
        #endregion

        #region Place Visitors
        public void PlaceVisitors()
        {
            CountAvailableSeats();
            OrderGroups();
            PlaceGroups();
            UnsortedVisitors = Registrations.Sum(group => group.UnseatedGroupMembers);
        }

        private void PlaceGroups()
        {
            foreach (var group in Registrations)
            {
                // Loop twice to make sure all group members are placed - if there aren't enough seats a group will be skipped
                // was a while loop, but that caused an infinite loop because some groups were never placed because of rules in the PlaceInSector method
                for (int i = 0; i < 2; i++)
                {
                    if (AvailableSeats >= group.Visitors.Count())
                    {
                        group.OrderGroupByAge();
                        group.DefaultCheckAndCount();
                        TryPlaceInSector(group);
                        CountAvailableSeats();
                    }
                    else
                    {
                        break;
                    }
                    ExecuteHappeningChecks();
                }
            }
        }

        private void TryPlaceInSector(Group group)
        {
            foreach (var sector in Sectors)
            {
                group.DefaultCheckAndCount();
                // if sector is full skip to next sector and check if unseated group members fit in the sector
                if (!sector.CheckIfFull())
                {
                    // try placing the group in the sector
                    PlaceInSector(sector, group);

                    // if group is not yet placed, place group in next sector
                    if (!group.IsPlaced)
                    {
                        continue;
                    }
                    break;
                }
            }
        }

        private void PlaceInSector(Sector sector, Group group)
        {
            // if group contains children and the sector has front seats available, place children in front seats
            if (group.ContainsChildren && !sector.FrontSeatsTaken && !group.ChildrenArePlaced)
            {
                PlaceChildrenInSector(sector, group);
            }
            // if group does not contain children place group in sector
            else if (!group.ContainsChildren || group.ChildrenArePlaced)
            {
                sector.PlaceInRow(group);
            }
        }

        public void PlaceChildrenInSector(Sector sector, Group group)
        {
            // Make sure there are enough seats available to put a parent with the children in the front row
            if (sector.Rows[0].AvailableSeats > group.ChildrenCount)
            {
                sector.PlaceInFirstRow(group);
                group.DefaultCheckAndCount();
                if (group.ChildrenArePlaced && !group.IsPlaced)
                {
                    sector.PlaceInRow(group);
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
        private bool ExecuteCreateGroupChecks(Group group)
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

        public void ExecuteHappeningChecks()
        {
            CheckIfFull();
            CheckIfFrontSeatsTaken();
        }

        private bool CheckIfFrontSeatsTaken()
        {
            FrontSeatsTaken = true;
            foreach (var sector in Sectors)
            {
                if (!sector.FrontSeatsTaken)
                {
                    FrontSeatsTaken = false;
                    break;
                }
            }
            return FrontSeatsTaken;
        }

        private bool CheckIfFull()
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
            OrderRegistrationsByChildrenCount();
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
        private void OrderRegistrationsByChildrenCount()
        {
            var orderedGroupsOnChildrenCount = Registrations.OrderByDescending(x => x.ChildrenCount);
            Registrations = orderedGroupsOnChildrenCount.ToList();
        }

        private void OrderSectors()
        {
            OrderSectorsByTotalSeats();
        }
        private void OrderSectorsByTotalSeats()
        {
            var orderedSectionsOnRowCount = Sectors.OrderByDescending(x => x.TotalSeats);
            Sectors = orderedSectionsOnRowCount.ToList();
        }
        #endregion
    }
}
