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
        }

        // Methods

    }
}