// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.MapKeyManyToManyMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class MapKeyManyToManyMapper : IMapKeyManyToManyMapper, IColumnsMapper
  {
    private const string DefaultColumnName = "mapKeyRelation";
    private readonly HbmMapKeyManyToMany mapping;

    public MapKeyManyToManyMapper(HbmMapKeyManyToMany mapping) => this.mapping = mapping;

    public HbmMapKeyManyToMany MapKeyManyToManyMapping => this.mapping;

    public void ForeignKey(string foreignKeyName) => this.mapping.foreignkey = foreignKeyName;

    public void Formula(string formula)
    {
      if (formula == null)
        return;
      this.ResetColumnPlainValues();
      this.mapping.Items = (object[]) null;
      string[] strArray = formula.Split(new string[1]
      {
        Environment.NewLine
      }, StringSplitOptions.None);
      if (strArray.Length > 1)
        this.mapping.Items = (object[]) new HbmFormula[1]
        {
          new HbmFormula() { Text = strArray }
        };
      else
        this.mapping.formula = formula;
    }

    public void Column(Action<IColumnMapper> columnMapper)
    {
      if (this.mapping.Columns.Count<HbmColumn>() > 1)
        throw new MappingException("Multi-columns property can't be mapped through singlr-column API.");
      this.mapping.formula = (string) null;
      HbmColumn hbmColumn1 = this.mapping.Columns.SingleOrDefault<HbmColumn>();
      if (hbmColumn1 == null)
        hbmColumn1 = new HbmColumn()
        {
          name = this.mapping.column
        };
      HbmColumn hbmColumn2 = hbmColumn1;
      columnMapper((IColumnMapper) new ColumnMapper(hbmColumn2, "mapKeyRelation"));
      if (this.ColumnTagIsRequired(hbmColumn2))
      {
        this.mapping.Items = (object[]) new HbmColumn[1]
        {
          hbmColumn2
        };
        this.ResetColumnPlainValues();
      }
      else
        this.mapping.column = !"mapKeyRelation".Equals(hbmColumn2.name) ? hbmColumn2.name : (string) null;
    }

    public void Columns(params Action<IColumnMapper>[] columnMapper)
    {
      this.mapping.column = (string) null;
      int num = 1;
      List<HbmColumn> hbmColumnList = new List<HbmColumn>(columnMapper.Length);
      foreach (Action<IColumnMapper> action in columnMapper)
      {
        HbmColumn mapping = new HbmColumn();
        string memberName = "mapKeyRelation" + (object) num++;
        action((IColumnMapper) new ColumnMapper(mapping, memberName));
        hbmColumnList.Add(mapping);
      }
      this.mapping.Items = (object[]) hbmColumnList.ToArray();
    }

    public void Column(string name) => this.Column((Action<IColumnMapper>) (cm => cm.Name(name)));

    private bool ColumnTagIsRequired(HbmColumn hbm)
    {
      return hbm.length != null || hbm.precision != null || hbm.scale != null || hbm.notnull || hbm.unique || hbm.uniquekey != null || hbm.sqltype != null || hbm.index != null || hbm.@default != null || hbm.check != null;
    }

    private void ResetColumnPlainValues()
    {
      this.mapping.column = (string) null;
      this.mapping.formula = (string) null;
    }
  }
}
