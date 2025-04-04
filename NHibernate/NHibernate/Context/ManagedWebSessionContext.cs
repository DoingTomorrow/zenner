// Decompiled with JetBrains decompiler
// Type: NHibernate.Context.ManagedWebSessionContext
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System;
using System.Collections;

#nullable disable
namespace NHibernate.Context
{
  [Serializable]
  public class ManagedWebSessionContext : ICurrentSessionContext
  {
    private const string SessionFactoryMapKey = "NHibernate.Context.ManagedWebSessionContext.SessionFactoryMapKey";
    private readonly ISessionFactoryImplementor factory;

    public ManagedWebSessionContext(ISessionFactoryImplementor factory) => this.factory = factory;

    public ISession CurrentSession()
    {
      return ManagedWebSessionContext.GetExistingSession(ReflectiveHttpContext.HttpContextCurrentGetter(), (ISessionFactory) this.factory) ?? throw new HibernateException("No session bound to the current HttpContext");
    }

    public static void Bind(object httpContext, ISession session)
    {
      ManagedWebSessionContext.GetSessionMap(httpContext, true)[(object) ((ISessionImplementor) session).Factory] = (object) session;
    }

    public static bool HasBind(object httpContext, ISessionFactory factory)
    {
      return ManagedWebSessionContext.GetExistingSession(httpContext, factory) != null;
    }

    public static ISession Unbind(object httpContext, ISessionFactory factory)
    {
      ISession session = (ISession) null;
      IDictionary sessionMap = ManagedWebSessionContext.GetSessionMap(httpContext, false);
      if (sessionMap != null)
      {
        session = sessionMap[(object) factory] as ISession;
        sessionMap.Remove((object) factory);
      }
      return session;
    }

    private static ISession GetExistingSession(object httpContext, ISessionFactory factory)
    {
      IDictionary sessionMap = ManagedWebSessionContext.GetSessionMap(httpContext, false);
      return sessionMap == null ? (ISession) null : sessionMap[(object) factory] as ISession;
    }

    private static IDictionary GetSessionMap(object httpContext, bool create)
    {
      IDictionary dictionary = ReflectiveHttpContext.HttpContextItemsGetter(httpContext);
      if (!(dictionary[(object) "NHibernate.Context.ManagedWebSessionContext.SessionFactoryMapKey"] is IDictionary sessionMap) && create)
      {
        sessionMap = (IDictionary) new Hashtable();
        dictionary[(object) "NHibernate.Context.ManagedWebSessionContext.SessionFactoryMapKey"] = (object) sessionMap;
      }
      return sessionMap;
    }
  }
}
