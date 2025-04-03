// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteIndexConstraint
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

#nullable disable
namespace System.Data.SQLite
{
  public sealed class SQLiteIndexConstraint
  {
    public int iColumn;
    public SQLiteIndexConstraintOp op;
    public byte usable;
    public int iTermOffset;

    internal SQLiteIndexConstraint(
      UnsafeNativeMethods.sqlite3_index_constraint constraint)
      : this(constraint.iColumn, constraint.op, constraint.usable, constraint.iTermOffset)
    {
    }

    private SQLiteIndexConstraint(
      int iColumn,
      SQLiteIndexConstraintOp op,
      byte usable,
      int iTermOffset)
    {
      this.iColumn = iColumn;
      this.op = op;
      this.usable = usable;
      this.iTermOffset = iTermOffset;
    }
  }
}
