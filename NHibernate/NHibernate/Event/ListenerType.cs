// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.ListenerType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Event
{
  public enum ListenerType
  {
    NotValidType,
    Autoflush,
    Merge,
    Create,
    CreateOnFlush,
    Delete,
    DirtyCheck,
    Evict,
    Flush,
    FlushEntity,
    Load,
    LoadCollection,
    Lock,
    Refresh,
    Replicate,
    SaveUpdate,
    Save,
    PreUpdate,
    Update,
    PreLoad,
    PreDelete,
    PreInsert,
    PreCollectionRecreate,
    PreCollectionRemove,
    PreCollectionUpdate,
    PostLoad,
    PostInsert,
    PostUpdate,
    PostDelete,
    PostCommitUpdate,
    PostCommitInsert,
    PostCommitDelete,
    PostCollectionRecreate,
    PostCollectionRemove,
    PostCollectionUpdate,
  }
}
