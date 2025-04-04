// Decompiled with JetBrains decompiler
// Type: SQLite.PreparedSqlLiteInsertCommand
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;

#nullable disable
namespace SQLite
{
  public class PreparedSqlLiteInsertCommand : IDisposable
  {
    internal static readonly IntPtr NullStatement = new IntPtr();

    public bool Initialized { get; set; }

    protected SQLiteConnection Connection { get; set; }

    public string CommandText { get; set; }

    protected IntPtr Statement { get; set; }

    internal PreparedSqlLiteInsertCommand(SQLiteConnection conn) => this.Connection = conn;

    public int ExecuteNonQuery(object[] source)
    {
      if (!this.Connection.Trace)
        ;
      if (!this.Initialized)
      {
        this.Statement = this.Prepare();
        this.Initialized = true;
      }
      if (source != null)
      {
        for (int index = 0; index < source.Length; ++index)
          SQLiteCommand.BindParameter(this.Statement, index + 1, source[index], this.Connection.StoreDateTimeAsTicks);
      }
      SQLite3.Result r = SQLite3.Step(this.Statement);
      int num1;
      switch (r)
      {
        case SQLite3.Result.Error:
          string errmsg = SQLite3.GetErrmsg(this.Connection.Handle);
          int num2 = (int) SQLite3.Reset(this.Statement);
          throw SQLiteException.New(r, errmsg);
        case SQLite3.Result.Constraint:
          num1 = SQLite3.ExtendedErrCode(this.Connection.Handle) == SQLite3.ExtendedResult.ConstraintNotNull ? 1 : 0;
          break;
        case SQLite3.Result.Done:
          int num3 = SQLite3.Changes(this.Connection.Handle);
          int num4 = (int) SQLite3.Reset(this.Statement);
          return num3;
        default:
          num1 = 0;
          break;
      }
      if (num1 != 0)
      {
        int num5 = (int) SQLite3.Reset(this.Statement);
        throw NotNullConstraintViolationException.New(r, SQLite3.GetErrmsg(this.Connection.Handle));
      }
      int num6 = (int) SQLite3.Reset(this.Statement);
      throw SQLiteException.New(r, r.ToString());
    }

    protected virtual IntPtr Prepare()
    {
      return SQLite3.Prepare2(this.Connection.Handle, this.CommandText);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void Dispose(bool disposing)
    {
      if (!(this.Statement != PreparedSqlLiteInsertCommand.NullStatement))
        return;
      try
      {
        int num = (int) SQLite3.Finalize(this.Statement);
      }
      finally
      {
        this.Statement = PreparedSqlLiteInsertCommand.NullStatement;
        this.Connection = (SQLiteConnection) null;
      }
    }

    ~PreparedSqlLiteInsertCommand() => this.Dispose(false);
  }
}
