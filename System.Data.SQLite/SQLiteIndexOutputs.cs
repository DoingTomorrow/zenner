// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteIndexOutputs
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

#nullable disable
namespace System.Data.SQLite
{
  public sealed class SQLiteIndexOutputs
  {
    private SQLiteIndexConstraintUsage[] constraintUsages;
    private int indexNumber;
    private string indexString;
    private int needToFreeIndexString;
    private int orderByConsumed;
    private double? estimatedCost;
    private long? estimatedRows;
    private SQLiteIndexFlags? indexFlags;
    private long? columnsUsed;

    internal SQLiteIndexOutputs(int nConstraint)
    {
      this.constraintUsages = new SQLiteIndexConstraintUsage[nConstraint];
      for (int index = 0; index < nConstraint; ++index)
        this.constraintUsages[index] = new SQLiteIndexConstraintUsage();
    }

    public bool CanUseEstimatedRows() => UnsafeNativeMethods.sqlite3_libversion_number() >= 3008002;

    public bool CanUseIndexFlags() => UnsafeNativeMethods.sqlite3_libversion_number() >= 3009000;

    public bool CanUseColumnsUsed() => UnsafeNativeMethods.sqlite3_libversion_number() >= 3010000;

    public SQLiteIndexConstraintUsage[] ConstraintUsages => this.constraintUsages;

    public int IndexNumber
    {
      get => this.indexNumber;
      set => this.indexNumber = value;
    }

    public string IndexString
    {
      get => this.indexString;
      set => this.indexString = value;
    }

    public int NeedToFreeIndexString
    {
      get => this.needToFreeIndexString;
      set => this.needToFreeIndexString = value;
    }

    public int OrderByConsumed
    {
      get => this.orderByConsumed;
      set => this.orderByConsumed = value;
    }

    public double? EstimatedCost
    {
      get => this.estimatedCost;
      set => this.estimatedCost = value;
    }

    public long? EstimatedRows
    {
      get => this.estimatedRows;
      set => this.estimatedRows = value;
    }

    public SQLiteIndexFlags? IndexFlags
    {
      get => this.indexFlags;
      set => this.indexFlags = value;
    }

    public long? ColumnsUsed
    {
      get => this.columnsUsed;
      set => this.columnsUsed = value;
    }
  }
}
