using KarmaRewards.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Appacitive
{
    public class UserRepository : IUserRepository
    {
        public Task<User> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<User> CreateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<User>> FindUsersAsync(string query, string freeText, string orderBy, bool isAscending, int pagenumber = 1, int pagesize = 10)
        {
            throw new NotImplementedException();
        }
    }
}
