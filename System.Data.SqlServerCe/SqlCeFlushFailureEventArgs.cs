// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.SqlCeFlushFailureEventArgs
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

#nullable disable
namespace System.Data.SqlServerCe
{
  public sealed class SqlCeFlushFailureEventArgs : EventArgs
  {
    private object src;
    private SqlCeErrorCollection errors;

    internal SqlCeFlushFailureEventArgs(int hr, IntPtr pError, object src)
    {
      this.src = src;
      this.errors = new SqlCeErrorCollection();
      this.errors.FillErrorInformation(hr, pError);
    }

    public SqlCeErrorCollection Errors => this.errors;

    public string Message => this.Errors[0].Message;

    public override string ToString() => this.Message;
  }
}
