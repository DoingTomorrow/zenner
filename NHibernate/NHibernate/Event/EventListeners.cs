// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.EventListeners
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg;
using NHibernate.Event.Default;
using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Event
{
  [Serializable]
  public class EventListeners
  {
    private readonly List<object> initializedListeners = new List<object>();
    private static readonly IDictionary<ListenerType, Type> eventInterfaceFromType = (IDictionary<ListenerType, Type>) new Dictionary<ListenerType, Type>(28);
    private ILoadEventListener[] loadEventListeners = new ILoadEventListener[1]
    {
      (ILoadEventListener) new DefaultLoadEventListener()
    };
    private ISaveOrUpdateEventListener[] saveOrUpdateEventListeners = new ISaveOrUpdateEventListener[1]
    {
      (ISaveOrUpdateEventListener) new DefaultSaveOrUpdateEventListener()
    };
    private IMergeEventListener[] mergeEventListeners = new IMergeEventListener[1]
    {
      (IMergeEventListener) new DefaultMergeEventListener()
    };
    private IPersistEventListener[] persistEventListeners = new IPersistEventListener[1]
    {
      (IPersistEventListener) new DefaultPersistEventListener()
    };
    private IPersistEventListener[] persistOnFlushEventListeners = new IPersistEventListener[1]
    {
      (IPersistEventListener) new DefaultPersistOnFlushEventListener()
    };
    private IReplicateEventListener[] replicateEventListeners = new IReplicateEventListener[1]
    {
      (IReplicateEventListener) new DefaultReplicateEventListener()
    };
    private IDeleteEventListener[] deleteEventListeners = new IDeleteEventListener[1]
    {
      (IDeleteEventListener) new DefaultDeleteEventListener()
    };
    private IAutoFlushEventListener[] autoFlushEventListeners = new IAutoFlushEventListener[1]
    {
      (IAutoFlushEventListener) new DefaultAutoFlushEventListener()
    };
    private IDirtyCheckEventListener[] dirtyCheckEventListeners = new IDirtyCheckEventListener[1]
    {
      (IDirtyCheckEventListener) new DefaultDirtyCheckEventListener()
    };
    private IFlushEventListener[] flushEventListeners = new IFlushEventListener[1]
    {
      (IFlushEventListener) new DefaultFlushEventListener()
    };
    private IEvictEventListener[] evictEventListeners = new IEvictEventListener[1]
    {
      (IEvictEventListener) new DefaultEvictEventListener()
    };
    private ILockEventListener[] lockEventListeners = new ILockEventListener[1]
    {
      (ILockEventListener) new DefaultLockEventListener()
    };
    private IRefreshEventListener[] refreshEventListeners = new IRefreshEventListener[1]
    {
      (IRefreshEventListener) new DefaultRefreshEventListener()
    };
    private IFlushEntityEventListener[] flushEntityEventListeners = new IFlushEntityEventListener[1]
    {
      (IFlushEntityEventListener) new DefaultFlushEntityEventListener()
    };
    private IInitializeCollectionEventListener[] initializeCollectionEventListeners = new IInitializeCollectionEventListener[1]
    {
      (IInitializeCollectionEventListener) new DefaultInitializeCollectionEventListener()
    };
    private IPostLoadEventListener[] postLoadEventListeners = new IPostLoadEventListener[1]
    {
      (IPostLoadEventListener) new DefaultPostLoadEventListener()
    };
    private IPreLoadEventListener[] preLoadEventListeners = new IPreLoadEventListener[1]
    {
      (IPreLoadEventListener) new DefaultPreLoadEventListener()
    };
    private IPreDeleteEventListener[] preDeleteEventListeners = new IPreDeleteEventListener[0];
    private IPreUpdateEventListener[] preUpdateEventListeners = new IPreUpdateEventListener[0];
    private IPreInsertEventListener[] preInsertEventListeners = new IPreInsertEventListener[0];
    private IPostDeleteEventListener[] postDeleteEventListeners = new IPostDeleteEventListener[0];
    private IPostUpdateEventListener[] postUpdateEventListeners = new IPostUpdateEventListener[0];
    private IPostInsertEventListener[] postInsertEventListeners = new IPostInsertEventListener[0];
    private IPostDeleteEventListener[] postCommitDeleteEventListeners = new IPostDeleteEventListener[0];
    private IPostUpdateEventListener[] postCommitUpdateEventListeners = new IPostUpdateEventListener[0];
    private IPostInsertEventListener[] postCommitInsertEventListeners = new IPostInsertEventListener[0];
    private IPreCollectionRecreateEventListener[] preCollectionRecreateEventListeners = new IPreCollectionRecreateEventListener[0];
    private IPostCollectionRecreateEventListener[] postCollectionRecreateEventListeners = new IPostCollectionRecreateEventListener[0];
    private IPreCollectionRemoveEventListener[] preCollectionRemoveEventListeners = new IPreCollectionRemoveEventListener[0];
    private IPostCollectionRemoveEventListener[] postCollectionRemoveEventListeners = new IPostCollectionRemoveEventListener[0];
    private IPreCollectionUpdateEventListener[] preCollectionUpdateEventListeners = new IPreCollectionUpdateEventListener[0];
    private IPostCollectionUpdateEventListener[] postCollectionUpdateEventListeners = new IPostCollectionUpdateEventListener[0];
    private ISaveOrUpdateEventListener[] saveEventListeners = new ISaveOrUpdateEventListener[1]
    {
      (ISaveOrUpdateEventListener) new DefaultSaveEventListener()
    };
    private ISaveOrUpdateEventListener[] updateEventListeners = new ISaveOrUpdateEventListener[1]
    {
      (ISaveOrUpdateEventListener) new DefaultUpdateEventListener()
    };
    private IMergeEventListener[] saveOrUpdateCopyEventListeners = new IMergeEventListener[1]
    {
      (IMergeEventListener) new DefaultSaveOrUpdateCopyEventListener()
    };

    static EventListeners()
    {
      EventListeners.eventInterfaceFromType[ListenerType.Autoflush] = typeof (IAutoFlushEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.Merge] = typeof (IMergeEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.Create] = typeof (IPersistEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.CreateOnFlush] = typeof (IPersistEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.Delete] = typeof (IDeleteEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.DirtyCheck] = typeof (IDirtyCheckEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.Evict] = typeof (IEvictEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.Flush] = typeof (IFlushEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.FlushEntity] = typeof (IFlushEntityEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.Load] = typeof (ILoadEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.LoadCollection] = typeof (IInitializeCollectionEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.Lock] = typeof (ILockEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.Refresh] = typeof (IRefreshEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.Replicate] = typeof (IReplicateEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.SaveUpdate] = typeof (ISaveOrUpdateEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.Save] = typeof (ISaveOrUpdateEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.Update] = typeof (ISaveOrUpdateEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.PreLoad] = typeof (IPreLoadEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.PreUpdate] = typeof (IPreUpdateEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.PreDelete] = typeof (IPreDeleteEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.PreInsert] = typeof (IPreInsertEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.PreCollectionRecreate] = typeof (IPreCollectionRecreateEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.PreCollectionRemove] = typeof (IPreCollectionRemoveEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.PreCollectionUpdate] = typeof (IPreCollectionUpdateEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.PostLoad] = typeof (IPostLoadEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.PostUpdate] = typeof (IPostUpdateEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.PostDelete] = typeof (IPostDeleteEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.PostInsert] = typeof (IPostInsertEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.PostCommitUpdate] = typeof (IPostUpdateEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.PostCommitDelete] = typeof (IPostDeleteEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.PostCommitInsert] = typeof (IPostInsertEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.PostCollectionRecreate] = typeof (IPostCollectionRecreateEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.PostCollectionRemove] = typeof (IPostCollectionRemoveEventListener);
      EventListeners.eventInterfaceFromType[ListenerType.PostCollectionUpdate] = typeof (IPostCollectionUpdateEventListener);
      EventListeners.eventInterfaceFromType = (IDictionary<ListenerType, Type>) new UnmodifiableDictionary<ListenerType, Type>(EventListeners.eventInterfaceFromType);
    }

    public ILoadEventListener[] LoadEventListeners
    {
      get => this.loadEventListeners;
      set => this.loadEventListeners = value;
    }

    public ISaveOrUpdateEventListener[] SaveOrUpdateEventListeners
    {
      get => this.saveOrUpdateEventListeners;
      set
      {
        if (value == null)
          return;
        this.saveOrUpdateEventListeners = value;
      }
    }

    public IMergeEventListener[] MergeEventListeners
    {
      get => this.mergeEventListeners;
      set
      {
        if (value == null)
          return;
        this.mergeEventListeners = value;
      }
    }

    public IPersistEventListener[] PersistEventListeners
    {
      get => this.persistEventListeners;
      set
      {
        if (value == null)
          return;
        this.persistEventListeners = value;
      }
    }

    public IPersistEventListener[] PersistOnFlushEventListeners
    {
      get => this.persistOnFlushEventListeners;
      set
      {
        if (value == null)
          return;
        this.persistOnFlushEventListeners = value;
      }
    }

    public IReplicateEventListener[] ReplicateEventListeners
    {
      get => this.replicateEventListeners;
      set
      {
        if (value == null)
          return;
        this.replicateEventListeners = value;
      }
    }

    public IDeleteEventListener[] DeleteEventListeners
    {
      get => this.deleteEventListeners;
      set
      {
        if (value == null)
          return;
        this.deleteEventListeners = value;
      }
    }

    public IAutoFlushEventListener[] AutoFlushEventListeners
    {
      get => this.autoFlushEventListeners;
      set
      {
        if (value == null)
          return;
        this.autoFlushEventListeners = value;
      }
    }

    public IDirtyCheckEventListener[] DirtyCheckEventListeners
    {
      get => this.dirtyCheckEventListeners;
      set
      {
        if (value == null)
          return;
        this.dirtyCheckEventListeners = value;
      }
    }

    public IFlushEventListener[] FlushEventListeners
    {
      get => this.flushEventListeners;
      set
      {
        if (value == null)
          return;
        this.flushEventListeners = value;
      }
    }

    public IEvictEventListener[] EvictEventListeners
    {
      get => this.evictEventListeners;
      set
      {
        if (value == null)
          return;
        this.evictEventListeners = value;
      }
    }

    public ILockEventListener[] LockEventListeners
    {
      get => this.lockEventListeners;
      set
      {
        if (value == null)
          return;
        this.lockEventListeners = value;
      }
    }

    public IRefreshEventListener[] RefreshEventListeners
    {
      get => this.refreshEventListeners;
      set
      {
        if (value == null)
          return;
        this.refreshEventListeners = value;
      }
    }

    public IFlushEntityEventListener[] FlushEntityEventListeners
    {
      get => this.flushEntityEventListeners;
      set
      {
        if (value == null)
          return;
        this.flushEntityEventListeners = value;
      }
    }

    public IInitializeCollectionEventListener[] InitializeCollectionEventListeners
    {
      get => this.initializeCollectionEventListeners;
      set
      {
        if (value == null)
          return;
        this.initializeCollectionEventListeners = value;
      }
    }

    public IPostLoadEventListener[] PostLoadEventListeners
    {
      get => this.postLoadEventListeners;
      set
      {
        if (value == null)
          return;
        this.postLoadEventListeners = value;
      }
    }

    public IPreLoadEventListener[] PreLoadEventListeners
    {
      get => this.preLoadEventListeners;
      set
      {
        if (value == null)
          return;
        this.preLoadEventListeners = value;
      }
    }

    public IPreDeleteEventListener[] PreDeleteEventListeners
    {
      get => this.preDeleteEventListeners;
      set
      {
        if (value == null)
          return;
        this.preDeleteEventListeners = value;
      }
    }

    public IPreUpdateEventListener[] PreUpdateEventListeners
    {
      get => this.preUpdateEventListeners;
      set
      {
        if (value == null)
          return;
        this.preUpdateEventListeners = value;
      }
    }

    public IPreInsertEventListener[] PreInsertEventListeners
    {
      get => this.preInsertEventListeners;
      set
      {
        if (value == null)
          return;
        this.preInsertEventListeners = value;
      }
    }

    public IPostDeleteEventListener[] PostDeleteEventListeners
    {
      get => this.postDeleteEventListeners;
      set
      {
        if (value == null)
          return;
        this.postDeleteEventListeners = value;
      }
    }

    public IPostUpdateEventListener[] PostUpdateEventListeners
    {
      get => this.postUpdateEventListeners;
      set
      {
        if (value == null)
          return;
        this.postUpdateEventListeners = value;
      }
    }

    public IPostInsertEventListener[] PostInsertEventListeners
    {
      get => this.postInsertEventListeners;
      set
      {
        if (value == null)
          return;
        this.postInsertEventListeners = value;
      }
    }

    public IPostDeleteEventListener[] PostCommitDeleteEventListeners
    {
      get => this.postCommitDeleteEventListeners;
      set
      {
        if (value == null)
          return;
        this.postCommitDeleteEventListeners = value;
      }
    }

    public IPostUpdateEventListener[] PostCommitUpdateEventListeners
    {
      get => this.postCommitUpdateEventListeners;
      set
      {
        if (value == null)
          return;
        this.postCommitUpdateEventListeners = value;
      }
    }

    public IPostInsertEventListener[] PostCommitInsertEventListeners
    {
      get => this.postCommitInsertEventListeners;
      set
      {
        if (value == null)
          return;
        this.postCommitInsertEventListeners = value;
      }
    }

    public ISaveOrUpdateEventListener[] SaveEventListeners
    {
      get => this.saveEventListeners;
      set
      {
        if (value == null)
          return;
        this.saveEventListeners = value;
      }
    }

    public ISaveOrUpdateEventListener[] UpdateEventListeners
    {
      get => this.updateEventListeners;
      set
      {
        if (value == null)
          return;
        this.updateEventListeners = value;
      }
    }

    public IMergeEventListener[] SaveOrUpdateCopyEventListeners
    {
      get => this.saveOrUpdateCopyEventListeners;
      set
      {
        if (value == null)
          return;
        this.saveOrUpdateCopyEventListeners = value;
      }
    }

    public IPreCollectionRecreateEventListener[] PreCollectionRecreateEventListeners
    {
      get => this.preCollectionRecreateEventListeners;
      set
      {
        if (value == null)
          return;
        this.preCollectionRecreateEventListeners = value;
      }
    }

    public IPostCollectionRecreateEventListener[] PostCollectionRecreateEventListeners
    {
      get => this.postCollectionRecreateEventListeners;
      set
      {
        if (value == null)
          return;
        this.postCollectionRecreateEventListeners = value;
      }
    }

    public IPreCollectionRemoveEventListener[] PreCollectionRemoveEventListeners
    {
      get => this.preCollectionRemoveEventListeners;
      set
      {
        if (value == null)
          return;
        this.preCollectionRemoveEventListeners = value;
      }
    }

    public IPostCollectionRemoveEventListener[] PostCollectionRemoveEventListeners
    {
      get => this.postCollectionRemoveEventListeners;
      set
      {
        if (value == null)
          return;
        this.postCollectionRemoveEventListeners = value;
      }
    }

    public IPreCollectionUpdateEventListener[] PreCollectionUpdateEventListeners
    {
      get => this.preCollectionUpdateEventListeners;
      set
      {
        if (value == null)
          return;
        this.preCollectionUpdateEventListeners = value;
      }
    }

    public IPostCollectionUpdateEventListener[] PostCollectionUpdateEventListeners
    {
      get => this.postCollectionUpdateEventListeners;
      set
      {
        if (value == null)
          return;
        this.postCollectionUpdateEventListeners = value;
      }
    }

    public Type GetListenerClassFor(ListenerType type)
    {
      Type listenerClassFor;
      if (!EventListeners.eventInterfaceFromType.TryGetValue(type, out listenerClassFor))
        throw new MappingException("Unrecognized listener type [" + (object) type + "]");
      return listenerClassFor;
    }

    public virtual void InitializeListeners(Configuration cfg)
    {
      this.InitializeListeners(cfg, (object[]) this.loadEventListeners);
      this.InitializeListeners(cfg, (object[]) this.saveOrUpdateEventListeners);
      this.InitializeListeners(cfg, (object[]) this.saveOrUpdateCopyEventListeners);
      this.InitializeListeners(cfg, (object[]) this.mergeEventListeners);
      this.InitializeListeners(cfg, (object[]) this.persistEventListeners);
      this.InitializeListeners(cfg, (object[]) this.persistOnFlushEventListeners);
      this.InitializeListeners(cfg, (object[]) this.replicateEventListeners);
      this.InitializeListeners(cfg, (object[]) this.deleteEventListeners);
      this.InitializeListeners(cfg, (object[]) this.autoFlushEventListeners);
      this.InitializeListeners(cfg, (object[]) this.dirtyCheckEventListeners);
      this.InitializeListeners(cfg, (object[]) this.flushEventListeners);
      this.InitializeListeners(cfg, (object[]) this.evictEventListeners);
      this.InitializeListeners(cfg, (object[]) this.lockEventListeners);
      this.InitializeListeners(cfg, (object[]) this.refreshEventListeners);
      this.InitializeListeners(cfg, (object[]) this.flushEntityEventListeners);
      this.InitializeListeners(cfg, (object[]) this.initializeCollectionEventListeners);
      this.InitializeListeners(cfg, (object[]) this.postLoadEventListeners);
      this.InitializeListeners(cfg, (object[]) this.preLoadEventListeners);
      this.InitializeListeners(cfg, (object[]) this.preDeleteEventListeners);
      this.InitializeListeners(cfg, (object[]) this.preUpdateEventListeners);
      this.InitializeListeners(cfg, (object[]) this.preInsertEventListeners);
      this.InitializeListeners(cfg, (object[]) this.postDeleteEventListeners);
      this.InitializeListeners(cfg, (object[]) this.postUpdateEventListeners);
      this.InitializeListeners(cfg, (object[]) this.postInsertEventListeners);
      this.InitializeListeners(cfg, (object[]) this.postCommitDeleteEventListeners);
      this.InitializeListeners(cfg, (object[]) this.postCommitUpdateEventListeners);
      this.InitializeListeners(cfg, (object[]) this.postCommitInsertEventListeners);
      this.InitializeListeners(cfg, (object[]) this.saveEventListeners);
      this.InitializeListeners(cfg, (object[]) this.updateEventListeners);
      this.InitializeListeners(cfg, (object[]) this.preCollectionRecreateEventListeners);
      this.InitializeListeners(cfg, (object[]) this.postCollectionRecreateEventListeners);
      this.InitializeListeners(cfg, (object[]) this.preCollectionRemoveEventListeners);
      this.InitializeListeners(cfg, (object[]) this.postCollectionRemoveEventListeners);
      this.InitializeListeners(cfg, (object[]) this.preCollectionUpdateEventListeners);
      this.InitializeListeners(cfg, (object[]) this.postCollectionUpdateEventListeners);
    }

    private void InitializeListeners(Configuration cfg, object[] list)
    {
      this.initializedListeners.AddRange((IEnumerable<object>) list);
      foreach (object obj in list)
      {
        if (obj is IInitializable initializable)
          initializable.Initialize(cfg);
      }
    }

    public EventListeners ShallowCopy() => this;

    public void DestroyListeners()
    {
      try
      {
        foreach (object initializedListener in this.initializedListeners)
        {
          if (initializedListener is IDestructible destructible)
            destructible.Cleanup();
          else if (initializedListener is IDisposable disposable)
            disposable.Dispose();
        }
      }
      catch (Exception ex)
      {
        throw new HibernateException("could not destruct/dispose listeners", ex);
      }
    }
  }
}
