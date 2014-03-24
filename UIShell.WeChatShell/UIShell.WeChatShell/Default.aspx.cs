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
using UIShell.OSGi;
using System.Reflection;
using UIShell.PageFlowService;

namespace UIShell.WeChatShell
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get PageFlowService.
            IPageFlowService pageFlowService = BundleRuntime.Instance.GetFirstOrDefaultService<IPageFlowService>();
            if (pageFlowService == null)
            {
                throw new ServiceNotAvailableException(typeof(IPageFlowService).FullName, Properties.Resources.IOpenWorksWebShellName);
            }

            if (string.IsNullOrEmpty(pageFlowService.FirstPageNodeValue))
            {
                throw new Exception(Properties.Resources.CanNotFindAnAvailablePageNode);
            }
            // Redirect to first node.
            Server.Transfer(pageFlowService.FirstPageNodeValue);
        }
    }
}
