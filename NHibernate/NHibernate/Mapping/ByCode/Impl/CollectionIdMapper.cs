// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.CollectionIdMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class CollectionIdMapper : ICollectionIdMapper
  {
    private const string DefaultColumnName = "collection_key";
    private readonly HbmCollectionId hbmId;
    private string autosetType;

    public CollectionIdMapper(HbmCollectionId hbmId)
    {
      this.hbmId = hbmId;
      this.hbmId.column1 = "collection_key";
      this.Generator(Generators.Guid);
    }

    public void Generator(IGeneratorDef generator)
    {
      this.Generator(generator, (Action<IGeneratorMapper>) (x => { }));
    }

    public void Generator(IGeneratorDef generator, Action<IGeneratorMapper> generatorMapping)
    {
      if (generator == null)
        return;
      if (!generator.SupportedAsCollectionElementId)
        throw new NotSupportedException("The generator '" + generator.Class + "' cannot be used as collection element id.");
      this.ApplyGenerator(generator);
      generatorMapping((IGeneratorMapper) new GeneratorMapper(this.hbmId.generator));
    }

    public void Type(IIdentifierType persistentType)
    {
      if (persistentType == null)
        return;
      this.hbmId.type = persistentType.Name;
    }

    public void Column(string name)
    {
      if (string.IsNullOrEmpty(name))
        return;
      this.hbmId.column1 = name;
    }

    public void Length(int length) => this.hbmId.length = length.ToString();

    private void ApplyGenerator(IGeneratorDef generator)
    {
      HbmGenerator hbmGenerator = new HbmGenerator()
      {
        @class = generator.Class
      };
      object generatorParameters = generator.Params;
      hbmGenerator.param = generatorParameters == null ? (HbmParam[]) null : ((IEnumerable<PropertyInfo>) generatorParameters.GetType().GetProperties()).Select(pi => new
      {
        pi = pi,
        pname = pi.Name
      }).Select(_param1 => new
      {
        \u003C\u003Eh__TransparentIdentifier2 = _param1,
        pvalue = _param1.pi.GetValue(generatorParameters, (object[]) null)
      }).Select(_param0 => new HbmParam()
      {
        name = _param0.\u003C\u003Eh__TransparentIdentifier2.pname,
        Text = new string[1]
        {
          object.ReferenceEquals(_param0.pvalue, (object) null) ? "null" : _param0.pvalue.ToString()
        }
      }).ToArray<HbmParam>();
      this.hbmId.generator = hbmGenerator;
      this.AutosetTypeThroughGeneratorDef(generator);
    }

    private void AutosetTypeThroughGeneratorDef(IGeneratorDef generator)
    {
      if (!object.Equals((object) this.hbmId.type, (object) this.autosetType) || generator.DefaultReturnType == null)
        return;
      this.autosetType = generator.DefaultReturnType.GetNhTypeName();
      this.hbmId.type = this.autosetType;
    }
  }
}
