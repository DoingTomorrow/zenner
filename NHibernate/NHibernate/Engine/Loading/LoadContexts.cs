// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.Loading.LoadContexts
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Collection;
using NHibernate.Impl;
using NHibernate.Persister.Collection;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace NHibernate.Engine.Loading
{
  public class LoadContexts
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (LoadContexts));
    [NonSerialized]
    private readonly IPersistenceContext persistenceContext;
    private IDictionary collectionLoadContexts;
    private Dictionary<CollectionKey, LoadingCollectionEntry> xrefLoadingCollectionEntries;

    public LoadContexts(IPersistenceContext persistenceContext)
    {
      this.persistenceContext = persistenceContext;
    }

    public IPersistenceContext PersistenceContext => this.persistenceContext;

    private ISessionImplementor Session => this.PersistenceContext.Session;

    internal IDictionary<CollectionKey, LoadingCollectionEntry> LoadingCollectionXRefs
    {
      get => (IDictionary<CollectionKey, LoadingCollectionEntry>) this.xrefLoadingCollectionEntries;
    }

    public virtual void Cleanup(IDataReader resultSet)
    {
      if (this.collectionLoadContexts == null)
        return;
      ((CollectionLoadContext) this.collectionLoadContexts[(object) resultSet]).Cleanup();
      this.collectionLoadContexts.Remove((object) resultSet);
    }

    public void Cleanup()
    {
      if (this.collectionLoadContexts == null)
        return;
      foreach (CollectionLoadContext collectionLoadContext in (IEnumerable) this.collectionLoadContexts.Values)
      {
        LoadContexts.log.Warn((object) ("fail-safe cleanup (collections) : " + (object) collectionLoadContext));
        collectionLoadContext.Cleanup();
      }
      this.collectionLoadContexts.Clear();
    }

    public bool HasLoadingCollectionEntries
    {
      get => this.collectionLoadContexts != null && this.collectionLoadContexts.Count != 0;
    }

    public bool HasRegisteredLoadingCollectionEntries
    {
      get
      {
        return this.xrefLoadingCollectionEntries != null && this.xrefLoadingCollectionEntries.Count != 0;
      }
    }

    public CollectionLoadContext GetCollectionLoadContext(IDataReader resultSet)
    {
      CollectionLoadContext collectionLoadContext = (CollectionLoadContext) null;
      if (this.collectionLoadContexts == null)
        this.collectionLoadContexts = IdentityMap.Instantiate(8);
      else
        collectionLoadContext = (CollectionLoadContext) this.collectionLoadContexts[(object) resultSet];
      if (collectionLoadContext == null)
      {
        if (LoadContexts.log.IsDebugEnabled)
          LoadContexts.log.Debug((object) ("constructing collection load context for result set [" + (object) resultSet + "]"));
        collectionLoadContext = new CollectionLoadContext(this, resultSet);
        this.collectionLoadContexts[(object) resultSet] = (object) collectionLoadContext;
      }
      return collectionLoadContext;
    }

    public IPersistentCollection LocateLoadingCollection(
      ICollectionPersister persister,
      object ownerKey)
    {
      LoadingCollectionEntry loadingCollectionEntry = this.LocateLoadingCollectionEntry(new CollectionKey(persister, ownerKey, this.Session.EntityMode));
      if (loadingCollectionEntry != null)
      {
        if (LoadContexts.log.IsDebugEnabled)
          LoadContexts.log.Debug((object) ("returning loading collection:" + MessageHelper.InfoString(persister, ownerKey, this.Session.Factory)));
        return loadingCollectionEntry.Collection;
      }
      if (LoadContexts.log.IsDebugEnabled)
        LoadContexts.log.Debug((object) ("creating collection wrapper:" + MessageHelper.InfoString(persister, ownerKey, this.Session.Factory)));
      return (IPersistentCollection) null;
    }

    internal void RegisterLoadingCollectionXRef(
      CollectionKey entryKey,
      LoadingCollectionEntry entry)
    {
      if (this.xrefLoadingCollectionEntries == null)
        this.xrefLoadingCollectionEntries = new Dictionary<CollectionKey, LoadingCollectionEntry>();
      this.xrefLoadingCollectionEntries[entryKey] = entry;
    }

    internal void UnregisterLoadingCollectionXRef(CollectionKey key)
    {
      if (!this.HasRegisteredLoadingCollectionEntries)
        return;
      this.xrefLoadingCollectionEntries.Remove(key);
    }

    internal LoadingCollectionEntry LocateLoadingCollectionEntry(CollectionKey key)
    {
      if (this.xrefLoadingCollectionEntries == null)
        return (LoadingCollectionEntry) null;
      if (LoadContexts.log.IsDebugEnabled)
        LoadContexts.log.Debug((object) ("attempting to locate loading collection entry [" + (object) key + "] in any result-set context"));
      LoadingCollectionEntry loadingCollectionEntry;
      this.xrefLoadingCollectionEntries.TryGetValue(key, out loadingCollectionEntry);
      if (LoadContexts.log.IsDebugEnabled)
        LoadContexts.log.Debug((object) string.Format("collection [{0}] {1} in load context", (object) key, loadingCollectionEntry == null ? (object) "located" : (object) "not located"));
      return loadingCollectionEntry;
    }

    internal void CleanupCollectionXRefs(IEnumerable entryKeys)
    {
      foreach (CollectionKey entryKey in entryKeys)
        this.xrefLoadingCollectionEntries.Remove(entryKey);
    }
  }
}
