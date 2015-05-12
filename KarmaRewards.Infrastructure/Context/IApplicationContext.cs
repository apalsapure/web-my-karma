using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KarmaRewards.Infrastructure.Context
{
    public interface IApplicationContext : IDisposable
    {
        T Get<T>(string name, T defaultValue = default(T));

        string CorrelationId { get; }
    }
}
