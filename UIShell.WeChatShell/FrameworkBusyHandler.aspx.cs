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

using UIShell.OSGi;
using UIShell.OSGi.WebExtension;

namespace UIShell.WeChatShell
{
    public partial class FrameworkBusyHandler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BundleRuntime br = BundleRuntime.Instance;
            BundleRuntimeState state = br.State;

            switch (state)
            {
                case BundleRuntimeState.Started:
                case BundleRuntimeState.Starting:
                    hintLabel.Text = "系统正在启动，请稍等片刻，然后点击“返回首页”按钮再次访问。";
                    break;
                case BundleRuntimeState.Stopped:
                case BundleRuntimeState.Stopping:
                case BundleRuntimeState.Disposed:
                    hintLabel.Text = "系统正在终止运行或已经停止运行，请稍等片刻，然后点击“返回首页”按钮再次访问。";
                    break;
            }
        }

        protected void reloadButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(BundlePageHandlerFactory.DefaultPage);
        }
    }
}
