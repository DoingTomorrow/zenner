// Decompiled with JetBrains decompiler
// Type: GMMToMSSMigrator.GMMReadingValuesDTO
// Assembly: GMMToMSSMigrator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3ACF3C29-B99D-4830-8DFE-AD2278C0306B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMMToMSSMigrator.dll

using System;

#nullable disable
namespace GMMToMSSMigrator
{
  public class GMMReadingValuesDTO
  {
    public string SerialNr { get; set; }

    public DateTime? TimePoint { get; set; }

    public double Value { get; set; }

    public int PhysicalQuantity { get; set; }

    public int MeterType { get; set; }

    public int Calculation { get; set; }

    public int CalculationStart { get; set; }

    public int StorageInterval { get; set; }

    public int Creation { get; set; }

    public int ValueIdentIndex { get; set; }
  }
}
