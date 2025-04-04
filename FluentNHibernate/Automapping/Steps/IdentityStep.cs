// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Automapping.Steps.IdentityStep
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Automapping.Steps
{
  public class IdentityStep : IAutomappingStep
  {
    private readonly IAutomappingConfiguration cfg;

    public IdentityStep(IAutomappingConfiguration cfg) => this.cfg = cfg;

    public bool ShouldMap(Member member) => this.cfg.IsId(member);

    public void Map(ClassMappingBase classMap, Member member)
    {
      if (!(classMap is ClassMapping))
        return;
      IdMapping mapping1 = new IdMapping()
      {
        ContainingEntityType = classMap.Type
      };
      ColumnMapping mapping2 = new ColumnMapping();
      mapping2.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, member.Name);
      mapping1.AddColumn(0, mapping2);
      mapping1.Set((Expression<Func<IdMapping, object>>) (x => x.Name), 0, (object) member.Name);
      mapping1.Set((Expression<Func<IdMapping, object>>) (x => x.Type), 0, (object) new TypeReference(member.PropertyType));
      mapping1.Member = member;
      mapping1.Set((Expression<Func<IdMapping, object>>) (x => x.Generator), 0, (object) IdentityStep.GetDefaultGenerator(member));
      this.SetDefaultAccess(member, mapping1);
      ((ClassMapping) classMap).Set<IIdentityMapping>((Expression<Func<ClassMapping, IIdentityMapping>>) (x => x.Id), 0, (IIdentityMapping) mapping1);
    }

    private void SetDefaultAccess(Member member, IdMapping mapping)
    {
      Access access = MemberAccessResolver.Resolve(member);
      if (access != Access.Property && access != Access.Unset)
        mapping.Set((Expression<Func<IdMapping, object>>) (x => x.Access), 0, (object) access.ToString());
      if (!member.IsProperty || member.CanWrite)
        return;
      mapping.Set((Expression<Func<IdMapping, object>>) (x => x.Access), 0, (object) this.cfg.GetAccessStrategyForReadOnlyProperty(member).ToString());
    }

    private static GeneratorMapping GetDefaultGenerator(Member property)
    {
      GeneratorMapping mapping = new GeneratorMapping();
      GeneratorBuilder generatorBuilder = new GeneratorBuilder(mapping, property.PropertyType, 0);
      if (property.PropertyType == typeof (Guid))
        generatorBuilder.GuidComb();
      else if (property.PropertyType == typeof (int) || property.PropertyType == typeof (long))
        generatorBuilder.Identity();
      else
        generatorBuilder.Assigned();
      return mapping;
    }
  }
}
