// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.XPathResult
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public class XPathResult
  {
    public readonly bool CanWrite;
    public readonly PropertyDescriptor Property;
    private readonly object matchingBehavior;
    private readonly Func<XPathNavigator> create;
    private static readonly Dictionary<Type, string> XmlDataTypes = new Dictionary<Type, string>()
    {
      {
        typeof (int),
        "int"
      },
      {
        typeof (long),
        "long"
      },
      {
        typeof (short),
        "short"
      },
      {
        typeof (float),
        "float"
      },
      {
        typeof (double),
        "double"
      },
      {
        typeof (bool),
        "boolean"
      },
      {
        typeof (DateTime),
        "dateTime"
      },
      {
        typeof (byte),
        "byte"
      },
      {
        typeof (uint),
        "uint"
      },
      {
        typeof (ulong),
        "ulong"
      },
      {
        typeof (ushort),
        "ushort"
      }
    };

    public XPathResult(
      PropertyDescriptor property,
      object result,
      XPathContext context,
      object matchingBehavior)
      : this(property, result, context, matchingBehavior, (Func<XPathNavigator>) null)
    {
    }

    public XPathResult(Type type, object result, XPathContext context, object matchingBehavior)
      : this((PropertyDescriptor) null, result, context, matchingBehavior, (Func<XPathNavigator>) null)
    {
      this.Type = type;
    }

    public XPathResult(
      PropertyDescriptor property,
      object result,
      XPathContext context,
      object matchingBehavior,
      Func<XPathNavigator> create)
    {
      this.Result = result;
      this.Property = property;
      this.Type = property != null ? this.Property.PropertyType : (Type) null;
      this.Context = context;
      this.create = create;
      this.matchingBehavior = matchingBehavior;
      this.CanWrite = create != null || result is XPathNavigator;
    }

    public bool IsContainer
    {
      get => this.matchingBehavior == null || this.matchingBehavior is XmlArrayAttribute;
    }

    public Type Type { get; private set; }

    public object Result { get; private set; }

    public XPathContext Context { get; private set; }

    public XPathNavigator Container { get; private set; }

    public XmlMetadata XmlMeta { get; private set; }

    public bool OmitPolymorphism { get; private set; }

    public XPathNavigator GetNavigator(bool demand)
    {
      if (this.Result is XPathNavigator)
        return (XPathNavigator) this.Result;
      if (this.Result is XPathNodeIterator)
        return ((IEnumerable) this.Result).Cast<XPathNavigator>().FirstOrDefault<XPathNavigator>();
      if (!demand || this.create == null)
        return (XPathNavigator) null;
      XPathNavigator navigator = this.create();
      this.Result = (object) navigator;
      return navigator;
    }

    public XPathResult GetNodeAt(Type type, int index)
    {
      XPathNavigator result = this.Container;
      if (this.IsContainer)
      {
        if (result != null)
          result = this.Container.SelectSingleNode(string.Format("*[position()={0}]", (object) (index + 1)));
      }
      else if (this.Result is XPathNodeIterator)
        result = ((IEnumerable) this.Result).Cast<XPathNavigator>().ToArray<XPathNavigator>()[index];
      return new XPathResult(type, (object) result, this.Context, this.matchingBehavior);
    }

    public IEnumerable<XPathResult> GetNodes(Type type, Func<Type, XmlMetadata> getXmlMeta)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      XPathResult.\u003C\u003Ec__DisplayClassf cDisplayClassf1 = new XPathResult.\u003C\u003Ec__DisplayClassf();
      // ISSUE: reference to a compiler-generated field
      cDisplayClassf1.type = type;
      // ISSUE: reference to a compiler-generated field
      cDisplayClassf1.getXmlMeta = getXmlMeta;
      // ISSUE: reference to a compiler-generated field
      cDisplayClassf1.\u003C\u003E4__this = this;
      this.Container = (XPathNavigator) null;
      XPathNodeIterator result = this.Result as XPathNodeIterator;
      IEnumerable<XPathNavigator> source = Enumerable.Empty<XPathNavigator>();
      if (result == null)
      {
        this.Container = this.Result as XPathNavigator;
        if (this.IsContainer && this.Container != null)
        {
          if (this.Context.ListItemMeta != null)
          {
            // ISSUE: reference to a compiler-generated method
            return (IEnumerable<XPathResult>) this.Context.ListItemMeta.SelectMany<XmlArrayItemAttribute, XPathResult>(new Func<XmlArrayItemAttribute, IEnumerable<XPathResult>>(cDisplayClassf1.\u003CGetNodes\u003Eb__2)).OrderBy<XPathResult, XPathNavigator>((Func<XPathResult, XPathNavigator>) (r => (XPathNavigator) r.Result), (IComparer<XPathNavigator>) XPathPositionComparer.Instance);
          }
          source = this.Container.SelectChildren(XPathNodeType.Element).Cast<XPathNavigator>();
        }
      }
      else if (!this.IsContainer)
      {
        source = result.Cast<XPathNavigator>();
      }
      else
      {
        List<XPathNavigator> parents = result.Cast<XPathNavigator>().ToList<XPathNavigator>();
        this.Container = parents.FirstOrDefault<XPathNavigator>();
        if (this.Context.ListItemMeta != null)
          return (IEnumerable<XPathResult>) this.Context.ListItemMeta.SelectMany<XmlArrayItemAttribute, XPathResult>((Func<XmlArrayItemAttribute, IEnumerable<XPathResult>>) (item =>
          {
            // ISSUE: variable of a compiler-generated type
            XPathResult.\u003C\u003Ec__DisplayClassf cDisplayClassf = cDisplayClassf1;
            XmlArrayItemAttribute item1 = item;
            string name;
            string namespaceUri;
            XmlMetadata xmlMeta = this.GetItemQualifedName(type, item1, getXmlMeta, out name, out namespaceUri);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return parents.SelectMany<XPathNavigator, XPathNavigator>((Func<XPathNavigator, IEnumerable<XPathNavigator>>) (p => p.SelectChildren(name, namespaceUri).Cast<XPathNavigator>())).Select<XPathNavigator, XPathResult>((Func<XPathNavigator, XPathResult>) (r => new XPathResult(item1.Type ?? cDisplayClassf.type, (object) r, cDisplayClassf.\u003C\u003E4__this.Context, cDisplayClassf.\u003C\u003E4__this.matchingBehavior)
            {
              XmlMeta = xmlMeta
            }));
          })).OrderBy<XPathResult, XPathNavigator>((Func<XPathResult, XPathNavigator>) (r => (XPathNavigator) r.Result), (IComparer<XPathNavigator>) XPathPositionComparer.Instance);
        source = parents.SelectMany<XPathNavigator, XPathNavigator>((Func<XPathNavigator, IEnumerable<XPathNavigator>>) (p => p.SelectChildren(XPathNodeType.Element).Cast<XPathNavigator>()));
      }
      // ISSUE: reference to a compiler-generated method
      return source.Select<XPathNavigator, XPathResult>(new Func<XPathNavigator, XPathResult>(cDisplayClassf1.\u003CGetNodes\u003Eb__a));
    }

    public bool ReadObject(out object value)
    {
      XPathNavigator navigator = this.GetNavigator(false);
      foreach (IXPathSerializer serializer in this.Context.Serializers)
      {
        if (serializer.ReadObject(this, navigator, out value))
          return true;
      }
      value = (object) null;
      return false;
    }

    public bool WriteObject(object value)
    {
      XPathNavigator navigator = this.GetNavigator(true);
      foreach (IXPathSerializer serializer in this.Context.Serializers)
      {
        if (serializer.WriteObject(this, navigator, value))
          return true;
      }
      return false;
    }

    private XmlMetadata GetItemQualifedName(
      Type type,
      XmlArrayItemAttribute item,
      Func<Type, XmlMetadata> getXmlMeta,
      out string name,
      out string namespaceUri)
    {
      name = item.ElementName;
      namespaceUri = item.Namespace;
      type = item.Type ?? type;
      XmlMetadata itemQualifedName = getXmlMeta(type);
      if (string.IsNullOrEmpty(name))
      {
        name = itemQualifedName == null ? XPathResult.GetDataType(type) : itemQualifedName.XmlType.TypeName;
        namespaceUri = (string) null;
      }
      namespaceUri = this.Context.GetEffectiveNamespace(namespaceUri);
      return itemQualifedName;
    }

    public XPathResult CreateNode(Type type, object value, Func<Type, XmlMetadata> getXmlMeta)
    {
      string name = (string) null;
      string namespaceUri1 = (string) null;
      string namespaceUri2 = (string) null;
      bool flag = false;
      Type baseType = type;
      XmlMetadata xmlMetadata = getXmlMeta(type);
      if (xmlMetadata != null)
      {
        name = xmlMetadata.XmlType.TypeName;
        namespaceUri2 = xmlMetadata.XmlType.Namespace;
      }
      if (value != null)
        type = !(value is IDictionaryAdapter) ? value.GetType() : ((IDictionaryAdapter) value).Meta.Type;
      if (xmlMetadata == null)
        name = XPathResult.GetDataType(type);
      if (this.Context.ListItemMeta != null)
      {
        Type actualType = type;
        XmlArrayItemAttribute arrayItemAttribute = this.Context.ListItemMeta.FirstOrDefault<XmlArrayItemAttribute>((Func<XmlArrayItemAttribute, bool>) (li => (li.Type ?? baseType) == actualType));
        if (arrayItemAttribute != null)
        {
          type = arrayItemAttribute.Type ?? baseType;
          if (string.IsNullOrEmpty(arrayItemAttribute.ElementName))
          {
            if (arrayItemAttribute.Type != null)
            {
              xmlMetadata = getXmlMeta(arrayItemAttribute.Type);
              if (xmlMetadata != null)
              {
                name = xmlMetadata.XmlType.TypeName;
                namespaceUri2 = xmlMetadata.XmlType.Namespace;
              }
              else
                name = XPathResult.GetDataType(arrayItemAttribute.Type);
            }
          }
          else
          {
            name = arrayItemAttribute.ElementName;
            namespaceUri1 = arrayItemAttribute.Namespace;
          }
          flag = true;
        }
      }
      XPathNavigator xpathNavigator = (XPathNavigator) null;
      if (this.IsContainer)
      {
        if (this.Container == null && this.create != null)
          this.Container = this.create();
        if (this.Container != null)
          xpathNavigator = this.Context.AppendElement(name, namespaceUri1, this.Container);
      }
      else if (this.create != null)
        xpathNavigator = this.create();
      if (!string.IsNullOrEmpty(namespaceUri2))
        this.Context.CreateNamespace((string) null, namespaceUri2, xpathNavigator);
      return new XPathResult(type, (object) xpathNavigator, this.Context, this.matchingBehavior)
      {
        XmlMeta = xmlMetadata,
        OmitPolymorphism = flag
      };
    }

    public void RemoveAt(int index) => this.GetNodeAt((Type) null, index).Remove();

    public void Remove()
    {
      if (this.Result is XPathNavigator)
        ((XPathNavigator) this.Result).DeleteSelf();
      else if (this.Result is XPathNodeIterator)
      {
        foreach (XPathNavigator xpathNavigator in ((IEnumerable) this.Result).Cast<XPathNavigator>().ToArray<XPathNavigator>())
          xpathNavigator.DeleteSelf();
      }
      this.Result = (object) null;
    }

    public XPathNavigator RemoveChildren()
    {
      XPathNavigator navigator = this.GetNavigator(true);
      if (navigator != null)
      {
        foreach (XPathNavigator xpathNavigator in navigator.SelectChildren(XPathNodeType.All).Cast<XPathNavigator>().ToArray<XPathNavigator>())
          xpathNavigator.DeleteSelf();
      }
      return navigator;
    }

    private static string GetDataType(Type type)
    {
      string lower;
      if (!XPathResult.XmlDataTypes.TryGetValue(type, out lower))
        lower = type.Name.ToLower();
      return lower;
    }
  }
}
