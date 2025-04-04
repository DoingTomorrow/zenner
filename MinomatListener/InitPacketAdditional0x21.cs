// Decompiled with JetBrains decompiler
// Type: MinomatListener.InitPacketAdditional0x21
// Assembly: MinomatListener, Version=1.0.0.1, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: BC91232A-BFD0-4DD3-8B1E-2FFF28E228D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatListener.dll

using System;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatListener
{
  public sealed class InitPacketAdditional0x21
  {
    private const int MIN_SIZE = 11;

    public uint? PollingInterval { get; private set; }

    public byte? JobRestartCounter { get; private set; }

    public ushort? DueDate { get; private set; }

    public uint? ModemSystemTime { get; private set; }

    public bool IsMinolTestRunning
    {
      get
      {
        int num1;
        if (this.PollingInterval.HasValue)
        {
          uint? pollingInterval = this.PollingInterval;
          uint num2 = 0;
          if ((int) pollingInterval.GetValueOrDefault() == (int) num2 & pollingInterval.HasValue)
          {
            byte? jobRestartCounter = this.JobRestartCounter;
            if (jobRestartCounter.HasValue)
            {
              jobRestartCounter = this.JobRestartCounter;
              int? nullable = jobRestartCounter.HasValue ? new int?((int) jobRestartCounter.GetValueOrDefault()) : new int?();
              int num3 = 0;
              num1 = nullable.GetValueOrDefault() == num3 & nullable.HasValue ? 1 : 0;
              goto label_5;
            }
          }
        }
        num1 = 0;
label_5:
        return num1 != 0;
      }
    }

    public static InitPacketAdditional0x21 TryParse(byte[] buffer, ref int offset)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (buffer.Length - offset < 11)
        throw new ArgumentException("The additional info 0x21 of INIT packet has wrong length! Expected >=" + 11.ToString() + " bytes but received " + (buffer.Length - offset).ToString() + " bytes.");
      InitPacketAdditional0x21 packetAdditional0x21 = new InitPacketAdditional0x21();
      packetAdditional0x21.PollingInterval = new uint?(Util.SwapBytes(BitConverter.ToUInt32(buffer, offset)));
      offset += 4;
      packetAdditional0x21.JobRestartCounter = new byte?(buffer[offset++]);
      packetAdditional0x21.DueDate = new ushort?(Util.SwapBytes(BitConverter.ToUInt16(buffer, offset)));
      offset += 2;
      packetAdditional0x21.ModemSystemTime = new uint?(Util.SwapBytes(BitConverter.ToUInt32(buffer, offset)));
      offset += 4;
      return packetAdditional0x21;
    }
  }
}
