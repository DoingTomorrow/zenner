// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.CacheController
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using Microsoft.Synchronization.ClientServices.Common;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace Microsoft.Synchronization.ClientServices
{
  public class CacheController
  {
    private OfflineSyncProvider localProvider;
    private Uri serviceUri;
    private CacheControllerBehavior controllerBehavior;
    private HttpCacheRequestHandler cacheRequestHandler;
    private Guid changeSetId;
    private object lockObject = new object();
    private bool beginSessionComplete;

    public static void DebugMemory(string categoryName)
    {
    }

    public CacheControllerBehavior ControllerBehavior => this.controllerBehavior;

    public CacheController(Uri serviceUri, string scopeName, OfflineSyncProvider localProvider)
    {
      if (serviceUri == (Uri) null)
        throw new ArgumentNullException(nameof (serviceUri));
      if (string.IsNullOrEmpty(scopeName))
        throw new ArgumentNullException(nameof (scopeName));
      if (!serviceUri.Scheme.Equals("http", StringComparison.CurrentCultureIgnoreCase) && !serviceUri.Scheme.Equals("https", StringComparison.CurrentCultureIgnoreCase))
        throw new ArgumentException("Uri must be http or https schema", nameof (serviceUri));
      if (localProvider == null)
        throw new ArgumentNullException(nameof (localProvider));
      this.serviceUri = serviceUri;
      this.localProvider = localProvider;
      this.controllerBehavior = new CacheControllerBehavior();
      this.controllerBehavior.ScopeName = scopeName;
    }

    internal async Task<CacheRefreshStatistics> SynchronizeAsync()
    {
      CacheRefreshStatistics refreshStatistics = await this.SynchronizeAsync(CancellationToken.None);
      return refreshStatistics;
    }

    internal async Task<CacheRefreshStatistics> DownloadAsync(
      CancellationToken cancellationToken,
      IProgress<SyncProgressEvent> progress = null)
    {
      CacheRefreshStatistics statistics = new CacheRefreshStatistics();
      if (cancellationToken.IsCancellationRequested)
        cancellationToken.ThrowIfCancellationRequested();
      statistics.StartTime = DateTime.Now;
      progress?.Report(new SyncProgressEvent(SyncStage.StartingSync, TimeSpan.Zero));
      try
      {
        this.cacheRequestHandler = new HttpCacheRequestHandler(this.serviceUri, this.controllerBehavior);
        await this.localProvider.BeginSession();
        this.beginSessionComplete = true;
        if (cancellationToken.IsCancellationRequested)
          cancellationToken.ThrowIfCancellationRequested();
        statistics = await this.EnqueueDownloadRequest(statistics, cancellationToken, progress);
        statistics.EndTime = DateTime.Now;
        if (this.beginSessionComplete)
          this.localProvider.EndSession();
      }
      catch (OperationCanceledException ex)
      {
        statistics.EndTime = DateTime.Now;
        statistics.Cancelled = true;
        statistics.Error = (Exception) ex;
        if (this.beginSessionComplete)
          this.localProvider.EndSession();
      }
      catch (Exception ex)
      {
        statistics.EndTime = DateTime.Now;
        statistics.Error = ex;
        if (this.beginSessionComplete)
          this.localProvider.EndSession();
      }
      finally
      {
        this.ResetAsyncWorkerManager();
      }
      progress?.Report(new SyncProgressEvent(SyncStage.EndingSync, statistics.EndTime.Subtract(statistics.StartTime)));
      return statistics;
    }

    internal async Task<bool> IsVersionUpToDateDownload(
      CancellationToken cancellationToken,
      IProgress<SyncProgressEvent> progress = null)
    {
      CacheRefreshStatistics statistics = new CacheRefreshStatistics();
      bool isUpToDate = true;
      if (cancellationToken.IsCancellationRequested)
        cancellationToken.ThrowIfCancellationRequested();
      statistics.StartTime = DateTime.Now;
      progress?.Report(new SyncProgressEvent(SyncStage.StartingSync, TimeSpan.Zero));
      try
      {
        this.cacheRequestHandler = new HttpCacheRequestHandler(this.serviceUri, this.controllerBehavior);
        await this.localProvider.BeginSession();
        this.beginSessionComplete = true;
        if (cancellationToken.IsCancellationRequested)
          cancellationToken.ThrowIfCancellationRequested();
        isUpToDate = await this.EnqueuePartialDownloadRequest(statistics, cancellationToken, progress);
        statistics.EndTime = DateTime.Now;
        if (this.beginSessionComplete)
          this.localProvider.EndSession();
      }
      catch (OperationCanceledException ex)
      {
        statistics.EndTime = DateTime.Now;
        statistics.Cancelled = true;
        statistics.Error = (Exception) ex;
        if (this.beginSessionComplete)
          this.localProvider.EndSession();
      }
      catch (Exception ex)
      {
        statistics.EndTime = DateTime.Now;
        statistics.Error = ex;
        if (this.beginSessionComplete)
          this.localProvider.EndSession();
      }
      finally
      {
        this.ResetAsyncWorkerManager();
      }
      progress?.Report(new SyncProgressEvent(SyncStage.EndingSync, statistics.EndTime.Subtract(statistics.StartTime)));
      return isUpToDate;
    }

    internal async Task<bool> IsVersionUpToDateSend(
      CancellationToken cancellationToken,
      IProgress<SyncProgressEvent> progress = null)
    {
      CacheRefreshStatistics statistics = new CacheRefreshStatistics();
      bool isUpToDate = true;
      if (cancellationToken.IsCancellationRequested)
        cancellationToken.ThrowIfCancellationRequested();
      statistics.StartTime = DateTime.Now;
      progress?.Report(new SyncProgressEvent(SyncStage.StartingSync, TimeSpan.Zero));
      try
      {
        this.cacheRequestHandler = new HttpCacheRequestHandler(this.serviceUri, this.controllerBehavior);
        await this.localProvider.BeginSession();
        this.beginSessionComplete = true;
        if (cancellationToken.IsCancellationRequested)
          cancellationToken.ThrowIfCancellationRequested();
        isUpToDate = await this.EnqueuePartialUploadRequest(statistics, cancellationToken, progress);
        statistics.EndTime = DateTime.Now;
        if (this.beginSessionComplete)
          this.localProvider.EndSession();
      }
      catch (OperationCanceledException ex)
      {
        statistics.EndTime = DateTime.Now;
        statistics.Cancelled = true;
        statistics.Error = (Exception) ex;
        if (this.beginSessionComplete)
          this.localProvider.EndSession();
      }
      catch (Exception ex)
      {
        statistics.EndTime = DateTime.Now;
        statistics.Error = ex;
        if (this.beginSessionComplete)
          this.localProvider.EndSession();
      }
      finally
      {
        this.ResetAsyncWorkerManager();
      }
      progress?.Report(new SyncProgressEvent(SyncStage.EndingSync, statistics.EndTime.Subtract(statistics.StartTime)));
      return isUpToDate;
    }

    internal async Task<CacheRefreshStatistics> UploadAsync(
      CancellationToken cancellationToken,
      IProgress<SyncProgressEvent> progress = null)
    {
      CacheRefreshStatistics statistics = new CacheRefreshStatistics();
      if (cancellationToken.IsCancellationRequested)
        cancellationToken.ThrowIfCancellationRequested();
      statistics.StartTime = DateTime.Now;
      progress?.Report(new SyncProgressEvent(SyncStage.StartingSync, TimeSpan.Zero));
      try
      {
        this.cacheRequestHandler = new HttpCacheRequestHandler(this.serviceUri, this.controllerBehavior);
        await this.localProvider.BeginSession();
        this.beginSessionComplete = true;
        if (cancellationToken.IsCancellationRequested)
          cancellationToken.ThrowIfCancellationRequested();
        statistics = await this.EnqueueUploadRequest(statistics, cancellationToken, progress);
        statistics.EndTime = DateTime.Now;
        if (this.beginSessionComplete)
          this.localProvider.EndSession();
      }
      catch (OperationCanceledException ex)
      {
        statistics.EndTime = DateTime.Now;
        statistics.Cancelled = true;
        statistics.Error = (Exception) ex;
        if (this.beginSessionComplete)
          this.localProvider.EndSession();
      }
      catch (Exception ex)
      {
        statistics.EndTime = DateTime.Now;
        statistics.Error = ex;
        if (this.beginSessionComplete)
          this.localProvider.EndSession();
      }
      finally
      {
        this.ResetAsyncWorkerManager();
      }
      progress?.Report(new SyncProgressEvent(SyncStage.EndingSync, statistics.EndTime.Subtract(statistics.StartTime)));
      return statistics;
    }

    internal async Task<CacheRefreshStatistics> SynchronizeAsync(
      CancellationToken cancellationToken,
      IProgress<SyncProgressEvent> progress = null)
    {
      CacheRefreshStatistics statistics = new CacheRefreshStatistics();
      if (cancellationToken.IsCancellationRequested)
        cancellationToken.ThrowIfCancellationRequested();
      statistics.StartTime = DateTime.Now;
      progress?.Report(new SyncProgressEvent(SyncStage.StartingSync, TimeSpan.Zero));
      try
      {
        this.cacheRequestHandler = new HttpCacheRequestHandler(this.serviceUri, this.controllerBehavior);
        await this.localProvider.BeginSession();
        this.beginSessionComplete = true;
        if (cancellationToken.IsCancellationRequested)
          cancellationToken.ThrowIfCancellationRequested();
        statistics = await this.EnqueueUploadRequest(statistics, cancellationToken, progress);
        if (statistics.Error != null)
          throw new Exception("Error occured during Upload request.", statistics.Error);
        if (cancellationToken.IsCancellationRequested)
          cancellationToken.ThrowIfCancellationRequested();
        statistics = await this.EnqueueDownloadRequest(statistics, cancellationToken, progress);
        statistics.EndTime = DateTime.Now;
        if (this.beginSessionComplete)
          this.localProvider.EndSession();
      }
      catch (OperationCanceledException ex)
      {
        statistics.EndTime = DateTime.Now;
        statistics.Cancelled = true;
        statistics.Error = (Exception) ex;
        if (this.beginSessionComplete)
          this.localProvider.EndSession();
      }
      catch (Exception ex)
      {
        statistics.EndTime = DateTime.Now;
        statistics.Error = ex;
        if (this.beginSessionComplete)
          this.localProvider.EndSession();
      }
      finally
      {
        this.ResetAsyncWorkerManager();
      }
      progress?.Report(new SyncProgressEvent(SyncStage.EndingSync, statistics.EndTime.Subtract(statistics.StartTime)));
      return statistics;
    }

    private void ResetAsyncWorkerManager()
    {
      lock (this.lockObject)
      {
        this.cacheRequestHandler = (HttpCacheRequestHandler) null;
        this.controllerBehavior.Locked = false;
        this.beginSessionComplete = false;
      }
    }

    private async Task<CacheRefreshStatistics> EnqueueUploadRequest(
      CacheRefreshStatistics statistics,
      CancellationToken cancellationToken,
      IProgress<SyncProgressEvent> progress = null)
    {
      this.changeSetId = Guid.NewGuid();
      try
      {
        if (cancellationToken.IsCancellationRequested)
          cancellationToken.ThrowIfCancellationRequested();
        DateTime durationStartDate = DateTime.Now;
        ChangeSet changeSet = await this.localProvider.GetChangeSet(this.changeSetId);
        DateTime now;
        if (progress != null)
        {
          IProgress<SyncProgressEvent> progress1 = progress;
          now = DateTime.Now;
          SyncProgressEvent syncProgressEvent = new SyncProgressEvent(SyncStage.GetChanges, now.Subtract(durationStartDate), changes: changeSet != null ? changeSet.Data : (ICollection<IOfflineEntity>) null);
          progress1.Report(syncProgressEvent);
        }
        if (changeSet == null || changeSet.Data == null || changeSet.Data.Count == 0)
          return statistics;
        CacheRequest request = new CacheRequest()
        {
          RequestId = this.changeSetId,
          Format = this.ControllerBehavior.SerializationFormat,
          RequestType = CacheRequestType.UploadChanges,
          Changes = changeSet.Data,
          KnowledgeBlob = changeSet.ServerBlob,
          IsLastBatch = changeSet.IsLastBatch
        };
        durationStartDate = DateTime.Now;
        CacheRequestResult requestResult = await this.cacheRequestHandler.ProcessCacheRequestAsync(request, (object) changeSet.IsLastBatch, cancellationToken);
        CacheRefreshStatistics refreshStatistics = await this.ProcessCacheRequestResults(statistics, requestResult, cancellationToken);
        statistics = refreshStatistics;
        refreshStatistics = (CacheRefreshStatistics) null;
        if (progress != null)
        {
          IProgress<SyncProgressEvent> progress2 = progress;
          now = DateTime.Now;
          SyncProgressEvent syncProgressEvent = new SyncProgressEvent(SyncStage.UploadingChanges, now.Subtract(durationStartDate), changes: changeSet.Data, conflicts: (ICollection<Conflict>) requestResult.ChangeSetResponse.Conflicts, updatedItems: (ICollection<IOfflineEntity>) requestResult.ChangeSetResponse.UpdatedItems);
          progress2.Report(syncProgressEvent);
        }
        changeSet = (ChangeSet) null;
        request = (CacheRequest) null;
        requestResult = (CacheRequestResult) null;
      }
      catch (OperationCanceledException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        if (ExceptionUtility.IsFatal(ex))
          throw;
        else
          statistics.Error = ex;
      }
      return statistics;
    }

    private async Task<bool> EnqueuePartialUploadRequest(
      CacheRefreshStatistics statistics,
      CancellationToken cancellationToken,
      IProgress<SyncProgressEvent> progress = null)
    {
      this.changeSetId = Guid.NewGuid();
      bool isUpToDate = true;
      try
      {
        if (cancellationToken.IsCancellationRequested)
          cancellationToken.ThrowIfCancellationRequested();
        ChangeSet changeSet = await this.localProvider.GetChangeSet(this.changeSetId);
        if (changeSet != null && changeSet.Data.Count > 0)
          isUpToDate = false;
        changeSet = (ChangeSet) null;
      }
      catch (OperationCanceledException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        if (ExceptionUtility.IsFatal(ex))
          throw;
        else
          statistics.Error = ex;
      }
      return isUpToDate;
    }

    private async Task<bool> EnqueuePartialDownloadRequest(
      CacheRefreshStatistics statistics,
      CancellationToken cancellationToken,
      IProgress<SyncProgressEvent> progress = null)
    {
      bool isUpToDate = true;
      try
      {
        if (cancellationToken.IsCancellationRequested)
          cancellationToken.ThrowIfCancellationRequested();
        bool isLastBatch = false;
        while (!isLastBatch)
        {
          if (cancellationToken.IsCancellationRequested)
            cancellationToken.ThrowIfCancellationRequested();
          CacheRequest request = new CacheRequest()
          {
            Format = this.ControllerBehavior.SerializationFormat,
            RequestType = CacheRequestType.DownloadChanges,
            KnowledgeBlob = this.localProvider.GetServerBlob()
          };
          DateTime durationStartDate = DateTime.Now;
          CacheRequestResult requestResult = await this.cacheRequestHandler.ProcessCacheRequestAsync(request, (object) null, cancellationToken);
          if (requestResult.ChangeSet == null || requestResult.ChangeSet.IsLastBatch)
            isLastBatch = true;
          if (requestResult.ChangeSet != null && requestResult.ChangeSet.Data.Count > 0)
            isUpToDate = false;
          progress?.Report(new SyncProgressEvent(SyncStage.DownloadingChanges, DateTime.Now.Subtract(durationStartDate), changes: requestResult.ChangeSet != null ? requestResult.ChangeSet.Data : (ICollection<IOfflineEntity>) null));
          request = (CacheRequest) null;
          requestResult = (CacheRequestResult) null;
        }
      }
      catch (OperationCanceledException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        if (ExceptionUtility.IsFatal(ex))
          throw;
        else
          statistics.Error = ex;
      }
      return isUpToDate;
    }

    private async Task<CacheRefreshStatistics> EnqueueDownloadRequest(
      CacheRefreshStatistics statistics,
      CancellationToken cancellationToken,
      IProgress<SyncProgressEvent> progress = null)
    {
      try
      {
        if (cancellationToken.IsCancellationRequested)
          cancellationToken.ThrowIfCancellationRequested();
        bool isLastBatch = false;
        while (!isLastBatch)
        {
          if (cancellationToken.IsCancellationRequested)
            cancellationToken.ThrowIfCancellationRequested();
          CacheRequest request = new CacheRequest()
          {
            Format = this.ControllerBehavior.SerializationFormat,
            RequestType = CacheRequestType.DownloadChanges,
            KnowledgeBlob = this.localProvider.GetServerBlob()
          };
          DateTime durationStartDate = DateTime.Now;
          CacheRequestResult requestResult = await this.cacheRequestHandler.ProcessCacheRequestAsync(request, (object) null, cancellationToken);
          CacheRefreshStatistics refreshStatistics = await this.ProcessCacheRequestResults(statistics, requestResult, cancellationToken);
          statistics = refreshStatistics;
          refreshStatistics = (CacheRefreshStatistics) null;
          if (requestResult.ChangeSet == null || requestResult.ChangeSet.IsLastBatch)
            isLastBatch = true;
          progress?.Report(new SyncProgressEvent(SyncStage.DownloadingChanges, DateTime.Now.Subtract(durationStartDate), changes: requestResult.ChangeSet != null ? requestResult.ChangeSet.Data : (ICollection<IOfflineEntity>) null));
          request = (CacheRequest) null;
          requestResult = (CacheRequestResult) null;
        }
      }
      catch (OperationCanceledException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        if (ExceptionUtility.IsFatal(ex))
          throw;
        else
          statistics.Error = ex;
      }
      return statistics;
    }

    private async Task<CacheRefreshStatistics> ProcessCacheRequestResults(
      CacheRefreshStatistics statistics,
      CacheRequestResult cacheRequestResult,
      CancellationToken cancellationToken)
    {
      try
      {
        if (cancellationToken.IsCancellationRequested)
          cancellationToken.ThrowIfCancellationRequested();
        if (cacheRequestResult.Error != null)
        {
          if (cacheRequestResult.ChangeSetResponse != null && cacheRequestResult.HttpStep == HttpState.End)
            await this.localProvider.OnChangeSetUploaded(cacheRequestResult.Id, cacheRequestResult.ChangeSetResponse);
          statistics.Error = cacheRequestResult.Error;
          return statistics;
        }
        if (cacheRequestResult.ChangeSetResponse != null)
        {
          if (cacheRequestResult.ChangeSetResponse.Error == null && cacheRequestResult.HttpStep == HttpState.End)
            await this.localProvider.OnChangeSetUploaded(cacheRequestResult.Id, cacheRequestResult.ChangeSetResponse);
          if (cacheRequestResult.ChangeSetResponse.Error != null)
          {
            statistics.Error = cacheRequestResult.ChangeSetResponse.Error;
            return statistics;
          }
          statistics.TotalChangeSetsUploaded++;
          statistics.TotalUploads += cacheRequestResult.BatchUploadCount;
          foreach (Conflict e1 in cacheRequestResult.ChangeSetResponse.ConflictsInternal)
          {
            if (e1 is SyncConflict)
              statistics.TotalSyncConflicts++;
            else
              statistics.TotalSyncErrors++;
          }
          return statistics;
        }
        if (cacheRequestResult.ChangeSet != null && cacheRequestResult.ChangeSet.Data != null && cacheRequestResult.ChangeSet.Data.Count > 0)
        {
          statistics.TotalChangeSetsDownloaded++;
          statistics.TotalDownloads += (uint) cacheRequestResult.ChangeSet.Data.Count;
          await this.localProvider.SaveChangeSet(cacheRequestResult.ChangeSet);
        }
        return statistics;
      }
      catch (OperationCanceledException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        if (ExceptionUtility.IsFatal(ex))
          throw;
        else
          statistics.Error = ex;
      }
      return statistics;
    }
  }
}
