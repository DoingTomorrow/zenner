// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.SQLite.SQLiteContext
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using Microsoft.Synchronization.ClientServices.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace Microsoft.Synchronization.ClientServices.SQLite
{
  public class SQLiteContext : OfflineSyncProvider, IDisposable
  {
    private volatile bool loaded;
    private readonly Uri scopeUri;
    private readonly string scopeName;
    private readonly string databaseName;
    private CacheController cacheController;
    private readonly OfflineSchema schema;
    private volatile bool syncActive;
    private bool isDisposed;

    internal SQLiteManager Manager { get; private set; }

    public SQLiteConfiguration Configuration { get; set; }

    public ReadOnlyCollection<Conflict> Conflicts { get; private set; }

    public string DatabaseName
    {
      get
      {
        this.ThrowIfDisposed();
        return this.databaseName;
      }
    }

    public OfflineSchema Schema
    {
      get
      {
        this.ThrowIfDisposed();
        return this.schema;
      }
    }

    public Uri ScopeUri
    {
      get
      {
        this.ThrowIfDisposed();
        return this.scopeUri;
      }
    }

    public CacheController CacheController
    {
      get
      {
        this.ThrowIfDisposed();
        return this.cacheController;
      }
    }

    public SQLiteContext(OfflineSchema schema, string scopeName, string datbasePath, Uri uri)
    {
      if (schema == null)
        throw new ArgumentNullException("OfflineSchema");
      if (string.IsNullOrEmpty(scopeName))
        throw new ArgumentNullException(nameof (scopeName));
      if (string.IsNullOrEmpty(datbasePath))
        throw new ArgumentNullException("cachePath");
      if (uri == (Uri) null)
        throw new ArgumentNullException(nameof (uri));
      this.isDisposed = false;
      this.schema = schema;
      this.scopeUri = uri;
      this.scopeName = scopeName;
      this.databaseName = datbasePath;
      string databaseName = this.databaseName;
      this.Manager = new SQLiteManager(schema, databaseName);
      this.CreateCacheController();
    }

    public async Task LoadSchemaAsync()
    {
      await this.LoadSchemaAsync(CancellationToken.None, (IProgress<SyncProgressEvent>) null);
    }

    public async Task LoadSchemaAsync(
      CancellationToken cancellationToken,
      IProgress<SyncProgressEvent> progress)
    {
      this.ThrowIfDisposed();
      if (this.loaded)
        return;
      await this.CheckSchemaAndUriAsync(this.schema, this.scopeUri, this.scopeName, cancellationToken, progress);
      this.loaded = true;
    }

    public async Task<CacheRefreshStatistics> SynchronizeAsync()
    {
      if (!this.loaded)
        await this.LoadSchemaAsync();
      CacheRefreshStatistics refreshStatistics = await this.cacheController.SynchronizeAsync();
      return refreshStatistics;
    }

    public async Task<CacheRefreshStatistics> SynchronizeAsync(
      CancellationToken cancellationToken,
      IProgress<SyncProgressEvent> progress)
    {
      if (!this.loaded)
        await this.LoadSchemaAsync(cancellationToken, progress);
      CacheRefreshStatistics refreshStatistics = await this.cacheController.SynchronizeAsync(cancellationToken, progress);
      return refreshStatistics;
    }

    public async Task<CacheRefreshStatistics> DownloadAsync(
      CancellationToken cancellationToken,
      IProgress<SyncProgressEvent> progress)
    {
      if (!this.loaded)
        await this.LoadSchemaAsync(cancellationToken, progress);
      CacheRefreshStatistics refreshStatistics = await this.cacheController.DownloadAsync(cancellationToken, progress);
      return refreshStatistics;
    }

    public async Task<bool> IsVersionUpToDateDownload(
      CancellationToken cancellationToken,
      IProgress<SyncProgressEvent> progress)
    {
      if (!this.loaded)
        await this.LoadSchemaAsync(cancellationToken, progress);
      bool dateDownload = await this.cacheController.IsVersionUpToDateDownload(cancellationToken, progress);
      return dateDownload;
    }

    public async Task<bool> IsVersionUpToDateSend(
      CancellationToken cancellationToken,
      IProgress<SyncProgressEvent> progress)
    {
      if (!this.loaded)
        await this.LoadSchemaAsync(cancellationToken, progress);
      bool dateSend = await this.cacheController.IsVersionUpToDateSend(cancellationToken, progress);
      return dateSend;
    }

    public async Task<CacheRefreshStatistics> UploadAsync(
      CancellationToken cancellationToken,
      IProgress<SyncProgressEvent> progress)
    {
      if (!this.loaded)
        await this.LoadSchemaAsync(cancellationToken, progress);
      CacheRefreshStatistics refreshStatistics = await this.cacheController.UploadAsync(cancellationToken, progress);
      return refreshStatistics;
    }

    public override async Task BeginSession()
    {
      this.ThrowIfDisposed();
      if (this.syncActive)
        throw new InvalidOperationException("Sync session already active");
      await this.InternalBeginSessionAsync();
    }

    private async Task InternalBeginSessionAsync()
    {
      this.syncActive = true;
      if (this.loaded)
        return;
      await this.LoadSchemaAsync();
    }

    public override async Task<ChangeSet> GetChangeSet(Guid state)
    {
      ChangeSet changeSet = await Task.Run<ChangeSet>((Func<ChangeSet>) (() =>
      {
        this.ThrowIfDisposed();
        if (!this.syncActive)
          throw new InvalidOperationException("GetChangeSet cannot be called without calling BeginSession");
        ChangeSet changeSet1 = new ChangeSet();
        IEnumerable<IOfflineEntity> changes = this.Manager.GetChanges(state, this.Configuration.LastSyncDate);
        changeSet1.Data = (ICollection<IOfflineEntity>) changes.ToList<IOfflineEntity>();
        changeSet1.IsLastBatch = true;
        changeSet1.ServerBlob = this.Configuration.AnchorBlob;
        return changeSet1;
      }));
      return changeSet;
    }

    public override async Task OnChangeSetUploaded(Guid state, ChangeSetResponse response)
    {
      await Task.Run((Action) (() =>
      {
        this.ThrowIfDisposed();
        if (response == null)
          throw new ArgumentNullException(nameof (response));
        if (!this.syncActive)
          throw new InvalidOperationException("OnChangeSetUploaded cannot be called without calling BeginSession");
        if (response.Error != null)
          return;
        IEnumerable<SQLiteOfflineEntity> second = (IEnumerable<SQLiteOfflineEntity>) null;
        IEnumerable<SQLiteOfflineEntity> liteOfflineEntities1 = (IEnumerable<SQLiteOfflineEntity>) null;
        if (response.Conflicts != null)
        {
          second = response.Conflicts.Select<Conflict, SQLiteOfflineEntity>((Func<Conflict, SQLiteOfflineEntity>) (c => (SQLiteOfflineEntity) c.LiveEntity));
          this.Conflicts = response.Conflicts;
        }
        if (response.UpdatedItems != null && response.UpdatedItems.Count > 0)
          liteOfflineEntities1 = response.UpdatedItems.Cast<SQLiteOfflineEntity>();
        IEnumerable<SQLiteOfflineEntity> liteOfflineEntities2 = liteOfflineEntities1 ?? (IEnumerable<SQLiteOfflineEntity>) new List<SQLiteOfflineEntity>();
        if (second != null)
          liteOfflineEntities2 = liteOfflineEntities2.Concat<SQLiteOfflineEntity>(second);
        this.Manager.SaveDownloadedChanges(liteOfflineEntities2);
        this.Manager.UploadSucceeded(state);
        this.Configuration.AnchorBlob = response.ServerBlob;
        this.Configuration.LastSyncDate = DateTime.UtcNow;
      }));
    }

    public override byte[] GetServerBlob()
    {
      this.ThrowIfDisposed();
      if (!this.syncActive)
        throw new InvalidOperationException("GetServerBlob cannot be called without calling BeginSession");
      return this.Configuration.AnchorBlob;
    }

    public override async Task SaveChangeSet(ChangeSet changeSet)
    {
      await Task.Run((Action) (() =>
      {
        this.ThrowIfDisposed();
        if (changeSet == null)
          throw new ArgumentNullException(nameof (changeSet));
        if (!this.syncActive)
          throw new InvalidOperationException("SaveChangeSet cannot be called without calling BeginSession");
        this.Manager.SaveDownloadedChanges(changeSet.Data.Cast<SQLiteOfflineEntity>());
        this.Configuration.AnchorBlob = changeSet.ServerBlob;
        this.Manager.SaveConfiguration(this.Configuration);
      }));
    }

    public override void EndSession()
    {
      this.ThrowIfDisposed();
      this.syncActive = this.syncActive ? false : throw new InvalidOperationException("Sync session not active");
    }

    public void Close() => this.Dispose();

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.isDisposed)
        return;
      if (disposing)
        this.Manager = (SQLiteManager) null;
      this.isDisposed = true;
    }

    private void ThrowIfDisposed()
    {
      if (this.isDisposed)
        throw new ObjectDisposedException("Cannot access a disposed IsolatedStorageOfflineContext");
    }

    public void AddScopeParameters(string key, string value)
    {
      this.CacheController.ControllerBehavior.AddScopeParameters(key, value);
    }

    private void CreateCacheController()
    {
      this.cacheController = new CacheController(this.scopeUri, this.scopeName, (OfflineSyncProvider) this);
      CacheControllerBehavior controllerBehavior = this.cacheController.ControllerBehavior;
      MethodInfo declaredMethod = controllerBehavior.GetType().GetTypeInfo().GetDeclaredMethod("AddType");
      foreach (Type collection in this.schema.Collections)
        declaredMethod.MakeGenericMethod(collection).Invoke((object) controllerBehavior, new object[0]);
    }

    private async Task CheckSchemaAndUriAsync(
      OfflineSchema offlineSchema,
      Uri uri,
      string scope,
      CancellationToken cancellationToken,
      IProgress<SyncProgressEvent> progress)
    {
      await Task.Run((Action) (() =>
      {
        DateTime now1 = DateTime.Now;
        this.Configuration = this.Manager.ReadConfiguration(scope);
        progress?.Report(new SyncProgressEvent(SyncStage.ReadingConfiguration, DateTime.Now.Subtract(now1)));
        if (this.Configuration != null)
        {
          if (this.Configuration.ServiceUri.AbsoluteUri != uri.AbsoluteUri)
            throw new ArgumentException("Specified uri does not match uri previously used for the specified database");
          List<Type> list = offlineSchema.Collections.ToList<Type>();
          list.Sort((Comparison<Type>) ((x, y) => string.Compare(x.FullName, y.FullName, StringComparison.Ordinal)));
          if (list.Count != this.Configuration.Types.Count)
            throw new ArgumentException("Specified offlineSchema does not match database Offline schema previously used for cache path");
          this.Configuration.Types.Sort((Comparison<string>) ((x, y) => string.Compare(x, y, StringComparison.Ordinal)));
          if (list.Where<Type>((Func<Type, int, bool>) ((t, i) => t.FullName != this.Configuration.Types[i])).Any<Type>())
            throw new ArgumentException("Specified offlineSchema does not match database Offline schema previously used for cache path");
        }
        else
        {
          if (!this.Manager.ScopeTableExist())
          {
            DateTime now2 = DateTime.Now;
            this.Manager.CreateScopeTable();
            progress?.Report(new SyncProgressEvent(SyncStage.CreatingScope, DateTime.Now.Subtract(now2)));
          }
          List<string> list = offlineSchema.Collections.Select<Type, string>((Func<Type, string>) (type => type.FullName)).ToList<string>();
          list.Sort();
          this.Configuration = new SQLiteConfiguration()
          {
            AnchorBlob = (byte[]) null,
            LastSyncDate = new DateTime(1900, 1, 1),
            ScopeName = scope,
            ServiceUri = uri,
            Types = list
          };
          DateTime now3 = DateTime.Now;
          this.Manager.SaveConfiguration(this.Configuration);
          progress?.Report(new SyncProgressEvent(SyncStage.ApplyingConfiguration, DateTime.Now.Subtract(now3)));
        }
        if (this.schema == null || this.schema.Collections == null || this.schema.Collections.Count == 0)
          return;
        DateTime now4 = DateTime.Now;
        foreach (Type table in this.schema.Collections.Where<Type>((Func<Type, bool>) (table => table.Name != SQLiteConstants.ScopeInfo)))
          this.Manager.CreateTable(table);
        progress?.Report(new SyncProgressEvent(SyncStage.CheckingTables, DateTime.Now.Subtract(now4)));
      }));
    }
  }
}
