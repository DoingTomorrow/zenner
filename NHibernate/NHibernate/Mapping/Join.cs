// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.Join
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public class Join : ISqlCustomizable
  {
    private static readonly Alias PK_ALIAS = new Alias(15, "PK");
    private readonly List<Property> properties = new List<Property>();
    private Table table;
    private IKeyValue key;
    private PersistentClass persistentClass;
    private bool isSequentialSelect;
    private bool isInverse;
    private bool isOptional;
    private bool? isLazy;
    private SqlString customSQLInsert;
    private bool customInsertCallable;
    private ExecuteUpdateResultCheckStyle insertCheckStyle;
    private SqlString customSQLUpdate;
    private bool customUpdateCallable;
    private ExecuteUpdateResultCheckStyle updateCheckStyle;
    private SqlString customSQLDelete;
    private bool customDeleteCallable;
    private ExecuteUpdateResultCheckStyle deleteCheckStyle;

    public void AddProperty(Property prop)
    {
      this.properties.Add(prop);
      prop.PersistentClass = this.PersistentClass;
    }

    public bool ContainsProperty(Property prop) => this.properties.Contains(prop);

    public IEnumerable<Property> PropertyIterator => (IEnumerable<Property>) this.properties;

    public virtual Table Table
    {
      get => this.table;
      set => this.table = value;
    }

    public virtual IKeyValue Key
    {
      get => this.key;
      set => this.key = value;
    }

    public virtual PersistentClass PersistentClass
    {
      get => this.persistentClass;
      set => this.persistentClass = value;
    }

    public void CreateForeignKey()
    {
      if (this.IsInverse)
        return;
      this.Key.CreateForeignKeyOfEntity(this.persistentClass.EntityName);
    }

    public void CreatePrimaryKey(NHibernate.Dialect.Dialect dialect)
    {
      PrimaryKey primaryKey = new PrimaryKey();
      primaryKey.Table = this.table;
      primaryKey.Name = Join.PK_ALIAS.ToAliasString(this.table.Name, dialect);
      this.table.PrimaryKey = primaryKey;
      primaryKey.AddColumns(this.Key.ColumnIterator.OfType<Column>());
    }

    public int PropertySpan => this.properties.Count;

    public SqlString CustomSQLInsert => this.customSQLInsert;

    public SqlString CustomSQLDelete => this.customSQLDelete;

    public SqlString CustomSQLUpdate => this.customSQLUpdate;

    public bool IsCustomInsertCallable => this.customInsertCallable;

    public bool IsCustomDeleteCallable => this.customDeleteCallable;

    public bool IsCustomUpdateCallable => this.customUpdateCallable;

    public ExecuteUpdateResultCheckStyle CustomSQLInsertCheckStyle => this.insertCheckStyle;

    public ExecuteUpdateResultCheckStyle CustomSQLDeleteCheckStyle => this.deleteCheckStyle;

    public ExecuteUpdateResultCheckStyle CustomSQLUpdateCheckStyle => this.updateCheckStyle;

    public void SetCustomSQLInsert(
      string sql,
      bool callable,
      ExecuteUpdateResultCheckStyle checkStyle)
    {
      this.customSQLInsert = SqlString.Parse(sql);
      this.customInsertCallable = callable;
      this.insertCheckStyle = checkStyle;
    }

    public void SetCustomSQLDelete(
      string sql,
      bool callable,
      ExecuteUpdateResultCheckStyle checkStyle)
    {
      this.customSQLDelete = SqlString.Parse(sql);
      this.customDeleteCallable = callable;
      this.deleteCheckStyle = checkStyle;
    }

    public void SetCustomSQLUpdate(
      string sql,
      bool callable,
      ExecuteUpdateResultCheckStyle checkStyle)
    {
      this.customSQLUpdate = SqlString.Parse(sql);
      this.customUpdateCallable = callable;
      this.updateCheckStyle = checkStyle;
    }

    public virtual bool IsSequentialSelect
    {
      get => this.isSequentialSelect;
      set => this.isSequentialSelect = value;
    }

    public virtual bool IsInverse
    {
      get => this.isInverse;
      set => this.isInverse = value;
    }

    public bool IsLazy
    {
      get
      {
        if (!this.isLazy.HasValue)
          this.isLazy = new bool?(!this.PropertyIterator.Any<Property>((Func<Property, bool>) (property => !property.IsLazy)));
        return this.isLazy.Value;
      }
    }

    public virtual bool IsOptional
    {
      get => this.isOptional;
      set => this.isOptional = value;
    }
  }
}
