// Decompiled with JetBrains decompiler
// Type: NFCL_Handler.IuwCurrentData
// Assembly: NFCL_Handler, Version=2.3.2.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 887E21A2-7448-48CC-AF3E-C39E4C7B3AFD
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NFCL_Handler.dll

using System;
using ZR_ClassLibrary;

#nullable disable
namespace NFCL_Handler
{
  [Serializable]
  public class IuwCurrentData
  {
    public readonly string[] VOLUME_UNITS = new string[4]
    {
      "m\u00B3",
      "gal",
      "ft\u00B3",
      "??"
    };
    public readonly string[] FLOW_UNITS = new string[4]
    {
      "m\u00B3/h",
      "gal/min",
      "ft\u00B3/min",
      "??"
    };

    public DateTime? DeviceTime { get; set; }

    public byte UnitIndex { get; set; }

    public double Volume { get; set; }

    public double FlowVolume { get; set; }

    public double ReturnVolume { get; set; }

    public float Flow { get; set; }

    public float? Temperature { get; set; }

    public static IuwCurrentData Parse(byte[] bytes, int offset = 0)
    {
      IuwCurrentData iuwCurrentData = new IuwCurrentData();
      iuwCurrentData.DeviceTime = Util.ConvertToDateTime_SystemTime48(bytes, offset);
      offset += 6;
      iuwCurrentData.UnitIndex = bytes[offset++];
      iuwCurrentData.Volume = BitConverter.ToDouble(bytes, offset);
      offset += 8;
      iuwCurrentData.FlowVolume = BitConverter.ToDouble(bytes, offset);
      offset += 8;
      iuwCurrentData.ReturnVolume = BitConverter.ToDouble(bytes, offset);
      offset += 8;
      iuwCurrentData.Flow = BitConverter.ToSingle(bytes, offset);
      offset += 4;
      iuwCurrentData.Temperature = bytes.Length >= offset + 4 ? new float?(BitConverter.ToSingle(bytes, offset)) : new float?();
      if (iuwCurrentData.UnitIndex > (byte) 2)
        iuwCurrentData.UnitIndex = (byte) 3;
      return iuwCurrentData;
    }
  }
}
