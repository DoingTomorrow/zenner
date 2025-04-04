// Decompiled with JetBrains decompiler
// Type: EDC_Handler.VolumeMonitorEventArgs
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace EDC_Handler
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
  }
}
