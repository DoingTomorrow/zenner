// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.TechnicalParameters.TechnicalParameterMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.TechnicalParameters;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.TechnicalParameters
{
  public sealed class TechnicalParameterMap : ClassMap<TechnicalParameter>
  {
    public TechnicalParameterMap()
    {
      this.Table("t_TechnicalParameters");
      this.Id((Expression<Func<TechnicalParameter, object>>) (s => (object) s.Id)).GeneratedBy.Increment();
      this.Map((Expression<Func<TechnicalParameter, object>>) (s => s.CustomerNumber)).Nullable();
      this.Map((Expression<Func<TechnicalParameter, object>>) (s => (object) s.LicenseWebApiTimeout)).Not.Nullable();
      this.Map((Expression<Func<TechnicalParameter, object>>) (s => (object) s.ServerOpenConnectionTimeout)).Not.Nullable();
      this.Map((Expression<Func<TechnicalParameter, object>>) (s => (object) s.LastConnectionToLicenseServer)).Nullable();
      this.Map((Expression<Func<TechnicalParameter, object>>) (s => (object) s.LastLicenseUniqueId)).Nullable();
      this.Map((Expression<Func<TechnicalParameter, object>>) (s => (object) s.DaysNotifyUserAboutLicenseExpire)).Not.Nullable();
      this.Map((Expression<Func<TechnicalParameter, object>>) (s => (object) s.IntervalNotifyUserAboutLicenseExpire)).Not.Nullable();
      this.Map((Expression<Func<TechnicalParameter, object>>) (s => (object) s.LastReminderForLicenseExpire)).Nullable();
    }
  }
}
