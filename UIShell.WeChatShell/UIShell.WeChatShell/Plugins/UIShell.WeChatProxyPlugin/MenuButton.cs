using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UIShell.WeChatProxyPlugin
{
    public class MenuButton
    {
        public MenuType Type { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string Url { get; set; }
        public List<MenuButton> MenuSubButtons { set; get; }
    }
}