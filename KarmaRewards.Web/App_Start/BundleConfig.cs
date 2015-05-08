using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace KarmaRewards.Web
{
    public class BundleConfig
    {
        public class NonOrderingBundleOrderer : IBundleOrderer
        {
            public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
            {
                return files;
            }
        }

        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/resources/css/primary")
                .Include(
                    "~/Resources/plugins/font-awesome/css/font-awesome.min.css",
                    "~/Resources/plugins/jquery-ui/jquery-ui-1.10.3.custom.min.css",
                    "~/Resources/plugins/bootstrap/css/bootstrap.min.css",
                    "~/Resources/plugins/uniform/css/uniform.default.css",
                    "~/Resources/plugins/bootstrap-switch/css/bootstrap-switch.min.css",
                    "~/Resources/plugins/simple-line-icons/simple-line-icons.css",

                    //"~/Resources/css/components.css",
                    "~/Resources/css/plugins.css",
                    "~/Resources/css/structure.css",
                    "~/Resources/css/base.css",
                    "~/Resources/css/custom.css"
                    ));

            var primaryBundle = new ScriptBundle("~/bundles/scripts/primary")
                 .Include(
                    "~/Resources/plugins/jquery.min.js",
                    "~/Resources/plugins/jquery-migrate.min.js",
                    "~/Resources/plugins/jquery-ui/jquery-ui-1.10.3.custom.min.js",
                    "~/Resources/plugins/bootstrap/js/bootstrap.js",
                    "~/Resources/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js",
                    "~/Resources/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                    "~/Resources/plugins/jquery.blockui.min.js",
                    "~/Resources/plugins/uniform/jquery.uniform.min.js",
                    "~/Resources/plugins/bootstrap-switch/js/bootstrap-switch.min.js",
                    "~/Resources/plugins/jquery-validation/js/jquery.validate.js",
                    "~/Resources/plugins/jquery-validation/js/additional-methods.js",
                    "~/Resources/plugins/app.js"
                    );

            primaryBundle.Orderer = new NonOrderingBundleOrderer();

            bundles.Add(primaryBundle);

            bundles.Add(new ScriptBundle("~/bundles/scripts/login")
                .Include(
                    "~/Resources/plugins/backstretch/jquery.backstretch.min.js"
                ));
        }
    }
}