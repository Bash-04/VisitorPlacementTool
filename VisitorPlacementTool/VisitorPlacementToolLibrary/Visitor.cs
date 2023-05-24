using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisitorPlacementToolLibrary
{
    public class Visitor
    {
        // Properties
        public string Id { get; private set; }
        public string Name { get; private set; }
        public DateOnly DateOfBirth { get; private set; }
        public int Age { get; private set; }
        public bool Adult { get; private set; }
        public DateTime SignupDate { get; private set; }
        public Seat Seat { get; private set; }

        // Constructors
        public Visitor()
        {
            Id = Guid.NewGuid().ToString();
            GetRandomSignupDate();
            GetDateOfBirth();
            GetName();
        }

        // Methods
        #region GetVisitorInfo
        private void GetRandomSignupDate()
        {
            Random random = new Random();
            // 1 dag tot 8 maanden geleden aangemeld
            int days = random.Next(1, 244);
            SignupDate = DateTime.Now.AddDays(-days);
        }

        public void GetDateOfBirth()
        {
            Random random = new Random();
            int ageInDays = random.Next(365, 24000);

            DateOfBirth = DateOnly.FromDateTime(DateTime.Now.AddDays(-ageInDays));
            Age = DateTime.Now.Year - DateOfBirth.Year;
            if (Age >= 12)
            {
                Adult = true;
            }
            else
            {
                Adult = false;
            }
        }

        public void GetName()
        {
            Random r = new Random();
            int nameLength = r.Next(2, 5);
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "z" };
            string[] vowels = { "a", "e", "i", "o", "u", "y" };
            string name = "";
            name += consonants[r.Next(consonants.Length)].ToUpper();
            name += vowels[r.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b <= nameLength)
            {
                name += consonants[r.Next(consonants.Length)];
                b++;
                name += vowels[r.Next(vowels.Length)];
                b++;
            }

            Name = name;
        }
        #endregion
    }
}