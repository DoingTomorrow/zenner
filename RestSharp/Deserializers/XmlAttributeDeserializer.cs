// Decompiled with JetBrains decompiler
// Type: RestSharp.Deserializers.XmlAttributeDeserializer
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
  public class XmlAttributeDeserializer : IDeserializer
  {
    public string RootElement { get; set; }

    public string Namespace { get; set; }

    public string DateFormat { get; set; }

    public CultureInfo Culture { get; set; }

    public XmlAttributeDeserializer() => this.Culture = CultureInfo.InvariantCulture;

    public T Deserialize<T>(IRestResponse response)
    {
      if (response.Content == null)
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
          XName name1 = property.Name.AsNamespaced(this.Namespace);
          bool attribute1 = false;
          DeserializeAsAttribute attribute2 = property.GetAttribute<DeserializeAsAttribute>();
          if (attribute2 != null)
          {
            string name2 = attribute2.Name;
            name1 = name2 != null ? (XName) name2 : name1;
            attribute1 = attribute2.Attribute;
          }
          object valueFromXml = this.GetValueFromXml(root, name1, attribute1);
          if (valueFromXml == null)
          {
            if (type.IsGenericType)
            {
              Type genericArgument = type.GetGenericArguments()[0];
              XElement elementByName = this.GetElementByName(root, (XName) genericArgument.Name);
              if (elementByName != null)
              {
                IEnumerable<XElement> elements = root.Elements(elementByName.Name);
                IList instance = (IList) Activator.CreateInstance(type);
                this.PopulateListFromElements(genericArgument, elements, instance);
                property.SetValue(x, (object) instance, (object[]) null);
              }
            }
          }
          else
          {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>))
            {
              type = type.GetGenericArguments()[0];
              if (string.IsNullOrEmpty(valueFromXml.ToString()))
                continue;
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
              object obj = (object) new Guid(valueFromXml.ToString());
              property.SetValue(x, obj, (object[]) null);
            }
            else if (type.IsGenericType)
            {
              Type genericArgument = type.GetGenericArguments()[0];
              IList instance = (IList) Activator.CreateInstance(type);
              XElement elementByName = this.GetElementByName(root, name1);
              XElement first = elementByName.Elements().FirstOrDefault<XElement>();
              IEnumerable<XElement> elements = elementByName.Elements().Where<XElement>((Func<XElement, bool>) (d => d.Name == first.Name));
              this.PopulateListFromElements(genericArgument, elements, instance);
              property.SetValue(x, (object) instance, (object[]) null);
            }
            else if (type.IsSubclassOfRawGeneric(typeof (List<>)))
            {
              object obj = this.HandleListDerivative(x, root, name1.ToString(), type);
              property.SetValue(x, obj, (object[]) null);
            }
            else if (root != null)
            {
              XElement elementByName = this.GetElementByName(root, name1);
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
      Type genericArgument = type.BaseType.GetGenericArguments()[0];
      string str = genericArgument.Name;
      DeserializeAsAttribute attribute = ReflectionExtensions.GetAttribute<DeserializeAsAttribute>(genericArgument);
      if (attribute != null)
        str = attribute.Name ?? str;
      string lower = str.ToLower();
      string camelCase = str.ToCamelCase(this.Culture);
      IList instance = (IList) Activator.CreateInstance(type);
      IEnumerable<XElement> elements = (IEnumerable<XElement>) null;
      if (root.Descendants(str.AsNamespaced(this.Namespace)).Count<XElement>() != 0)
        elements = root.Descendants(genericArgument.Name.AsNamespaced(this.Namespace));
      if (root.Descendants((XName) lower).Count<XElement>() != 0)
        elements = root.Descendants((XName) lower);
      if (root.Descendants((XName) camelCase).Count<XElement>() != 0)
        elements = root.Descendants((XName) camelCase);
      this.PopulateListFromElements(genericArgument, elements, instance);
      this.Map((object) instance, root.Element(propName.AsNamespaced(this.Namespace)));
      return (object) instance;
    }

    private object CreateAndMap(Type t, XElement element)
    {
      object instance = Activator.CreateInstance(t);
      this.Map(instance, element);
      return instance;
    }

    private object GetValueFromXml(XElement root, XName name, bool attribute)
    {
      object valueFromXml = (object) null;
      if (root == null)
        return (object) null;
      if (attribute)
      {
        XAttribute attributeByName = this.GetAttributeByName(root, name);
        if (attributeByName != null)
          valueFromXml = (object) attributeByName.Value;
      }
      else
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
      XName name1 = XName.Get(name.LocalName.ToLower(), name.NamespaceName);
      XName name2 = XName.Get(name.LocalName.ToCamelCase(this.Culture), name.NamespaceName);
      if (root.Element(name) != null)
        return root.Element(name);
      if (root.Element(name1) != null)
        return root.Element(name1);
      if (root.Element(name2) != null)
        return root.Element(name2);
      return name == (XName) "Value" && root.Value != null ? root : root.Descendants().OrderBy<XElement, int>((Func<XElement, int>) (d => d.Ancestors().Count<XElement>())).FirstOrDefault<XElement>((Func<XElement, bool>) (d => d.Name.LocalName.RemoveUnderscoresAndDashes() == name.LocalName)) ?? (XElement) null;
    }

    private XAttribute GetAttributeByName(XElement root, XName name)
    {
      XName name1 = XName.Get(name.LocalName.ToLower(), name.NamespaceName);
      XName name2 = XName.Get(name.LocalName.ToCamelCase(this.Culture), name.NamespaceName);
      if (root.Attribute(name) != null)
        return root.Attribute(name);
      if (root.Attribute(name1) != null)
        return root.Attribute(name1);
      return root.Attribute(name2) != null ? root.Attribute(name2) : root.Attributes().FirstOrDefault<XAttribute>((Func<XAttribute, bool>) (d => d.Name.LocalName.RemoveUnderscoresAndDashes() == name.LocalName)) ?? (XAttribute) null;
    }
  }
}
