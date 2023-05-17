using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisitorPlacementToolLibrary
{
    public class Group
    {
        // Properties
        public string Id { get; private set; }
        public List<Visitor> Visitors { get; private set; }

        // Constructors
        public Group()
        {
            Id = Guid.NewGuid().ToString();
            Visitors = new List<Visitor>();
        }

        // Methods
        public bool containAdult()
        {
            bool containsAdult = false;
            foreach (var visitor in Visitors)
            {
                if (visitor.Adult)
                {
                    containsAdult = true;
                    break;
                }
            }
            return containsAdult;
        }
    }
}