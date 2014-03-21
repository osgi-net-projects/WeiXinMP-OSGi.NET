using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using UIShell.OSGi.WebExtension;
using UIShell.OSGi;
using UIShell.iOpenWorks.Bootstrapper;
using System.IO;

namespace UIShell.WeChatShell
{
    public class Global : BundleHttpApplication
    {
        protected override BundleRuntime CreateBundleRuntime()
        {
            BundleRuntime br = base.CreateBundleRuntime();
            br.EnableBundleClassLoaderCache = true;
            br.EnableGlobalAssemblyFeature = true;
            return br;
        }

        protected override void Application_Start(object sender, EventArgs e)
        {
            if (AutoUpdateCoreFiles)
            {
                new CoreFileUpdater().UpdateCoreFiles(CoreFileUpdateCheckType.Daily);
            }
            // Create a repository folder to store the downloaded plugins.
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "repository");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            base.Application_Start(sender, e);
        }

        /// <summary>
        /// 是否启用自动更新。
        /// </summary>
        private static bool AutoUpdateCoreFiles
        {
            get
            {
                string autoUpdateCoreFiles = System.Web.Configuration.WebConfigurationManager.AppSettings["AutoUpdateCoreFiles"];
                if (!string.IsNullOrEmpty(autoUpdateCoreFiles))
                {
                    try
                    {
                        return bool.Parse(autoUpdateCoreFiles);
                    }
                    catch { }
                }

                return false;
            }
        }
    }
}