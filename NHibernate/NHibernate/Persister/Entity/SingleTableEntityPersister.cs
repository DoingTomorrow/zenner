// Decompiled with JetBrains decompiler
// Type: NHibernate.Persister.Entity.SingleTableEntityPersister
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache;
using NHibernate.Engine;
using NHibernate.Mapping;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace NHibernate.Persister.Entity
{
  public class SingleTableEntityPersister : 
    AbstractEntityPersister,
    IQueryable,
    ILoadable,
    IEntityPersister,
    IOptimisticCacheSource,
    IPropertyMapping,
    IJoinable
  {
    private readonly int joinSpan;
    private readonly string[] qualifiedTableNames;
    private readonly bool[] isInverseTable;
    private readonly bool[] isNullableTable;
    private readonly string[][] keyColumnNames;
    private readonly bool[] cascadeDeleteEnabled;
    private readonly bool hasSequentialSelects;
    private readonly string[] spaces;
    private readonly string[] subclassClosure;
    private readonly string[] subclassTableNameClosure;
    private readonly bool[] subclassTableIsLazyClosure;
    private readonly bool[] isInverseSubclassTable;
    private readonly bool[] isNullableSubclassTable;
    private readonly bool[] subclassTableSequentialSelect;
    private readonly string[][] subclassTableKeyColumnClosure;
    private readonly bool[] isClassOrSuperclassTable;
    private readonly int[] propertyTableNumbers;
    private readonly int[] subclassPropertyTableNumberClosure;
    private readonly int[] subclassColumnTableNumberClosure;
    private readonly int[] subclassFormulaTableNumberClosure;
    private readonly Dictionary<object, string> subclassesByDiscriminatorValue = new Dictionary<object, string>();
    private readonly bool forceDiscriminator;
    private readonly string discriminatorColumnName;
    private readonly string discriminatorFormula;
    private readonly string discriminatorFormulaTemplate;
    private readonly string discriminatorAlias;
    private readonly IType discriminatorType;
    private readonly string discriminatorSQLValue;
    private readonly object discriminatorValue;
    private readonly bool discriminatorInsertable;
    private readonly string[] constraintOrderedTableNames;
    private readonly string[][] constraintOrderedKeyColumnNames;
    private readonly Dictionary<string, int> propertyTableNumbersByNameAndSubclass = new Dictionary<string, int>();
    private readonly Dictionary<string, SqlString> sequentialSelectStringsByEntityName = new Dictionary<string, SqlString>();
    private static readonly object NullDiscriminator = new object();
    private static readonly object NotNullDiscriminator = new object();

    public SingleTableEntityPersister(
      PersistentClass persistentClass,
      ICacheConcurrencyStrategy cache,
      ISessionFactoryImplementor factory,
      IMapping mapping)
      : base(persistentClass, cache, factory)
    {
      this.joinSpan = persistentClass.JoinClosureSpan + 1;
      this.qualifiedTableNames = new string[this.joinSpan];
      this.isInverseTable = new bool[this.joinSpan];
      this.isNullableTable = new bool[this.joinSpan];
      this.keyColumnNames = new string[this.joinSpan][];
      this.qualifiedTableNames[0] = persistentClass.RootTable.GetQualifiedName(factory.Dialect, factory.Settings.DefaultCatalogName, factory.Settings.DefaultSchemaName);
      this.isInverseTable[0] = false;
      this.isNullableTable[0] = false;
      this.keyColumnNames[0] = this.IdentifierColumnNames;
      this.cascadeDeleteEnabled = new bool[this.joinSpan];
      this.customSQLInsert = new SqlString[this.joinSpan];
      this.customSQLUpdate = new SqlString[this.joinSpan];
      this.customSQLDelete = new SqlString[this.joinSpan];
      this.insertCallable = new bool[this.joinSpan];
      this.updateCallable = new bool[this.joinSpan];
      this.deleteCallable = new bool[this.joinSpan];
      this.insertResultCheckStyles = new ExecuteUpdateResultCheckStyle[this.joinSpan];
      this.updateResultCheckStyles = new ExecuteUpdateResultCheckStyle[this.joinSpan];
      this.deleteResultCheckStyles = new ExecuteUpdateResultCheckStyle[this.joinSpan];
      this.customSQLInsert[0] = persistentClass.CustomSQLInsert;
      this.insertCallable[0] = this.customSQLInsert[0] != null && persistentClass.IsCustomInsertCallable;
      this.insertResultCheckStyles[0] = persistentClass.CustomSQLInsertCheckStyle ?? ExecuteUpdateResultCheckStyle.DetermineDefault(this.customSQLInsert[0], this.insertCallable[0]);
      this.customSQLUpdate[0] = persistentClass.CustomSQLUpdate;
      this.updateCallable[0] = this.customSQLUpdate[0] != null && persistentClass.IsCustomUpdateCallable;
      this.updateResultCheckStyles[0] = persistentClass.CustomSQLUpdateCheckStyle ?? ExecuteUpdateResultCheckStyle.DetermineDefault(this.customSQLUpdate[0], this.updateCallable[0]);
      this.customSQLDelete[0] = persistentClass.CustomSQLDelete;
      this.deleteCallable[0] = this.customSQLDelete[0] != null && persistentClass.IsCustomDeleteCallable;
      this.deleteResultCheckStyles[0] = persistentClass.CustomSQLDeleteCheckStyle ?? ExecuteUpdateResultCheckStyle.DetermineDefault(this.customSQLDelete[0], this.deleteCallable[0]);
      int index1 = 1;
      foreach (NHibernate.Mapping.Join join in persistentClass.JoinClosureIterator)
      {
        this.qualifiedTableNames[index1] = join.Table.GetQualifiedName(factory.Dialect, factory.Settings.DefaultCatalogName, factory.Settings.DefaultSchemaName);
        this.isInverseTable[index1] = join.IsInverse;
        this.isNullableTable[index1] = join.IsOptional;
        this.cascadeDeleteEnabled[index1] = join.Key.IsCascadeDeleteEnabled && factory.Dialect.SupportsCascadeDelete;
        this.customSQLInsert[index1] = join.CustomSQLInsert;
        this.insertCallable[index1] = this.customSQLInsert[index1] != null && join.IsCustomInsertCallable;
        this.insertResultCheckStyles[index1] = join.CustomSQLInsertCheckStyle ?? ExecuteUpdateResultCheckStyle.DetermineDefault(this.customSQLInsert[index1], this.insertCallable[index1]);
        this.customSQLUpdate[index1] = join.CustomSQLUpdate;
        this.updateCallable[index1] = this.customSQLUpdate[index1] != null && join.IsCustomUpdateCallable;
        this.updateResultCheckStyles[index1] = join.CustomSQLUpdateCheckStyle ?? ExecuteUpdateResultCheckStyle.DetermineDefault(this.customSQLUpdate[index1], this.updateCallable[index1]);
        this.customSQLDelete[index1] = join.CustomSQLDelete;
        this.deleteCallable[index1] = this.customSQLDelete[index1] != null && join.IsCustomDeleteCallable;
        this.deleteResultCheckStyles[index1] = join.CustomSQLDeleteCheckStyle ?? ExecuteUpdateResultCheckStyle.DetermineDefault(this.customSQLDelete[index1], this.deleteCallable[index1]);
        this.keyColumnNames[index1] = join.Key.ColumnIterator.OfType<Column>().Select<Column, string>((Func<Column, string>) (col => col.GetQuotedName(factory.Dialect))).ToArray<string>();
        ++index1;
      }
      this.constraintOrderedTableNames = new string[this.qualifiedTableNames.Length];
      this.constraintOrderedKeyColumnNames = new string[this.qualifiedTableNames.Length][];
      int index2 = this.qualifiedTableNames.Length - 1;
      int index3 = 0;
      while (index2 >= 0)
      {
        this.constraintOrderedTableNames[index3] = this.qualifiedTableNames[index2];
        this.constraintOrderedKeyColumnNames[index3] = this.keyColumnNames[index2];
        --index2;
        ++index3;
      }
      this.spaces = ((IEnumerable<string>) this.qualifiedTableNames).Concat<string>((IEnumerable<string>) persistentClass.SynchronizedTables).ToArray<string>();
      bool flag1 = this.IsInstrumented(EntityMode.Poco);
      bool flag2 = false;
      List<string> stringList = new List<string>();
      List<string[]> strArrayList = new List<string[]>();
      List<bool> boolList1 = new List<bool>();
      List<bool> boolList2 = new List<bool>();
      List<bool> boolList3 = new List<bool>();
      List<bool> boolList4 = new List<bool>();
      List<bool> boolList5 = new List<bool>();
      stringList.Add(this.qualifiedTableNames[0]);
      strArrayList.Add(this.IdentifierColumnNames);
      boolList1.Add(true);
      boolList2.Add(false);
      boolList3.Add(false);
      boolList4.Add(false);
      boolList5.Add(false);
      foreach (NHibernate.Mapping.Join join in persistentClass.SubclassJoinClosureIterator)
      {
        boolList1.Add(persistentClass.IsClassOrSuperclassJoin(join));
        boolList2.Add(join.IsSequentialSelect);
        boolList3.Add(join.IsInverse);
        boolList4.Add(join.IsOptional);
        boolList5.Add(flag1 && join.IsLazy);
        if (join.IsSequentialSelect && !persistentClass.IsClassOrSuperclassJoin(join))
          flag2 = true;
        stringList.Add(join.Table.GetQualifiedName(factory.Dialect, factory.Settings.DefaultCatalogName, factory.Settings.DefaultSchemaName));
        string[] array = join.Key.ColumnIterator.OfType<Column>().Select<Column, string>((Func<Column, string>) (col => col.GetQuotedName(factory.Dialect))).ToArray<string>();
        strArrayList.Add(array);
      }
      this.subclassTableSequentialSelect = boolList2.ToArray();
      this.subclassTableNameClosure = stringList.ToArray();
      this.subclassTableIsLazyClosure = boolList5.ToArray();
      this.subclassTableKeyColumnClosure = strArrayList.ToArray();
      this.isClassOrSuperclassTable = boolList1.ToArray();
      this.isInverseSubclassTable = boolList3.ToArray();
      this.isNullableSubclassTable = boolList4.ToArray();
      this.hasSequentialSelects = flag2;
      if (persistentClass.IsPolymorphic)
      {
        IValue discriminator = persistentClass.Discriminator;
        if (discriminator == null)
          throw new MappingException("Discriminator mapping required for single table polymorphic persistence");
        this.forceDiscriminator = persistentClass.IsForceDiscriminator;
        IEnumerator<ISelectable> enumerator = discriminator.ColumnIterator.GetEnumerator();
        enumerator.MoveNext();
        ISelectable current = enumerator.Current;
        if (discriminator.HasFormula)
        {
          Formula formula = (Formula) current;
          this.discriminatorFormula = formula.FormulaString;
          this.discriminatorFormulaTemplate = formula.GetTemplate(factory.Dialect, factory.SQLFunctionRegistry);
          this.discriminatorColumnName = (string) null;
          this.discriminatorAlias = "clazz_";
        }
        else
        {
          Column column = (Column) current;
          this.discriminatorColumnName = column.GetQuotedName(factory.Dialect);
          this.discriminatorAlias = column.GetAlias(factory.Dialect, persistentClass.RootTable);
          this.discriminatorFormula = (string) null;
          this.discriminatorFormulaTemplate = (string) null;
        }
        this.discriminatorType = persistentClass.Discriminator.Type;
        if (persistentClass.IsDiscriminatorValueNull)
        {
          this.discriminatorValue = SingleTableEntityPersister.NullDiscriminator;
          this.discriminatorSQLValue = InFragment.Null;
          this.discriminatorInsertable = false;
        }
        else if (persistentClass.IsDiscriminatorValueNotNull)
        {
          this.discriminatorValue = SingleTableEntityPersister.NotNullDiscriminator;
          this.discriminatorSQLValue = InFragment.NotNull;
          this.discriminatorInsertable = false;
        }
        else
        {
          this.discriminatorInsertable = persistentClass.IsDiscriminatorInsertable && !discriminator.HasFormula;
          try
          {
            IDiscriminatorType discriminatorType = (IDiscriminatorType) this.discriminatorType;
            this.discriminatorValue = discriminatorType.StringToObject(persistentClass.DiscriminatorValue);
            this.discriminatorSQLValue = discriminatorType.ObjectToSQLString(this.discriminatorValue, factory.Dialect);
          }
          catch (InvalidCastException ex)
          {
            throw new MappingException(string.Format("Illegal discriminator type: {0} of entity {1}", (object) this.discriminatorType.Name, (object) persistentClass.EntityName), (Exception) ex);
          }
          catch (Exception ex)
          {
            throw new MappingException("Could not format discriminator value to SQL string of entity " + persistentClass.EntityName, ex);
          }
        }
      }
      else
      {
        this.forceDiscriminator = false;
        this.discriminatorInsertable = false;
        this.discriminatorColumnName = (string) null;
        this.discriminatorAlias = (string) null;
        this.discriminatorType = (IType) null;
        this.discriminatorValue = (object) null;
        this.discriminatorSQLValue = (string) null;
        this.discriminatorFormula = (string) null;
        this.discriminatorFormulaTemplate = (string) null;
      }
      this.propertyTableNumbers = new int[this.PropertySpan];
      int num1 = 0;
      foreach (Property prop in persistentClass.PropertyClosureIterator)
        this.propertyTableNumbers[num1++] = persistentClass.GetJoinNumber(prop);
      List<int> intList1 = new List<int>();
      List<int> intList2 = new List<int>();
      List<int> intList3 = new List<int>();
      foreach (Property prop in persistentClass.SubclassPropertyClosureIterator)
      {
        int joinNumber = persistentClass.GetJoinNumber(prop);
        intList3.Add(joinNumber);
        this.propertyTableNumbersByNameAndSubclass[prop.PersistentClass.EntityName + (object) '.' + prop.Name] = joinNumber;
        foreach (ISelectable selectable in prop.ColumnIterator)
        {
          if (selectable.IsFormula)
            intList2.Add(joinNumber);
          else
            intList1.Add(joinNumber);
        }
      }
      this.subclassColumnTableNumberClosure = intList1.ToArray();
      this.subclassFormulaTableNumberClosure = intList2.ToArray();
      this.subclassPropertyTableNumberClosure = intList3.ToArray();
      this.subclassClosure = new string[persistentClass.SubclassSpan + 1];
      this.subclassClosure[0] = this.EntityName;
      if (persistentClass.IsPolymorphic)
        this.subclassesByDiscriminatorValue[this.discriminatorValue] = this.EntityName;
      if (persistentClass.IsPolymorphic)
      {
        int num2 = 1;
        foreach (Subclass subclass in persistentClass.SubclassIterator)
        {
          this.subclassClosure[num2++] = subclass.EntityName;
          if (subclass.IsDiscriminatorValueNull)
            this.subclassesByDiscriminatorValue[SingleTableEntityPersister.NullDiscriminator] = subclass.EntityName;
          else if (subclass.IsDiscriminatorValueNotNull)
          {
            this.subclassesByDiscriminatorValue[SingleTableEntityPersister.NotNullDiscriminator] = subclass.EntityName;
          }
          else
          {
            if (this.discriminatorType == null)
              throw new MappingException("Not available discriminator type of entity " + persistentClass.EntityName);
            try
            {
              this.subclassesByDiscriminatorValue[((IIdentifierType) this.discriminatorType).StringToObject(subclass.DiscriminatorValue)] = subclass.EntityName;
            }
            catch (InvalidCastException ex)
            {
              throw new MappingException(string.Format("Illegal discriminator type: {0} of entity {1}", (object) this.discriminatorType.Name, (object) persistentClass.EntityName), (Exception) ex);
            }
            catch (Exception ex)
            {
              throw new MappingException("Error parsing discriminator value of entity " + persistentClass.EntityName, ex);
            }
          }
        }
      }
      this.InitLockers();
      this.InitSubclassPropertyAliasesMap(persistentClass);
      this.PostConstruct(mapping);
    }

    public override string DiscriminatorColumnName => this.discriminatorColumnName;

    protected override string DiscriminatorFormulaTemplate => this.discriminatorFormulaTemplate;

    public override IType DiscriminatorType => this.discriminatorType;

    public override string DiscriminatorSQLValue => this.discriminatorSQLValue;

    public override object DiscriminatorValue => this.discriminatorValue;

    public virtual string[] SubclassClosure => this.subclassClosure;

    public override string[] PropertySpaces => this.spaces;

    protected internal override int[] PropertyTableNumbersInSelect => this.propertyTableNumbers;

    protected override int[] SubclassColumnTableNumberClosure
    {
      get => this.subclassColumnTableNumberClosure;
    }

    protected override int[] SubclassFormulaTableNumberClosure
    {
      get => this.subclassFormulaTableNumberClosure;
    }

    protected internal override int[] PropertyTableNumbers => this.propertyTableNumbers;

    public override bool IsMultiTable => this.TableSpan > 1;

    public override string[] ConstraintOrderedTableNameClosure => this.constraintOrderedTableNames;

    public override string[][] ContraintOrderedTableKeyColumnClosure
    {
      get => this.constraintOrderedKeyColumnNames;
    }

    protected override bool IsInverseTable(int j) => this.isInverseTable[j];

    protected override bool IsInverseSubclassTable(int j) => this.isInverseSubclassTable[j];

    protected internal override string DiscriminatorAlias => this.discriminatorAlias;

    public override string TableName => this.qualifiedTableNames[0];

    public override string GetSubclassForDiscriminatorValue(object value)
    {
      string discriminatorValue;
      if (value == null)
        this.subclassesByDiscriminatorValue.TryGetValue(SingleTableEntityPersister.NullDiscriminator, out discriminatorValue);
      else if (!this.subclassesByDiscriminatorValue.TryGetValue(value, out discriminatorValue))
        this.subclassesByDiscriminatorValue.TryGetValue(SingleTableEntityPersister.NotNullDiscriminator, out discriminatorValue);
      return discriminatorValue;
    }

    protected bool IsDiscriminatorFormula => this.discriminatorColumnName == null;

    protected string DiscriminatorFormula => this.discriminatorFormula;

    protected override string GetTableName(int table) => this.qualifiedTableNames[table];

    protected override string[] GetKeyColumns(int table) => this.keyColumnNames[table];

    protected override bool IsTableCascadeDeleteEnabled(int j) => this.cascadeDeleteEnabled[j];

    protected override bool IsPropertyOfTable(int property, int table)
    {
      return this.propertyTableNumbers[property] == table;
    }

    protected override bool IsSubclassTableSequentialSelect(int table)
    {
      return this.subclassTableSequentialSelect[table] && !this.isClassOrSuperclassTable[table];
    }

    public override string FromTableFragment(string name) => this.TableName + (object) ' ' + name;

    public override string FilterFragment(string alias)
    {
      string str = this.DiscriminatorFilterFragment(alias);
      if (this.HasWhere)
        str = str + " and " + this.GetSQLWhereString(alias);
      return str;
    }

    public override string OneToManyFilterFragment(string alias)
    {
      return !this.forceDiscriminator ? string.Empty : this.DiscriminatorFilterFragment(alias);
    }

    private string DiscriminatorFilterFragment(string alias)
    {
      if (!this.NeedsDiscriminator)
        return string.Empty;
      InFragment inFragment = new InFragment();
      if (this.IsDiscriminatorFormula)
        inFragment.SetFormula(alias, this.DiscriminatorFormulaTemplate);
      else
        inFragment.SetColumn(alias, this.DiscriminatorColumnName);
      string[] subclassClosure = this.SubclassClosure;
      int num = 0;
      foreach (string entityName in subclassClosure)
      {
        IQueryable entityPersister = (IQueryable) this.Factory.GetEntityPersister(entityName);
        if (!entityPersister.IsAbstract)
        {
          inFragment.AddValue((object) entityPersister.DiscriminatorSQLValue);
          ++num;
        }
      }
      if (num == 0)
        throw new NotSupportedException(string.Format("The class {0} can't be instatiated and does not have mapped subclasses; \npossible solutions:\n- don't map the abstract class\n- map the its subclasses.", (object) subclassClosure[0]));
      return new StringBuilder(50).Append(" and ").Append(inFragment.ToFragmentString().ToString()).ToString();
    }

    private bool NeedsDiscriminator => this.forceDiscriminator || this.IsInherited;

    public override string GetSubclassPropertyTableName(int i)
    {
      return this.subclassTableNameClosure[this.subclassPropertyTableNumberClosure[i]];
    }

    protected override void AddDiscriminatorToSelect(
      NHibernate.SqlCommand.SelectFragment select,
      string name,
      string suffix)
    {
      if (this.IsDiscriminatorFormula)
        select.AddFormula(name, this.DiscriminatorFormulaTemplate, this.DiscriminatorAlias);
      else
        select.AddColumn(name, this.DiscriminatorColumnName, this.DiscriminatorAlias);
    }

    protected override int GetSubclassPropertyTableNumber(int i)
    {
      return this.subclassPropertyTableNumberClosure[i];
    }

    protected override int TableSpan => this.joinSpan;

    protected override void AddDiscriminatorToInsert(SqlInsertBuilder insert)
    {
      if (!this.discriminatorInsertable)
        return;
      insert.AddColumn(this.DiscriminatorColumnName, this.DiscriminatorSQLValue);
    }

    protected override bool IsSubclassPropertyDeferred(string propertyName, string entityName)
    {
      return this.hasSequentialSelects && this.IsSubclassTableSequentialSelect(this.GetSubclassPropertyTableNumber(propertyName, entityName));
    }

    public override bool HasSequentialSelect => this.hasSequentialSelects;

    public int GetSubclassPropertyTableNumber(string propertyName, string entityName)
    {
      IType type = this.propertyMapping.ToType(propertyName);
      if (type.IsAssociationType && ((IAssociationType) type).UseLHSPrimaryKey)
        return 0;
      int propertyTableNumber;
      this.propertyTableNumbersByNameAndSubclass.TryGetValue(entityName + (object) '.' + propertyName, out propertyTableNumber);
      return propertyTableNumber;
    }

    protected override SqlString GetSequentialSelect(string entityName)
    {
      SqlString sequentialSelect;
      this.sequentialSelectStringsByEntityName.TryGetValue(entityName, out sequentialSelect);
      return sequentialSelect;
    }

    private SqlString GenerateSequentialSelect(ILoadable persister)
    {
      AbstractEntityPersister abstractEntityPersister = (AbstractEntityPersister) persister;
      HashSet<int> source = new HashSet<int>();
      string[] propertyNames = abstractEntityPersister.PropertyNames;
      string[] propertySubclassNames = abstractEntityPersister.PropertySubclassNames;
      for (int index = 0; index < propertyNames.Length; ++index)
      {
        int propertyTableNumber = this.GetSubclassPropertyTableNumber(propertyNames[index], propertySubclassNames[index]);
        if (this.IsSubclassTableSequentialSelect(propertyTableNumber) && !this.IsSubclassTableLazy(propertyTableNumber))
          source.Add(propertyTableNumber);
      }
      if (source.Count == 0)
        return (SqlString) null;
      List<int> intList1 = new List<int>();
      int[] tableNumberClosure1 = this.SubclassColumnTableNumberClosure;
      for (int index = 0; index < this.SubclassColumnClosure.Length; ++index)
      {
        if (source.Contains(tableNumberClosure1[index]))
          intList1.Add(index);
      }
      List<int> intList2 = new List<int>();
      int[] tableNumberClosure2 = this.SubclassFormulaTableNumberClosure;
      for (int index = 0; index < this.SubclassFormulaTemplateClosure.Length; ++index)
      {
        if (source.Contains(tableNumberClosure2[index]))
          intList2.Add(index);
      }
      return this.RenderSelect(source.ToArray<int>(), intList1.ToArray(), intList2.ToArray());
    }

    protected override string[] GetSubclassTableKeyColumns(int j)
    {
      return this.subclassTableKeyColumnClosure[j];
    }

    public override string GetSubclassTableName(int j) => this.subclassTableNameClosure[j];

    protected override int SubclassTableSpan => this.subclassTableNameClosure.Length;

    protected override bool IsClassOrSuperclassTable(int j) => this.isClassOrSuperclassTable[j];

    protected internal override bool IsSubclassTableLazy(int j)
    {
      return this.subclassTableIsLazyClosure[j];
    }

    protected override bool IsNullableTable(int j) => this.isNullableTable[j];

    protected override bool IsNullableSubclassTable(int j) => this.isNullableSubclassTable[j];

    public override string GetPropertyTableName(string propertyName)
    {
      int? propertyIndexOrNull = this.EntityMetamodel.GetPropertyIndexOrNull(propertyName);
      return !propertyIndexOrNull.HasValue ? (string) null : this.qualifiedTableNames[this.propertyTableNumbers[propertyIndexOrNull.Value]];
    }

    public override void PostInstantiate()
    {
      base.PostInstantiate();
      if (!this.hasSequentialSelects)
        return;
      string[] subclassClosure = this.SubclassClosure;
      for (int index = 1; index < subclassClosure.Length; ++index)
      {
        ILoadable entityPersister = (ILoadable) this.Factory.GetEntityPersister(subclassClosure[index]);
        if (!entityPersister.IsAbstract)
        {
          SqlString sequentialSelect = this.GenerateSequentialSelect(entityPersister);
          this.sequentialSelectStringsByEntityName[subclassClosure[index]] = sequentialSelect;
        }
      }
    }
  }
}
