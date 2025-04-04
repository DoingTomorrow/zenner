// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.IKeyMapper`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public interface IKeyMapper<TEntity> : IColumnsMapper where TEntity : class
  {
    void OnDelete(OnDeleteAction deleteAction);

    void PropertyRef<TProperty>(
      Expression<Func<TEntity, TProperty>> propertyGetter);

    void Update(bool consideredInUpdateQuery);

    void ForeignKey(string foreignKeyName);

    void NotNullable(bool notnull);

    void Unique(bool unique);
  }
}
