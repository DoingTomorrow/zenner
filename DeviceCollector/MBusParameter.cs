// Decompiled with JetBrains decompiler
// Type: DeviceCollector.MBusParameter
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public class MBusParameter : DeviceParameter
  {
    public long ParaValue;
    public int StorageNr;
    public DeviceParameter.ParamUnit Unit;
    public int Exponent;
    private static int[] DIF_ParameterByteLen = new int[16]
    {
      0,
      1,
      2,
      3,
      4,
      4,
      6,
      8,
      0,
      1,
      2,
      3,
      4,
      -1,
      6,
      -1
    };

    public bool ScanReceivedParameter(ref ByteField Data, ref int ReadPtr) => true;
  }
}
