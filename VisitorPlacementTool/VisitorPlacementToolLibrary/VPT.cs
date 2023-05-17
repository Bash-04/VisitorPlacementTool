namespace VisitorPlacementToolLibrary
{
    public class VPT
    {
        // Properties
        public List<Happening> Happenings { get; private set; }

        // Constructors
        public VPT() 
        {
            Happenings = new List<Happening>();
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
    }
}