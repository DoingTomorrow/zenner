// Decompiled with JetBrains decompiler
// Type: NHibernate.Context.CallSessionContext
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System;
using System.Collections;
using System.Runtime.Remoting.Messaging;

#nullable disable
namespace NHibernate.Context
{
  [Serializable]
  public class CallSessionContext(ISessionFactoryImplementor factory) : MapBasedSessionContext(factory)
  {
    private const string SessionFactoryMapKey = "NHibernate.Context.CallSessionContext.SessionFactoryMapKey";

    protected override void SetMap(IDictionary value)
    {
      CallContext.SetData("NHibernate.Context.CallSessionContext.SessionFactoryMapKey", (object) value);
    }

    protected override IDictionary GetMap()
    {
      return CallContext.GetData("NHibernate.Context.CallSessionContext.SessionFactoryMapKey") as IDictionary;
    }
  }
}
