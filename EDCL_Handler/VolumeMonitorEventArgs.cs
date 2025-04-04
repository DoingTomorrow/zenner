// Decompiled with JetBrains decompiler
// Type: EDCL_Handler.VolumeMonitorEventArgs
// Assembly: EDCL_Handler, Version=2.2.10.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F3010E47-8885-4BE8-8551-D37B09710D3C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDCL_Handler.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace EDCL_Handler
{
  public sealed class VolumeMonitorEventArgs : CancelEventArgs
  {
    public byte CoilDetectionResult_A;
    public byte CoilDetectionResult_B;
    public byte StateMachineValue;
    public byte PollCounter;
    public short PulseForwardCount;
    public ushort PulseReturnCount;
    public ushort PulseErrorCount;

    public byte CoilSampleResult => (byte) ((int) this.StateMachineValue >> 1 & 15);

    public bool TamperSensorStatus => ((int) this.StateMachineValue & 64) == 64;

    public bool RemovalSensorStatus => ((int) this.StateMachineValue & 128) == 128;

    public byte Checksum
    {
      get
      {
        return (byte) ((uint) this.CoilDetectionResult_A + (uint) this.CoilDetectionResult_B + (uint) this.StateMachineValue + (uint) this.PollCounter);
      }
    }

    public override string ToString()
    {
      return string.Format("#{0:X2}#{1:X2}#{2:X2}#{3:X2}#{4:X2}, {0}, {1}, {2}, {3}, {4}, 0, 0, 0, 0, 0, 0, 0101, 0", (object) this.CoilDetectionResult_A, (object) this.CoilDetectionResult_B, (object) this.StateMachineValue, (object) this.PollCounter, (object) this.Checksum);
    }

    public IEnumerable<byte> ToByteArray()
    {
      List<byte> byteArray = new List<byte>();
      byteArray.Add(this.CoilDetectionResult_A);
      byteArray.Add(this.CoilDetectionResult_B);
      byteArray.Add(this.StateMachineValue);
      byteArray.Add(this.PollCounter);
      byteArray.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.PulseForwardCount));
      byteArray.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.PulseReturnCount));
      byteArray.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.PulseErrorCount));
      return (IEnumerable<byte>) byteArray;
    }

    public static VolumeMonitorEventArgs Parse(byte[] buffer, ref int offset)
    {
      VolumeMonitorEventArgs monitorEventArgs = new VolumeMonitorEventArgs()
      {
        CoilDetectionResult_A = buffer[offset],
        CoilDetectionResult_B = buffer[offset + 1],
        StateMachineValue = buffer[offset + 2],
        PollCounter = buffer[offset + 3],
        PulseForwardCount = BitConverter.ToInt16(buffer, offset + 4),
        PulseReturnCount = BitConverter.ToUInt16(buffer, offset + 6),
        PulseErrorCount = BitConverter.ToUInt16(buffer, offset + 8)
      };
      offset += 10;
      return monitorEventArgs;
    }
  }
}
