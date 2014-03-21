using System;
using System.Collections.Generic;
using System.Text;
using UIShell.OSGi;
using UIShell.ExtensionProviderService;

namespace UIShell.WeChatProxyPlugin
{
    public class Activator : IBundleActivator
    {
        public static IExtensionService ExtensionService { get; private set; }
        public static WeChatProxyContainer WeChatProxyContainer { get; private set; }

        public void Start(IBundleContext context)
        {
            ExtensionService = context.GetFirstOrDefaultService<IExtensionService>();
            WeChatProxyContainer = new WeChatProxyContainer();
        }

        public void Stop(IBundleContext context)
        {
            //todo:
        }
    }
}
