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
            this.CurrentAddress = new Address();
        }

        public Address CurrentAddress { get; set; }
    }
}
