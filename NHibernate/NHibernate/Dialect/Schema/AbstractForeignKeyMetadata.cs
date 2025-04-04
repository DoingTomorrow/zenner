// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Schema.AbstractForeignKeyMetadata
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections.Generic;
using System.Data;

#nullable disable
namespace NHibernate.Dialect.Schema
{
  public class AbstractForeignKeyMetadata : IForeignKeyMetadata
  {
    private string name;
    private readonly List<IColumnMetadata> columns = new List<IColumnMetadata>();

    public AbstractForeignKeyMetadata(DataRow rs)
    {
    }

    public string Name
    {
      get => this.name;
      protected set => this.name = value;
    }

    public void AddColumn(IColumnMetadata column)
    {
      if (column == null)
        return;
      this.columns.Add(column);
    }

    public IColumnMetadata[] Columns => this.columns.ToArray();

    public override string ToString() => "ForeignKeyMetadata(" + this.name + (object) ')';
  }
}
