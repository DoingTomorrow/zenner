// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XContainerWrapper
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System.Collections.Generic;
using System.Xml.Linq;

#nullable disable
namespace Newtonsoft.Json.Converters
{
  internal class XContainerWrapper(XContainer container) : XObjectWrapper((XObject) container)
  {
    private List<IXmlNode> _childNodes;

    private XContainer Container => (XContainer) this.WrappedNode;

    public override List<IXmlNode> ChildNodes
    {
      get
      {
        if (this._childNodes == null)
        {
          this._childNodes = new List<IXmlNode>();
          foreach (XObject node in this.Container.Nodes())
            this._childNodes.Add(XContainerWrapper.WrapNode(node));
        }
        return this._childNodes;
      }
    }

    public override IXmlNode ParentNode
    {
      get
      {
        return this.Container.Parent == null ? (IXmlNode) null : XContainerWrapper.WrapNode((XObject) this.Container.Parent);
      }
    }

    internal static IXmlNode WrapNode(XObject node)
    {
      switch (node)
      {
        case XDocument _:
          return (IXmlNode) new XDocumentWrapper((XDocument) node);
        case XElement _:
          return (IXmlNode) new XElementWrapper((XElement) node);
        case XContainer _:
          return (IXmlNode) new XContainerWrapper((XContainer) node);
        case XProcessingInstruction _:
          return (IXmlNode) new XProcessingInstructionWrapper((XProcessingInstruction) node);
        case XText _:
          return (IXmlNode) new XTextWrapper((XText) node);
        case XComment _:
          return (IXmlNode) new XCommentWrapper((XComment) node);
        case XAttribute _:
          return (IXmlNode) new XAttributeWrapper((XAttribute) node);
        case XDocumentType _:
          return (IXmlNode) new XDocumentTypeWrapper((XDocumentType) node);
        default:
          return (IXmlNode) new XObjectWrapper(node);
      }
    }

    public override IXmlNode AppendChild(IXmlNode newChild)
    {
      this.Container.Add(newChild.WrappedNode);
      this._childNodes = (List<IXmlNode>) null;
      return newChild;
    }
  }
}
