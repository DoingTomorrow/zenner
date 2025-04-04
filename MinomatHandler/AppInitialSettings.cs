// Decompiled with JetBrains decompiler
// Type: MinomatHandler.AppInitialSettings
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using System;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatHandler
{
  public sealed class AppInitialSettings
  {
    public uint Challenge { get; set; }

    public uint GsmId { get; set; }

    public uint SapConfigNumber { get; set; }

    public byte[] MD5 { get; set; }

    public static AppInitialSettings Parse(byte[] payload)
    {
      if (payload == null)
        throw new ArgumentNullException("Payload can not be null!");
      if (payload.Length < 14)
        throw new ArgumentException("Wrong length of payload!");
      return payload[0] == (byte) 49 ? new AppInitialSettings()
      {
        Challenge = BitConverter.ToUInt32(payload, 1),
        GsmId = BitConverter.ToUInt32(payload, 5),
        SapConfigNumber = BitConverter.ToUInt32(payload, 9)
      } : throw new ArgumentException("Wrong first byte in payload! Expected: 0x31, Actual: 0x" + payload[0].ToString("X2") + " Buffer: " + Util.ByteArrayToHexString(payload));
    }

    public override string ToString()
    {
      return string.Format("Challenge: {0}, GSM ID: {1}, SAP ID: {2}", (object) this.Challenge, (object) this.GsmId, (object) this.SapConfigNumber);
    }
  }
}
