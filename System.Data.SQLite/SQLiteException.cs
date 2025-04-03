// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteException
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Data.Common;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;

#nullable disable
namespace System.Data.SQLite
{
  [Serializable]
  public sealed class SQLiteException : DbException, ISerializable
  {
    private SQLiteErrorCode _errorCode;

    private SQLiteException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this._errorCode = (SQLiteErrorCode) info.GetInt32("errorCode");
    }

    public SQLiteException(SQLiteErrorCode errorCode, string message)
      : base(SQLiteException.GetStockErrorMessage(errorCode, message))
    {
      this._errorCode = errorCode;
    }

    public SQLiteException(string message)
      : this(SQLiteErrorCode.Unknown, message)
    {
    }

    public SQLiteException()
    {
    }

    public SQLiteException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info?.AddValue("errorCode", (object) this._errorCode);
      base.GetObjectData(info, context);
    }

    public SQLiteErrorCode ResultCode => this._errorCode;

    public override int ErrorCode => (int) this._errorCode;

    private static string GetErrorString(SQLiteErrorCode errorCode)
    {
      return typeof (SQLite3).InvokeMember(nameof (GetErrorString), BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod, (Binder) null, (object) null, new object[1]
      {
        (object) errorCode
      }) as string;
    }

    private static string GetStockErrorMessage(SQLiteErrorCode errorCode, string message)
    {
      return HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "{0}{1}{2}", (object) SQLiteException.GetErrorString(errorCode), (object) Environment.NewLine, (object) message).Trim();
    }
  }
}
