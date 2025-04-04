// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.OneToOnePart`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;
using System;
using System.Diagnostics;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class OneToOnePart<TOther> : IOneToOneMappingProvider
  {
    private readonly Type entity;
    private readonly Member member;
    private readonly AccessStrategyBuilder<OneToOnePart<TOther>> access;
    private readonly FetchTypeExpression<OneToOnePart<TOther>> fetch;
    private readonly CascadeExpression<OneToOnePart<TOther>> cascade;
    private readonly AttributeStore attributes = new AttributeStore();
    private bool nextBool = true;

    public OneToOnePart(Type entity, Member member)
    {
      this.access = new AccessStrategyBuilder<OneToOnePart<TOther>>(this, (Action<string>) (value => this.attributes.Set(nameof (Access), 2, (object) value)));
      this.fetch = new FetchTypeExpression<OneToOnePart<TOther>>(this, (Action<string>) (value => this.attributes.Set(nameof (Fetch), 2, (object) value)));
      this.cascade = new CascadeExpression<OneToOnePart<TOther>>(this, (Action<string>) (value => this.attributes.Set(nameof (Cascade), 2, (object) value)));
      this.entity = entity;
      this.member = member;
      this.SetDefaultAccess();
    }

    private void SetDefaultAccess()
    {
      FluentNHibernate.Mapping.Access access = MemberAccessResolver.Resolve(this.member);
      if (access == FluentNHibernate.Mapping.Access.Property || access == FluentNHibernate.Mapping.Access.Unset)
        return;
      this.attributes.Set("Access", 0, (object) access.ToString());
    }

    public OneToOnePart<TOther> Class<T>() => this.Class(typeof (T));

    public OneToOnePart<TOther> Class(Type type)
    {
      this.attributes.Set(nameof (Class), 2, (object) new TypeReference(type));
      return this;
    }

    public FetchTypeExpression<OneToOnePart<TOther>> Fetch => this.fetch;

    public OneToOnePart<TOther> ForeignKey()
    {
      return this.ForeignKey(string.Format("FK_{0}To{1}", (object) this.member.DeclaringType.Name, (object) this.member.Name));
    }

    public OneToOnePart<TOther> ForeignKey(string foreignKeyName)
    {
      this.attributes.Set(nameof (ForeignKey), 2, (object) foreignKeyName);
      return this;
    }

    public OneToOnePart<TOther> PropertyRef(Expression<Func<TOther, object>> expression)
    {
      return this.PropertyRef(expression.ToMember<TOther, object>().Name);
    }

    public OneToOnePart<TOther> PropertyRef(string propertyName)
    {
      this.attributes.Set(nameof (PropertyRef), 2, (object) propertyName);
      return this;
    }

    public OneToOnePart<TOther> Constrained()
    {
      this.attributes.Set(nameof (Constrained), 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    public CascadeExpression<OneToOnePart<TOther>> Cascade => this.cascade;

    public AccessStrategyBuilder<OneToOnePart<TOther>> Access => this.access;

    public OneToOnePart<TOther> LazyLoad()
    {
      if (this.nextBool)
        this.LazyLoad(Laziness.Proxy);
      else
        this.LazyLoad(Laziness.False);
      this.nextBool = true;
      return this;
    }

    public OneToOnePart<TOther> LazyLoad(Laziness laziness)
    {
      this.attributes.Set("Lazy", 2, (object) laziness.ToString());
      this.nextBool = true;
      return this;
    }

    public OneToOnePart<TOther> EntityName(string entityName)
    {
      this.attributes.Set(nameof (EntityName), 2, (object) entityName);
      return this;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public OneToOnePart<TOther> Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return this;
      }
    }

    OneToOneMapping IOneToOneMappingProvider.GetOneToOneMapping()
    {
      OneToOneMapping getOneToOneMapping = new OneToOneMapping(this.attributes.Clone());
      getOneToOneMapping.ContainingEntityType = this.entity;
      getOneToOneMapping.Set<TypeReference>((Expression<Func<OneToOneMapping, TypeReference>>) (x => x.Class), 0, new TypeReference(typeof (TOther)));
      getOneToOneMapping.Set<string>((Expression<Func<OneToOneMapping, string>>) (x => x.Name), 0, this.member.Name);
      return getOneToOneMapping;
    }
  }
}
