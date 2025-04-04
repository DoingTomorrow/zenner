// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.XmlHbmBinding.TypeBinder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping;
using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Cfg.XmlHbmBinding
{
  public class TypeBinder : Binder
  {
    private readonly SimpleValue value;

    public TypeBinder(SimpleValue value, Mappings mappings)
      : base(mappings)
    {
      this.value = value != null ? value : throw new ArgumentNullException(nameof (value));
    }

    public void Bind(string typeName)
    {
      if (string.IsNullOrEmpty(typeName))
        return;
      this.Bind(new HbmType() { name = typeName });
    }

    public void Bind(HbmType typeMapping)
    {
      if (typeMapping == null)
        return;
      string name = typeMapping.name;
      if (name == null)
        return;
      Dictionary<string, string> parameters = new Dictionary<string, string>();
      if (typeMapping.param != null)
        System.Array.ForEach<HbmParam>(typeMapping.param, (Action<HbmParam>) (p => parameters[p.name] = p.Text.LinesToString()));
      this.BindThroughTypeDefOrType(name, (IDictionary<string, string>) parameters);
    }

    private void BindThroughTypeDefOrType(
      string originalTypeName,
      IDictionary<string, string> parameters)
    {
      string str = (string) null;
      TypeDef typeDef1 = this.Mappings.GetTypeDef(originalTypeName);
      if (typeDef1 != null)
      {
        str = Binder.FullQualifiedClassName(typeDef1.TypeClass, this.Mappings);
        parameters = new Dictionary<string, string>(typeDef1.Parameters).AddOrOverride<string, string>(parameters);
      }
      else
      {
        TypeDef typeDef2;
        if (Binder.NeedQualifiedClassName(originalTypeName) && (typeDef2 = this.Mappings.GetTypeDef(Binder.FullQualifiedClassName(originalTypeName, this.Mappings))) != null)
        {
          str = typeDef2.TypeClass;
          parameters = new Dictionary<string, string>(typeDef2.Parameters).AddOrOverride<string, string>(parameters);
        }
      }
      if (parameters.Count != 0)
        this.value.TypeParameters = parameters;
      string className = str ?? originalTypeName;
      if (Binder.NeedQualifiedClassName(className))
        className = Binder.FullQualifiedClassName(className, this.Mappings);
      this.value.TypeName = className;
    }
  }
}
