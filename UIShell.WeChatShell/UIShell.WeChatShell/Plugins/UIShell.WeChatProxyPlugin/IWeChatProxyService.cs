using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIShell.OSGi;

namespace UIShell.WeChatProxyPlugin
{
    public interface IWeChatProxyService
    {
        void AddWeChatProxy(string name, string token, string hanlder, IBundle bundle, string appid, string secret);
        void RemoveWeChatProxy(string name, IBundle bundle);
    }
}
