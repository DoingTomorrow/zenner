// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.DenormalizedTable
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public class DenormalizedTable : Table
  {
    private readonly Table includedTable;

    public DenormalizedTable(Table includedTable)
    {
      this.includedTable = includedTable;
      includedTable.SetHasDenormalizedTables();
    }

    public override IEnumerable<Column> ColumnIterator
    {
      get
      {
        return (IEnumerable<Column>) new JoinedEnumerable<Column>(this.includedTable.ColumnIterator, base.ColumnIterator);
      }
    }

    public override IEnumerable<UniqueKey> UniqueKeyIterator
    {
      get
      {
        Dictionary<string, UniqueKey> to = new Dictionary<string, UniqueKey>();
        ArrayHelper.AddAll<string, UniqueKey>((IDictionary<string, UniqueKey>) to, this.UniqueKeys);
        ArrayHelper.AddAll<string, UniqueKey>((IDictionary<string, UniqueKey>) to, this.includedTable.UniqueKeys);
        return (IEnumerable<UniqueKey>) to.Values;
      }
    }

    public override IEnumerable<Index> IndexIterator
    {
      get
      {
        List<Index> first = new List<Index>();
        foreach (Index index1 in this.includedTable.IndexIterator)
        {
          Index index2 = new Index();
          index2.Name = this.Name + index1.Name;
          index2.Table = (Table) this;
          index2.AddColumns(index1.ColumnIterator);
          first.Add(index2);
        }
        return (IEnumerable<Index>) new JoinedEnumerable<Index>((IEnumerable<Index>) first, base.IndexIterator);
      }
    }

    public override void CreateForeignKeys()
    {
      this.includedTable.CreateForeignKeys();
      foreach (ForeignKey fk in (IEnumerable) this.includedTable.ForeignKeyIterator)
        this.CreateForeignKey(this.GetForeignKeyName(fk), (IEnumerable<Column>) fk.Columns, fk.ReferencedEntityName);
    }

    private string GetForeignKeyName(ForeignKey fk)
    {
      return string.Format("KF{0}", (object) (fk.Name.GetHashCode() ^ this.Name.GetHashCode()).ToString("X"));
    }

    public override Column GetColumn(Column column)
    {
      return base.GetColumn(column) ?? this.includedTable.GetColumn(column);
    }

    public override bool ContainsColumn(Column column)
    {
      return base.ContainsColumn(column) || this.includedTable.ContainsColumn(column);
    }

    public override PrimaryKey PrimaryKey
    {
      get => this.includedTable.PrimaryKey;
      set => base.PrimaryKey = value;
    }
  }
}
