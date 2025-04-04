// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.IGeneratorDef
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public interface IGeneratorDef
  {
    string Class { get; }

    object Params { get; }

    Type DefaultReturnType { get; }

    bool SupportedAsCollectionElementId { get; }
  }
}
