// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExcelUtilities.RangeAddress
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExcelUtilities
{
  public class RangeAddress
  {
    private static RangeAddress _empty = new RangeAddress();

    public RangeAddress() => this.Address = string.Empty;

    internal string Address { get; set; }

    public string Worksheet { get; internal set; }

    public int FromCol { get; internal set; }

    public int ToCol { get; internal set; }

    public int FromRow { get; internal set; }

    public int ToRow { get; internal set; }

    public override string ToString() => this.Address;

    public static RangeAddress Empty => RangeAddress._empty;

    public bool CollidesWith(RangeAddress other)
    {
      return !(other.Worksheet != this.Worksheet) && other.FromRow <= this.ToRow && other.FromCol <= this.ToCol && this.FromRow <= other.ToRow && this.FromCol <= other.ToCol;
    }
  }
}
