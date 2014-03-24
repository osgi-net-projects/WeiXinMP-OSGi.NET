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

        public List<WeChatProxy> WeChatProxies = new List<WeChatProxy>();

        public WeChatProxyContainer()
        {
            var extensions = Activator.ExtensionService.GetExtensionProviders(WECHAT_PROXY);
            foreach (var extension in extensions)
            {
                string name = extension.AttributesCollection[NAME_ATTRIBUTE];
                string token = extension.AttributesCollection[TOKEN_ATTRIBUTE];
                string hanlder = extension.AttributesCollection[HANDLER_ATTRIBUTE];
                string symbolicName = extension.Bundle.SymbolicName;

                if (Validate(hanlder, HANDLER_ATTRIBUTE, symbolicName) && Validate(token, TOKEN_ATTRIBUTE, symbolicName))
                {
                    WeChatProxies.Add(new WeChatProxy() { Name = name, Token = token, Handler = hanlder, Bundle = extension.Bundle });
                }
            }
        }

        private bool Validate(string value, string name, string symbolicName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                FileLogUtility.Error(string.Format("{0} is not specified in bundle {1} for extension point {2} so it's ignored.", name, symbolicName, WECHAT_PROXY));

                return false;
            }

            return true;
        }

        private bool Validate(ref string value, string name, string symbolicName, string defaultValue)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                value = defaultValue;
                FileLogUtility.Error(string.Format("{0} is not specified in bundle {1} for extension point {2} so it's set to default {3}.", name, symbolicName, WECHAT_PROXY, defaultValue));
            }

            return true;
        }
    }
}