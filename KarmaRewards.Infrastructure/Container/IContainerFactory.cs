using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Infrastructure.Container
{
    public interface IContainerFactory
    {
        IDependencyContainer CreateContainer(ContainerSettings settings);
    }
}
