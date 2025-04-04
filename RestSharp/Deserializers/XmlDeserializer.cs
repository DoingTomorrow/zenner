// Decompiled with JetBrains decompiler
// Type: RestSharp.Deserializers.XmlDeserializer
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using RestSharp.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

#nullable disable
namespace RestSharp.Deserializers
{
  public class XmlDeserializer : IDeserializer
  {
    public string RootElement { get; set; }

    public string Namespace { get; set; }

    public string DateFormat { get; set; }

    public CultureInfo Culture { get; set; }

    public XmlDeserializer() => this.Culture = CultureInfo.InvariantCulture;

    public T Deserialize<T>(IRestResponse response)
    {
      if (string.IsNullOrEmpty(response.Content))
        return default (T);
      XDocument xdoc = XDocument.Parse(response.Content);
      XElement root = xdoc.Root;
      if (this.RootElement.HasValue() && xdoc.Root != null)
        root = xdoc.Root.Element(this.RootElement.AsNamespaced(this.Namespace));
      if (!this.Namespace.HasValue())
        this.RemoveNamespace(xdoc);
      T x = Activator.CreateInstance<T>();
      Type type = x.GetType();
      if (type.IsSubclassOfRawGeneric(typeof (List<>)))
        x = (T) this.HandleListDerivative((object) x, root, type.Name, type);
      else
        this.Map((object) x, root);
      return x;
    }

    private void RemoveNamespace(XDocument xdoc)
    {
      foreach (XElement xelement in xdoc.Root.DescendantsAndSelf())
      {
        if (xelement.Name.Namespace != XNamespace.None)
          xelement.Name = XNamespace.None.GetName(xelement.Name.LocalName);
        if (xelement.Attributes().Any<XAttribute>((Func<XAttribute, bool>) (a => a.IsNamespaceDeclaration || a.Name.Namespace != XNamespace.None)))
          xelement.ReplaceAttributes((object) xelement.Attributes().Select<XAttribute, XAttribute>((Func<XAttribute, XAttribute>) (a =>
          {
            if (a.IsNamespaceDeclaration)
              return (XAttribute) null;
            return !(a.Name.Namespace != XNamespace.None) ? a : new XAttribute(XNamespace.None.GetName(a.Name.LocalName), (object) a.Value);
          })));
      }
    }

    private void Map(object x, XElement root)
    {
      foreach (PropertyInfo property in x.GetType().GetProperties())
      {
        Type type = property.PropertyType;
        if (type.IsPublic && property.CanWrite)
        {
          XName name = property.Name.AsNamespaced(this.Namespace);
          object valueFromXml = this.GetValueFromXml(root, name);
          if (valueFromXml == null)
          {
            if (type.IsGenericType)
            {
              Type genericArgument = type.GetGenericArguments()[0];
              XElement elementByName = this.GetElementByName(root, (XName) genericArgument.Name);
              IList instance = (IList) Activator.CreateInstance(type);
              if (elementByName != null)
              {
                IEnumerable<XElement> elements = root.Elements(elementByName.Name);
                this.PopulateListFromElements(genericArgument, elements, instance);
              }
              property.SetValue(x, (object) instance, (object[]) null);
            }
          }
          else
          {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>))
            {
              if (valueFromXml == null || string.IsNullOrEmpty(valueFromXml.ToString()))
              {
                property.SetValue(x, (object) null, (object[]) null);
                continue;
              }
              type = type.GetGenericArguments()[0];
            }
            if (type == typeof (bool))
            {
              string lower = valueFromXml.ToString().ToLower();
              property.SetValue(x, (object) XmlConvert.ToBoolean(lower), (object[]) null);
            }
            else if (type.IsPrimitive)
              property.SetValue(x, valueFromXml.ChangeType(type, this.Culture), (object[]) null);
            else if (type.IsEnum)
            {
              object enumValue = type.FindEnumValue(valueFromXml.ToString(), this.Culture);
              property.SetValue(x, enumValue, (object[]) null);
            }
            else if (type == typeof (Uri))
            {
              Uri uri = new Uri(valueFromXml.ToString(), UriKind.RelativeOrAbsolute);
              property.SetValue(x, (object) uri, (object[]) null);
            }
            else if (type == typeof (string))
              property.SetValue(x, valueFromXml, (object[]) null);
            else if (type == typeof (DateTime))
            {
              object obj = !this.DateFormat.HasValue() ? (object) DateTime.Parse(valueFromXml.ToString(), (IFormatProvider) this.Culture) : (object) DateTime.ParseExact(valueFromXml.ToString(), this.DateFormat, (IFormatProvider) this.Culture);
              property.SetValue(x, obj, (object[]) null);
            }
            else if (type == typeof (Decimal))
            {
              object obj = (object) Decimal.Parse(valueFromXml.ToString(), (IFormatProvider) this.Culture);
              property.SetValue(x, obj, (object[]) null);
            }
            else if (type == typeof (Guid))
            {
              object obj = (object) (string.IsNullOrEmpty(valueFromXml.ToString()) ? Guid.Empty : new Guid(valueFromXml.ToString()));
              property.SetValue(x, obj, (object[]) null);
            }
            else if (type == typeof (TimeSpan))
            {
              TimeSpan timeSpan = XmlConvert.ToTimeSpan(valueFromXml.ToString());
              property.SetValue(x, (object) timeSpan, (object[]) null);
            }
            else if (type.IsGenericType)
            {
              Type genericArgument = type.GetGenericArguments()[0];
              IList instance = (IList) Activator.CreateInstance(type);
              XElement elementByName = this.GetElementByName(root, property.Name.AsNamespaced(this.Namespace));
              if (elementByName.HasElements)
              {
                XElement xelement = elementByName.Elements().FirstOrDefault<XElement>();
                IEnumerable<XElement> elements = elementByName.Elements(xelement.Name);
                this.PopulateListFromElements(genericArgument, elements, instance);
              }
              property.SetValue(x, (object) instance, (object[]) null);
            }
            else if (type.IsSubclassOfRawGeneric(typeof (List<>)))
            {
              object obj = this.HandleListDerivative(x, root, property.Name, type);
              property.SetValue(x, obj, (object[]) null);
            }
            else if (root != null)
            {
              XElement elementByName = this.GetElementByName(root, name);
              if (elementByName != null)
              {
                object andMap = this.CreateAndMap(type, elementByName);
                property.SetValue(x, andMap, (object[]) null);
              }
            }
          }
        }
      }
    }

    private void PopulateListFromElements(Type t, IEnumerable<XElement> elements, IList list)
    {
      foreach (XElement element in elements)
      {
        object andMap = this.CreateAndMap(t, element);
        list.Add(andMap);
      }
    }

    private object HandleListDerivative(object x, XElement root, string propName, Type type)
    {
      Type t = !type.IsGenericType ? type.BaseType.GetGenericArguments()[0] : type.GetGenericArguments()[0];
      IList instance = (IList) Activator.CreateInstance(type);
      IEnumerable<XElement> xelements = root.Descendants(t.Name.AsNamespaced(this.Namespace));
      string name = t.Name;
      if (!xelements.Any<XElement>())
      {
        XName name1 = name.ToLower().AsNamespaced(this.Namespace);
        xelements = root.Descendants(name1);
      }
      if (!xelements.Any<XElement>())
      {
        XName name2 = name.ToCamelCase(this.Culture).AsNamespaced(this.Namespace);
        xelements = root.Descendants(name2);
      }
      if (!xelements.Any<XElement>())
        xelements = root.Descendants().Where<XElement>((Func<XElement, bool>) (e => e.Name.LocalName.RemoveUnderscoresAndDashes() == name));
      if (!xelements.Any<XElement>())
      {
        XName lowerName = name.ToLower().AsNamespaced(this.Namespace);
        xelements = root.Descendants().Where<XElement>((Func<XElement, bool>) (e => (XName) e.Name.LocalName.RemoveUnderscoresAndDashes() == lowerName));
      }
      this.PopulateListFromElements(t, xelements, instance);
      if (!type.IsGenericType)
        this.Map((object) instance, root.Element(propName.AsNamespaced(this.Namespace)) ?? root);
      return (object) instance;
    }

    private object CreateAndMap(Type t, XElement element)
    {
      object x;
      if (t == typeof (string))
        x = (object) element.Value;
      else if (t.IsPrimitive)
      {
        x = element.Value.ChangeType(t, this.Culture);
      }
      else
      {
        x = Activator.CreateInstance(t);
        this.Map(x, element);
      }
      return x;
    }

    private object GetValueFromXml(XElement root, XName name)
    {
      object valueFromXml = (object) null;
      if (root != null)
      {
        XElement elementByName = this.GetElementByName(root, name);
        if (elementByName == null)
        {
          XAttribute attributeByName = this.GetAttributeByName(root, name);
          if (attributeByName != null)
            valueFromXml = (object) attributeByName.Value;
        }
        else if (!elementByName.IsEmpty || elementByName.HasElements || elementByName.HasAttributes)
          valueFromXml = (object) elementByName.Value;
      }
      return valueFromXml;
    }

    private XElement GetElementByName(XElement root, XName name)
    {
      XName name1 = name.LocalName.ToLower().AsNamespaced(name.NamespaceName);
      XName name2 = name.LocalName.ToCamelCase(this.Culture).AsNamespaced(name.NamespaceName);
      if (root.Element(name) != null)
        return root.Element(name);
      if (root.Element(name1) != null)
        return root.Element(name1);
      if (root.Element(name2) != null)
        return root.Element(name2);
      return name == "Value".AsNamespaced(name.NamespaceName) ? root : (root.Descendants().OrderBy<XElement, int>((Func<XElement, int>) (d => d.Ancestors().Count<XElement>())).FirstOrDefault<XElement>((Func<XElement, bool>) (d => d.Name.LocalName.RemoveUnderscoresAndDashes() == name.LocalName)) ?? root.Descendants().OrderBy<XElement, int>((Func<XElement, int>) (d => d.Ancestors().Count<XElement>())).FirstOrDefault<XElement>((Func<XElement, bool>) (d => d.Name.LocalName.RemoveUnderscoresAndDashes() == name.LocalName.ToLower()))) ?? (XElement) null;
    }

    private XAttribute GetAttributeByName(XElement root, XName name)
    {
      XName name1 = name.LocalName.ToLower().AsNamespaced(name.NamespaceName);
      XName name2 = name.LocalName.ToCamelCase(this.Culture).AsNamespaced(name.NamespaceName);
      if (root.Attribute(name) != null)
        return root.Attribute(name);
      if (root.Attribute(name1) != null)
        return root.Attribute(name1);
      return root.Attribute(name2) != null ? root.Attribute(name2) : root.Attributes().FirstOrDefault<XAttribute>((Func<XAttribute, bool>) (d => d.Name.LocalName.RemoveUnderscoresAndDashes() == name.LocalName)) ?? (XAttribute) null;
    }
  }
}
