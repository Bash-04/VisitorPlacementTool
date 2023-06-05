using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace VisitorPlacementToolLibrary
{
    public class Happening
    {
        // Properties
        public string Id { get; private set; }
        public int MaxVisitors { get; private set; }
        private DateTime SignupDeadline { get; set; }
        public List<Sector> Sectors { get; private set; }
        public List<Group> Registrations { get; set; }
        public int AvailableSeats { get; private set; }
        public int VisitorCount { get; private set; }
        public int UnseatedVisitors { get; private set; }
        private bool Full { get; set; }
        private bool FrontSeatsTaken { get; set; }
        private bool BackSeatsTaken { get; set; }
        private int MaxRowLenght { get; set; }
        public int ClosedSectors { get; private set; }

        // Constructors
        public Happening()
        {
            Sectors = new List<Sector>();
            Registrations = new List<Group>();

            Id = Guid.NewGuid().ToString();
            Random random = new Random();
            int daysToSignup = random.Next(1, 31);
            SignupDeadline = DateTime.Now.AddDays(-daysToSignup);

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
            MaxRowLenght = 10;

            for (int i = 0; i < sectorCount; i++)
            {
                Char sectorLetter = (Char)((true ? 65 : 97) + (Sectors.Count()));
                int RowsCount = random.Next(1, 4);
                int RowLength = random.Next(3, MaxRowLenght+1);

                Sector sector = new Sector(sectorLetter, RowsCount, RowLength);
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
                    // If visitor is too late, don't add to group
                    if (visitor.SignupDate < SignupDeadline)
                    {
                        group.Visitors.Add(visitor);
                    }
                    else
                    {
                        i--;
                        continue;
                    }
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
            OrderGroups();
            PlaceGroups();
            UnseatedVisitors = Registrations.Sum(group => group.UnseatedGroupMembers);
            CloseUnusedSectors();
        }

        private void CloseUnusedSectors()
        {
            foreach (var sector in Sectors)
            {
                if (sector.AvailableSeats == sector.TotalSeats)
                {
                    sector.Close();
                    ClosedSectors++;
                }
            }
        }

        private void PlaceGroups()
        {
            foreach (var group in Registrations)
            {
                // while loop to make sure all group members are placed - if there aren't enough seats a group will be skipped
                CountAvailableSeats();
                while (!group.IsPlaced)
                {
                    // if there are enough seats available for the group, place the group
                    if (AvailableSeats >= group.Visitors.Count())
                    {
                        group.OrderGroupByAge();
                        group.DefaultCheckAndCount();
                        if (!TryPlaceInSector(group))
                        {
                            break;
                        }
                        CountAvailableSeats();
                    }
                    else
                    {
                        break;
                    }
                    ExecuteHappeningChecks();
                }
                // if the happening is full, stop placing groups
                if (Full)
                {
                    break;
                }
            }
        }

        private bool TryPlaceInSector(Group group)
        {
            bool groupCanBePlaced = true;
            // while loop to make sure all group members are placed - if there aren't enough seats a group will be skipped
            while (!group.IsPlaced && groupCanBePlaced)
            {
                ExecuteHappeningChecks();
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
                // if no group members are placed in any sector, group cannot be placed
                if (group.UnseatedGroupMembers == group.Visitors.Count())
                {
                    groupCanBePlaced = false;
                }
            }
            return groupCanBePlaced;
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
                sector.PlaceInBackRows(group);
            }
            // if back seats are taken and front seats are available, place group in front seats
            if (CheckIfBackSeatsTaken() && !sector.FrontSeatsTaken) 
            { 
                sector.PlaceInFirstRow(group);
            }
        }

        private void PlaceChildrenInSector(Sector sector, Group group)
        {
            // Make sure there are enough seats available to put a parent with the children in the front row
            if (sector.Rows[0].AvailableSeats > group.ChildrenCount)
            {
                sector.PlaceInFirstRow(group);
                group.DefaultCheckAndCount();
                // if children are placed and not all group members are placed, place the rest of the group in the back rows
                if (group.ChildrenArePlaced && !group.IsPlaced)
                {
                    sector.PlaceInRow(group);
                }
            }
        }
        #endregion

        #region Count
        private int CountMaxVisitors()
        {
            MaxVisitors = 0;

            foreach (var sector in Sectors)
            {
                MaxVisitors += sector.TotalSeats;
            }

            return MaxVisitors;
        }

        private int CountAvailableSeats()
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
        public bool ExecuteCreateGroupChecks(Group group)
        {
            bool groupIsValid = true;
            // if the group contains to many children for being on the first row with at least one adult, skip the group
            if (group.ChildrenCount > MaxRowLenght-1 && group.AdultCount < 2)
            {
                groupIsValid = false;
            }
            // If the group contains an adult, add it to the list of groups
            else if (!group.ContainsAdult)
            {
                groupIsValid = false;
            }
            else if (group.ChildrenCount > 9 && group.AdultCount > 1)
            {
                List<Group> groups = SplitGroup(group);
                foreach (var g in groups)
                {
                    g.DefaultCheckAndCount();
                    Registrations.Add(g);
                }
            }
            else
            {
                group.DefaultCheckAndCount();
                Registrations.Add(group);
            }
            return groupIsValid;
        }

        private List<Group> SplitGroup(Group group)
        {
            Group group1 = new Group();
            Group group2 = new Group();

            int countVisitor1 = 0;
            int countVisitor2 = 1;
            for (int i = 0; i < group.Visitors.Count(); i++)
            {
                if (countVisitor1 >= group.Visitors.Count())
                {
                    break;
                }
                group1.Visitors.Add(group.Visitors[countVisitor1]);
                countVisitor1 += 2;
                if (group.Visitors.Count() >= countVisitor2)
                {
                    group2.Visitors.Add(group.Visitors[countVisitor2]);
                    countVisitor2 += 2;
                }
            }
            List<Group> groups = new List<Group>
            {
                group1,
                group2
            };

            return groups;
        }

        private void ExecuteHappeningChecks()
        {
            CheckIfFull();
            CheckIfFrontSeatsTaken();
            CheckIfBackSeatsTaken();
        }

        private bool CheckIfBackSeatsTaken()
        {
            BackSeatsTaken = true;
            foreach (var sector in Sectors)
            {
                if (!sector.CheckIfBackRowSeatsAreTaken())
                {
                    BackSeatsTaken = false;
                    break;
                }
            }
            return BackSeatsTaken;
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

        #region Order Registrations
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
        #endregion

        #region Order Sectors
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

        #endregion
    }
}
