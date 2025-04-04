// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.Query.Sql.NativeSQLQueryScalarReturn
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Engine.Query.Sql
{
  [Serializable]
  public class NativeSQLQueryScalarReturn : INativeSQLQueryReturn
  {
    private readonly string columnAlias;
    private readonly IType type;

    public NativeSQLQueryScalarReturn(string alias, IType type)
    {
      this.columnAlias = !string.IsNullOrEmpty(alias) ? alias : throw new ArgumentNullException(nameof (alias), "A valid scalar alias must be specified.");
      this.type = type;
    }

    public string ColumnAlias => this.columnAlias;

    public IType Type => this.type;

    public override bool Equals(object obj) => this.Equals(obj as NativeSQLQueryScalarReturn);

    public bool Equals(NativeSQLQueryScalarReturn other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      return object.ReferenceEquals((object) this, (object) other) || object.Equals((object) other.columnAlias, (object) this.columnAlias);
    }

    public override int GetHashCode() => this.columnAlias.GetHashCode();
  }
}
