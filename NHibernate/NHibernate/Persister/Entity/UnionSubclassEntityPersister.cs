// Decompiled with JetBrains decompiler
// Type: NHibernate.Persister.Entity.UnionSubclassEntityPersister
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Cache;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Id;
using NHibernate.Mapping;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.Util;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.Persister.Entity
{
  public class UnionSubclassEntityPersister : AbstractEntityPersister
  {
    private readonly string subquery;
    private readonly string tableName;
    private readonly string[] subclassClosure;
    private readonly string[] spaces;
    private readonly string[] subclassSpaces;
    private readonly string discriminatorSQLValue;
    private readonly object discriminatorValue;
    private readonly Dictionary<int, string> subclassByDiscriminatorValue = new Dictionary<int, string>();
    private readonly string[] constraintOrderedTableNames;
    private readonly string[][] constraintOrderedKeyColumnNames;

    public UnionSubclassEntityPersister(
      PersistentClass persistentClass,
      ICacheConcurrencyStrategy cache,
      ISessionFactoryImplementor factory,
      IMapping mapping)
      : base(persistentClass, cache, factory)
    {
      if (this.IdentifierGenerator is IdentityGenerator)
        throw new MappingException("Cannot use identity column key generation with <union-subclass> mapping for: " + this.EntityName);
      this.tableName = persistentClass.Table.GetQualifiedName(factory.Dialect, factory.Settings.DefaultCatalogName, factory.Settings.DefaultSchemaName);
      SqlString customSqlInsert = persistentClass.CustomSQLInsert;
      bool callable1 = customSqlInsert != null && persistentClass.IsCustomInsertCallable;
      ExecuteUpdateResultCheckStyle resultCheckStyle1 = customSqlInsert == null ? ExecuteUpdateResultCheckStyle.Count : persistentClass.CustomSQLInsertCheckStyle ?? ExecuteUpdateResultCheckStyle.DetermineDefault(customSqlInsert, callable1);
      this.customSQLInsert = new SqlString[1]
      {
        customSqlInsert
      };
      this.insertCallable = new bool[1]{ callable1 };
      this.insertResultCheckStyles = new ExecuteUpdateResultCheckStyle[1]
      {
        resultCheckStyle1
      };
      SqlString customSqlUpdate = persistentClass.CustomSQLUpdate;
      bool callable2 = customSqlUpdate != null && persistentClass.IsCustomUpdateCallable;
      ExecuteUpdateResultCheckStyle resultCheckStyle2 = customSqlUpdate == null ? ExecuteUpdateResultCheckStyle.Count : persistentClass.CustomSQLUpdateCheckStyle ?? ExecuteUpdateResultCheckStyle.DetermineDefault(customSqlUpdate, callable2);
      this.customSQLUpdate = new SqlString[1]
      {
        customSqlUpdate
      };
      this.updateCallable = new bool[1]{ callable2 };
      this.updateResultCheckStyles = new ExecuteUpdateResultCheckStyle[1]
      {
        resultCheckStyle2
      };
      SqlString customSqlDelete = persistentClass.CustomSQLDelete;
      bool callable3 = customSqlDelete != null && persistentClass.IsCustomDeleteCallable;
      ExecuteUpdateResultCheckStyle resultCheckStyle3 = customSqlDelete == null ? ExecuteUpdateResultCheckStyle.Count : persistentClass.CustomSQLDeleteCheckStyle ?? ExecuteUpdateResultCheckStyle.DetermineDefault(customSqlDelete, callable3);
      this.customSQLDelete = new SqlString[1]
      {
        customSqlDelete
      };
      this.deleteCallable = new bool[1]{ callable3 };
      this.deleteResultCheckStyles = new ExecuteUpdateResultCheckStyle[1]
      {
        resultCheckStyle3
      };
      this.discriminatorValue = (object) persistentClass.SubclassId;
      this.discriminatorSQLValue = persistentClass.SubclassId.ToString();
      this.subclassClosure = new string[persistentClass.SubclassSpan + 1];
      this.subclassClosure[0] = this.EntityName;
      this.subclassByDiscriminatorValue[persistentClass.SubclassId] = persistentClass.EntityName;
      if (persistentClass.IsPolymorphic)
      {
        int num = 1;
        foreach (Subclass subclass in persistentClass.SubclassIterator)
        {
          this.subclassClosure[num++] = subclass.EntityName;
          this.subclassByDiscriminatorValue[subclass.SubclassId] = subclass.EntityName;
        }
      }
      int length = 1 + persistentClass.SynchronizedTables.Count;
      this.spaces = new string[length];
      this.spaces[0] = this.tableName;
      IEnumerator<string> enumerator = persistentClass.SynchronizedTables.GetEnumerator();
      for (int index = 1; index < length; ++index)
      {
        enumerator.MoveNext();
        this.spaces[index] = enumerator.Current;
      }
      HashedSet<string> hashedSet = new HashedSet<string>();
      foreach (Table table in persistentClass.SubclassTableClosureIterator)
        hashedSet.Add(table.GetQualifiedName(factory.Dialect, factory.Settings.DefaultCatalogName, factory.Settings.DefaultSchemaName));
      this.subclassSpaces = new string[hashedSet.Count];
      hashedSet.CopyTo(this.subclassSpaces, 0);
      this.subquery = this.GenerateSubquery(persistentClass, mapping);
      if (this.IsMultiTable)
      {
        int identifierColumnSpan = this.IdentifierColumnSpan;
        List<string> stringList1 = new List<string>();
        List<string[]> strArrayList = new List<string[]>();
        if (!this.IsAbstract)
        {
          stringList1.Add(this.tableName);
          strArrayList.Add(this.IdentifierColumnNames);
        }
        foreach (Table table in persistentClass.SubclassTableClosureIterator)
        {
          if (!table.IsAbstractUnionTable)
          {
            string qualifiedName = table.GetQualifiedName(factory.Dialect, factory.Settings.DefaultCatalogName, factory.Settings.DefaultSchemaName);
            stringList1.Add(qualifiedName);
            List<string> stringList2 = new List<string>(identifierColumnSpan);
            foreach (Column column in table.PrimaryKey.ColumnIterator)
              stringList2.Add(column.GetQuotedName(factory.Dialect));
            strArrayList.Add(stringList2.ToArray());
          }
        }
        this.constraintOrderedTableNames = stringList1.ToArray();
        this.constraintOrderedKeyColumnNames = strArrayList.ToArray();
      }
      else
      {
        this.constraintOrderedTableNames = new string[1]
        {
          this.tableName
        };
        this.constraintOrderedKeyColumnNames = new string[1][]
        {
          this.IdentifierColumnNames
        };
      }
      this.InitLockers();
      this.InitSubclassPropertyAliasesMap(persistentClass);
      this.PostConstruct(mapping);
    }

    public override string[] QuerySpaces => this.subclassSpaces;

    public override IType DiscriminatorType => (IType) NHibernateUtil.Int32;

    public override string DiscriminatorSQLValue => this.discriminatorSQLValue;

    public override object DiscriminatorValue => this.discriminatorValue;

    public string[] SubclassClosure => this.subclassClosure;

    public override string[] PropertySpaces => this.spaces;

    protected internal override int[] PropertyTableNumbersInSelect => new int[this.PropertySpan];

    public override bool IsMultiTable => this.IsAbstract || this.HasSubclasses;

    protected override int[] SubclassColumnTableNumberClosure
    {
      get => new int[this.SubclassColumnClosure.Length];
    }

    protected override int[] SubclassFormulaTableNumberClosure
    {
      get => new int[this.SubclassFormulaClosure.Length];
    }

    protected internal override int[] PropertyTableNumbers => new int[this.PropertySpan];

    public override string[] ConstraintOrderedTableNameClosure => this.constraintOrderedTableNames;

    public override string[][] ContraintOrderedTableKeyColumnClosure
    {
      get => this.constraintOrderedKeyColumnNames;
    }

    public override string TableName => this.subquery;

    public override string GetSubclassForDiscriminatorValue(object value)
    {
      string discriminatorValue;
      this.subclassByDiscriminatorValue.TryGetValue((int) value, out discriminatorValue);
      return discriminatorValue;
    }

    protected internal virtual bool IsDiscriminatorFormula => false;

    protected internal virtual SqlString GenerateSelectString(LockMode lockMode)
    {
      SqlSimpleSelectBuilder simpleSelectBuilder = new SqlSimpleSelectBuilder(this.Factory.Dialect, (IMapping) this.Factory).SetLockMode(lockMode).SetTableName(this.TableName).AddColumns(this.IdentifierColumnNames).AddColumns(this.SubclassColumnClosure, this.SubclassColumnAliasClosure, this.SubclassColumnLaziness).AddColumns(this.SubclassFormulaClosure, this.SubclassFormulaAliasClosure, this.SubclassFormulaLaziness);
      if (this.HasSubclasses)
      {
        if (this.IsDiscriminatorFormula)
          simpleSelectBuilder.AddColumn(this.DiscriminatorFormula, this.DiscriminatorAlias);
        else
          simpleSelectBuilder.AddColumn(this.DiscriminatorColumnName, this.DiscriminatorAlias);
      }
      if (this.Factory.Settings.IsCommentsEnabled)
        simpleSelectBuilder.SetComment("load " + this.EntityName);
      return simpleSelectBuilder.AddWhereFragment(this.IdentifierColumnNames, this.IdentifierType, "=").ToSqlString();
    }

    protected internal string DiscriminatorFormula => (string) null;

    protected override string GetTableName(int table) => this.tableName;

    protected override string[] GetKeyColumns(int table) => this.IdentifierColumnNames;

    protected override bool IsTableCascadeDeleteEnabled(int j) => false;

    protected override bool IsPropertyOfTable(int property, int j) => true;

    public override string FromTableFragment(string name) => this.TableName + (object) ' ' + name;

    public override string FilterFragment(string alias)
    {
      return !this.HasWhere ? string.Empty : " and " + this.GetSQLWhereString(alias);
    }

    public override string GetSubclassPropertyTableName(int i) => this.TableName;

    protected override void AddDiscriminatorToSelect(
      NHibernate.SqlCommand.SelectFragment select,
      string name,
      string suffix)
    {
      select.AddColumn(name, this.DiscriminatorColumnName, this.DiscriminatorAlias);
    }

    protected override int GetSubclassPropertyTableNumber(int i) => 0;

    public override int GetSubclassPropertyTableNumber(string propertyName) => 0;

    protected override int TableSpan => 1;

    protected string GenerateSubquery(PersistentClass model, IMapping mapping)
    {
      NHibernate.Dialect.Dialect dialect = this.Factory.Dialect;
      Settings settings = this.Factory.Settings;
      if (!model.HasSubclasses)
        return model.Table.GetQualifiedName(dialect, settings.DefaultCatalogName, settings.DefaultSchemaName);
      HashedSet<Column> hashedSet = new HashedSet<Column>();
      foreach (Table table in model.SubclassTableClosureIterator)
      {
        if (!table.IsAbstractUnionTable)
        {
          foreach (Column o in table.ColumnIterator)
            hashedSet.Add(o);
        }
      }
      StringBuilder stringBuilder = new StringBuilder().Append("( ");
      foreach (PersistentClass persistentClass in (IEnumerable<PersistentClass>) new JoinedEnumerable<PersistentClass>((IEnumerable<PersistentClass>) new SingletonEnumerable<PersistentClass>(model), (IEnumerable<PersistentClass>) new SafetyEnumerable<PersistentClass>((IEnumerable) model.SubclassIterator)))
      {
        Table table = persistentClass.Table;
        if (!table.IsAbstractUnionTable)
        {
          stringBuilder.Append("select ");
          foreach (Column column in (Set<Column>) hashedSet)
          {
            if (!table.ContainsColumn(column))
            {
              SqlType sqlTypeCode = column.GetSqlTypeCode(mapping);
              stringBuilder.Append(dialect.GetSelectClauseNullString(sqlTypeCode)).Append(" as ");
            }
            stringBuilder.Append(column.Name);
            stringBuilder.Append(", ");
          }
          stringBuilder.Append(persistentClass.SubclassId).Append(" as clazz_");
          stringBuilder.Append(" from ").Append(table.GetQualifiedName(dialect, settings.DefaultCatalogName, settings.DefaultSchemaName));
          stringBuilder.Append(" union ");
          if (dialect.SupportsUnionAll)
            stringBuilder.Append("all ");
        }
      }
      if (stringBuilder.Length > 2)
        stringBuilder.Length -= dialect.SupportsUnionAll ? 11 : 7;
      return stringBuilder.Append(" )").ToString();
    }

    protected override string[] GetSubclassTableKeyColumns(int j)
    {
      if (j != 0)
        throw new AssertionFailure("only one table");
      return this.IdentifierColumnNames;
    }

    public override string GetSubclassTableName(int j)
    {
      if (j != 0)
        throw new AssertionFailure("only one table");
      return this.tableName;
    }

    protected override int SubclassTableSpan => 1;

    protected override bool IsClassOrSuperclassTable(int j)
    {
      if (j != 0)
        throw new AssertionFailure("only one table");
      return true;
    }

    public override string GetPropertyTableName(string propertyName) => this.TableName;
  }
}
