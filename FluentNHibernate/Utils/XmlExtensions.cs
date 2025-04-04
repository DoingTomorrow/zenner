// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Utils.XmlExtensions
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace FluentNHibernate.Utils
{
  internal static class XmlExtensions
  {
    public static XmlElement AddElement(this XmlDocument document, string name)
    {
      XmlElement element = document.CreateElement(name);
      document.AppendChild((XmlNode) element);
      return element;
    }

    public static XmlElement AddElement(this XmlNode element, string name)
    {
      XmlElement element1 = element.OwnerDocument.CreateElement(name);
      element.AppendChild((XmlNode) element1);
      return element1;
    }

    public static XmlElement WithAtt(this XmlElement element, string key, bool value)
    {
      return element.WithAtt(key, value.ToString().ToLowerInvariant());
    }

    public static XmlElement WithAtt(this XmlElement element, string key, int value)
    {
      return element.WithAtt(key, value.ToString());
    }

    public static XmlElement WithAtt(this XmlElement element, string key, TypeReference value)
    {
      return element.WithAtt(key, value.ToString());
    }

    public static XmlElement WithAtt(this XmlElement element, string key, string attValue)
    {
      element.SetAttribute(key, attValue);
      return element;
    }

    public static void SetAttributeOnChild(
      this XmlElement element,
      string childName,
      string attName,
      string attValue)
    {
      (element[childName] ?? element.AddElement(childName)).SetAttribute(attName, attValue);
    }

    public static XmlElement WithProperties(
      this XmlElement element,
      Dictionary<string, string> properties)
    {
      foreach (KeyValuePair<string, string> property in properties)
        element.SetAttribute(property.Key, property.Value);
      return element;
    }

    public static XmlElement SetColumnProperty(this XmlElement element, string name, string value)
    {
      (element["column"] ?? element.AddElement("column")).WithAtt(name, value);
      return element;
    }

    public static void ImportAndAppendChild(this XmlDocument document, XmlDocument toImport)
    {
      XmlNode newChild = document.ImportNode((XmlNode) toImport.DocumentElement, true);
      document.DocumentElement.AppendChild(newChild);
    }
  }
}
