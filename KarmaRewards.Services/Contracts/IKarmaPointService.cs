using KarmaRewards.Model;
using System.Threading.Tasks;

namespace KarmaRewards.Services
{
    public interface IKarmaPointService
    {
        Task<KarmaPoints> Save(KarmaPoints points);

        Task<KarmaPoints> Get(string id);
    }
}
