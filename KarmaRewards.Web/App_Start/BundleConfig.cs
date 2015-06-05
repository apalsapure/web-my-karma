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

                    "~/Resources/plugins/datatables/dataTables.bootstrap.css",
                    "~/Resources/plugins/iCheck/minimal/_all.css",
                    "~/Resources/plugins/select2/select2.css",
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


            var secondaryBundle = new ScriptBundle("~/bundles/scripts/secondary")
                 .Include(
                    "~/Scripts/init.js",
                    "~/Scripts/configuration.js",
                    "~/Scripts/utils/utils.js",
                    "~/Resources/plugins/jquery-validation/js/jquery.validate.js",
                    "~/Resources/plugins/jquery-validation/js/additional-methods.js",
                    "~/Resources/plugins/appacitive-js-sdk.js",
                    "~/Resources/plugins/utils/appacitiveInit.js",
                    "~/Resources/plugins/underscore-min.js",
                    "~/Resources/plugins/backbone.js",
                    "~/Resources/plugins/handlebars-1.3.js",
                    "~/Resources/Plugins/select2/select2.js",
                    "~/Resources/Plugins/iCheck/icheck.min.js",
                    "~/Scripts/Utils/modalWin.js",
                    "~/Scripts/Models/Common/errorModel.js",
                    "~/Scripts/Views/Common/Error/errorView.js"
                    );

            secondaryBundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(secondaryBundle);

            bundles.Add(new ScriptBundle("~/bundles/scripts/grid-lib")
                .Include(
                    "~/Resources/plugins/datatables/jquery.dataTables.js",
                    "~/Resources/plugins/datatables/dataTables.bootstrap.js",
                    "~/Scripts/Views/Common/Datatable/datatable.js",
                    "~/Scripts/Views/Common/Datatable/gridView.js"
                ));


            bundles.Add(new ScriptBundle("~/bundles/scripts/user-add")
                .Include(
                    "~/Scripts/Models/userModel.js",
                    "~/Scripts/Views/User/addUserView.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/scripts/input-mask")
                .Include(
                    "~/Resources/plugins/input-mask/jquery.inputmask.js",
                    "~/Resources/plugins/input-mask/jquery.inputmask.date.extensions.js",
                    "~/Resources/plugins/input-mask/jquery.inputmask.extensions.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/scripts/login")
                .Include(
                    "~/Resources/plugins/backstretch/jquery.backstretch.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/scripts/user-manage")
                .Include(
                "~/Scripts/Models/userModel.js",
                "~/Scripts/Views/User/userListView.js"
                ));
        }
    }
}