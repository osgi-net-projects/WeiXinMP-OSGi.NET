using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UIShell.OSGi.Utility;

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

        public List<WeChatProxy> WeChatProxies
        {
            get
            {
                BuildWeChatProxies();

                return _weChatProxies;
            }
        }

        private void BuildWeChatProxies()
        {
            _weChatProxies = new List<WeChatProxy>();

            var extensions = Activator.ExtensionService.GetExtensionProviders(WECHAT_PROXY);
            foreach (var extension in extensions)
            {
                string name = extension.AttributesCollection[NAME_ATTRIBUTE];
                string token = extension.AttributesCollection[TOKEN_ATTRIBUTE];
                string hanlder = extension.AttributesCollection[HANDLER_ATTRIBUTE];
                string appid = extension.AttributesCollection[APPID_ATTRIBUTE];
                string secret = extension.AttributesCollection[SECRET_ATTRIBUTE];
                string symbolicName = extension.Bundle.SymbolicName;

                if (Utility.Validate(hanlder, HANDLER_ATTRIBUTE, symbolicName, WECHAT_PROXY) && Utility.Validate(token, TOKEN_ATTRIBUTE, symbolicName, WECHAT_PROXY))
                {
                    _weChatProxies.Add(new WeChatProxy() { Name = name, Token = token, Handler = hanlder, Bundle = extension.Bundle, AppId = appid, Secret = secret });
                }
            }
        }
    }
}