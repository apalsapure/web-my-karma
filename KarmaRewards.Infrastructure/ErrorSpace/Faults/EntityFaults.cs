using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Infrastructure.ErrorSpace.Faults
{
    public class EntityFaults
    {
        public static Exception DoesNotMatch(string type)
        {
            return new BaseApplicationException(Errors.EntityFault + type + ".")
            {
                ErrorCode = HttpErrorCode.Conflict
            };
        }
    }
}
