// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.SqlCeException
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

using System.Text;

#nullable disable
namespace System.Data.SqlServerCe
{
  [Serializable]
  public class SqlCeException : SystemException
  {
    internal SqlCeErrorCollection errors;

    internal SqlCeException()
      : this(string.Empty)
    {
    }

    private SqlCeException(string msg)
      : base(msg)
    {
    }

    private SqlCeException(string msg, Exception inner)
      : base(msg, inner)
    {
    }

    private static string BuildExceptionMessage(SqlCeErrorCollection errors)
    {
      return errors != null && errors.Count > 0 ? errors[0].Message : string.Empty;
    }

    internal static SqlCeException FillErrorInformation(int hr, IntPtr pIError)
    {
      SqlCeErrorCollection errors = new SqlCeErrorCollection();
      errors.FillErrorInformation(hr, pIError);
      return SqlCeException.CreateException(errors);
    }

    internal static SqlCeException FillErrorCollection(int hr, IntPtr pISSCEErrors)
    {
      SqlCeErrorCollection errors = new SqlCeErrorCollection();
      errors.FillErrorCollection(hr, pISSCEErrors);
      return SqlCeException.CreateException(errors);
    }

    internal static SqlCeException CreateException(SqlCeErrorCollection errors)
    {
      SqlCeException exception;
      switch (errors[0].NativeError)
      {
        case 25090:
          exception = (SqlCeException) new SqlCeLockTimeoutException();
          break;
        case 25126:
          exception = (SqlCeException) new SqlCeTransactionInProgressException();
          break;
        case 25138:
          exception = (SqlCeException) new SqlCeInvalidDatabaseFormatException();
          break;
        default:
          exception = new SqlCeException(SqlCeException.BuildExceptionMessage(errors));
          break;
      }
      exception.errors = errors;
      return exception;
    }

    internal static SqlCeException CreateException(string message) => new SqlCeException(message);

    internal static SqlCeException CreateException(string message, Exception inner)
    {
      return new SqlCeException(message, inner);
    }

    public SqlCeErrorCollection Errors
    {
      get
      {
        if (this.errors == null)
          this.errors = new SqlCeErrorCollection();
        return this.errors;
      }
    }

    public new int HResult => this.Errors.Count > 0 ? this.Errors[0].HResult : -1;

    public int NativeError => this.Errors.Count > 0 ? this.Errors[0].NativeError : -1;

    public override string Message
    {
      get
      {
        return !string.IsNullOrEmpty(SqlCeException.BuildExceptionMessage(this.Errors)) ? SqlCeException.BuildExceptionMessage(this.Errors) : base.Message;
      }
    }

    public override string Source => this.Errors.Count > 0 ? this.Errors[0].Source : string.Empty;

    public override string ToString()
    {
      if (this.Errors.Count == 0)
        return this.Message;
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < this.Errors.Count; ++index)
      {
        if (index > 0)
          stringBuilder.Append("; ");
        stringBuilder.Append(this.Errors[index].Message);
      }
      return stringBuilder.ToString();
    }
  }
}
