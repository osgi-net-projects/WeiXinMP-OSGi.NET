using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;

namespace UIShell.ExtensionProviderService
{
    public interface IExtensionService
    {
        List<ExtensionProvider> GetExtensionProviders(string extensionPoint);
    }
}
