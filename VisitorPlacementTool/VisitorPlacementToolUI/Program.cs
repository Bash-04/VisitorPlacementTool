// See https://aka.ms/new-console-template for more information
using System.Numerics;
using static System.Collections.Specialized.BitVector32;
using VisitorPlacementToolLibrary;
using System.Runtime.ExceptionServices;

Console.WriteLine("Hello, World!");

VPT vpt = new VPT();

vpt.TryCreateNewHappening();

vpt.TryCreateRandomVisitors();

foreach (var sector in vpt.Happening.Sectors)
{
    foreach(var row in sector.Rows)
    {
        foreach(var seat in row.Seats)
        {
            Console.WriteLine($"{seat.Code} - {seat.Visitor.Name}");
        }
    }
}
Console.WriteLine($"{vpt.Happening.Sectors.Count()} Sectors");
Console.WriteLine($"{vpt.Happening.MaxVisitors} Seats");
Console.WriteLine();

foreach (var group in vpt.Groups)
{
    Console.WriteLine($"{group.Id} - {group.Visitors.Count()}");
    foreach (var visitor in group.Visitors)
    {
        Console.WriteLine($"    {visitor.Name} - {visitor.Adult} - {visitor.AssignedSeat}");
    }
    Console.WriteLine();
}

Console.WriteLine(vpt.RandomVisitorAmount);
