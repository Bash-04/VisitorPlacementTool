using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisitorPlacementToolLibrary
{
    public class Group
    {
        // Properties
        public string Id { get; private set; }
        public List<Visitor> Visitors { get; private set; }
        public DateTime EarliestSignupDate { get; private set; }
        public bool ContainsAdult { get; private set; }
        public int ChildrenCount { get; private set; }
        public int AdultCount { get; private set; }
        public int UnsortedGroupMembers { get; set; }

        // Constructors
        public Group()
        {
            Id = Guid.NewGuid().ToString();
            Visitors = new List<Visitor>();
        }

        // Methods
        #region Check
        public void DefaultCheckAndCount()
        {
            CountAdultsAndChildren();
            CheckIfNewVisitorHasEarliestSignupDate();
        }

        private bool CheckIfNewVisitorHasEarliestSignupDate()
        {
            bool hasEarliestSignupDate = false;
            if (Visitors.Count() == 1)
            {
                EarliestSignupDate = Visitors[0].SignupDate;
                hasEarliestSignupDate = true;
            }
            else
            {
                for (int i = Visitors.Count() - 1; i < Visitors.Count(); i++)
                {
                    if (Visitors[i].SignupDate < EarliestSignupDate)
                    {
                        EarliestSignupDate = Visitors[i].SignupDate;
                        hasEarliestSignupDate = true;
                        break;
                    }
                }
            }
            return hasEarliestSignupDate;
        }
        #endregion

        #region Count
        private bool CountAdultsAndChildren()
        {
            ContainsAdult = false;
            foreach (var visitor in Visitors)
            {
                if (visitor.Adult)
                {
                    AdultCount++;
                    ContainsAdult = true;
                }
                else
                {
                    ChildrenCount++;
                }
                UnsortedGroupMembers++;
            }
            return ContainsAdult;
        }
        #endregion

        #region Order by
        public void OrderGroupByAge()
        {
            var orderedGroupOnAge = Visitors.OrderByDescending(x => x.DateOfBirth);
            Visitors = orderedGroupOnAge.ToList();
        }
        #endregion
    }
}