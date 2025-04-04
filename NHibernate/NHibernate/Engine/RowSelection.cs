// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.RowSelection
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Engine
{
  [Serializable]
  public sealed class RowSelection
  {
    public static readonly int NoValue = -1;
    private int firstRow = RowSelection.NoValue;
    private int maxRows = RowSelection.NoValue;
    private int timeout = RowSelection.NoValue;
    private int fetchSize = RowSelection.NoValue;

    public int FirstRow
    {
      get => this.firstRow;
      set => this.firstRow = value;
    }

    public int MaxRows
    {
      get => this.maxRows;
      set => this.maxRows = value;
    }

    public int Timeout
    {
      get => this.timeout;
      set => this.timeout = value;
    }

    public int FetchSize
    {
      get => this.fetchSize;
      set => this.fetchSize = value;
    }

    public bool DefinesLimits => this.maxRows != RowSelection.NoValue || this.firstRow > 0;
  }
}
