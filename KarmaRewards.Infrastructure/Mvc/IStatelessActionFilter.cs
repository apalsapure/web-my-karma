using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace KarmaRewards.Infrastructure.Mvc
{
    public interface IStatelessActionFilter
    {
        string Name { get; }

        object OnBeforeAction(ControllerContext controllerContext, ActionDescriptor actionDescriptor, IDictionary<string, object> parameters, ref ActionResult result);

        void OnAfterAction(ControllerContext controllerContext, ActionDescriptor actionDescriptor, IDictionary<string, object> parameters, ref ActionResult result, object correlationState);
    }
}
