// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.ComponentJoin
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using NHibernate.Util;
using System.Text;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  public class ComponentJoin : FromElement
  {
    private readonly string columns;
    private readonly string componentPath;
    private readonly string componentProperty;
    private readonly ComponentType componentType;

    public ComponentJoin(
      FromClause fromClause,
      FromElement origin,
      string alias,
      string componentPath,
      ComponentType componentType)
      : base(fromClause, origin, alias)
    {
      this.componentPath = componentPath;
      this.componentType = componentType;
      this.componentProperty = StringHelper.Unqualify(componentPath);
      fromClause.AddJoinByPathMap(componentPath, (FromElement) this);
      this.InitializeComponentJoin((FromElementType) new ComponentJoin.ComponentFromElementType(this));
      this.columns = string.Join(", ", origin.GetPropertyMapping("").ToColumns(this.TableAlias, this.componentProperty));
    }

    public string ComponentPath => this.componentPath;

    public ComponentType ComponentType => this.componentType;

    public string ComponentProperty => this.componentProperty;

    public override IType DataType
    {
      get => (IType) this.ComponentType;
      set => base.DataType = value;
    }

    public override string GetIdentityColumn() => this.columns;

    public override string GetDisplayText()
    {
      return "ComponentJoin{path=" + this.ComponentPath + ", type=" + (object) this.componentType.ReturnedClass + "}";
    }

    public class ComponentFromElementType : FromElementType
    {
      private readonly ComponentJoin fromElement;
      private readonly IPropertyMapping propertyMapping;

      public ComponentFromElementType(ComponentJoin fromElement)
        : base((FromElement) fromElement)
      {
        this.fromElement = fromElement;
        this.propertyMapping = (IPropertyMapping) new ComponentJoin.ComponentFromElementType.ComponentPropertyMapping(this);
      }

      public ComponentJoin FromElement => this.fromElement;

      public override IType DataType => (IType) this.fromElement.ComponentType;

      public override IQueryableCollection QueryableCollection
      {
        get => (IQueryableCollection) null;
        set => base.QueryableCollection = value;
      }

      public override IPropertyMapping GetPropertyMapping(string propertyName)
      {
        return this.propertyMapping;
      }

      public override IType GetPropertyType(string propertyName, string propertyPath)
      {
        return this.fromElement.ComponentType.Subtypes[this.fromElement.ComponentType.GetPropertyIndex(propertyName)];
      }

      public override string RenderScalarIdentifierSelect(int i)
      {
        string[] columns = this.GetBasePropertyMapping().ToColumns(this.fromElement.TableAlias, this.fromElement.ComponentProperty);
        StringBuilder stringBuilder = new StringBuilder();
        for (int y = 0; y < columns.Length; ++y)
        {
          string str = columns[y];
          if (y > 0)
            stringBuilder.Append(", ");
          stringBuilder.Append(str).Append(" as ").Append(NameGenerator.ScalarName(i, y));
        }
        return stringBuilder.ToString();
      }

      protected IPropertyMapping GetBasePropertyMapping()
      {
        return this.fromElement.Origin.GetPropertyMapping("");
      }

      private class ComponentPropertyMapping : IPropertyMapping
      {
        private readonly ComponentJoin.ComponentFromElementType fromElementType;

        public ComponentPropertyMapping(
          ComponentJoin.ComponentFromElementType fromElementType)
        {
          this.fromElementType = fromElementType;
        }

        public IType Type => (IType) this.fromElementType.FromElement.ComponentType;

        public IType ToType(string propertyName)
        {
          return this.fromElementType.GetBasePropertyMapping().ToType(this.GetPropertyPath(propertyName));
        }

        public bool TryToType(string propertyName, out IType type)
        {
          return this.fromElementType.GetBasePropertyMapping().TryToType(this.GetPropertyPath(propertyName), out type);
        }

        public string[] ToColumns(string alias, string propertyName)
        {
          return this.fromElementType.GetBasePropertyMapping().ToColumns(alias, this.GetPropertyPath(propertyName));
        }

        public string[] ToColumns(string propertyName)
        {
          return this.fromElementType.GetBasePropertyMapping().ToColumns(this.GetPropertyPath(propertyName));
        }

        private string GetPropertyPath(string propertyName)
        {
          return this.fromElementType.FromElement.ComponentPath + (object) '.' + propertyName;
        }
      }
    }
  }
}
