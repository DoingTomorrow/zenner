// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Automapping.Steps.ReferenceStep
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Automapping.Steps
{
  public class ReferenceStep : IAutomappingStep
  {
    private readonly Func<Member, bool> findPropertyconvention = (Func<Member, bool>) (p => p.PropertyType.Namespace != "System" && p.PropertyType.Namespace != "System.Collections.Generic" && p.PropertyType.Namespace != "Iesi.Collections.Generic" && !p.PropertyType.IsEnum);
    private readonly IAutomappingConfiguration cfg;

    public ReferenceStep(IAutomappingConfiguration cfg) => this.cfg = cfg;

    public bool ShouldMap(Member member) => this.findPropertyconvention(member);

    public void Map(ClassMappingBase classMap, Member member)
    {
      ManyToOneMapping mapping = this.CreateMapping(member);
      mapping.ContainingEntityType = classMap.Type;
      classMap.AddReference(mapping);
    }

    private ManyToOneMapping CreateMapping(Member member)
    {
      ManyToOneMapping mapping1 = new ManyToOneMapping()
      {
        Member = member
      };
      mapping1.Set<string>((Expression<Func<ManyToOneMapping, string>>) (x => x.Name), 0, member.Name);
      mapping1.Set<TypeReference>((Expression<Func<ManyToOneMapping, TypeReference>>) (x => x.Class), 0, new TypeReference(member.PropertyType));
      ColumnMapping mapping2 = new ColumnMapping();
      mapping2.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, member.Name + "_id");
      mapping1.AddColumn(0, mapping2);
      this.SetDefaultAccess(member, mapping1);
      return mapping1;
    }

    private void SetDefaultAccess(Member member, ManyToOneMapping mapping)
    {
      Access access = MemberAccessResolver.Resolve(member);
      if (access != Access.Property && access != Access.Unset)
        mapping.Set<string>((Expression<Func<ManyToOneMapping, string>>) (x => x.Access), 0, access.ToString());
      if (!member.IsProperty || member.CanWrite)
        return;
      mapping.Set<string>((Expression<Func<ManyToOneMapping, string>>) (x => x.Access), 0, this.cfg.GetAccessStrategyForReadOnlyProperty(member).ToString());
    }
  }
}
