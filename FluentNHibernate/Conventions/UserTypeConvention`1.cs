// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.UserTypeConvention`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using NHibernate.UserTypes;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions
{
  public abstract class UserTypeConvention<TUserType> : 
    IUserTypeConvention,
    IPropertyConventionAcceptance,
    IConventionAcceptance<IPropertyInspector>,
    IPropertyConvention,
    IConvention<IPropertyInspector, IPropertyInstance>,
    IConvention
    where TUserType : IUserType, new()
  {
    public virtual void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
    {
      TUserType userType = new TUserType();
      criteria.Expect((Expression<Func<IPropertyInspector, bool>>) (x => x.Type == userType.ReturnedType));
    }

    public virtual void Apply(IPropertyInstance instance) => instance.CustomType<TUserType>();
  }
}
