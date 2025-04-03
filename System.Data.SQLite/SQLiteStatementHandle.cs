// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteStatementHandle
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Runtime.InteropServices;
using System.Threading;

#nullable disable
namespace System.Data.SQLite
{
  internal sealed class SQLiteStatementHandle : CriticalHandle
  {
    private SQLiteConnectionHandle cnn;

    public static implicit operator IntPtr(SQLiteStatementHandle stmt)
    {
      return stmt != null ? stmt.handle : IntPtr.Zero;
    }

    internal SQLiteStatementHandle(SQLiteConnectionHandle cnn, IntPtr stmt)
      : this()
    {
      this.cnn = cnn;
      this.SetHandle(stmt);
    }

    private SQLiteStatementHandle()
      : base(IntPtr.Zero)
    {
    }

    protected override bool ReleaseHandle()
    {
      try
      {
        IntPtr stmt = Interlocked.Exchange(ref this.handle, IntPtr.Zero);
        if (stmt != IntPtr.Zero)
          SQLiteBase.FinalizeStatement(this.cnn, stmt);
      }
      catch (SQLiteException ex)
      {
      }
      finally
      {
        this.SetHandleAsInvalid();
      }
      return true;
    }

    public override bool IsInvalid => this.handle == IntPtr.Zero;
  }
}
