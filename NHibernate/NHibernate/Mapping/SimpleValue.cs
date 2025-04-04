// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.SimpleValue
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Id;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public class SimpleValue : IKeyValue, IValue
  {
    private readonly List<ISelectable> columns = new List<ISelectable>();
    private IType type;
    private IDictionary<string, string> typeParameters;
    private IDictionary<string, string> identifierGeneratorProperties;
    private string identifierGeneratorStrategy = "assigned";
    private string nullValue;
    private Table table;
    private string foreignKeyName;
    private bool cascadeDeleteEnabled;
    private bool isAlternateUniqueKey;
    private string typeName;

    public SimpleValue()
    {
    }

    public SimpleValue(Table table) => this.table = table;

    public virtual IEnumerable<Column> ConstraintColumns
    {
      get => (IEnumerable<Column>) new SafetyEnumerable<Column>((IEnumerable) this.columns);
    }

    public string ForeignKeyName
    {
      get => this.foreignKeyName;
      set => this.foreignKeyName = value;
    }

    public Table Table
    {
      get => this.table;
      set => this.table = value;
    }

    public IDictionary<string, string> IdentifierGeneratorProperties
    {
      get => this.identifierGeneratorProperties;
      set => this.identifierGeneratorProperties = value;
    }

    public string IdentifierGeneratorStrategy
    {
      get => this.identifierGeneratorStrategy;
      set => this.identifierGeneratorStrategy = value;
    }

    public virtual bool IsComposite => false;

    public void CreateForeignKeyOfEntity(string entityName)
    {
      if (this.HasFormula || "none".Equals(this.ForeignKeyName, StringComparison.InvariantCultureIgnoreCase))
        return;
      this.table.CreateForeignKey(this.ForeignKeyName, this.ConstraintColumns, entityName).CascadeDeleteEnabled = this.cascadeDeleteEnabled;
    }

    public bool IsCascadeDeleteEnabled
    {
      get => this.cascadeDeleteEnabled;
      set => this.cascadeDeleteEnabled = value;
    }

    public bool IsIdentityColumn(NHibernate.Dialect.Dialect dialect)
    {
      return IdentifierGeneratorFactory.GetIdentifierGeneratorClass(this.identifierGeneratorStrategy, dialect) == typeof (IdentityGenerator);
    }

    public string NullValue
    {
      get => this.nullValue;
      set => this.nullValue = value;
    }

    public virtual bool IsUpdateable => true;

    public virtual bool IsTypeSpecified => this.typeName != null;

    public IDictionary<string, string> TypeParameters
    {
      get => this.typeParameters;
      set
      {
        if (CollectionHelper.DictionaryEquals<string, string>(this.typeParameters, value))
          return;
        this.typeParameters = value;
        this.type = (IType) null;
      }
    }

    public string TypeName
    {
      get => this.typeName;
      set
      {
        if ((this.typeName != null || value == null) && (this.typeName == null || this.typeName.Equals(value)))
          return;
        this.typeName = value;
        this.type = (IType) null;
      }
    }

    public IIdentifierGenerator CreateIdentifierGenerator(
      NHibernate.Dialect.Dialect dialect,
      string defaultCatalog,
      string defaultSchema,
      RootClass rootClass)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      if (!string.IsNullOrEmpty(defaultSchema))
        dictionary[PersistentIdGeneratorParmsNames.Schema] = defaultSchema;
      if (!string.IsNullOrEmpty(defaultCatalog))
        dictionary[PersistentIdGeneratorParmsNames.Catalog] = defaultCatalog;
      if (rootClass != null)
        dictionary[IdGeneratorParmsNames.EntityName] = rootClass.EntityName;
      string quotedName1 = this.Table.GetQuotedName(dialect);
      dictionary[PersistentIdGeneratorParmsNames.Table] = quotedName1;
      IEnumerator enumerator = (IEnumerator) this.ColumnIterator.GetEnumerator();
      enumerator.MoveNext();
      string quotedName2 = ((Column) enumerator.Current).GetQuotedName(dialect);
      dictionary[PersistentIdGeneratorParmsNames.PK] = quotedName2;
      if (rootClass != null)
      {
        StringBuilder stringBuilder = new StringBuilder();
        bool flag = false;
        foreach (Table identityTable in (IEnumerable<Table>) rootClass.IdentityTables)
        {
          if (flag)
            stringBuilder.Append(", ");
          flag = true;
          stringBuilder.Append(identityTable.GetQuotedName(dialect));
        }
        dictionary[PersistentIdGeneratorParmsNames.Tables] = stringBuilder.ToString();
      }
      else
        dictionary[PersistentIdGeneratorParmsNames.Tables] = quotedName1;
      if (this.identifierGeneratorProperties != null)
        ArrayHelper.AddAll<string, string>((IDictionary<string, string>) dictionary, this.identifierGeneratorProperties);
      return IdentifierGeneratorFactory.Create(this.identifierGeneratorStrategy, this.Type, (IDictionary<string, string>) dictionary, dialect);
    }

    public virtual int ColumnSpan => this.columns.Count;

    public virtual IEnumerable<ISelectable> ColumnIterator
    {
      get => (IEnumerable<ISelectable>) this.columns;
    }

    public virtual IType Type
    {
      get
      {
        if (this.type == null)
        {
          if (string.IsNullOrEmpty(this.typeName))
            throw new MappingException("No type name specified");
          this.type = this.GetHeuristicType();
          if (this.type == null)
          {
            string message = "Could not determine type for: " + this.typeName;
            if (this.columns != null && this.columns.Count > 0)
              message = message + ", for columns: " + StringHelper.CollectionToString((ICollection) this.columns);
            throw new MappingException(message);
          }
        }
        return this.type;
      }
    }

    private IType GetHeuristicType()
    {
      IType type = (IType) null;
      if (this.ColumnSpan == 1 && !this.columns[0].IsFormula)
      {
        Column column = (Column) this.columns[0];
        if (column.IsLengthDefined())
          type = TypeFactory.BuiltInType(this.typeName, column.Length) ?? TypeFactory.HeuristicType(this.typeName, this.typeParameters, new int?(column.Length));
        else if (column.IsPrecisionDefined())
          type = TypeFactory.BuiltInType(this.typeName, Convert.ToByte(column.Precision), Convert.ToByte(column.Scale));
      }
      return type ?? TypeFactory.HeuristicType(this.typeName, this.typeParameters);
    }

    public bool HasFormula
    {
      get
      {
        foreach (ISelectable selectable in this.ColumnIterator)
        {
          if (selectable.IsFormula)
            return true;
        }
        return false;
      }
    }

    public virtual bool IsNullable
    {
      get
      {
        foreach (ISelectable selectable in this.ColumnIterator)
        {
          if (selectable.IsFormula)
            return true;
          if (!((Column) selectable).IsNullable)
            return false;
        }
        return true;
      }
    }

    public virtual bool[] ColumnUpdateability => this.ColumnInsertability;

    public virtual bool[] ColumnInsertability
    {
      get
      {
        bool[] columnInsertability = new bool[this.ColumnSpan];
        int num = 0;
        foreach (ISelectable selectable in this.ColumnIterator)
          columnInsertability[num++] = !selectable.IsFormula;
        return columnInsertability;
      }
    }

    public bool IsSimpleValue => true;

    public virtual bool IsValid(IMapping mapping)
    {
      return this.ColumnSpan == this.Type.GetColumnSpan(mapping);
    }

    public virtual void CreateForeignKey()
    {
    }

    public virtual FetchMode FetchMode
    {
      get => FetchMode.Select;
      set => throw new NotSupportedException();
    }

    public bool IsAlternateUniqueKey
    {
      get => this.isAlternateUniqueKey;
      set => this.isAlternateUniqueKey = value;
    }

    public virtual void SetTypeUsingReflection(
      string className,
      string propertyName,
      string accesorName)
    {
      if (this.typeName != null)
        return;
      if (className == null)
        throw new MappingException("you must specify types for a dynamic entity: " + propertyName);
      try
      {
        this.typeName = ReflectHelper.ReflectedPropertyClass(className, propertyName, accesorName).AssemblyQualifiedName;
      }
      catch (HibernateException ex)
      {
        throw new MappingException("Problem trying to set property type by reflection", (Exception) ex);
      }
    }

    public virtual object Accept(IValueVisitor visitor) => visitor.Accept((IValue) this);

    public virtual void AddColumn(Column column)
    {
      if (!this.columns.Contains((ISelectable) column))
        this.columns.Add((ISelectable) column);
      column.Value = (IValue) this;
      column.TypeIndex = this.columns.Count - 1;
      this.type = (IType) null;
    }

    public virtual void AddFormula(Formula formula) => this.columns.Add((ISelectable) formula);

    public override string ToString()
    {
      return string.Format("{0}({1})", (object) this.GetType().FullName, (object) StringHelper.CollectionToString((ICollection) this.columns));
    }
  }
}
