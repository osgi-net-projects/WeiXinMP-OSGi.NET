using System;
using System.Collections.Generic;
using System.Text;
using UIShell.OSGi;

namespace UIShell.ExtensionProviderService
{
    public class Activator : IBundleActivator
    {
        public static ExtensionProviderHandler ExtensionProviderHandler { get; private set; }

        public void Start(IBundleContext context)
        {
            context.AddService<IExtensionService>(new ExtensionService());

            ExtensionProviderHandler = new ExtensionProviderHandler(context.Bundle);
        }

        public void Stop(IBundleContext context)
        {
            //todo:
        }
    }
}
