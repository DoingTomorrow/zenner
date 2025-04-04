// Decompiled with JetBrains decompiler
// Type: NHibernate.Context.CurrentSessionContext
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System;

#nullable disable
namespace NHibernate.Context
{
  [Serializable]
  public abstract class CurrentSessionContext : ICurrentSessionContext
  {
    protected abstract ISession Session { get; set; }

    public virtual ISession CurrentSession()
    {
      return this.Session != null ? this.Session : throw new HibernateException("No session bound to the current context");
    }

    public static void Bind(ISession session)
    {
      CurrentSessionContext.GetCurrentSessionContext(session.SessionFactory).Session = session;
    }

    public static bool HasBind(ISessionFactory factory)
    {
      return CurrentSessionContext.GetCurrentSessionContext(factory).Session != null;
    }

    public static ISession Unbind(ISessionFactory factory)
    {
      ISession session = CurrentSessionContext.GetCurrentSessionContext(factory).Session;
      CurrentSessionContext.GetCurrentSessionContext(factory).Session = (ISession) null;
      return session;
    }

    private static CurrentSessionContext GetCurrentSessionContext(ISessionFactory factory)
    {
      if (!(factory is ISessionFactoryImplementor factoryImplementor))
        throw new HibernateException("Session factory does not implement ISessionFactoryImplementor.");
      if (factoryImplementor.CurrentSessionContext == null)
        throw new HibernateException("No current session context configured.");
      if (!(factoryImplementor.CurrentSessionContext is CurrentSessionContext currentSessionContext))
        throw new HibernateException("Current session context does not extend class CurrentSessionContext.");
      return currentSessionContext;
    }
  }
}
