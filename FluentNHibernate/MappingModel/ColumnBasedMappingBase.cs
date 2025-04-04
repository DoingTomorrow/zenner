// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.ColumnBasedMappingBase
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.Collections;
using System;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.MappingModel
{
  [Serializable]
  public abstract class ColumnBasedMappingBase : MappingBase, IHasColumnMappings
  {
    private readonly LayeredColumns columns = new LayeredColumns();
    protected readonly AttributeStore attributes;

    protected ColumnBasedMappingBase(AttributeStore underlyingStore)
    {
      this.attributes = underlyingStore.Clone();
    }

    public IEnumerable<ColumnMapping> Columns => this.columns.Columns;

    public void AddColumn(int layer, ColumnMapping mapping)
    {
      this.columns.AddColumn(layer, mapping);
    }

    public void MakeColumnsEmpty(int layer) => this.columns.MakeColumnsEmpty(layer);

    public bool Equals(ColumnBasedMappingBase other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return other.columns.ContentEquals(this.columns) && object.Equals((object) other.attributes, (object) this.attributes);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return obj.GetType() == typeof (ColumnBasedMappingBase) && this.Equals((ColumnBasedMappingBase) obj);
    }

    public override int GetHashCode()
    {
      return (this.columns != null ? this.columns.GetHashCode() : 0) * 397 ^ (this.attributes != null ? this.attributes.GetHashCode() : 0);
    }
  }
}
