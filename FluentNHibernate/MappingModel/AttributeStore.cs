// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.AttributeStore
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace FluentNHibernate.MappingModel
{
  [Serializable]
  public class AttributeStore
  {
    private readonly AttributeLayeredValues layeredValues;

    public AttributeStore() => this.layeredValues = new AttributeLayeredValues();

    public object Get(string property)
    {
      LayeredValues layeredValue = this.layeredValues[property];
      if (!layeredValue.Any<KeyValuePair<int, object>>())
        return (object) null;
      int key = layeredValue.Max<KeyValuePair<int, object>>((Func<KeyValuePair<int, object>, int>) (x => x.Key));
      return layeredValue[key];
    }

    public void Set(string attribute, int layer, object value)
    {
      this.layeredValues[attribute][layer] = value;
    }

    public bool IsSpecified(string attribute)
    {
      return this.layeredValues[attribute].Any<KeyValuePair<int, object>>();
    }

    public void CopyTo(AttributeStore theirStore)
    {
      this.layeredValues.CopyTo(theirStore.layeredValues);
    }

    public AttributeStore Clone()
    {
      AttributeStore theirStore = new AttributeStore();
      this.CopyTo(theirStore);
      return theirStore;
    }

    public bool Equals(AttributeStore other)
    {
      return other != null && other.layeredValues.ContentEquals(this.layeredValues);
    }

    public override bool Equals(object obj)
    {
      return obj.GetType() == typeof (AttributeStore) && this.Equals((AttributeStore) obj);
    }

    public override int GetHashCode()
    {
      return (this.layeredValues != null ? this.layeredValues.GetHashCode() : 0) * 397;
    }

    public void Merge(AttributeStore columnAttributes)
    {
      columnAttributes.layeredValues.CopyTo(this.layeredValues);
    }
  }
}
