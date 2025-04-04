// Decompiled with JetBrains decompiler
// Type: SmokeDetectorHandler.MinoprotectIII_Events
// Assembly: SmokeDetectorHandler, Version=2.20.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E8970E7-4D1B-41F1-9589-E7C5C5D80A7B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmokeDetectorHandler.dll

using DeviceCollector;
using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace SmokeDetectorHandler
{
  public sealed class MinoprotectIII_Events
  {
    public byte[] Buffer { get; private set; }

    public SmokeDetectorEvent EventIdentification { get; set; }

    public DateTime? EventDate { get; set; }

    public ushort EventData { get; set; }

    public object EventValue
    {
      get
      {
        SmokeDetectorEvent eventIdentification = this.EventIdentification;
        if ((uint) eventIdentification <= 128U)
        {
          if ((uint) eventIdentification <= 8U)
          {
            if ((uint) (eventIdentification - (ushort) 1) > 1U && eventIdentification != SmokeDetectorEvent.BatteryWarningRadio)
            {
              if (eventIdentification == SmokeDetectorEvent.SmokeChamberPollutionForewarning)
                goto label_23;
              else
                goto label_29;
            }
          }
          else if ((uint) eventIdentification <= 32U)
          {
            switch (eventIdentification)
            {
              case SmokeDetectorEvent.SmokeChamberPollutionWarning:
                goto label_23;
              case SmokeDetectorEvent.PushButtonFailure:
                if (this.EventData == (ushort) 256)
                  return (object) false;
                if (this.EventData == (ushort) 0)
                  return (object) false;
                return this.EventData == (ushort) 1 ? (object) true : (object) this.EventData;
              default:
                goto label_29;
            }
          }
          else if (eventIdentification == SmokeDetectorEvent.HornFailure || eventIdentification == SmokeDetectorEvent.RemovingDetection)
            goto label_23;
          else
            goto label_29;
        }
        else if ((uint) eventIdentification <= 2048U)
        {
          if ((uint) eventIdentification <= 512U)
          {
            if (eventIdentification != SmokeDetectorEvent.TestAlarmReleased && eventIdentification != SmokeDetectorEvent.SmokeAlarmReleased)
              goto label_29;
          }
          else if (eventIdentification == SmokeDetectorEvent.IngressAperturesObstructionDetected || eventIdentification == SmokeDetectorEvent.ObjectInSurroundingAreaDetected)
            goto label_23;
          else
            goto label_29;
        }
        else if ((uint) eventIdentification <= 8192U)
        {
          if (eventIdentification == SmokeDetectorEvent.LED_Failure)
            return (object) (LED_Broken) this.EventData;
          if (eventIdentification == SmokeDetectorEvent.Bit13_undefined)
            goto label_29;
          else
            goto label_29;
        }
        else if (eventIdentification == SmokeDetectorEvent.Bit14_undefined || eventIdentification == SmokeDetectorEvent.Bit15_undefined)
          goto label_29;
        else
          goto label_29;
        return (object) this.EventData;
label_23:
        if (this.EventData == (ushort) 256)
          return (object) false;
        return this.EventData == (ushort) 1 ? (object) true : (object) this.EventData;
label_29:
        return (object) this.EventData;
      }
    }

    internal static List<MinoprotectIII_Events> Parse(byte[] buffer)
    {
      if (buffer == null)
        throw new NullReferenceException("Can not parse evets of smoke detector! The buffer is null.");
      List<MinoprotectIII_Events> minoprotectIiiEventsList = new List<MinoprotectIII_Events>();
      for (int srcOffset = 0; srcOffset < buffer.Length; srcOffset += 5)
      {
        byte[] dst = new byte[5];
        System.Buffer.BlockCopy((Array) buffer, srcOffset, (Array) dst, 0, dst.Length);
        SmokeDetectorEvent smokeDetectorEvent = MinoprotectIII_Events.TranslateToSmokeDetectorEvent(buffer[srcOffset]);
        if (smokeDetectorEvent != 0)
          minoprotectIiiEventsList.Add(new MinoprotectIII_Events()
          {
            Buffer = dst,
            EventIdentification = smokeDetectorEvent,
            EventData = BitConverter.ToUInt16(buffer, srcOffset + 1),
            EventDate = Util.ConvertToDate_MBus_CP16_TypeG(buffer, srcOffset + 3)
          });
      }
      return minoprotectIiiEventsList;
    }

    private static SmokeDetectorEvent TranslateToSmokeDetectorEvent(byte value)
    {
      switch (value)
      {
        case 1:
          return SmokeDetectorEvent.BatteryForewarning;
        case 2:
          return SmokeDetectorEvent.BatteryFault;
        case 3:
          return SmokeDetectorEvent.BatteryWarningRadio;
        case 4:
          return SmokeDetectorEvent.SmokeChamberPollutionForewarning;
        case 5:
          return SmokeDetectorEvent.SmokeChamberPollutionWarning;
        case 6:
          return SmokeDetectorEvent.PushButtonFailure;
        case 7:
          return SmokeDetectorEvent.HornFailure;
        case 8:
          return SmokeDetectorEvent.RemovingDetection;
        case 9:
          return SmokeDetectorEvent.TestAlarmReleased;
        case 10:
          return SmokeDetectorEvent.SmokeAlarmReleased;
        case 11:
          return SmokeDetectorEvent.IngressAperturesObstructionDetected;
        case 12:
          return SmokeDetectorEvent.ObjectInSurroundingAreaDetected;
        case 13:
          return SmokeDetectorEvent.LED_Failure;
        default:
          return ~(SmokeDetectorEvent.BatteryForewarning | SmokeDetectorEvent.BatteryFault | SmokeDetectorEvent.BatteryWarningRadio | SmokeDetectorEvent.SmokeChamberPollutionForewarning | SmokeDetectorEvent.SmokeChamberPollutionWarning | SmokeDetectorEvent.PushButtonFailure | SmokeDetectorEvent.HornFailure | SmokeDetectorEvent.RemovingDetection | SmokeDetectorEvent.TestAlarmReleased | SmokeDetectorEvent.SmokeAlarmReleased | SmokeDetectorEvent.IngressAperturesObstructionDetected | SmokeDetectorEvent.ObjectInSurroundingAreaDetected | SmokeDetectorEvent.LED_Failure | SmokeDetectorEvent.Bit13_undefined | SmokeDetectorEvent.Bit14_undefined | SmokeDetectorEvent.Bit15_undefined);
      }
    }

    public override string ToString()
    {
      return "Buffer: " + Util.ByteArrayToHexString(this.Buffer) + "    " + (this.EventDate.HasValue ? this.EventDate.Value.ToShortDateString() + "  " : "Invalid date") + " " + this.EventIdentification.ToString() + " " + (this.EventValue != null ? this.EventValue.ToString() : "null");
    }

    internal MinoprotectIII_Events DeepCopy()
    {
      return new MinoprotectIII_Events()
      {
        Buffer = this.Buffer,
        EventIdentification = this.EventIdentification,
        EventDate = this.EventDate,
        EventData = this.EventData
      };
    }
  }
}
