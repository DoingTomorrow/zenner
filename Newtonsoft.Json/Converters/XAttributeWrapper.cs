// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XAttributeWrapper
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System.Xml.Linq;

#nullable disable
namespace Newtonsoft.Json.Converters
{
  internal class XAttributeWrapper(XAttribute attribute) : XObjectWrapper((XObject) attribute)
  {
    private XAttribute Attribute => (XAttribute) this.WrappedNode;

    public override string Value
    {
      get => this.Attribute.Value;
      set => this.Attribute.Value = value;
    }

    public override string LocalName => this.Attribute.Name.LocalName;

    public override string NamespaceUri => this.Attribute.Name.NamespaceName;

    public override IXmlNode ParentNode
    {
      get
      {
        return this.Attribute.Parent == null ? (IXmlNode) null : XContainerWrapper.WrapNode((XObject) this.Attribute.Parent);
      }
    }
  }
}
