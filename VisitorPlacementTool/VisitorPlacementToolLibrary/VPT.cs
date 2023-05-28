namespace VisitorPlacementToolLibrary
{
    public class VPT
    {
        // Properties
        public Happening Happening { get; private set; }
        public List<Group> Groups { get; private set; }
        public int RandomVisitorAmount { get; private set; }

        // Constructors
        public VPT() 
        {
            Groups = new List<Group>();
            RandomVisitorAmount = 0;
        }

        // Methods
        #region Create happening
        public bool TryCreateNewHappening()
        {
            bool happeningIsCreated = false;

            Happening happening = new Happening();

            happening.CreateSectors();

            Happening = happening;

            return happeningIsCreated;
        }
        #endregion

        #region Create and group visitors
        public bool TryCreateRandomVisitors()
        {
            bool visitorsAreCreated = false;

            int minVisitors = Convert.ToInt32(Happening.MaxVisitors*.8);
            int maxVisitors = Convert.ToInt32(Happening.MaxVisitors*1.3);

            Random random = new Random();
            int visitorCount = random.Next(minVisitors, maxVisitors);
            int groupedVisitors = 0;

            while (groupedVisitors != visitorCount)
            {
                int groupSize = 0;
                Group group = new Group();

                if (visitorCount - groupedVisitors >= 20)
                {
                    groupSize = random.Next(1, 21);
                }
                else
                {
                    groupSize = random.Next(1, visitorCount - groupedVisitors);
                }

                for (int i = 0; i < groupSize; i++)
                {
                    Visitor visitor = new Visitor();
                    group.Visitors.Add(visitor); 
                    group.CheckIfAdultAndEarliestSignup();
                }

                // If the group contains an adult, add it to the list of groups
                if (!group.ContainsAdult)
                {
                    break;
                }

                groupedVisitors += group.Visitors.Count();
                Groups.Add(group);
            }

            RandomVisitorAmount = visitorCount;

            return visitorsAreCreated;
        }
        #endregion

        #region Sorting algorithm
        public void StartSorting()
        {
            OrderGroupsBySignupDate();
            SortGroups();
        }

        #region Sorting algorithm visitors over seats
        private void SortGroups()
        {
            foreach (var group in Groups)
            {
                if (Happening.CheckIfFull())
                {
                    Happening.Full = true;
                }
                else
                {
                    SortVisitors(group);
                }
            }
        }

        private void SortVisitors(Group group)
        {
            foreach (var visitor in group.Visitors)
            {
                SortOverSectors(visitor);
            }
        }

        private void SortOverSectors(Visitor visitor)
        {
            foreach (var sector in Happening.Sectors)
            {
                if (sector.Full)
                {
                }
                else
                {
                    SortOverRows(sector, visitor);
                    if (visitor.AssignedSeat != "")
                    {
                        sector.CheckIfFull();
                        break;
                    }
                }
            }
        }

        private void SortOverRows(Sector sector, Visitor visitor)
        {
            foreach (var row in sector.Rows)
            {
                if (row.CheckIfFull())
                {
                    row.Full = true;
                }
                else
                {
                    SortOverSeats(row, visitor);
                    if (visitor.AssignedSeat != "")
                    {
                        row.CheckIfFull();
                        break;
                    }
                }
            }
        }

        private void SortOverSeats(Row row, Visitor visitor)
        {
            foreach (var seat in row.Seats)
            {
                if (seat.Visitor.Name == "")
                {
                    seat.AssignVisitorToSeat(visitor);
                    visitor.AssignedSeat = seat.Code;
                    break;
                }
            }
        }
        #endregion

        private void OrderGroupsBySignupDate()
        {
            var orderedGroupsOnSignupDate = Groups.OrderBy(x => x.EarliestSignupDate.Date);
            Groups = orderedGroupsOnSignupDate.ToList();
        }
        #endregion
    }
}
