// Decompiled with JetBrains decompiler
// Type: S4_Handler.TransducerPairState
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

#nullable disable
namespace S4_Handler
{
  public class TransducerPairState
  {
    public bool Perfect = false;
    public double CycleErrorsInPercent = 100.0;
    public uint SuccessfulCycles;
    public uint UpCounts;
    public uint DownCounts;
    public int DiffCounts;
  }
}
