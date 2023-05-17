﻿// See https://aka.ms/new-console-template for more information
using System.Numerics;
using static System.Collections.Specialized.BitVector32;
using VisitorPlacementToolLibrary;
using System.Runtime.ExceptionServices;

Console.WriteLine("Hello, World!");

VPT vpt = new VPT();

vpt.TryCreateNewHappening();

foreach (var happening in vpt.Happenings)
{
    foreach (var sector in happening.Sectors)
    {
        foreach(var row in sector.Rows)
        {
            foreach(var seat in row.Seats)
            {
                Console.WriteLine(seat.Code);
            }
        }
    }
    Console.WriteLine($"{happening.Sectors.Count()} Sectors");
    Console.WriteLine($"{happening.MaxVisitors} Seats");
    Console.WriteLine();
}
