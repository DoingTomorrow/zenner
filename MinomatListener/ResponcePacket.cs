// Decompiled with JetBrains decompiler
// Type: MinomatListener.ResponcePacket
// Assembly: MinomatListener, Version=1.0.0.1, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: BC91232A-BFD0-4DD3-8B1E-2FFF28E228D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatListener.dll

using NLog;
using System;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatListener
{
  public sealed class ResponcePacket
  {
    private static Logger logger = LogManager.GetLogger(nameof (ResponcePacket));
    private const int MIN_SIZE = 9;

    public uint ChallengeEncoded { get; private set; }

    public uint ChallengeEncodedOld { get; private set; }

    public byte[] SCGI { get; private set; }

    public override string ToString()
    {
      return this.SCGI != null ? string.Format("ChallengeEncoded: {0}, ChallengeEncodedOld: {1},  SCGI: {2}", (object) this.ChallengeEncoded, (object) this.ChallengeEncodedOld, (object) Util.ByteArrayToHexString(this.SCGI)) : string.Format("ChallengeEncoded: {0}, ChallengeEncodedOld: {1},  SCGI: NULL", (object) this.ChallengeEncoded, (object) this.ChallengeEncodedOld);
    }

    public static ResponcePacket TryParse(byte[] buffer)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (buffer.Length < 9)
        throw new ArgumentException("RESP packet has wrong length! Expected >= " + 9.ToString() + " bytes but received " + buffer.Length.ToString() + " bytes.");
      if (buffer[0] != (byte) 3)
        throw new Exception("The first byte in RESP packet is wrong! Expected 0x03 but received 0x" + buffer[0].ToString("X2"));
      ResponcePacket responcePacket = new ResponcePacket();
      responcePacket.ChallengeEncoded = Util.SwapBytes(BitConverter.ToUInt32(buffer, 1));
      responcePacket.ChallengeEncodedOld = Util.SwapBytes(BitConverter.ToUInt32(buffer, 5));
      if (buffer.Length > 9)
      {
        byte[] destinationArray = new byte[buffer.Length - 9];
        Array.Copy((Array) buffer, 9, (Array) destinationArray, 0, destinationArray.Length);
        responcePacket.SCGI = destinationArray;
      }
      return responcePacket;
    }
  }
}
