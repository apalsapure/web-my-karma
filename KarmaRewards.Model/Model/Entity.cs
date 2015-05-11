using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KarmaRewards.Model
{
    [Serializable]
    public abstract class Entity
    {
        protected Entity()
        {
            this.UtcCreatedAt = this.UtcLastUpdatedAt = DateTime.UtcNow;
        }

        public string Id { get; set; }

        public DateTime UtcCreatedAt { get; set; }

        public DateTime UtcLastUpdatedAt { get; set; }
    }
}
