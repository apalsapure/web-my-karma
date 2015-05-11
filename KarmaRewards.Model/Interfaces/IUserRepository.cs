using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Model
{
    public interface IUserRepository
    {
        Task<User> GetByNameAsync(string name);

        Task<User> CreateAsync(User user);

        Task<Model.PagedList<User>> FindUsersAsync(string query, string freeText, string orderBy, bool isAscending, int pagenumber = 1, int pagesize = 10);
    }
}
