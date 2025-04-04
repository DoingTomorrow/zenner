// Decompiled with JetBrains decompiler
// Type: Excel.Core.OpenXmlFormat.XlsxDimension
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using System;

#nullable disable
namespace Excel.Core.OpenXmlFormat
{
  internal class XlsxDimension
  {
    private int _FirstRow;
    private int _LastRow;
    private int _FirstCol;
    private int _LastCol;

    public XlsxDimension(string value) => this.ParseDimensions(value);

    public XlsxDimension(int rows, int cols)
    {
      this.FirstRow = 1;
      this.LastRow = rows;
      this.FirstCol = 1;
      this.LastCol = cols;
    }

    public int FirstRow
    {
      get => this._FirstRow;
      set => this._FirstRow = value;
    }

    public int LastRow
    {
      get => this._LastRow;
      set => this._LastRow = value;
    }

    public int FirstCol
    {
      get => this._FirstCol;
      set => this._FirstCol = value;
    }

    public int LastCol
    {
      get => this._LastCol;
      set => this._LastCol = value;
    }

    public void ParseDimensions(string value)
    {
      string[] strArray = value.Split(':');
      int val1;
      int val2;
      XlsxDimension.XlsxDim(strArray[0], out val1, out val2);
      this.FirstCol = val1;
      this.FirstRow = val2;
      if (strArray.Length == 1)
      {
        this.LastCol = this.FirstCol;
        this.LastRow = this.FirstRow;
      }
      else
      {
        XlsxDimension.XlsxDim(strArray[1], out val1, out val2);
        this.LastCol = val1;
        this.LastRow = val2;
      }
    }

    public static void XlsxDim(string value, out int val1, out int val2)
    {
      int index1 = 0;
      val1 = 0;
      int[] numArray = new int[value.Length - 1];
      for (; index1 < value.Length && !char.IsDigit(value[index1]); ++index1)
        numArray[index1] = (int) value[index1] - 65 + 1;
      for (int index2 = 0; index2 < index1; ++index2)
        val1 += (int) ((double) numArray[index2] * Math.Pow(26.0, (double) (index1 - index2 - 1)));
      val2 = int.Parse(value.Substring(index1));
    }
  }
}
