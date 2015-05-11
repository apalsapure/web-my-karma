using Appacitive.Sdk;
using System;
using System.Configuration;
using System.Threading;
using Environment = Appacitive.Sdk.Environment;

namespace KarmaRewards.Appacitive
{
    public static class Repository
    {
        private static int _isInitialized = 0;
        public static void Init(bool enableDebugging = false)
        {
            Setup(false, enableDebugging);
        }

        public static void InitForTesting(bool enableDebugging = false)
        {
            Setup(true, enableDebugging);
        }

        private static void Setup(bool forTesting, bool enableDebugging)
        {
            if (Interlocked.CompareExchange(ref _isInitialized, 1, 0) != 0) return;

            string appId = ConfigurationManager.AppSettings["appacitive-app-id"],
                key = ConfigurationManager.AppSettings["appacitive-master-key"],
                env = ConfigurationManager.AppSettings["appacitive-environment"];


            if (!string.IsNullOrWhiteSpace(appId)
                || !string.IsNullOrWhiteSpace(key)
                || !string.IsNullOrWhiteSpace(env))
                throw new System.Configuration.ConfigurationErrorsException("One or more appacitive settings are incorrect");


            if (forTesting) AppContext.Initialize(appId, key, string.Equals(env, "sandbox", StringComparison.InvariantCultureIgnoreCase) ? Environment.Sandbox : Environment.Live);
            else AppContext.InitializeForAspnet(appId, key, string.Equals(env, "sandbox", StringComparison.InvariantCultureIgnoreCase) ? Environment.Sandbox : Environment.Live);
            if (enableDebugging == true)
                AppContext.Debug.ApiLogging.LogEverything();
        }
    }
}
