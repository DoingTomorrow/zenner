// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.RadioTest.RadioTestRunMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.RadioTest;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.RadioTest
{
  public class RadioTestRunMap : ClassMap<RadioTestRun>
  {
    public RadioTestRunMap()
    {
      this.Table("t_RadioTestRun");
      this.Id((Expression<Func<RadioTestRun, object>>) (rtd => (object) rtd.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<RadioTestRun, object>>) (rt => rt.TrKonfig), "TrKonfig").Not.Nullable();
      this.Map((Expression<Func<RadioTestRun, object>>) (rt => (object) rt.TrNumber), "TrNumber").Not.Nullable();
      this.Map((Expression<Func<RadioTestRun, object>>) (rt => rt.TrUserLocation)).Length(30).Nullable();
      this.Map((Expression<Func<RadioTestRun, object>>) (rt => rt.TrDeviceLocation)).Length(30).Nullable();
      this.Map((Expression<Func<RadioTestRun, object>>) (rt => rt.TrComment)).Length(30).Nullable();
      this.Map((Expression<Func<RadioTestRun, object>>) (rt => (object) rt.TrBestChoice)).Nullable();
      this.Map((Expression<Func<RadioTestRun, object>>) (rt => rt.TrType)).Not.Nullable();
      this.References<TestOrder>((Expression<Func<RadioTestRun, TestOrder>>) (m => m.TestOrder), "TestOrderId");
      this.HasMany<RadioTestRunDevice>((Expression<Func<RadioTestRun, IEnumerable<RadioTestRunDevice>>>) (x => x.RadioTestRunDevices)).KeyColumn("RadioTestRunId").Cascade.AllDeleteOrphan().Inverse();
      this.Map((Expression<Func<RadioTestRun, object>>) (rt => (object) rt.LastRadioTestRunMDMExportOn));
      this.Map((Expression<Func<RadioTestRun, object>>) (rt => (object) rt.UpdateDate));
    }
  }
}
