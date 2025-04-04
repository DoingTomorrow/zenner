// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XmlElementWrapper
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System.Xml;

#nullable disable
namespace Newtonsoft.Json.Converters
{
  internal class XmlElementWrapper : XmlNodeWrapper, IXmlElement, IXmlNode
  {
    private readonly XmlElement _element;

    public XmlElementWrapper(XmlElement element)
      : base((XmlNode) element)
    {
      this._element = element;
    }

    public void SetAttributeNode(IXmlNode attribute)
    {
      this._element.SetAttributeNode((XmlAttribute) ((XmlNodeWrapper) attribute).WrappedNode);
    }

    public string GetPrefixOfNamespace(string namespaceUri)
    {
      return this._element.GetPrefixOfNamespace(namespaceUri);
    }

    public bool IsEmpty => this._element.IsEmpty;
  }
}
