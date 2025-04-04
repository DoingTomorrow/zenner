// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.SqlCeTransaction
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

using System.Data.Common;
using System.Runtime.InteropServices;

#nullable disable
namespace System.Data.SqlServerCe
{
  public sealed class SqlCeTransaction : DbTransaction
  {
    private object thisLock = new object();
    private bool isZombied;
    private bool isDisposed;
    private bool isFinalized;
    internal SqlCeConnection connection;
    private IsolationLevel isolationLevel;
    private IntPtr pQpSession;
    private IntPtr pTx;
    private IntPtr pError;
    private long ullTransactionBsn;
    private SqlCeChangeTracking m_tracking;

    protected override DbConnection DbConnection => (DbConnection) this.connection;

    internal SeTransactionFlags EngineFlags
    {
      get
      {
        this.EnterPublicAPI();
        SeTransactionFlags seTxFlags = SeTransactionFlags.NOFLAGS;
        this.ProcessResults(NativeMethods.GetTransactionFlags(this.ITransact, ref seTxFlags));
        return seTxFlags;
      }
      set
      {
        this.EnterPublicAPI();
        SeTransactionFlags seTxFlags = SeTransactionFlags.NOFLAGS;
        this.ProcessResults(NativeMethods.GetTransactionFlags(this.ITransact, ref seTxFlags));
        this.ProcessResults(NativeMethods.SetTransactionFlag(this.ITransact, seTxFlags, false, this.pError));
        this.ProcessResults(NativeMethods.SetTransactionFlag(this.ITransact, value, true, this.pError));
      }
    }

    private SqlCeConnection Connection => this.connection;

    public override IsolationLevel IsolationLevel
    {
      get
      {
        if (IntPtr.Zero == this.Connection.ITransact)
          throw new InvalidOperationException(Res.GetString("ADP_TransactionZombied", (object) this.GetType().Name));
        return this.isolationLevel;
      }
    }

    public Guid TrackingContext
    {
      get
      {
        this.EnterPublicAPI();
        string empty = string.Empty;
        IntPtr pGuidTrackingContext = (IntPtr) 0;
        int trackingContext = NativeMethods.GetTrackingContext(this.ITransact, out pGuidTrackingContext, this.pError);
        if (trackingContext != 0)
          this.ProcessResults(trackingContext);
        string stringBstr = Marshal.PtrToStringBSTR(pGuidTrackingContext);
        NativeMethods.uwutil_SysFreeString(pGuidTrackingContext);
        return new Guid(stringBstr);
      }
      set
      {
        this.EnterPublicAPI();
        IntPtr bstr = Marshal.StringToBSTR("{" + value.ToString() + "}");
        int hr = NativeMethods.SetTrackingContext(this.ITransact, ref bstr, this.pError);
        NativeMethods.uwutil_SysFreeString(bstr);
        this.ProcessResults(hr);
      }
    }

    public long CurrentTransactionBsn => this.ullTransactionBsn;

    internal bool IsZombied => this.isZombied;

    internal void SetTrackingObject(SqlCeChangeTracking trk) => this.m_tracking = trk;

    internal SqlCeTransaction(
      SqlCeConnection connection,
      IsolationLevel isolationLevel,
      IntPtr pTx,
      IntPtr pQpSession)
    {
      this.pTx = pTx;
      this.pQpSession = pQpSession;
      this.isZombied = false;
      this.isDisposed = false;
      this.isFinalized = false;
      this.isolationLevel = isolationLevel;
      this.connection = connection;
      this.ullTransactionBsn = 0L;
      this.m_tracking = (SqlCeChangeTracking) null;
      int errorInstance = NativeMethods.CreateErrorInstance(ref this.pError);
      if (errorInstance != 0)
        this.ProcessResults(errorInstance);
      int transactionBsn = NativeMethods.GetTransactionBsn(pTx, ref this.ullTransactionBsn, this.pError);
      if (transactionBsn == 0)
        return;
      this.ProcessResults(transactionBsn);
    }

    ~SqlCeTransaction() => this.Dispose(false);

    private void ReleaseNativeInterfaces()
    {
      if (IntPtr.Zero != this.pQpSession)
        NativeMethods.SafeRelease(ref this.pQpSession);
      if (IntPtr.Zero != this.pTx)
        NativeMethods.SafeRelease(ref this.pTx);
      if (!(IntPtr.Zero != this.pError))
        return;
      NativeMethods.SafeDelete(ref this.pError);
    }

    private void EnterPublicAPI()
    {
      if (this.isDisposed)
        throw new ObjectDisposedException(nameof (SqlCeTransaction));
      if (this.Connection == null || IntPtr.Zero == this.Connection.ITransact)
        this.isZombied = true;
      if (this.isZombied)
        throw new InvalidOperationException(Res.GetString("ADP_TransactionZombied", (object) this.GetType().Name));
    }

    public new void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected override void Dispose(bool disposing)
    {
      lock (this.thisLock)
      {
        if (this.isFinalized)
          return;
        if (disposing)
        {
          if (IntPtr.Zero != this.pTx)
            this.Rollback();
          this.isDisposed = true;
        }
        this.ReleaseNativeInterfaces();
        if (disposing)
          return;
        this.isFinalized = true;
      }
    }

    internal IntPtr ITransact => this.pTx;

    internal IntPtr IQPSession => this.pQpSession;

    public override void Commit() => this.Commit(CommitMode.Deferred);

    public void Commit(CommitMode mode)
    {
      this.EnterPublicAPI();
      if (this.connection.HasOpenedCursors(this))
        throw new InvalidOperationException(Res.GetString("SQLCE_OpenedCursorsOnTxCommit"));
      try
      {
        if (this.m_tracking != null)
          this.m_tracking.Dispose(true);
        int hr = NativeMethods.CommitTransaction(this.pTx, mode, this.pError);
        this.isZombied = true;
        if (hr != 0)
          this.ProcessResults(hr);
        if (this.connection == null)
          return;
        this.connection.Zombie(this);
        this.connection.RemoveWeakReference((object) this);
      }
      finally
      {
        this.Dispose(false);
      }
    }

    public override void Rollback()
    {
      this.EnterPublicAPI();
      if (this.connection.HasOpenedCursors(this))
        throw new InvalidOperationException(Res.GetString("SQLCE_OpenedCursorsOnTxAbort"));
      try
      {
        if (this.m_tracking != null)
          this.m_tracking.Dispose(true);
        int hr = NativeMethods.AbortTransaction(this.pTx, this.pError);
        if (hr != 0)
          this.ProcessResults(hr);
        this.isZombied = true;
        this.connection.Zombie(this);
        this.connection.RemoveWeakReference((object) this);
      }
      finally
      {
        this.Dispose(false);
      }
    }

    private void ProcessResults(int hr)
    {
      Exception exception = (Exception) this.connection.ProcessResults(hr, this.pError, (object) this);
      if (exception != null)
        throw exception;
    }
  }
}
