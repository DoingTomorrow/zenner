// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.IdMapper
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
  public class IdMapper : IIdMapper, IAccessorPropertyMapper
  {
    private readonly IAccessorPropertyMapper accessorMapper;
    private readonly HbmId hbmId;

    public IdMapper(HbmId hbmId)
      : this((MemberInfo) null, hbmId)
    {
    }

    public IdMapper(MemberInfo member, HbmId hbmId)
    {
      this.hbmId = hbmId;
      if (member != null)
      {
        System.Type propertyOrFieldType = member.GetPropertyOrFieldType();
        hbmId.name = member.Name;
        hbmId.type1 = propertyOrFieldType.GetNhTypeName();
        this.accessorMapper = (IAccessorPropertyMapper) new AccessorPropertyMapper(member.DeclaringType, member.Name, (Action<string>) (x => hbmId.access = x));
      }
      else
      {
        hbmId.type1 = typeof (int).GetNhTypeName();
        this.accessorMapper = (IAccessorPropertyMapper) new IdMapper.NoMemberPropertyMapper();
      }
    }

    public void Generator(IGeneratorDef generator)
    {
      this.Generator(generator, (Action<IGeneratorMapper>) (x => { }));
    }

    public void Generator(IGeneratorDef generator, Action<IGeneratorMapper> generatorMapping)
    {
      this.ApplyGenerator(generator);
      generatorMapping((IGeneratorMapper) new GeneratorMapper(this.hbmId.generator));
    }

    public void Access(Accessor accessor) => this.accessorMapper.Access(accessor);

    public void Access(System.Type accessorType) => this.accessorMapper.Access(accessorType);

    public void Type(IIdentifierType persistentType)
    {
      if (persistentType == null)
        return;
      this.hbmId.type1 = persistentType.Name;
      this.hbmId.type = (HbmType) null;
    }

    public void UnsavedValue(object value)
    {
      this.hbmId.unsavedvalue = value != null ? value.ToString() : "null";
    }

    public void Column(string name) => this.hbmId.column1 = name;

    public void Length(int length) => this.hbmId.length = length.ToString();

    private void ApplyGenerator(IGeneratorDef generator)
    {
      HbmGenerator hbmGenerator = new HbmGenerator()
      {
        @class = generator.Class
      };
      if (this.hbmId.name == null)
      {
        System.Type defaultReturnType = generator.DefaultReturnType;
        if (defaultReturnType != null)
          this.hbmId.type1 = defaultReturnType.GetNhTypeName();
      }
      object generatorParameters = generator.Params;
      hbmGenerator.param = generatorParameters == null ? (HbmParam[]) null : ((IEnumerable<PropertyInfo>) generatorParameters.GetType().GetProperties()).Select(pi => new
      {
        pi = pi,
        pname = pi.Name
      }).Select(_param1 => new
      {
        \u003C\u003Eh__TransparentIdentifier6 = _param1,
        pvalue = _param1.pi.GetValue(generatorParameters, (object[]) null)
      }).Select(_param0 => new HbmParam()
      {
        name = _param0.\u003C\u003Eh__TransparentIdentifier6.pname,
        Text = new string[1]
        {
          object.ReferenceEquals(_param0.pvalue, (object) null) ? "null" : _param0.pvalue.ToString()
        }
      }).ToArray<HbmParam>();
      this.hbmId.generator = hbmGenerator;
    }

    private class NoMemberPropertyMapper : IAccessorPropertyMapper
    {
      public void Access(Accessor accessor)
      {
      }

      public void Access(System.Type accessorType)
      {
      }
    }
  }
}
