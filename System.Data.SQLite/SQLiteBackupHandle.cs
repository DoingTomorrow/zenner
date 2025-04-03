// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteBackupHandle
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Runtime.InteropServices;
using System.Threading;

#nullable disable
namespace System.Data.SQLite
{
  internal sealed class SQLiteBackupHandle : CriticalHandle
  {
    private SQLiteConnectionHandle cnn;

    public static implicit operator IntPtr(SQLiteBackupHandle backup)
    {
      return backup != null ? backup.handle : IntPtr.Zero;
    }

    internal SQLiteBackupHandle(SQLiteConnectionHandle cnn, IntPtr backup)
      : this()
    {
      this.cnn = cnn;
      this.SetHandle(backup);
    }

    private SQLiteBackupHandle()
      : base(IntPtr.Zero)
    {
    }

    protected override bool ReleaseHandle()
    {
      try
      {
        IntPtr backup = Interlocked.Exchange(ref this.handle, IntPtr.Zero);
        if (backup != IntPtr.Zero)
          SQLiteBase.FinishBackup(this.cnn, backup);
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
