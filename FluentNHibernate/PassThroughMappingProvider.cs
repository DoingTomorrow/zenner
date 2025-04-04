// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.PassThroughMappingProvider
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate
{
  public class PassThroughMappingProvider : IMappingProvider
  {
    private readonly ClassMapping mapping;

    public PassThroughMappingProvider(ClassMapping mapping) => this.mapping = mapping;

    public ClassMapping GetClassMapping() => this.mapping;

    public HibernateMapping GetHibernateMapping() => new HibernateMapping();

    public IEnumerable<Member> GetIgnoredProperties() => (IEnumerable<Member>) new Member[0];
  }
}
