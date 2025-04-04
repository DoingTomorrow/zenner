// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Entity.CascadeEntityLoader
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.Util;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Loader.Entity
{
  public class CascadeEntityLoader : AbstractEntityLoader
  {
    public CascadeEntityLoader(
      IOuterJoinLoadable persister,
      CascadingAction action,
      ISessionFactoryImplementor factory)
      : base(persister, persister.IdentifierType, factory, (IDictionary<string, IFilter>) new CollectionHelper.EmptyMapClass<string, IFilter>())
    {
      this.InitFromWalker((JoinWalker) new CascadeEntityJoinWalker(persister, action, factory));
      this.PostInstantiate();
      AbstractEntityLoader.log.Debug((object) string.Format("Static select for action {0} on entity {1}: {2}", (object) action, (object) this.entityName, (object) this.SqlString));
    }
  }
}
