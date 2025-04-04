// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.ClassBased.ComponentMappingBase
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Visitors;
using System;

#nullable disable
namespace FluentNHibernate.MappingModel.ClassBased
{
  [Serializable]
  public abstract class ComponentMappingBase : ClassMappingBase
  {
    private readonly AttributeStore attributes;

    protected ComponentMappingBase()
      : this(new AttributeStore())
    {
    }

    protected ComponentMappingBase(AttributeStore attributes)
      : base(attributes)
    {
      this.attributes = attributes;
    }

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
      if (this.Parent != null)
        visitor.Visit(this.Parent);
      base.AcceptVisitor(visitor);
    }

    public Type ContainingEntityType { get; set; }

    public Member Member { get; set; }

    public ParentMapping Parent => this.attributes.GetOrDefault<ParentMapping>(nameof (Parent));

    public bool Unique => this.attributes.GetOrDefault<bool>(nameof (Unique));

    public bool Insert => this.attributes.GetOrDefault<bool>(nameof (Insert));

    public bool Update => this.attributes.GetOrDefault<bool>(nameof (Update));

    public string Access => this.attributes.GetOrDefault<string>(nameof (Access));

    public bool OptimisticLock => this.attributes.GetOrDefault<bool>(nameof (OptimisticLock));

    public bool Equals(ComponentMappingBase other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return this.Equals((ClassMappingBase) other) && object.Equals((object) other.attributes, (object) this.attributes) && object.Equals((object) other.ContainingEntityType, (object) this.ContainingEntityType) && object.Equals((object) other.Member, (object) this.Member);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      return object.ReferenceEquals((object) this, obj) || this.Equals(obj as ComponentMappingBase);
    }

    public override int GetHashCode()
    {
      return ((base.GetHashCode() * 397 ^ (this.attributes != null ? this.attributes.GetHashCode() : 0)) * 397 ^ (this.ContainingEntityType != null ? this.ContainingEntityType.GetHashCode() : 0)) * 397 ^ (this.Member != (Member) null ? this.Member.GetHashCode() : 0);
    }
  }
}
