// Decompiled with JetBrains decompiler
// Type: NHibernate.Id.ForeignGenerator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Id
{
  public class ForeignGenerator : IIdentifierGenerator, IConfigurable
  {
    private string propertyName;
    private string entityName;

    public object Generate(ISessionImplementor sessionImplementor, object obj)
    {
      ISession session = (ISession) sessionImplementor;
      IEntityPersister entityPersister = sessionImplementor.Factory.GetEntityPersister(this.entityName);
      object propertyValue = entityPersister.GetPropertyValue(obj, this.propertyName, sessionImplementor.EntityMode);
      if (propertyValue == null)
        throw new IdentifierGenerationException("attempted to assign id from null one-to-one property: " + this.propertyName);
      IType propertyType = entityPersister.GetPropertyType(this.propertyName);
      EntityType entityType = !propertyType.IsEntityType ? (EntityType) entityPersister.GetPropertyType("_identifierMapper." + this.propertyName) : (EntityType) propertyType;
      object obj1;
      try
      {
        obj1 = ForeignKeys.GetEntityIdentifierIfNotUnsaved(entityType.GetAssociatedEntityName(), propertyValue, sessionImplementor);
      }
      catch (TransientObjectException ex)
      {
        obj1 = session.Save(entityType.GetAssociatedEntityName(), propertyValue);
      }
      return session.Contains(obj) ? IdentifierGeneratorFactory.ShortCircuitIndicator : obj1;
    }

    public void Configure(IType type, IDictionary<string, string> parms, NHibernate.Dialect.Dialect dialect)
    {
      parms.TryGetValue(IdGeneratorParmsNames.EntityName, out this.entityName);
      parms.TryGetValue("property", out this.propertyName);
      if (this.propertyName == null || this.propertyName.Length == 0)
        throw new MappingException("param named \"property\" is required for foreign id generation strategy");
    }
  }
}
