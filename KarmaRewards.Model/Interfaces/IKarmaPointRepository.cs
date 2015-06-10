using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Model
{
    public interface IKarmaPointRepository
    {
        Task<KarmaPoints> Save(KarmaPoints points);

        Task<KarmaPoints> Get(string id);
    }
}
