// Decompiled with JetBrains decompiler
// Type: NHibernate.Id.Assigned
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Type;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Id
{
  public class Assigned : IIdentifierGenerator, IConfigurable
  {
    private string entityName;

    public object Generate(ISessionImplementor session, object obj)
    {
      if (obj is IPersistentCollection)
        throw new IdentifierGenerationException("Illegal use of assigned id generation for a toplevel collection");
      return session.GetEntityPersister(this.entityName, obj).GetIdentifier(obj, session.EntityMode) ?? throw new IdentifierGenerationException("ids for this class must be manually assigned before calling save(): " + obj.GetType().FullName);
    }

    public void Configure(IType type, IDictionary<string, string> parms, NHibernate.Dialect.Dialect dialect)
    {
      parms.TryGetValue(IdGeneratorParmsNames.EntityName, out this.entityName);
      if (this.entityName == null)
        throw new MappingException("no entity name");
    }
  }
}
