// Decompiled with JetBrains decompiler
// Type: Excel.Core.FormatReader
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

#nullable disable
namespace Excel.Core
{
  public class FormatReader
  {
    private const char escapeChar = '\\';

    public string FormatString { get; set; }

    public bool IsDateFormatString()
    {
      char[] anyOf = new char[10]
      {
        'y',
        'm',
        'd',
        's',
        'h',
        'Y',
        'M',
        'D',
        'S',
        'H'
      };
      if (this.FormatString.IndexOfAny(anyOf) >= 0)
      {
        foreach (char dateChar in anyOf)
        {
          for (int pos = this.FormatString.IndexOf(dateChar); pos > -1; pos = this.FormatString.IndexOf(dateChar, pos + 1))
          {
            if (!this.IsSurroundedByBracket(dateChar, pos) && !this.IsPrecededByBackSlash(dateChar, pos) && !this.IsSurroundedByQuotes(dateChar, pos))
              return true;
          }
        }
      }
      return false;
    }

    private bool IsSurroundedByQuotes(char dateChar, int pos)
    {
      if (pos == this.FormatString.Length - 1)
        return false;
      int num1 = this.NumberOfUnescapedOccurances('"', this.FormatString.Substring(pos + 1));
      int num2 = this.NumberOfUnescapedOccurances('"', this.FormatString.Substring(0, pos));
      return num1 % 2 == 1 && num2 % 2 == 1;
    }

    private bool IsPrecededByBackSlash(char dateChar, int pos)
    {
      return pos != 0 && this.FormatString[pos - 1].CompareTo('\\') == 0;
    }

    private bool IsSurroundedByBracket(char dateChar, int pos)
    {
      if (pos == this.FormatString.Length - 1)
        return false;
      int num1 = this.NumberOfUnescapedOccurances('[', this.FormatString.Substring(0, pos)) - this.NumberOfUnescapedOccurances(']', this.FormatString.Substring(0, pos));
      int num2 = this.NumberOfUnescapedOccurances('[', this.FormatString.Substring(pos + 1));
      int num3 = this.NumberOfUnescapedOccurances(']', this.FormatString.Substring(pos + 1)) - num2;
      return num1 % 2 == 1 && num3 % 2 == 1;
    }

    private int NumberOfUnescapedOccurances(char value, string src)
    {
      int num = 0;
      char ch1 = char.MinValue;
      foreach (char ch2 in src)
      {
        if ((int) ch2 == (int) value && (ch1 == char.MinValue || ch1.CompareTo('\\') != 0))
        {
          ++num;
          ch1 = ch2;
        }
      }
      return num;
    }
  }
}
