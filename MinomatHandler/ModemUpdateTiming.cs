// Decompiled with JetBrains decompiler
// Type: MinomatHandler.ModemUpdateTiming
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using System;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatHandler
{
  public sealed class ModemUpdateTiming
  {
    public DateTime? StartTime { get; set; }

    public DateTime? FinishTime { get; set; }

    internal static ModemUpdateTiming Parse(byte[] payload)
    {
      if (payload == null)
        throw new ArgumentNullException("input parameter'payload' can not be null!");
      if (payload[0] != (byte) 237)
        throw new SCGiError(SCGiErrorType.UnknownResponce, "Wrong first byte of payload! Expected: 0xED Buffer: " + Util.ByteArrayToHexString(payload));
      if (payload.Length == 2)
        return new ModemUpdateTiming();
      uint num = payload.Length == 10 ? BitConverter.ToUInt32(payload, 1) : throw new ArgumentOutOfRangeException("Wrong length of payload! Expected length: 10, Actual: " + payload.Length.ToString());
      uint uint32 = BitConverter.ToUInt32(payload, 5);
      return new ModemUpdateTiming()
      {
        StartTime = new DateTime?(new DateTime(2001, 1, 1).AddSeconds((double) num)),
        FinishTime = new DateTime?(new DateTime(2001, 1, 1).AddSeconds((double) uint32))
      };
    }
  }
}
