// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.CollectionEntry
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Collection;
using NHibernate.Impl;
using NHibernate.Persister.Collection;
using System;
using System.Collections;

#nullable disable
namespace NHibernate.Engine
{
  [Serializable]
  public class CollectionEntry
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (CollectionEntry));
    private object snapshot;
    private string role;
    [NonSerialized]
    private ICollectionPersister loadedPersister;
    private object loadedKey;
    [NonSerialized]
    private bool reached;
    [NonSerialized]
    private bool processed;
    [NonSerialized]
    private bool doupdate;
    [NonSerialized]
    private bool doremove;
    [NonSerialized]
    private bool dorecreate;
    [NonSerialized]
    private bool ignore;
    [NonSerialized]
    private ICollectionPersister currentPersister;
    [NonSerialized]
    private object currentKey;

    public CollectionEntry(ICollectionPersister persister, IPersistentCollection collection)
    {
      this.ignore = false;
      collection.ClearDirty();
      this.snapshot = persister.IsMutable ? (object) collection.GetSnapshot(persister) : (object) (ICollection) null;
      collection.SetSnapshot(this.loadedKey, this.role, this.snapshot);
    }

    public CollectionEntry(
      IPersistentCollection collection,
      ICollectionPersister loadedPersister,
      object loadedKey,
      bool ignore)
    {
      this.ignore = ignore;
      this.loadedKey = loadedKey;
      this.SetLoadedPersister(loadedPersister);
      collection.SetSnapshot(loadedKey, this.role, (object) null);
    }

    public CollectionEntry(ICollectionPersister loadedPersister, object loadedKey)
    {
      this.ignore = false;
      this.loadedKey = loadedKey;
      this.SetLoadedPersister(loadedPersister);
    }

    internal CollectionEntry(IPersistentCollection collection, ISessionFactoryImplementor factory)
    {
      this.ignore = false;
      this.loadedKey = collection.Key;
      this.SetLoadedPersister(factory.GetCollectionPersister(collection.Role));
      this.snapshot = collection.StoredSnapshot;
    }

    public object Key => this.loadedKey;

    public string Role
    {
      get => this.role;
      set => this.role = value;
    }

    public object Snapshot => this.snapshot;

    public bool IsReached
    {
      get => this.reached;
      set => this.reached = value;
    }

    public bool IsProcessed
    {
      get => this.processed;
      set => this.processed = value;
    }

    public bool IsDoupdate
    {
      get => this.doupdate;
      set => this.doupdate = value;
    }

    public bool IsDoremove
    {
      get => this.doremove;
      set => this.doremove = value;
    }

    public bool IsDorecreate
    {
      get => this.dorecreate;
      set => this.dorecreate = value;
    }

    public bool IsIgnore => this.ignore;

    public ICollectionPersister CurrentPersister
    {
      get => this.currentPersister;
      set => this.currentPersister = value;
    }

    public object CurrentKey
    {
      get => this.currentKey;
      set => this.currentKey = value;
    }

    public object LoadedKey => this.loadedKey;

    public ICollectionPersister LoadedPersister => this.loadedPersister;

    public bool WasDereferenced => this.loadedKey == null;

    private void Dirty(IPersistentCollection collection)
    {
      if (!collection.WasInitialized || collection.IsDirty || this.LoadedPersister == null || !this.LoadedPersister.IsMutable || !collection.IsDirectlyAccessible && !this.LoadedPersister.ElementType.IsMutable || collection.EqualsSnapshot(this.LoadedPersister))
        return;
      collection.Dirty();
    }

    public void PreFlush(IPersistentCollection collection)
    {
      if (collection.IsDirty && this.LoadedPersister != null && !this.LoadedPersister.IsMutable)
        throw new HibernateException("changed an immutable collection instance: " + MessageHelper.InfoString(this.LoadedPersister.Role, this.LoadedKey));
      this.Dirty(collection);
      if (CollectionEntry.log.IsDebugEnabled && collection.IsDirty && this.loadedPersister != null)
        CollectionEntry.log.Debug((object) ("Collection dirty: " + MessageHelper.InfoString(this.loadedPersister, this.loadedKey)));
      this.doupdate = false;
      this.doremove = false;
      this.dorecreate = false;
      this.reached = false;
      this.processed = false;
    }

    public void PostInitialize(IPersistentCollection collection)
    {
      this.snapshot = this.LoadedPersister.IsMutable ? (object) collection.GetSnapshot(this.LoadedPersister) : (object) (ICollection) null;
      collection.SetSnapshot(this.loadedKey, this.role, this.snapshot);
    }

    public void PostFlush(IPersistentCollection collection)
    {
      if (this.IsIgnore)
        this.ignore = false;
      else if (!this.IsProcessed)
        throw new AssertionFailure("collection [" + collection.Role + "] was not processed by flush()");
      collection.SetSnapshot(this.loadedKey, this.role, this.snapshot);
    }

    public void AfterAction(IPersistentCollection collection)
    {
      this.loadedKey = this.CurrentKey;
      this.SetLoadedPersister(this.CurrentPersister);
      if (collection.WasInitialized && (this.IsDoremove || this.IsDorecreate || this.IsDoupdate))
        this.snapshot = this.loadedPersister == null || !this.loadedPersister.IsMutable ? (object) (ICollection) null : (object) collection.GetSnapshot(this.loadedPersister);
      collection.PostAction();
    }

    private void SetLoadedPersister(ICollectionPersister persister)
    {
      this.loadedPersister = persister;
      this.Role = persister == null ? (string) null : persister.Role;
    }

    internal void AfterDeserialize(ISessionFactoryImplementor factory)
    {
      this.loadedPersister = factory.GetCollectionPersister(this.role);
    }

    public ICollection GetOrphans(string entityName, IPersistentCollection collection)
    {
      if (this.snapshot == null)
        throw new AssertionFailure("no collection snapshot for orphan delete");
      return collection.GetOrphans(this.snapshot, entityName);
    }

    public bool IsSnapshotEmpty(IPersistentCollection collection)
    {
      return collection.WasInitialized && (this.LoadedPersister == null || this.LoadedPersister.IsMutable) && collection.IsSnapshotEmpty(this.Snapshot);
    }

    public override string ToString()
    {
      string str = nameof (CollectionEntry) + MessageHelper.InfoString(this.loadedPersister.Role, this.loadedKey);
      if (this.currentPersister != null)
        str = str + "->" + MessageHelper.InfoString(this.currentPersister.Role, this.currentKey);
      return str;
    }
  }
}
