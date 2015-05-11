using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Model
{
    public class Profile : Entity
    {
        public Profile()
        {
            this.Roles = new List<string>();
            this.DateOfBirth = null;
        }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public List<string> Roles { get; private set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Nullable<DateTime> DateOfBirth { get; set; }
    }
}
