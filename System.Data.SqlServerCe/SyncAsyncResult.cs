// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.SyncAsyncResult
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

using System.Runtime.InteropServices;
using System.Threading;

#nullable disable
namespace System.Data.SqlServerCe
{
  internal class SyncAsyncResult : IAsyncResult
  {
    private ManualResetEvent completeEvent;
    private object callerContext;
    private AsyncCallback callerCompletionCallback;
    private OnStartTableUpload callerOnStartTableUpload;
    private OnStartTableDownload callerOnStartTableDownload;
    private OnSynchronization callerOnSynchronization;
    private SqlCeReplication callerReplobj;
    private bool isCompleted;
    private Exception exception;

    internal SyncAsyncResult(
      SqlCeReplication replication,
      AsyncCallback completionCallback,
      object context)
    {
      this.completeEvent = new ManualResetEvent(false);
      this.callerContext = context;
      this.callerCompletionCallback = completionCallback;
      this.callerReplobj = replication;
    }

    internal SyncAsyncResult(
      SqlCeReplication replication,
      AsyncCallback completionCallback,
      OnStartTableUpload onStartTableUpload,
      OnStartTableDownload onStartTableDownload,
      OnSynchronization onSynchronization,
      object context)
    {
      this.completeEvent = new ManualResetEvent(false);
      this.callerReplobj = replication;
      this.callerCompletionCallback = completionCallback;
      this.callerOnStartTableUpload = onStartTableUpload;
      this.callerOnStartTableDownload = onStartTableDownload;
      this.callerOnSynchronization = onSynchronization;
      this.callerContext = context;
    }

    internal void SyncThread()
    {
      try
      {
        if (this.callerOnStartTableUpload == null && this.callerOnStartTableDownload == null && this.callerOnSynchronization == null)
          this.callerReplobj.Synchronize();
        else
          this.BeginSyncAndStatusReporting();
      }
      catch (Exception ex)
      {
        this.exception = ex;
      }
      this.callerCompletionCallback((IAsyncResult) this);
      this.isCompleted = true;
      this.completeEvent.Set();
    }

    private void DispatchEventToCaller(SyncStatus status, string tablename, int percentComplete)
    {
      if (SyncStatus.Uploading == status)
      {
        if (this.callerOnStartTableUpload == null)
          return;
        this.callerOnStartTableUpload((IAsyncResult) this, tablename);
      }
      else if (SyncStatus.Downloading == status)
      {
        if (this.callerOnStartTableDownload == null)
          return;
        this.callerOnStartTableDownload((IAsyncResult) this, tablename);
      }
      else
      {
        if (SyncStatus.Synchronizing != status || this.callerOnSynchronization == null)
          return;
        this.callerOnSynchronization((IAsyncResult) this, percentComplete);
      }
    }

    private void BeginSyncAndStatusReporting()
    {
      IntPtr zero = IntPtr.Zero;
      NativeMethods.CheckHRESULT(this.callerReplobj.pIErrors, SyncAsyncResult.uwrepl_AsyncReplication(this.callerReplobj.pIReplication, ref zero));
      try
      {
        bool pCompleted;
        do
        {
          SyncStatus pSyncStatus = SyncStatus.Start;
          pCompleted = false;
          IntPtr rbzTableName = new IntPtr(0);
          string tablename = (string) null;
          int pPrecentCompleted = 0;
          int hr = SyncAsyncResult.uwrepl_WaitForNextStatusReport(zero, ref pSyncStatus, ref rbzTableName, ref pPrecentCompleted, ref pCompleted);
          try
          {
            NativeMethods.CheckHRESULT(this.callerReplobj.pIErrors, hr);
            tablename = Marshal.PtrToStringUni(rbzTableName);
          }
          finally
          {
            NativeMethods.uwutil_SysFreeString(rbzTableName);
          }
          this.DispatchEventToCaller(pSyncStatus, tablename, pPrecentCompleted);
        }
        while (!pCompleted);
        int pHr = 0;
        NativeMethods.CheckHRESULT(this.callerReplobj.pIErrors, SyncAsyncResult.uwrepl_GetSyncResult(zero, ref pHr));
        NativeMethods.CheckHRESULT(this.callerReplobj.pIErrors, pHr);
      }
      finally
      {
        if (IntPtr.Zero != zero)
        {
          int num = (int) NativeMethods.uwutil_ReleaseCOMPtr(zero);
        }
      }
    }

    public Exception GetException() => this.exception;

    bool IAsyncResult.IsCompleted => this.isCompleted;

    bool IAsyncResult.CompletedSynchronously => false;

    WaitHandle IAsyncResult.AsyncWaitHandle => (WaitHandle) this.completeEvent;

    object IAsyncResult.AsyncState => this.callerContext;

    [DllImport("sqlceme35.dll")]
    private static extern int uwrepl_AsyncReplication(
      IntPtr pIReplication,
      ref IntPtr pAsyncIReplication);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrepl_WaitForNextStatusReport(
      IntPtr pAsyncReplication,
      ref SyncStatus pSyncStatus,
      ref IntPtr rbzTableName,
      ref int pPrecentCompleted,
      ref bool pCompleted);

    [DllImport("sqlceme35.dll")]
    private static extern int uwrepl_GetSyncResult(IntPtr pIReplication, ref int pHr);
  }
}
