// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteValue
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

#nullable disable
namespace System.Data.SQLite
{
  public sealed class SQLiteValue : ISQLiteNativeHandle
  {
    private IntPtr pValue;
    private bool persisted;
    private object value;

    private SQLiteValue(IntPtr pValue) => this.pValue = pValue;

    private void PreventNativeAccess() => this.pValue = IntPtr.Zero;

    internal static SQLiteValue[] ArrayFromSizeAndIntPtr(int argc, IntPtr argv)
    {
      if (argc < 0)
        return (SQLiteValue[]) null;
      if (argv == IntPtr.Zero)
        return (SQLiteValue[]) null;
      SQLiteValue[] sqLiteValueArray = new SQLiteValue[argc];
      int index = 0;
      int offset = 0;
      while (index < sqLiteValueArray.Length)
      {
        IntPtr pValue = SQLiteMarshal.ReadIntPtr(argv, offset);
        sqLiteValueArray[index] = pValue != IntPtr.Zero ? new SQLiteValue(pValue) : (SQLiteValue) null;
        ++index;
        offset += IntPtr.Size;
      }
      return sqLiteValueArray;
    }

    public IntPtr NativeHandle => this.pValue;

    public bool Persisted => this.persisted;

    public object Value
    {
      get
      {
        if (!this.persisted)
          throw new InvalidOperationException("value was not persisted");
        return this.value;
      }
    }

    public TypeAffinity GetTypeAffinity()
    {
      return this.pValue == IntPtr.Zero ? TypeAffinity.None : UnsafeNativeMethods.sqlite3_value_type(this.pValue);
    }

    public int GetBytes()
    {
      return this.pValue == IntPtr.Zero ? 0 : UnsafeNativeMethods.sqlite3_value_bytes(this.pValue);
    }

    public int GetInt()
    {
      return this.pValue == IntPtr.Zero ? 0 : UnsafeNativeMethods.sqlite3_value_int(this.pValue);
    }

    public long GetInt64()
    {
      return this.pValue == IntPtr.Zero ? 0L : UnsafeNativeMethods.sqlite3_value_int64(this.pValue);
    }

    public double GetDouble()
    {
      return this.pValue == IntPtr.Zero ? 0.0 : UnsafeNativeMethods.sqlite3_value_double(this.pValue);
    }

    public string GetString()
    {
      if (this.pValue == IntPtr.Zero)
        return (string) null;
      int len = 0;
      return SQLiteString.StringFromUtf8IntPtr(UnsafeNativeMethods.sqlite3_value_text_interop(this.pValue, ref len), len);
    }

    public byte[] GetBlob()
    {
      return this.pValue == IntPtr.Zero ? (byte[]) null : SQLiteBytes.FromIntPtr(UnsafeNativeMethods.sqlite3_value_blob(this.pValue), this.GetBytes());
    }

    public bool Persist()
    {
      switch (this.GetTypeAffinity())
      {
        case TypeAffinity.Uninitialized:
          this.value = (object) null;
          this.PreventNativeAccess();
          return this.persisted = true;
        case TypeAffinity.Int64:
          this.value = (object) this.GetInt64();
          this.PreventNativeAccess();
          return this.persisted = true;
        case TypeAffinity.Double:
          this.value = (object) this.GetDouble();
          this.PreventNativeAccess();
          return this.persisted = true;
        case TypeAffinity.Text:
          this.value = (object) this.GetString();
          this.PreventNativeAccess();
          return this.persisted = true;
        case TypeAffinity.Blob:
          this.value = (object) this.GetBytes();
          this.PreventNativeAccess();
          return this.persisted = true;
        case TypeAffinity.Null:
          this.value = (object) DBNull.Value;
          this.PreventNativeAccess();
          return this.persisted = true;
        default:
          return false;
      }
    }
  }
}
