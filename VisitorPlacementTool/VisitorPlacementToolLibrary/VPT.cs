namespace VisitorPlacementToolLibrary
{
    public class VPT
    {
        // Properties
        public List<Event> Events { get; private set; }

        // Constructors
        public VPT() 
        {
            Events = new List<Event>();
        }

        // Methods
        public bool TryCreateNewEvent()
        {
            bool eventIsCreated = false;

            return eventIsCreated;
        }
    }
}