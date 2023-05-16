﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisitorPlacementToolLibrary
{
    public class Visitor
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public DateOnly DateOfBirth { get; private set; }
        public int Age { get; private set; }
        public bool Adult { get; private set; }
        public DateOnly SignupDate { get; private set; }
        public Seat Seat { get; private set; }
    }
}