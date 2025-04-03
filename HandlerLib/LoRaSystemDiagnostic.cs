// Decompiled with JetBrains decompiler
// Type: HandlerLib.LoRaSystemDiagnostic
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;

#nullable disable
namespace HandlerLib
{
  public sealed class LoRaSystemDiagnostic : ReturnValue
  {
    public byte DiagnosticConfig { get; set; }

    public byte DailyStartTime { get; set; }

    public byte DailyEndTime { get; set; }

    public ushort RemainigActivity { get; set; }

    public uint RemainigDiagnostic { get; set; }

    public static ushort getDailyTimeFromString(string argument)
    {
      try
      {
        string[] strArray = argument.Split(':');
        ushort num1 = ushort.Parse(strArray[0]);
        ushort num2 = ushort.Parse(strArray[1]);
        if (num1 < (ushort) 0 || num1 > (ushort) 24 || num2 < (ushort) 0 || num2 > (ushort) 59)
          throw new Exception("Time is out of range");
        if (num1 == (ushort) 24)
          num1 = (ushort) 0;
        return (ushort) ((int) num1 * 6 + (int) num2 / 10);
      }
      catch (Exception ex)
      {
        throw new Exception("DailyTime is incorrect !!!", ex);
      }
    }

    public string getDiagnosticIntervalAsString()
    {
      string intervalAsString = "n/a";
      if (this.DiagnosticConfig == byte.MaxValue)
        return "Diagnostic stopped!";
      byte num1 = (byte) ((uint) this.DiagnosticConfig & 15U);
      byte num2 = (byte) (((int) this.DiagnosticConfig & 240) >> 4);
      switch (num1)
      {
        case 0:
          intervalAsString = "0.25 h";
          break;
        case 1:
          intervalAsString = "0.50 h";
          break;
        case 2:
          intervalAsString = "1.00 h";
          break;
        case 3:
          intervalAsString = "2.00 h";
          break;
        case 4:
          intervalAsString = "3.00 h";
          break;
        case 5:
          intervalAsString = "6.00 h";
          break;
        case 6:
          intervalAsString = "12.00 h";
          break;
        case 7:
          intervalAsString = "24.00 h";
          break;
      }
      return intervalAsString;
    }

    public string getDiagnosticRepeatAsString()
    {
      string diagnosticRepeatAsString = "n/a";
      if (this.DiagnosticConfig == byte.MaxValue)
        return "Diagnostic stopped!";
      byte num = (byte) ((uint) this.DiagnosticConfig & 15U);
      switch ((byte) (((int) this.DiagnosticConfig & 240) >> 4))
      {
        case 0:
          diagnosticRepeatAsString = "5";
          break;
        case 1:
          diagnosticRepeatAsString = "10";
          break;
        case 2:
          diagnosticRepeatAsString = "20";
          break;
        case 3:
          diagnosticRepeatAsString = "50";
          break;
        case 4:
          diagnosticRepeatAsString = "100";
          break;
        case 5:
          diagnosticRepeatAsString = "200";
          break;
        case 6:
          diagnosticRepeatAsString = "500";
          break;
        case 7:
          diagnosticRepeatAsString = "1000";
          break;
      }
      return diagnosticRepeatAsString;
    }

    public string getDailyStartEndTimeAsString(bool startTime = true)
    {
      ushort num1 = startTime ? (ushort) this.DailyStartTime : (ushort) this.DailyEndTime;
      int num2 = (int) num1 / 6;
      string str1 = num2.ToString("00");
      num2 = (int) num1 % 6 * 10;
      string str2 = num2.ToString("00");
      return str1 + ":" + str2;
    }
  }
}
