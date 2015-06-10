using SDK = Appacitive.Sdk;
using Appacitive.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using KarmaRewards.Model;
using KarmaRewards.Infrastructure;

namespace KarmaRewards.Appacitive
{
    public class KarmaPointRepository : IKarmaPointRepository
    {
        public async Task<KarmaPoints> Save(KarmaPoints points)
        {
            var obj = points.ToAPPoint();
            if (string.IsNullOrEmpty(points.Id))
            {
                await SDK.APConnection
                        .New("owner")
                        .FromExistingObject("user", points.To)
                        .ToNewObject("points", obj)
                        .SaveAsync();
            }
            else await obj.SaveAsync();

            return obj.ToPoints();
        }

        public async Task<KarmaPoints> Get(string id)
        {
            try
            {
                var obj = await SDK.APObjects.GetAsync("points", id);
                return obj.ToPoints();
            }
            catch (ObjectNotFoundException)
            {
                return null;
            }
        }
    }
}
