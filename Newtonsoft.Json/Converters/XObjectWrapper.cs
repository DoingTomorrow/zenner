// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XObjectWrapper
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

#nullable disable
namespace Newtonsoft.Json.Converters
{
  internal class XObjectWrapper : IXmlNode
  {
    private static readonly List<IXmlNode> EmptyChildNodes = new List<IXmlNode>();
    private readonly XObject _xmlObject;

    public XObjectWrapper(XObject xmlObject) => this._xmlObject = xmlObject;

    public object WrappedNode => (object) this._xmlObject;

    public virtual XmlNodeType NodeType => this._xmlObject.NodeType;

    public virtual string LocalName => (string) null;

    public virtual List<IXmlNode> ChildNodes => XObjectWrapper.EmptyChildNodes;

    public virtual List<IXmlNode> Attributes => (List<IXmlNode>) null;

    public virtual IXmlNode ParentNode => (IXmlNode) null;

    public virtual string Value
    {
      get => (string) null;
      set => throw new InvalidOperationException();
    }

    public virtual IXmlNode AppendChild(IXmlNode newChild) => throw new InvalidOperationException();

    public virtual string NamespaceUri => (string) null;
  }
}
