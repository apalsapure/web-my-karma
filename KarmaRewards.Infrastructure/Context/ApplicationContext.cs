using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Infrastructure.Context
{
    [Serializable]
    public class ApplicationContext : BaseApplicationContext
    {
        public ApplicationContext()
            : base()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public static class Keys
        {
            public static readonly string TransactionId = "txnId";
        }

        public string Id { get; private set; }

        public string TransactionId
        {
            get
            {
                return this.Get<string>(Keys.TransactionId, Guid.NewGuid().ToString());
            }
        }


        public static ApplicationContext Current
        {
            get { return ApplicationContextScope.Current as ApplicationContext; }
        }
    }
}
