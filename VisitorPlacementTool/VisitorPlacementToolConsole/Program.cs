using VisitorPlacementToolLibrary;

Happening happening = new Happening();
happening.PlaceVisitors();

foreach (var sector in happening.Sectors)
{
    foreach (var row in sector.Rows)
    {
        foreach (var seat in row.Seats)
        {
            Console.WriteLine($"{seat.Code} - {seat.Visitor.Name}");
        }
    }
}
Console.WriteLine($"{happening.Sectors.Count()} sectors");
Console.WriteLine($"{happening.MaxVisitors} seats");
Console.WriteLine($"{happening.AvailableSeats} empty seats");
Console.WriteLine();

foreach (var group in happening.Registrations)
{
    Console.WriteLine($"{group.Id} - {group.Visitors.Count()} Visitors - {group.UnseatedGroupMembers} Unseated");
    foreach (var visitor in group.Visitors)
    {
        string adultOrChild = visitor.Adult ? "adult" : "child";
        Console.WriteLine($"    {visitor.Name} - {adultOrChild} - {visitor.AssignedSeat}");
    }
    Console.WriteLine();
}

Console.WriteLine($"{happening.Registrations.Count()} groups");
Console.WriteLine($"{happening.VisitorCount} visitors");
Console.WriteLine($"{happening.UnsortedVisitors} unseated visitors");
