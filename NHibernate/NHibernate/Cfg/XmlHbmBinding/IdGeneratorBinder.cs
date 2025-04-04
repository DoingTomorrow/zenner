// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.XmlHbmBinding.IdGeneratorBinder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Id;
using NHibernate.Mapping;
using NHibernate.Util;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Cfg.XmlHbmBinding
{
  public class IdGeneratorBinder(Mappings mappings) : Binder(mappings)
  {
    public void BindGenerator(SimpleValue id, HbmGenerator generatorMapping)
    {
      if (generatorMapping == null)
        return;
      if (generatorMapping.@class == null)
        throw new MappingException("no class given for generator");
      TypeDef typeDef = this.mappings.GetTypeDef(generatorMapping.@class);
      if (typeDef != null)
      {
        id.IdentifierGeneratorStrategy = typeDef.TypeClass;
        Dictionary<string, string> to = new Dictionary<string, string>(typeDef.Parameters);
        ArrayHelper.AddAll<string, string>((IDictionary<string, string>) to, this.GetGeneratorProperties(generatorMapping, id.Table.Schema));
        id.IdentifierGeneratorProperties = (IDictionary<string, string>) to;
      }
      else
      {
        id.IdentifierGeneratorStrategy = generatorMapping.@class;
        id.IdentifierGeneratorProperties = this.GetGeneratorProperties(generatorMapping, id.Table.Schema);
      }
    }

    private IDictionary<string, string> GetGeneratorProperties(
      HbmGenerator generatorMapping,
      string schema)
    {
      Dictionary<string, string> generatorProperties = new Dictionary<string, string>();
      if (schema != null)
        generatorProperties[PersistentIdGeneratorParmsNames.Schema] = schema;
      else if (this.mappings.SchemaName != null)
        generatorProperties[PersistentIdGeneratorParmsNames.Schema] = this.mappings.SchemaName;
      if (this.mappings.PreferPooledValuesLo != null)
        generatorProperties["id.optimizer.pooled.prefer_lo"] = this.mappings.PreferPooledValuesLo;
      foreach (HbmParam hbmParam in generatorMapping.param ?? new HbmParam[0])
        generatorProperties[hbmParam.name] = hbmParam.GetText();
      return (IDictionary<string, string>) generatorProperties;
    }
  }
}
