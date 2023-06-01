using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorPlacementToolLibrary
{
    public class Group
    {
        // Properties
        public string Id { get; private set; }
        public List<Visitor> Visitors { get; private set; }
        public DateTime EarliestSignupDate { get; private set; }
        public bool ContainsAdult { get; private set; }
        public bool ContainsChildren { get; private set; }
        public int ChildrenCount { get; private set; }
        public int AdultCount { get; private set; }
        public int UnseatedGroupMembers { get; set; }
        public bool IsPlaced { get; private set; }
        public bool ChildrenArePlaced { get; private set; }

        // Constructors
        public Group()
        {
            Id = Guid.NewGuid().ToString();
            Visitors = new List<Visitor>();
            EarliestSignupDate = DateTime.Now;
        }

        // Methods
        #region Check
        public void DefaultCheckAndCount()
        {
            CountAdultsAndChildren();
            CheckEarliestSignupDate();
            CheckSeated();
        }

        private void CheckSeated()
        {
            CheckIfAllVisitorsAreSeated();
            CheckIfChildrenAreSeated();
            CountUnseatedGroupMembers();
        }

        private void CheckIfAllVisitorsAreSeated()
        {
            if (Visitors.Count(x => x.Seated) == Visitors.Count())
            {
                IsPlaced = true;
            }
            else
            {
                IsPlaced = false;
            }
        }

        private void CheckIfChildrenAreSeated()
        {
            if (Visitors.Count(x => x.Seated && x.Adult == false) == ChildrenCount)
            {
                ChildrenArePlaced = true;
            }
            else
            {
                ChildrenArePlaced = false;
            }
        }

        private bool CheckEarliestSignupDate()
        {
            bool hasEarliestSignupDate = false;
            if (Visitors.Count() == 1)
            {
                EarliestSignupDate = Visitors[0].SignupDate;
                hasEarliestSignupDate = true;
            }
            else
            {
                for (int i = 0; i < Visitors.Count(); i++)
                {
                    if (Visitors[i].SignupDate.Date < EarliestSignupDate.Date)
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
            AdultCount = 0;
            ChildrenCount = 0;
            UnseatedGroupMembers = 0;
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
                    ContainsChildren = true;
                }
                UnseatedGroupMembers++;
            }
            return ContainsAdult;
        }

        private void CountUnseatedGroupMembers()
        {
            UnseatedGroupMembers = Visitors.Count(x => x.Seated == false);
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
