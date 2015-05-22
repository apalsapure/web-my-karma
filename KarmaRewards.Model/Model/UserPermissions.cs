using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Model
{
    public class UserPermissions : Entity
    {
        public UserPermissions()
        {
            Roles = new List<string>();
            Permissions = new AccessPermissions();
        }

        public List<string> Roles { get; set; }

        public AccessPermissions Permissions { get; set; }
    }
}
