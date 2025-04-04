// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.ApplicationParameters.ApplicationParametersMapping
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.ApplicationParamenters;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.ApplicationParameters
{
  public class ApplicationParametersMapping
  {
    public class ApplicationParameterMap : ClassMap<ApplicationParameter>
    {
      public ApplicationParameterMap()
      {
        this.Table("t_ApplicationParameters");
        this.Id((Expression<Func<ApplicationParameter, object>>) (appParam => (object) appParam.Id)).Column("Id").GeneratedBy.Identity();
        this.Map((Expression<Func<ApplicationParameter, object>>) (appParam => appParam.Parameter)).Length(50).Not.Nullable();
        this.Map((Expression<Func<ApplicationParameter, object>>) (appParam => appParam.Value)).Length(8000).Nullable();
        this.Map((Expression<Func<ApplicationParameter, object>>) (appParam => appParam.Label)).Length(200).Not.Nullable();
        this.Map((Expression<Func<ApplicationParameter, object>>) (appParam => appParam.Category)).Length(100).Not.Nullable();
        this.Map((Expression<Func<ApplicationParameter, object>>) (appParam => appParam.Description)).Length(1000).Nullable();
        this.Map((Expression<Func<ApplicationParameter, object>>) (appParam => (object) appParam.ViewObjectType)).Length(50).Not.Nullable();
        this.Map((Expression<Func<ApplicationParameter, object>>) (appParam => (object) appParam.DataType)).Length(20).Not.Nullable();
        this.Map((Expression<Func<ApplicationParameter, object>>) (appParam => appParam.Scope)).Length(20).Not.Nullable();
        this.Map((Expression<Func<ApplicationParameter, object>>) (appParam => (object) appParam.OrderInCategory)).Not.Nullable();
        this.Map((Expression<Func<ApplicationParameter, object>>) (appParam => (object) appParam.OrderInPage)).Not.Nullable();
      }
    }
  }
}
