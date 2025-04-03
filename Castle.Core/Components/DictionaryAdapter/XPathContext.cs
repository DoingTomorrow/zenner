// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.XPathContext
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
using System.Xml.Xsl;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public class XPathContext : XsltContext
  {
    public const string Prefix = "castle-da";
    public const string NamespaceUri = "urn:castleproject.org:da";
    public const string IgnoreNamespace = "_";
    private const string Xsd = "http://www.w3.org/2001/XMLSchema";
    private const string Xsi = "http://www.w3.org/2001/XMLSchema-instance";
    private readonly XPathContext parent;
    private IDictionary<string, Func<IXsltContextFunction>> functions;
    private List<IXPathSerializer> serializers;
    private int prefixCount;

    public XPathContext()
      : this(new System.Xml.NameTable())
    {
    }

    public XPathContext(System.Xml.NameTable nameTable)
      : base(nameTable)
    {
      this.Arguments = new XsltArgumentList();
      this.functions = (IDictionary<string, Func<IXsltContextFunction>>) new Dictionary<string, Func<IXsltContextFunction>>();
      this.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
      this.AddNamespace("xsd", "http://www.w3.org/2001/XMLSchema");
      this.AddNamespace("castle-da", "urn:castleproject.org:da");
      this.AddFunction("castle-da", "match", (IXsltContextFunction) XPathContext.MatchFunction.Instance);
    }

    public XPathContext(XPathContext parent)
      : base((System.Xml.NameTable) parent.NameTable)
    {
      this.parent = parent;
      this.Arguments = new XsltArgumentList();
      this.functions = (IDictionary<string, Func<IXsltContextFunction>>) new Dictionary<string, Func<IXsltContextFunction>>();
    }

    public override string DefaultNamespace
    {
      get
      {
        string defaultNamespace = base.DefaultNamespace;
        if (string.IsNullOrEmpty(defaultNamespace) && this.parent != null)
          defaultNamespace = this.parent.DefaultNamespace;
        return defaultNamespace;
      }
    }

    public XsltArgumentList Arguments { get; private set; }

    public IEnumerable<XmlArrayItemAttribute> ListItemMeta { get; private set; }

    public IEnumerable<IXPathSerializer> Serializers
    {
      get
      {
        return ((IEnumerable<IXPathSerializer>) this.serializers ?? Enumerable.Empty<IXPathSerializer>()).Union<IXPathSerializer>(this.parent != null ? this.parent.Serializers : Enumerable.Empty<IXPathSerializer>());
      }
    }

    public XPathContext ApplyBehaviors(IEnumerable behaviors)
    {
      new BehaviorVisitor().OfType<XmlTypeAttribute>((Action<XmlTypeAttribute>) (attrib =>
      {
        if (string.IsNullOrEmpty(attrib.Namespace))
          return;
        this.AddNamespace(string.Empty, attrib.Namespace);
      })).OfType<XmlNamespaceAttribute>((Action<XmlNamespaceAttribute>) (attrib =>
      {
        this.AddNamespace(attrib.Prefix, attrib.NamespaceUri);
        if (!attrib.Default)
          return;
        this.AddNamespace(string.Empty, attrib.NamespaceUri);
      })).OfType<XmlArrayItemAttribute>((Action<XmlArrayItemAttribute>) (attrib =>
      {
        this.ListItemMeta = this.ListItemMeta ?? (IEnumerable<XmlArrayItemAttribute>) new List<XmlArrayItemAttribute>();
        ((List<XmlArrayItemAttribute>) this.ListItemMeta).Add(attrib);
      })).OfType<XPathFunctionAttribute>((Action<XPathFunctionAttribute>) (attrib => this.AddFunction(attrib.Prefix, attrib.Name, attrib.Function))).OfType<IXPathSerializer>((Action<IXPathSerializer>) (attrib => this.AddSerializer(attrib))).Apply(behaviors);
      return this;
    }

    public XPathContext CreateChild(IEnumerable behaviors)
    {
      return new XPathContext(this).ApplyBehaviors(behaviors);
    }

    public XPathContext CreateChild(params object[] behaviors)
    {
      return this.CreateChild((IEnumerable) behaviors);
    }

    public override bool HasNamespace(string prefix)
    {
      if (base.HasNamespace(prefix))
        return true;
      return this.parent != null && this.parent.HasNamespace(prefix);
    }

    public override string LookupNamespace(string prefix)
    {
      string str = base.LookupNamespace(prefix);
      if (str == null && this.parent != null)
        str = this.parent.LookupNamespace(prefix);
      return str;
    }

    public override string LookupPrefix(string uri)
    {
      string str = base.LookupPrefix(uri);
      if (string.IsNullOrEmpty(str) && this.parent != null)
        str = this.parent.LookupPrefix(uri);
      return str;
    }

    public string AddNamespace(string namespaceUri)
    {
      string prefix = this.LookupPrefix(namespaceUri);
      if (string.IsNullOrEmpty(prefix))
      {
        prefix = this.GetUniquePrefix();
        this.AddNamespace(prefix, namespaceUri);
      }
      return prefix;
    }

    public XPathContext AddSerializer(IXPathSerializer serializer)
    {
      this.serializers = this.serializers ?? new List<IXPathSerializer>();
      this.serializers.Insert(0, serializer);
      return this;
    }

    public XPathContext AddFunction(string prefix, string name, IXsltContextFunction function)
    {
      this.functions[XPathContext.GetQualifiedName(prefix, name)] = (Func<IXsltContextFunction>) (() => function);
      return this;
    }

    public XPathContext AddFunction(
      string prefix,
      string name,
      Func<IXsltContextFunction> function)
    {
      this.functions[XPathContext.GetQualifiedName(prefix, name)] = function;
      return this;
    }

    public override IXsltContextFunction ResolveFunction(
      string prefix,
      string name,
      XPathResultType[] argTypes)
    {
      Func<IXsltContextFunction> func;
      if (this.functions.TryGetValue(XPathContext.GetQualifiedName(prefix, name), out func))
        return func();
      return this.parent == null ? (IXsltContextFunction) null : this.parent.ResolveFunction(prefix, name, argTypes);
    }

    public override IXsltContextVariable ResolveVariable(string prefix, string name)
    {
      return (IXsltContextVariable) new XPathContext.XPathVariable(name);
    }

    public bool Evaluate(XPathExpression xpath, XPathNavigator source, out object result)
    {
      xpath = xpath.Clone();
      xpath.SetContext((XmlNamespaceManager) this);
      result = source.Evaluate(xpath);
      if (xpath.ReturnType == XPathResultType.NodeSet && ((XPathNodeIterator) result).Count == 0)
        result = (object) null;
      return result != null;
    }

    public XPathNavigator SelectSingleNode(XPathExpression xpath, XPathNavigator source)
    {
      xpath = xpath.Clone();
      xpath.SetContext((XmlNamespaceManager) this);
      return source.SelectSingleNode(xpath);
    }

    public bool Matches(XPathExpression xpath, XPathNavigator source)
    {
      xpath = xpath.Clone();
      xpath.SetContext((XmlNamespaceManager) this);
      return source.Matches(xpath);
    }

    public void AddStandardNamespaces(XPathNavigator source)
    {
      this.CreateNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance", source);
      this.CreateNamespace("xsd", "http://www.w3.org/2001/XMLSchema", source);
    }

    public string CreateNamespace(string prefix, string namespaceUri, XPathNavigator source)
    {
      if (!string.IsNullOrEmpty(namespaceUri))
      {
        source = source.Clone();
        source.MoveToRoot();
        source.MoveToChild(XPathNodeType.Element);
        if (string.IsNullOrEmpty(prefix))
          prefix = this.AddNamespace(namespaceUri);
        string str = source.GetNamespace(prefix);
        if (str == namespaceUri)
          return prefix;
        if (!string.IsNullOrEmpty(str))
          return (string) null;
        source.CreateAttribute("xmlns", prefix, "", namespaceUri);
      }
      return prefix;
    }

    public XPathNavigator CreateAttribute(string name, string namespaceUri, XPathNavigator source)
    {
      source.CreateAttribute((string) null, name, namespaceUri, "");
      source.MoveToAttribute(name, namespaceUri ?? "");
      return source;
    }

    public XPathNavigator AppendElement(string name, string namespaceUri, XPathNavigator source)
    {
      namespaceUri = this.GetEffectiveNamespace(namespaceUri);
      source.AppendChildElement(this.LookupPrefix(namespaceUri), name, namespaceUri, "");
      return source.SelectSingleNode("*[position()=last()]");
    }

    public void SetXmlType(string name, string namespaceUri, XPathNavigator source)
    {
      namespaceUri = this.GetEffectiveNamespace(namespaceUri);
      string prefix = this.CreateNamespace((string) null, namespaceUri, source);
      source.CreateAttribute("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance", XPathContext.GetQualifiedName(prefix, name));
    }

    public XmlQualifiedName GetXmlType(XPathNavigator source)
    {
      string attribute = source.GetAttribute("type", "http://www.w3.org/2001/XMLSchema-instance");
      if (string.IsNullOrEmpty(attribute))
        return (XmlQualifiedName) null;
      string ns = (string) null;
      string name1;
      string name2 = XPathContext.SplitQualifiedName(attribute, out name1);
      if (name2 != null)
        ns = source.GetNamespace(name2);
      return new XmlQualifiedName(name1, ns);
    }

    public string GetEffectiveNamespace(string namespaceUri)
    {
      return namespaceUri ?? this.DefaultNamespace;
    }

    public override int CompareDocument(string baseUri, string nextbaseUri) => 0;

    public override bool Whitespace => true;

    public override bool PreserveWhitespace(XPathNavigator node) => true;

    private string GetUniquePrefix()
    {
      return this.parent != null ? this.parent.GetUniquePrefix() : "da" + (object) ++this.prefixCount;
    }

    private static string GetQualifiedName(string prefix, string name)
    {
      return string.IsNullOrEmpty(prefix) ? name : string.Format("{0}:{1}", (object) prefix, (object) name);
    }

    private static string SplitQualifiedName(string qualifiedName, out string name)
    {
      string[] strArray = qualifiedName.Split(':');
      if (strArray.Length == 1)
      {
        name = strArray[0];
        return (string) null;
      }
      name = strArray.Length == 2 ? strArray[1] : throw new ArgumentException(string.Format("Invalid qualified name {0}.  Expected [prefix:]name format", (object) qualifiedName));
      return strArray[0];
    }

    public class XPathVariable : IXsltContextVariable
    {
      private readonly string name;

      public XPathVariable(string name) => this.name = name;

      public bool IsLocal => false;

      public bool IsParam => false;

      public XPathResultType VariableType => XPathResultType.Any;

      public object Evaluate(XsltContext xsltContext)
      {
        return ((XPathContext) xsltContext).Arguments.GetParam(this.name, (string) null);
      }
    }

    public class MatchFunction : IXsltContextFunction
    {
      public static readonly XPathContext.MatchFunction Instance = new XPathContext.MatchFunction();

      protected MatchFunction()
      {
      }

      public int Minargs => 1;

      public int Maxargs => 2;

      public XPathResultType[] ArgTypes
      {
        get
        {
          return new XPathResultType[2]
          {
            XPathResultType.String,
            XPathResultType.String
          };
        }
      }

      public XPathResultType ReturnType => XPathResultType.Boolean;

      public object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
      {
        if (!((string) args[0]).Equals(docContext.LocalName, StringComparison.OrdinalIgnoreCase))
          return (object) false;
        return args.Length > 1 && !args[1].Equals((object) "_") && !((string) args[1]).Equals(docContext.NamespaceURI, StringComparison.OrdinalIgnoreCase) ? (object) false : (object) true;
      }
    }
  }
}
