// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.DefaultFlushEventListener
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System;

#nullable disable
namespace NHibernate.Event.Default
{
  [Serializable]
  public class DefaultFlushEventListener : AbstractFlushingEventListener, IFlushEventListener
  {
    public virtual void OnFlush(FlushEvent @event)
    {
      IEventSource session = @event.Session;
      if (session.PersistenceContext.EntityEntries.Count <= 0 && session.PersistenceContext.CollectionEntries.Count <= 0)
        return;
      this.FlushEverythingToExecutions(@event);
      this.PerformExecutions(session);
      this.PostFlush((ISessionImplementor) session);
      if (!session.Factory.Statistics.IsStatisticsEnabled)
        return;
      session.Factory.StatisticsImplementor.Flush();
    }
  }
}
