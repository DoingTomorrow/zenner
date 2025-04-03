// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteModuleCommon
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Globalization;

#nullable disable
namespace System.Data.SQLite
{
  public class SQLiteModuleCommon : SQLiteModuleNoop
  {
    private static readonly string declareSql = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "CREATE TABLE {0}(x);", (object) typeof (SQLiteModuleCommon).Name);
    private bool objectIdentity;
    private bool disposed;

    public SQLiteModuleCommon(string name)
      : this(name, false)
    {
    }

    public SQLiteModuleCommon(string name, bool objectIdentity)
      : base(name)
    {
      this.objectIdentity = objectIdentity;
    }

    protected virtual string GetSqlForDeclareTable() => SQLiteModuleCommon.declareSql;

    protected virtual SQLiteErrorCode CursorTypeMismatchError(
      SQLiteVirtualTableCursor cursor,
      Type type)
    {
      if (type != (Type) null)
        this.SetCursorError(cursor, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "not a \"{0}\" cursor", (object) type));
      else
        this.SetCursorError(cursor, "cursor type mismatch");
      return SQLiteErrorCode.Error;
    }

    protected virtual string GetStringFromObject(SQLiteVirtualTableCursor cursor, object value)
    {
      if (value == null)
        return (string) null;
      return value is string ? (string) value : value.ToString();
    }

    protected virtual long MakeRowId(int rowIndex, int hashCode)
    {
      return (long) rowIndex << 32 | (long) (uint) hashCode;
    }

    protected virtual long GetRowIdFromObject(SQLiteVirtualTableCursor cursor, object value)
    {
      return this.MakeRowId(cursor != null ? cursor.GetRowIndex() : 0, SQLiteMarshal.GetHashCode(value, this.objectIdentity));
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteModuleCommon).Name);
    }

    protected override void Dispose(bool disposing)
    {
      try
      {
        int num = this.disposed ? 1 : 0;
      }
      finally
      {
        base.Dispose(disposing);
        this.disposed = true;
      }
    }
  }
}
