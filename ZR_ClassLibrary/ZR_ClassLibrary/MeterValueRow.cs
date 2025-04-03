// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.MeterValueRow
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;

#nullable disable
namespace ZR_ClassLibrary
{
  public struct MeterValueRow
  {
    public int MeterId;
    public byte ValueIdentIndex;
    public DateTime TimePoint;
    public double Value;
    public byte PhysicalQuantity;
    public byte MeterType;
    public byte Calculation;
    public byte CalculationStart;
    public byte StorageInterval;
    public byte Creation;
    public string SerialNr;
    public string NodeName;
    public int NodeID;

    public string MeterTypeString
    {
      get => ValueIdent.Translate<ValueIdent.ValueIdPart_MeterType>(this.MeterType);
    }

    public string PhysicalQuantityString
    {
      get => ValueIdent.Translate<ValueIdent.ValueIdPart_PhysicalQuantity>(this.PhysicalQuantity);
    }

    public string CalculationString
    {
      get => ValueIdent.Translate<ValueIdent.ValueIdPart_Calculation>(this.Calculation);
    }

    public string StorageIntervalString
    {
      get => ValueIdent.Translate<ValueIdent.ValueIdPart_StorageInterval>(this.StorageInterval);
    }

    public string CalculationStartString
    {
      get => ValueIdent.Translate<ValueIdent.ValueIdPart_CalculationStart>(this.CalculationStart);
    }

    public string CreationString
    {
      get => ValueIdent.Translate<ValueIdent.ValueIdPart_Creation>(this.Creation);
    }

    public string ValueIdentIndexString
    {
      get => ValueIdent.TranslateIndex(this.PhysicalQuantity, this.ValueIdentIndex);
    }

    public string Unit => ValueIdent.GetUnit(this.PhysicalQuantity);
  }
}
