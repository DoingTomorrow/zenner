// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.OneToOneMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using System;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class OneToOneMapper : IOneToOneMapper, IEntityPropertyMapper, IAccessorPropertyMapper
  {
    private readonly IAccessorPropertyMapper _entityPropertyMapper;
    private readonly MemberInfo _member;
    private readonly HbmOneToOne _oneToOne;

    public OneToOneMapper(MemberInfo member, HbmOneToOne oneToOne)
      : this(member, member == null ? (IAccessorPropertyMapper) new NoMemberPropertyMapper() : (IAccessorPropertyMapper) new AccessorPropertyMapper(member.DeclaringType, member.Name, (Action<string>) (x => oneToOne.access = x)), oneToOne)
    {
    }

    public OneToOneMapper(
      MemberInfo member,
      IAccessorPropertyMapper accessorMapper,
      HbmOneToOne oneToOne)
    {
      this._member = member;
      this._oneToOne = oneToOne;
      if (member == null)
        this._oneToOne.access = "none";
      this._entityPropertyMapper = member == null ? (IAccessorPropertyMapper) new NoMemberPropertyMapper() : accessorMapper;
    }

    public void Cascade(NHibernate.Mapping.ByCode.Cascade cascadeStyle)
    {
      this._oneToOne.cascade = cascadeStyle.Exclude(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans).ToCascadeString();
    }

    public void Access(Accessor accessor) => this._entityPropertyMapper.Access(accessor);

    public void Access(Type accessorType) => this._entityPropertyMapper.Access(accessorType);

    public void OptimisticLock(bool takeInConsiderationForOptimisticLock)
    {
    }

    public void Lazy(LazyRelation lazyRelation)
    {
      this._oneToOne.lazy = lazyRelation.ToHbm();
      this._oneToOne.lazySpecified = this._oneToOne.lazy != HbmLaziness.Proxy;
    }

    public void Constrained(bool value) => this._oneToOne.constrained = value;

    public void PropertyReference(MemberInfo propertyInTheOtherSide)
    {
      if (propertyInTheOtherSide == null)
      {
        this._oneToOne.propertyref = (string) null;
      }
      else
      {
        if (this._member != null && propertyInTheOtherSide.DeclaringType != this._member.GetPropertyOrFieldType())
          throw new ArgumentOutOfRangeException(nameof (propertyInTheOtherSide), string.Format("Expected a member of {0} found the member {1} of {2}", (object) this._member.GetPropertyOrFieldType(), (object) propertyInTheOtherSide, (object) propertyInTheOtherSide.DeclaringType));
        this._oneToOne.propertyref = propertyInTheOtherSide.Name;
      }
    }

    public void Formula(string formula)
    {
      if (formula == null)
      {
        this._oneToOne.formula = (HbmFormula[]) null;
        this._oneToOne.formula1 = (string) null;
      }
      else
      {
        string[] strArray = formula.Split(new string[1]
        {
          Environment.NewLine
        }, StringSplitOptions.None);
        if (strArray.Length > 1)
        {
          this._oneToOne.formula = new HbmFormula[1]
          {
            new HbmFormula() { Text = strArray }
          };
          this._oneToOne.formula1 = (string) null;
        }
        else
        {
          this._oneToOne.formula1 = formula;
          this._oneToOne.formula = (HbmFormula[]) null;
        }
      }
    }

    public void ForeignKey(string foreignKeyName) => this._oneToOne.foreignkey = foreignKeyName;
  }
}
