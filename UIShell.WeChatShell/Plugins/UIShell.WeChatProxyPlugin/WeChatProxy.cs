using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UIShell.OSGi;

namespace UIShell.WeChatProxyPlugin
{
    public class WeChatProxy
    {
        public string Name { set; get; }
        public string Token { set; get; }
        public string Handler { set; get; }
        public IBundle Bundle { set; get; }
    }
}