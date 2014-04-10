using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UIShell.OSGi.Utility;
using UIShell.OSGi;

namespace UIShell.WeChatProxyPlugin
{
    public class WeChatProxyContainer
    {
        private const string WECHAT_PROXY = "UIShell.WeChatProxy";
        private const string NAME_ATTRIBUTE = "Name";
        private const string TOKEN_ATTRIBUTE = "Token";
        private const string HANDLER_ATTRIBUTE = "Handler";
        private const string APPID_ATTRIBUTE = "AppId";
        private const string SECRET_ATTRIBUTE = "Secret";
        private List<WeChatProxy> _weChatProxies = new List<WeChatProxy>();
        internal List<WeChatProxy> ServiceWeChatProxies = new List<WeChatProxy>();

        internal List<WeChatProxy> WeChatProxies
        {
            get
            {
                BuildWeChatProxiesFromExtension();
                _weChatProxies.AddRange(ServiceWeChatProxies.Where(i => Activator.Context.GetBundles().Contains(i.Bundle) && i.Bundle.State == BundleState.Active));

                return _weChatProxies;
            }
        }

        private void BuildWeChatProxiesFromExtension()
        {
            _weChatProxies = new List<WeChatProxy>();

            var extensions = Activator.ExtensionService.GetExtensionProviders(WECHAT_PROXY);
            foreach (var extension in extensions)
            {
                string name = extension.AttributesCollection[NAME_ATTRIBUTE];
                string tokenArray = extension.AttributesCollection[TOKEN_ATTRIBUTE];
                string hanlder = extension.AttributesCollection[HANDLER_ATTRIBUTE];
                string appid = extension.AttributesCollection[APPID_ATTRIBUTE];
                string secret = extension.AttributesCollection[SECRET_ATTRIBUTE];
                string symbolicName = extension.Bundle.SymbolicName;

                var tokens = tokenArray.Split(',');

                foreach (var token in tokens)
                {
                    if (Utility.Validate(hanlder, HANDLER_ATTRIBUTE, symbolicName, WECHAT_PROXY) && Utility.Validate(token, TOKEN_ATTRIBUTE, symbolicName, WECHAT_PROXY))
                    {
                        _weChatProxies.Add(new WeChatProxy() { Name = name, Token = token, Handler = hanlder, Bundle = extension.Bundle, AppID = appid, Secret = secret });
                    }
                }
            }
        }
    }
}