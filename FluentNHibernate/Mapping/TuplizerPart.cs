// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.TuplizerPart
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class TuplizerPart
  {
    private readonly TuplizerMapping mapping;

    public TuplizerPart(TuplizerMapping mapping) => this.mapping = mapping;

    public TuplizerPart Type(System.Type type)
    {
      this.mapping.Set<TypeReference>((Expression<Func<TuplizerMapping, TypeReference>>) (x => x.Type), 2, new TypeReference(type));
      return this;
    }

    public TuplizerPart Type(string type)
    {
      this.mapping.Set<TypeReference>((Expression<Func<TuplizerMapping, TypeReference>>) (x => x.Type), 2, new TypeReference(type));
      return this;
    }

    public TuplizerPart Type<T>() => this.Type(typeof (T));

    public TuplizerPart Mode(TuplizerMode mode)
    {
      this.mapping.Set<TuplizerMode>((Expression<Func<TuplizerMapping, TuplizerMode>>) (x => x.Mode), 2, mode);
      return this;
    }

    public TuplizerPart EntityName(string entityName)
    {
      this.mapping.Set<string>((Expression<Func<TuplizerMapping, string>>) (x => x.EntityName), 2, entityName);
      return this;
    }
  }
}
