// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.SqlCeError
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace System.Data.SqlServerCe
{
  public sealed class SqlCeError
  {
    private int hResult;
    private string message;
    private string formattedMessage;
    private int nativeError;
    private string source;
    private int[] numericErrorParameters = new int[3];
    private string[] errorParameters = new string[3]
    {
      string.Empty,
      string.Empty,
      string.Empty
    };
    internal static Regex ParamInfoRegex = new Regex("(?<=[\\[])[^\\[\\]]+(?=[\\]][\\s]*$)", RegexOptions.IgnoreCase);

    internal SqlCeError(
      int hResult,
      string message,
      int nativeError,
      string source,
      int numericParameter1,
      int numericParameter2,
      int numericParameter3,
      string errorParameter1,
      string errorParameter2,
      string errorParameter3)
    {
      this.hResult = hResult;
      this.message = message;
      this.nativeError = nativeError;
      this.source = source;
      this.numericErrorParameters[0] = numericParameter1;
      this.numericErrorParameters[1] = numericParameter2;
      this.numericErrorParameters[2] = numericParameter3;
      this.errorParameters[0] = errorParameter1 != null ? errorParameter1 : string.Empty;
      this.errorParameters[1] = errorParameter2 != null ? errorParameter2 : string.Empty;
      if (errorParameter3 == null)
        this.errorParameters[2] = string.Empty;
      else
        this.errorParameters[2] = errorParameter3;
    }

    public int HResult => this.hResult;

    public string Message
    {
      get
      {
        if (this.formattedMessage != null)
          return this.formattedMessage;
        try
        {
          this.formattedMessage = this.FormatErrorMessage(this);
          return this.formattedMessage;
        }
        catch (Exception ex)
        {
          if (!ADP.IsCatchableExceptionType(ex))
            throw ex;
          return this.message != null ? this.message : string.Empty;
        }
      }
    }

    public int NativeError => this.nativeError;

    public string Source => this.source == null ? string.Empty : this.source;

    public int[] NumericErrorParameters => this.numericErrorParameters;

    public string[] ErrorParameters => this.errorParameters;

    public override string ToString() => typeof (SqlCeError).ToString() + ": " + this.Message;

    internal static SqlCeError GetDefaultError(int hr)
    {
      return new SqlCeError(hr, Res.GetString("ADP_NoErrorInfoPresent"), -1, string.Empty, 0, 0, 0, string.Empty, string.Empty, string.Empty);
    }

    private string FormatErrorMessage(SqlCeError err)
    {
      string input = err.message;
      string[] strArray = new string[6]
      {
        "",
        "",
        "",
        "",
        "",
        ""
      };
      Match match;
      if ((match = SqlCeError.ParamInfoRegex.Match(input)).Success)
      {
        string str = match.Value;
        int startIndex = 0;
        for (int index = 0; index < 6; ++index)
        {
          int num = str.IndexOf(',', startIndex);
          if (-1 != num)
          {
            strArray[index] = str.Substring(startIndex, num - startIndex);
            startIndex = num + 1;
          }
          else
            strArray[index] = str.Substring(startIndex);
        }
        input = input.Substring(0, match.Index - 1).Trim();
      }
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = true;
      for (int index = 0; index < 3; ++index)
      {
        int numericErrorParameter = err.NumericErrorParameters[index];
        if (strArray[index].Trim().Length > 0)
        {
          if (!flag)
            stringBuilder.Append(",");
          stringBuilder.Append(strArray[index]);
          stringBuilder.Append(" = ");
          stringBuilder.Append(numericErrorParameter);
          flag = false;
        }
        else if (numericErrorParameter != 0)
        {
          if (!flag)
            stringBuilder.Append(",");
          stringBuilder.Append(numericErrorParameter);
          flag = false;
        }
      }
      for (int index = 0; index < 3; ++index)
      {
        string errorParameter = err.ErrorParameters[index];
        if (strArray[index + 3].Trim().Length > 0)
        {
          if (!flag)
            stringBuilder.Append(",");
          stringBuilder.Append(strArray[index + 3]);
          stringBuilder.Append(" = ");
          stringBuilder.Append(errorParameter);
          flag = false;
        }
        else if (errorParameter.Length > 0)
        {
          if (!flag)
            stringBuilder.Append(",");
          stringBuilder.Append(errorParameter);
          flag = false;
        }
      }
      if (stringBuilder.ToString().Length <= 0)
        return input;
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0} [ {1} ]", (object) input, (object) stringBuilder.ToString());
    }
  }
}
