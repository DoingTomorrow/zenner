// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XDocumentTypeWrapper
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System.Xml.Linq;

#nullable disable
namespace Newtonsoft.Json.Converters
{
  internal class XDocumentTypeWrapper : XObjectWrapper, IXmlDocumentType, IXmlNode
  {
    private readonly XDocumentType _documentType;

    public XDocumentTypeWrapper(XDocumentType documentType)
      : base((XObject) documentType)
    {
      this._documentType = documentType;
    }

    public string Name => this._documentType.Name;

    public string System => this._documentType.SystemId;

    public string Public => this._documentType.PublicId;

    public string InternalSubset => this._documentType.InternalSubset;

    public override string LocalName => "DOCTYPE";
  }
}
