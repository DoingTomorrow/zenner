// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Exceptions.ExcelErrorCodes
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Exceptions
{
  public class ExcelErrorCodes
  {
    private static readonly IEnumerable<string> Codes = (IEnumerable<string>) new List<string>()
    {
      ExcelErrorCodes.Value.Code,
      ExcelErrorCodes.Name.Code,
      ExcelErrorCodes.NoValueAvaliable.Code
    };

    private ExcelErrorCodes(string code) => this.Code = code;

    public string Code { get; private set; }

    public override int GetHashCode() => this.Code.GetHashCode();

    public override bool Equals(object obj)
    {
      return (object) (obj as ExcelErrorCodes) != null && ((ExcelErrorCodes) obj).Code.Equals(this.Code);
    }

    public static bool operator ==(ExcelErrorCodes c1, ExcelErrorCodes c2)
    {
      return c1.Code.Equals(c2.Code);
    }

    public static bool operator !=(ExcelErrorCodes c1, ExcelErrorCodes c2)
    {
      return !c1.Code.Equals(c2.Code);
    }

    public static bool IsErrorCode(object valueToTest)
    {
      if (valueToTest == null)
        return false;
      string candidate = valueToTest.ToString();
      return ExcelErrorCodes.Codes.FirstOrDefault<string>((Func<string, bool>) (x => x == candidate)) != null;
    }

    public static ExcelErrorCodes Value => new ExcelErrorCodes("#VALUE!");

    public static ExcelErrorCodes Name => new ExcelErrorCodes("#NAME?");

    public static ExcelErrorCodes NoValueAvaliable => new ExcelErrorCodes("#N/A");
  }
}
