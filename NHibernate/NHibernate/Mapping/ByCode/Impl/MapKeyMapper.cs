// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.MapKeyMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Type;
using NHibernate.UserTypes;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class MapKeyMapper : IMapKeyMapper, IColumnsMapper
  {
    private readonly HbmMapKey hbmMapKey;

    public MapKeyMapper(HbmMapKey hbmMapKey) => this.hbmMapKey = hbmMapKey;

    public HbmMapKey MapKeyMapping => this.hbmMapKey;

    public void Column(Action<IColumnMapper> columnMapper)
    {
      if (this.hbmMapKey.Columns.Count<HbmColumn>() > 1)
        throw new MappingException("Multi-columns property can't be mapped through singlr-column API.");
      this.hbmMapKey.formula = (string) null;
      HbmColumn hbmColumn1 = this.hbmMapKey.Columns.SingleOrDefault<HbmColumn>();
      if (hbmColumn1 == null)
        hbmColumn1 = new HbmColumn()
        {
          name = this.hbmMapKey.column,
          length = this.hbmMapKey.length
        };
      HbmColumn hbmColumn2 = hbmColumn1;
      string memberName = "mapKey";
      columnMapper((IColumnMapper) new ColumnMapper(hbmColumn2, memberName));
      if (this.ColumnTagIsRequired(hbmColumn2))
      {
        this.hbmMapKey.Items = (object[]) new HbmColumn[1]
        {
          hbmColumn2
        };
        this.ResetColumnPlainValues();
      }
      else
      {
        this.hbmMapKey.column = !memberName.Equals(hbmColumn2.name) ? hbmColumn2.name : (string) null;
        this.hbmMapKey.length = hbmColumn2.length;
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
        string memberName = "mapKey" + (object) num++;
        action((IColumnMapper) new ColumnMapper(mapping, memberName));
        hbmColumnList.Add(mapping);
      }
      this.hbmMapKey.Items = (object[]) hbmColumnList.ToArray();
    }

    public void Column(string name) => this.Column((Action<IColumnMapper>) (x => x.Name(name)));

    public void Type(IType persistentType)
    {
      if (persistentType == null)
        return;
      this.hbmMapKey.type = persistentType.Name;
    }

    public void Type<TPersistentType>() => this.Type(typeof (TPersistentType));

    public void Type(System.Type persistentType)
    {
      if (persistentType == null)
        throw new ArgumentNullException(nameof (persistentType));
      this.hbmMapKey.type = typeof (IUserType).IsAssignableFrom(persistentType) || typeof (IType).IsAssignableFrom(persistentType) ? persistentType.AssemblyQualifiedName : throw new ArgumentOutOfRangeException(nameof (persistentType), "Expected type implementing IUserType or IType.");
    }

    public void Length(int length) => this.Column((Action<IColumnMapper>) (x => x.Length(length)));

    public void Formula(string formula)
    {
      if (formula == null)
        return;
      this.ResetColumnPlainValues();
      this.hbmMapKey.Items = (object[]) null;
      string[] strArray = formula.Split(new string[1]
      {
        Environment.NewLine
      }, StringSplitOptions.None);
      if (strArray.Length > 1)
        this.hbmMapKey.Items = (object[]) new HbmFormula[1]
        {
          new HbmFormula() { Text = strArray }
        };
      else
        this.hbmMapKey.formula = formula;
    }

    private void ResetColumnPlainValues()
    {
      this.hbmMapKey.column = (string) null;
      this.hbmMapKey.length = (string) null;
      this.hbmMapKey.formula = (string) null;
    }

    private bool ColumnTagIsRequired(HbmColumn hbm)
    {
      return hbm.precision != null || hbm.scale != null || hbm.notnull || hbm.unique || hbm.uniquekey != null || hbm.sqltype != null || hbm.index != null || hbm.@default != null || hbm.check != null;
    }
  }
}
