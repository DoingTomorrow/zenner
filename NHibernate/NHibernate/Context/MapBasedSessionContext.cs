// Decompiled with JetBrains decompiler
// Type: NHibernate.Context.MapBasedSessionContext
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System.Collections;

#nullable disable
namespace NHibernate.Context
{
  public abstract class MapBasedSessionContext : CurrentSessionContext
  {
    private readonly ISessionFactoryImplementor _factory;

    protected MapBasedSessionContext(ISessionFactoryImplementor factory) => this._factory = factory;

    protected override ISession Session
    {
      get
      {
        IDictionary map = this.GetMap();
        return map == null ? (ISession) null : map[(object) this._factory] as ISession;
      }
      set
      {
        IDictionary dictionary = this.GetMap();
        if (dictionary == null)
        {
          dictionary = (IDictionary) new Hashtable();
          this.SetMap(dictionary);
        }
        dictionary[(object) this._factory] = (object) value;
      }
    }

    protected abstract IDictionary GetMap();

    protected abstract void SetMap(IDictionary value);
  }
}
