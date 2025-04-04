// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.ManyToOneInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public class ManyToOneInstance : 
    ManyToOneInspector,
    IManyToOneInstance,
    IManyToOneInspector,
    IAccessInspector,
    IExposedThroughPropertyInspector,
    IInspector
  {
    private readonly ManyToOneMapping mapping;
    private bool nextBool = true;

    public ManyToOneInstance(ManyToOneMapping mapping)
      : base(mapping)
    {
      this.mapping = mapping;
    }

    public void Column(string columnName)
    {
      ColumnMapping columnMapping = this.mapping.Columns.FirstOrDefault<ColumnMapping>();
      ColumnMapping mapping = columnMapping == null ? new ColumnMapping() : columnMapping.Clone();
      mapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 1, columnName);
      this.mapping.AddColumn(1, mapping);
    }

    public void Formula(string formula)
    {
      this.mapping.Set<string>((Expression<Func<ManyToOneMapping, string>>) (x => x.Formula), 1, formula);
      this.mapping.MakeColumnsEmpty(2);
    }

    public void CustomClass<T>() => this.CustomClass(typeof (T));

    public void CustomClass(Type type)
    {
      this.mapping.Set<TypeReference>((Expression<Func<ManyToOneMapping, TypeReference>>) (x => x.Class), 1, new TypeReference(type));
    }

    public IAccessInstance Access
    {
      get
      {
        return (IAccessInstance) new AccessInstance((Action<string>) (value => this.mapping.Set<string>((Expression<Func<ManyToOneMapping, string>>) (x => x.Access), 1, value)));
      }
    }

    public ICascadeInstance Cascade
    {
      get
      {
        return (ICascadeInstance) new CascadeInstance((Action<string>) (value => this.mapping.Set<string>((Expression<Func<ManyToOneMapping, string>>) (x => x.Cascade), 1, value)));
      }
    }

    public IFetchInstance Fetch
    {
      get
      {
        return (IFetchInstance) new FetchInstance((Action<string>) (value => this.mapping.Set<string>((Expression<Func<ManyToOneMapping, string>>) (x => x.Fetch), 1, value)));
      }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IManyToOneInstance Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return (IManyToOneInstance) this;
      }
    }

    public INotFoundInstance NotFound
    {
      get
      {
        return (INotFoundInstance) new NotFoundInstance((Action<string>) (value => this.mapping.Set<string>((Expression<Func<ManyToOneMapping, string>>) (x => x.NotFound), 1, value)));
      }
    }

    public void Index(string index)
    {
      foreach (ColumnMapping column in this.mapping.Columns)
      {
        Expression<Func<ColumnMapping, string>> expression = (Expression<Func<ColumnMapping, string>>) (x => x.Index);
        string str = index;
        column.Set<string>(expression, 1, str);
      }
    }

    public void Insert()
    {
      this.mapping.Set<bool>((Expression<Func<ManyToOneMapping, bool>>) (x => x.Insert), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void OptimisticLock()
    {
      this.mapping.Set<bool>((Expression<Func<ManyToOneMapping, bool>>) (x => x.OptimisticLock), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void LazyLoad()
    {
      this.LazyLoad(this.nextBool ? Laziness.Proxy : Laziness.False);
      this.nextBool = true;
    }

    public void LazyLoad(Laziness laziness)
    {
      this.mapping.Set<string>((Expression<Func<ManyToOneMapping, string>>) (x => x.Lazy), 1, laziness.ToString());
      this.nextBool = true;
    }

    public void Nullable()
    {
      foreach (ColumnMapping column in this.mapping.Columns)
      {
        Expression<Func<ColumnMapping, bool>> expression = (Expression<Func<ColumnMapping, bool>>) (x => x.NotNull);
        int num = !this.nextBool ? 1 : 0;
        column.Set<bool>(expression, 1, num != 0);
      }
      this.nextBool = true;
    }

    public void PropertyRef(string property)
    {
      this.mapping.Set<string>((Expression<Func<ManyToOneMapping, string>>) (x => x.PropertyRef), 1, property);
    }

    public void ReadOnly()
    {
      this.mapping.Set<bool>((Expression<Func<ManyToOneMapping, bool>>) (x => x.Insert), 1, (!this.nextBool ? 1 : 0) != 0);
      this.mapping.Set<bool>((Expression<Func<ManyToOneMapping, bool>>) (x => x.Update), 1, (!this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void Unique()
    {
      foreach (ColumnMapping column in this.mapping.Columns)
      {
        Expression<Func<ColumnMapping, bool>> expression = (Expression<Func<ColumnMapping, bool>>) (x => x.Unique);
        int num = this.nextBool ? 1 : 0;
        column.Set<bool>(expression, 1, num != 0);
      }
      this.nextBool = true;
    }

    public void UniqueKey(string key)
    {
      foreach (ColumnMapping column in this.mapping.Columns)
      {
        Expression<Func<ColumnMapping, string>> expression = (Expression<Func<ColumnMapping, string>>) (x => x.UniqueKey);
        string str = key;
        column.Set<string>(expression, 1, str);
      }
    }

    public void Update()
    {
      this.mapping.Set<bool>((Expression<Func<ManyToOneMapping, bool>>) (x => x.Update), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void ForeignKey(string key)
    {
      this.mapping.Set<string>((Expression<Func<ManyToOneMapping, string>>) (x => x.ForeignKey), 1, key);
    }

    public void OverrideInferredClass(Type type)
    {
      this.mapping.Set<TypeReference>((Expression<Func<ManyToOneMapping, TypeReference>>) (x => x.Class), 1, new TypeReference(type));
    }
  }
}
