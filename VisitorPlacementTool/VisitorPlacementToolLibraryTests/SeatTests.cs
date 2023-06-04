using Microsoft.VisualStudio.TestTools.UnitTesting;
using VisitorPlacementToolLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorPlacementToolLibrary.Tests
{
    [TestClass()]
    public class SeatTests
    {
        [TestMethod()]
        public void AssignVisitorToSeatTest()
        {
            // Arrange
            Seat seat = new Seat(1, "A1");
            Visitor visitor = new Visitor();

            // Act
            seat.AssignVisitorToSeat(visitor);
            visitor.SeatVisitor(seat.Code);
            Console.WriteLine($"Seat {seat.Code} has {seat.Visitor.Name} assigned to it");
            Console.WriteLine($"{visitor.Name} is assigned to seat {visitor.AssignedSeat}");

            // Assert
            Assert.IsTrue(seat.Visitor == visitor);
        }
    }
}