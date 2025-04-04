// Decompiled with JetBrains decompiler
// Type: NHibernate.Collection.IPersistentCollection
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Loader;
using NHibernate.Persister.Collection;
using NHibernate.Type;
using System.Collections;
using System.Data;

#nullable disable
namespace NHibernate.Collection
{
  public interface IPersistentCollection
  {
    object Owner { get; set; }

    object GetValue();

    bool RowUpdatePossible { get; }

    object Key { get; }

    string Role { get; }

    bool IsUnreferenced { get; }

    bool IsDirty { get; }

    object StoredSnapshot { get; }

    bool Empty { get; }

    void SetSnapshot(object key, string role, object snapshot);

    void PostAction();

    void BeginRead();

    bool EndRead(ICollectionPersister persister);

    bool AfterInitialize(ICollectionPersister persister);

    bool IsDirectlyAccessible { get; }

    bool UnsetSession(ISessionImplementor currentSession);

    bool SetCurrentSession(ISessionImplementor session);

    void InitializeFromCache(ICollectionPersister persister, object disassembled, object owner);

    IEnumerable Entries(ICollectionPersister persister);

    object ReadFrom(
      IDataReader reader,
      ICollectionPersister role,
      ICollectionAliases descriptor,
      object owner);

    object GetIdentifier(object entry, int i);

    object GetIndex(object entry, int i, ICollectionPersister persister);

    object GetElement(object entry);

    object GetSnapshotElement(object entry, int i);

    void BeforeInitialize(ICollectionPersister persister, int anticipatedSize);

    bool EqualsSnapshot(ICollectionPersister persister);

    bool IsSnapshotEmpty(object snapshot);

    object Disassemble(ICollectionPersister persister);

    bool NeedsRecreate(ICollectionPersister persister);

    ICollection GetSnapshot(ICollectionPersister persister);

    void ForceInitialization();

    bool EntryExists(object entry, int i);

    bool NeedsInserting(object entry, int i, IType elemType);

    bool NeedsUpdating(object entry, int i, IType elemType);

    IEnumerable GetDeletes(ICollectionPersister persister, bool indexIsFormula);

    bool IsWrapper(object collection);

    bool WasInitialized { get; }

    bool HasQueuedOperations { get; }

    IEnumerable QueuedAdditionIterator { get; }

    ICollection GetQueuedOrphans(string entityName);

    void ClearDirty();

    void Dirty();

    void PreInsert(ICollectionPersister persister);

    void AfterRowInsert(ICollectionPersister persister, object entry, int i, object id);

    ICollection GetOrphans(object snapshot, string entityName);
  }
}
