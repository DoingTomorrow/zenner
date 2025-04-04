// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.DiscriminatorMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Type;
using System;
using System.Linq;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class DiscriminatorMapper : IDiscriminatorMapper
  {
    private readonly HbmDiscriminator discriminatorMapping;

    public DiscriminatorMapper(HbmDiscriminator discriminatorMapping)
    {
      this.discriminatorMapping = discriminatorMapping != null ? discriminatorMapping : throw new ArgumentNullException(nameof (discriminatorMapping));
    }

    public void Column(string column)
    {
      this.Column((Action<IColumnMapper>) (cm => cm.Name(column)));
    }

    public void Column(Action<IColumnMapper> columnMapper)
    {
      this.discriminatorMapping.formula = (string) null;
      HbmColumn hbmColumn1 = this.discriminatorMapping.Columns.SingleOrDefault<HbmColumn>();
      if (hbmColumn1 == null)
        hbmColumn1 = new HbmColumn()
        {
          name = this.discriminatorMapping.column,
          length = this.discriminatorMapping.length,
          notnull = this.discriminatorMapping.notnull,
          notnullSpecified = this.discriminatorMapping.notnull
        };
      HbmColumn hbmColumn2 = hbmColumn1;
      columnMapper((IColumnMapper) new ColumnMapper(hbmColumn2, "class"));
      if (this.ColumnTagIsRequired(hbmColumn2))
      {
        this.discriminatorMapping.Item = (object) hbmColumn2;
        this.ResetColumnPlainValues();
      }
      else
      {
        this.discriminatorMapping.column = !"class".Equals(hbmColumn2.name) ? hbmColumn2.name : (string) null;
        this.discriminatorMapping.length = hbmColumn2.length;
        this.discriminatorMapping.notnull = hbmColumn2.notnull;
        this.discriminatorMapping.Item = (object) null;
      }
    }

    public void Type(IType persistentType)
    {
      if (persistentType == null)
        return;
      this.discriminatorMapping.type = persistentType.Name;
    }

    public void Type(IDiscriminatorType persistentType) => this.Type(persistentType.GetType());

    public void Type<TPersistentType>() where TPersistentType : IDiscriminatorType
    {
      this.Type(typeof (TPersistentType));
    }

    public void Type(System.Type persistentType)
    {
      if (persistentType == null)
        throw new ArgumentNullException(nameof (persistentType));
      this.discriminatorMapping.type = typeof (IDiscriminatorType).IsAssignableFrom(persistentType) ? persistentType.AssemblyQualifiedName : throw new ArgumentOutOfRangeException(nameof (persistentType), "Expected type implementing IDiscriminatorType");
    }

    public void Formula(string formula)
    {
      if (formula == null)
        return;
      this.ResetColumnPlainValues();
      this.discriminatorMapping.Item = (object) null;
      string[] strArray = formula.Split(new string[1]
      {
        Environment.NewLine
      }, StringSplitOptions.None);
      if (strArray.Length > 1)
        this.discriminatorMapping.Item = (object) new HbmFormula()
        {
          Text = strArray
        };
      else
        this.discriminatorMapping.formula = formula;
    }

    public void Force(bool force) => this.discriminatorMapping.force = force;

    public void Insert(bool applyOnInsert) => this.discriminatorMapping.insert = applyOnInsert;

    public void NotNullable(bool isNotNullable)
    {
      this.discriminatorMapping.notnull = isNotNullable;
    }

    public void Length(int length) => this.Column((Action<IColumnMapper>) (x => x.Length(length)));

    private bool ColumnTagIsRequired(HbmColumn hbm)
    {
      return hbm.sqltype != null || hbm.@default != null || hbm.check != null || hbm.comment != null || hbm.index != null || hbm.precision != null || hbm.scale != null || hbm.unique || hbm.uniquekey != null;
    }

    private void ResetColumnPlainValues()
    {
      this.discriminatorMapping.column = (string) null;
      this.discriminatorMapping.length = (string) null;
      this.discriminatorMapping.notnull = true;
      this.discriminatorMapping.formula = (string) null;
    }
  }
}
