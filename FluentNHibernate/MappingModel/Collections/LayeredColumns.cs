// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Collections.LayeredColumns
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace FluentNHibernate.MappingModel.Collections
{
  [Serializable]
  public class LayeredColumns
  {
    private readonly LayeredValues layeredValues = new LayeredValues();

    public IEnumerable<ColumnMapping> Columns
    {
      get
      {
        if (this.layeredValues.Any<KeyValuePair<int, object>>())
        {
          int maxLayer = this.layeredValues.Keys.Max();
          HashSet<ColumnMapping> values = (HashSet<ColumnMapping>) this.layeredValues[maxLayer];
          foreach (ColumnMapping value in values)
            yield return value;
        }
      }
    }

    public void AddColumn(int layer, ColumnMapping mapping)
    {
      if (!this.layeredValues.ContainsKey(layer))
        this.layeredValues[layer] = (object) new HashSet<ColumnMapping>((IEqualityComparer<ColumnMapping>) new LayeredColumns.ColumnMappingComparer());
      ((HashSet<ColumnMapping>) this.layeredValues[layer]).Add(mapping);
    }

    public void MakeColumnsEmpty(int layer)
    {
      this.layeredValues[layer] = (object) new HashSet<ColumnMapping>();
    }

    public bool ContentEquals(LayeredColumns columns)
    {
      return this.layeredValues.ContentEquals<int, object>((IDictionary<int, object>) columns.layeredValues);
    }

    [Serializable]
    private class ColumnMappingComparer : IEqualityComparer<ColumnMapping>
    {
      public bool Equals(ColumnMapping x, ColumnMapping y) => x.Equals(y);

      public int GetHashCode(ColumnMapping obj) => obj.GetHashCode();
    }
  }
}
