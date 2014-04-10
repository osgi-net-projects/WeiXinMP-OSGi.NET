using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UIShell.OSGi.Utility;

namespace UIShell.WeChatProxyPlugin
{
    public class Utility
    {
        public static bool Validate(string value, string name, string symbolicName, string extensionPoint)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                FileLogUtility.Error(string.Format("{0} is not specified or not in correct format in bundle {1} for extension point {2} so it's or part of it's ignored.", name, symbolicName, extensionPoint));

                return false;
            }

            return true;
        }

        public static bool Validate(ref string value, string name, string symbolicName, string extensionPoint, string defaultValue)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                value = defaultValue;
                FileLogUtility.Error(string.Format("{0} is not specified in bundle {1} for extension point {2} so it's set to default {3}.", name, symbolicName, extensionPoint, defaultValue));
            }

            return true;
        }

        public static void NotInCorrectFormat(string name, string symbolicName, string extensionPoint)
        {
            FileLogUtility.Error(string.Format("{0} is not in correct format in bundle {1} for extension point {2} so it's ignored.", name, symbolicName, extensionPoint));
        }
    }
}