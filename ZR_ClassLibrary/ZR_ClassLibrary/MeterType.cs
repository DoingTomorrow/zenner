// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.MeterType
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;

#nullable disable
namespace ZR_ClassLibrary
{
  public sealed class MeterType
  {
    public int MeterTypeID;
    public string MTypeTableName;
    public string Typename;
    public DateTime GenerateDate;
    public string Description;

    public override string ToString() => this.Description;
  }
}
