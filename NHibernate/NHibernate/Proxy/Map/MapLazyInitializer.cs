// Decompiled with JetBrains decompiler
// Type: NHibernate.Proxy.Map.MapLazyInitializer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System;
using System.Collections;

#nullable disable
namespace NHibernate.Proxy.Map
{
  [Serializable]
  public class MapLazyInitializer(string entityName, object id, ISessionImplementor session) : 
    AbstractLazyInitializer(entityName, id, session)
  {
    public IDictionary Map => (IDictionary) this.GetImplementation();

    public override Type PersistentClass
    {
      get => throw new NotSupportedException("dynamic-map entity representation");
    }
  }
}
