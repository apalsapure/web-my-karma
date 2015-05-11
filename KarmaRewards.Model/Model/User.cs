using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Model
{
    public class User : Entity
    {
        public Profile Profile { get; set; }

        public List<Identity> Identity { get; set; }
    }
}
