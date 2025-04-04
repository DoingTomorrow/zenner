// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.VersionMapper
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
  public class VersionMapper : IVersionMapper, IAccessorPropertyMapper, IColumnsMapper
  {
    private readonly IAccessorPropertyMapper entityPropertyMapper;
    private readonly MemberInfo member;
    private readonly HbmVersion versionMapping;

    public VersionMapper(MemberInfo member, HbmVersion hbmVersion)
    {
      if (member == null)
        throw new ArgumentNullException(nameof (member));
      if (hbmVersion == null)
        throw new ArgumentNullException(nameof (hbmVersion));
      this.member = member;
      this.versionMapping = hbmVersion;
      this.versionMapping.name = member.Name;
      this.entityPropertyMapper = (IAccessorPropertyMapper) new AccessorPropertyMapper(member.DeclaringType, member.Name, (Action<string>) (x => this.versionMapping.access = x));
    }

    public void Column(Action<IColumnMapper> columnMapper)
    {
      HbmColumn hbmColumn1 = this.versionMapping.Columns.Count<HbmColumn>() <= 1 ? this.versionMapping.Columns.SingleOrDefault<HbmColumn>() : throw new MappingException("Multi-columns property can't be mapped through single-column API.");
      if (hbmColumn1 == null)
        hbmColumn1 = new HbmColumn()
        {
          name = this.versionMapping.column1
        };
      HbmColumn hbmColumn2 = hbmColumn1;
      string name = this.member.Name;
      columnMapper((IColumnMapper) new ColumnMapper(hbmColumn2, this.member != null ? name : "unnamedcolumn"));
      if (this.ColumnTagIsRequired(hbmColumn2))
      {
        this.versionMapping.column = new HbmColumn[1]
        {
          hbmColumn2
        };
        this.ResetColumnPlainValues();
      }
      else
        this.versionMapping.column1 = name == null || !name.Equals(hbmColumn2.name) ? hbmColumn2.name : (string) null;
    }

    public void Columns(params Action<IColumnMapper>[] columnMapper)
    {
      this.versionMapping.column1 = (string) null;
      int num = 1;
      List<HbmColumn> hbmColumnList = new List<HbmColumn>(columnMapper.Length);
      foreach (Action<IColumnMapper> action in columnMapper)
      {
        HbmColumn mapping = new HbmColumn();
        string memberName = (this.member != null ? (object) this.member.Name : (object) "unnamedcolumn").ToString() + (object) num++;
        action((IColumnMapper) new ColumnMapper(mapping, memberName));
        hbmColumnList.Add(mapping);
      }
      this.versionMapping.column = hbmColumnList.ToArray();
    }

    public void Column(string name) => this.Column((Action<IColumnMapper>) (x => x.Name(name)));

    private bool ColumnTagIsRequired(HbmColumn hbm)
    {
      return hbm.length != null || hbm.precision != null || hbm.scale != null || hbm.notnull || hbm.unique || hbm.uniquekey != null || hbm.sqltype != null || hbm.index != null || hbm.@default != null || hbm.check != null;
    }

    private void ResetColumnPlainValues() => this.versionMapping.column1 = (string) null;

    public void Type(IVersionType persistentType)
    {
      if (persistentType == null)
        return;
      this.versionMapping.type = persistentType.Name;
    }

    public void Type<TPersistentType>() where TPersistentType : IUserVersionType
    {
      this.Type(typeof (TPersistentType));
    }

    public void Type(System.Type persistentType)
    {
      if (persistentType == null)
        throw new ArgumentNullException(nameof (persistentType));
      this.versionMapping.type = typeof (IUserVersionType).IsAssignableFrom(persistentType) ? persistentType.AssemblyQualifiedName : throw new ArgumentOutOfRangeException(nameof (persistentType), "Expected type implementing IUserVersionType");
    }

    public void UnsavedValue(object value)
    {
      this.versionMapping.unsavedvalue = value != null ? value.ToString() : "null";
    }

    public void Insert(bool useInInsert)
    {
      this.versionMapping.insert = useInInsert;
      this.versionMapping.insertSpecified = !useInInsert;
    }

    public void Generated(VersionGeneration generatedByDb)
    {
      if (generatedByDb == null)
        return;
      this.versionMapping.generated = generatedByDb.ToHbm();
    }

    public void Access(Accessor accessor) => this.entityPropertyMapper.Access(accessor);

    public void Access(System.Type accessorType) => this.entityPropertyMapper.Access(accessorType);
  }
}
