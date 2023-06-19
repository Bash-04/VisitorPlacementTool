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


        [TestMethod()]
        public void PlaceVisitorsWithTenChildrenTest()
        {
            // Arrange
            Happening happening = new Happening();
            happening.Registrations = new List<Group>();
            Group group = new Group();
            DateOnly dateOfBirthChild = new DateOnly(2015, 1, 1);
            DateOnly dateOfBirthAdult = new DateOnly(2000, 1, 1);
            for (int i = 0; i < 10; i++)
            {
                group.Visitors.Add(new Visitor(dateOfBirthChild));
            }
            for (int i = 0; i < 2; i++)
            {
                group.Visitors.Add(new Visitor(dateOfBirthAdult));
            }
            group.DefaultCheckAndCount();

            // Act
            happening.ExecuteCreateGroupChecks(group);
            Console.WriteLine($"{happening.Registrations.Count()} groups for this event");
            foreach (var g in happening.Registrations)
            {
                Console.WriteLine(g.Id);
                foreach (var visitor in g.Visitors)
                {
                    Console.WriteLine($"{visitor.Name} - {(visitor.Adult ? "adult" : "child")}");
                }
                Console.WriteLine();
            }

            // Assert
            Assert.IsTrue(happening.Registrations.Count() == 2);
        }

        [TestMethod()]
        public void FailPlaceVisitorsWithTenChildrenAndOneParrentTest()
        {
            // Arrange
            Happening happening = new Happening();
            happening.Registrations = new List<Group>();
            Group group = new Group();
            DateOnly dateOfBirthChild = new DateOnly(2015, 1, 1);
            DateOnly dateOfBirthAdult = new DateOnly(2000, 1, 1);
            for (int i = 0; i < 10; i++)
            {
                group.Visitors.Add(new Visitor(dateOfBirthChild));
            }
            group.Visitors.Add(new Visitor(dateOfBirthAdult));
            group.DefaultCheckAndCount();

            // Act
            happening.ExecuteCreateGroupChecks(group);
            Console.WriteLine($"{happening.Registrations.Count()} groups for this event");
            foreach (var g in happening.Registrations)
            {
                Console.WriteLine(g.Id);
                foreach (var visitor in g.Visitors)
                {
                    Console.WriteLine($"{visitor.Name} - {(visitor.Adult ? "adult" : "child")}");
                }
                Console.WriteLine();
            }

            // Assert
            Assert.IsTrue(happening.Registrations.Count() == 0);
        }
    }
}