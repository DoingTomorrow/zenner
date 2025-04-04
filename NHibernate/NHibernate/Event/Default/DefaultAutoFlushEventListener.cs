// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.DefaultAutoFlushEventListener
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System;

#nullable disable
namespace NHibernate.Event.Default
{
  [Serializable]
  public class DefaultAutoFlushEventListener : AbstractFlushingEventListener, IAutoFlushEventListener
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (DefaultAutoFlushEventListener));

    public virtual void OnAutoFlush(AutoFlushEvent @event)
    {
      IEventSource session = @event.Session;
      if (!this.FlushMightBeNeeded((ISessionImplementor) session))
        return;
      int collectionRemovalsCount = session.ActionQueue.CollectionRemovalsCount;
      this.FlushEverythingToExecutions((FlushEvent) @event);
      if (this.FlushIsReallyNeeded(@event, session))
      {
        if (DefaultAutoFlushEventListener.log.IsDebugEnabled)
          DefaultAutoFlushEventListener.log.Debug((object) "Need to execute flush");
        this.PerformExecutions(session);
        this.PostFlush((ISessionImplementor) session);
        if (session.Factory.Statistics.IsStatisticsEnabled)
          session.Factory.StatisticsImplementor.Flush();
      }
      else
      {
        if (DefaultAutoFlushEventListener.log.IsDebugEnabled)
          DefaultAutoFlushEventListener.log.Debug((object) "Dont need to execute flush");
        session.ActionQueue.ClearFromFlushNeededCheck(collectionRemovalsCount);
      }
      @event.FlushRequired = this.FlushIsReallyNeeded(@event, session);
    }

    private bool FlushIsReallyNeeded(AutoFlushEvent @event, IEventSource source)
    {
      return source.ActionQueue.AreTablesToBeUpdated(@event.QuerySpaces) || ((ISessionImplementor) source).FlushMode == FlushMode.Always;
    }

    private bool FlushMightBeNeeded(ISessionImplementor source)
    {
      if (source.FlushMode < FlushMode.Auto || source.DontFlushFromFind != 0)
        return false;
      return source.PersistenceContext.EntityEntries.Count > 0 || source.PersistenceContext.CollectionEntries.Count > 0;
    }
  }
}
