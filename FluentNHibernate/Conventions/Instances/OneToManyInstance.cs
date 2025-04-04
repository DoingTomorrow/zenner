// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.OneToManyInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public class OneToManyInstance : 
    OneToManyInspector,
    IOneToManyInstance,
    IOneToManyInspector,
    IRelationshipInstance,
    IRelationshipInspector,
    IInspector
  {
    private readonly OneToManyMapping mapping;

    public OneToManyInstance(OneToManyMapping mapping)
      : base(mapping)
    {
      this.mapping = mapping;
    }

    public INotFoundInstance NotFound
    {
      get
      {
        return (INotFoundInstance) new NotFoundInstance((Action<string>) (value => this.mapping.Set<string>((Expression<Func<OneToManyMapping, string>>) (x => x.NotFound), 1, value)));
      }
    }

    public void CustomClass<T>() => this.CustomClass(typeof (T));

    public void CustomClass(Type type)
    {
      this.mapping.Set<TypeReference>((Expression<Func<OneToManyMapping, TypeReference>>) (x => x.Class), 1, new TypeReference(type));
    }
  }
}
