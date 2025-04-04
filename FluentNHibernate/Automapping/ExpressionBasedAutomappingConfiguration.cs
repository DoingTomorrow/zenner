// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Automapping.ExpressionBasedAutomappingConfiguration
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.Automapping
{
  internal class ExpressionBasedAutomappingConfiguration : DefaultAutomappingConfiguration
  {
    private readonly AutoMappingExpressions expressions;

    public ExpressionBasedAutomappingConfiguration(AutoMappingExpressions expressions)
    {
      this.expressions = expressions;
    }

    public override bool ShouldMap(Member member)
    {
      return this.expressions.FindMembers != null ? this.expressions.FindMembers(member) : base.ShouldMap(member);
    }

    public override bool IsId(Member member)
    {
      return this.expressions.FindIdentity != null ? this.expressions.FindIdentity(member) : base.IsId(member);
    }

    public override Type GetParentSideForManyToMany(Type left, Type right)
    {
      return this.expressions.GetParentSideForManyToMany != null ? this.expressions.GetParentSideForManyToMany(left, right) : base.GetParentSideForManyToMany(left, right);
    }

    public override bool IsConcreteBaseType(Type type)
    {
      return this.expressions.IsConcreteBaseType != null ? this.expressions.IsConcreteBaseType(type) : base.IsConcreteBaseType(type);
    }

    public override bool IsComponent(Type type)
    {
      return this.expressions.IsComponentType != null ? this.expressions.IsComponentType(type) : base.IsComponent(type);
    }

    public override string GetComponentColumnPrefix(Member member)
    {
      return this.expressions.GetComponentColumnPrefix != null ? this.expressions.GetComponentColumnPrefix(member) : base.GetComponentColumnPrefix(member);
    }

    public override bool IsDiscriminated(Type type)
    {
      return this.expressions.IsDiscriminated != null ? this.expressions.IsDiscriminated(type) : base.IsDiscriminated(type);
    }

    public override string GetDiscriminatorColumn(Type type)
    {
      return this.expressions.DiscriminatorColumn != null ? this.expressions.DiscriminatorColumn(type) : base.GetDiscriminatorColumn(type);
    }

    public override bool AbstractClassIsLayerSupertype(Type type)
    {
      return this.expressions.AbstractClassIsLayerSupertype != null ? this.expressions.AbstractClassIsLayerSupertype(type) : base.AbstractClassIsLayerSupertype(type);
    }

    public override string SimpleTypeCollectionValueColumn(Member member)
    {
      return this.expressions.SimpleTypeCollectionValueColumn != null ? this.expressions.SimpleTypeCollectionValueColumn(member) : base.SimpleTypeCollectionValueColumn(member);
    }
  }
}
