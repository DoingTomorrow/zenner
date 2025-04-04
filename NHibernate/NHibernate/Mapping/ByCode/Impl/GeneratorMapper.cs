// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.GeneratorMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class GeneratorMapper : IGeneratorMapper
  {
    private readonly HbmGenerator generator;

    public GeneratorMapper(HbmGenerator generator) => this.generator = generator;

    public void Params(object generatorParameters)
    {
      if (generatorParameters == null)
        return;
      this.generator.param = ((IEnumerable<PropertyInfo>) generatorParameters.GetType().GetProperties()).Select(pi => new
      {
        pi = pi,
        pname = pi.Name
      }).Select(_param1 => new
      {
        \u003C\u003Eh__TransparentIdentifier0 = _param1,
        pvalue = _param1.pi.GetValue(generatorParameters, (object[]) null)
      }).Select(_param0 => new HbmParam()
      {
        name = _param0.\u003C\u003Eh__TransparentIdentifier0.pname,
        Text = new string[1]
        {
          object.ReferenceEquals(_param0.pvalue, (object) null) ? "null" : _param0.pvalue.ToString()
        }
      }).ToArray<HbmParam>();
    }
  }
}
