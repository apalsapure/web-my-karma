using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Model
{
    public class Strings
    {
        public static bool IsNumber(string value)
        {
            long number;
            return long.TryParse(value, out number);
        }
    }
}
