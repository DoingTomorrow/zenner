// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteConnectionHandle
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Runtime.InteropServices;
using System.Threading;

#nullable disable
namespace System.Data.SQLite
{
  internal sealed class SQLiteConnectionHandle : CriticalHandle
  {
    private bool ownHandle;

    public static implicit operator IntPtr(SQLiteConnectionHandle db)
    {
      return db != null ? db.handle : IntPtr.Zero;
    }

    internal SQLiteConnectionHandle(IntPtr db, bool ownHandle)
      : this(ownHandle)
    {
      this.ownHandle = ownHandle;
      this.SetHandle(db);
    }

    private SQLiteConnectionHandle(bool ownHandle)
      : base(IntPtr.Zero)
    {
    }

    protected override bool ReleaseHandle()
    {
      if (!this.ownHandle)
        return true;
      try
      {
        IntPtr db = Interlocked.Exchange(ref this.handle, IntPtr.Zero);
        if (db != IntPtr.Zero)
          SQLiteBase.CloseConnection(this, db);
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

    public bool OwnHandle => this.ownHandle;

    public override bool IsInvalid => this.handle == IntPtr.Zero;
  }
}
