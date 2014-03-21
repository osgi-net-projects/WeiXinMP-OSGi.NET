using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UIShell.WeChatProxyPlugin
{
    public class WeChatProxyContainer
    {
        private const string WECHAT_PROXY = "UIShell.WeChatProxy";
        private const string NAME_ATTRIBUTE = "Name";
        private const string TOKEN_ATTRIBUTE = "Token";
        private const string HANDLER_ATTRIBUTE = "Handler";
        public List<WeChatProxy> WeChatProxies = new List<WeChatProxy>();

        public WeChatProxyContainer()
        {
            var extensions = Activator.ExtensionService.GetExtensionProviders(WECHAT_PROXY);
            foreach (var extension in extensions)
            {
                WeChatProxies.Add(new WeChatProxy() { Name = extension.AttributesCollection[NAME_ATTRIBUTE], Token = extension.AttributesCollection[TOKEN_ATTRIBUTE], Handler = extension.AttributesCollection[HANDLER_ATTRIBUTE], Bundle = extension.Bundle });
            }
        }
    }
}