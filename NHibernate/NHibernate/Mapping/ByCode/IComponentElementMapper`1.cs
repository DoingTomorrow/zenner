// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.IComponentElementMapper`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public interface IComponentElementMapper<TComponent> : 
    IComponentAttributesMapper<TComponent>,
    IEntityPropertyMapper,
    IAccessorPropertyMapper
  {
    void Property<TProperty>(
      Expression<Func<TComponent, TProperty>> property,
      Action<IPropertyMapper> mapping);

    void Property<TProperty>(Expression<Func<TComponent, TProperty>> property);

    void Component<TNestedComponent>(
      Expression<Func<TComponent, TNestedComponent>> property,
      Action<IComponentElementMapper<TNestedComponent>> mapping)
      where TNestedComponent : class;

    void ManyToOne<TProperty>(
      Expression<Func<TComponent, TProperty>> property,
      Action<IManyToOneMapper> mapping)
      where TProperty : class;

    void ManyToOne<TProperty>(Expression<Func<TComponent, TProperty>> property) where TProperty : class;
  }
}
