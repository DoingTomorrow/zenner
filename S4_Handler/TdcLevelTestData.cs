// Decompiled with JetBrains decompiler
// Type: S4_Handler.TdcLevelTestData
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

#nullable disable
namespace S4_Handler
{
  public class TdcLevelTestData
  {
    internal const int arrayBytes = 6;
    internal const uint ramByteSize = 64;
    public uint[] cntDn = new uint[6];
    public uint[] cntUp = new uint[6];
    public byte[] wvrUp = new byte[6];
    public byte[] wvrDn = new byte[6];
    public byte measCounter;
    public byte offsetOldUp;
    public byte offsetOldDn;
  }
}
