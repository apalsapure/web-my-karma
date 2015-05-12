using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Infrastructure.Context
{
    [Serializable]
    public class ApplicationContextScope : BaseApplicationContextScope<IApplicationContext>
    {
        public ApplicationContextScope(IApplicationContext context)
            : base(context)
        {
        }
    }
}
