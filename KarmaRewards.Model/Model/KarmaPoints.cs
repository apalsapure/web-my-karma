using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Model
{
    public class KarmaPoints : Entity
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Reason { get; set; }
        public string ModeratedBy { get; set; }
        public string ModerateReason { get; set; }
        public int Points { get; set; }
    }
}
