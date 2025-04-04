// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.SqlCeRemoteDataAccess
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

using System.Collections;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace System.Data.SqlServerCe
{
  public sealed class SqlCeRemoteDataAccess : IDisposable
  {
    private IntPtr pIRda = IntPtr.Zero;
    private IntPtr pIErrors = IntPtr.Zero;

    public SqlCeRemoteDataAccess()
    {
      NativeMethods.LoadNativeBinaries();
      this.pIRda = new IntPtr(0);
      IntPtr pCreationIError = new IntPtr(0);
      int hr = SqlCeRemoteDataAccess.uwrda_RemoteDataAccess(ref this.pIRda, ref pCreationIError);
      if (NativeMethods.Failed(hr))
      {
        SqlCeException sqlCeException = SqlCeException.FillErrorInformation(hr, pCreationIError);
        int num = IntPtr.Zero != pCreationIError ? (int) NativeMethods.uwutil_ReleaseCOMPtr(pCreationIError) : throw sqlCeException;
      }
      else
      {
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_get_ErrorPointer(this.pIRda, ref this.pIErrors));
        NativeMethods.DllAddRef();
      }
    }

    public SqlCeRemoteDataAccess(string internetUrl, string localConnectionString)
    {
      NativeMethods.LoadNativeBinaries();
      this.pIRda = new IntPtr(0);
      IntPtr pCreationIError = new IntPtr(0);
      int hr = SqlCeRemoteDataAccess.uwrda_RemoteDataAccess(ref this.pIRda, ref pCreationIError);
      if (NativeMethods.Failed(hr))
      {
        SqlCeException sqlCeException = SqlCeException.FillErrorInformation(hr, pCreationIError);
        int num = IntPtr.Zero != pCreationIError ? (int) NativeMethods.uwutil_ReleaseCOMPtr(pCreationIError) : throw sqlCeException;
      }
      else
      {
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_get_ErrorPointer(this.pIRda, ref this.pIErrors));
        this.InternetUrl = internetUrl;
        this.LocalConnectionString = localConnectionString;
        NativeMethods.DllAddRef();
      }
    }

    public SqlCeRemoteDataAccess(
      string internetUrl,
      string internetLogin,
      string internetPassword,
      string localConnectionString)
    {
      NativeMethods.LoadNativeBinaries();
      this.pIRda = new IntPtr(0);
      IntPtr pCreationIError = new IntPtr(0);
      int hr = SqlCeRemoteDataAccess.uwrda_RemoteDataAccess(ref this.pIRda, ref pCreationIError);
      if (NativeMethods.Failed(hr))
      {
        SqlCeException sqlCeException = SqlCeException.FillErrorInformation(hr, pCreationIError);
        int num = IntPtr.Zero != pCreationIError ? (int) NativeMethods.uwutil_ReleaseCOMPtr(pCreationIError) : throw sqlCeException;
      }
      else
      {
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_get_ErrorPointer(this.pIRda, ref this.pIErrors));
        this.InternetUrl = internetUrl;
        this.LocalConnectionString = localConnectionString;
        this.InternetLogin = internetLogin;
        this.InternetPassword = internetPassword;
        NativeMethods.DllAddRef();
      }
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    ~SqlCeRemoteDataAccess() => this.Dispose(false);

    private void Dispose(bool disposing)
    {
      if (IntPtr.Zero != this.pIRda)
      {
        int num = (int) NativeMethods.uwutil_ReleaseCOMPtr(this.pIRda);
        this.pIRda = IntPtr.Zero;
      }
      if (IntPtr.Zero != this.pIErrors)
      {
        int num = (int) NativeMethods.uwutil_ReleaseCOMPtr(this.pIErrors);
        this.pIErrors = IntPtr.Zero;
      }
      GC.KeepAlive((object) this.pIRda);
      GC.KeepAlive((object) this.pIErrors);
      if (!disposing)
        return;
      NativeMethods.DllRelease();
    }

    public string LocalConnectionString
    {
      get
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        IntPtr rbz = new IntPtr(0);
        int connectionString = SqlCeRemoteDataAccess.uwrda_get_LocalConnectionString(this.pIRda, ref rbz);
        try
        {
          NativeMethods.CheckHRESULT(this.pIErrors, connectionString);
          return Marshal.PtrToStringUni(rbz);
        }
        finally
        {
          NativeMethods.uwutil_SysFreeString(rbz);
        }
      }
      set
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        StringBuilder stringBuilder = new StringBuilder();
        Hashtable connectionString = ConStringUtil.ParseConnectionString(ref value);
        foreach (string key in (IEnumerable) connectionString.Keys)
        {
          stringBuilder.Append(key);
          stringBuilder.Append("=\"");
          stringBuilder.Append(connectionString[(object) key]);
          stringBuilder.Append("\";");
        }
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_put_LocalConnectionString(this.pIRda, stringBuilder.ToString()));
      }
    }

    public string InternetUrl
    {
      get
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        IntPtr rbz = new IntPtr(0);
        int internetUrl = SqlCeRemoteDataAccess.uwrda_get_InternetUrl(this.pIRda, ref rbz);
        try
        {
          NativeMethods.CheckHRESULT(this.pIErrors, internetUrl);
          return Marshal.PtrToStringUni(rbz);
        }
        finally
        {
          NativeMethods.uwutil_SysFreeString(rbz);
        }
      }
      set
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_put_InternetUrl(this.pIRda, value));
      }
    }

    public string InternetLogin
    {
      get
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        IntPtr rbz = new IntPtr(0);
        int internetLogin = SqlCeRemoteDataAccess.uwrda_get_InternetLogin(this.pIRda, ref rbz);
        try
        {
          NativeMethods.CheckHRESULT(this.pIErrors, internetLogin);
          return Marshal.PtrToStringUni(rbz);
        }
        finally
        {
          NativeMethods.uwutil_SysFreeString(rbz);
        }
      }
      set
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_put_InternetLogin(this.pIRda, value));
      }
    }

    public string InternetPassword
    {
      get
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        IntPtr rbz = new IntPtr(0);
        int internetPassword = SqlCeRemoteDataAccess.uwrda_get_InternetPassword(this.pIRda, ref rbz);
        try
        {
          NativeMethods.CheckHRESULT(this.pIErrors, internetPassword);
          return Marshal.PtrToStringUni(rbz);
        }
        finally
        {
          NativeMethods.uwutil_SysFreeString(rbz);
        }
      }
      set
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_put_InternetPassword(this.pIRda, value));
      }
    }

    public bool ConnectionManager
    {
      get
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        bool ConnectionManager = false;
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_get_ConnectionManager(this.pIRda, ref ConnectionManager));
        return ConnectionManager;
      }
      set
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_put_ConnectionManager(this.pIRda, value));
      }
    }

    public string InternetProxyServer
    {
      get
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        IntPtr rbz = new IntPtr(0);
        int internetProxyServer = SqlCeRemoteDataAccess.uwrda_get_InternetProxyServer(this.pIRda, ref rbz);
        try
        {
          NativeMethods.CheckHRESULT(this.pIErrors, internetProxyServer);
          return Marshal.PtrToStringUni(rbz);
        }
        finally
        {
          NativeMethods.uwutil_SysFreeString(rbz);
        }
      }
      set
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_put_InternetProxyServer(this.pIRda, value));
      }
    }

    public string InternetProxyLogin
    {
      get
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        IntPtr rbz = new IntPtr(0);
        int internetProxyLogin = SqlCeRemoteDataAccess.uwrda_get_InternetProxyLogin(this.pIRda, ref rbz);
        try
        {
          NativeMethods.CheckHRESULT(this.pIErrors, internetProxyLogin);
          return Marshal.PtrToStringUni(rbz);
        }
        finally
        {
          NativeMethods.uwutil_SysFreeString(rbz);
        }
      }
      set
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_put_InternetProxyLogin(this.pIRda, value));
      }
    }

    public string InternetProxyPassword
    {
      get
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        IntPtr rbz = new IntPtr(0);
        int internetProxyPassword = SqlCeRemoteDataAccess.uwrda_get_InternetProxyPassword(this.pIRda, ref rbz);
        try
        {
          NativeMethods.CheckHRESULT(this.pIErrors, internetProxyPassword);
          return Marshal.PtrToStringUni(rbz);
        }
        finally
        {
          NativeMethods.uwutil_SysFreeString(rbz);
        }
      }
      set
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_put_InternetProxyPassword(this.pIRda, value));
      }
    }

    public int ConnectTimeout
    {
      get
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        int connectTimeout = 0;
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_get_ConnectTimeout(this.pIRda, ref connectTimeout));
        return connectTimeout;
      }
      set
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_put_ConnectTimeout(this.pIRda, value));
      }
    }

    public int SendTimeout
    {
      get
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        int SendTimeout = 0;
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_get_SendTimeout(this.pIRda, ref SendTimeout));
        return SendTimeout;
      }
      set
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_put_SendTimeout(this.pIRda, value));
      }
    }

    public int ReceiveTimeout
    {
      get
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        int ReceiveTimeout = 0;
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_get_ReceiveTimeout(this.pIRda, ref ReceiveTimeout));
        return ReceiveTimeout;
      }
      set
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_put_ReceiveTimeout(this.pIRda, value));
      }
    }

    private int DataSendTimeout
    {
      get
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        int DataSendTimeout = 0;
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_get_DataSendTimeout(this.pIRda, ref DataSendTimeout));
        return DataSendTimeout;
      }
      set
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_put_DataSendTimeout(this.pIRda, value));
      }
    }

    private int DataReceiveTimeout
    {
      get
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        int DataReceiveTimeout = 0;
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_get_DataReceiveTimeout(this.pIRda, ref DataReceiveTimeout));
        return DataReceiveTimeout;
      }
      set
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_put_DataReceiveTimeout(this.pIRda, value));
      }
    }

    private int ControlSendTimeout
    {
      get
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        int ControlSendTimeout = 0;
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_get_ControlSendTimeout(this.pIRda, ref ControlSendTimeout));
        return ControlSendTimeout;
      }
      set
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_put_ControlSendTimeout(this.pIRda, value));
      }
    }

    private int ControlReceiveTimeout
    {
      get
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        int ControlReceiveTimeout = 0;
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_get_ControlReceiveTimeout(this.pIRda, ref ControlReceiveTimeout));
        return ControlReceiveTimeout;
      }
      set
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_put_ControlReceiveTimeout(this.pIRda, value));
      }
    }

    public short ConnectionRetryTimeout
    {
      get
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        ushort ConnectionRetryTimeout = 0;
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_get_ConnectionRetryTimeout(this.pIRda, ref ConnectionRetryTimeout));
        return (short) ConnectionRetryTimeout;
      }
      set
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_put_ConnectionRetryTimeout(this.pIRda, (ushort) value));
      }
    }

    public short CompressionLevel
    {
      get
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        ushort CompressionLevel = 0;
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_get_CompressionLevel(this.pIRda, ref CompressionLevel));
        return (short) CompressionLevel;
      }
      set
      {
        if (IntPtr.Zero == this.pIRda)
          throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
        NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_put_CompressionLevel(this.pIRda, (ushort) value));
      }
    }

    public void Pull(string localTableName, string sqlSelectString, string oleDBConnectionString)
    {
      if (IntPtr.Zero == this.pIRda)
        throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
      NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_Pull(this.pIRda, localTableName, sqlSelectString, oleDBConnectionString, RdaTrackOption.TrackingOff, ""));
    }

    public void Pull(
      string localTableName,
      string sqlSelectString,
      string oleDBConnectionString,
      RdaTrackOption trackOption)
    {
      if (IntPtr.Zero == this.pIRda)
        throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
      NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_Pull(this.pIRda, localTableName, sqlSelectString, oleDBConnectionString, trackOption, ""));
    }

    public void Pull(
      string localTableName,
      string sqlSelectString,
      string oleDBConnectionString,
      RdaTrackOption trackOption,
      string errorTable)
    {
      if (IntPtr.Zero == this.pIRda)
        throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
      NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_Pull(this.pIRda, localTableName, sqlSelectString, oleDBConnectionString, trackOption, errorTable));
    }

    public void Push(string localTableName, string oleDBConnectionString)
    {
      if (IntPtr.Zero == this.pIRda)
        throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
      NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_Push(this.pIRda, localTableName, oleDBConnectionString, RdaBatchOption.BatchingOff));
    }

    public void Push(
      string localTableName,
      string oleDBConnectionString,
      RdaBatchOption batchOption)
    {
      if (IntPtr.Zero == this.pIRda)
        throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
      NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_Push(this.pIRda, localTableName, oleDBConnectionString, batchOption));
    }

    public void SubmitSql(string sqlString, string oleDBConnectionString)
    {
      if (IntPtr.Zero == this.pIRda)
        throw new ObjectDisposedException(nameof (SqlCeRemoteDataAccess));
      NativeMethods.CheckHRESULT(this.pIErrors, SqlCeRemoteDataAccess.uwrda_SubmitSql(this.pIRda, sqlString, oleDBConnectionString));
    }

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_RemoteDataAccess(ref IntPtr pIRda, ref IntPtr pCreationIError);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_get_ErrorPointer(IntPtr pIRda, ref IntPtr pIErrors);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_get_LocalConnectionString(IntPtr pIRda, ref IntPtr rbz);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_put_LocalConnectionString(
      IntPtr pIRda,
      [MarshalAs(UnmanagedType.LPWStr)] string zLocalConnectionString);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_get_InternetUrl(IntPtr pIRda, ref IntPtr rbz);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_put_InternetUrl(IntPtr pIRda, [MarshalAs(UnmanagedType.LPWStr)] string InternetUrl);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_get_InternetLogin(IntPtr pIRda, ref IntPtr rbz);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_put_InternetLogin(IntPtr pIRda, [MarshalAs(UnmanagedType.LPWStr)] string InternetLogin);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_get_InternetPassword(IntPtr pIRda, ref IntPtr rbz);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_put_InternetPassword(IntPtr pIRda, [MarshalAs(UnmanagedType.LPWStr)] string InternetPassword);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_get_InternetProxyServer(IntPtr pIRda, ref IntPtr rbz);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_put_InternetProxyServer(
      IntPtr pIRda,
      [MarshalAs(UnmanagedType.LPWStr)] string InternetProxyServer);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_get_InternetProxyLogin(IntPtr pIRda, ref IntPtr rbz);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_put_InternetProxyLogin(IntPtr pIRda, [MarshalAs(UnmanagedType.LPWStr)] string InternetProxyLogin);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_get_InternetProxyPassword(IntPtr pIRda, ref IntPtr rbz);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_put_InternetProxyPassword(
      IntPtr pIRda,
      [MarshalAs(UnmanagedType.LPWStr)] string InternetProxyPassword);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_get_ConnectTimeout(IntPtr pIRda, ref int connectTimeout);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_put_ConnectTimeout(IntPtr pIRda, int connectTimeout);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_get_SendTimeout(IntPtr pIRda, ref int SendTimeout);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_put_SendTimeout(IntPtr pIRda, int SendTimeout);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_get_ReceiveTimeout(IntPtr pIRda, ref int ReceiveTimeout);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_put_ReceiveTimeout(IntPtr pIRda, int ReceiveTimeout);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_get_DataSendTimeout(IntPtr pIRda, ref int DataSendTimeout);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_put_DataSendTimeout(IntPtr pIRda, int DataSendTimeout);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_get_DataReceiveTimeout(IntPtr pIRda, ref int DataReceiveTimeout);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_put_DataReceiveTimeout(IntPtr pIRda, int DataReceiveTimeout);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_get_ControlSendTimeout(IntPtr pIRda, ref int ControlSendTimeout);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_put_ControlSendTimeout(IntPtr pIRda, int ControlSendTimeout);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_get_ControlReceiveTimeout(
      IntPtr pIRda,
      ref int ControlReceiveTimeout);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_put_ControlReceiveTimeout(
      IntPtr pIRda,
      int ControlReceiveTimeout);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_get_ConnectionRetryTimeout(
      IntPtr pIRda,
      ref ushort ConnectionRetryTimeout);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_put_ConnectionRetryTimeout(
      IntPtr pIRda,
      ushort ConnectionRetryTimeout);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_get_CompressionLevel(IntPtr pIRda, ref ushort CompressionLevel);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_put_CompressionLevel(IntPtr pIRda, ushort CompressionLevel);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_get_ConnectionManager(IntPtr pIRda, ref bool ConnectionManager);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_put_ConnectionManager(IntPtr pIRda, bool ConnectionManager);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_Pull(
      IntPtr pIRda,
      [MarshalAs(UnmanagedType.LPWStr)] string zLocalTableName,
      [MarshalAs(UnmanagedType.LPWStr)] string zSqlSelectString,
      [MarshalAs(UnmanagedType.LPWStr)] string zOleDbConnectionString,
      RdaTrackOption trackOption,
      [MarshalAs(UnmanagedType.LPWStr)] string zErrorTable);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_Push(
      IntPtr pIRda,
      [MarshalAs(UnmanagedType.LPWStr)] string zLocalTableName,
      [MarshalAs(UnmanagedType.LPWStr)] string zOleDbConnectionString,
      RdaBatchOption batchOption);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrda_SubmitSql(
      IntPtr pIRda,
      [MarshalAs(UnmanagedType.LPWStr)] string zSqlString,
      [MarshalAs(UnmanagedType.LPWStr)] string zOleDbConnectionString);
  }
}
