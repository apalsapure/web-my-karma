using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Model
{
    public class AccessControl
    {
        public static void Initialize(IUserAccessRepository accessControlRepository, IUserAccessConfiguration configuration, IIdentityProvider idProvider)
        {
            AccessControl.AccessControlRepository = accessControlRepository;
            AccessControl.IdentityProvider = idProvider;
            AccessControl.Configuration = configuration;
        }

        public static IUserAccessRepository AccessControlRepository { get; private set; }

        public static IIdentityProvider IdentityProvider { get; private set; }

        public static IUserAccessConfiguration Configuration { get; private set; }
    }
}
