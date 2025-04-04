// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.ElementMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Type;
using NHibernate.UserTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class ElementMapper : IElementMapper, IColumnsMapper
  {
    private const string DefaultColumnName = "element";
    private readonly HbmElement elementMapping;
    private readonly System.Type elementType;

    public ElementMapper(System.Type elementType, HbmElement elementMapping)
    {
      if (elementType == null)
        throw new ArgumentNullException(nameof (elementType));
      if (elementMapping == null)
        throw new ArgumentNullException(nameof (elementMapping));
      this.elementType = elementType;
      this.elementMapping = elementMapping;
      this.elementMapping.type1 = elementType.GetNhTypeName();
    }

    public void Column(Action<IColumnMapper> columnMapper)
    {
      if (this.elementMapping.Columns.Count<HbmColumn>() > 1)
        throw new MappingException("Multi-columns property can't be mapped through singlr-column API.");
      this.elementMapping.formula = (string) null;
      HbmColumn hbmColumn1 = this.elementMapping.Columns.SingleOrDefault<HbmColumn>();
      if (hbmColumn1 == null)
        hbmColumn1 = new HbmColumn()
        {
          name = this.elementMapping.column,
          length = this.elementMapping.length,
          scale = this.elementMapping.scale,
          precision = this.elementMapping.precision,
          notnull = this.elementMapping.notnull,
          unique = this.elementMapping.unique,
          uniqueSpecified = this.elementMapping.unique
        };
      HbmColumn hbmColumn2 = hbmColumn1;
      columnMapper((IColumnMapper) new ColumnMapper(hbmColumn2, "element"));
      if (this.ColumnTagIsRequired(hbmColumn2))
      {
        this.elementMapping.Items = (object[]) new HbmColumn[1]
        {
          hbmColumn2
        };
        this.ResetColumnPlainValues();
      }
      else
      {
        this.elementMapping.column = !"element".Equals(hbmColumn2.name) ? hbmColumn2.name : (string) null;
        this.elementMapping.length = hbmColumn2.length;
        this.elementMapping.precision = hbmColumn2.precision;
        this.elementMapping.scale = hbmColumn2.scale;
        this.elementMapping.notnull = hbmColumn2.notnull;
        this.elementMapping.unique = hbmColumn2.unique;
      }
    }

    public void Columns(params Action<IColumnMapper>[] columnMapper)
    {
      this.ResetColumnPlainValues();
      int num = 1;
      List<HbmColumn> hbmColumnList = new List<HbmColumn>(columnMapper.Length);
      foreach (Action<IColumnMapper> action in columnMapper)
      {
        HbmColumn mapping = new HbmColumn();
        string memberName = "element" + (object) num++;
        action((IColumnMapper) new ColumnMapper(mapping, memberName));
        hbmColumnList.Add(mapping);
      }
      this.elementMapping.Items = (object[]) hbmColumnList.ToArray();
    }

    public void Column(string name) => this.Column((Action<IColumnMapper>) (x => x.Name(name)));

    private bool ColumnTagIsRequired(HbmColumn hbm)
    {
      return hbm.uniquekey != null || hbm.sqltype != null || hbm.index != null || hbm.@default != null || hbm.check != null;
    }

    private void ResetColumnPlainValues()
    {
      this.elementMapping.column = (string) null;
      this.elementMapping.length = (string) null;
      this.elementMapping.precision = (string) null;
      this.elementMapping.scale = (string) null;
      this.elementMapping.notnull = false;
      this.elementMapping.unique = false;
      this.elementMapping.formula = (string) null;
    }

    public void Type(IType persistentType)
    {
      if (persistentType == null)
        return;
      this.elementMapping.type1 = persistentType.Name;
      this.elementMapping.type = (HbmType) null;
    }

    public void Type<TPersistentType>() => this.Type(typeof (TPersistentType), (object) null);

    public void Type<TPersistentType>(object parameters)
    {
      this.Type(typeof (TPersistentType), parameters);
    }

    public void Type(System.Type persistentType, object parameters)
    {
      if (persistentType == null)
        throw new ArgumentNullException(nameof (persistentType));
      if (!typeof (IUserType).IsAssignableFrom(persistentType) && !typeof (IType).IsAssignableFrom(persistentType))
        throw new ArgumentOutOfRangeException(nameof (persistentType), "Expected type implementing IUserType or IType.");
      if (parameters != null)
      {
        this.elementMapping.type1 = (string) null;
        this.elementMapping.type = new HbmType()
        {
          name = persistentType.AssemblyQualifiedName,
          param = ((IEnumerable<PropertyInfo>) parameters.GetType().GetProperties()).Select(pi => new
          {
            pi = pi,
            pname = pi.Name
          }).Select(_param1 => new
          {
            \u003C\u003Eh__TransparentIdentifier4 = _param1,
            pvalue = _param1.pi.GetValue(parameters, (object[]) null)
          }).Select(_param0 => new HbmParam()
          {
            name = _param0.\u003C\u003Eh__TransparentIdentifier4.pname,
            Text = new string[1]
            {
              object.ReferenceEquals(_param0.pvalue, (object) null) ? "null" : _param0.pvalue.ToString()
            }
          }).ToArray<HbmParam>()
        };
      }
      else
      {
        this.elementMapping.type1 = persistentType.AssemblyQualifiedName;
        this.elementMapping.type = (HbmType) null;
      }
    }

    public void Length(int length) => this.Column((Action<IColumnMapper>) (x => x.Length(length)));

    public void Precision(short precision)
    {
      this.Column((Action<IColumnMapper>) (x => x.Precision(precision)));
    }

    public void Scale(short scale) => this.Column((Action<IColumnMapper>) (x => x.Scale(scale)));

    public void NotNullable(bool notnull)
    {
      this.Column((Action<IColumnMapper>) (x => x.NotNullable(notnull)));
    }

    public void Unique(bool unique) => this.Column((Action<IColumnMapper>) (x => x.Unique(unique)));

    public void Formula(string formula)
    {
      if (formula == null)
        return;
      this.ResetColumnPlainValues();
      this.elementMapping.Items = (object[]) null;
      string[] strArray = formula.Split(new string[1]
      {
        Environment.NewLine
      }, StringSplitOptions.None);
      if (strArray.Length > 1)
        this.elementMapping.Items = (object[]) new HbmFormula[1]
        {
          new HbmFormula() { Text = strArray }
        };
      else
        this.elementMapping.formula = formula;
    }
  }
}
