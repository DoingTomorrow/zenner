// Decompiled with JetBrains decompiler
// Type: NHibernate.Context.ThreadLocalSessionContext
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace NHibernate.Context
{
  [Serializable]
  public class ThreadLocalSessionContext : ICurrentSessionContext
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (ThreadLocalSessionContext));
    [ThreadStatic]
    protected static IDictionary<ISessionFactory, ISession> context;
    protected readonly ISessionFactoryImplementor factory;

    public ThreadLocalSessionContext(ISessionFactoryImplementor factory) => this.factory = factory;

    public ISession CurrentSession()
    {
      ISession current = ThreadLocalSessionContext.ExistingSession((ISessionFactory) this.factory);
      if (current == null)
      {
        current = this.BuildOrObtainSession();
        if (this.NeedsWrapping(current))
          current = this.Wrap(current);
        ThreadLocalSessionContext.DoBind(current, (ISessionFactory) this.factory);
      }
      return current;
    }

    private static void CleanupAnyOrphanedSession(ISessionFactory factory)
    {
      ISession session = ThreadLocalSessionContext.DoUnbind(factory, false);
      if (session == null)
        return;
      ThreadLocalSessionContext.log.Warn((object) "Already session bound on call to bind(); make sure you clean up your sessions!");
      try
      {
        if (session.Transaction != null)
        {
          if (session.Transaction.IsActive)
          {
            try
            {
              session.Transaction.Rollback();
            }
            catch (Exception ex)
            {
              ThreadLocalSessionContext.log.Debug((object) "Unable to rollback transaction for orphaned session", ex);
            }
          }
        }
        session.Close();
      }
      catch (Exception ex)
      {
        ThreadLocalSessionContext.log.Debug((object) "Unable to close orphaned session", ex);
      }
    }

    public static void Bind(ISession session)
    {
      ISessionFactory sessionFactory = session.SessionFactory;
      ThreadLocalSessionContext.CleanupAnyOrphanedSession(sessionFactory);
      ThreadLocalSessionContext.DoBind(session, sessionFactory);
    }

    public static ISession Unbind(ISessionFactory factory)
    {
      return ThreadLocalSessionContext.DoUnbind(factory, true);
    }

    private static void DoBind(ISession current, ISessionFactory factory)
    {
      ThreadLocalSessionContext.context = ThreadLocalSessionContext.context ?? (IDictionary<ISessionFactory, ISession>) new Dictionary<ISessionFactory, ISession>();
      ThreadLocalSessionContext.context.Add(factory, current);
    }

    private static ISession DoUnbind(ISessionFactory factory, bool releaseMapIfEmpty)
    {
      ISession session = (ISession) null;
      if (ThreadLocalSessionContext.context != null)
      {
        if (ThreadLocalSessionContext.context.ContainsKey(factory))
        {
          session = ThreadLocalSessionContext.context[factory];
          ThreadLocalSessionContext.context.Remove(factory);
        }
        if (releaseMapIfEmpty && ThreadLocalSessionContext.context.Count == 0)
          ThreadLocalSessionContext.context = (IDictionary<ISessionFactory, ISession>) null;
      }
      return session;
    }

    private ISession Wrap(ISession current) => current;

    private bool NeedsWrapping(ISession current) => false;

    protected ISession BuildOrObtainSession()
    {
      return this.factory.OpenSession((IDbConnection) null, this.IsAutoFlushEnabled(), this.IsAutoCloseEnabled(), this.GetConnectionReleaseMode());
    }

    private ConnectionReleaseMode GetConnectionReleaseMode()
    {
      return this.factory.Settings.ConnectionReleaseMode;
    }

    protected virtual bool IsAutoCloseEnabled() => true;

    protected virtual bool IsAutoFlushEnabled() => true;

    private static ISession ExistingSession(ISessionFactory factory)
    {
      if (ThreadLocalSessionContext.context == null)
        return (ISession) null;
      return ThreadLocalSessionContext.context.ContainsKey(factory) ? ThreadLocalSessionContext.context[factory] : (ISession) null;
    }
  }
}
