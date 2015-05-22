using System;

namespace KarmaRewards.Model
{
    public class User : Entity
    {
        public User()
        {
            this.Profile = new Profile();
            this.Permissions = new UserPermissions();
            this.DateOfBirth = null;
            this.DateOfBirthStr = "";
        }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Nullable<DateTime> DateOfBirth { get; set; }

        public string DateOfBirthStr { get; set; }

        public string ImageUrl { get; set; }

        public string Gender { get; set; }

        public string Provider { get; set; }

        public string Designation { get; set; }
        
        public DateTime JoiningDate { get; set; }

        public bool IsEnabled { get; set; }

        public string JoiningDateStr { get; set; }

        public Profile Profile { get; set; }

        public UserPermissions Permissions { get; set; }
    }
}
