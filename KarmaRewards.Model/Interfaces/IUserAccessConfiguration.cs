using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Model
{
    public interface IUserAccessConfiguration
    {
        IEnumerable<Feature> Features { get; }

        IEnumerable<Role> Roles { get; }
    }
}
