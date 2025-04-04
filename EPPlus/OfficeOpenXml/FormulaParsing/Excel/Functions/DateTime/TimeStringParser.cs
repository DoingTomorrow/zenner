// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime.TimeStringParser
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Text.RegularExpressions;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime
{
  public class TimeStringParser
  {
    private const string RegEx24 = "^[0-9]{1,2}(\\:[0-9]{1,2}){0,2}$";
    private const string RegEx12 = "^[0-9]{1,2}(\\:[0-9]{1,2}){0,2}( PM| AM)$";

    private double GetSerialNumber(int hour, int minute, int second)
    {
      double num = 86400.0;
      return ((double) hour * 60.0 * 60.0 + (double) minute * 60.0 + (double) second) / num;
    }

    private void ValidateValues(int hour, int minute, int second)
    {
      if (second < 0 || second > 59)
        throw new FormatException("Illegal value for second: " + (object) second);
      if (minute < 0 || minute > 59)
        throw new FormatException("Illegal value for minute: " + (object) minute);
    }

    public virtual double Parse(string input) => this.InternalParse(input);

    public virtual bool CanParse(string input)
    {
      return Regex.IsMatch(input, "^[0-9]{1,2}(\\:[0-9]{1,2}){0,2}$") || Regex.IsMatch(input, "^[0-9]{1,2}(\\:[0-9]{1,2}){0,2}( PM| AM)$") || System.DateTime.TryParse(input, out System.DateTime _);
    }

    private double InternalParse(string input)
    {
      if (Regex.IsMatch(input, "^[0-9]{1,2}(\\:[0-9]{1,2}){0,2}$"))
        return this.Parse24HourTimeString(input);
      if (Regex.IsMatch(input, "^[0-9]{1,2}(\\:[0-9]{1,2}){0,2}( PM| AM)$"))
        return this.Parse12HourTimeString(input);
      System.DateTime result;
      return System.DateTime.TryParse(input, out result) ? this.GetSerialNumber(result.Hour, result.Minute, result.Second) : -1.0;
    }

    private double Parse12HourTimeString(string input)
    {
      string empty = string.Empty;
      string str = input.Substring(input.Length - 2, 2);
      int hour;
      int minute;
      int second;
      TimeStringParser.GetValuesFromString(input, out hour, out minute, out second);
      if (str == "PM")
        hour += 12;
      this.ValidateValues(hour, minute, second);
      return this.GetSerialNumber(hour, minute, second);
    }

    private double Parse24HourTimeString(string input)
    {
      int hour;
      int minute;
      int second;
      TimeStringParser.GetValuesFromString(input, out hour, out minute, out second);
      this.ValidateValues(hour, minute, second);
      return this.GetSerialNumber(hour, minute, second);
    }

    private static void GetValuesFromString(
      string input,
      out int hour,
      out int minute,
      out int second)
    {
      hour = 0;
      minute = 0;
      second = 0;
      string[] strArray = input.Split(':');
      hour = int.Parse(strArray[0]);
      if (strArray.Length > 1)
        minute = int.Parse(strArray[1]);
      if (strArray.Length <= 2)
        return;
      string s = Regex.Replace(strArray[2], "[^0-9]+$", string.Empty);
      second = int.Parse(s);
    }
  }
}
