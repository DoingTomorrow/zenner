// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.XmlHbmBinding.ValuePropertyBinder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Cfg.XmlHbmBinding
{
  public class ValuePropertyBinder : Binder
  {
    private readonly SimpleValue value;

    public ValuePropertyBinder(SimpleValue value, Mappings mappings)
      : base(mappings)
    {
      this.value = value != null ? value : throw new ArgumentNullException(nameof (value));
    }

    public void BindSimpleValue(HbmProperty propertyMapping, string propertyPath, bool isNullable)
    {
      new TypeBinder(this.value, this.Mappings).Bind(propertyMapping.Type);
      HbmFormula[] array = propertyMapping.Formulas.ToArray<HbmFormula>();
      if (array.Length > 0)
        this.BindFormulas((IEnumerable<HbmFormula>) array);
      else
        new ColumnsBinder(this.value, this.Mappings).Bind(propertyMapping.Columns, isNullable, (Func<HbmColumn>) (() => new HbmColumn()
        {
          name = this.mappings.NamingStrategy.PropertyToColumnName(propertyPath),
          length = propertyMapping.length,
          scale = propertyMapping.scale,
          precision = propertyMapping.precision,
          notnull = propertyMapping.notnull,
          notnullSpecified = propertyMapping.notnullSpecified,
          unique = propertyMapping.unique,
          uniqueSpecified = true,
          uniquekey = propertyMapping.uniquekey,
          index = propertyMapping.index
        }));
    }

    public void BindSimpleValue(HbmElement element, string propertyPath, bool isNullable)
    {
      new TypeBinder(this.value, this.Mappings).Bind(element.Type);
      HbmFormula[] array = element.Formulas.ToArray<HbmFormula>();
      if (array.Length > 0)
        this.BindFormulas((IEnumerable<HbmFormula>) array);
      else
        new ColumnsBinder(this.value, this.Mappings).Bind(element.Columns, isNullable, (Func<HbmColumn>) (() => new HbmColumn()
        {
          name = this.mappings.NamingStrategy.PropertyToColumnName(propertyPath),
          length = element.length,
          scale = element.scale,
          precision = element.precision,
          notnull = element.notnull,
          notnullSpecified = true,
          unique = element.unique,
          uniqueSpecified = true
        }));
    }

    public void BindSimpleValue(HbmKey propertyMapping, string propertyPath, bool isNullable)
    {
      new ColumnsBinder(this.value, this.Mappings).Bind(propertyMapping.Columns, isNullable, (Func<HbmColumn>) (() => new HbmColumn()
      {
        name = this.mappings.NamingStrategy.PropertyToColumnName(propertyPath),
        notnull = propertyMapping.notnull,
        notnullSpecified = propertyMapping.notnullSpecified,
        unique = propertyMapping.unique,
        uniqueSpecified = propertyMapping.uniqueSpecified
      }));
    }

    public void BindSimpleValue(
      HbmManyToMany manyToManyMapping,
      string propertyPath,
      bool isNullable)
    {
      HbmFormula[] array = manyToManyMapping.Formulas.ToArray<HbmFormula>();
      if (array.Length > 0)
        this.BindFormulas((IEnumerable<HbmFormula>) array);
      else
        new ColumnsBinder(this.value, this.Mappings).Bind(manyToManyMapping.Columns, isNullable, (Func<HbmColumn>) (() => new HbmColumn()
        {
          name = this.mappings.NamingStrategy.PropertyToColumnName(propertyPath),
          unique = manyToManyMapping.unique,
          uniqueSpecified = true
        }));
    }

    public void BindSimpleValue(HbmCollectionId collectionIdMapping, string propertyPath)
    {
      new TypeBinder(this.value, this.Mappings).Bind(collectionIdMapping.Type);
      new ColumnsBinder(this.value, this.Mappings).Bind(collectionIdMapping.Columns, false, (Func<HbmColumn>) (() => new HbmColumn()
      {
        name = this.mappings.NamingStrategy.PropertyToColumnName(propertyPath),
        length = collectionIdMapping.length
      }));
    }

    public void BindSimpleValue(
      HbmListIndex listIndexMapping,
      string propertyPath,
      bool isNullable)
    {
      new TypeBinder(this.value, this.Mappings).Bind(NHibernateUtil.Int32.Name);
      new ColumnsBinder(this.value, this.Mappings).Bind(listIndexMapping.Columns, isNullable, (Func<HbmColumn>) (() => new HbmColumn()
      {
        name = this.mappings.NamingStrategy.PropertyToColumnName(propertyPath)
      }));
    }

    public void BindSimpleValue(HbmIndex indexMapping, string propertyPath, bool isNullable)
    {
      new TypeBinder(this.value, this.Mappings).Bind(indexMapping.Type);
      new ColumnsBinder(this.value, this.Mappings).Bind(indexMapping.Columns, isNullable, (Func<HbmColumn>) (() => new HbmColumn()
      {
        name = this.mappings.NamingStrategy.PropertyToColumnName(propertyPath),
        length = indexMapping.length
      }));
    }

    public void BindSimpleValue(HbmMapKey mapKeyMapping, string propertyPath, bool isNullable)
    {
      new TypeBinder(this.value, this.Mappings).Bind(mapKeyMapping.Type);
      HbmFormula[] array = mapKeyMapping.Formulas.ToArray<HbmFormula>();
      if (array.Length > 0)
        this.BindFormulas((IEnumerable<HbmFormula>) array);
      else
        new ColumnsBinder(this.value, this.Mappings).Bind(mapKeyMapping.Columns, isNullable, (Func<HbmColumn>) (() => new HbmColumn()
        {
          name = this.mappings.NamingStrategy.PropertyToColumnName(propertyPath),
          length = mapKeyMapping.length
        }));
    }

    public void BindSimpleValue(
      HbmManyToOne manyToOneMapping,
      string propertyPath,
      bool isNullable)
    {
      ColumnsBinder binder = new ColumnsBinder(this.value, this.Mappings);
      object[] array = manyToOneMapping.ColumnsAndFormulas.ToArray<object>();
      if (array.Length > 0)
        this.AddColumnsAndOrFormulas(binder, array, isNullable);
      else
        binder.Bind(new HbmColumn()
        {
          name = this.mappings.NamingStrategy.PropertyToColumnName(propertyPath),
          notnull = manyToOneMapping.notnull,
          notnullSpecified = manyToOneMapping.notnullSpecified,
          unique = manyToOneMapping.unique,
          uniqueSpecified = true,
          uniquekey = manyToOneMapping.uniquekey,
          index = manyToOneMapping.index
        }, (isNullable ? 1 : 0) != 0);
    }

    public void BindSimpleValue(
      HbmIndexManyToMany indexManyToManyMapping,
      string propertyPath,
      bool isNullable)
    {
      new ColumnsBinder(this.value, this.Mappings).Bind(indexManyToManyMapping.Columns, isNullable, (Func<HbmColumn>) (() => new HbmColumn()
      {
        name = this.mappings.NamingStrategy.PropertyToColumnName(propertyPath)
      }));
    }

    public void BindSimpleValue(
      HbmMapKeyManyToMany mapKeyManyToManyMapping,
      string propertyPath,
      bool isNullable)
    {
      HbmFormula[] array = mapKeyManyToManyMapping.Formulas.ToArray<HbmFormula>();
      if (array.Length > 0)
        this.BindFormulas((IEnumerable<HbmFormula>) array);
      else
        new ColumnsBinder(this.value, this.Mappings).Bind(mapKeyManyToManyMapping.Columns, isNullable, (Func<HbmColumn>) (() => new HbmColumn()
        {
          name = this.mappings.NamingStrategy.PropertyToColumnName(propertyPath)
        }));
    }

    public void BindSimpleValue(
      HbmKeyProperty mapKeyManyToManyMapping,
      string propertyPath,
      bool isNullable)
    {
      new TypeBinder(this.value, this.Mappings).Bind(mapKeyManyToManyMapping.Type);
      new ColumnsBinder(this.value, this.Mappings).Bind(mapKeyManyToManyMapping.Columns, isNullable, (Func<HbmColumn>) (() => new HbmColumn()
      {
        name = this.mappings.NamingStrategy.PropertyToColumnName(propertyPath),
        length = mapKeyManyToManyMapping.length
      }));
    }

    public void BindSimpleValue(
      HbmKeyManyToOne mapKeyManyToManyMapping,
      string propertyPath,
      bool isNullable)
    {
      new ColumnsBinder(this.value, this.Mappings).Bind(mapKeyManyToManyMapping.Columns, isNullable, (Func<HbmColumn>) (() => new HbmColumn()
      {
        name = this.mappings.NamingStrategy.PropertyToColumnName(propertyPath)
      }));
    }

    private void AddColumnsAndOrFormulas(
      ColumnsBinder binder,
      object[] columnsAndFormulas,
      bool isNullable)
    {
      foreach (object columnsAndFormula in columnsAndFormulas)
      {
        if (columnsAndFormula.GetType() == typeof (HbmFormula))
          this.BindFormula((HbmFormula) columnsAndFormula);
        if (columnsAndFormula.GetType() == typeof (HbmColumn))
          binder.Bind((HbmColumn) columnsAndFormula, isNullable);
      }
    }

    private void BindFormula(HbmFormula formula)
    {
      this.value.AddFormula(new Formula()
      {
        FormulaString = formula.Text.LinesToString()
      });
    }

    private void BindFormulas(IEnumerable<HbmFormula> formulas)
    {
      foreach (HbmFormula formula in formulas)
        this.BindFormula(formula);
    }
  }
}
