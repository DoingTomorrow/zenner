// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.DefaultDirtyCheckEventListener
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Event.Default
{
  [Serializable]
  public class DefaultDirtyCheckEventListener : 
    AbstractFlushingEventListener,
    IDirtyCheckEventListener
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (DefaultDirtyCheckEventListener));

    public virtual void OnDirtyCheck(DirtyCheckEvent @event)
    {
      int collectionRemovalsCount = @event.Session.ActionQueue.CollectionRemovalsCount;
      try
      {
        this.FlushEverythingToExecutions((FlushEvent) @event);
        bool anyQueuedActions = @event.Session.ActionQueue.HasAnyQueuedActions;
        DefaultDirtyCheckEventListener.log.Debug(anyQueuedActions ? (object) "session dirty" : (object) "session not dirty");
        @event.Dirty = anyQueuedActions;
      }
      finally
      {
        @event.Session.ActionQueue.ClearFromFlushNeededCheck(collectionRemovalsCount);
      }
    }
  }
}
