using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Model
{
    public class Identity : Entity
    {
        public Identity(string userName, string email, AuthenticationProvider authenticationProvider, string firstName, string lastName = null)
        {
            this.Username = userName;
            this.Email = email;
            this.Provider = authenticationProvider.ToString();
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Provider { get; set; }


        /// <summary>
        /// Gives String representation of Current Instance of Identity object
        /// </summary>
        /// <returns>String representation of Current Instance of Identity object</returns>
        public override string ToString()
        {
            return this.Username + "|" + this.Email + "|" + this.Provider + "|" + this.FirstName + "|" + this.LastName;
        }

    }
}
