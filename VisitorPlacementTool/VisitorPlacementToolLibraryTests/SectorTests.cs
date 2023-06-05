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
    public class SectorTests
    {
        [TestMethod()]
        public void CreateRowsTest()
        {
            // Arrange
            char a = 'A';
            Sector sector = new Sector(a, 2, 7);

            // Act
            sector.CreateRows();
            Console.WriteLine($"{sector.Rows.Count()} rows");
            Console.WriteLine($"{sector.Rows.Sum(x => x.Seats.Count())} seats");

            // Assert
            Assert.IsTrue(sector.Rows.Count > 0);
        }

        [TestMethod()]
        public void PlaceInRowTest()
        {
            // Arrange
            Sector sector = new Sector('A', 2, 7);
            Group group = new Group();
            for (int i = 0; i < 10; i++)
            {
                group.Visitors.Add(new Visitor());
            }
            group.OrderGroupByAge();
            sector.CreateRows();

            // Act
            sector.PlaceInRow(group);
            foreach (var vis in group.Visitors)
            {
                Console.WriteLine($"{vis.Name} is an {(vis.Adult ? "adult" : "child")} and is placed in {vis.AssignedSeat}");
            }

            // Assert
            foreach (var vis in group.Visitors)
            {
                Assert.IsTrue(vis.AssignedSeat != "");
            }
        }

        [TestMethod()]
        public void PlaceInFirstRowTest()
        {
            // Arrange
            Sector sector = new Sector('A', 2, 7);
            Group group = new Group();
            for (int i = 0; i < 5; i++)
            {
                group.Visitors.Add(new Visitor());
            }
            group.OrderGroupByAge();
            sector.CreateRows();

            // Act
            sector.PlaceInFirstRow(group);
            foreach (var vis in group.Visitors)
            {
                Console.WriteLine($"{vis.Name} is an {(vis.Adult ? "adult" : "child")} and is placed in {vis.AssignedSeat}");
            }
            foreach (var row in sector.Rows)
            {
                row.CountAvailableSeats();
            }

            // Assert
            Assert.IsTrue(sector.Rows[0].AvailableSeats == 2);
            Assert.IsTrue(sector.Rows[1].AvailableSeats == 7);
        }

        [TestMethod()]
        public void PlaceInBackRowsTest()
        {
            // Arrange
            Sector sector = new Sector('A', 2, 7);
            Group group = new Group();
            for (int i = 0; i < 7; i++)
            {
                group.Visitors.Add(new Visitor());
            }
            group.OrderGroupByAge();
            sector.CreateRows();

            // Act
            sector.PlaceInBackRows(group);
            foreach (var vis in group.Visitors)
            {
                string childOrAdult = vis.Adult ? "adult" : "child";
                if (childOrAdult == "child")
                {
                    Console.WriteLine($"{vis.Name} is an {childOrAdult} and is not placed");
                }
                else
                {
                    Console.WriteLine($"{vis.Name} is an {childOrAdult} and is placed in {vis.AssignedSeat}");
                }
            }
            foreach (var row in sector.Rows)
            {
                row.CountAvailableSeats();
            }

            // Assert
            Assert.IsTrue(sector.Rows[1].AvailableSeats == group.Visitors.Count() - group.Visitors.Count(x => x.Adult == true));
        }

        [TestMethod()]
        public void CountAvailableSeatsTest()
        {
            // Arrange
            char a = 'A';
            Sector sector = new Sector(a, 3, 7);

            // Act
            Console.WriteLine($"{sector.Rows.Count()} rows");
            Console.WriteLine($"{sector.Rows.Sum(x => x.Seats.Count())} seats");

            // Assert
            Assert.IsTrue(sector.Rows.Sum(x => x.Seats.Count()) == 21);
        }

        [TestMethod()]
        public void CheckIfBackRowSeatsAreTakenTest()
        {
            // Arrange
            Sector sector = new Sector('A', 2, 7);
            Group group = new Group();
            DateOnly dateOfBirth = new DateOnly(2000, 1, 1);
            for (int i = 0; i < 7; i++)
            {
                group.Visitors.Add(new Visitor(dateOfBirth));
            }
            sector.CreateRows();

            // Act
            sector.PlaceInBackRows(group);
            foreach (var vis in group.Visitors)
            {
                string childOrAdult = vis.Adult ? "adult" : "child";
                if (childOrAdult == "child")
                {
                    Console.WriteLine($"{vis.Name} is an {childOrAdult} and is not placed");
                }
                else
                {
                    Console.WriteLine($"{vis.Name} is an {childOrAdult} and is placed in {vis.AssignedSeat}");
                }
            }
            foreach (var row in sector.Rows)
            {
                row.CountAvailableSeats();
            }

            // Assert
            Assert.IsTrue(sector.CheckIfBackRowSeatsAreTaken());
        }

        [TestMethod()]
        public void CheckIfFullTest()
        {
            // Arrange
            Sector sector = new Sector('A', 2, 7);
            Group group = new Group();
            for (int i = 0; i < 14; i++)
            {
                group.Visitors.Add(new Visitor());
            }
            group.OrderGroupByAge();
            sector.CreateRows();

            // Act
            sector.PlaceInRow(group);
            foreach (var vis in group.Visitors)
            {
                string childOrAdult = vis.Adult ? "adult" : "child";
                Console.WriteLine($"{vis.Name} is an {childOrAdult} and is placed in {vis.AssignedSeat}");
            }

            // Assert
            foreach (var vis in group.Visitors)
            {
                Assert.IsTrue(vis.AssignedSeat != "");
            }
        }

        [TestMethod()]
        public void CloseSectorTest()
        {
            // Arrange
            Sector sector = new Sector('A', 2, 7);
            sector.CountAvailableSeats();

            // Act
            if (sector.TotalSeats == sector.AvailableSeats)
            {
                sector.Close();
            }
            Console.WriteLine($"Sector opened: {sector.Opened}");
            
            // Assert
            Assert.IsFalse(sector.Opened);
        }
    }
}