// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XCommentWrapper
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System.Xml.Linq;

#nullable disable
namespace Newtonsoft.Json.Converters
{
  internal class XCommentWrapper(XComment text) : XObjectWrapper((XObject) text)
  {
    private XComment Text => (XComment) this.WrappedNode;

    public override string Value
    {
      get => this.Text.Value;
      set => this.Text.Value = value;
    }

    public override IXmlNode ParentNode
    {
      get
      {
        return this.Text.Parent == null ? (IXmlNode) null : XContainerWrapper.WrapNode((XObject) this.Text.Parent);
      }
    }
  }
}
