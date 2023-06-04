using Microsoft.VisualStudio.TestTools.UnitTesting;
using VisitorPlacementToolLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace VisitorPlacementToolLibrary.Tests
{
    [TestClass()]
    public class GroupTests
    {
        [TestMethod()]
        public void DefaultCheckAndCountTest()
        {
            // Arrange
            Group group = new Group();
            for (int i = 0; i < 10; i++)
            {
                Visitor visitor = new Visitor();
                group.Visitors.Add(visitor);
            }

            // Act
            group.DefaultCheckAndCount();
            Console.WriteLine($"{group.ChildrenCount} children");
            Console.WriteLine($"{group.AdultCount} adults");

            Visitor earliestSignup = group.Visitors.OrderBy(x => x.SignupDate).First();

            Visitor visitorCheck = group.Visitors.Where(x => x.SignupDate == group.EarliestSignupDate).ToList().Last();

            // Assert
            Assert.AreEqual(group.ChildrenCount, group.Visitors.Count(x => x.Adult == false));
            Assert.AreEqual(earliestSignup.SignupDate, visitorCheck.SignupDate);
            Assert.AreEqual(group.Visitors.Count(), group.Visitors.Count(x => x.Seated == false));
        }

        [TestMethod()]
        public void OrderGroupByAgeTest()
        {
            // Arrange
            Group group = new Group();
            for (int i = 0; i < 10; i++)
            {
                Visitor visitor = new Visitor();
                group.Visitors.Add(visitor);
            }

            Group orginalGroup = group;

            // Act
            Console.WriteLine("Original sorting");
            foreach (var visitor in orginalGroup.Visitors)
            {
                Console.WriteLine($"    {visitor.Name} - {(visitor.Adult ? "adult" : "child")} - {visitor.Age} years old");
            }

            group.OrderGroupByAge();
            Console.WriteLine("Sorted by age");
            foreach (var visitor in group.Visitors)
            {
                Console.WriteLine($"    {visitor.Name} - {(visitor.Adult ? "adult" : "child")} - {visitor.Age} years old");
            }
            
            // Assert
            Assert.AreEqual(orginalGroup.OrderGroupByAge(), group);
        }
    }
}