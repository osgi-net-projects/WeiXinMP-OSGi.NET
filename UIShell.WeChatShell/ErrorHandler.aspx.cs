using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using UIShell.OSGi.WebExtension;

namespace UIShell.WeChatShell
{
    public partial class ErrorHandler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            errorLabel.Text = BundlePageHandlerFactory.CurrentErrorMessage;
        }

        protected void btnRestart_Click(object sender, EventArgs e)
        {
            IBundleRuntimeHttpHost host = Context.ApplicationInstance as IBundleRuntimeHttpHost;
            host.RestartAppDomain();
        }
    }
}
