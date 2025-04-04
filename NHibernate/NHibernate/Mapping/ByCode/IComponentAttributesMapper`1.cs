// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.IComponentAttributesMapper`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public interface IComponentAttributesMapper<TComponent> : 
    IEntityPropertyMapper,
    IAccessorPropertyMapper
  {
    void Parent<TProperty>(Expression<Func<TComponent, TProperty>> parent) where TProperty : class;

    void Parent<TProperty>(
      Expression<Func<TComponent, TProperty>> parent,
      Action<IComponentParentMapper> parentMapping)
      where TProperty : class;

    void Update(bool consideredInUpdateQuery);

    void Insert(bool consideredInInsertQuery);

    void Lazy(bool isLazy);

    void Unique(bool unique);

    void Class<TConcrete>() where TConcrete : TComponent;
  }
}
