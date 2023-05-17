namespace VisitorPlacementToolLibrary
{
    public class VPT
    {
        // Properties
        public List<Happening> Happenings { get; private set; }
        public List<Group> groups { get; private set; }
        public int RandomVisitorAmount { get; private set; }

        // Constructors
        public VPT() 
        {
            Happenings = new List<Happening>();
            groups = new List<Group>();
            RandomVisitorAmount = 0;
        }

        // Methods
        #region Create happening
        public bool TryCreateNewHappening()
        {
            bool happeningIsCreated = false;

            Happening happening = new Happening();

            happening.CreateSectors();

            Happenings.Add(happening);

            return happeningIsCreated;
        }
        #endregion

        #region Create and group visitors
        public bool TryCreateRandomVisitors()
        {
            bool visitorsAreCreated = false;

            int minVisitors = Convert.ToInt32(Happenings[0].MaxVisitors*.8);
            int maxVisitors = Convert.ToInt32(Happenings[0].MaxVisitors*1.3);

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
                }

                if (!group.containAdult())
                {
                    break;
                }

                groupedVisitors += group.Visitors.Count();
                groups.Add(group);
            }

            RandomVisitorAmount = visitorCount;

            return visitorsAreCreated;
        }
        #endregion

        #region Sorting algorithm
        public bool SortVisitors()
        {
            bool visitorsAreSorted = false;

            foreach (var happening in Happenings)
            {
                visitorsAreSorted = true;
            }

            return visitorsAreSorted;
        }
        #endregion
    }
}
