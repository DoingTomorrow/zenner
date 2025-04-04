// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XmlNodeWrapper
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Newtonsoft.Json.Converters
{
  internal class XmlNodeWrapper : IXmlNode
  {
    private readonly XmlNode _node;
    private List<IXmlNode> _childNodes;
    private List<IXmlNode> _attributes;

    public XmlNodeWrapper(XmlNode node) => this._node = node;

    public object WrappedNode => (object) this._node;

    public XmlNodeType NodeType => this._node.NodeType;

    public virtual string LocalName => this._node.LocalName;

    public List<IXmlNode> ChildNodes
    {
      get
      {
        if (this._childNodes == null)
        {
          this._childNodes = new List<IXmlNode>(this._node.ChildNodes.Count);
          foreach (XmlNode childNode in this._node.ChildNodes)
            this._childNodes.Add(XmlNodeWrapper.WrapNode(childNode));
        }
        return this._childNodes;
      }
    }

    internal static IXmlNode WrapNode(XmlNode node)
    {
      switch (node.NodeType)
      {
        case XmlNodeType.Element:
          return (IXmlNode) new XmlElementWrapper((XmlElement) node);
        case XmlNodeType.DocumentType:
          return (IXmlNode) new XmlDocumentTypeWrapper((XmlDocumentType) node);
        case XmlNodeType.XmlDeclaration:
          return (IXmlNode) new XmlDeclarationWrapper((XmlDeclaration) node);
        default:
          return (IXmlNode) new XmlNodeWrapper(node);
      }
    }

    public List<IXmlNode> Attributes
    {
      get
      {
        if (this._node.Attributes == null)
          return (List<IXmlNode>) null;
        if (this._attributes == null)
        {
          this._attributes = new List<IXmlNode>(this._node.Attributes.Count);
          foreach (XmlNode attribute in (XmlNamedNodeMap) this._node.Attributes)
            this._attributes.Add(XmlNodeWrapper.WrapNode(attribute));
        }
        return this._attributes;
      }
    }

    public IXmlNode ParentNode
    {
      get
      {
        XmlNode node = this._node is XmlAttribute ? (XmlNode) ((XmlAttribute) this._node).OwnerElement : this._node.ParentNode;
        return node == null ? (IXmlNode) null : XmlNodeWrapper.WrapNode(node);
      }
    }

    public string Value
    {
      get => this._node.Value;
      set => this._node.Value = value;
    }

    public IXmlNode AppendChild(IXmlNode newChild)
    {
      this._node.AppendChild(((XmlNodeWrapper) newChild)._node);
      this._childNodes = (List<IXmlNode>) null;
      this._attributes = (List<IXmlNode>) null;
      return newChild;
    }

    public string NamespaceUri => this._node.NamespaceURI;
  }
}
