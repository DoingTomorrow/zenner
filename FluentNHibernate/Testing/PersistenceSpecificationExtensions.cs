// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Testing.PersistenceSpecificationExtensions
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Testing.Values;
using FluentNHibernate.Utils;
using FluentNHibernate.Utils.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Testing
{
  public static class PersistenceSpecificationExtensions
  {
    public static PersistenceSpecification<T> CheckProperty<T>(
      this PersistenceSpecification<T> spec,
      Expression<Func<T, object>> expression,
      object propertyValue)
    {
      return spec.CheckProperty<T>(expression, propertyValue, (IEqualityComparer) null);
    }

    public static PersistenceSpecification<T> CheckProperty<T>(
      this PersistenceSpecification<T> spec,
      Expression<Func<T, object>> expression,
      object propertyValue,
      IEqualityComparer propertyComparer)
    {
      Accessor accessor = ReflectionHelper.GetAccessor<T>(expression);
      return spec.RegisterCheckedProperty((Property<T>) new Property<T, object>(accessor, propertyValue), propertyComparer);
    }

    public static PersistenceSpecification<T> CheckProperty<T, TListElement>(
      this PersistenceSpecification<T> spec,
      Expression<Func<T, Array>> expression,
      IEnumerable<TListElement> propertyValue)
    {
      return spec.CheckProperty<T, TListElement>(expression, propertyValue, (IEqualityComparer) null);
    }

    public static PersistenceSpecification<T> CheckProperty<T, TListElement>(
      this PersistenceSpecification<T> spec,
      Expression<Func<T, Array>> expression,
      IEnumerable<TListElement> propertyValue,
      IEqualityComparer elementComparer)
    {
      Accessor accessor = ReflectionHelper.GetAccessor<T, Array>(expression);
      return spec.RegisterCheckedProperty((Property<T>) new List<T, TListElement>(accessor, propertyValue), elementComparer);
    }

    public static PersistenceSpecification<T> CheckProperty<T, TProperty>(
      this PersistenceSpecification<T> spec,
      Expression<Func<T, TProperty>> expression,
      TProperty propertyValue,
      Action<T, TProperty> propertySetter)
    {
      return spec.CheckProperty<T, TProperty>(expression, propertyValue, (IEqualityComparer) null, propertySetter);
    }

    public static PersistenceSpecification<T> CheckProperty<T, TProperty>(
      this PersistenceSpecification<T> spec,
      Expression<Func<T, TProperty>> expression,
      TProperty propertyValue,
      IEqualityComparer propertyComparer,
      Action<T, TProperty> propertySetter)
    {
      return spec.RegisterCheckedProperty((Property<T>) new Property<T, TProperty>(ReflectionHelper.GetAccessor<T, TProperty>(expression), propertyValue)
      {
        ValueSetter = (Action<T, Accessor, TProperty>) ((target, propertyInfo, value) => propertySetter(target, value))
      }, propertyComparer);
    }

    public static PersistenceSpecification<T> CheckReference<T>(
      this PersistenceSpecification<T> spec,
      Expression<Func<T, object>> expression,
      object propertyValue)
    {
      return spec.CheckReference<T>(expression, propertyValue, (IEqualityComparer) null);
    }

    public static PersistenceSpecification<T> CheckReference<T>(
      this PersistenceSpecification<T> spec,
      Expression<Func<T, object>> expression,
      object propertyValue,
      IEqualityComparer propertyComparer)
    {
      Accessor accessor = ReflectionHelper.GetAccessor<T>(expression);
      return spec.RegisterCheckedProperty((Property<T>) new ReferenceProperty<T, object>(accessor, propertyValue), propertyComparer);
    }

    public static PersistenceSpecification<T> CheckReference<T, TReference>(
      this PersistenceSpecification<T> spec,
      Expression<Func<T, object>> expression,
      TReference propertyValue,
      params Func<TReference, object>[] propertiesToCompare)
    {
      return propertiesToCompare == null || propertiesToCompare.Length == 0 ? spec.CheckReference<T>(expression, (object) propertyValue, (IEqualityComparer) null) : spec.CheckReference<T>(expression, (object) propertyValue, (IEqualityComparer) new PersistenceSpecificationExtensions.FuncEqualityComparer<TReference>((IEnumerable<Func<TReference, object>>) propertiesToCompare));
    }

    public static PersistenceSpecification<T> CheckReference<T, TProperty>(
      this PersistenceSpecification<T> spec,
      Expression<Func<T, TProperty>> expression,
      TProperty propertyValue,
      Action<T, TProperty> propertySetter)
    {
      return spec.CheckReference<T, TProperty>(expression, propertyValue, (IEqualityComparer) null, propertySetter);
    }

    public static PersistenceSpecification<T> CheckReference<T, TProperty>(
      this PersistenceSpecification<T> spec,
      Expression<Func<T, TProperty>> expression,
      TProperty propertyValue,
      IEqualityComparer propertyComparer,
      Action<T, TProperty> propertySetter)
    {
      ReferenceProperty<T, TProperty> referenceProperty = new ReferenceProperty<T, TProperty>(ReflectionHelper.GetAccessor<T, TProperty>(expression), propertyValue);
      referenceProperty.ValueSetter = (Action<T, Accessor, TProperty>) ((target, propertyInfo, value) => propertySetter(target, value));
      return spec.RegisterCheckedProperty((Property<T>) referenceProperty, propertyComparer);
    }

    public static PersistenceSpecification<T> CheckList<T, TListElement>(
      this PersistenceSpecification<T> spec,
      Expression<Func<T, IEnumerable<TListElement>>> expression,
      IEnumerable<TListElement> propertyValue)
    {
      return spec.CheckList<T, TListElement>(expression, propertyValue, (IEqualityComparer) null);
    }

    public static PersistenceSpecification<T> CheckList<T, TListElement>(
      this PersistenceSpecification<T> spec,
      Expression<Func<T, IEnumerable<TListElement>>> expression,
      IEnumerable<TListElement> propertyValue,
      IEqualityComparer elementComparer)
    {
      Accessor accessor = ReflectionHelper.GetAccessor<T, IEnumerable<TListElement>>(expression);
      return spec.RegisterCheckedProperty((Property<T>) new ReferenceList<T, TListElement>(accessor, propertyValue), elementComparer);
    }

    public static PersistenceSpecification<T> CheckList<T, TListElement>(
      this PersistenceSpecification<T> spec,
      Expression<Func<T, IEnumerable<TListElement>>> expression,
      IEnumerable<TListElement> propertyValue,
      params Func<TListElement, object>[] propertiesToCompare)
    {
      return propertiesToCompare == null || propertiesToCompare.Length == 0 ? spec.CheckList<T, TListElement>(expression, propertyValue, (IEqualityComparer) null) : spec.CheckList<T, TListElement>(expression, propertyValue, (IEqualityComparer) new PersistenceSpecificationExtensions.FuncEqualityComparer<TListElement>((IEnumerable<Func<TListElement, object>>) propertiesToCompare));
    }

    public static PersistenceSpecification<T> CheckList<T, TListElement>(
      this PersistenceSpecification<T> spec,
      Expression<Func<T, IEnumerable<TListElement>>> expression,
      IEnumerable<TListElement> propertyValue,
      Action<T, TListElement> listItemSetter)
    {
      return spec.CheckList<T, TListElement>(expression, propertyValue, (IEqualityComparer) null, listItemSetter);
    }

    public static PersistenceSpecification<T> CheckList<T, TListElement>(
      this PersistenceSpecification<T> spec,
      Expression<Func<T, IEnumerable<TListElement>>> expression,
      IEnumerable<TListElement> propertyValue,
      IEqualityComparer elementComparer,
      Action<T, TListElement> listItemSetter)
    {
      ReferenceList<T, TListElement> referenceList = new ReferenceList<T, TListElement>(ReflectionHelper.GetAccessor<T, IEnumerable<TListElement>>(expression), propertyValue);
      referenceList.ValueSetter = (Action<T, Accessor, IEnumerable<TListElement>>) ((target, propertyInfo, value) =>
      {
        foreach (TListElement listElement in value)
          listItemSetter(target, listElement);
      });
      return spec.RegisterCheckedProperty((Property<T>) referenceList, elementComparer);
    }

    public static PersistenceSpecification<T> CheckList<T, TListElement>(
      this PersistenceSpecification<T> spec,
      Expression<Func<T, IEnumerable<TListElement>>> expression,
      IEnumerable<TListElement> propertyValue,
      Action<T, IEnumerable<TListElement>> listSetter)
    {
      return spec.CheckList<T, TListElement>(expression, propertyValue, (IEqualityComparer) null, listSetter);
    }

    public static PersistenceSpecification<T> CheckList<T, TListElement>(
      this PersistenceSpecification<T> spec,
      Expression<Func<T, IEnumerable<TListElement>>> expression,
      IEnumerable<TListElement> propertyValue,
      IEqualityComparer elementComparer,
      Action<T, IEnumerable<TListElement>> listSetter)
    {
      ReferenceList<T, TListElement> referenceList = new ReferenceList<T, TListElement>(ReflectionHelper.GetAccessor<T, IEnumerable<TListElement>>(expression), propertyValue);
      referenceList.ValueSetter = (Action<T, Accessor, IEnumerable<TListElement>>) ((target, propertyInfo, value) => listSetter(target, value));
      return spec.RegisterCheckedProperty((Property<T>) referenceList, elementComparer);
    }

    public static PersistenceSpecification<T> CheckComponentList<T, TListElement>(
      this PersistenceSpecification<T> spec,
      Expression<Func<T, object>> expression,
      IEnumerable<TListElement> propertyValue)
    {
      return spec.CheckComponentList<T, TListElement>(expression, propertyValue, (IEqualityComparer) null);
    }

    public static PersistenceSpecification<T> CheckComponentList<T, TListElement>(
      this PersistenceSpecification<T> spec,
      Expression<Func<T, object>> expression,
      IEnumerable<TListElement> propertyValue,
      IEqualityComparer elementComparer)
    {
      Accessor accessor = ReflectionHelper.GetAccessor<T>(expression);
      return spec.RegisterCheckedProperty((Property<T>) new List<T, TListElement>(accessor, propertyValue), elementComparer);
    }

    public static PersistenceSpecification<T> CheckComponentList<T, TListElement>(
      this PersistenceSpecification<T> spec,
      Expression<Func<T, IEnumerable<TListElement>>> expression,
      IEnumerable<TListElement> propertyValue,
      Action<T, TListElement> listItemSetter)
    {
      return spec.CheckComponentList<T, TListElement>(expression, propertyValue, (IEqualityComparer) null, listItemSetter);
    }

    public static PersistenceSpecification<T> CheckComponentList<T, TListElement>(
      this PersistenceSpecification<T> spec,
      Expression<Func<T, IEnumerable<TListElement>>> expression,
      IEnumerable<TListElement> propertyValue,
      IEqualityComparer elementComparer,
      Action<T, TListElement> listItemSetter)
    {
      List<T, TListElement> list = new List<T, TListElement>(ReflectionHelper.GetAccessor<T, IEnumerable<TListElement>>(expression), propertyValue);
      list.ValueSetter = (Action<T, Accessor, IEnumerable<TListElement>>) ((target, propertyInfo, value) =>
      {
        foreach (TListElement listElement in value)
          listItemSetter(target, listElement);
      });
      return spec.RegisterCheckedProperty((Property<T>) list, elementComparer);
    }

    public static PersistenceSpecification<T> CheckComponentList<T, TListElement>(
      this PersistenceSpecification<T> spec,
      Expression<Func<T, IEnumerable<TListElement>>> expression,
      IEnumerable<TListElement> propertyValue,
      Action<T, IEnumerable<TListElement>> listSetter)
    {
      return spec.CheckComponentList<T, TListElement>(expression, propertyValue, (IEqualityComparer) null, listSetter);
    }

    public static PersistenceSpecification<T> CheckComponentList<T, TListElement>(
      this PersistenceSpecification<T> spec,
      Expression<Func<T, IEnumerable<TListElement>>> expression,
      IEnumerable<TListElement> propertyValue,
      IEqualityComparer elementComparer,
      Action<T, IEnumerable<TListElement>> listSetter)
    {
      List<T, TListElement> list = new List<T, TListElement>(ReflectionHelper.GetAccessor<T, IEnumerable<TListElement>>(expression), propertyValue);
      list.ValueSetter = (Action<T, Accessor, IEnumerable<TListElement>>) ((target, propertyInfo, value) => listSetter(target, value));
      return spec.RegisterCheckedProperty((Property<T>) list, elementComparer);
    }

    [Obsolete("CheckEnumerable has been replaced with CheckList")]
    public static PersistenceSpecification<T> CheckEnumerable<T, TItem>(
      this PersistenceSpecification<T> spec,
      Expression<Func<T, IEnumerable<TItem>>> expression,
      Action<T, TItem> addAction,
      IEnumerable<TItem> itemsToAdd)
    {
      return spec.CheckList<T, TItem>(expression, itemsToAdd, addAction);
    }

    private class FuncEqualityComparer<T> : EqualityComparer<T>
    {
      private readonly IEnumerable<Func<T, object>> comparisons;

      public FuncEqualityComparer(IEnumerable<Func<T, object>> comparisons)
      {
        this.comparisons = comparisons;
      }

      public override bool Equals(T x, T y)
      {
        return this.comparisons.All<Func<T, object>>((Func<Func<T, object>, bool>) (func => object.Equals(func(x), func(y))));
      }

      public override int GetHashCode(T obj) => throw new NotSupportedException();
    }
  }
}
