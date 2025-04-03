// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteContext
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

#nullable disable
namespace System.Data.SQLite
{
  public sealed class SQLiteContext : ISQLiteNativeHandle
  {
    private IntPtr pContext;

    internal SQLiteContext(IntPtr pContext) => this.pContext = pContext;

    public IntPtr NativeHandle => this.pContext;

    public void SetNull()
    {
      if (this.pContext == IntPtr.Zero)
        throw new InvalidOperationException();
      UnsafeNativeMethods.sqlite3_result_null(this.pContext);
    }

    public void SetDouble(double value)
    {
      if (this.pContext == IntPtr.Zero)
        throw new InvalidOperationException();
      UnsafeNativeMethods.sqlite3_result_double(this.pContext, value);
    }

    public void SetInt(int value)
    {
      if (this.pContext == IntPtr.Zero)
        throw new InvalidOperationException();
      UnsafeNativeMethods.sqlite3_result_int(this.pContext, value);
    }

    public void SetInt64(long value)
    {
      if (this.pContext == IntPtr.Zero)
        throw new InvalidOperationException();
      UnsafeNativeMethods.sqlite3_result_int64(this.pContext, value);
    }

    public void SetString(string value)
    {
      if (this.pContext == IntPtr.Zero)
        throw new InvalidOperationException();
      byte[] utf8BytesFromString = SQLiteString.GetUtf8BytesFromString(value);
      if (utf8BytesFromString == null)
        throw new ArgumentNullException(nameof (value));
      UnsafeNativeMethods.sqlite3_result_text(this.pContext, utf8BytesFromString, utf8BytesFromString.Length, (IntPtr) -1);
    }

    public void SetError(string value)
    {
      if (this.pContext == IntPtr.Zero)
        throw new InvalidOperationException();
      byte[] utf8BytesFromString = SQLiteString.GetUtf8BytesFromString(value);
      if (utf8BytesFromString == null)
        throw new ArgumentNullException(nameof (value));
      UnsafeNativeMethods.sqlite3_result_error(this.pContext, utf8BytesFromString, utf8BytesFromString.Length);
    }

    public void SetErrorCode(SQLiteErrorCode value)
    {
      if (this.pContext == IntPtr.Zero)
        throw new InvalidOperationException();
      UnsafeNativeMethods.sqlite3_result_error_code(this.pContext, value);
    }

    public void SetErrorTooBig()
    {
      if (this.pContext == IntPtr.Zero)
        throw new InvalidOperationException();
      UnsafeNativeMethods.sqlite3_result_error_toobig(this.pContext);
    }

    public void SetErrorNoMemory()
    {
      if (this.pContext == IntPtr.Zero)
        throw new InvalidOperationException();
      UnsafeNativeMethods.sqlite3_result_error_nomem(this.pContext);
    }

    public void SetBlob(byte[] value)
    {
      if (this.pContext == IntPtr.Zero)
        throw new InvalidOperationException();
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      UnsafeNativeMethods.sqlite3_result_blob(this.pContext, value, value.Length, (IntPtr) -1);
    }

    public void SetZeroBlob(int value)
    {
      if (this.pContext == IntPtr.Zero)
        throw new InvalidOperationException();
      UnsafeNativeMethods.sqlite3_result_zeroblob(this.pContext, value);
    }

    public void SetValue(SQLiteValue value)
    {
      if (this.pContext == IntPtr.Zero)
        throw new InvalidOperationException();
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      UnsafeNativeMethods.sqlite3_result_value(this.pContext, value.NativeHandle);
    }
  }
}
