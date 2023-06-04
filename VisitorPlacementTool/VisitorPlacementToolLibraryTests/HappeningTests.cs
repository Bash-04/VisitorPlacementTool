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
    public class HappeningTests
    {
        [TestMethod()]
        public void HappeningTest()
        {
            // Arrange
            Happening happening = new Happening();

            // Act
            Console.WriteLine($"{happening.Sectors.Count()} sectors created");
            Console.WriteLine($"{happening.Registrations.Count()} groups registered");

            // Assert
            Assert.IsTrue(happening.Sectors.Count() > 0 && happening.Registrations.Count() > 0);
        }

        [TestMethod()]
        public void PlaceVisitorsTest()
        {
            // Arrange
            Happening happening = new Happening();

            // Act
            happening.PlaceVisitors();
            Console.WriteLine($"{happening.MaxVisitors} max visitors for this event");
            Console.WriteLine($"{happening.Sectors.Sum(x => x.AvailableSeats)} empty seats for this event");
            Console.WriteLine();
            Console.WriteLine($"{happening.Registrations.Sum(x => x.Visitors.Count)} registered visitors");
            Console.WriteLine($"{happening.UnseatedVisitors} unseated visitors for this event");

            // Assert
            Assert.AreNotEqual(happening.AvailableSeats, happening.MaxVisitors);
        }
    }
}