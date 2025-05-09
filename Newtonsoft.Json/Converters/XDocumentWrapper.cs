﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XDocumentWrapper
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

#nullable disable
namespace Newtonsoft.Json.Converters
{
  internal class XDocumentWrapper(XDocument document) : XContainerWrapper((XContainer) document), IXmlDocument, IXmlNode
  {
    private XDocument Document => (XDocument) this.WrappedNode;

    public override List<IXmlNode> ChildNodes
    {
      get
      {
        List<IXmlNode> childNodes = base.ChildNodes;
        if (this.Document.Declaration != null && childNodes[0].NodeType != XmlNodeType.XmlDeclaration)
          childNodes.Insert(0, (IXmlNode) new XDeclarationWrapper(this.Document.Declaration));
        return childNodes;
      }
    }

    public IXmlNode CreateComment(string text)
    {
      return (IXmlNode) new XObjectWrapper((XObject) new XComment(text));
    }

    public IXmlNode CreateTextNode(string text)
    {
      return (IXmlNode) new XObjectWrapper((XObject) new XText(text));
    }

    public IXmlNode CreateCDataSection(string data)
    {
      return (IXmlNode) new XObjectWrapper((XObject) new XCData(data));
    }

    public IXmlNode CreateWhitespace(string text)
    {
      return (IXmlNode) new XObjectWrapper((XObject) new XText(text));
    }

    public IXmlNode CreateSignificantWhitespace(string text)
    {
      return (IXmlNode) new XObjectWrapper((XObject) new XText(text));
    }

    public IXmlNode CreateXmlDeclaration(string version, string encoding, string standalone)
    {
      return (IXmlNode) new XDeclarationWrapper(new XDeclaration(version, encoding, standalone));
    }

    public IXmlNode CreateXmlDocumentType(
      string name,
      string publicId,
      string systemId,
      string internalSubset)
    {
      return (IXmlNode) new XDocumentTypeWrapper(new XDocumentType(name, publicId, systemId, internalSubset));
    }

    public IXmlNode CreateProcessingInstruction(string target, string data)
    {
      return (IXmlNode) new XProcessingInstructionWrapper(new XProcessingInstruction(target, data));
    }

    public IXmlElement CreateElement(string elementName)
    {
      return (IXmlElement) new XElementWrapper(new XElement((XName) elementName));
    }

    public IXmlElement CreateElement(string qualifiedName, string namespaceUri)
    {
      return (IXmlElement) new XElementWrapper(new XElement(XName.Get(MiscellaneousUtils.GetLocalName(qualifiedName), namespaceUri)));
    }

    public IXmlNode CreateAttribute(string name, string value)
    {
      return (IXmlNode) new XAttributeWrapper(new XAttribute((XName) name, (object) value));
    }

    public IXmlNode CreateAttribute(string qualifiedName, string namespaceUri, string value)
    {
      return (IXmlNode) new XAttributeWrapper(new XAttribute(XName.Get(MiscellaneousUtils.GetLocalName(qualifiedName), namespaceUri), (object) value));
    }

    public IXmlElement DocumentElement
    {
      get
      {
        return this.Document.Root == null ? (IXmlElement) null : (IXmlElement) new XElementWrapper(this.Document.Root);
      }
    }

    public override IXmlNode AppendChild(IXmlNode newChild)
    {
      if (!(newChild is XDeclarationWrapper xdeclarationWrapper))
        return base.AppendChild(newChild);
      this.Document.Declaration = xdeclarationWrapper.Declaration;
      return (IXmlNode) xdeclarationWrapper;
    }
  }
}
