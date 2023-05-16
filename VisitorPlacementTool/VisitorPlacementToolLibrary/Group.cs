using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisitorPlacementToolLibrary
{
    public class Group
    {
        public string Id { get; private set; }
        public List<Visitor> Visitors { get; private set; }
    }
}