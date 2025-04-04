// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.XmlAccess.ExcelNumberFormatXml
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Style.XmlAccess
{
  public sealed class ExcelNumberFormatXml : StyleXmlHelper
  {
    private const string fmtPath = "@formatCode";
    private int _numFmtId;
    private string _format = string.Empty;
    private ExcelNumberFormatXml.ExcelFormatTranslator _translator;

    internal ExcelNumberFormatXml(XmlNamespaceManager nameSpaceManager)
      : base(nameSpaceManager)
    {
    }

    internal ExcelNumberFormatXml(XmlNamespaceManager nameSpaceManager, bool buildIn)
      : base(nameSpaceManager)
    {
      this.BuildIn = buildIn;
    }

    internal ExcelNumberFormatXml(XmlNamespaceManager nsm, XmlNode topNode)
      : base(nsm, topNode)
    {
      this._numFmtId = this.GetXmlNodeInt("@numFmtId");
      this._format = this.GetXmlNodeString("@formatCode");
    }

    public bool BuildIn { get; private set; }

    public int NumFmtId
    {
      get => this._numFmtId;
      set => this._numFmtId = value;
    }

    internal override string Id => this._format;

    public string Format
    {
      get => this._format;
      set
      {
        this._numFmtId = ExcelNumberFormat.GetFromBuildIdFromFormat(value);
        this._format = value;
      }
    }

    internal string GetNewID(int NumFmtId, string Format)
    {
      if (NumFmtId < 0)
        NumFmtId = ExcelNumberFormat.GetFromBuildIdFromFormat(Format);
      return NumFmtId.ToString();
    }

    internal static void AddBuildIn(
      XmlNamespaceManager NameSpaceManager,
      ExcelStyleCollection<ExcelNumberFormatXml> NumberFormats)
    {
      NumberFormats.Add("General", new ExcelNumberFormatXml(NameSpaceManager, true)
      {
        NumFmtId = 0,
        Format = "General"
      });
      NumberFormats.Add("0", new ExcelNumberFormatXml(NameSpaceManager, true)
      {
        NumFmtId = 1,
        Format = "0"
      });
      NumberFormats.Add("0.00", new ExcelNumberFormatXml(NameSpaceManager, true)
      {
        NumFmtId = 2,
        Format = "0.00"
      });
      NumberFormats.Add("#,##0", new ExcelNumberFormatXml(NameSpaceManager, true)
      {
        NumFmtId = 3,
        Format = "#,##0"
      });
      NumberFormats.Add("#,##0.00", new ExcelNumberFormatXml(NameSpaceManager, true)
      {
        NumFmtId = 4,
        Format = "#,##0.00"
      });
      NumberFormats.Add("0%", new ExcelNumberFormatXml(NameSpaceManager, true)
      {
        NumFmtId = 9,
        Format = "0%"
      });
      NumberFormats.Add("0.00%", new ExcelNumberFormatXml(NameSpaceManager, true)
      {
        NumFmtId = 10,
        Format = "0.00%"
      });
      NumberFormats.Add("0.00E+00", new ExcelNumberFormatXml(NameSpaceManager, true)
      {
        NumFmtId = 11,
        Format = "0.00E+00"
      });
      NumberFormats.Add("# ?/?", new ExcelNumberFormatXml(NameSpaceManager, true)
      {
        NumFmtId = 12,
        Format = "# ?/?"
      });
      NumberFormats.Add("# ??/??", new ExcelNumberFormatXml(NameSpaceManager, true)
      {
        NumFmtId = 13,
        Format = "# ??/??"
      });
      NumberFormats.Add("mm-dd-yy", new ExcelNumberFormatXml(NameSpaceManager, true)
      {
        NumFmtId = 14,
        Format = "mm-dd-yy"
      });
      NumberFormats.Add("d-mmm-yy", new ExcelNumberFormatXml(NameSpaceManager, true)
      {
        NumFmtId = 15,
        Format = "d-mmm-yy"
      });
      NumberFormats.Add("d-mmm", new ExcelNumberFormatXml(NameSpaceManager, true)
      {
        NumFmtId = 16,
        Format = "d-mmm"
      });
      NumberFormats.Add("mmm-yy", new ExcelNumberFormatXml(NameSpaceManager, true)
      {
        NumFmtId = 17,
        Format = "mmm-yy"
      });
      NumberFormats.Add("h:mm AM/PM", new ExcelNumberFormatXml(NameSpaceManager, true)
      {
        NumFmtId = 18,
        Format = "h:mm AM/PM"
      });
      NumberFormats.Add("h:mm:ss AM/PM", new ExcelNumberFormatXml(NameSpaceManager, true)
      {
        NumFmtId = 19,
        Format = "h:mm:ss AM/PM"
      });
      NumberFormats.Add("h:mm", new ExcelNumberFormatXml(NameSpaceManager, true)
      {
        NumFmtId = 20,
        Format = "h:mm"
      });
      NumberFormats.Add("h:mm:ss", new ExcelNumberFormatXml(NameSpaceManager, true)
      {
        NumFmtId = 21,
        Format = "h:mm:ss"
      });
      NumberFormats.Add("m/d/yy h:mm", new ExcelNumberFormatXml(NameSpaceManager, true)
      {
        NumFmtId = 22,
        Format = "m/d/yy h:mm"
      });
      NumberFormats.Add("#,##0 ;(#,##0)", new ExcelNumberFormatXml(NameSpaceManager, true)
      {
        NumFmtId = 37,
        Format = "#,##0 ;(#,##0)"
      });
      NumberFormats.Add("#,##0 ;[Red](#,##0)", new ExcelNumberFormatXml(NameSpaceManager, true)
      {
        NumFmtId = 38,
        Format = "#,##0 ;[Red](#,##0)"
      });
      NumberFormats.Add("#,##0.00;(#,##0.00)", new ExcelNumberFormatXml(NameSpaceManager, true)
      {
        NumFmtId = 39,
        Format = "#,##0.00;(#,##0.00)"
      });
      NumberFormats.Add("#,##0.00;[Red](#,#)", new ExcelNumberFormatXml(NameSpaceManager, true)
      {
        NumFmtId = 40,
        Format = "#,##0.00;[Red](#,#)"
      });
      NumberFormats.Add("mm:ss", new ExcelNumberFormatXml(NameSpaceManager, true)
      {
        NumFmtId = 45,
        Format = "mm:ss"
      });
      NumberFormats.Add("[h]:mm:ss", new ExcelNumberFormatXml(NameSpaceManager, true)
      {
        NumFmtId = 46,
        Format = "[h]:mm:ss"
      });
      NumberFormats.Add("mmss.0", new ExcelNumberFormatXml(NameSpaceManager, true)
      {
        NumFmtId = 47,
        Format = "mmss.0"
      });
      NumberFormats.Add("##0.0", new ExcelNumberFormatXml(NameSpaceManager, true)
      {
        NumFmtId = 48,
        Format = "##0.0"
      });
      NumberFormats.Add("@", new ExcelNumberFormatXml(NameSpaceManager, true)
      {
        NumFmtId = 49,
        Format = "@"
      });
      NumberFormats.NextId = 164;
    }

    internal override XmlNode CreateXmlNode(XmlNode topNode)
    {
      this.TopNode = topNode;
      this.SetXmlNodeString("@numFmtId", this.NumFmtId.ToString());
      this.SetXmlNodeString("@formatCode", this.Format);
      return this.TopNode;
    }

    internal ExcelNumberFormatXml.ExcelFormatTranslator FormatTranslator
    {
      get
      {
        if (this._translator == null)
          this._translator = new ExcelNumberFormatXml.ExcelFormatTranslator(this.Format, this.NumFmtId);
        return this._translator;
      }
    }

    internal enum eFormatType
    {
      Unknown,
      Number,
      DateTime,
    }

    internal class ExcelFormatTranslator
    {
      private CultureInfo _ci;

      internal ExcelFormatTranslator(string format, int numFmtID)
      {
        if (numFmtID == 14)
        {
          this.NetFormat = this.NetFormatForWidth = "d";
          this.NetTextFormat = this.NetTextFormatForWidth = "";
          this.DataType = ExcelNumberFormatXml.eFormatType.DateTime;
        }
        else if (format.ToLower() == "general")
        {
          this.NetFormat = this.NetFormatForWidth = "0.#####";
          this.NetTextFormat = this.NetTextFormatForWidth = "";
          this.DataType = ExcelNumberFormatXml.eFormatType.Number;
        }
        else
        {
          this.ToNetFormat(format, false);
          this.ToNetFormat(format, true);
        }
      }

      internal string NetTextFormat { get; private set; }

      internal string NetFormat { get; private set; }

      internal CultureInfo Culture
      {
        get => this._ci == null ? CultureInfo.CurrentCulture : this._ci;
        private set => this._ci = value;
      }

      internal ExcelNumberFormatXml.eFormatType DataType { get; private set; }

      internal string NetTextFormatForWidth { get; private set; }

      internal string NetFormatForWidth { get; private set; }

      internal string FractionFormat { get; private set; }

      private void ToNetFormat(string ExcelFormat, bool forColWidth)
      {
        this.DataType = ExcelNumberFormatXml.eFormatType.Unknown;
        int num1 = 0;
        bool flag1 = false;
        bool flag2 = false;
        string input = "";
        bool flag3 = false;
        bool flag4 = false;
        bool flag5 = false;
        bool flag6 = false;
        string str1 = "";
        bool flag7 = ExcelFormat.Contains("AM/PM");
        StringBuilder stringBuilder = new StringBuilder();
        this.Culture = (CultureInfo) null;
        string str2 = "";
        string str3 = "";
        if (flag7)
        {
          ExcelFormat = Regex.Replace(ExcelFormat, "AM/PM", "");
          this.DataType = ExcelNumberFormatXml.eFormatType.DateTime;
        }
        for (int index1 = 0; index1 < ExcelFormat.Length; ++index1)
        {
          char ch1 = ExcelFormat[index1];
          if (ch1 == '"')
            flag1 = !flag1;
          else if (flag6)
            flag6 = false;
          else if (flag1 && !flag2)
            stringBuilder.Append(ch1);
          else if (flag2)
          {
            if (ch1 == ']')
            {
              flag2 = false;
              if (input[0] == '$')
              {
                string[] strArray = Regex.Split(input, "-");
                if (strArray[0].Length > 1)
                  stringBuilder.Append("\"" + strArray[0].Substring(1, strArray[0].Length - 1) + "\"");
                if (strArray.Length > 1)
                {
                  if (strArray[1].ToLower() == "f800")
                    str1 = "D";
                  else if (strArray[1].ToLower() == "f400")
                  {
                    str1 = "T";
                  }
                  else
                  {
                    int num2 = int.Parse(strArray[1], NumberStyles.HexNumber);
                    try
                    {
                      this.Culture = CultureInfo.GetCultureInfo(num2 & (int) ushort.MaxValue);
                    }
                    catch
                    {
                      this.Culture = (CultureInfo) null;
                    }
                  }
                }
              }
            }
            else
              input += (string) (object) ch1;
          }
          else if (flag5)
          {
            if (forColWidth)
              stringBuilder.AppendFormat("\"{0}\"", (object) ch1);
            flag5 = false;
          }
          else if (ch1 == ';')
          {
            ++num1;
            if (this.DataType == ExcelNumberFormatXml.eFormatType.DateTime || num1 == 3)
            {
              str2 = stringBuilder.ToString();
              stringBuilder = new StringBuilder();
            }
            else
              stringBuilder.Append(ch1);
          }
          else
          {
            char ch2 = ch1.ToString().ToLower()[0];
            if (this.DataType == ExcelNumberFormatXml.eFormatType.Unknown)
            {
              if (ch1 == '0' || ch1 == '#' || ch1 == '.')
                this.DataType = ExcelNumberFormatXml.eFormatType.Number;
              else if (ch2 == 'y' || ch2 == 'm' || ch2 == 'd' || ch2 == 'h' || ch2 == 'm' || ch2 == 's')
                this.DataType = ExcelNumberFormatXml.eFormatType.DateTime;
            }
            if (flag3)
            {
              stringBuilder.Append(ch1);
              flag3 = false;
            }
            else
            {
              switch (ch1)
              {
                case '#':
                case '%':
                case ',':
                case '.':
                case '0':
label_42:
                  stringBuilder.Append(ch1);
                  continue;
                case '[':
                  input = "";
                  flag2 = true;
                  continue;
                case '\\':
                  flag3 = true;
                  continue;
                default:
                  switch (ch2)
                  {
                    case 'd':
                    case 's':
                      goto label_42;
                    case 'h':
                      if (flag7)
                        stringBuilder.Append('h');
                      else
                        stringBuilder.Append('H');
                      flag4 = true;
                      continue;
                    case 'm':
                      if (flag4)
                      {
                        stringBuilder.Append('m');
                        continue;
                      }
                      stringBuilder.Append('M');
                      continue;
                    default:
                      switch (ch1)
                      {
                        case '*':
                          flag6 = true;
                          continue;
                        case '/':
                          if (this.DataType == ExcelNumberFormatXml.eFormatType.Number)
                          {
                            int length = stringBuilder.Length;
                            int index2 = index1 - 1;
                            while (index2 >= 0 && (ExcelFormat[index2] == '?' || ExcelFormat[index2] == '#' || ExcelFormat[index2] == '0'))
                              --index2;
                            if (index2 > 0)
                              stringBuilder.Remove(stringBuilder.Length - (index1 - index2 - 1), index1 - index2 - 1);
                            int index3 = index1 + 1;
                            while (index3 < ExcelFormat.Length && (ExcelFormat[index3] == '?' || ExcelFormat[index3] == '#' || ExcelFormat[index3] >= '0' && ExcelFormat[index3] <= '9'))
                              ++index3;
                            index1 = index3;
                            if (this.FractionFormat != "")
                              this.FractionFormat = ExcelFormat.Substring(index2 + 1, index3 - index2 - 1);
                            stringBuilder.Append('?');
                            continue;
                          }
                          stringBuilder.Append('/');
                          continue;
                        case '?':
                          stringBuilder.Append(' ');
                          continue;
                        case '@':
                          stringBuilder.Append("{0}");
                          continue;
                        case '_':
                          flag5 = true;
                          continue;
                        default:
                          stringBuilder.Append(ch1);
                          continue;
                      }
                  }
              }
            }
          }
        }
        if (flag7)
          str2 += "tt";
        if (str2 == "")
          str2 = stringBuilder.ToString();
        else
          str3 = stringBuilder.ToString();
        if (str1 != "")
          str2 = str1;
        if (forColWidth)
        {
          this.NetFormatForWidth = str2;
          this.NetTextFormatForWidth = str3;
        }
        else
        {
          this.NetFormat = str2;
          this.NetTextFormat = str3;
        }
        if (this.Culture != null)
          return;
        this.Culture = CultureInfo.CurrentCulture;
      }

      internal string FormatFraction(double d)
      {
        int num1 = (int) d;
        string[] strArray = this.FractionFormat.Split('/');
        int result;
        if (!int.TryParse(strArray[1], out result))
          result = 0;
        if (d == 0.0 || double.IsNaN(d))
          return strArray[0].Trim() == "" && strArray[1].Trim() == "" ? new string(' ', this.FractionFormat.Length) : 0.ToString(strArray[0]) + "/" + 1.ToString(strArray[0]);
        int length = strArray[1].Length;
        string str = d < 0.0 ? "-" : "";
        int num2;
        int num3;
        if (result == 0)
        {
          List<double> doubleList1 = new List<double>()
          {
            1.0,
            0.0
          };
          List<double> doubleList2 = new List<double>()
          {
            0.0,
            1.0
          };
          if (length < 1 && length > 12)
            throw new ArgumentException("Number of digits out of range (1-12)");
          int num4 = 0;
          for (int y = 0; y < length; ++y)
            num4 += 9 * (int) Math.Pow(10.0, (double) y);
          double d1 = 1.0 / (Math.Abs(d) - (double) num1);
          double num5 = double.NaN;
          int index1 = 2;
          int index2 = 1;
          while (true)
          {
            ++index2;
            double num6 = Math.Floor(d1);
            doubleList1.Add(num6 * doubleList1[index2 - 1] + doubleList1[index2 - 2]);
            if (doubleList1[index2] <= (double) num4)
            {
              doubleList2.Add(num6 * doubleList2[index2 - 1] + doubleList2[index2 - 2]);
              double num7 = doubleList1[index2] / doubleList2[index2];
              if (doubleList2[index2] <= (double) num4)
              {
                index1 = index2;
                if (num7 != num5 && num7 != d)
                {
                  num5 = num7;
                  d1 = 1.0 / (d1 - num6);
                }
                else
                  break;
              }
              else
                break;
            }
            else
              break;
          }
          num2 = (int) doubleList1[index1];
          num3 = (int) doubleList2[index1];
        }
        else
        {
          num2 = (int) Math.Round((d - (double) num1) / (1.0 / (double) result), 0);
          num3 = result;
        }
        if (num2 == num3 || num2 == 0)
        {
          if (num2 == num3)
            ++num1;
          return str + num1.ToString(this.NetFormat).Replace("?", new string(' ', this.FractionFormat.Length));
        }
        return num1 == 0 ? str + this.FmtInt((double) num2, strArray[0]) + "/" + this.FmtInt((double) num3, strArray[1]) : str + num1.ToString(this.NetFormat).Replace("?", this.FmtInt((double) num2, strArray[0]) + "/" + this.FmtInt((double) num3, strArray[1]));
      }

      private string FmtInt(double value, string format)
      {
        string str1 = value.ToString("#");
        string str2 = "";
        if (str1.Length < format.Length)
        {
          for (int index = format.Length - str1.Length - 1; index >= 0; --index)
          {
            if (format[index] == '?')
              str2 += " ";
            else if (format[index] == ' ')
              str2 += "0";
          }
        }
        return str2 + str1;
      }
    }
  }
}
