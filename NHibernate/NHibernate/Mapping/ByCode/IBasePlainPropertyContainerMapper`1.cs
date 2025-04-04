// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.IBasePlainPropertyContainerMapper`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public interface IBasePlainPropertyContainerMapper<TContainer> : 
    IMinimalPlainPropertyContainerMapper<TContainer>
  {
    void Component<TComponent>(
      Expression<Func<TContainer, TComponent>> property,
      Action<IComponentMapper<TComponent>> mapping)
      where TComponent : class;

    void Component<TComponent>(Expression<Func<TContainer, TComponent>> property) where TComponent : class;

    void Component<TComponent>(
      Expression<Func<TContainer, IDictionary>> property,
      TComponent dynamicComponentTemplate,
      Action<IDynamicComponentMapper<TComponent>> mapping)
      where TComponent : class;

    void Component<TComponent>(
      string notVisiblePropertyOrFieldName,
      Action<IComponentMapper<TComponent>> mapping)
      where TComponent : class;

    void Component<TComponent>(string notVisiblePropertyOrFieldName) where TComponent : class;

    void Component<TComponent>(
      string notVisiblePropertyOrFieldName,
      TComponent dynamicComponentTemplate,
      Action<IDynamicComponentMapper<TComponent>> mapping)
      where TComponent : class;

    void Any<TProperty>(
      Expression<Func<TContainer, TProperty>> property,
      Type idTypeOfMetaType,
      Action<IAnyMapper> mapping)
      where TProperty : class;

    void Any<TProperty>(
      string notVisiblePropertyOrFieldName,
      Type idTypeOfMetaType,
      Action<IAnyMapper> mapping)
      where TProperty : class;
  }
}
