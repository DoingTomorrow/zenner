// Decompiled with JetBrains decompiler
// Type: MSS.Business.Utils.CustomerConfiguration
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;

#nullable disable
namespace MSS.Business.Utils
{
  public static class CustomerConfiguration
  {
    private static NameValueCollection _settings;

    private static void LoadCustomerSettings(string customerName)
    {
      XDocument document = XDocument.Load(ConfigurationManager.AppSettings["CustomerSettings"]);
      System.Diagnostics.Debug.Assert(CustomerConfiguration.AreCustomerPropertiesMatching(document), "Please check the customer-settings sections from CustomerSettings.config file.");
      CustomerConfiguration._settings = new NameValueCollection();
      TypeHelperExtensionMethods.ForEach<XElement>(document.Descendants((XName) "customer-settings").Where<XElement>((Func<XElement, bool>) (node => node.Attribute((XName) "name").Value == customerName)).Elements<XElement>((XName) "property"), (Action<XElement>) (p => CustomerConfiguration._settings.Add(p.Attribute((XName) "name").Value, p.Value)));
    }

    private static bool AreCustomerPropertiesMatching(XDocument document)
    {
      IEnumerable<XElement> source1 = document.Descendants((XName) "customer-settings");
      List<string> propertiesList = new List<string>();
      TypeHelperExtensionMethods.ForEach<XElement>(source1.FirstOrDefault<XElement>().Elements((XName) "property"), (Action<XElement>) (p => propertiesList.Add(p.Attribute((XName) "name").Value)));
      foreach (XContainer xcontainer in source1)
      {
        IEnumerable<XElement> source2 = xcontainer.Elements((XName) "property");
        if (source2.Count<XElement>() != propertiesList.Count)
          return false;
        foreach (XElement xelement in source2)
        {
          if (!propertiesList.Contains(xelement.Attribute((XName) "name").Value))
            return false;
        }
      }
      return true;
    }

    private static string GetPropertyValue(string customerName, string propertyName)
    {
      if (CustomerConfiguration._settings == null)
        CustomerConfiguration.LoadCustomerSettings(customerName);
      return CustomerConfiguration._settings != null && CustomerConfiguration._settings[propertyName] != null ? CustomerConfiguration._settings[propertyName] : throw new InvalidEnumArgumentException(propertyName);
    }

    public static T GetPropertyValue<T>(string propertyName)
    {
      string propertyValue = CustomerConfiguration.GetPropertyValue(propertyName);
      return (T) TypeDescriptor.GetConverter(typeof (T)).ConvertFromInvariantString(propertyValue);
    }

    public static string GetPropertyValue(string propertyName)
    {
      return CustomerConfiguration.GetPropertyValue(ConfigurationManager.AppSettings["Customer"], propertyName);
    }
  }
}
