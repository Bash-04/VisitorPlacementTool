using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisitorPlacementToolLibrary
{
    public class Event
    {
        public int Id { get; private set; }
        public int MaxVisitors { get; private set; }
        public DateOnly SignupDeadline { get; private set; }
        public List<Section> Sections { get; private set; }
        public List<Group> Registrations { get; private set; }
    }
}