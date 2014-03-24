using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UIShell.OSGi;

namespace UIShell.ExtensionProviderService
{
    public class ExtensionProviderHandler
    {
        private IBundle _bundle;
        public ExtensionProviderCollection ExtensionProviders = new ExtensionProviderCollection();

        public ExtensionProviderHandler(IBundle bundle)
        {
            _bundle = bundle;

            HandleBootstrapLayoutExtensions();

            bundle.Context.ExtensionChanged += Context_ExtensionChanged;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void Context_ExtensionChanged(object sender, ExtensionEventArgs e)
        {
            HandleBootstrapLayoutExtensions(e.ExtensionPoint);
        }

        private void HandleBootstrapLayoutExtensions(string extensionPoint = null)
        {
            if (_bundle.Context != null)
            {
                List<Extension> extensions = new List<Extension>();

                if (string.IsNullOrEmpty(extensionPoint))
                {
                    ExtensionProviders.Clear();

                    foreach (var extPoint in _bundle.Context.GetExtensionPoints())
                    {
                        extensions.AddRange(_bundle.Context.GetExtensions(extPoint.Point));
                    }
                }
                else
                {
                    if (ExtensionProviders.ContainsKey(extensionPoint))
                        ExtensionProviders[extensionPoint].Clear();

                    extensions = _bundle.Context.GetExtensions(extensionPoint);
                }

                foreach (var extension in extensions)
                {
                    ExtensionProvider.BuildBootstrapLayout(extension, ExtensionProviders);
                }
            }
        }
    }
}
