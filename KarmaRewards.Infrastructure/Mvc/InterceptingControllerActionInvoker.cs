using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Async;

namespace KarmaRewards.Infrastructure.Mvc
{
    public class InterceptingControllerActionInvoker : AsyncControllerActionInvoker
    {
        private IActionFilterFactory _factory;

        public InterceptingControllerActionInvoker(IActionFilterFactory factory)
        {
            _factory = factory;
        }

        protected override ActionResult InvokeActionMethod(ControllerContext controllerContext, ActionDescriptor actionDescriptor, IDictionary<string, object> parameters)
        {
            try
            {
                return Execute(controllerContext, actionDescriptor, parameters);
            }
            catch
            {
                return new HttpStatusCodeResult(503, "Interception chain invocation failure. Check Tavisca.Common.Infrastructure.Mvc.InterceptingControllerActionInvoker.");
            }
        }

        private ActionResult Execute(ControllerContext controllerContext, ActionDescriptor actionDescriptor, IDictionary<string, object> parameters)
        {
            var callStack = new Stack<IStatelessActionFilter>();
            ActionResult result = null;
            bool shortCircuit = false;
            var filters = _factory.CreateFilters();
            foreach (var filter in filters)
            {
                var state = filter.OnBeforeAction(controllerContext, actionDescriptor, parameters, ref result);
                if (state != null)
                    controllerContext.RequestContext.HttpContext.Items[filter.Name] = state;
                callStack.Push(filter);
                if (result != null)
                {
                    shortCircuit = true;
                    break;
                }
            }
            if (shortCircuit == false)
                result = base.InvokeActionMethod(controllerContext, actionDescriptor, parameters);
            while (callStack.Count > 0)
            {
                object correlationState = null;
                var filter = callStack.Pop();
                var items = controllerContext.RequestContext.HttpContext.Items;
                if (items.Contains(filter.Name) == true)
                    correlationState = items[filter.Name];
                else
                    correlationState = null;
                filter.OnAfterAction(controllerContext, actionDescriptor, parameters, ref result, correlationState);
            }
            return result;
        }
    }
}
