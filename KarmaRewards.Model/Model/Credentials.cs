using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Model
{
    public class Credentials
    {
        public static Credentials WithUsernamePasswordCredentials(string username, string password)
        {
            return new Credentials()
            {
                Type = "usernamepassword",
                Tokens = new Dictionary<string, string> { { "username", username }, { "password", password } }
            };
        }

        public string Type { get; set; }

        public Dictionary<string, string> Tokens { get; set; }

        public int Expiry { get; set; }

        public int Attempts { get; set; }

        public bool CreateNew { get; set; }
    }
}
