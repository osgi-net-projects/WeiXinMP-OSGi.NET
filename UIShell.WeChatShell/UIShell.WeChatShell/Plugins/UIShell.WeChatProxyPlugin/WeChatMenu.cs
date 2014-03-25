using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UIShell.OSGi;

namespace UIShell.WeChatProxyPlugin
{
    public class WeChatMenu
    {
        public string Name { set; get; }
        public string AppId { set; get; }
        public string Secret { set; get; }
        public List<MenuButton> MenuButtons { set; get; }
        public IBundle Bundle { set; get; }
    }
}