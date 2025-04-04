// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.ManyToOnePart`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class ManyToOnePart<TOther> : IManyToOneMappingProvider
  {
    private readonly AccessStrategyBuilder<ManyToOnePart<TOther>> access;
    private readonly FetchTypeExpression<ManyToOnePart<TOther>> fetch;
    private readonly NotFoundExpression<ManyToOnePart<TOther>> notFound;
    private readonly CascadeExpression<ManyToOnePart<TOther>> cascade;
    private readonly IList<string> columns = (IList<string>) new List<string>();
    private bool nextBool = true;
    private readonly AttributeStore attributes = new AttributeStore();
    private readonly AttributeStore columnAttributes = new AttributeStore();
    private readonly Type entity;
    private readonly Member member;

    public ManyToOnePart(Type entity, Member member)
    {
      this.entity = entity;
      this.member = member;
      this.access = new AccessStrategyBuilder<ManyToOnePart<TOther>>(this, (Action<string>) (value => this.attributes.Set(nameof (Access), 2, (object) value)));
      this.fetch = new FetchTypeExpression<ManyToOnePart<TOther>>(this, (Action<string>) (value => this.attributes.Set(nameof (Fetch), 2, (object) value)));
      this.cascade = new CascadeExpression<ManyToOnePart<TOther>>(this, (Action<string>) (value => this.attributes.Set(nameof (Cascade), 2, (object) value)));
      this.notFound = new NotFoundExpression<ManyToOnePart<TOther>>(this, (Action<string>) (value => this.attributes.Set(nameof (NotFound), 2, (object) value)));
      this.SetDefaultAccess();
    }

    private void SetDefaultAccess()
    {
      FluentNHibernate.Mapping.Access access = MemberAccessResolver.Resolve(this.member);
      if (access == FluentNHibernate.Mapping.Access.Property || access == FluentNHibernate.Mapping.Access.Unset)
        return;
      this.attributes.Set("Access", 0, (object) access.ToString());
    }

    public FetchTypeExpression<ManyToOnePart<TOther>> Fetch => this.fetch;

    public NotFoundExpression<ManyToOnePart<TOther>> NotFound => this.notFound;

    public ManyToOnePart<TOther> Unique()
    {
      this.columnAttributes.Set(nameof (Unique), 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    public ManyToOnePart<TOther> UniqueKey(string keyName)
    {
      this.columnAttributes.Set(nameof (UniqueKey), 2, (object) keyName);
      return this;
    }

    public ManyToOnePart<TOther> Index(string indexName)
    {
      this.columnAttributes.Set(nameof (Index), 2, (object) indexName);
      return this;
    }

    public ManyToOnePart<TOther> Class<T>() => this.Class(typeof (T));

    public ManyToOnePart<TOther> Class(Type type)
    {
      this.attributes.Set(nameof (Class), 2, (object) new TypeReference(type));
      return this;
    }

    public ManyToOnePart<TOther> ReadOnly()
    {
      this.attributes.Set("Insert", 2, (object) !this.nextBool);
      this.attributes.Set("Update", 2, (object) !this.nextBool);
      this.nextBool = true;
      return this;
    }

    public ManyToOnePart<TOther> LazyLoad()
    {
      if (this.nextBool)
        this.LazyLoad(Laziness.Proxy);
      else
        this.LazyLoad(Laziness.False);
      this.nextBool = true;
      return this;
    }

    public ManyToOnePart<TOther> LazyLoad(Laziness laziness)
    {
      this.attributes.Set("Lazy", 2, (object) laziness.ToString());
      this.nextBool = true;
      return this;
    }

    public ManyToOnePart<TOther> ForeignKey()
    {
      return this.ForeignKey(string.Format("FK_{0}To{1}", (object) this.member.DeclaringType.Name, (object) this.member.Name));
    }

    public ManyToOnePart<TOther> ForeignKey(string foreignKeyName)
    {
      this.attributes.Set(nameof (ForeignKey), 2, (object) foreignKeyName);
      return this;
    }

    public ManyToOnePart<TOther> Insert()
    {
      this.attributes.Set(nameof (Insert), 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    public ManyToOnePart<TOther> Update()
    {
      this.attributes.Set(nameof (Update), 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    public ManyToOnePart<TOther> Column(string name)
    {
      this.columns.Clear();
      this.columns.Add(name);
      return this;
    }

    public ManyToOnePart<TOther> Columns(params string[] newColumns)
    {
      foreach (string newColumn in newColumns)
        this.columns.Add(newColumn);
      return this;
    }

    public ManyToOnePart<TOther> Columns(
      params Expression<Func<TOther, object>>[] newColumns)
    {
      foreach (Expression<Func<TOther, object>> newColumn in newColumns)
        this.Columns(newColumn.ToMember<TOther, object>().Name);
      return this;
    }

    public ManyToOnePart<TOther> Formula(string formula)
    {
      this.attributes.Set(nameof (Formula), 2, (object) formula);
      return this;
    }

    public CascadeExpression<ManyToOnePart<TOther>> Cascade => this.cascade;

    public ManyToOnePart<TOther> PropertyRef(Expression<Func<TOther, object>> expression)
    {
      return this.PropertyRef(expression.ToMember<TOther, object>().Name);
    }

    public ManyToOnePart<TOther> PropertyRef(string property)
    {
      this.attributes.Set(nameof (PropertyRef), 2, (object) property);
      return this;
    }

    public ManyToOnePart<TOther> Nullable()
    {
      this.columnAttributes.Set("NotNull", 2, (object) !this.nextBool);
      this.nextBool = true;
      return this;
    }

    public ManyToOnePart<TOther> EntityName(string entityName)
    {
      this.attributes.Set(nameof (EntityName), 2, (object) entityName);
      return this;
    }

    public AccessStrategyBuilder<ManyToOnePart<TOther>> Access => this.access;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public ManyToOnePart<TOther> Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return this;
      }
    }

    ManyToOneMapping IManyToOneMappingProvider.GetManyToOneMapping()
    {
      ManyToOneMapping manyToOneMapping = new ManyToOneMapping(this.attributes.Clone())
      {
        ContainingEntityType = this.entity,
        Member = this.member
      };
      manyToOneMapping.Set<string>((Expression<Func<ManyToOneMapping, string>>) (x => x.Name), 0, this.member.Name);
      manyToOneMapping.Set<TypeReference>((Expression<Func<ManyToOneMapping, TypeReference>>) (x => x.Class), 0, new TypeReference(typeof (TOther)));
      if (this.columns.Count == 0 && !manyToOneMapping.IsSpecified("Formula"))
        manyToOneMapping.AddColumn(0, this.CreateColumn(this.member.Name + "_id"));
      foreach (string column1 in (IEnumerable<string>) this.columns)
      {
        ColumnMapping column2 = this.CreateColumn(column1);
        manyToOneMapping.AddColumn(2, column2);
      }
      return manyToOneMapping;
    }

    private ColumnMapping CreateColumn(string column)
    {
      ColumnMapping column1 = new ColumnMapping(this.columnAttributes.Clone());
      column1.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, column);
      return column1;
    }

    public ManyToOnePart<TOther> OptimisticLock()
    {
      this.attributes.Set(nameof (OptimisticLock), 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }
  }
}
