// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteLog
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Diagnostics;
using System.Globalization;

#nullable disable
namespace System.Data.SQLite
{
  public static class SQLiteLog
  {
    private static object syncRoot = new object();
    private static EventHandler _domainUnload;
    private static SQLiteLogEventHandler _defaultHandler;
    private static SQLiteLogCallback _callback;
    private static SQLiteBase _sql;
    private static bool _enabled;

    private static event SQLiteLogEventHandler _handlers;

    public static void Initialize()
    {
      if (SQLite3.StaticIsInitialized() || !AppDomain.CurrentDomain.IsDefaultAppDomain() && UnsafeNativeMethods.GetSettingValue("Force_SQLiteLog", (string) null) == null)
        return;
      lock (SQLiteLog.syncRoot)
      {
        if (SQLiteLog._domainUnload == null)
        {
          SQLiteLog._domainUnload = new EventHandler(SQLiteLog.DomainUnload);
          AppDomain.CurrentDomain.DomainUnload += SQLiteLog._domainUnload;
        }
        if (SQLiteLog._sql == null)
          SQLiteLog._sql = (SQLiteBase) new SQLite3(SQLiteDateFormats.ISO8601, DateTimeKind.Unspecified, (string) null, IntPtr.Zero, (string) null, false);
        if (SQLiteLog._callback == null)
        {
          SQLiteLog._callback = new SQLiteLogCallback(SQLiteLog.LogCallback);
          SQLiteErrorCode errorCode = SQLiteLog._sql.SetLogCallback(SQLiteLog._callback);
          if (errorCode != SQLiteErrorCode.Ok)
            throw new SQLiteException(errorCode, "Failed to initialize logging.");
        }
        SQLiteLog._enabled = true;
        SQLiteLog.AddDefaultHandler();
      }
    }

    private static void DomainUnload(object sender, EventArgs e)
    {
      lock (SQLiteLog.syncRoot)
      {
        SQLiteLog.RemoveDefaultHandler();
        SQLiteLog._enabled = false;
        if (SQLiteLog._sql != null)
        {
          SQLiteErrorCode errorCode1 = SQLiteLog._sql.Shutdown();
          if (errorCode1 != SQLiteErrorCode.Ok)
            throw new SQLiteException(errorCode1, "Failed to shutdown interface.");
          SQLiteErrorCode errorCode2 = SQLiteLog._sql.SetLogCallback((SQLiteLogCallback) null);
          if (errorCode2 != SQLiteErrorCode.Ok)
            throw new SQLiteException(errorCode2, "Failed to shutdown logging.");
        }
        if (SQLiteLog._callback != null)
          SQLiteLog._callback = (SQLiteLogCallback) null;
        if (SQLiteLog._domainUnload == null)
          return;
        AppDomain.CurrentDomain.DomainUnload -= SQLiteLog._domainUnload;
        SQLiteLog._domainUnload = (EventHandler) null;
      }
    }

    public static event SQLiteLogEventHandler Log
    {
      add
      {
        lock (SQLiteLog.syncRoot)
        {
          SQLiteLog._handlers -= value;
          SQLiteLog._handlers += value;
        }
      }
      remove
      {
        lock (SQLiteLog.syncRoot)
          SQLiteLog._handlers -= value;
      }
    }

    public static bool Enabled
    {
      get
      {
        lock (SQLiteLog.syncRoot)
          return SQLiteLog._enabled;
      }
      set
      {
        lock (SQLiteLog.syncRoot)
          SQLiteLog._enabled = value;
      }
    }

    public static void LogMessage(string message) => SQLiteLog.LogMessage((object) null, message);

    public static void LogMessage(SQLiteErrorCode errorCode, string message)
    {
      SQLiteLog.LogMessage((object) errorCode, message);
    }

    public static void LogMessage(int errorCode, string message)
    {
      SQLiteLog.LogMessage((object) errorCode, message);
    }

    private static void LogMessage(object errorCode, string message)
    {
      bool enabled;
      SQLiteLogEventHandler liteLogEventHandler;
      lock (SQLiteLog.syncRoot)
      {
        enabled = SQLiteLog._enabled;
        liteLogEventHandler = SQLiteLog._handlers == null ? (SQLiteLogEventHandler) null : SQLiteLog._handlers.Clone() as SQLiteLogEventHandler;
      }
      if (!enabled || liteLogEventHandler == null)
        return;
      liteLogEventHandler((object) null, new LogEventArgs(IntPtr.Zero, errorCode, message, (object) null));
    }

    private static void InitializeDefaultHandler()
    {
      lock (SQLiteLog.syncRoot)
      {
        if (SQLiteLog._defaultHandler != null)
          return;
        SQLiteLog._defaultHandler = new SQLiteLogEventHandler(SQLiteLog.LogEventHandler);
      }
    }

    public static void AddDefaultHandler()
    {
      SQLiteLog.InitializeDefaultHandler();
      SQLiteLog.Log += SQLiteLog._defaultHandler;
    }

    public static void RemoveDefaultHandler()
    {
      SQLiteLog.InitializeDefaultHandler();
      SQLiteLog.Log -= SQLiteLog._defaultHandler;
    }

    private static void LogCallback(IntPtr pUserData, int errorCode, IntPtr pMessage)
    {
      bool enabled;
      SQLiteLogEventHandler liteLogEventHandler;
      lock (SQLiteLog.syncRoot)
      {
        enabled = SQLiteLog._enabled;
        liteLogEventHandler = SQLiteLog._handlers == null ? (SQLiteLogEventHandler) null : SQLiteLog._handlers.Clone() as SQLiteLogEventHandler;
      }
      if (!enabled || liteLogEventHandler == null)
        return;
      liteLogEventHandler((object) null, new LogEventArgs(pUserData, (object) errorCode, SQLiteConvert.UTF8ToString(pMessage, -1), (object) null));
    }

    private static void LogEventHandler(object sender, LogEventArgs e)
    {
      if (e == null)
        return;
      string message = e.Message;
      string str1;
      if (message == null)
      {
        str1 = "<null>";
      }
      else
      {
        str1 = message.Trim();
        if (str1.Length == 0)
          str1 = "<empty>";
      }
      object errorCode = e.ErrorCode;
      string str2 = "error";
      if (errorCode is SQLiteErrorCode || errorCode is int)
      {
        switch ((SQLiteErrorCode) ((int) errorCode & (int) byte.MaxValue))
        {
          case SQLiteErrorCode.Ok:
            str2 = "message";
            break;
          case SQLiteErrorCode.Notice:
            str2 = "notice";
            break;
          case SQLiteErrorCode.Warning:
            str2 = "warning";
            break;
          case SQLiteErrorCode.Row:
          case SQLiteErrorCode.Done:
            str2 = "data";
            break;
        }
      }
      else if (errorCode == null)
        str2 = "trace";
      if (errorCode != null && !object.ReferenceEquals(errorCode, (object) string.Empty))
        Trace.WriteLine(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "SQLite {0} ({1}): {2}", (object) str2, errorCode, (object) str1));
      else
        Trace.WriteLine(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "SQLite {0}: {1}", (object) str2, (object) str1));
    }
  }
}
