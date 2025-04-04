// Decompiled with JetBrains decompiler
// Type: MinomatHandler.ModemCounter
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using System;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatHandler
{
  public sealed class ModemCounter
  {
    public ModemCounterType Type { get; set; }

    public ushort Value { get; set; }

    public ushort Errors { get; set; }

    public DateTime? Timepoint { get; set; }

    public static ModemCounter Parse(byte[] payload)
    {
      if (payload == null)
        throw new ArgumentNullException("Input parameter'payload' can not be null!");
      if (payload.Length != 10)
        throw new ArgumentOutOfRangeException("Wrong length of payload! Expected length: 10, Actual: " + payload.Length.ToString());
      if (payload[0] != (byte) 223)
        throw new SCGiError(SCGiErrorType.UnknownResponce, "Wrong first byte of payload! Expected: 0xDF Buffer: " + Util.ByteArrayToHexString(payload));
      ModemCounter modemCounter = new ModemCounter();
      modemCounter.Type = (ModemCounterType) Enum.ToObject(typeof (ModemCounterType), payload[1]);
      modemCounter.Value = BitConverter.ToUInt16(payload, 2);
      modemCounter.Errors = BitConverter.ToUInt16(payload, 4);
      uint uint32 = BitConverter.ToUInt32(payload, 6);
      if (uint32 > 0U)
        modemCounter.Timepoint = new DateTime?(new DateTime(2001, 1, 1).AddSeconds((double) uint32));
      return modemCounter;
    }
  }
}
