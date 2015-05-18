using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Model
{
    public class Feature
    {
        public Feature(int index, string name)
        {
            this.Index = index;
            this.Name = name;
        }

        public static bool TryResolve(string featureName, out Feature feature)
        {
            feature = AccessControl.Configuration.Features.SingleOrDefault(f => f.Name.Equals(featureName, StringComparison.OrdinalIgnoreCase) == true);
            if (feature == null)
                return false;
            else
                return true;
        }

        public string Name { get; private set; }

        public int Index { get; private set; }
    }
}
