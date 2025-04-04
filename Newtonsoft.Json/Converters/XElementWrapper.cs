// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XElementWrapper
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System.Collections.Generic;
using System.Xml.Linq;

#nullable disable
namespace Newtonsoft.Json.Converters
{
  internal class XElementWrapper(XElement element) : XContainerWrapper((XContainer) element), IXmlElement, IXmlNode
  {
    private List<IXmlNode> _attributes;

    private XElement Element => (XElement) this.WrappedNode;

    public void SetAttributeNode(IXmlNode attribute)
    {
      this.Element.Add(((XObjectWrapper) attribute).WrappedNode);
      this._attributes = (List<IXmlNode>) null;
    }

    public override List<IXmlNode> Attributes
    {
      get
      {
        if (this._attributes == null)
        {
          this._attributes = new List<IXmlNode>();
          foreach (XAttribute attribute in this.Element.Attributes())
            this._attributes.Add((IXmlNode) new XAttributeWrapper(attribute));
          string namespaceUri = this.NamespaceUri;
          if (!string.IsNullOrEmpty(namespaceUri) && namespaceUri != this.ParentNode?.NamespaceUri && string.IsNullOrEmpty(this.GetPrefixOfNamespace(namespaceUri)))
          {
            bool flag = false;
            foreach (IXmlNode attribute in this._attributes)
            {
              if (attribute.LocalName == "xmlns" && string.IsNullOrEmpty(attribute.NamespaceUri) && attribute.Value == namespaceUri)
                flag = true;
            }
            if (!flag)
              this._attributes.Insert(0, (IXmlNode) new XAttributeWrapper(new XAttribute((XName) "xmlns", (object) namespaceUri)));
          }
        }
        return this._attributes;
      }
    }

    public override IXmlNode AppendChild(IXmlNode newChild)
    {
      IXmlNode xmlNode = base.AppendChild(newChild);
      this._attributes = (List<IXmlNode>) null;
      return xmlNode;
    }

    public override string Value
    {
      get => this.Element.Value;
      set => this.Element.Value = value;
    }

    public override string LocalName => this.Element.Name.LocalName;

    public override string NamespaceUri => this.Element.Name.NamespaceName;

    public string GetPrefixOfNamespace(string namespaceUri)
    {
      return this.Element.GetPrefixOfNamespace((XNamespace) namespaceUri);
    }

    public bool IsEmpty => this.Element.IsEmpty;
  }
}
