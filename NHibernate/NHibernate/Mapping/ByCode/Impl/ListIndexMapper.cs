// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.ListIndexMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using System;
using System.Linq;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class ListIndexMapper : IListIndexMapper
  {
    private const string DefaultIndexColumnName = "ListIdx";
    private readonly HbmListIndex mapping;
    private readonly Type ownerEntityType;

    public ListIndexMapper(Type ownerEntityType, HbmListIndex mapping)
    {
      this.ownerEntityType = ownerEntityType;
      this.mapping = mapping;
    }

    public void Column(string columnName)
    {
      this.Column((Action<IColumnMapper>) (x => x.Name(columnName)));
    }

    public void Column(Action<IColumnMapper> columnMapper)
    {
      HbmColumn hbmColumn1 = this.mapping.Columns.SingleOrDefault<HbmColumn>();
      if (hbmColumn1 == null)
        hbmColumn1 = new HbmColumn()
        {
          name = this.mapping.column1
        };
      HbmColumn hbmColumn2 = hbmColumn1;
      columnMapper((IColumnMapper) new ColumnMapper(hbmColumn2, "ListIdx"));
      if (this.ColumnTagIsRequired(hbmColumn2))
      {
        this.mapping.column = hbmColumn2;
        this.ResetColumnPlainValues();
      }
      else
        this.mapping.column1 = !"ListIdx".Equals(hbmColumn2.name) ? hbmColumn2.name : (string) null;
    }

    private void ResetColumnPlainValues() => this.mapping.column1 = (string) null;

    private bool ColumnTagIsRequired(HbmColumn hbm)
    {
      return hbm.length != null || hbm.precision != null || hbm.scale != null || hbm.notnull || hbm.unique || hbm.uniquekey != null || hbm.sqltype != null || hbm.index != null || hbm.@default != null || hbm.check != null;
    }

    public void Base(int baseIndex)
    {
      this.mapping.@base = baseIndex > 0 ? baseIndex.ToString() : throw new ArgumentOutOfRangeException(nameof (baseIndex), "The baseIndex should be positive value");
    }
  }
}
