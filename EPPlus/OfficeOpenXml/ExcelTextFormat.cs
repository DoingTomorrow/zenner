// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelTextFormat
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Globalization;
using System.Text;

#nullable disable
namespace OfficeOpenXml
{
  public class ExcelTextFormat
  {
    public ExcelTextFormat()
    {
      this.Delimiter = ',';
      this.TextQualifier = char.MinValue;
      this.EOL = "\r\n";
      this.Culture = CultureInfo.InvariantCulture;
      this.DataTypes = (eDataTypes[]) null;
      this.SkipLinesBeginning = 0;
      this.SkipLinesEnd = 0;
      this.Encoding = Encoding.ASCII;
    }

    public char Delimiter { get; set; }

    public char TextQualifier { get; set; }

    public string EOL { get; set; }

    public eDataTypes[] DataTypes { get; set; }

    public CultureInfo Culture { get; set; }

    public int SkipLinesBeginning { get; set; }

    public int SkipLinesEnd { get; set; }

    public Encoding Encoding { get; set; }
  }
}
