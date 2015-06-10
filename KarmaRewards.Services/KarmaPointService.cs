using KarmaRewards.Infrastructure;
using KarmaRewards.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Services
{
    public class KarmaPointService : IKarmaPointService
    {
        [InjectionConstructor]
        public KarmaPointService(IKarmaPointRepository karmaPointRepository)
        {
            this.Repository = karmaPointRepository;
        }

        public IKarmaPointRepository Repository { get; set; }

        public async Task<Model.KarmaPoints> Save(Model.KarmaPoints points)
        {
            return await this.Repository.Save(points);
        }

        public async Task<Model.KarmaPoints> Get(string id)
        {
            return await this.Repository.Get(id);
        }
    }
}
