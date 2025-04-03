// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteIndexConstraintUsage
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

#nullable disable
namespace System.Data.SQLite
{
  public sealed class SQLiteIndexConstraintUsage
  {
    public int argvIndex;
    public byte omit;

    internal SQLiteIndexConstraintUsage()
    {
    }

    internal SQLiteIndexConstraintUsage(
      UnsafeNativeMethods.sqlite3_index_constraint_usage constraintUsage)
      : this(constraintUsage.argvIndex, constraintUsage.omit)
    {
    }

    private SQLiteIndexConstraintUsage(int argvIndex, byte omit)
    {
      this.argvIndex = argvIndex;
      this.omit = omit;
    }
  }
}
