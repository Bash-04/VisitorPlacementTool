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
        public bool TryCreateNewHappening()
        {
            bool happeningIsCreated = false;

            Happening happening = new Happening();

            happening.CreateSectors();

            Happenings.Add(happening);

            return happeningIsCreated;
        }

        public bool TryCreateRandomVisitors()
        {
            bool visitorsAreCreated = false;

            Random random = new Random();
            int visitorCount = random.Next(20, 800);
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

                groupedVisitors += group.Visitors.Count();
                groups.Add(group);
            }

            RandomVisitorAmount = visitorCount;

            return visitorsAreCreated;
        }
    }
}