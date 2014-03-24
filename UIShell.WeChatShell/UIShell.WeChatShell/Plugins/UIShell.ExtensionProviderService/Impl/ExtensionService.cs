using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace UIShell.ExtensionProviderService
{
    public class ExtensionService : IExtensionService
    {
        public List<ExtensionProvider> GetExtensionProviders(string extensionPoint)
        {
            try
            {
                return Activator.ExtensionProviderHandler.ExtensionProviders.Where(i => i.Key == extensionPoint).First().Value;
            }
            catch
            {
                return new List<ExtensionProvider>();
            }
        }
    }
}
