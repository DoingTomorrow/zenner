// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.MeterData
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;

#nullable disable
namespace ZR_ClassLibrary
{
  public sealed class MeterData
  {
    public int MeterID;
    public DateTime? TimePoint;
    public int PValueID;
    public string PValue;
    public byte[] PValueBinary;
    public byte SyncStatus;

    public override string ToString() => this.MeterID.ToString() + " " + this.TimePoint.ToString();
  }
}
