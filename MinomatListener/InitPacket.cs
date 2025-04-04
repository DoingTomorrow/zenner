// Decompiled with JetBrains decompiler
// Type: MinomatListener.InitPacket
// Assembly: MinomatListener, Version=1.0.0.1, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: BC91232A-BFD0-4DD3-8B1E-2FFF28E228D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatListener.dll

using System;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatListener
{
  public sealed class InitPacket
  {
    private const int MIN_SIZE = 17;

    public ulong ChallengeAndGsmIDEncoded { get; private set; }

    public uint ChallengeEncoded => (uint) (this.ChallengeAndGsmIDEncoded >> 32);

    public uint GsmIDEncoded => (uint) this.ChallengeAndGsmIDEncoded;

    public uint SapConfigNr { get; private set; }

    public InitPacketAdditional0x21 Additional0x21 { get; private set; }

    public uint? MasterGsmID { get; private set; }

    public ushort? VersionMasterFirmware { get; private set; }

    public ushort? VersionMasterModemFirmware { get; private set; }

    public uint? ScenarioNumber { get; private set; }

    public override string ToString()
    {
      return string.Format("SAP: {0}, GsmIDEncoded: {1},  ChallengeEncoded: {2}", (object) this.SapConfigNr, (object) this.GsmIDEncoded, (object) this.ChallengeEncoded);
    }

    public static InitPacket TryParse(byte[] buffer)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (buffer.Length < 17)
        throw new ArgumentException("INIT packet has wrong length! Expected >= " + 17.ToString() + " bytes but received " + buffer.Length.ToString() + " bytes.");
      if (buffer[0] > (byte) 0)
        throw new Exception("The first byte in INIT packet is wrong! Expected 0x00 but received 0x" + buffer[0].ToString("X2"));
      InitPacket initPacket = new InitPacket();
      initPacket.ChallengeAndGsmIDEncoded = Util.SwapBytes(BitConverter.ToUInt64(buffer, 1));
      initPacket.SapConfigNr = Util.SwapBytes(BitConverter.ToUInt32(buffer, 13));
      int offset = 17;
      while (buffer.Length - 1 > offset)
      {
        switch (buffer[offset++])
        {
          case 33:
            initPacket.Additional0x21 = InitPacketAdditional0x21.TryParse(buffer, ref offset);
            break;
          case 34:
            initPacket.MasterGsmID = new uint?(Util.SwapBytes(BitConverter.ToUInt32(buffer, offset)));
            offset += 4;
            break;
          case 35:
            byte[] numArray = buffer;
            int index = offset;
            int startIndex = index + 1;
            switch (numArray[index])
            {
              case 0:
                initPacket.VersionMasterFirmware = new ushort?(Util.SwapBytes(BitConverter.ToUInt16(buffer, startIndex)));
                offset = startIndex + 2;
                break;
              case 1:
                initPacket.VersionMasterModemFirmware = new ushort?(Util.SwapBytes(BitConverter.ToUInt16(buffer, startIndex)));
                offset = startIndex + 2;
                break;
              default:
                throw new Exception("The INIT packet contains additional information (0x23) which is unknown! Buffer: " + Util.ByteArrayToHexString(buffer));
            }
            break;
          case 36:
            initPacket.ScenarioNumber = new uint?(Util.SwapBytes(BitConverter.ToUInt32(buffer, offset)));
            offset += 4;
            break;
          default:
            throw new Exception("The INIT packet contains additional information which is unknown! Buffer: " + Util.ByteArrayToHexString(buffer));
        }
      }
      return initPacket;
    }
  }
}
