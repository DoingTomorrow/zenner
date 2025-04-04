// Decompiled with JetBrains decompiler
// Type: NHibernate.Context.WebSessionContext
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
  public class WebSessionContext(ISessionFactoryImplementor factory) : MapBasedSessionContext(factory)
  {
    private const string SessionFactoryMapKey = "NHibernate.Context.WebSessionContext.SessionFactoryMapKey";

    protected override IDictionary GetMap()
    {
      return ReflectiveHttpContext.HttpContextCurrentItems[(object) "NHibernate.Context.WebSessionContext.SessionFactoryMapKey"] as IDictionary;
    }

    protected override void SetMap(IDictionary value)
    {
      ReflectiveHttpContext.HttpContextCurrentItems[(object) "NHibernate.Context.WebSessionContext.SessionFactoryMapKey"] = (object) value;
    }
  }
}
