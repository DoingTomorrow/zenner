// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.AbstractDictionaryAdapterVisitor
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public abstract class AbstractDictionaryAdapterVisitor : IDictionaryAdapterVisitor
  {
    public virtual void VisitDictionaryAdapter(IDictionaryAdapter dictionaryAdapter)
    {
      foreach (PropertyDescriptor property in (IEnumerable<PropertyDescriptor>) dictionaryAdapter.This.Properties.Values)
      {
        Type collectionItemType;
        if (AbstractDictionaryAdapterVisitor.IsCollection(property, out collectionItemType))
          this.VisitCollection(dictionaryAdapter, property, collectionItemType);
        else if (property.PropertyType.IsInterface)
          this.VisitInterface(dictionaryAdapter, property);
        else
          this.VisitProperty(dictionaryAdapter, property);
      }
    }

    void IDictionaryAdapterVisitor.VisitProperty(
      IDictionaryAdapter dictionaryAdapter,
      PropertyDescriptor property)
    {
      this.VisitProperty(dictionaryAdapter, property);
    }

    protected virtual void VisitProperty(
      IDictionaryAdapter dictionaryAdapter,
      PropertyDescriptor property)
    {
    }

    void IDictionaryAdapterVisitor.VisitInterface(
      IDictionaryAdapter dictionaryAdapter,
      PropertyDescriptor property)
    {
      this.VisitInterface(dictionaryAdapter, property);
    }

    protected virtual void VisitInterface(
      IDictionaryAdapter dictionaryAdapter,
      PropertyDescriptor property)
    {
    }

    void IDictionaryAdapterVisitor.VisitCollection(
      IDictionaryAdapter dictionaryAdapter,
      PropertyDescriptor property,
      Type collectionItemType)
    {
      this.VisitCollection(dictionaryAdapter, property, collectionItemType);
    }

    protected virtual void VisitCollection(
      IDictionaryAdapter dictionaryAdapter,
      PropertyDescriptor property,
      Type collectionItemType)
    {
      this.VisitProperty(dictionaryAdapter, property);
    }

    private static bool IsCollection(PropertyDescriptor property, out Type collectionItemType)
    {
      collectionItemType = (Type) null;
      Type propertyType = property.PropertyType;
      if (propertyType == typeof (string) || !typeof (IEnumerable).IsAssignableFrom(propertyType))
        return false;
      if (propertyType.IsArray)
        collectionItemType = propertyType.GetElementType();
      else if (propertyType.IsGenericType)
      {
        Type[] genericArguments = propertyType.GetGenericArguments();
        collectionItemType = genericArguments[0];
      }
      else
        collectionItemType = typeof (object);
      return true;
    }
  }
}
