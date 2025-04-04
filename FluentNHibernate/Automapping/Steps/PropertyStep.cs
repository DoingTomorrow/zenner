// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Automapping.Steps.PropertyStep
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;
using System;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Automapping.Steps
{
  public class PropertyStep : IAutomappingStep
  {
    private readonly IConventionFinder conventionFinder;
    private readonly IAutomappingConfiguration cfg;

    public PropertyStep(IConventionFinder conventionFinder, IAutomappingConfiguration cfg)
    {
      this.conventionFinder = conventionFinder;
      this.cfg = cfg;
    }

    public bool ShouldMap(Member member)
    {
      return this.HasExplicitTypeConvention(member) || PropertyStep.IsMappableToColumnType(member);
    }

    private bool HasExplicitTypeConvention(Member property)
    {
      return this.conventionFinder.Find<IPropertyConvention>().Where<IPropertyConvention>((Func<IPropertyConvention, bool>) (c =>
      {
        if (!typeof (IUserTypeConvention).IsAssignableFrom(c.GetType()))
          return false;
        ConcreteAcceptanceCriteria<IPropertyInspector> criteria = new ConcreteAcceptanceCriteria<IPropertyInspector>();
        if (c is IConventionAcceptance<IPropertyInspector> conventionAcceptance2)
          conventionAcceptance2.Accept((IAcceptanceCriteria<IPropertyInspector>) criteria);
        PropertyMapping mapping = new PropertyMapping()
        {
          Member = property
        };
        mapping.Set<TypeReference>((Expression<Func<PropertyMapping, TypeReference>>) (x => x.Type), 0, new TypeReference(property.PropertyType));
        return criteria.Matches((IInspector) new PropertyInspector(mapping));
      })).FirstOrDefault<IPropertyConvention>() != null;
    }

    private static bool IsMappableToColumnType(Member property)
    {
      return property.PropertyType.Namespace == "System" || property.PropertyType.FullName == "System.Drawing.Bitmap" || property.PropertyType.IsEnum;
    }

    public void Map(ClassMappingBase classMap, Member member)
    {
      classMap.AddProperty(this.GetPropertyMapping(classMap.Type, member));
    }

    private PropertyMapping GetPropertyMapping(Type type, Member property)
    {
      PropertyMapping mapping1 = new PropertyMapping()
      {
        ContainingEntityType = type,
        Member = property
      };
      ColumnMapping mapping2 = new ColumnMapping();
      mapping2.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, property.Name);
      mapping1.AddColumn(0, mapping2);
      mapping1.Set<string>((Expression<Func<PropertyMapping, string>>) (x => x.Name), 0, mapping1.Member.Name);
      mapping1.Set<TypeReference>((Expression<Func<PropertyMapping, TypeReference>>) (x => x.Type), 0, PropertyStep.GetDefaultType(property));
      this.SetDefaultAccess(property, mapping1);
      return mapping1;
    }

    private void SetDefaultAccess(Member member, PropertyMapping mapping)
    {
      Access access = MemberAccessResolver.Resolve(member);
      if (access != Access.Property && access != Access.Unset)
        mapping.Set<string>((Expression<Func<PropertyMapping, string>>) (x => x.Access), 0, access.ToString());
      if (!member.IsProperty || member.CanWrite)
        return;
      mapping.Set<string>((Expression<Func<PropertyMapping, string>>) (x => x.Access), 0, this.cfg.GetAccessStrategyForReadOnlyProperty(member).ToString());
    }

    private static TypeReference GetDefaultType(Member property)
    {
      TypeReference defaultType = new TypeReference(property.PropertyType);
      if (property.PropertyType.IsEnum())
        defaultType = new TypeReference(typeof (GenericEnumMapper<>).MakeGenericType(property.PropertyType));
      if (property.PropertyType.IsNullable() && property.PropertyType.IsEnum())
        defaultType = new TypeReference(typeof (GenericEnumMapper<>).MakeGenericType(property.PropertyType.GetGenericArguments()[0]));
      return defaultType;
    }
  }
}
