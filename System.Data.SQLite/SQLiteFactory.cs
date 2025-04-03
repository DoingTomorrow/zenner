// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteFactory
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Data.Common;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;

#nullable disable
namespace System.Data.SQLite
{
  public sealed class SQLiteFactory : DbProviderFactory, IDisposable, IServiceProvider
  {
    private bool disposed;
    public static readonly SQLiteFactory Instance = new SQLiteFactory();
    private static readonly string DefaultTypeName = "System.Data.SQLite.Linq.SQLiteProviderServices, System.Data.SQLite.Linq, Version={0}, Culture=neutral, PublicKeyToken=db937bc2d44ff139";
    private static readonly BindingFlags DefaultBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;
    private static Type _dbProviderServicesType;
    private static object _sqliteServices;

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteFactory).Name);
    }

    private void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      this.disposed = true;
    }

    ~SQLiteFactory() => this.Dispose(false);

    public event SQLiteLogEventHandler Log
    {
      add
      {
        this.CheckDisposed();
        SQLiteLog.Log += value;
      }
      remove
      {
        this.CheckDisposed();
        SQLiteLog.Log -= value;
      }
    }

    public override DbCommand CreateCommand()
    {
      this.CheckDisposed();
      return (DbCommand) new SQLiteCommand();
    }

    public override DbCommandBuilder CreateCommandBuilder()
    {
      this.CheckDisposed();
      return (DbCommandBuilder) new SQLiteCommandBuilder();
    }

    public override DbConnection CreateConnection()
    {
      this.CheckDisposed();
      return (DbConnection) new SQLiteConnection();
    }

    public override DbConnectionStringBuilder CreateConnectionStringBuilder()
    {
      this.CheckDisposed();
      return (DbConnectionStringBuilder) new SQLiteConnectionStringBuilder();
    }

    public override DbDataAdapter CreateDataAdapter()
    {
      this.CheckDisposed();
      return (DbDataAdapter) new SQLiteDataAdapter();
    }

    public override DbParameter CreateParameter()
    {
      this.CheckDisposed();
      return (DbParameter) new SQLiteParameter();
    }

    static SQLiteFactory()
    {
      UnsafeNativeMethods.Initialize();
      SQLiteLog.Initialize();
      SQLiteFactory._dbProviderServicesType = Type.GetType(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "System.Data.Common.DbProviderServices, System.Data.Entity, Version={0}, Culture=neutral, PublicKeyToken=b77a5c561934e089", (object) "4.0.0.0"), false);
    }

    object IServiceProvider.GetService(Type serviceType)
    {
      return serviceType == typeof (ISQLiteSchemaExtensions) || SQLiteFactory._dbProviderServicesType != (Type) null && serviceType == SQLiteFactory._dbProviderServicesType ? this.GetSQLiteProviderServicesInstance() : (object) null;
    }

    [ReflectionPermission(SecurityAction.Assert, MemberAccess = true)]
    private object GetSQLiteProviderServicesInstance()
    {
      if (SQLiteFactory._sqliteServices == null)
      {
        string settingValue = UnsafeNativeMethods.GetSettingValue("TypeName_SQLiteProviderServices", (string) null);
        Version version = this.GetType().Assembly.GetName().Version;
        string typeName;
        if (settingValue != null)
          typeName = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, settingValue, (object) version);
        else
          typeName = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, SQLiteFactory.DefaultTypeName, (object) version);
        Type type = Type.GetType(typeName, false);
        if (type != (Type) null)
        {
          FieldInfo field = type.GetField("Instance", SQLiteFactory.DefaultBindingFlags);
          if (field != (FieldInfo) null)
            SQLiteFactory._sqliteServices = field.GetValue((object) null);
        }
      }
      return SQLiteFactory._sqliteServices;
    }
  }
}
