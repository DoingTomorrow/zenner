// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteConvert
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace System.Data.SQLite
{
  public abstract class SQLiteConvert
  {
    private const DbType FallbackDefaultDbType = DbType.Object;
    private const string FullFormat = "yyyy-MM-ddTHH:mm:ss.fffffffK";
    private static readonly string FallbackDefaultTypeName = string.Empty;
    protected static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private static readonly double OleAutomationEpochAsJulianDay = 2415018.5;
    private static readonly long MinimumJd = SQLiteConvert.computeJD(DateTime.MinValue);
    private static readonly long MaximumJd = SQLiteConvert.computeJD(DateTime.MaxValue);
    private static string[] _datetimeFormats = new string[31]
    {
      "THHmmssK",
      "THHmmK",
      "HH:mm:ss.FFFFFFFK",
      "HH:mm:ssK",
      "HH:mmK",
      "yyyy-MM-dd HH:mm:ss.FFFFFFFK",
      "yyyy-MM-dd HH:mm:ssK",
      "yyyy-MM-dd HH:mmK",
      "yyyy-MM-ddTHH:mm:ss.FFFFFFFK",
      "yyyy-MM-ddTHH:mmK",
      "yyyy-MM-ddTHH:mm:ssK",
      "yyyyMMddHHmmssK",
      "yyyyMMddHHmmK",
      "yyyyMMddTHHmmssFFFFFFFK",
      "THHmmss",
      "THHmm",
      "HH:mm:ss.FFFFFFF",
      "HH:mm:ss",
      "HH:mm",
      "yyyy-MM-dd HH:mm:ss.FFFFFFF",
      "yyyy-MM-dd HH:mm:ss",
      "yyyy-MM-dd HH:mm",
      "yyyy-MM-ddTHH:mm:ss.FFFFFFF",
      "yyyy-MM-ddTHH:mm",
      "yyyy-MM-ddTHH:mm:ss",
      "yyyyMMddHHmmss",
      "yyyyMMddHHmm",
      "yyyyMMddTHHmmssFFFFFFF",
      "yyyy-MM-dd",
      "yyyyMMdd",
      "yy-MM-dd"
    };
    private static readonly string _datetimeFormatUtc = SQLiteConvert._datetimeFormats[5];
    private static readonly string _datetimeFormatLocal = SQLiteConvert._datetimeFormats[19];
    private static Encoding _utf8 = (Encoding) new UTF8Encoding();
    internal SQLiteDateFormats _datetimeFormat;
    internal DateTimeKind _datetimeKind;
    internal string _datetimeFormatString;
    private static Type[] _affinitytotype = new Type[8]
    {
      typeof (object),
      typeof (long),
      typeof (double),
      typeof (string),
      typeof (byte[]),
      typeof (object),
      typeof (DateTime),
      typeof (object)
    };
    private static DbType[] _typetodbtype = new DbType[19]
    {
      DbType.Object,
      DbType.Binary,
      DbType.Object,
      DbType.Boolean,
      DbType.SByte,
      DbType.SByte,
      DbType.Byte,
      DbType.Int16,
      DbType.UInt16,
      DbType.Int32,
      DbType.UInt32,
      DbType.Int64,
      DbType.UInt64,
      DbType.Single,
      DbType.Double,
      DbType.Decimal,
      DbType.DateTime,
      DbType.Object,
      DbType.String
    };
    private static int[] _dbtypetocolumnsize = new int[26]
    {
      int.MaxValue,
      int.MaxValue,
      1,
      1,
      8,
      8,
      8,
      8,
      8,
      16,
      2,
      4,
      8,
      int.MaxValue,
      1,
      4,
      int.MaxValue,
      8,
      2,
      4,
      8,
      8,
      int.MaxValue,
      int.MaxValue,
      int.MaxValue,
      int.MaxValue
    };
    private static object[] _dbtypetonumericprecision = new object[26]
    {
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) 3,
      (object) DBNull.Value,
      (object) 19,
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) 53,
      (object) 53,
      (object) DBNull.Value,
      (object) 5,
      (object) 10,
      (object) 19,
      (object) DBNull.Value,
      (object) 3,
      (object) 24,
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) 5,
      (object) 10,
      (object) 19,
      (object) 53,
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) DBNull.Value
    };
    private static object[] _dbtypetonumericscale = new object[26]
    {
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) 0,
      (object) DBNull.Value,
      (object) 4,
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) 0,
      (object) 0,
      (object) 0,
      (object) DBNull.Value,
      (object) 0,
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) 0,
      (object) 0,
      (object) 0,
      (object) 0,
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) DBNull.Value
    };
    private static Type[] _dbtypeToType = new Type[26]
    {
      typeof (string),
      typeof (byte[]),
      typeof (byte),
      typeof (bool),
      typeof (Decimal),
      typeof (DateTime),
      typeof (DateTime),
      typeof (Decimal),
      typeof (double),
      typeof (Guid),
      typeof (short),
      typeof (int),
      typeof (long),
      typeof (object),
      typeof (sbyte),
      typeof (float),
      typeof (string),
      typeof (DateTime),
      typeof (ushort),
      typeof (uint),
      typeof (ulong),
      typeof (double),
      typeof (string),
      typeof (string),
      typeof (string),
      typeof (string)
    };
    private static TypeAffinity[] _typecodeAffinities = new TypeAffinity[19]
    {
      TypeAffinity.Null,
      TypeAffinity.Blob,
      TypeAffinity.Null,
      TypeAffinity.Int64,
      TypeAffinity.Int64,
      TypeAffinity.Int64,
      TypeAffinity.Int64,
      TypeAffinity.Int64,
      TypeAffinity.Int64,
      TypeAffinity.Int64,
      TypeAffinity.Int64,
      TypeAffinity.Int64,
      TypeAffinity.Int64,
      TypeAffinity.Double,
      TypeAffinity.Double,
      TypeAffinity.Double,
      TypeAffinity.DateTime,
      TypeAffinity.Null,
      TypeAffinity.Text
    };
    private static object _syncRoot = new object();
    private static SQLiteDbTypeMap _typeNames = (SQLiteDbTypeMap) null;

    internal SQLiteConvert(SQLiteDateFormats fmt, DateTimeKind kind, string fmtString)
    {
      this._datetimeFormat = fmt;
      this._datetimeKind = kind;
      this._datetimeFormatString = fmtString;
    }

    public static byte[] ToUTF8(string sourceText)
    {
      if (sourceText == null)
        return (byte[]) null;
      byte[] bytes = new byte[SQLiteConvert._utf8.GetByteCount(sourceText) + 1];
      bytes[SQLiteConvert._utf8.GetBytes(sourceText, 0, sourceText.Length, bytes, 0)] = (byte) 0;
      return bytes;
    }

    public byte[] ToUTF8(DateTime dateTimeValue)
    {
      return SQLiteConvert.ToUTF8(this.ToString(dateTimeValue));
    }

    public virtual string ToString(IntPtr nativestring, int nativestringlen)
    {
      return SQLiteConvert.UTF8ToString(nativestring, nativestringlen);
    }

    public static string UTF8ToString(IntPtr nativestring, int nativestringlen)
    {
      if (nativestring == IntPtr.Zero || nativestringlen == 0)
        return string.Empty;
      if (nativestringlen < 0)
      {
        nativestringlen = 0;
        while (Marshal.ReadByte(nativestring, nativestringlen) != (byte) 0)
          ++nativestringlen;
        if (nativestringlen == 0)
          return string.Empty;
      }
      byte[] numArray = new byte[nativestringlen];
      Marshal.Copy(nativestring, numArray, 0, nativestringlen);
      return SQLiteConvert._utf8.GetString(numArray, 0, nativestringlen);
    }

    private static bool isValidJd(long jd)
    {
      return jd >= SQLiteConvert.MinimumJd && jd <= SQLiteConvert.MaximumJd;
    }

    private static long DoubleToJd(double julianDay) => (long) (julianDay * 86400000.0);

    private static double JdToDouble(long jd) => (double) jd / 86400000.0;

    private static DateTime computeYMD(long jd, DateTime? badValue)
    {
      if (!SQLiteConvert.isValidJd(jd))
        return badValue.HasValue ? badValue.Value : throw new ArgumentException("Not a supported Julian Day value.");
      int num1 = (int) ((jd + 43200000L) / 86400000L);
      int num2 = (int) (((double) num1 - 1867216.25) / 36524.25);
      int num3 = num1 + 1 + num2 - num2 / 4 + 1524;
      int num4 = (int) (((double) num3 - 122.1) / 365.25);
      int num5 = 36525 * num4 / 100;
      int num6 = (int) ((double) (num3 - num5) / 30.6001);
      int num7 = (int) (30.6001 * (double) num6);
      int day = num3 - num5 - num7;
      int month = num6 < 14 ? num6 - 1 : num6 - 13;
      int year = month > 2 ? num4 - 4716 : num4 - 4715;
      try
      {
        return new DateTime(year, month, day);
      }
      catch
      {
        if (badValue.HasValue)
          return badValue.Value;
        throw;
      }
    }

    private static DateTime computeHMS(long jd, DateTime? badValue)
    {
      if (!SQLiteConvert.isValidJd(jd))
        return badValue.HasValue ? badValue.Value : throw new ArgumentException("Not a supported Julian Day value.");
      Decimal num1 = (Decimal) (int) ((jd + 43200000L) % 86400000L) / 1000.0M;
      int num2 = (int) num1;
      int millisecond = (int) ((num1 - (Decimal) num2) * 1000.0M);
      Decimal num3 = num1 - (Decimal) num2;
      int hour = num2 / 3600;
      int num4 = num2 - hour * 3600;
      int minute = num4 / 60;
      int second = (int) (num3 + (Decimal) (num4 - minute * 60));
      try
      {
        DateTime minValue = DateTime.MinValue;
        return new DateTime(minValue.Year, minValue.Month, minValue.Day, hour, minute, second, millisecond);
      }
      catch
      {
        if (badValue.HasValue)
          return badValue.Value;
        throw;
      }
    }

    private static long computeJD(DateTime dateTime)
    {
      int year = dateTime.Year;
      int month = dateTime.Month;
      int day = dateTime.Day;
      if (month <= 2)
      {
        --year;
        month += 12;
      }
      int num1 = year / 100;
      int num2 = 2 - num1 + num1 / 4;
      return (long) (((double) (36525 * (year + 4716) / 100 + 306001 * (month + 1) / 10000 + day + num2) - 1524.5) * 86400000.0) + (long) (dateTime.Hour * 3600000 + dateTime.Minute * 60000 + dateTime.Second * 1000 + dateTime.Millisecond);
    }

    public DateTime ToDateTime(string dateText)
    {
      return SQLiteConvert.ToDateTime(dateText, this._datetimeFormat, this._datetimeKind, this._datetimeFormatString);
    }

    public static DateTime ToDateTime(
      string dateText,
      SQLiteDateFormats format,
      DateTimeKind kind,
      string formatString)
    {
      switch (format)
      {
        case SQLiteDateFormats.Ticks:
          return SQLiteConvert.TicksToDateTime(Convert.ToInt64(dateText, (IFormatProvider) CultureInfo.InvariantCulture), kind);
        case SQLiteDateFormats.JulianDay:
          return SQLiteConvert.ToDateTime(Convert.ToDouble(dateText, (IFormatProvider) CultureInfo.InvariantCulture), kind);
        case SQLiteDateFormats.UnixEpoch:
          return SQLiteConvert.UnixEpochToDateTime(Convert.ToInt64(dateText, (IFormatProvider) CultureInfo.InvariantCulture), kind);
        case SQLiteDateFormats.InvariantCulture:
          return formatString != null ? DateTime.SpecifyKind(DateTime.ParseExact(dateText, formatString, (IFormatProvider) DateTimeFormatInfo.InvariantInfo, kind == DateTimeKind.Utc ? DateTimeStyles.AdjustToUniversal : DateTimeStyles.None), kind) : DateTime.SpecifyKind(DateTime.Parse(dateText, (IFormatProvider) DateTimeFormatInfo.InvariantInfo, kind == DateTimeKind.Utc ? DateTimeStyles.AdjustToUniversal : DateTimeStyles.None), kind);
        case SQLiteDateFormats.CurrentCulture:
          return formatString != null ? DateTime.SpecifyKind(DateTime.ParseExact(dateText, formatString, (IFormatProvider) DateTimeFormatInfo.CurrentInfo, kind == DateTimeKind.Utc ? DateTimeStyles.AdjustToUniversal : DateTimeStyles.None), kind) : DateTime.SpecifyKind(DateTime.Parse(dateText, (IFormatProvider) DateTimeFormatInfo.CurrentInfo, kind == DateTimeKind.Utc ? DateTimeStyles.AdjustToUniversal : DateTimeStyles.None), kind);
        default:
          return formatString != null ? DateTime.SpecifyKind(DateTime.ParseExact(dateText, formatString, (IFormatProvider) DateTimeFormatInfo.InvariantInfo, kind == DateTimeKind.Utc ? DateTimeStyles.AdjustToUniversal : DateTimeStyles.None), kind) : DateTime.SpecifyKind(DateTime.ParseExact(dateText, SQLiteConvert._datetimeFormats, (IFormatProvider) DateTimeFormatInfo.InvariantInfo, kind == DateTimeKind.Utc ? DateTimeStyles.AdjustToUniversal : DateTimeStyles.None), kind);
      }
    }

    public DateTime ToDateTime(double julianDay)
    {
      return SQLiteConvert.ToDateTime(julianDay, this._datetimeKind);
    }

    public static DateTime ToDateTime(double julianDay, DateTimeKind kind)
    {
      long jd = SQLiteConvert.DoubleToJd(julianDay);
      DateTime ymd = SQLiteConvert.computeYMD(jd, new DateTime?());
      DateTime hms = SQLiteConvert.computeHMS(jd, new DateTime?());
      return new DateTime(ymd.Year, ymd.Month, ymd.Day, hms.Hour, hms.Minute, hms.Second, hms.Millisecond, kind);
    }

    internal static DateTime UnixEpochToDateTime(long seconds, DateTimeKind kind)
    {
      return DateTime.SpecifyKind(SQLiteConvert.UnixEpoch.AddSeconds((double) seconds), kind);
    }

    internal static DateTime TicksToDateTime(long ticks, DateTimeKind kind)
    {
      return new DateTime(ticks, kind);
    }

    public static double ToJulianDay(DateTime value)
    {
      return SQLiteConvert.JdToDouble(SQLiteConvert.computeJD(value));
    }

    public static long ToUnixEpoch(DateTime value)
    {
      return value.Subtract(SQLiteConvert.UnixEpoch).Ticks / 10000000L;
    }

    private static string GetDateTimeKindFormat(DateTimeKind kind, string formatString)
    {
      if (formatString != null)
        return formatString;
      return kind != DateTimeKind.Utc ? SQLiteConvert._datetimeFormatLocal : SQLiteConvert._datetimeFormatUtc;
    }

    public string ToString(DateTime dateValue)
    {
      return SQLiteConvert.ToString(dateValue, this._datetimeFormat, this._datetimeKind, this._datetimeFormatString);
    }

    public static string ToString(
      DateTime dateValue,
      SQLiteDateFormats format,
      DateTimeKind kind,
      string formatString)
    {
      switch (format)
      {
        case SQLiteDateFormats.Ticks:
          return dateValue.Ticks.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        case SQLiteDateFormats.JulianDay:
          return SQLiteConvert.ToJulianDay(dateValue).ToString((IFormatProvider) CultureInfo.InvariantCulture);
        case SQLiteDateFormats.UnixEpoch:
          return (dateValue.Subtract(SQLiteConvert.UnixEpoch).Ticks / 10000000L).ToString();
        case SQLiteDateFormats.InvariantCulture:
          return dateValue.ToString(formatString != null ? formatString : "yyyy-MM-ddTHH:mm:ss.fffffffK", (IFormatProvider) CultureInfo.InvariantCulture);
        case SQLiteDateFormats.CurrentCulture:
          return dateValue.ToString(formatString != null ? formatString : "yyyy-MM-ddTHH:mm:ss.fffffffK", (IFormatProvider) CultureInfo.CurrentCulture);
        default:
          return dateValue.Kind != DateTimeKind.Unspecified ? dateValue.ToString(SQLiteConvert.GetDateTimeKindFormat(dateValue.Kind, formatString), (IFormatProvider) CultureInfo.InvariantCulture) : DateTime.SpecifyKind(dateValue, kind).ToString(SQLiteConvert.GetDateTimeKindFormat(kind, formatString), (IFormatProvider) CultureInfo.InvariantCulture);
      }
    }

    internal DateTime ToDateTime(IntPtr ptr, int len) => this.ToDateTime(this.ToString(ptr, len));

    public static string[] Split(string source, char separator)
    {
      char[] anyOf1 = new char[2]{ '"', separator };
      char[] anyOf2 = new char[1]{ '"' };
      int startIndex = 0;
      List<string> stringList = new List<string>();
      while (source.Length > 0)
      {
        int num1 = source.IndexOfAny(anyOf1, startIndex);
        if (num1 != -1)
        {
          if ((int) source[num1] == (int) anyOf1[0])
          {
            int num2 = source.IndexOfAny(anyOf2, num1 + 1);
            if (num2 != -1)
              startIndex = num2 + 1;
            else
              break;
          }
          else
          {
            string str = source.Substring(0, num1).Trim();
            if (str.Length > 1 && (int) str[0] == (int) anyOf2[0] && (int) str[str.Length - 1] == (int) str[0])
              str = str.Substring(1, str.Length - 2);
            source = source.Substring(num1 + 1).Trim();
            if (str.Length > 0)
              stringList.Add(str);
            startIndex = 0;
          }
        }
        else
          break;
      }
      if (source.Length > 0)
      {
        string str = source.Trim();
        if (str.Length > 1 && (int) str[0] == (int) anyOf2[0] && (int) str[str.Length - 1] == (int) str[0])
          str = str.Substring(1, str.Length - 2);
        stringList.Add(str);
      }
      string[] array = new string[stringList.Count];
      stringList.CopyTo(array, 0);
      return array;
    }

    internal static string[] NewSplit(
      string value,
      char separator,
      bool keepQuote,
      ref string error)
    {
      if (separator == '\\' || separator == '"')
      {
        error = "separator character cannot be the escape or quote characters";
        return (string[]) null;
      }
      if (value == null)
      {
        error = "string value to split cannot be null";
        return (string[]) null;
      }
      int length = value.Length;
      if (length == 0)
        return new string[0];
      List<string> stringList = new List<string>();
      StringBuilder stringBuilder = new StringBuilder();
      int num = 0;
      bool flag1 = false;
      bool flag2 = false;
      while (num < length)
      {
        char ch = value[num++];
        if (flag1)
        {
          if (ch != '\\' && ch != '"' && (int) ch != (int) separator)
            stringBuilder.Append('\\');
          stringBuilder.Append(ch);
          flag1 = false;
        }
        else
        {
          switch (ch)
          {
            case '"':
              if (keepQuote)
                stringBuilder.Append(ch);
              flag2 = !flag2;
              continue;
            case '\\':
              flag1 = true;
              continue;
            default:
              if ((int) ch == (int) separator)
              {
                if (flag2)
                {
                  stringBuilder.Append(ch);
                  continue;
                }
                stringList.Add(stringBuilder.ToString());
                stringBuilder.Length = 0;
                continue;
              }
              stringBuilder.Append(ch);
              continue;
          }
        }
      }
      if (flag1 || flag2)
      {
        error = "unbalanced escape or quote character found";
        return (string[]) null;
      }
      if (stringBuilder.Length > 0)
        stringList.Add(stringBuilder.ToString());
      return stringList.ToArray();
    }

    public static string ToStringWithProvider(object obj, IFormatProvider provider)
    {
      switch (obj)
      {
        case null:
          return (string) null;
        case string _:
          return (string) obj;
        case IConvertible convertible:
          return convertible.ToString(provider);
        default:
          return obj.ToString();
      }
    }

    internal static bool ToBoolean(object obj, IFormatProvider provider, bool viaFramework)
    {
      if (obj == null)
        return false;
      TypeCode typeCode = Type.GetTypeCode(obj.GetType());
      switch (typeCode)
      {
        case TypeCode.Empty:
        case TypeCode.DBNull:
          return false;
        case TypeCode.Boolean:
          return (bool) obj;
        case TypeCode.Char:
          return (char) obj != char.MinValue;
        case TypeCode.SByte:
          return (sbyte) obj != (sbyte) 0;
        case TypeCode.Byte:
          return (byte) obj != (byte) 0;
        case TypeCode.Int16:
          return (short) obj != (short) 0;
        case TypeCode.UInt16:
          return (ushort) obj != (ushort) 0;
        case TypeCode.Int32:
          return (int) obj != 0;
        case TypeCode.UInt32:
          return (uint) obj != 0U;
        case TypeCode.Int64:
          return (long) obj != 0L;
        case TypeCode.UInt64:
          return (ulong) obj != 0UL;
        case TypeCode.Single:
          return (double) (float) obj != 0.0;
        case TypeCode.Double:
          return (double) obj != 0.0;
        case TypeCode.Decimal:
          return (Decimal) obj != 0M;
        case TypeCode.String:
          return !viaFramework ? SQLiteConvert.ToBoolean(SQLiteConvert.ToStringWithProvider(obj, provider)) : Convert.ToBoolean(obj, provider);
        default:
          throw new SQLiteException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Cannot convert type {0} to boolean", (object) typeCode));
      }
    }

    public static bool ToBoolean(object source)
    {
      return source is bool flag ? flag : SQLiteConvert.ToBoolean(SQLiteConvert.ToStringWithProvider(source, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static bool ToBoolean(string source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (string.Compare(source, 0, bool.TrueString, 0, source.Length, StringComparison.OrdinalIgnoreCase) == 0)
        return true;
      if (string.Compare(source, 0, bool.FalseString, 0, source.Length, StringComparison.OrdinalIgnoreCase) == 0)
        return false;
      switch (source.ToLower(CultureInfo.InvariantCulture))
      {
        case "y":
        case "yes":
        case "on":
        case "1":
          return true;
        case "n":
        case "no":
        case "off":
        case "0":
          return false;
        default:
          throw new ArgumentException(nameof (source));
      }
    }

    internal static Type SQLiteTypeToType(SQLiteType t)
    {
      return t.Type == DbType.Object ? SQLiteConvert._affinitytotype[(int) t.Affinity] : SQLiteConvert.DbTypeToType(t.Type);
    }

    internal static DbType TypeToDbType(Type typ)
    {
      TypeCode typeCode = Type.GetTypeCode(typ);
      if (typeCode != TypeCode.Object)
        return SQLiteConvert._typetodbtype[(int) typeCode];
      if (typ == typeof (byte[]))
        return DbType.Binary;
      return typ == typeof (Guid) ? DbType.Guid : DbType.String;
    }

    internal static int DbTypeToColumnSize(DbType typ)
    {
      return SQLiteConvert._dbtypetocolumnsize[(int) typ];
    }

    internal static object DbTypeToNumericPrecision(DbType typ)
    {
      return SQLiteConvert._dbtypetonumericprecision[(int) typ];
    }

    internal static object DbTypeToNumericScale(DbType typ)
    {
      return SQLiteConvert._dbtypetonumericscale[(int) typ];
    }

    private static string GetDefaultTypeName(SQLiteConnection connection)
    {
      if (((connection != null ? (long) connection.Flags : 0L) & 134217728L) == 134217728L)
        return SQLiteConvert.FallbackDefaultTypeName;
      string name = "Use_SQLiteConvert_DefaultTypeName";
      object obj = (object) null;
      string @default = (string) null;
      if (connection != null)
      {
        if (connection.TryGetCachedSetting(name, (object) @default, out obj))
          goto label_8;
      }
      try
      {
        obj = (object) UnsafeNativeMethods.GetSettingValue(name, @default) ?? (object) SQLiteConvert.FallbackDefaultTypeName;
      }
      finally
      {
        connection?.SetCachedSetting(name, obj);
      }
label_8:
      return SQLiteConvert.SettingValueToString(obj);
    }

    private static void DefaultTypeNameWarning(
      DbType dbType,
      SQLiteConnectionFlags flags,
      string typeName)
    {
      if ((flags & SQLiteConnectionFlags.TraceWarning) != SQLiteConnectionFlags.TraceWarning)
        return;
      Trace.WriteLine(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "WARNING: Type mapping failed, returning default name \"{0}\" for type {1}.", (object) typeName, (object) dbType));
    }

    private static void DefaultDbTypeWarning(
      string typeName,
      SQLiteConnectionFlags flags,
      DbType? dbType)
    {
      if (string.IsNullOrEmpty(typeName) || (flags & SQLiteConnectionFlags.TraceWarning) != SQLiteConnectionFlags.TraceWarning)
        return;
      Trace.WriteLine(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "WARNING: Type mapping failed, returning default type {0} for name \"{1}\".", (object) dbType, (object) typeName));
    }

    internal static string DbTypeToTypeName(
      SQLiteConnection connection,
      DbType dbType,
      SQLiteConnectionFlags flags)
    {
      string typeName = (string) null;
      if (connection != null)
      {
        flags |= connection.Flags;
        if ((flags & SQLiteConnectionFlags.UseConnectionTypes) == SQLiteConnectionFlags.UseConnectionTypes)
        {
          SQLiteDbTypeMap typeNames = connection._typeNames;
          SQLiteDbTypeMapping liteDbTypeMapping;
          if (typeNames != null && typeNames.TryGetValue(dbType, out liteDbTypeMapping))
            return liteDbTypeMapping.typeName;
        }
        typeName = connection.DefaultTypeName;
      }
      if ((flags & SQLiteConnectionFlags.NoGlobalTypes) == SQLiteConnectionFlags.NoGlobalTypes)
      {
        if (typeName != null)
          return typeName;
        string defaultTypeName = SQLiteConvert.GetDefaultTypeName(connection);
        SQLiteConvert.DefaultTypeNameWarning(dbType, flags, defaultTypeName);
        return defaultTypeName;
      }
      lock (SQLiteConvert._syncRoot)
      {
        if (SQLiteConvert._typeNames == null)
          SQLiteConvert._typeNames = SQLiteConvert.GetSQLiteDbTypeMap();
        SQLiteDbTypeMapping liteDbTypeMapping;
        if (SQLiteConvert._typeNames.TryGetValue(dbType, out liteDbTypeMapping))
          return liteDbTypeMapping.typeName;
      }
      if (typeName != null)
        return typeName;
      string defaultTypeName1 = SQLiteConvert.GetDefaultTypeName(connection);
      SQLiteConvert.DefaultTypeNameWarning(dbType, flags, defaultTypeName1);
      return defaultTypeName1;
    }

    internal static Type DbTypeToType(DbType typ) => SQLiteConvert._dbtypeToType[(int) typ];

    internal static TypeAffinity TypeToAffinity(Type typ)
    {
      TypeCode typeCode = Type.GetTypeCode(typ);
      if (typeCode != TypeCode.Object)
        return SQLiteConvert._typecodeAffinities[(int) typeCode];
      return typ == typeof (byte[]) || typ == typeof (Guid) ? TypeAffinity.Blob : TypeAffinity.Text;
    }

    private static SQLiteDbTypeMap GetSQLiteDbTypeMap()
    {
      return new SQLiteDbTypeMap((IEnumerable<SQLiteDbTypeMapping>) new SQLiteDbTypeMapping[72]
      {
        new SQLiteDbTypeMapping("BIGINT", DbType.Int64, false),
        new SQLiteDbTypeMapping("BIGUINT", DbType.UInt64, false),
        new SQLiteDbTypeMapping("BINARY", DbType.Binary, false),
        new SQLiteDbTypeMapping("BIT", DbType.Boolean, true),
        new SQLiteDbTypeMapping("BLOB", DbType.Binary, true),
        new SQLiteDbTypeMapping("BOOL", DbType.Boolean, false),
        new SQLiteDbTypeMapping("BOOLEAN", DbType.Boolean, false),
        new SQLiteDbTypeMapping("CHAR", DbType.AnsiStringFixedLength, true),
        new SQLiteDbTypeMapping("CLOB", DbType.String, false),
        new SQLiteDbTypeMapping("COUNTER", DbType.Int64, false),
        new SQLiteDbTypeMapping("CURRENCY", DbType.Decimal, false),
        new SQLiteDbTypeMapping("DATE", DbType.DateTime, false),
        new SQLiteDbTypeMapping("DATETIME", DbType.DateTime, true),
        new SQLiteDbTypeMapping("DECIMAL", DbType.Decimal, true),
        new SQLiteDbTypeMapping("DOUBLE", DbType.Double, false),
        new SQLiteDbTypeMapping("FLOAT", DbType.Double, false),
        new SQLiteDbTypeMapping("GENERAL", DbType.Binary, false),
        new SQLiteDbTypeMapping("GUID", DbType.Guid, false),
        new SQLiteDbTypeMapping("IDENTITY", DbType.Int64, false),
        new SQLiteDbTypeMapping("IMAGE", DbType.Binary, false),
        new SQLiteDbTypeMapping("INT", DbType.Int32, true),
        new SQLiteDbTypeMapping("INT8", DbType.SByte, false),
        new SQLiteDbTypeMapping("INT16", DbType.Int16, false),
        new SQLiteDbTypeMapping("INT32", DbType.Int32, false),
        new SQLiteDbTypeMapping("INT64", DbType.Int64, false),
        new SQLiteDbTypeMapping("INTEGER", DbType.Int64, true),
        new SQLiteDbTypeMapping("INTEGER8", DbType.SByte, false),
        new SQLiteDbTypeMapping("INTEGER16", DbType.Int16, false),
        new SQLiteDbTypeMapping("INTEGER32", DbType.Int32, false),
        new SQLiteDbTypeMapping("INTEGER64", DbType.Int64, false),
        new SQLiteDbTypeMapping("LOGICAL", DbType.Boolean, false),
        new SQLiteDbTypeMapping("LONG", DbType.Int64, false),
        new SQLiteDbTypeMapping("LONGCHAR", DbType.String, false),
        new SQLiteDbTypeMapping("LONGTEXT", DbType.String, false),
        new SQLiteDbTypeMapping("LONGVARCHAR", DbType.String, false),
        new SQLiteDbTypeMapping("MEMO", DbType.String, false),
        new SQLiteDbTypeMapping("MONEY", DbType.Decimal, false),
        new SQLiteDbTypeMapping("NCHAR", DbType.StringFixedLength, true),
        new SQLiteDbTypeMapping("NOTE", DbType.String, false),
        new SQLiteDbTypeMapping("NTEXT", DbType.String, false),
        new SQLiteDbTypeMapping("NUMBER", DbType.Decimal, false),
        new SQLiteDbTypeMapping("NUMERIC", DbType.Decimal, false),
        new SQLiteDbTypeMapping("NVARCHAR", DbType.String, true),
        new SQLiteDbTypeMapping("OLEOBJECT", DbType.Binary, false),
        new SQLiteDbTypeMapping("RAW", DbType.Binary, false),
        new SQLiteDbTypeMapping("REAL", DbType.Double, true),
        new SQLiteDbTypeMapping("SINGLE", DbType.Single, true),
        new SQLiteDbTypeMapping("SMALLDATE", DbType.DateTime, false),
        new SQLiteDbTypeMapping("SMALLINT", DbType.Int16, true),
        new SQLiteDbTypeMapping("SMALLUINT", DbType.UInt16, true),
        new SQLiteDbTypeMapping("STRING", DbType.String, false),
        new SQLiteDbTypeMapping("TEXT", DbType.String, false),
        new SQLiteDbTypeMapping("TIME", DbType.DateTime, false),
        new SQLiteDbTypeMapping("TIMESTAMP", DbType.DateTime, false),
        new SQLiteDbTypeMapping("TINYINT", DbType.Byte, true),
        new SQLiteDbTypeMapping("TINYSINT", DbType.SByte, true),
        new SQLiteDbTypeMapping("UINT", DbType.UInt32, true),
        new SQLiteDbTypeMapping("UINT8", DbType.Byte, false),
        new SQLiteDbTypeMapping("UINT16", DbType.UInt16, false),
        new SQLiteDbTypeMapping("UINT32", DbType.UInt32, false),
        new SQLiteDbTypeMapping("UINT64", DbType.UInt64, false),
        new SQLiteDbTypeMapping("ULONG", DbType.UInt64, false),
        new SQLiteDbTypeMapping("UNIQUEIDENTIFIER", DbType.Guid, true),
        new SQLiteDbTypeMapping("UNSIGNEDINTEGER", DbType.UInt64, true),
        new SQLiteDbTypeMapping("UNSIGNEDINTEGER8", DbType.Byte, false),
        new SQLiteDbTypeMapping("UNSIGNEDINTEGER16", DbType.UInt16, false),
        new SQLiteDbTypeMapping("UNSIGNEDINTEGER32", DbType.UInt32, false),
        new SQLiteDbTypeMapping("UNSIGNEDINTEGER64", DbType.UInt64, false),
        new SQLiteDbTypeMapping("VARBINARY", DbType.Binary, false),
        new SQLiteDbTypeMapping("VARCHAR", DbType.AnsiString, true),
        new SQLiteDbTypeMapping("VARCHAR2", DbType.AnsiString, false),
        new SQLiteDbTypeMapping("YESNO", DbType.Boolean, false)
      });
    }

    internal static bool IsStringDbType(DbType type)
    {
      switch (type)
      {
        case DbType.AnsiString:
        case DbType.String:
        case DbType.AnsiStringFixedLength:
        case DbType.StringFixedLength:
          return true;
        default:
          return false;
      }
    }

    private static string SettingValueToString(object value)
    {
      if (value is string)
        return (string) value;
      return value?.ToString();
    }

    private static DbType GetDefaultDbType(SQLiteConnection connection)
    {
      if (((connection != null ? (long) connection.Flags : 0L) & 134217728L) == 134217728L)
        return DbType.Object;
      bool flag = false;
      string name = "Use_SQLiteConvert_DefaultDbType";
      object defaultDbType = (object) null;
      string @default = (string) null;
      if (connection == null || !connection.TryGetCachedSetting(name, (object) @default, out defaultDbType))
        defaultDbType = (object) UnsafeNativeMethods.GetSettingValue(name, @default) ?? (object) DbType.Object;
      else
        flag = true;
      try
      {
        if (!(defaultDbType is DbType))
        {
          defaultDbType = SQLiteConnection.TryParseEnum(typeof (DbType), SQLiteConvert.SettingValueToString(defaultDbType), true);
          if (!(defaultDbType is DbType))
            defaultDbType = (object) DbType.Object;
        }
        return (DbType) defaultDbType;
      }
      finally
      {
        if (!flag && connection != null)
          connection.SetCachedSetting(name, defaultDbType);
      }
    }

    public static string GetStringOrNull(object value)
    {
      if (value == null)
        return (string) null;
      if (value is string)
        return (string) value;
      return value == DBNull.Value ? (string) null : value.ToString();
    }

    internal static bool LooksLikeNull(string text) => text == null;

    internal static bool LooksLikeInt64(string text)
    {
      long result;
      return long.TryParse(text, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture, out result) && string.Equals(result.ToString((IFormatProvider) CultureInfo.InvariantCulture), text, StringComparison.Ordinal);
    }

    internal static bool LooksLikeDouble(string text)
    {
      double result;
      return double.TryParse(text, NumberStyles.Float | NumberStyles.AllowThousands, (IFormatProvider) CultureInfo.InvariantCulture, out result) && string.Equals(result.ToString((IFormatProvider) CultureInfo.InvariantCulture), text, StringComparison.Ordinal);
    }

    internal static bool LooksLikeDateTime(SQLiteConvert convert, string text)
    {
      if (convert == null)
        return false;
      try
      {
        DateTime dateTime = convert.ToDateTime(text);
        if (string.Equals(convert.ToString(dateTime), text, StringComparison.Ordinal))
          return true;
      }
      catch
      {
      }
      return false;
    }

    internal static DbType TypeNameToDbType(
      SQLiteConnection connection,
      string typeName,
      SQLiteConnectionFlags flags)
    {
      DbType? dbType = new DbType?();
      if (connection != null)
      {
        flags |= connection.Flags;
        if ((flags & SQLiteConnectionFlags.UseConnectionTypes) == SQLiteConnectionFlags.UseConnectionTypes)
        {
          SQLiteDbTypeMap typeNames = connection._typeNames;
          if (typeNames != null && typeName != null)
          {
            SQLiteDbTypeMapping liteDbTypeMapping;
            if (typeNames.TryGetValue(typeName, out liteDbTypeMapping))
              return liteDbTypeMapping.dataType;
            int length = typeName.IndexOf('(');
            if (length > 0 && typeNames.TryGetValue(typeName.Substring(0, length).TrimEnd(), out liteDbTypeMapping))
              return liteDbTypeMapping.dataType;
          }
        }
        dbType = connection.DefaultDbType;
      }
      if ((flags & SQLiteConnectionFlags.NoGlobalTypes) == SQLiteConnectionFlags.NoGlobalTypes)
      {
        if (dbType.HasValue)
          return dbType.Value;
        dbType = new DbType?(SQLiteConvert.GetDefaultDbType(connection));
        SQLiteConvert.DefaultDbTypeWarning(typeName, flags, dbType);
        return dbType.Value;
      }
      lock (SQLiteConvert._syncRoot)
      {
        if (SQLiteConvert._typeNames == null)
          SQLiteConvert._typeNames = SQLiteConvert.GetSQLiteDbTypeMap();
        if (typeName != null)
        {
          SQLiteDbTypeMapping liteDbTypeMapping;
          if (SQLiteConvert._typeNames.TryGetValue(typeName, out liteDbTypeMapping))
            return liteDbTypeMapping.dataType;
          int length = typeName.IndexOf('(');
          if (length > 0)
          {
            if (SQLiteConvert._typeNames.TryGetValue(typeName.Substring(0, length).TrimEnd(), out liteDbTypeMapping))
              return liteDbTypeMapping.dataType;
          }
        }
      }
      if (dbType.HasValue)
        return dbType.Value;
      dbType = new DbType?(SQLiteConvert.GetDefaultDbType(connection));
      SQLiteConvert.DefaultDbTypeWarning(typeName, flags, dbType);
      return dbType.Value;
    }
  }
}
