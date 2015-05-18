using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Model
{
    public class User : Entity
    {
        public User()
        {
            this.Profile = new Profile();
            this.DateOfBirth = null;
        }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Nullable<DateTime> DateOfBirth { get; set; }

        public string ImageUrl { get; set; }

        public string Gender { get; set; }

        public string Provider { get; set; }

        public string Designation { get; set; }

        public DateTime JoiningDate { get; set; }

        public Profile Profile { get; set; }
    }
}
