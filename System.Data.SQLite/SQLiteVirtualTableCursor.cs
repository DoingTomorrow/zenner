// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteVirtualTableCursor
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

#nullable disable
namespace System.Data.SQLite
{
  public class SQLiteVirtualTableCursor : ISQLiteNativeHandle, IDisposable
  {
    protected static readonly int InvalidRowIndex;
    private int rowIndex;
    private SQLiteVirtualTable table;
    private int indexNumber;
    private string indexString;
    private SQLiteValue[] values;
    private IntPtr nativeHandle;
    private bool disposed;

    public SQLiteVirtualTableCursor(SQLiteVirtualTable table)
      : this()
    {
      this.table = table;
    }

    private SQLiteVirtualTableCursor() => this.rowIndex = SQLiteVirtualTableCursor.InvalidRowIndex;

    public virtual SQLiteVirtualTable Table
    {
      get
      {
        this.CheckDisposed();
        return this.table;
      }
    }

    public virtual int IndexNumber
    {
      get
      {
        this.CheckDisposed();
        return this.indexNumber;
      }
    }

    public virtual string IndexString
    {
      get
      {
        this.CheckDisposed();
        return this.indexString;
      }
    }

    public virtual SQLiteValue[] Values
    {
      get
      {
        this.CheckDisposed();
        return this.values;
      }
    }

    protected virtual int TryPersistValues(SQLiteValue[] values)
    {
      int num = 0;
      if (values != null)
      {
        foreach (SQLiteValue sqLiteValue in values)
        {
          if (sqLiteValue != null && sqLiteValue.Persist())
            ++num;
        }
      }
      return num;
    }

    public virtual void Filter(int indexNumber, string indexString, SQLiteValue[] values)
    {
      this.CheckDisposed();
      if (values != null && this.TryPersistValues(values) != values.Length)
        throw new SQLiteException("failed to persist one or more values");
      this.indexNumber = indexNumber;
      this.indexString = indexString;
      this.values = values;
    }

    public virtual int GetRowIndex() => this.rowIndex;

    public virtual void NextRowIndex() => ++this.rowIndex;

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
        throw new ObjectDisposedException(typeof (SQLiteVirtualTableCursor).Name);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      this.disposed = true;
    }

    ~SQLiteVirtualTableCursor() => this.Dispose(false);
  }
}
