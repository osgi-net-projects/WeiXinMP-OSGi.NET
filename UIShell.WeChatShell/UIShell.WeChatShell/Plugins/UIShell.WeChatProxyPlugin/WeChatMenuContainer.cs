using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UIShell.ExtensionProviderService;

namespace UIShell.WeChatProxyPlugin
{
    public class WeChatMenuContainer
    {
        private const string WECHAT_MENU = "UIShell.WeChatMenu";
        private const string NAME_ATTRIBUTE = "Name";
        private const string TYPE_ATTRIBUTE = "Type";
        private const string KEY_ATTRIBUTE = "Key";
        private const string URL_ATTRIBUTE = "Url";
        private const string APPID_ATTRIBUTE = "AppId";
        private const string SECRET_ATTRIBUTE = "Secret";

        private List<WeChatMenu> _weChatMenus = new List<WeChatMenu>();

        public List<WeChatMenu> WeChatMenus
        {
            get
            {
                BuildWeChatMenus();

                return _weChatMenus;
            }
        }

        private void BuildWeChatMenus()
        {
            _weChatMenus = new List<WeChatMenu>();

            var extensions = Activator.ExtensionService.GetExtensionProviders(WECHAT_MENU);
            foreach (var extension in extensions)
            {
                string name = extension.AttributesCollection[NAME_ATTRIBUTE];
                string appid = extension.AttributesCollection[APPID_ATTRIBUTE];
                string secret = extension.AttributesCollection[SECRET_ATTRIBUTE];
                string symbolicName = extension.Bundle.SymbolicName;

                if (Utility.Validate(appid, APPID_ATTRIBUTE, symbolicName, WECHAT_MENU) && Utility.Validate(secret, SECRET_ATTRIBUTE, symbolicName, WECHAT_MENU))
                {
                    _weChatMenus.Add(new WeChatMenu() { Name = name, AppId = appid, Secret = secret, Bundle = extension.Bundle, MenuButtons = GenerateMenuButtons(extension.ChildExtensionProvider, symbolicName) });
                }
            }
        }

        private List<MenuButton> GenerateMenuButtons(ExtensionProviderCollection collections, string symbolicName)
        {
            List<MenuButton> buttons = new List<MenuButton>();

            foreach (var collection in collections)
            {
                foreach (var val in collection.Value)
                {
                    string name = val.AttributesCollection[NAME_ATTRIBUTE];
                    string type = val.AttributesCollection[TYPE_ATTRIBUTE];

                    if (Utility.Validate(name, NAME_ATTRIBUTE, symbolicName, WECHAT_MENU))
                    {
                        if (Utility.Validate(type, TYPE_ATTRIBUTE, symbolicName, WECHAT_MENU))
                        {
                            var buttonType = MenuType.Click;
                            if (!Enum.TryParse<MenuType>(type, out buttonType))
                            {
                                Utility.NotInCorrectFormat(TYPE_ATTRIBUTE, symbolicName, WECHAT_MENU);
                                continue;
                            }

                            string key = val.AttributesCollection[KEY_ATTRIBUTE];
                            string url = val.AttributesCollection[URL_ATTRIBUTE];

                            if ((buttonType == MenuType.Click && Utility.Validate(key, KEY_ATTRIBUTE, symbolicName, WECHAT_MENU)) || (buttonType == MenuType.View && Utility.Validate(url, URL_ATTRIBUTE, symbolicName, WECHAT_MENU)))
                            {
                                buttons.Add(new MenuButton() { Name = name, Type = buttonType, Key = key, Url = url, MenuSubButtons = GenerateMenuButtons(val.ChildExtensionProvider, symbolicName) });
                            }
                        }
                        else
                        {
                            buttons.Add(new MenuButton() { Name = name, MenuSubButtons = GenerateMenuButtons(val.ChildExtensionProvider, symbolicName) });
                        }
                    }
                }
            }

            return buttons;
        }
    }
}