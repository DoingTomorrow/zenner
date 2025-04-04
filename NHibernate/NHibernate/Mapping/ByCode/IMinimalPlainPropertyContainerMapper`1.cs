// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.IMinimalPlainPropertyContainerMapper`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public interface IMinimalPlainPropertyContainerMapper<TContainer>
  {
    void Property<TProperty>(Expression<Func<TContainer, TProperty>> property);

    void Property<TProperty>(
      Expression<Func<TContainer, TProperty>> property,
      Action<IPropertyMapper> mapping);

    void Property(string notVisiblePropertyOrFieldName, Action<IPropertyMapper> mapping);

    void ManyToOne<TProperty>(
      Expression<Func<TContainer, TProperty>> property,
      Action<IManyToOneMapper> mapping)
      where TProperty : class;

    void ManyToOne<TProperty>(Expression<Func<TContainer, TProperty>> property) where TProperty : class;

    void ManyToOne<TProperty>(
      string notVisiblePropertyOrFieldName,
      Action<IManyToOneMapper> mapping)
      where TProperty : class;
  }
}
