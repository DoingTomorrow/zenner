// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.IManyToAnyMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public interface IManyToAnyMapper
  {
    void MetaType(IType metaType);

    void MetaType<TMetaType>();

    void MetaType(System.Type metaType);

    void IdType(IType idType);

    void IdType<TIdType>();

    void IdType(System.Type idType);

    void Columns(Action<IColumnMapper> idColumnMapping, Action<IColumnMapper> classColumnMapping);

    void MetaValue(object value, System.Type entityType);
  }
}
