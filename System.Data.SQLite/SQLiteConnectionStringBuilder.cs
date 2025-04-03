// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteConnectionStringBuilder
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Collections;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace System.Data.SQLite
{
  [DefaultProperty("DataSource")]
  [DefaultMember("Item")]
  public sealed class SQLiteConnectionStringBuilder : DbConnectionStringBuilder
  {
    private Hashtable _properties;

    public SQLiteConnectionStringBuilder() => this.Initialize((string) null);

    public SQLiteConnectionStringBuilder(string connectionString)
    {
      this.Initialize(connectionString);
    }

    private void Initialize(string cnnString)
    {
      this._properties = new Hashtable((IEqualityComparer) StringComparer.OrdinalIgnoreCase);
      try
      {
        this.GetProperties(this._properties);
      }
      catch (NotImplementedException ex)
      {
        this.FallbackGetProperties(this._properties);
      }
      if (string.IsNullOrEmpty(cnnString))
        return;
      this.ConnectionString = cnnString;
    }

    [Browsable(true)]
    [DefaultValue(3)]
    public int Version
    {
      get
      {
        object obj;
        this.TryGetValue("version", out obj);
        return Convert.ToInt32(obj, (IFormatProvider) CultureInfo.CurrentCulture);
      }
      set => this["version"] = value == 3 ? (object) value : throw new NotSupportedException();
    }

    [DisplayName("Synchronous")]
    [Browsable(true)]
    [DefaultValue(SynchronizationModes.Normal)]
    public SynchronizationModes SyncMode
    {
      get
      {
        object obj;
        this.TryGetValue("synchronous", out obj);
        return obj is string ? (SynchronizationModes) TypeDescriptor.GetConverter(typeof (SynchronizationModes)).ConvertFrom(obj) : (SynchronizationModes) obj;
      }
      set => this["synchronous"] = (object) value;
    }

    [DefaultValue(false)]
    [Browsable(true)]
    [DisplayName("Use UTF-16 Encoding")]
    public bool UseUTF16Encoding
    {
      get
      {
        object source;
        this.TryGetValue("useutf16encoding", out source);
        return SQLiteConvert.ToBoolean(source);
      }
      set => this["useutf16encoding"] = (object) value;
    }

    [Browsable(true)]
    [DefaultValue(false)]
    public bool Pooling
    {
      get
      {
        object source;
        this.TryGetValue("pooling", out source);
        return SQLiteConvert.ToBoolean(source);
      }
      set => this["pooling"] = (object) value;
    }

    [DefaultValue(true)]
    [Browsable(true)]
    [DisplayName("Binary GUID")]
    public bool BinaryGUID
    {
      get
      {
        object source;
        this.TryGetValue("binaryguid", out source);
        return SQLiteConvert.ToBoolean(source);
      }
      set => this["binaryguid"] = (object) value;
    }

    [Browsable(true)]
    [DisplayName("Data Source")]
    [DefaultValue("")]
    public string DataSource
    {
      get
      {
        object obj;
        this.TryGetValue("data source", out obj);
        return obj?.ToString();
      }
      set => this["data source"] = (object) value;
    }

    [DefaultValue(null)]
    [DisplayName("URI")]
    [Browsable(true)]
    public string Uri
    {
      get
      {
        object obj;
        this.TryGetValue("uri", out obj);
        return obj?.ToString();
      }
      set => this["uri"] = (object) value;
    }

    [DisplayName("Full URI")]
    [Browsable(true)]
    [DefaultValue(null)]
    public string FullUri
    {
      get
      {
        object obj;
        this.TryGetValue("fulluri", out obj);
        return obj?.ToString();
      }
      set => this["fulluri"] = (object) value;
    }

    [Browsable(true)]
    [DisplayName("Default Timeout")]
    [DefaultValue(30)]
    public int DefaultTimeout
    {
      get
      {
        object obj;
        this.TryGetValue("default timeout", out obj);
        return Convert.ToInt32(obj, (IFormatProvider) CultureInfo.CurrentCulture);
      }
      set => this["default timeout"] = (object) value;
    }

    [DisplayName("Busy Timeout")]
    [Browsable(true)]
    [DefaultValue(0)]
    public int BusyTimeout
    {
      get
      {
        object obj;
        this.TryGetValue("busytimeout", out obj);
        return Convert.ToInt32(obj, (IFormatProvider) CultureInfo.CurrentCulture);
      }
      set => this["busytimeout"] = (object) value;
    }

    [Browsable(true)]
    [DefaultValue(3)]
    [DisplayName("Prepare Retries")]
    public int PrepareRetries
    {
      get
      {
        object obj;
        this.TryGetValue("prepareretries", out obj);
        return Convert.ToInt32(obj, (IFormatProvider) CultureInfo.CurrentCulture);
      }
      set => this["prepareretries"] = (object) value;
    }

    [DefaultValue(0)]
    [DisplayName("Progress Ops")]
    [Browsable(true)]
    public int ProgressOps
    {
      get
      {
        object obj;
        this.TryGetValue("progressops", out obj);
        return Convert.ToInt32(obj, (IFormatProvider) CultureInfo.CurrentCulture);
      }
      set => this["progressops"] = (object) value;
    }

    [Browsable(true)]
    [DefaultValue(true)]
    public bool Enlist
    {
      get
      {
        object source;
        this.TryGetValue("enlist", out source);
        return SQLiteConvert.ToBoolean(source);
      }
      set => this["enlist"] = (object) value;
    }

    [DisplayName("Fail If Missing")]
    [Browsable(true)]
    [DefaultValue(false)]
    public bool FailIfMissing
    {
      get
      {
        object source;
        this.TryGetValue("failifmissing", out source);
        return SQLiteConvert.ToBoolean(source);
      }
      set => this["failifmissing"] = (object) value;
    }

    [DefaultValue(false)]
    [DisplayName("Legacy Format")]
    [Browsable(true)]
    public bool LegacyFormat
    {
      get
      {
        object source;
        this.TryGetValue("legacy format", out source);
        return SQLiteConvert.ToBoolean(source);
      }
      set => this["legacy format"] = (object) value;
    }

    [DisplayName("Read Only")]
    [DefaultValue(false)]
    [Browsable(true)]
    public bool ReadOnly
    {
      get
      {
        object source;
        this.TryGetValue("read only", out source);
        return SQLiteConvert.ToBoolean(source);
      }
      set => this["read only"] = (object) value;
    }

    [DefaultValue("")]
    [Browsable(true)]
    [PasswordPropertyText(true)]
    public string Password
    {
      get
      {
        object obj;
        this.TryGetValue("password", out obj);
        return obj?.ToString();
      }
      set => this["password"] = (object) value;
    }

    [DisplayName("Hexadecimal Password")]
    [PasswordPropertyText(true)]
    [DefaultValue(null)]
    [Browsable(true)]
    public byte[] HexPassword
    {
      get
      {
        object text;
        if (this.TryGetValue("hexpassword", out text))
        {
          if (text is string)
            return SQLiteConnection.FromHexString((string) text);
          if (text != null)
            return (byte[]) text;
        }
        return (byte[]) null;
      }
      set => this["hexpassword"] = (object) SQLiteConnection.ToHexString(value);
    }

    [DefaultValue(4096)]
    [DisplayName("Page Size")]
    [Browsable(true)]
    public int PageSize
    {
      get
      {
        object obj;
        this.TryGetValue("page size", out obj);
        return Convert.ToInt32(obj, (IFormatProvider) CultureInfo.CurrentCulture);
      }
      set => this["page size"] = (object) value;
    }

    [Browsable(true)]
    [DisplayName("Maximum Page Count")]
    [DefaultValue(0)]
    public int MaxPageCount
    {
      get
      {
        object obj;
        this.TryGetValue("max page count", out obj);
        return Convert.ToInt32(obj, (IFormatProvider) CultureInfo.CurrentCulture);
      }
      set => this["max page count"] = (object) value;
    }

    [DisplayName("Cache Size")]
    [Browsable(true)]
    [DefaultValue(-2000)]
    public int CacheSize
    {
      get
      {
        object obj;
        this.TryGetValue("cache size", out obj);
        return Convert.ToInt32(obj, (IFormatProvider) CultureInfo.CurrentCulture);
      }
      set => this["cache size"] = (object) value;
    }

    [DisplayName("DateTime Format")]
    [Browsable(true)]
    [DefaultValue(SQLiteDateFormats.ISO8601)]
    public SQLiteDateFormats DateTimeFormat
    {
      get
      {
        object obj;
        if (this.TryGetValue("datetimeformat", out obj))
        {
          if (obj is SQLiteDateFormats dateTimeFormat)
            return dateTimeFormat;
          if (obj != null)
            return (SQLiteDateFormats) TypeDescriptor.GetConverter(typeof (SQLiteDateFormats)).ConvertFrom(obj);
        }
        return SQLiteDateFormats.ISO8601;
      }
      set => this["datetimeformat"] = (object) value;
    }

    [DisplayName("DateTime Kind")]
    [Browsable(true)]
    [DefaultValue(DateTimeKind.Unspecified)]
    public DateTimeKind DateTimeKind
    {
      get
      {
        object obj;
        if (this.TryGetValue("datetimekind", out obj))
        {
          if (obj is DateTimeKind dateTimeKind)
            return dateTimeKind;
          if (obj != null)
            return (DateTimeKind) TypeDescriptor.GetConverter(typeof (DateTimeKind)).ConvertFrom(obj);
        }
        return DateTimeKind.Unspecified;
      }
      set => this["datetimekind"] = (object) value;
    }

    [Browsable(true)]
    [DisplayName("DateTime Format String")]
    [DefaultValue(null)]
    public string DateTimeFormatString
    {
      get
      {
        object timeFormatString;
        if (this.TryGetValue("datetimeformatstring", out timeFormatString))
        {
          if (timeFormatString is string)
            return (string) timeFormatString;
          if (timeFormatString != null)
            return timeFormatString.ToString();
        }
        return (string) null;
      }
      set => this["datetimeformatstring"] = (object) value;
    }

    [DefaultValue("sqlite_default_schema")]
    [DisplayName("Base Schema Name")]
    [Browsable(true)]
    public string BaseSchemaName
    {
      get
      {
        object baseSchemaName;
        if (this.TryGetValue("baseschemaname", out baseSchemaName))
        {
          if (baseSchemaName is string)
            return (string) baseSchemaName;
          if (baseSchemaName != null)
            return baseSchemaName.ToString();
        }
        return (string) null;
      }
      set => this["baseschemaname"] = (object) value;
    }

    [DefaultValue(SQLiteJournalModeEnum.Default)]
    [Browsable(true)]
    [DisplayName("Journal Mode")]
    public SQLiteJournalModeEnum JournalMode
    {
      get
      {
        object obj;
        this.TryGetValue("journal mode", out obj);
        return obj is string ? (SQLiteJournalModeEnum) TypeDescriptor.GetConverter(typeof (SQLiteJournalModeEnum)).ConvertFrom(obj) : (SQLiteJournalModeEnum) obj;
      }
      set => this["journal mode"] = (object) value;
    }

    [Browsable(true)]
    [DisplayName("Default Isolation Level")]
    [DefaultValue(IsolationLevel.Serializable)]
    public IsolationLevel DefaultIsolationLevel
    {
      get
      {
        object obj;
        this.TryGetValue("default isolationlevel", out obj);
        return obj is string ? (IsolationLevel) TypeDescriptor.GetConverter(typeof (IsolationLevel)).ConvertFrom(obj) : (IsolationLevel) obj;
      }
      set => this["default isolationlevel"] = (object) value;
    }

    [DefaultValue(~DbType.AnsiString)]
    [DisplayName("Default Database Type")]
    [Browsable(true)]
    public DbType DefaultDbType
    {
      get
      {
        object defaultDbType;
        if (this.TryGetValue("defaultdbtype", out defaultDbType))
        {
          if (defaultDbType is string)
            return (DbType) TypeDescriptor.GetConverter(typeof (DbType)).ConvertFrom(defaultDbType);
          if (defaultDbType != null)
            return (DbType) defaultDbType;
        }
        return ~DbType.AnsiString;
      }
      set => this["defaultdbtype"] = (object) value;
    }

    [Browsable(true)]
    [DisplayName("Default Type Name")]
    [DefaultValue(null)]
    public string DefaultTypeName
    {
      get
      {
        object obj;
        this.TryGetValue("defaulttypename", out obj);
        return obj?.ToString();
      }
      set => this["defaulttypename"] = (object) value;
    }

    [DisplayName("VFS Name")]
    [DefaultValue(null)]
    [Browsable(true)]
    public string VfsName
    {
      get
      {
        object obj;
        this.TryGetValue("vfsname", out obj);
        return obj?.ToString();
      }
      set => this["vfsname"] = (object) value;
    }

    [DisplayName("Foreign Keys")]
    [Browsable(true)]
    [DefaultValue(false)]
    public bool ForeignKeys
    {
      get
      {
        object source;
        this.TryGetValue("foreign keys", out source);
        return SQLiteConvert.ToBoolean(source);
      }
      set => this["foreign keys"] = (object) value;
    }

    [DisplayName("Recursive Triggers")]
    [Browsable(true)]
    [DefaultValue(false)]
    public bool RecursiveTriggers
    {
      get
      {
        object source;
        this.TryGetValue("recursive triggers", out source);
        return SQLiteConvert.ToBoolean(source);
      }
      set => this["recursive triggers"] = (object) value;
    }

    [Browsable(true)]
    [DefaultValue(null)]
    [DisplayName("ZipVFS Version")]
    public string ZipVfsVersion
    {
      get
      {
        object obj;
        this.TryGetValue("zipvfsversion", out obj);
        return obj?.ToString();
      }
      set => this["zipvfsversion"] = (object) value;
    }

    [Browsable(true)]
    [DefaultValue(SQLiteConnectionFlags.Default)]
    public SQLiteConnectionFlags Flags
    {
      get
      {
        object obj;
        if (this.TryGetValue("flags", out obj))
        {
          if (obj is SQLiteConnectionFlags flags)
            return flags;
          if (obj != null)
            return (SQLiteConnectionFlags) TypeDescriptor.GetConverter(typeof (SQLiteConnectionFlags)).ConvertFrom(obj);
        }
        return SQLiteConnectionFlags.Default;
      }
      set => this["flags"] = (object) value;
    }

    [Browsable(true)]
    [DefaultValue(true)]
    [DisplayName("Set Defaults")]
    public bool SetDefaults
    {
      get
      {
        object source;
        this.TryGetValue("setdefaults", out source);
        return SQLiteConvert.ToBoolean(source);
      }
      set => this["setdefaults"] = (object) value;
    }

    [DefaultValue(true)]
    [DisplayName("To Full Path")]
    [Browsable(true)]
    public bool ToFullPath
    {
      get
      {
        object source;
        this.TryGetValue("tofullpath", out source);
        return SQLiteConvert.ToBoolean(source);
      }
      set => this["tofullpath"] = (object) value;
    }

    [DisplayName("No Default Flags")]
    [Browsable(true)]
    [DefaultValue(false)]
    public bool NoDefaultFlags
    {
      get
      {
        object source;
        this.TryGetValue("nodefaultflags", out source);
        return SQLiteConvert.ToBoolean(source);
      }
      set => this["nodefaultflags"] = (object) value;
    }

    [DefaultValue(false)]
    [DisplayName("No Shared Flags")]
    [Browsable(true)]
    public bool NoSharedFlags
    {
      get
      {
        object source;
        this.TryGetValue("nosharedflags", out source);
        return SQLiteConvert.ToBoolean(source);
      }
      set => this["nosharedflags"] = (object) value;
    }

    public override bool TryGetValue(string keyword, out object value)
    {
      bool flag = base.TryGetValue(keyword, out value);
      if (!this._properties.ContainsKey((object) keyword) || !(this._properties[(object) keyword] is PropertyDescriptor property))
        return flag;
      if (flag)
      {
        if (property.PropertyType == typeof (bool))
          value = (object) SQLiteConvert.ToBoolean(value);
        else if (property.PropertyType != typeof (byte[]))
          value = TypeDescriptor.GetConverter(property.PropertyType).ConvertFrom(value);
      }
      else if (property.Attributes[typeof (DefaultValueAttribute)] is DefaultValueAttribute attribute)
      {
        value = attribute.Value;
        flag = true;
      }
      return flag;
    }

    private void FallbackGetProperties(Hashtable propertyList)
    {
      foreach (PropertyDescriptor property in TypeDescriptor.GetProperties((object) this, true))
      {
        if (property.Name != "ConnectionString" && !propertyList.ContainsKey((object) property.DisplayName))
          propertyList.Add((object) property.DisplayName, (object) property);
      }
    }
  }
}
