// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.ForeignGeneratorDef
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public class ForeignGeneratorDef : IGeneratorDef
  {
    private readonly object param;

    public ForeignGeneratorDef(MemberInfo foreignProperty)
    {
      this.param = foreignProperty != null ? (object) new
      {
        property = foreignProperty.Name
      } : throw new ArgumentNullException(nameof (foreignProperty));
    }

    public string Class => "foreign";

    public object Params => this.param;

    public Type DefaultReturnType => (Type) null;

    public bool SupportedAsCollectionElementId => false;
  }
}
