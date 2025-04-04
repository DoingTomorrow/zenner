// Decompiled with JetBrains decompiler
// Type: NHibernate.Persister.Entity.JoinedSubclassEntityPersister
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache;
using NHibernate.Engine;
using NHibernate.Mapping;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Persister.Entity
{
  public class JoinedSubclassEntityPersister : AbstractEntityPersister
  {
    private readonly int tableSpan;
    private readonly string[] tableNames;
    private readonly string[] naturalOrderTableNames;
    private readonly string[][] tableKeyColumns;
    private readonly string[][] naturalOrderTableKeyColumns;
    private readonly bool[] naturalOrderCascadeDeleteEnabled;
    private readonly string[] spaces;
    private readonly string[] subclassClosure;
    private readonly string[] subclassTableNameClosure;
    private readonly string[][] subclassTableKeyColumnClosure;
    private readonly bool[] isClassOrSuperclassTable;
    private readonly int[] naturalOrderPropertyTableNumbers;
    private readonly int[] propertyTableNumbers;
    private readonly int[] subclassPropertyTableNumberClosure;
    private readonly int[] subclassColumnTableNumberClosure;
    private readonly int[] subclassFormulaTableNumberClosure;
    private readonly Dictionary<object, string> subclassesByDiscriminatorValue = new Dictionary<object, string>();
    private readonly string[] discriminatorValues;
    private readonly string[] notNullColumnNames;
    private readonly int[] notNullColumnTableNumbers;
    private readonly string[] constraintOrderedTableNames;
    private readonly string[][] constraintOrderedKeyColumnNames;
    private readonly string discriminatorSQLString;
    private readonly object discriminatorValue;

    public JoinedSubclassEntityPersister(
      PersistentClass persistentClass,
      ICacheConcurrencyStrategy cache,
      ISessionFactoryImplementor factory,
      IMapping mapping)
      : base(persistentClass, cache, factory)
    {
      if (persistentClass.IsPolymorphic)
      {
        try
        {
          this.discriminatorValue = (object) persistentClass.SubclassId;
          this.discriminatorSQLString = this.discriminatorValue.ToString();
        }
        catch (Exception ex)
        {
          throw new MappingException("Could not format discriminator value to SQL string", ex);
        }
      }
      else
      {
        this.discriminatorValue = (object) null;
        this.discriminatorSQLString = (string) null;
      }
      if (this.OptimisticLockMode > Versioning.OptimisticLock.Version)
        throw new MappingException(string.Format("optimistic-lock=all|dirty not supported for joined-subclass mappings [{0}]", (object) this.EntityName));
      int identifierColumnSpan = this.IdentifierColumnSpan;
      List<string> stringList1 = new List<string>();
      List<string[]> strArrayList1 = new List<string[]>();
      List<bool> boolList1 = new List<bool>();
      IEnumerator<IKeyValue> enumerator = persistentClass.KeyClosureIterator.GetEnumerator();
      foreach (Table table in persistentClass.TableClosureIterator)
      {
        enumerator.MoveNext();
        IKeyValue current = enumerator.Current;
        string qualifiedName = table.GetQualifiedName(factory.Dialect, factory.Settings.DefaultCatalogName, factory.Settings.DefaultSchemaName);
        stringList1.Add(qualifiedName);
        List<string> stringList2 = new List<string>(identifierColumnSpan);
        foreach (Column column in (IEnumerable<Column>) new SafetyEnumerable<Column>((IEnumerable) current.ColumnIterator))
          stringList2.Add(column.GetQuotedName(factory.Dialect));
        strArrayList1.Add(stringList2.ToArray());
        boolList1.Add(current.IsCascadeDeleteEnabled && factory.Dialect.SupportsCascadeDelete);
      }
      this.naturalOrderTableNames = stringList1.ToArray();
      this.naturalOrderTableKeyColumns = strArrayList1.ToArray();
      this.naturalOrderCascadeDeleteEnabled = boolList1.ToArray();
      List<string> stringList3 = new List<string>();
      List<bool> boolList2 = new List<bool>();
      List<string[]> strArrayList2 = new List<string[]>();
      foreach (Table closureTable in persistentClass.SubclassTableClosureIterator)
      {
        boolList2.Add(persistentClass.IsClassOrSuperclassTable(closureTable));
        string qualifiedName = closureTable.GetQualifiedName(factory.Dialect, factory.Settings.DefaultCatalogName, factory.Settings.DefaultSchemaName);
        stringList3.Add(qualifiedName);
        List<string> stringList4 = new List<string>(identifierColumnSpan);
        foreach (Column column in closureTable.PrimaryKey.ColumnIterator)
          stringList4.Add(column.GetQuotedName(factory.Dialect));
        strArrayList2.Add(stringList4.ToArray());
      }
      this.subclassTableNameClosure = stringList3.ToArray();
      this.subclassTableKeyColumnClosure = strArrayList2.ToArray();
      this.isClassOrSuperclassTable = boolList2.ToArray();
      this.constraintOrderedTableNames = new string[this.subclassTableNameClosure.Length];
      this.constraintOrderedKeyColumnNames = new string[this.subclassTableNameClosure.Length][];
      int index1 = 0;
      int index2 = this.subclassTableNameClosure.Length - 1;
      while (index2 >= 0)
      {
        this.constraintOrderedTableNames[index1] = this.subclassTableNameClosure[index2];
        this.constraintOrderedKeyColumnNames[index1] = this.subclassTableKeyColumnClosure[index2];
        --index2;
        ++index1;
      }
      this.tableSpan = this.naturalOrderTableNames.Length;
      this.tableNames = JoinedSubclassEntityPersister.Reverse(this.naturalOrderTableNames);
      this.tableKeyColumns = JoinedSubclassEntityPersister.Reverse(this.naturalOrderTableKeyColumns);
      JoinedSubclassEntityPersister.Reverse((object[]) this.subclassTableNameClosure, this.tableSpan);
      JoinedSubclassEntityPersister.Reverse((object[]) this.subclassTableKeyColumnClosure, this.tableSpan);
      this.spaces = ArrayHelper.Join(this.tableNames, ArrayHelper.ToStringArray((ICollection<string>) persistentClass.SynchronizedTables));
      this.customSQLInsert = new SqlString[this.tableSpan];
      this.customSQLUpdate = new SqlString[this.tableSpan];
      this.customSQLDelete = new SqlString[this.tableSpan];
      this.insertCallable = new bool[this.tableSpan];
      this.updateCallable = new bool[this.tableSpan];
      this.deleteCallable = new bool[this.tableSpan];
      this.insertResultCheckStyles = new ExecuteUpdateResultCheckStyle[this.tableSpan];
      this.updateResultCheckStyles = new ExecuteUpdateResultCheckStyle[this.tableSpan];
      this.deleteResultCheckStyles = new ExecuteUpdateResultCheckStyle[this.tableSpan];
      PersistentClass persistentClass1 = persistentClass;
      int index3 = this.tableSpan - 1;
      for (; persistentClass1 != null; persistentClass1 = persistentClass1.Superclass)
      {
        this.customSQLInsert[index3] = persistentClass1.CustomSQLInsert;
        this.insertCallable[index3] = this.customSQLInsert[index3] != null && persistentClass1.IsCustomInsertCallable;
        this.insertResultCheckStyles[index3] = persistentClass1.CustomSQLInsertCheckStyle ?? ExecuteUpdateResultCheckStyle.DetermineDefault(this.customSQLInsert[index3], this.insertCallable[index3]);
        this.customSQLUpdate[index3] = persistentClass1.CustomSQLUpdate;
        this.updateCallable[index3] = this.customSQLUpdate[index3] != null && persistentClass1.IsCustomUpdateCallable;
        this.updateResultCheckStyles[index3] = persistentClass1.CustomSQLUpdateCheckStyle ?? ExecuteUpdateResultCheckStyle.DetermineDefault(this.customSQLUpdate[index3], this.updateCallable[index3]);
        this.customSQLDelete[index3] = persistentClass1.CustomSQLDelete;
        this.deleteCallable[index3] = this.customSQLDelete[index3] != null && persistentClass1.IsCustomDeleteCallable;
        this.deleteResultCheckStyles[index3] = persistentClass1.CustomSQLDeleteCheckStyle ?? ExecuteUpdateResultCheckStyle.DetermineDefault(this.customSQLDelete[index3], this.deleteCallable[index3]);
        --index3;
      }
      if (index3 != -1)
        throw new AssertionFailure("Tablespan does not match height of joined-subclass hierarchy.");
      int propertySpan = this.PropertySpan;
      this.naturalOrderPropertyTableNumbers = new int[propertySpan];
      this.propertyTableNumbers = new int[propertySpan];
      int index4 = 0;
      foreach (Property property in persistentClass.PropertyClosureIterator)
      {
        string qualifiedName = property.Value.Table.GetQualifiedName(factory.Dialect, factory.Settings.DefaultCatalogName, factory.Settings.DefaultSchemaName);
        this.propertyTableNumbers[index4] = JoinedSubclassEntityPersister.GetTableId(qualifiedName, this.tableNames);
        this.naturalOrderPropertyTableNumbers[index4] = JoinedSubclassEntityPersister.GetTableId(qualifiedName, this.naturalOrderTableNames);
        ++index4;
      }
      List<int> intList1 = new List<int>();
      List<int> intList2 = new List<int>();
      List<int> intList3 = new List<int>();
      foreach (Property property in persistentClass.SubclassPropertyClosureIterator)
      {
        int tableId = JoinedSubclassEntityPersister.GetTableId(property.Value.Table.GetQualifiedName(factory.Dialect, factory.Settings.DefaultCatalogName, factory.Settings.DefaultSchemaName), this.subclassTableNameClosure);
        intList3.Add(tableId);
        foreach (ISelectable selectable in property.ColumnIterator)
        {
          if (selectable.IsFormula)
            intList2.Add(tableId);
          else
            intList1.Add(tableId);
        }
      }
      this.subclassColumnTableNumberClosure = intList1.ToArray();
      this.subclassPropertyTableNumberClosure = intList3.ToArray();
      this.subclassFormulaTableNumberClosure = intList2.ToArray();
      int length = persistentClass.SubclassSpan + 1;
      this.subclassClosure = new string[length];
      this.subclassClosure[length - 1] = this.EntityName;
      if (persistentClass.IsPolymorphic)
      {
        this.subclassesByDiscriminatorValue[this.discriminatorValue] = this.EntityName;
        this.discriminatorValues = new string[length];
        this.discriminatorValues[length - 1] = this.discriminatorSQLString;
        this.notNullColumnTableNumbers = new int[length];
        int tableId = JoinedSubclassEntityPersister.GetTableId(persistentClass.Table.GetQualifiedName(factory.Dialect, factory.Settings.DefaultCatalogName, factory.Settings.DefaultSchemaName), this.subclassTableNameClosure);
        this.notNullColumnTableNumbers[length - 1] = tableId;
        this.notNullColumnNames = new string[length];
        this.notNullColumnNames[length - 1] = this.subclassTableKeyColumnClosure[tableId][0];
      }
      else
      {
        this.discriminatorValues = (string[]) null;
        this.notNullColumnTableNumbers = (int[]) null;
        this.notNullColumnNames = (string[]) null;
      }
      int index5 = 0;
      foreach (Subclass subclass in persistentClass.SubclassIterator)
      {
        this.subclassClosure[index5] = subclass.EntityName;
        try
        {
          if (persistentClass.IsPolymorphic)
          {
            int subclassId = subclass.SubclassId;
            this.subclassesByDiscriminatorValue[(object) subclassId] = subclass.EntityName;
            this.discriminatorValues[index5] = subclassId.ToString();
            int tableId = JoinedSubclassEntityPersister.GetTableId(subclass.Table.GetQualifiedName(factory.Dialect, factory.Settings.DefaultCatalogName, factory.Settings.DefaultSchemaName), this.subclassTableNameClosure);
            this.notNullColumnTableNumbers[index5] = tableId;
            this.notNullColumnNames[index5] = this.subclassTableKeyColumnClosure[tableId][0];
          }
        }
        catch (Exception ex)
        {
          throw new MappingException("Error parsing discriminator value", ex);
        }
        ++index5;
      }
      this.InitLockers();
      this.InitSubclassPropertyAliasesMap(persistentClass);
      this.PostConstruct(mapping);
    }

    public override IType DiscriminatorType => (IType) NHibernateUtil.Int32;

    public override string DiscriminatorSQLValue => this.discriminatorSQLString;

    public override object DiscriminatorValue => this.discriminatorValue;

    public override string[] PropertySpaces => this.spaces;

    public override string[] IdentifierColumnNames => this.tableKeyColumns[0];

    protected internal override int[] PropertyTableNumbersInSelect => this.propertyTableNumbers;

    public override bool IsMultiTable => true;

    protected override int[] SubclassColumnTableNumberClosure
    {
      get => this.subclassColumnTableNumberClosure;
    }

    protected override int[] SubclassFormulaTableNumberClosure
    {
      get => this.subclassFormulaTableNumberClosure;
    }

    protected internal override int[] PropertyTableNumbers => this.naturalOrderPropertyTableNumbers;

    public override string[] ConstraintOrderedTableNameClosure => this.constraintOrderedTableNames;

    public override string[][] ContraintOrderedTableKeyColumnClosure
    {
      get => this.constraintOrderedKeyColumnNames;
    }

    public override string RootTableName => this.naturalOrderTableNames[0];

    public override string GetSubclassPropertyTableName(int i)
    {
      return this.subclassTableNameClosure[this.subclassPropertyTableNumberClosure[i]];
    }

    public override string GetSubclassForDiscriminatorValue(object value)
    {
      string discriminatorValue;
      this.subclassesByDiscriminatorValue.TryGetValue(value, out discriminatorValue);
      return discriminatorValue;
    }

    protected override string GetTableName(int table) => this.naturalOrderTableNames[table];

    protected override string[] GetKeyColumns(int table) => this.naturalOrderTableKeyColumns[table];

    protected override bool IsTableCascadeDeleteEnabled(int j)
    {
      return this.naturalOrderCascadeDeleteEnabled[j];
    }

    protected override bool IsPropertyOfTable(int property, int table)
    {
      return this.naturalOrderPropertyTableNumbers[property] == table;
    }

    private static void Reverse(object[] objects, int len)
    {
      object[] objArray = new object[len];
      for (int index = 0; index < len; ++index)
        objArray[index] = objects[len - index - 1];
      for (int index = 0; index < len; ++index)
        objects[index] = objArray[index];
    }

    private static string[] Reverse(string[] objects)
    {
      int length = objects.Length;
      string[] strArray = new string[length];
      for (int index = 0; index < length; ++index)
        strArray[index] = objects[length - index - 1];
      return strArray;
    }

    private static string[][] Reverse(string[][] objects)
    {
      int length = objects.Length;
      string[][] strArray = new string[length][];
      for (int index = 0; index < length; ++index)
        strArray[index] = objects[length - index - 1];
      return strArray;
    }

    public override string FromTableFragment(string alias) => this.TableName + (object) ' ' + alias;

    public override string TableName => this.tableNames[0];

    private static int GetTableId(string tableName, string[] tables)
    {
      for (int tableId = 0; tableId < tables.Length; ++tableId)
      {
        if (tableName.Equals(tables[tableId]))
          return tableId;
      }
      throw new AssertionFailure(string.Format("Table [{0}] not found", (object) tableName));
    }

    protected override void AddDiscriminatorToSelect(
      NHibernate.SqlCommand.SelectFragment select,
      string name,
      string suffix)
    {
      if (!this.HasSubclasses)
        return;
      select.SetExtraSelectList(this.DiscriminatorFragment(name), this.DiscriminatorAlias);
    }

    private CaseFragment DiscriminatorFragment(string alias)
    {
      CaseFragment caseFragment = this.Factory.Dialect.CreateCaseFragment();
      for (int index = 0; index < this.discriminatorValues.Length; ++index)
        caseFragment.AddWhenColumnNotNull(this.GenerateTableAlias(alias, this.notNullColumnTableNumbers[index]), this.notNullColumnNames[index], this.discriminatorValues[index]);
      return caseFragment;
    }

    public override string FilterFragment(string alias)
    {
      return !this.HasWhere ? string.Empty : " and " + this.GetSQLWhereString(this.GenerateFilterConditionAlias(alias));
    }

    public override string GenerateFilterConditionAlias(string rootAlias)
    {
      return this.GenerateTableAlias(rootAlias, this.tableSpan - 1);
    }

    public override string[] ToColumns(string alias, string propertyName)
    {
      if (!"class".Equals(propertyName))
        return base.ToColumns(alias, propertyName);
      return new string[1]
      {
        this.DiscriminatorFragment(alias).ToSqlStringFragment()
      };
    }

    protected override int GetSubclassPropertyTableNumber(int i)
    {
      return this.subclassPropertyTableNumberClosure[i];
    }

    protected override int TableSpan => this.tableSpan;

    protected override string[] GetSubclassTableKeyColumns(int j)
    {
      return this.subclassTableKeyColumnClosure[j];
    }

    public override string GetSubclassTableName(int j) => this.subclassTableNameClosure[j];

    protected override int SubclassTableSpan => this.subclassTableNameClosure.Length;

    protected override bool IsClassOrSuperclassTable(int j) => this.isClassOrSuperclassTable[j];

    public override string GetPropertyTableName(string propertyName)
    {
      int? propertyIndexOrNull = this.EntityMetamodel.GetPropertyIndexOrNull(propertyName);
      return !propertyIndexOrNull.HasValue ? (string) null : this.tableNames[this.propertyTableNumbers[propertyIndexOrNull.Value]];
    }

    public override string GetRootTableAlias(string drivingAlias)
    {
      return this.GenerateTableAlias(drivingAlias, JoinedSubclassEntityPersister.GetTableId(this.RootTableName, this.tableNames));
    }

    public override Declarer GetSubclassPropertyDeclarer(string propertyPath)
    {
      return "class".Equals(propertyPath) ? Declarer.SubClass : base.GetSubclassPropertyDeclarer(propertyPath);
    }
  }
}
