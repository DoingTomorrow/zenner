// Decompiled with JetBrains decompiler
// Type: MinomatHandler.MessUnit
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using System;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatHandler
{
  public sealed class MessUnit
  {
    public uint ID { get; set; }

    public MeasurementCategory Category { get; set; }

    public MeasurementValueType Type { get; set; }

    public RadioProtocol Protocol { get; set; }

    public static MessUnit Parse(uint id, byte[] buffer)
    {
      if (buffer == null)
        throw new ArgumentNullException("Buffer can not be null!");
      if (buffer.Length != 6)
        throw new ArgumentException("Wrong length of buffer! Expected: 6, Actual: " + buffer.Length.ToString());
      if (!Enum.IsDefined(typeof (MeasurementCategory), (object) buffer[2]))
        throw new ArgumentException("Unknown MeasurementCategory received! Value: 0x" + buffer[2].ToString("X2") + " Payload: 0x" + Util.ByteArrayToHexString(buffer));
      if (!Enum.IsDefined(typeof (MeasurementValueType), (object) buffer[3]))
        throw new ArgumentException("Unknown MeasurementValueType received! Value: 0x" + buffer[3].ToString("X2") + " Payload: 0x" + Util.ByteArrayToHexString(buffer));
      return Enum.IsDefined(typeof (RadioProtocol), (object) buffer[4]) ? new MessUnit()
      {
        ID = id,
        Category = (MeasurementCategory) Enum.ToObject(typeof (MeasurementCategory), buffer[2]),
        Type = (MeasurementValueType) Enum.ToObject(typeof (MeasurementValueType), buffer[3]),
        Protocol = (RadioProtocol) Enum.ToObject(typeof (RadioProtocol), buffer[4])
      } : throw new ArgumentException("Unknown RadioProtocol received! Value: 0x" + buffer[4].ToString("X2") + " Payload: 0x" + Util.ByteArrayToHexString(buffer));
    }

    public override string ToString()
    {
      return string.Format("ID: {0}, Category: {1}, Type: {2}, Protocol: {3}", (object) this.ID, (object) this.Category, (object) this.Type, (object) this.Protocol);
    }
  }
}
