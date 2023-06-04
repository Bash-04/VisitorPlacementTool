using Microsoft.VisualStudio.TestTools.UnitTesting;
using VisitorPlacementToolLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;

namespace VisitorPlacementToolLibrary.Tests
{
    [TestClass()]
    public class RowTests
    {
        [TestMethod()]
        public void CreateSeatsTest()
        {
            // Arrange
            Row row = new Row(1, 'A');

            // Act
            row.CreateSeats(5);
            foreach (Seat seat in row.Seats)
            {
                Console.WriteLine(seat.Code);
            }

            // Assert
            Assert.AreEqual(5, row.Seats.Count());
        }

        [TestMethod()]
        public Row PlaceVisitorsTest()
        {
            // Arrange
            Row row = new Row(1, 'A');
            row.CreateSeats(5);
            Group group = new Group();
            for (int i = 0; i < 5; i++)
            {
                group.Visitors.Add(new Visitor());
            }

            // Act
            row.PlaceVisitors(group);
            foreach (var seat in row.Seats)
            {
                Console.WriteLine($"{seat.Code} is {(seat.Occupied ? "occupied" : "empty")} by {seat.Visitor.Name}");
            }

            // Assert
            foreach (Visitor visitor in group.Visitors)
            {
                Assert.IsTrue(visitor.AssignedSeat != "");
            }
            return row;
        }

        [TestMethod()]
        public void CountAvailableSeatsTest()
        {
            // Arrange
            Row row = new Row(1, 'A');

            // Act
            row.CreateSeats(5);
            row.CountAvailableSeats();
            Console.WriteLine($"There are {row.AvailableSeats} available seats");

            // Assert
            Assert.AreEqual(row.Seats.Count(), row.AvailableSeats);
        }

        [TestMethod()]
        public void CheckIfFullTest()
        {
            // Arrange
            Row row = new Row(1, 'A');
            row.CreateSeats(5);
            Group group = new Group();
            for (int i = 0; i < 5; i++)
            {
                group.Visitors.Add(new Visitor());
            }
            row.PlaceVisitors(group);

            // Act
            Console.WriteLine($"There are {row.CountAvailableSeats()} available seats");
            bool full = row.CheckIfFull();
            Console.WriteLine($"The row is {(full ? "full" : "not full")}");

            // Assert
            Assert.IsTrue(full);
        }
    }
}