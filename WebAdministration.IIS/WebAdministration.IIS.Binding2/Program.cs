﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Web.Administration;

namespace WebAdministration.IIS.Binding2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServerManager serverManager = new ServerManager())
            {
                Configuration config = serverManager.GetApplicationHostConfiguration();
                ConfigurationSection sitesSection = config.GetSection("system.applicationHost/sites");
                ConfigurationElementCollection sitesCollection = sitesSection.GetCollection();

                ConfigurationElement siteElement = FindElement(sitesCollection, "site", "name", @"EHR");

                if (siteElement == null) throw new InvalidOperationException("Element not found!");

                ConfigurationElementCollection bindingsCollection = siteElement.GetCollection("bindings");
                ConfigurationElement bindingElement = bindingsCollection.CreateElement("binding");
                bindingElement["protocol"] = @"http";
                bindingElement["bindingInformation"] = @"*:80:ss.text";
                bindingsCollection.Add(bindingElement);
                //==============================
               
                X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadWrite);

                // Here, directory is my install dir, and (directory)\bin\certificate.pfx is where the cert file is.
                // 1234 is the password to the certfile (exported from IIS)
                //X509Certificate2 certificate = new X509Certificate2(directory + @"\bin\certificate.pfx", "1234");

                //store.Add(certificate);

                //var binding = site.Bindings.Add("*:443:", certificate.GetCertHash(), store.Name);
                //binding.Protocol = "https";

                Binding bindingElement1 = bindingsCollection.CreateElement("binding") as Binding;
                //bindingElement1["protocol"] = @"https";
                //bindingElement1["bindingInformation"] = @"*:443:";
                if (bindingElement1 != null)
                {
                    bindingElement1.Protocol= @"https";
                    bindingElement1.BindingInformation = @"*:443:";
                    bindingsCollection.Add(bindingElement1);
                }
                //=======================================================
                serverManager.CommitChanges();
            }
            Console.WriteLine("Success");
        }
        private static ConfigurationElement FindElement(ConfigurationElementCollection collection, string elementTagName, params string[] keyValues)
        {
            foreach (ConfigurationElement element in collection)
            {
                if (String.Equals(element.ElementTagName, elementTagName, StringComparison.OrdinalIgnoreCase))
                {
                    bool matches = true;
                    for (int i = 0; i < keyValues.Length; i += 2)
                    {
                        object o = element.GetAttributeValue(keyValues[i]);
                        string value = null;
                        if (o != null)
                        {
                            value = o.ToString();
                        }
                        if (!String.Equals(value, keyValues[i + 1], StringComparison.OrdinalIgnoreCase))
                        {
                            matches = false;
                            break;
                        }
                    }
                    if (matches)
                    {
                        return element;
                    }
                }
            }
            return null;
        }
    }
}
