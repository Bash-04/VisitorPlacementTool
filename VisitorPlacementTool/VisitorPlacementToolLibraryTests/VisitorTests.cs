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
    public class VisitorTests
    {
        [TestMethod()]
        public void SeatVisitorTest()
        {
            // Arrange
            Visitor visitor = new Visitor();
            Seat seat = new Seat(1, "A1");

            // Act
            seat.AssignVisitorToSeat(visitor);
            visitor.SeatVisitor(seat.Code);
            Console.WriteLine($"Seat {seat.Code} has {seat.Visitor.Name} assigned to it");
            Console.WriteLine($"{visitor.Name} is assigned to seat {visitor.AssignedSeat}");

            // Assert
            Assert.AreEqual(seat.Code, visitor.AssignedSeat);
        }
    }
}