// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.XPathAdapter
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public class XPathAdapter : 
    DictionaryBehaviorAttribute,
    IDictionaryInitializer,
    IDictionaryPropertyGetter,
    IDictionaryPropertySetter,
    IDictionaryBehavior,
    IDictionaryCreateStrategy
  {
    private readonly Func<XPathNavigator> createRoot;
    private XPathNavigator root;
    private XmlMetadata xmlMeta;
    private static readonly XPathExpression XPathElement = XPathExpression.Compile("*[castle-da:match($key,$ns)]");
    private static readonly XPathExpression XPathAttribute = XPathExpression.Compile("@*[castle-da:match($key,$ns)]");
    private static readonly XPathExpression XPathElementOrAttribute = XPathExpression.Compile("(*|@*)[castle-da:match($key,$ns)]");

    public XPathNavigator Root => this.EnsureOffRoot();

    public XPathAdapter Parent { get; private set; }

    public IXPathNavigable Source { get; private set; }

    public XPathContext Context { get; private set; }

    public XPathAdapter()
      : this((IXPathNavigable) new XmlDocument())
    {
    }

    public XPathAdapter(IXPathNavigable source)
    {
      this.Source = source;
      this.Context = new XPathContext();
      this.root = source.CreateNavigator();
    }

    protected XPathAdapter(XPathNavigator source, XPathAdapter parent)
    {
      this.Parent = parent;
      this.Context = parent.Context.CreateChild();
      this.root = source.Clone();
    }

    protected XPathAdapter(Func<XPathNavigator> createSource, XPathAdapter parent)
    {
      this.Parent = parent;
      this.Context = parent.Context.CreateChild();
      this.createRoot = createSource;
    }

    void IDictionaryInitializer.Initialize(IDictionaryAdapter dictionaryAdapter, object[] behaviors)
    {
      DictionaryAdapterMeta meta = dictionaryAdapter.Meta;
      if (meta.MetaInitializers.OfType<XPathBehavior>().FirstOrDefault<XPathBehavior>() == null)
        throw new InvalidOperationException(string.Format("Interface {0} requested xpath support, but was not configured properly.  Did you forget to add an XPathBehavior?", (object) meta.Type.FullName));
      dictionaryAdapter.This.CreateStrategy = (IDictionaryCreateStrategy) this;
      this.Context.ApplyBehaviors((IEnumerable) behaviors);
      this.xmlMeta = dictionaryAdapter.GetXmlMeta();
      if (!string.IsNullOrEmpty(this.xmlMeta.XmlType.Namespace))
        this.Context.AddNamespace(string.Empty, this.xmlMeta.XmlType.Namespace);
      if (this.Parent != null)
        return;
      foreach (object behavior in behaviors)
      {
        if (behavior is Castle.Components.DictionaryAdapter.XPathAttribute)
        {
          XPathExpression compiledExpression = ((Castle.Components.DictionaryAdapter.XPathAttribute) behavior).CompiledExpression;
          if (XPathAdapter.MoveOffRoot(this.root, XPathNodeType.Element) && !this.Context.Matches(compiledExpression, this.root))
          {
            XPathNavigator xpathNavigator = this.Context.SelectSingleNode(compiledExpression, this.root);
            if (xpathNavigator != null)
            {
              this.root = xpathNavigator;
              break;
            }
          }
          else
            break;
        }
      }
      XPathAdapter.MoveOffRoot(this.root, XPathNodeType.Element);
    }

    object IDictionaryPropertyGetter.GetPropertyValue(
      IDictionaryAdapter dictionaryAdapter,
      string key,
      object storedValue,
      PropertyDescriptor property,
      bool ifExists)
    {
      return XPathAdapter.ShouldIgnoreProperty(property) ? storedValue : dictionaryAdapter.This.ExtendedProperties[(object) property.PropertyName] ?? this.ReadProperty(this.EvaluateProperty(key, property, dictionaryAdapter), ifExists, dictionaryAdapter);
    }

    bool IDictionaryPropertySetter.SetPropertyValue(
      IDictionaryAdapter dictionaryAdapter,
      string key,
      ref object value,
      PropertyDescriptor property)
    {
      if (XPathAdapter.ShouldIgnoreProperty(property))
        return true;
      this.EnsureOffRoot();
      if (this.root.CanEdit)
      {
        XPathResult property1 = this.EvaluateProperty(key, property, dictionaryAdapter);
        if (property1.CanWrite)
          this.WriteProperty(property1, value, dictionaryAdapter);
      }
      return false;
    }

    object IDictionaryCreateStrategy.Create(
      IDictionaryAdapter adapter,
      Type type,
      IDictionary dictionary)
    {
      dictionary = dictionary ?? (IDictionary) new Hashtable();
      DictionaryDescriptor dictionaryDescriptor = new DictionaryDescriptor();
      adapter.This.Descriptor.CopyBehaviors((PropertyDescriptor) dictionaryDescriptor, (Func<IDictionaryBehavior, bool>) (b => !(b is XPathAdapter)));
      dictionaryDescriptor.AddBehavior((IDictionaryBehavior) XPathBehavior.Instance).AddBehavior((IDictionaryBehavior) new XPathAdapter((IXPathNavigable) new XmlDocument()));
      return adapter.This.Factory.GetAdapter(type, dictionary, (PropertyDescriptor) dictionaryDescriptor);
    }

    private object ReadProperty(
      XPathResult result,
      bool ifExists,
      IDictionaryAdapter dictionaryAdapter)
    {
      Type type = result.Type;
      object obj;
      if (this.ReadCustom(result, out obj))
        return obj;
      if (type != typeof (string))
      {
        if (typeof (IXPathNavigable).IsAssignableFrom(type))
          return this.ReadFragment(result);
        if (type.IsArray || typeof (IEnumerable).IsAssignableFrom(type))
          return this.ReadCollection(result, dictionaryAdapter);
        if (type.IsInterface)
          return this.ReadComponent(result, ifExists, dictionaryAdapter);
      }
      return this.ReadSimple(result);
    }

    private object ReadFragment(XPathResult result)
    {
      XPathNavigator navigator = result.GetNavigator(false);
      if (result.Type != typeof (XmlElement))
        return (object) navigator.Clone();
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.Load(navigator.ReadSubtree());
      return (object) xmlDocument.DocumentElement;
    }

    private object ReadSimple(XPathResult result)
    {
      XPathNavigator navigator = result.GetNavigator(false);
      if (navigator != null)
      {
        if (result.Type.IsEnum)
          return Enum.Parse(result.Type, navigator.Value);
        try
        {
          return navigator.ValueAs(result.Type);
        }
        catch (InvalidCastException ex)
        {
          object obj;
          if (DefaultXmlSerializer.Instance.ReadObject(result, navigator, out obj))
            return obj;
        }
      }
      return result.Result != null ? Convert.ChangeType(result.Result, result.Type) : (object) null;
    }

    private object ReadComponent(
      XPathResult result,
      bool ifExists,
      IDictionaryAdapter dictionaryAdapter)
    {
      XPathNavigator navigator = result.GetNavigator(false);
      if (navigator == null && ifExists)
        return (object) null;
      Type type = result.Type;
      XPathAdapter xpathAdapter;
      if (navigator != null)
      {
        if (result.XmlMeta != null)
        {
          type = result.XmlMeta.Type;
        }
        else
        {
          XmlQualifiedName xmlType = this.Context.GetXmlType(navigator);
          type = dictionaryAdapter.GetXmlSubclass(xmlType, type) ?? type;
        }
        xpathAdapter = new XPathAdapter(navigator, this);
      }
      else
        xpathAdapter = new XPathAdapter((Func<XPathNavigator>) (() => result.GetNavigator(true)), this);
      object adapter = dictionaryAdapter.This.Factory.GetAdapter(type, (IDictionary) new Hashtable(), new DictionaryDescriptor().AddBehavior((IDictionaryBehavior) XPathBehavior.Instance, (IDictionaryBehavior) xpathAdapter));
      if (result.Property != null)
        dictionaryAdapter.This.ExtendedProperties[(object) result.Property.PropertyName] = adapter;
      return adapter;
    }

    private object ReadCollection(XPathResult result, IDictionaryAdapter dictionaryAdapter)
    {
      if (result.Type.IsArray)
        return this.ReadArray(result, dictionaryAdapter);
      return result.Type.IsGenericType ? this.ReadList(result, dictionaryAdapter) : (object) null;
    }

    private object ReadArray(XPathResult result, IDictionaryAdapter dictionaryAdapter)
    {
      Type elementType = result.Type.GetElementType();
      object[] array = result.GetNodes(elementType, (Func<Type, XmlMetadata>) (type => dictionaryAdapter.GetXmlMeta(type))).Select<XPathResult, object>((Func<XPathResult, object>) (node => this.ReadProperty(node, false, dictionaryAdapter))).ToArray<object>();
      Array instance = Array.CreateInstance(elementType, array.Length);
      array.CopyTo(instance, 0);
      return (object) instance;
    }

    private object ReadList(XPathResult result, IDictionaryAdapter dictionaryAdapter)
    {
      Type type1 = (Type) null;
      Type[] genericArguments = result.Type.GetGenericArguments();
      Type genericTypeDefinition = result.Type.GetGenericTypeDefinition();
      Type itemType = genericArguments[0];
      Type type2;
      if (genericTypeDefinition == typeof (List<>))
      {
        type2 = typeof (EditableList<>).MakeGenericType(genericArguments);
      }
      else
      {
        type2 = typeof (EditableBindingList<>).MakeGenericType(genericArguments);
        type1 = typeof (BindingListInitializer<>).MakeGenericType(genericArguments);
      }
      IList instance = (IList) Activator.CreateInstance(type2);
      Func<Type, XmlMetadata> getXmlMeta = (Func<Type, XmlMetadata>) (type => dictionaryAdapter.GetXmlMeta(type));
      foreach (XPathResult node in result.GetNodes(itemType, getXmlMeta))
        instance.Add(this.ReadProperty(node, false, dictionaryAdapter));
      if (type1 != null)
      {
        Func<object> func = (Func<object>) (() => this.ReadProperty(result.CreateNode(itemType, (object) null, getXmlMeta), false, dictionaryAdapter));
        Action<int, object> action1 = (Action<int, object>) ((index, item) => this.WriteProperty(result.CreateNode(itemType, item, getXmlMeta), item, dictionaryAdapter));
        Action<int, object> action2 = (Action<int, object>) ((index, item) => this.WriteProperty(result.GetNodeAt(itemType, index), item, dictionaryAdapter));
        Action<int> action3 = (Action<int>) (index => result.RemoveAt(index));
        ((IValueInitializer) Activator.CreateInstance(type1, (object) action1, (object) func, (object) action2, (object) action3)).Initialize(dictionaryAdapter, (object) instance);
      }
      return (object) instance;
    }

    private bool ReadCustom(XPathResult result, out object value) => result.ReadObject(out value);

    private void WriteProperty(
      XPathResult result,
      object value,
      IDictionaryAdapter dictionaryAdapter)
    {
      Type type = result.Type;
      if (this.WriteCustom(result, value, dictionaryAdapter))
        return;
      if (type == typeof (string))
        this.WriteSimple(result, value, dictionaryAdapter);
      else if (typeof (IXPathNavigable).IsAssignableFrom(type))
        this.WriteFragment(result, (IXPathNavigable) value);
      else if (type.IsArray || typeof (IEnumerable).IsAssignableFrom(type))
        this.WriteCollection(result, value, dictionaryAdapter);
      else if (type.IsInterface)
        this.WriteComponent(result, value, dictionaryAdapter);
      else
        this.WriteSimple(result, value, dictionaryAdapter);
    }

    private void WriteFragment(XPathResult result, IXPathNavigable value)
    {
      XPathNavigator navigator = result.GetNavigator(true);
      if (navigator == null)
        this.root.AppendChild(value.CreateNavigator());
      else if (value != null)
        navigator.ReplaceSelf(value.CreateNavigator());
      else
        navigator.DeleteSelf();
    }

    private void WriteSimple(
      XPathResult result,
      object value,
      IDictionaryAdapter dictionaryAdapter)
    {
      if (value == null)
      {
        result.Remove();
        if (result.Property == null)
          return;
        dictionaryAdapter.This.ExtendedProperties.Remove((object) result.Property.PropertyName);
      }
      else
      {
        XPathNavigator navigator = result.GetNavigator(true);
        if (result.Type.IsEnum)
        {
          navigator.SetTypedValue((object) value.ToString());
        }
        else
        {
          try
          {
            navigator.SetTypedValue(value);
          }
          catch (InvalidCastException ex)
          {
            if (!DefaultXmlSerializer.Instance.WriteObject(result, navigator, value) || result.Property == null)
              return;
            dictionaryAdapter.This.ExtendedProperties[(object) result.Property.PropertyName] = value;
          }
        }
      }
    }

    private void WriteComponent(
      XPathResult result,
      object value,
      IDictionaryAdapter dictionaryAdapter)
    {
      if (result.Property != null)
        dictionaryAdapter.This.ExtendedProperties.Remove((object) result.Property.PropertyName);
      if (value == null)
      {
        result.Remove();
      }
      else
      {
        if (!(value is IDictionaryAdapter dictionaryAdapter1))
          return;
        XPathNavigator source = result.RemoveChildren();
        if (result.Type != dictionaryAdapter1.Meta.Type && !result.OmitPolymorphism)
        {
          XmlTypeAttribute xmlType = dictionaryAdapter1.GetXmlMeta().XmlType;
          this.Context.SetXmlType(xmlType.TypeName, xmlType.Namespace, source);
        }
        IDictionaryAdapter other = (IDictionaryAdapter) this.ReadComponent(result, false, dictionaryAdapter);
        dictionaryAdapter1.CopyTo(other);
      }
    }

    private void WriteCollection(
      XPathResult result,
      object value,
      IDictionaryAdapter dictionaryAdapter)
    {
      result.Remove();
      if (value == null)
        return;
      if (result.Type.IsArray)
      {
        this.WriteArray(result, value, dictionaryAdapter);
      }
      else
      {
        if (!result.Type.IsGenericType)
          return;
        this.WriteList(result, value, dictionaryAdapter);
      }
    }

    private void WriteArray(XPathResult result, object value, IDictionaryAdapter dictionaryAdapter)
    {
      Array array = (Array) value;
      Type elementType = array.GetType().GetElementType();
      foreach (object obj in array)
        this.WriteProperty(result.CreateNode(elementType, obj, (Func<Type, XmlMetadata>) (type => dictionaryAdapter.GetXmlMeta(type))), obj, dictionaryAdapter);
    }

    private void WriteList(XPathResult result, object value, IDictionaryAdapter dictionaryAdapter)
    {
      Type genericArgument = result.Type.GetGenericArguments()[0];
      foreach (object obj in (IEnumerable) value)
        this.WriteProperty(result.CreateNode(genericArgument, obj, (Func<Type, XmlMetadata>) (type => dictionaryAdapter.GetXmlMeta(type))), obj, dictionaryAdapter);
    }

    private bool WriteCustom(
      XPathResult result,
      object value,
      IDictionaryAdapter dictionaryAdapter)
    {
      if (!result.WriteObject(value))
        return false;
      if (result.Property != null)
        dictionaryAdapter.This.ExtendedProperties[(object) result.Property.PropertyName] = value;
      return true;
    }

    public static XPathAdapter For(object adapter)
    {
      if (adapter == null)
        throw new ArgumentNullException(nameof (adapter));
      if (adapter is IDictionaryAdapter dictionaryAdapter)
      {
        XPathAdapter xpathAdapter = (XPathAdapter) null;
        ICollection<IDictionaryPropertyGetter> getters = dictionaryAdapter.This.Descriptor.Getters;
        if (getters != null)
          xpathAdapter = getters.OfType<XPathAdapter>().SingleOrDefault<XPathAdapter>();
        if (xpathAdapter != null)
          return xpathAdapter;
      }
      return (XPathAdapter) null;
    }

    private XPathResult EvaluateProperty(
      string key,
      PropertyDescriptor property,
      IDictionaryAdapter dictionaryAdapter)
    {
      XPathExpression xpath = (XPathExpression) null;
      object matchingBehavior = (object) null;
      Func<XPathNavigator> create1 = (Func<XPathNavigator>) null;
      XPathContext keyContext = this.Context.CreateChild((IEnumerable) Enumerable.Repeat<object>((object) dictionaryAdapter.GetXmlMeta(property.Property.DeclaringType).XmlType, 1).Union<object>((IEnumerable<object>) property.Behaviors));
      object result;
      foreach (object behavior in property.Behaviors)
      {
        string name = key;
        string ns = (string) null;
        Func<XPathNavigator> func = (Func<XPathNavigator>) null;
        switch (behavior)
        {
          case XmlElementAttribute _:
            xpath = XPathAdapter.XPathElement;
            XPathNavigator node1 = this.root.Clone();
            XmlElementAttribute elementAttribute = (XmlElementAttribute) behavior;
            if (!string.IsNullOrEmpty(elementAttribute.ElementName))
              name = elementAttribute.ElementName;
            if (!string.IsNullOrEmpty(elementAttribute.Namespace))
              ns = elementAttribute.Namespace;
            func = (Func<XPathNavigator>) (() => keyContext.AppendElement(name, ns, node1));
            break;
          case XmlAttributeAttribute _:
            xpath = XPathAdapter.XPathAttribute;
            XPathNavigator node2 = this.root.Clone();
            XmlAttributeAttribute attributeAttribute = (XmlAttributeAttribute) behavior;
            if (!string.IsNullOrEmpty(attributeAttribute.AttributeName))
              name = attributeAttribute.AttributeName;
            if (!string.IsNullOrEmpty(attributeAttribute.Namespace))
              ns = attributeAttribute.Namespace;
            func = (Func<XPathNavigator>) (() => keyContext.CreateAttribute(name, ns, node2));
            break;
          case XmlArrayAttribute _:
            xpath = XPathAdapter.XPathElement;
            XPathNavigator node3 = this.root.Clone();
            XmlArrayAttribute xmlArrayAttribute = (XmlArrayAttribute) behavior;
            if (!string.IsNullOrEmpty(xmlArrayAttribute.ElementName))
              name = xmlArrayAttribute.ElementName;
            if (!string.IsNullOrEmpty(xmlArrayAttribute.Namespace))
              ns = xmlArrayAttribute.Namespace;
            func = (Func<XPathNavigator>) (() => keyContext.AppendElement(name, ns, node3));
            break;
          case Castle.Components.DictionaryAdapter.XPathAttribute _:
            xpath = ((Castle.Components.DictionaryAdapter.XPathAttribute) behavior).CompiledExpression;
            break;
          default:
            continue;
        }
        if (xpath != null)
        {
          keyContext.Arguments.Clear();
          keyContext.Arguments.AddParam(nameof (key), "", (object) name);
          keyContext.Arguments.AddParam("ns", "", (object) (ns ?? "_"));
          if (keyContext.Evaluate(xpath, this.Root, out result))
          {
            Func<XPathNavigator> create2 = func ?? create1;
            return new XPathResult(property, result, keyContext, behavior, create2);
          }
        }
        matchingBehavior = matchingBehavior ?? behavior;
        create1 = create1 ?? func;
      }
      if (xpath != null)
        return new XPathResult(property, (object) null, keyContext, matchingBehavior, create1);
      keyContext.Arguments.Clear();
      keyContext.Arguments.AddParam(nameof (key), "", (object) key);
      keyContext.Arguments.AddParam("ns", "", (object) "_");
      Func<XPathNavigator> create3 = create1 ?? (Func<XPathNavigator>) (() => keyContext.AppendElement(key, (string) null, this.root));
      keyContext.Evaluate(XPathAdapter.XPathElementOrAttribute, this.Root, out result);
      return new XPathResult(property, result, keyContext, (object) null, create3);
    }

    private static bool ShouldIgnoreProperty(PropertyDescriptor property)
    {
      return ((IEnumerable<object>) property.Behaviors).Any<object>((Func<object, bool>) (behavior => behavior is XmlIgnoreAttribute));
    }

    private XPathNavigator EnsureOffRoot()
    {
      if (this.root == null && this.createRoot != null)
        this.root = this.createRoot().Clone();
      if (this.root != null && !XPathAdapter.MoveOffRoot(this.root, XPathNodeType.Element))
      {
        string namespaceUri = "";
        XmlRootAttribute xmlRoot = this.xmlMeta.XmlRoot;
        string name;
        if (xmlRoot != null)
        {
          name = xmlRoot.ElementName;
          namespaceUri = xmlRoot.Namespace;
        }
        else
          name = this.xmlMeta.XmlType.TypeName;
        this.root = this.Context.AppendElement(name, namespaceUri, this.root);
        this.Context.AddStandardNamespaces(this.root);
      }
      return this.root;
    }

    private static bool MoveOffRoot(XPathNavigator source, XPathNodeType to)
    {
      return source.NodeType != XPathNodeType.Root || source.MoveToChild(to);
    }
  }
}
