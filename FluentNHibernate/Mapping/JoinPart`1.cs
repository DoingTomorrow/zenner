// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.JoinPart`1
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
  public class JoinPart<T> : ClasslikeMapBase<T>, IJoinMappingProvider
  {
    private readonly MappingProviderStore providers;
    private readonly IList<string> columns = (IList<string>) new List<string>();
    private readonly FetchTypeExpression<JoinPart<T>> fetch;
    private readonly AttributeStore attributes = new AttributeStore();
    private bool nextBool = true;

    public JoinPart(string tableName)
      : this(tableName, new MappingProviderStore())
    {
    }

    protected JoinPart(string tableName, MappingProviderStore providers)
      : base(providers)
    {
      this.providers = providers;
      this.fetch = new FetchTypeExpression<JoinPart<T>>(this, (Action<string>) (value => this.attributes.Set(nameof (Fetch), 2, (object) value)));
      this.attributes.Set("TableName", 0, (object) tableName);
      this.attributes.Set("Key", 0, (object) new KeyMapping()
      {
        ContainingEntityType = typeof (T)
      });
    }

    public JoinPart<T> KeyColumn(string column)
    {
      this.columns.Clear();
      this.columns.Add(column);
      return this;
    }

    public JoinPart<T> KeyColumn(params string[] columnNames)
    {
      this.columns.Clear();
      ((IEnumerable<string>) columnNames).Each<string>(new Action<string>(((ICollection<string>) this.columns).Add));
      return this;
    }

    public JoinPart<T> Schema(string schema)
    {
      this.attributes.Set(nameof (Schema), 2, (object) schema);
      return this;
    }

    public FetchTypeExpression<JoinPart<T>> Fetch => this.fetch;

    public JoinPart<T> Inverse()
    {
      this.attributes.Set(nameof (Inverse), 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    public JoinPart<T> Optional()
    {
      this.attributes.Set(nameof (Optional), 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    public JoinPart<T> Catalog(string catalog)
    {
      this.attributes.Set(nameof (Catalog), 2, (object) catalog);
      return this;
    }

    public JoinPart<T> Subselect(string subselect)
    {
      this.attributes.Set(nameof (Subselect), 2, (object) subselect);
      return this;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public JoinPart<T> Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return this;
      }
    }

    public void Table(string tableName) => this.attributes.Set("TableName", 2, (object) tableName);

    JoinMapping IJoinMappingProvider.GetJoinMapping()
    {
      JoinMapping joinMapping = new JoinMapping(this.attributes.Clone())
      {
        ContainingEntityType = typeof (T)
      };
      if (this.columns.Count == 0)
      {
        ColumnMapping mapping = new ColumnMapping();
        mapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, typeof (T).Name + "_id");
        joinMapping.Key.AddColumn(0, mapping);
      }
      else
      {
        foreach (string column in (IEnumerable<string>) this.columns)
        {
          ColumnMapping mapping = new ColumnMapping();
          mapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, column);
          joinMapping.Key.AddColumn(2, mapping);
        }
      }
      foreach (IPropertyMappingProvider property in (IEnumerable<IPropertyMappingProvider>) this.providers.Properties)
        joinMapping.AddProperty(property.GetPropertyMapping());
      foreach (IComponentMappingProvider component in (IEnumerable<IComponentMappingProvider>) this.providers.Components)
        joinMapping.AddComponent(component.GetComponentMapping());
      foreach (IManyToOneMappingProvider reference in (IEnumerable<IManyToOneMappingProvider>) this.providers.References)
        joinMapping.AddReference(reference.GetManyToOneMapping());
      foreach (IAnyMappingProvider any in (IEnumerable<IAnyMappingProvider>) this.providers.Anys)
        joinMapping.AddAny(any.GetAnyMapping());
      foreach (ICollectionMappingProvider collection in (IEnumerable<ICollectionMappingProvider>) this.providers.Collections)
        joinMapping.AddCollection(collection.GetCollectionMapping());
      return joinMapping;
    }
  }
}
