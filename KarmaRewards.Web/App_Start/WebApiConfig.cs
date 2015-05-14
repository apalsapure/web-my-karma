using KarmaRewards.Infrastructure;
using KarmaRewards.Infrastructure.WebApi;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Filters;

namespace KarmaRewards.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //config.EnableSystemDiagnosticsTracing();
            // Web API routes
            config.MapHttpAttributeRoutes();
            // Map handlers
            // config.MessageHandlers.Add(new ResponseWrappingHandler());
            //config.MessageHandlers.Add(ObjectFactory.Resolve<InterceptingDelegatingHandler>());
            // Register filters
            RegisterWebApiFilters(config.Filters);
            // Setup dependency container
            config.DependencyResolver = new DependencyResolver();
            // Configure the formatter
            //ConfigureFormatter(config);
        }

        private static void RegisterWebApiFilters(HttpFilterCollection filters)
        {
            // Setup context
            // filters.Add(new ContextSetupFilter());
            // Validation filter
            //filters.Add(new ValidateModelAttribute());
            // Configure filters
            //filters.Add(new ExceptionShieldingFilterAttribute());
        }

        //private static void ConfigureFormatter(HttpConfiguration config)
        //{
        //    // Enable json formatter for content type text/html
        //    config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        //    config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
        //    config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        //    // Add converters
        //    // Get all the factories and add all the converters.
        //    var factories = ObjectFactory.ResolveAll<IConverterFactory>();
        //    foreach (var factory in factories)
        //    {
        //        var converters = factory.GetConverters();
        //        foreach (var converter in converters)
        //            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(converter);
        //    }
        //}
    }
}