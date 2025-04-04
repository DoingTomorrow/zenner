// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.TechnicalParameters.TechnicalParameterMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.TechnicalParameters;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.TechnicalParameters
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
