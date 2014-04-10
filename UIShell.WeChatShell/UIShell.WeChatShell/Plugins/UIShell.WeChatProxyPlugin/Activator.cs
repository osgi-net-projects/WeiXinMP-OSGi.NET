using System;
using System.Collections.Generic;
using System.Text;
using UIShell.OSGi;
using UIShell.ExtensionProviderService;
using UIShell.WeChatProxyPlugin.Impl;

namespace UIShell.WeChatProxyPlugin
{
    public class Activator : IBundleActivator
    {
        public static IBundleContext Context { get; private set; }
        public static IExtensionService ExtensionService { get; private set; }
        public static WeChatProxyContainer WeChatProxyContainer { get; private set; }
        public static WeChatMenuContainer WeChatMenuContainer { get; private set; }
        public static IWeChatProxyService WeChatProxyService { get; private set; }

        public void Start(IBundleContext context)
        {
            Context = context;
            ExtensionService = context.GetFirstOrDefaultService<IExtensionService>();
            WeChatProxyContainer = new WeChatProxyContainer();
            WeChatMenuContainer = new WeChatMenuContainer();
            WeChatProxyService = new WeChatProxyService();
            context.AddService<IWeChatProxyService>(WeChatProxyService);
        }

        public void Stop(IBundleContext context)
        {
            //todo:
        }
    }
}
