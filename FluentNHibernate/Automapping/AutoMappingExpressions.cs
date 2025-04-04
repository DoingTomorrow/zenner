// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Automapping.AutoMappingExpressions
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.Automapping
{
  public class AutoMappingExpressions
  {
    [Obsolete("Use an instance of IAutomappingConfiguration for configuration, and override ShouldMap(Member)")]
    public Func<Member, bool> FindMembers;
    [Obsolete("Use an instance of IAutomappingConfiguration for configuration, and override IsId")]
    public Func<Member, bool> FindIdentity;
    [Obsolete("Use an instance of IAutomappingConfiguration for configuration, and override GetParentSideForManyToMany")]
    public Func<Type, Type, Type> GetParentSideForManyToMany;
    [Obsolete("Use IgnoreBase<T> or IgnoreBase(Type): AutoMap.AssemblyOf<Entity>().IgnoreBase(typeof(Parent<>))", true)]
    public Func<Type, bool> IsBaseType;
    [Obsolete("Use an instance of IAutomappingConfiguration for configuration, and override IsConcreteBaseType")]
    public Func<Type, bool> IsConcreteBaseType;
    [Obsolete("Use an instance of IAutomappingConfiguration for configuration, and override IsComponent")]
    public Func<Type, bool> IsComponentType;
    [Obsolete("Use an instance of IAutomappingConfiguration for configuration, and override GetComponentColumnPrefix")]
    public Func<Member, string> GetComponentColumnPrefix;
    [Obsolete("Use an instance of IAutomappingConfiguration for configuration, and override IsDiscriminated")]
    public Func<Type, bool> IsDiscriminated;
    [Obsolete("Use an instance of IAutomappingConfiguration for configuration, and override GetDiscriminatorColumn")]
    public Func<Type, string> DiscriminatorColumn;
    [Obsolete("Use an instance of IAutomappingConfiguration for configuration, and override IsDiscriminated", true)]
    public Func<Type, FluentNHibernate.Automapping.SubclassStrategy> SubclassStrategy;
    [Obsolete("Use an instance of IAutomappingConfiguration for configuration, and override AbstractClassIsLayerSupertype")]
    public Func<Type, bool> AbstractClassIsLayerSupertype;
    [Obsolete("Use an instance of IAutomappingConfiguration for configuration, and override SimpleTypeCollectionValueColumn")]
    public Func<Member, string> SimpleTypeCollectionValueColumn;
  }
}
