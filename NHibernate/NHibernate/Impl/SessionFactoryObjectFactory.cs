// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.SessionFactoryObjectFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace NHibernate.Impl
{
  public static class SessionFactoryObjectFactory
  {
    private static readonly IInternalLogger log;
    private static readonly IDictionary<string, ISessionFactory> Instances = (IDictionary<string, ISessionFactory>) new Dictionary<string, ISessionFactory>();
    private static readonly IDictionary<string, ISessionFactory> NamedInstances = (IDictionary<string, ISessionFactory>) new Dictionary<string, ISessionFactory>();

    static SessionFactoryObjectFactory()
    {
      SessionFactoryObjectFactory.log = LoggerProvider.LoggerFor(typeof (SessionFactoryObjectFactory));
      SessionFactoryObjectFactory.log.Debug((object) "initializing class SessionFactoryObjectFactory");
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public static void AddInstance(
      string uid,
      string name,
      ISessionFactory instance,
      IDictionary<string, string> properties)
    {
      if (SessionFactoryObjectFactory.log.IsDebugEnabled)
      {
        string str = !string.IsNullOrEmpty(name) ? name : "unnamed";
        SessionFactoryObjectFactory.log.Debug((object) ("registered: " + uid + "(" + str + ")"));
      }
      SessionFactoryObjectFactory.Instances[uid] = instance;
      if (!string.IsNullOrEmpty(name))
      {
        SessionFactoryObjectFactory.log.Info((object) ("Factory name:" + name));
        SessionFactoryObjectFactory.NamedInstances[name] = instance;
      }
      else
        SessionFactoryObjectFactory.log.Info((object) "no name configured");
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public static void RemoveInstance(
      string uid,
      string name,
      IDictionary<string, string> properties)
    {
      if (!string.IsNullOrEmpty(name))
      {
        SessionFactoryObjectFactory.log.Info((object) ("unbinding factory: " + name));
        SessionFactoryObjectFactory.NamedInstances.Remove(name);
      }
      SessionFactoryObjectFactory.Instances.Remove(uid);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public static ISessionFactory GetNamedInstance(string name)
    {
      SessionFactoryObjectFactory.log.Debug((object) ("lookup: name=" + name));
      ISessionFactory namedInstance;
      if (!SessionFactoryObjectFactory.NamedInstances.TryGetValue(name, out namedInstance))
        SessionFactoryObjectFactory.log.Warn((object) ("Not found: " + name));
      return namedInstance;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public static ISessionFactory GetInstance(string uid)
    {
      SessionFactoryObjectFactory.log.Debug((object) ("lookup: uid=" + uid));
      ISessionFactory instance;
      if (!SessionFactoryObjectFactory.Instances.TryGetValue(uid, out instance))
        SessionFactoryObjectFactory.log.Warn((object) ("Not found: " + uid));
      return instance;
    }
  }
}
