﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml;
using UIShell.OSGi;
using UIShell.OSGi.Utility;

namespace UIShell.ExtensionProviderService
{
    public class ExtensionProvider
    {
        public string ExtensionPoint { get; set; }
        public NameValueCollection AttributesCollection { get; set; }
        public IBundle Bundle { get; set; }
        public ExtensionProviderCollection ChildExtensionProvider { get; set; }

        public static void BuildBootstrapLayout(Extension extension, ExtensionProviderCollection extensionProviders)
        {
            foreach (var xmlNode in extension.Data)
            {
                if (xmlNode is XmlComment)
                {
                    continue;
                }

                NameValueCollection attributesCollection = new NameValueCollection();
                foreach (XmlAttribute attr in xmlNode.Attributes)
                {
                    attributesCollection.Add(attr.Name, attr.Value);
                }

                var extensionPoint = xmlNode.ParentNode.FirstChild.ParentNode.Attributes["Point"].Value;
                ExtensionProvider extensionProvider = new ExtensionProvider { ExtensionPoint = extensionPoint, AttributesCollection = attributesCollection, Bundle = extension.Owner, ChildExtensionProvider = GenerateChildExtensionProvider(xmlNode.ChildNodes, extension, extensionPoint) };
                extensionProviders.AddExtensionProvider(extensionProvider);
            }
        }

        private static ExtensionProviderCollection GenerateChildExtensionProvider(XmlNodeList childNodes, Extension extension, string extensionPoint)
        {
            ExtensionProviderCollection extensionProviders = new ExtensionProviderCollection();

            foreach (XmlNode xmlNode in childNodes)
            {
                if (xmlNode is XmlComment)
                {
                    continue;
                }

                NameValueCollection attributesCollection = new NameValueCollection();
                foreach (XmlAttribute attr in xmlNode.Attributes)
                {
                    attributesCollection.Add(attr.Name, attr.Value);
                }

                ExtensionProvider extensionProvider = new ExtensionProvider { ExtensionPoint = extensionPoint, AttributesCollection = attributesCollection, Bundle = extension.Owner, ChildExtensionProvider = GenerateChildExtensionProvider(xmlNode.ChildNodes, extension, extensionPoint) };
                extensionProviders.AddExtensionProvider(extensionProvider);
            }

            return extensionProviders;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("ExtensionPoint: ");
            sb.Append(ExtensionPoint);
            sb.Append(", BundleSymbolicName: ");
            sb.Append(Bundle.SymbolicName);

            foreach (var attr in AttributesCollection)
            {
                sb.Append(", ");
                sb.Append(attr.ToString());
                sb.Append(": ");
                sb.Append(AttributesCollection[attr.ToString()]);
            }

            return sb.ToString();
        }
    }

    public class ExtensionProviderCollection : Dictionary<string, List<ExtensionProvider>>
    {
        public void AddExtensionProvider(ExtensionProvider extensionProvider)
        {
            if (ContainsKey(extensionProvider.ExtensionPoint))
            {
                this[extensionProvider.ExtensionPoint].Add(extensionProvider);
            }
            else
            {
                Add(extensionProvider.ExtensionPoint, new List<ExtensionProvider>() { extensionProvider });
            }
        }
    }
}
