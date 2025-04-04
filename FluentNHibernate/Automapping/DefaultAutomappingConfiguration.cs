// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Automapping.DefaultAutomappingConfiguration
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Automapping.Steps;
using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;
using System;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Automapping
{
  public class DefaultAutomappingConfiguration : IAutomappingConfiguration
  {
    public virtual bool ShouldMap(Member member) => member.IsProperty && member.IsPublic;

    public virtual bool ShouldMap(Type type)
    {
      return !type.ClosesInterface(typeof (IAutoMappingOverride<>)) && !type.HasInterface(typeof (IMappingProvider)) && !type.IsNested && type.IsClass;
    }

    public virtual bool IsId(Member member)
    {
      return member.Name.Equals("id", StringComparison.InvariantCultureIgnoreCase);
    }

    public virtual Access GetAccessStrategyForReadOnlyProperty(Member member)
    {
      return MemberAccessResolver.Resolve(member);
    }

    public virtual Type GetParentSideForManyToMany(Type left, Type right)
    {
      return left.FullName.CompareTo(right.FullName) >= 0 ? right : left;
    }

    public virtual bool IsConcreteBaseType(Type type) => false;

    public virtual bool IsComponent(Type type) => false;

    public virtual string GetComponentColumnPrefix(Member member) => member.Name;

    public virtual bool IsDiscriminated(Type type) => false;

    public virtual string GetDiscriminatorColumn(Type type) => "discriminator";

    [Obsolete("Use IsDiscriminated instead.", true)]
    public SubclassStrategy GetSubclassStrategy(Type type) => throw new NotSupportedException();

    public virtual bool AbstractClassIsLayerSupertype(Type type) => true;

    public virtual string SimpleTypeCollectionValueColumn(Member member) => "Value";

    public virtual bool IsVersion(Member member)
    {
      List<string> stringList = new List<string>()
      {
        "version",
        "timestamp"
      };
      List<Type> typeList = new List<Type>()
      {
        typeof (int),
        typeof (long),
        typeof (TimeSpan),
        typeof (byte[])
      };
      return stringList.Contains(member.Name.ToLowerInvariant()) && typeList.Contains(member.PropertyType);
    }

    public virtual IEnumerable<IAutomappingStep> GetMappingSteps(
      AutoMapper mapper,
      IConventionFinder conventionFinder)
    {
      return (IEnumerable<IAutomappingStep>) new IAutomappingStep[7]
      {
        (IAutomappingStep) new IdentityStep((IAutomappingConfiguration) this),
        (IAutomappingStep) new VersionStep((IAutomappingConfiguration) this),
        (IAutomappingStep) new ComponentStep((IAutomappingConfiguration) this),
        (IAutomappingStep) new PropertyStep(conventionFinder, (IAutomappingConfiguration) this),
        (IAutomappingStep) new HasManyToManyStep((IAutomappingConfiguration) this),
        (IAutomappingStep) new ReferenceStep((IAutomappingConfiguration) this),
        (IAutomappingStep) new HasManyStep((IAutomappingConfiguration) this)
      };
    }
  }
}
