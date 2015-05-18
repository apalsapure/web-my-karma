using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Model
{
    public class AccessPermissions
    {
        public AccessPermissions()
        {
            Allowed = new List<string>();
            Denied = new List<string>();
        }


        public List<string> Allowed { get; set; }

        public List<string> Denied { get; set; }

        internal bool HasAccess(Feature feature)
        {
            return (this.Allowed.Contains(feature.Index.ToString()) && (!this.Denied.Contains(feature.Index.ToString())));
        }

        internal bool HasDeniedAccess(Feature feature)
        {
            return this.Denied.Contains(feature.Index.ToString());
        }
    }
}
