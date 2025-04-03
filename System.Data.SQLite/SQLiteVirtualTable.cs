// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteVirtualTable
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

#nullable disable
namespace System.Data.SQLite
{
  public class SQLiteVirtualTable : ISQLiteNativeHandle, IDisposable
  {
    private const int ModuleNameIndex = 0;
    private const int DatabaseNameIndex = 1;
    private const int TableNameIndex = 2;
    private string[] arguments;
    private SQLiteIndex index;
    private IntPtr nativeHandle;
    private bool disposed;

    public SQLiteVirtualTable(string[] arguments) => this.arguments = arguments;

    public virtual string[] Arguments
    {
      get
      {
        this.CheckDisposed();
        return this.arguments;
      }
    }

    public virtual string ModuleName
    {
      get
      {
        this.CheckDisposed();
        string[] arguments = this.Arguments;
        return arguments != null && arguments.Length > 0 ? arguments[0] : (string) null;
      }
    }

    public virtual string DatabaseName
    {
      get
      {
        this.CheckDisposed();
        string[] arguments = this.Arguments;
        return arguments != null && arguments.Length > 1 ? arguments[1] : (string) null;
      }
    }

    public virtual string TableName
    {
      get
      {
        this.CheckDisposed();
        string[] arguments = this.Arguments;
        return arguments != null && arguments.Length > 2 ? arguments[2] : (string) null;
      }
    }

    public virtual SQLiteIndex Index
    {
      get
      {
        this.CheckDisposed();
        return this.index;
      }
    }

    public virtual bool BestIndex(SQLiteIndex index)
    {
      this.CheckDisposed();
      this.index = index;
      return true;
    }

    public virtual bool Rename(string name)
    {
      this.CheckDisposed();
      if (this.arguments == null || this.arguments.Length <= 2)
        return false;
      this.arguments[2] = name;
      return true;
    }

    public virtual IntPtr NativeHandle
    {
      get
      {
        this.CheckDisposed();
        return this.nativeHandle;
      }
      internal set => this.nativeHandle = value;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteVirtualTable).Name);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      this.disposed = true;
    }

    ~SQLiteVirtualTable() => this.Dispose(false);
  }
}
