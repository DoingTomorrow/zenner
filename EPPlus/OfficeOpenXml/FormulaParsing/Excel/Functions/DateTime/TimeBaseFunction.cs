// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime.TimeBaseFunction
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime
{
  public abstract class TimeBaseFunction : ExcelFunction
  {
    public TimeBaseFunction() => this.TimeStringParser = new TimeStringParser();

    protected TimeStringParser TimeStringParser { get; private set; }

    protected double SerialNumber { get; private set; }

    public void ValidateAndInitSerialNumber(IEnumerable<FunctionArgument> arguments)
    {
      this.ValidateArguments(arguments, 1);
      this.SerialNumber = this.ArgToDecimal(arguments, 0);
    }

    protected double SecondsInADay => 86400.0;

    protected double GetTimeSerialNumber(double seconds) => seconds / this.SecondsInADay;

    protected double GetSeconds(double serialNumber) => serialNumber * this.SecondsInADay;

    protected double GetHour(double serialNumber)
    {
      return (double) ((int) this.GetSeconds(serialNumber) / 3600);
    }

    protected double GetMinute(double serialNumber)
    {
      double num = this.GetSeconds(serialNumber) - this.GetHour(serialNumber) * 60.0 * 60.0;
      return (num - num % 60.0) / 60.0;
    }

    protected double GetSecond(double serialNumber) => this.GetSeconds(serialNumber) % 60.0;
  }
}
