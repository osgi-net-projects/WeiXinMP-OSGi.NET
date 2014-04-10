using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UIShell.OSGi;
using UIShell.OSGi.Utility;

namespace UIShell.WeChatProxyPlugin.Impl
{
    public class WeChatProxyService : IWeChatProxyService
    {
        public void AddWeChatProxy(string name, string token, string hanlder, IBundle bundle, string appid, string secret)
        {
            if (!string.IsNullOrEmpty(hanlder) && !string.IsNullOrEmpty(token))
            {
                var item = Activator.WeChatProxyContainer.WeChatProxies.Where(i => i.Token == token).FirstOrDefault();

                if (item == null)
                    Activator.WeChatProxyContainer.ServiceWeChatProxies.Add(new WeChatProxy() { Name = name, Token = token, Handler = hanlder, Bundle = bundle, AppID = appid, Secret = secret });
                else
                    FileLogUtility.Error(string.Format("{0} provides the same Token {1} that has been registered when calling AddWeChatProxy in IWeChatProxyService so it's ignored.", bundle.SymbolicName, token));
            }
            else
                FileLogUtility.Error(string.Format("Hander and Token should not be empty when calling AddWeChatProxy in IWeChatProxyService from Bundle {0}.", bundle.SymbolicName));
        }

        public void RemoveWeChatProxy(string name, IBundle bundle)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var serviceProxies = Activator.WeChatProxyContainer.ServiceWeChatProxies;
                var item = serviceProxies.Where(i => i.Name == name && i.Bundle == bundle).FirstOrDefault();
                if (serviceProxies.Contains(item))
                    serviceProxies.Remove(item);
            }
            else
                FileLogUtility.Error(string.Format("Name hould not be empty when calling RemoveWeChatProxy in IWeChatProxyService from Bundle {0}.", bundle.SymbolicName));
        }
    }
}