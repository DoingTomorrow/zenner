// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.ApplicationParameters.ApplicationParametersMapping
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.ApplicationParamenters;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.ApplicationParameters
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
