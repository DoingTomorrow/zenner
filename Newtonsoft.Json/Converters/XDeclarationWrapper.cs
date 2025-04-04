// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XDeclarationWrapper
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System.Xml;
using System.Xml.Linq;

#nullable disable
namespace Newtonsoft.Json.Converters
{
  internal class XDeclarationWrapper : XObjectWrapper, IXmlDeclaration, IXmlNode
  {
    internal XDeclaration Declaration { get; private set; }

    public XDeclarationWrapper(XDeclaration declaration)
      : base((XObject) null)
    {
      this.Declaration = declaration;
    }

    public override XmlNodeType NodeType => XmlNodeType.XmlDeclaration;

    public string Version => this.Declaration.Version;

    public string Encoding
    {
      get => this.Declaration.Encoding;
      set => this.Declaration.Encoding = value;
    }

    public string Standalone
    {
      get => this.Declaration.Standalone;
      set => this.Declaration.Standalone = value;
    }
  }
}
