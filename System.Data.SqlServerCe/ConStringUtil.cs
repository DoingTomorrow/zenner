// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.ConStringUtil
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;

#nullable disable
namespace System.Data.SqlServerCe
{
  internal sealed class ConStringUtil
  {
    private const int CONNECTION_OPTIONS = 16;
    private const int CONNECTION_SYNONYMS = 40;
    private const int ENCRYPTION_MODE_OPTIONS = 3;
    internal const string EM_ENGINE_DEFAULT_KEY = "engine default";
    internal const string EM_PLATFORM_DEFAULT_KEY = "platform default";
    internal const string EM_PPC2003_COMPATIBILITY_KEY = "ppc2003 compatibility";
    private const int BOOL_YES_NO_OPTIONS = 4;
    private const string YES_KEY = "yes";
    private const string NO_KEY = "no";
    private const string TRUE_KEY = "true";
    private const string FALSE_KEY = "false";
    internal const string DATA_SOURCE_KEY = "data source";
    internal const string LCID_KEY = "locale identifier";
    internal const string PERSIST_SECURITY_INFO_KEY = "persist security info";
    internal const string PASSWORD_KEY = "ssce:database password";
    internal const string ENCRYPT_DB_KEY = "ssce:encrypt database";
    internal const string ENLIST_KEY = "ssce:enlist";
    internal const string ENCRYPTION_MODE_KEY = "ssce:encryption mode";
    internal const string MAX_BUFFER_SIZE_KEY = "ssce:max buffer size";
    internal const string TEMP_FILE_DIR_KEY = "ssce:temp file directory";
    internal const string TEMP_FILE_MAX_SIZE_KEY = "ssce:temp file max size";
    internal const string AUTO_SHRINK_THRESHOLD_KEY = "ssce:autoshrink threshold";
    internal const string FLUSH_INTERVAL_KEY = "ssce:flush interval";
    internal const string MAX_DATABASE_SIZE_KEY = "ssce:max database size";
    internal const string DEFAULT_LOCK_ESCALATION_KEY = "ssce:default lock escalation";
    internal const string DEFAULT_LOCK_TIMEOUT_KEY = "ssce:default lock timeout";
    internal const string MODE_KEY = "ssce:mode";
    internal const string CASE_SENSITIVE_KEY = "ssce:case sensitive";
    internal const string CASE_SENSITIVE_SYNONYM = "case sensitive";
    internal const string ENCRYPTION_MODE_SYNONYM = "encryption mode";
    internal const string PWD_SYNONYM = "pwd";
    internal const string PASSWORD_SYNONYM = "password";
    internal const string READ_ONLY = "read only";
    internal const string READ_WRITE = "read write";
    internal const string EXCLUSIVE = "exclusive";
    internal const string SHARED_READ = "shared read";
    internal const string DATADIRECTORY_MACRO = "|DataDirectory|";
    internal const string DATADIRECTORY = "DataDirectory";
    private const int SE_CB_DEFAULT_BUFFER_POOL = 4194304;
    private const int SE_CB_MIN_BUFFER_POOL = 262144;
    private const int SE_CB_MAX_BUFFER_POOL = 268431360;
    private const int SE_AUTO_SHRINK_THRESHOLD_DEFAULT = 60;
    private const int SE_AUTO_SHRINK_OFF = 0;
    private const int SE_AUTO_SHRINK_THRESHOLD_MAX = 100;
    private const int SE_C_DEFAULT_LOCK_ESCALATION = 100;
    private const int SE_C_MIN_LOCK_ESCALATION = 50;
    private const int SE_FLUSH_INTERVAL_DEFAULT = 10;
    private const int SE_FLUSH_INTERVAL_MIN = 1;
    private const int SE_FLUSH_INTERVAL_MAX = 1000;
    private const int SE_C_MAX_MAX_PAGES = 1047549;
    private const int SE_C_DEFAULT_MAX_PAGES = 65536;
    private const int SE_C_MIN_MAX_PAGES = 4096;
    private const int SE_DBWAIT_NONE = 0;
    private const int SE_DBWAIT_DEFAULT = 5000;
    internal const string DATA_SOURCE = "";
    internal const string PASSWORD = "";
    internal const bool PERSIST_SECURITY_INFO = false;
    internal const bool ENLIST = true;
    private const char EQUAL_SIGN = '=';
    private const char SEMI_COLON = ';';
    private const char SINGLE_QUOTE = '\'';
    private const char DOUBLE_QUOTE = '"';
    private const char SPACE = ' ';
    private const char TAB = '\t';
    private const char BACKSLASH = '\\';
    private static Hashtable _defaultConnectionOptions;
    private static Hashtable _connectionSynonymMapping;
    private static Hashtable _boolYesNoMapping;
    private static Hashtable _EncryptionModeMapping;

    private ConStringUtil()
    {
    }

    internal static Hashtable NullConnectionOptionsTable() => new Hashtable(16);

    internal static Hashtable DefaultConnectionOptionsTable()
    {
      Hashtable hashtable = ConStringUtil._defaultConnectionOptions;
      if (hashtable == null)
      {
        hashtable = new Hashtable(16);
        hashtable[(object) "data source"] = (object) "";
        hashtable[(object) "ssce:database password"] = (object) "";
        hashtable[(object) "persist security info"] = (object) false;
        hashtable[(object) "ssce:enlist"] = (object) true;
        ConStringUtil._defaultConnectionOptions = hashtable;
      }
      return hashtable;
    }

    private static void InitMappingsTables()
    {
      if (ConStringUtil._connectionSynonymMapping == null)
        ConStringUtil._connectionSynonymMapping = new Hashtable(40)
        {
          [(object) "data source"] = (object) "data source",
          [(object) "datasource"] = (object) "data source",
          [(object) "locale identifier"] = (object) "locale identifier",
          [(object) "initial lcid"] = (object) "locale identifier",
          [(object) "lcid"] = (object) "locale identifier",
          [(object) "database password"] = (object) "ssce:database password",
          [(object) "ssce:database password"] = (object) "ssce:database password",
          [(object) "encrypt database"] = (object) "ssce:encrypt database",
          [(object) "ssce:encrypt database"] = (object) "ssce:encrypt database",
          [(object) "enlist"] = (object) "ssce:enlist",
          [(object) "ssce:enlist"] = (object) "ssce:enlist",
          [(object) "encryption mode"] = (object) "ssce:encryption mode",
          [(object) "ssce:encryption mode"] = (object) "ssce:encryption mode",
          [(object) "password"] = (object) "ssce:database password",
          [(object) "pwd"] = (object) "ssce:database password",
          [(object) "encrypt"] = (object) "ssce:encrypt database",
          [(object) "persist security info"] = (object) "persist security info",
          [(object) "max buffer size"] = (object) "ssce:max buffer size",
          [(object) "ssce:max buffer size"] = (object) "ssce:max buffer size",
          [(object) "temp file directory"] = (object) "ssce:temp file directory",
          [(object) "temp path"] = (object) "ssce:temp file directory",
          [(object) "ssce:temp file directory"] = (object) "ssce:temp file directory",
          [(object) "temp file max size"] = (object) "ssce:temp file max size",
          [(object) "ssce:temp file max size"] = (object) "ssce:temp file max size",
          [(object) "autoshrink threshold"] = (object) "ssce:autoshrink threshold",
          [(object) "ssce:autoshrink threshold"] = (object) "ssce:autoshrink threshold",
          [(object) "flush interval"] = (object) "ssce:flush interval",
          [(object) "ssce:flush interval"] = (object) "ssce:flush interval",
          [(object) "max database size"] = (object) "ssce:max database size",
          [(object) "ssce:max database size"] = (object) "ssce:max database size",
          [(object) "default lock escalation"] = (object) "ssce:default lock escalation",
          [(object) "ssce:default lock escalation"] = (object) "ssce:default lock escalation",
          [(object) "default lock timeout"] = (object) "ssce:default lock timeout",
          [(object) "ssce:default lock timeout"] = (object) "ssce:default lock timeout",
          [(object) "ssce:mode"] = (object) "ssce:mode",
          [(object) "mode"] = (object) "ssce:mode",
          [(object) "file mode"] = (object) "ssce:mode",
          [(object) "ssce:case sensitive"] = (object) "ssce:case sensitive",
          [(object) "case sensitive"] = (object) "ssce:case sensitive",
          [(object) "casesensitive"] = (object) "ssce:case sensitive"
        };
      if (ConStringUtil._boolYesNoMapping == null)
        ConStringUtil._boolYesNoMapping = new Hashtable(4)
        {
          [(object) "yes"] = (object) true,
          [(object) "no"] = (object) false,
          [(object) "true"] = (object) true,
          [(object) "false"] = (object) false
        };
      if (ConStringUtil._EncryptionModeMapping != null)
        return;
      ConStringUtil._EncryptionModeMapping = new Hashtable(3)
      {
        [(object) "platform default"] = (object) 1,
        [(object) "engine default"] = (object) 2,
        [(object) "ppc2003 compatibility"] = (object) 3
      };
    }

    internal static string RemoveKeyValuesFromString(string conString, string removeKey)
    {
      string conString1 = conString;
      int index = 0;
      int length = 0;
      ConStringUtil.SkipWhiteSpace(conString1, ref index);
      while (index < conString1.Length)
      {
        if (conString1[index] == ';')
        {
          ++index;
          length = index;
          ConStringUtil.SkipWhiteSpace(conString1, ref index);
        }
        else
        {
          int num = conString1.IndexOf('=', index);
          string lower = conString1.Substring(index, num - index).TrimEnd((char[]) null).ToLower(CultureInfo.InvariantCulture);
          index = num + 1;
          if (removeKey == "ssce:database password" && ("password" == lower || "pwd" == lower || "ssce:database password" == lower))
          {
            string str = conString1.Substring(0, length);
            ConStringUtil.SkipValue(conString1, ref index);
            if (index < conString1.Length && conString1[index] == ';')
              ++index;
            conString1 = str + conString1.Substring(index);
            index = length;
            ConStringUtil.SkipWhiteSpace(conString1, ref index);
          }
          else
          {
            ConStringUtil.SkipValue(conString1, ref index);
            if (index < conString1.Length && conString1[index] == ';')
            {
              ++index;
              length = index;
              ConStringUtil.SkipWhiteSpace(conString1, ref index);
            }
          }
        }
      }
      return conString1;
    }

    private static void SkipValue(string conString, ref int index)
    {
      ConStringUtil.SkipWhiteSpace(conString, ref index);
      if (index == conString.Length)
        return;
      char ch = conString[index];
      switch (ch)
      {
        case '"':
        case '\'':
          do
          {
            int num = conString.IndexOf(ch, index + 1);
            index = num + 1;
          }
          while (index < conString.Length && (int) conString[index] == (int) ch);
          ConStringUtil.SkipWhiteSpace(conString, ref index);
          break;
        case ';':
          break;
        default:
          index = conString.IndexOf(';', index + 1);
          if (index != -1)
            break;
          index = conString.Length;
          break;
      }
    }

    private static string ReplaceDataDirectory(string inputString)
    {
      string str = inputString;
      if (!string.IsNullOrEmpty(inputString) && inputString.StartsWith("|DataDirectory|", StringComparison.InvariantCultureIgnoreCase))
      {
        string path1 = AppDomain.CurrentDomain.GetData("DataDirectory") as string;
        if (string.IsNullOrEmpty(path1))
          path1 = AppDomain.CurrentDomain.BaseDirectory ?? Environment.CurrentDirectory;
        if (string.IsNullOrEmpty(path1))
          path1 = string.Empty;
        int length = "|DataDirectory|".Length;
        if (inputString.Length > "|DataDirectory|".Length && '\\' == inputString["|DataDirectory|".Length])
          ++length;
        str = Path.Combine(path1, inputString.Substring(length));
      }
      return str;
    }

    internal static Hashtable ParseConnectionString(ref string connectionString)
    {
      ConStringUtil.InitMappingsTables();
      Hashtable values;
      if (connectionString != null && connectionString.Length != 0)
      {
        values = ConStringUtil.NullConnectionOptionsTable();
        ConStringUtil.ParseStringIntoHashtable(connectionString, values);
        ConStringUtil.VerifyValues(values);
      }
      else
        values = ConStringUtil.DefaultConnectionOptionsTable();
      return values;
    }

    private static void ParseStringIntoHashtable(string conString, Hashtable values)
    {
      int currentPosition = 0;
      int vallength = 0;
      bool isempty = false;
      string key = string.Empty;
      string empty = string.Empty;
      char[] charArray = conString.ToCharArray();
      char[] valuebuf = new char[conString.Length];
      while (currentPosition < conString.Length)
      {
        currentPosition = ConStringUtil.GetKeyValuePair(charArray, currentPosition, out key, valuebuf, out vallength, out isempty);
        if (!isempty)
        {
          string str = new string(valuebuf, 0, vallength);
          if (!ConStringUtil.InsertKeyValueIntoHash(values, ref key, str))
            throw new ArgumentException(Res.GetString("SQL_InvalidConStringOption", (object) key));
        }
      }
    }

    private static Exception ConnectionStringSyntax(int index, char[] connectionString)
    {
      return (Exception) new ArgumentException(Res.GetString("ADP_ConnectionStringSyntax", (object) index));
    }

    private static int GetKeyValuePair(
      char[] connectionString,
      int currentPosition,
      out string key,
      char[] valuebuf,
      out int vallength,
      out bool isempty)
    {
      ConStringUtil.PARSERSTATE parserstate = ConStringUtil.PARSERSTATE.NothingYet;
      int bufPosition = 0;
      int index = currentPosition;
      key = (string) null;
      vallength = -1;
      isempty = false;
      char minValue = char.MinValue;
      if (connectionString.Length >= int.MaxValue)
        throw new OverflowException();
      for (; currentPosition < connectionString.Length; ++currentPosition)
      {
        minValue = connectionString[currentPosition];
        switch (parserstate)
        {
          case ConStringUtil.PARSERSTATE.NothingYet:
            if (';' != minValue && !char.IsWhiteSpace(minValue))
            {
              index = currentPosition;
              if (minValue == char.MinValue)
              {
                parserstate = ConStringUtil.PARSERSTATE.NullTermination;
                break;
              }
              if (char.IsControl(minValue))
                throw ConStringUtil.ConnectionStringSyntax(currentPosition, connectionString);
              parserstate = ConStringUtil.PARSERSTATE.Key;
              bufPosition = 0;
              goto default;
            }
            else
              break;
          case ConStringUtil.PARSERSTATE.Key:
            if ('=' == minValue)
            {
              parserstate = ConStringUtil.PARSERSTATE.KeyEqual;
              break;
            }
            if (!char.IsWhiteSpace(minValue) && char.IsControl(minValue))
              throw ConStringUtil.ConnectionStringSyntax(currentPosition, connectionString);
            goto default;
          case ConStringUtil.PARSERSTATE.KeyEqual:
            if ('=' == minValue)
            {
              parserstate = ConStringUtil.PARSERSTATE.Key;
              goto default;
            }
            else
            {
              key = ConStringUtil.GetKey(valuebuf, bufPosition);
              bufPosition = 0;
              parserstate = ConStringUtil.PARSERSTATE.KeyEnd;
              goto case ConStringUtil.PARSERSTATE.KeyEnd;
            }
          case ConStringUtil.PARSERSTATE.KeyEnd:
            if (!char.IsWhiteSpace(minValue))
            {
              if ('\'' == minValue)
              {
                parserstate = ConStringUtil.PARSERSTATE.SingleQuoteValue;
                break;
              }
              if ('"' == minValue)
              {
                parserstate = ConStringUtil.PARSERSTATE.DoubleQuoteValue;
                break;
              }
              if (';' != minValue && minValue != char.MinValue)
              {
                if (char.IsControl(minValue))
                  throw ConStringUtil.ConnectionStringSyntax(currentPosition, connectionString);
                parserstate = ConStringUtil.PARSERSTATE.UnquotedValue;
                goto default;
              }
              else
                goto label_59;
            }
            else
              break;
          case ConStringUtil.PARSERSTATE.UnquotedValue:
            if (char.IsWhiteSpace(minValue) || !char.IsControl(minValue) && ';' != minValue)
              goto default;
            else
              goto label_59;
          case ConStringUtil.PARSERSTATE.DoubleQuoteValue:
            if ('"' == minValue)
            {
              parserstate = ConStringUtil.PARSERSTATE.DoubleQuoteValueQuote;
              break;
            }
            if (minValue == char.MinValue)
              throw ConStringUtil.ConnectionStringSyntax(currentPosition, connectionString);
            goto default;
          case ConStringUtil.PARSERSTATE.DoubleQuoteValueQuote:
            if ('"' == minValue)
            {
              parserstate = ConStringUtil.PARSERSTATE.DoubleQuoteValue;
              goto default;
            }
            else
            {
              parserstate = ConStringUtil.PARSERSTATE.DoubleQuoteValueEnd;
              goto case ConStringUtil.PARSERSTATE.DoubleQuoteValueEnd;
            }
          case ConStringUtil.PARSERSTATE.DoubleQuoteValueEnd:
            if (!char.IsWhiteSpace(minValue))
            {
              if (';' != minValue)
              {
                if (minValue != char.MinValue)
                  throw ConStringUtil.ConnectionStringSyntax(currentPosition, connectionString);
                parserstate = ConStringUtil.PARSERSTATE.NullTermination;
                break;
              }
              goto label_59;
            }
            else
              break;
          case ConStringUtil.PARSERSTATE.SingleQuoteValue:
            if ('\'' == minValue)
            {
              parserstate = ConStringUtil.PARSERSTATE.SingleQuoteValueQuote;
              break;
            }
            if (minValue == char.MinValue)
              throw ConStringUtil.ConnectionStringSyntax(currentPosition, connectionString);
            goto default;
          case ConStringUtil.PARSERSTATE.SingleQuoteValueQuote:
            if ('\'' == minValue)
            {
              parserstate = ConStringUtil.PARSERSTATE.SingleQuoteValue;
              goto default;
            }
            else
            {
              parserstate = ConStringUtil.PARSERSTATE.SingleQuoteValueEnd;
              goto case ConStringUtil.PARSERSTATE.SingleQuoteValueEnd;
            }
          case ConStringUtil.PARSERSTATE.SingleQuoteValueEnd:
            if (!char.IsWhiteSpace(minValue))
            {
              if (';' != minValue)
              {
                if (minValue != char.MinValue)
                  throw ConStringUtil.ConnectionStringSyntax(currentPosition, connectionString);
                parserstate = ConStringUtil.PARSERSTATE.NullTermination;
                break;
              }
              goto label_59;
            }
            else
              break;
          case ConStringUtil.PARSERSTATE.NullTermination:
            if (minValue != char.MinValue && !char.IsWhiteSpace(minValue))
              throw ConStringUtil.ConnectionStringSyntax(index, connectionString);
            break;
          default:
            valuebuf[bufPosition++] = minValue;
            break;
        }
      }
      if (ConStringUtil.PARSERSTATE.KeyEqual == parserstate)
      {
        key = ConStringUtil.GetKey(valuebuf, bufPosition);
        bufPosition = 0;
      }
      if (ConStringUtil.PARSERSTATE.Key == parserstate || ConStringUtil.PARSERSTATE.DoubleQuoteValue == parserstate || ConStringUtil.PARSERSTATE.SingleQuoteValue == parserstate)
        throw ConStringUtil.ConnectionStringSyntax(index, connectionString);
label_59:
      if (ConStringUtil.PARSERSTATE.UnquotedValue == parserstate)
      {
        bufPosition = ConStringUtil.TrimWhiteSpace(valuebuf, bufPosition);
        if ('\'' == valuebuf[bufPosition - 1] || '"' == valuebuf[bufPosition - 1])
          throw ConStringUtil.ConnectionStringSyntax(currentPosition - 1, connectionString);
      }
      else if (ConStringUtil.PARSERSTATE.KeyEqual != parserstate && ConStringUtil.PARSERSTATE.KeyEnd != parserstate)
        isempty = 0 == bufPosition;
      if (';' == minValue && currentPosition < connectionString.Length)
        ++currentPosition;
      vallength = bufPosition;
      return currentPosition;
    }

    private static string GetKey(char[] valuebuf, int bufPosition)
    {
      bufPosition = ConStringUtil.TrimWhiteSpace(valuebuf, bufPosition);
      byte[] bytes = Encoding.Unicode.GetBytes(valuebuf, 0, bufPosition);
      return Encoding.Unicode.GetString(bytes, 0, bytes.Length).ToLower(CultureInfo.InvariantCulture);
    }

    private static int TrimWhiteSpace(char[] valuebuf, int bufPosition)
    {
      while (0 < bufPosition && char.IsWhiteSpace(valuebuf[bufPosition - 1]))
        --bufPosition;
      return bufPosition;
    }

    private static void SkipWhiteSpace(string conString, ref int index)
    {
      while (index < conString.Length)
      {
        switch (conString[index])
        {
          case '\t':
          case ' ':
            ++index;
            continue;
          default:
            return;
        }
      }
    }

    private static bool InsertKeyValueIntoHash(Hashtable values, ref string key, string value)
    {
      bool flag = true;
      if (string.Empty == value)
        value = (string) null;
      if (values.ContainsKey((object) key))
        values[(object) key] = (object) value;
      else if (ConStringUtil._connectionSynonymMapping.ContainsKey((object) key))
      {
        key = (string) ConStringUtil._connectionSynonymMapping[(object) key];
        values[(object) key] = (object) value;
      }
      else
        flag = false;
      return flag;
    }

    internal static int MapEncryptionMode(string value)
    {
      return value != null && ConStringUtil._EncryptionModeMapping.ContainsKey((object) value) ? (int) ConStringUtil._EncryptionModeMapping[(object) value] : 0;
    }

    private static void VerifyValues(Hashtable values)
    {
      ConStringUtil.SetDataSource(values);
      ConStringUtil.SetPersistSecurityInfo(values);
      ConStringUtil.SetEncryptDataBase(values);
      ConStringUtil.SetEncryptionMode(values);
      ConStringUtil.SetEnlistDataBase(values);
      ConStringUtil.SetMaxBufferSize(values);
      ConStringUtil.SetLocale(values);
      ConStringUtil.SetAutoShrinkThreshold(values);
      ConStringUtil.SetMaxDatabaseSize(values);
      ConStringUtil.SetDefaultLockEscalation(values);
      ConStringUtil.SetDefaultLockTimeout(values);
      ConStringUtil.SetFlushInterval(values);
      ConStringUtil.SetFileMode(values);
      ConStringUtil.SetTempFileMaxSize(values);
      ConStringUtil.SetCaseSensitivity(values);
      ConStringUtil.SetStringType("data source", values);
      ConStringUtil.SetStringType("ssce:database password", values);
    }

    private static void SetCaseSensitivity(Hashtable table)
    {
      string str = (string) table[(object) "ssce:case sensitive"];
      string key = str;
      if (str != null)
        key = str.ToLower(CultureInfo.InvariantCulture);
      if (key == null)
        return;
      table[(object) "ssce:case sensitive"] = ConStringUtil._boolYesNoMapping.ContainsKey((object) key) ? (object) (bool) ConStringUtil._boolYesNoMapping[(object) key] : throw new ArgumentException(Res.GetString("SQL_InvalidConnectionOptionValue", (object) "ssce:case sensitive", (object) str));
    }

    private static void SetDataSource(Hashtable table)
    {
      string inputString = (string) table[(object) "data source"];
      if (inputString == null)
        return;
      table[(object) "data source"] = (object) ConStringUtil.ReplaceDataDirectory(inputString);
    }

    private static void SetLocale(Hashtable table)
    {
      string str = (string) table[(object) "locale identifier"];
      if (str == null)
        return;
      int num = ConStringUtil.SetIntType(str, "locale identifier");
      table[(object) "locale identifier"] = num >= 0 ? (object) num : throw new ArgumentException(Res.GetString("SQLCE_ArgumentOutOfRange", (object) "locale identifier", (object) 0, (object) int.MaxValue));
    }

    private static void SetTempFileMaxSize(Hashtable table)
    {
      string str = (string) table[(object) "ssce:temp file max size"];
      int num;
      if (str != null)
      {
        num = ConStringUtil.SetIntType(str, "ssce:temp file max size");
        if (16 > num || 4091 < num)
          throw new ArgumentException(Res.GetString("SQLCE_ArgumentOutOfRange", (object) "ssce:temp file max size", (object) 16, (object) 4091));
      }
      else
        num = 256;
      table[(object) "ssce:temp file max size"] = (object) num;
    }

    private static void SetMaxBufferSize(Hashtable table)
    {
      string str = (string) table[(object) "ssce:max buffer size"];
      int num;
      if (str != null)
      {
        num = ConStringUtil.SetIntType(str, "ssce:max buffer size");
        if (256 > num || 262140 < num)
          throw new ArgumentException(Res.GetString("SQLCE_ArgumentOutOfRange", (object) "ssce:max buffer size", (object) 256, (object) 262140));
      }
      else
        num = 4096;
      table[(object) "ssce:max buffer size"] = (object) num;
    }

    private static void SetEnlistDataBase(Hashtable table)
    {
      string str = (string) table[(object) "ssce:enlist"];
      string key = str;
      if (str != null)
        key = str.ToLower(CultureInfo.InvariantCulture);
      if (key != null)
        table[(object) "ssce:enlist"] = ConStringUtil._boolYesNoMapping.ContainsKey((object) key) ? (object) (bool) ConStringUtil._boolYesNoMapping[(object) key] : throw new ArgumentException(Res.GetString("SQL_InvalidConnectionOptionValue", (object) "ssce:enlist", (object) str));
      else
        table[(object) "ssce:enlist"] = (object) true;
    }

    private static void SetEncryptionMode(Hashtable table)
    {
      string str = (string) table[(object) "ssce:encryption mode"];
      string key = str;
      if (str != null)
        key = str.ToLower(CultureInfo.InvariantCulture);
      if (key == null)
        return;
      table[(object) "ssce:encryption mode"] = ConStringUtil._EncryptionModeMapping.ContainsKey((object) key) ? (object) key : throw new ArgumentException(Res.GetString("SQL_InvalidConnectionOptionValue", (object) "ssce:encryption mode", (object) str));
    }

    private static void SetEncryptDataBase(Hashtable table)
    {
      string str = (string) table[(object) "ssce:encrypt database"];
      string key = str;
      if (str != null)
        key = str.ToLower(CultureInfo.InvariantCulture);
      if (key == null)
        return;
      table[(object) "ssce:encrypt database"] = ConStringUtil._boolYesNoMapping.ContainsKey((object) key) ? (object) (bool) ConStringUtil._boolYesNoMapping[(object) key] : throw new ArgumentException(Res.GetString("SQL_InvalidConnectionOptionValue", (object) "ssce:encrypt database", (object) str));
    }

    private static void SetPersistSecurityInfo(Hashtable table)
    {
      string str = (string) table[(object) "persist security info"];
      string key = str;
      if (str != null)
        key = str.ToLower(CultureInfo.InvariantCulture);
      if (key != null)
      {
        table[(object) "persist security info"] = ConStringUtil._boolYesNoMapping.ContainsKey((object) key) ? (object) (bool) ConStringUtil._boolYesNoMapping[(object) key] : throw new ArgumentException(Res.GetString("SQL_InvalidConnectionOptionValue", (object) "persist security info", (object) str));
      }
      else
      {
        if (!table.Contains((object) "persist security info"))
          return;
        table[(object) "persist security info"] = (object) false;
      }
    }

    private static void SetAutoShrinkThreshold(Hashtable table)
    {
      string str = (string) table[(object) "ssce:autoshrink threshold"];
      int num;
      if (str != null)
      {
        num = ConStringUtil.SetIntType(str, "ssce:autoshrink threshold");
        if (0 > num || 100 < num)
          throw new ArgumentException(Res.GetString("SQLCE_ArgumentOutOfRange", (object) "ssce:autoshrink threshold", (object) 0, (object) 100));
      }
      else
        num = 60;
      table[(object) "ssce:autoshrink threshold"] = (object) num;
    }

    private static void SetMaxDatabaseSize(Hashtable table)
    {
      string str = (string) table[(object) "ssce:max database size"];
      int num;
      if (str != null)
      {
        num = ConStringUtil.SetIntType(str, "ssce:max database size");
        if (16 > num || 4091 < num)
          throw new ArgumentException(Res.GetString("SQLCE_ArgumentOutOfRange", (object) "ssce:max database size", (object) 16, (object) 4091));
      }
      else
        num = 256;
      table[(object) "ssce:max database size"] = (object) num;
    }

    private static void SetDefaultLockEscalation(Hashtable table)
    {
      string str = (string) table[(object) "ssce:default lock escalation"];
      int num;
      if (str != null)
      {
        num = ConStringUtil.SetIntType(str, "ssce:default lock escalation");
        if (50 > num)
          throw new ArgumentException(Res.GetString("SQLCE_ArgumentOutOfRange", (object) "ssce:default lock escalation", (object) 50, (object) int.MaxValue));
      }
      else
        num = 100;
      table[(object) "ssce:default lock escalation"] = (object) num;
    }

    private static void SetDefaultLockTimeout(Hashtable table)
    {
      string str = (string) table[(object) "ssce:default lock timeout"];
      int num;
      if (str != null)
      {
        num = ConStringUtil.SetIntType(str, "ssce:default lock timeout");
        if (0 > num)
          throw new ArgumentException(Res.GetString("SQLCE_ArgumentOutOfRange", (object) "ssce:default lock timeout", (object) 0, (object) int.MaxValue));
      }
      else
        num = 5000;
      table[(object) "ssce:default lock timeout"] = (object) num;
    }

    private static void SetFlushInterval(Hashtable table)
    {
      string str = (string) table[(object) "ssce:flush interval"];
      int num;
      if (str != null)
      {
        num = ConStringUtil.SetIntType(str, "ssce:flush interval");
        if (1 > num || 1000 < num)
          throw new ArgumentException(Res.GetString("SQLCE_ArgumentOutOfRange", (object) "ssce:flush interval", (object) 1, (object) 1000));
      }
      else
        num = 10;
      table[(object) "ssce:flush interval"] = (object) num;
    }

    private static int SetIntType(string value, string key)
    {
      if (value != value.Trim())
        throw new ArgumentException(Res.GetString("SQLCE_InvalidValueForKeyValue", (object) key));
      try
      {
        return int.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
      }
      catch (Exception ex)
      {
        throw new ArgumentException(Res.GetString("SQLCE_InvalidValueForKeyValue", (object) key), ex);
      }
    }

    private static void SetFileMode(Hashtable table)
    {
      string str1 = (string) table[(object) "ssce:mode"];
      string str2 = str1;
      if (str2 != null)
      {
        string lower = str2.ToLower(CultureInfo.CurrentCulture);
        switch (lower)
        {
          case "read only":
          case "read write":
          case "exclusive":
          case "shared read":
            break;
          default:
            throw new ArgumentException(Res.GetString("SQL_InvalidConnectionOptionValue", (object) "ssce:mode", (object) lower));
        }
      }
      if (!table.Contains((object) "ssce:mode"))
        return;
      table[(object) "ssce:mode"] = (object) str1;
    }

    private static void SetStringType(string key, Hashtable values)
    {
      string str = (string) values[(object) key];
      if (str == null)
        return;
      values[(object) key] = (object) str;
    }

    private enum PARSERSTATE
    {
      NothingYet = 1,
      Key = 2,
      KeyEqual = 3,
      KeyEnd = 4,
      UnquotedValue = 5,
      DoubleQuoteValue = 6,
      DoubleQuoteValueQuote = 7,
      DoubleQuoteValueEnd = 8,
      SingleQuoteValue = 9,
      SingleQuoteValueQuote = 10, // 0x0000000A
      SingleQuoteValueEnd = 11, // 0x0000000B
      NullTermination = 12, // 0x0000000C
    }
  }
}
