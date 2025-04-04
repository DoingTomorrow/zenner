// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.RadioTest.RadioTestRunDeviceMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.RadioTest;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.RadioTest
{
  public class RadioTestRunDeviceMap : ClassMap<RadioTestRunDevice>
  {
    public RadioTestRunDeviceMap()
    {
      this.Table("t_RadioTestRunDevice");
      this.Id((Expression<Func<RadioTestRunDevice, object>>) (rtd => (object) rtd.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<RadioTestRunDevice, object>>) (rtd => rtd.TgKonfig));
      this.Map((Expression<Func<RadioTestRunDevice, object>>) (rtd => (object) rtd.TgNumber));
      this.Map((Expression<Func<RadioTestRunDevice, object>>) (rtd => rtd.TgRadioId));
      this.Map((Expression<Func<RadioTestRunDevice, object>>) (rtd => rtd.TgLastRssi)).Nullable();
      this.Map((Expression<Func<RadioTestRunDevice, object>>) (rtd => (object) rtd.TgAverage)).Nullable();
      this.Map((Expression<Func<RadioTestRunDevice, object>>) (rtd => rtd.TgFabrikat)).Nullable();
      this.References<RadioTestRun>((Expression<Func<RadioTestRunDevice, RadioTestRun>>) (m => m.RadioTestRun), "RadioTestRunId");
      this.Map((Expression<Func<RadioTestRunDevice, object>>) (rtd => (object) rtd.LastRadioTestRunDeviceMDMExportOn));
      this.Map((Expression<Func<RadioTestRunDevice, object>>) (rtd => (object) rtd.UpdateDate));
    }
  }
}
