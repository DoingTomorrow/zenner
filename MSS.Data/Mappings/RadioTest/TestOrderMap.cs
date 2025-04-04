// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.RadioTest.TestOrderMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.RadioTest;
using MSS.Core.Model.Structures;
using MSS.Core.Model.UsersManagement;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.RadioTest
{
  public class TestOrderMap : ClassMap<TestOrder>
  {
    public TestOrderMap()
    {
      this.Table("t_TestOrder");
      this.Id((Expression<Func<TestOrder, object>>) (rtd => (object) rtd.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<TestOrder, object>>) (rt => rt.AuftrArt));
      this.Map((Expression<Func<TestOrder, object>>) (rt => rt.BuildingNumber));
      this.Map((Expression<Func<TestOrder, object>>) (rt => rt.IsChanged));
      this.Map((Expression<Func<TestOrder, object>>) (rt => rt.OrderNumber));
      this.Map((Expression<Func<TestOrder, object>>) (rt => rt.OrderTypeMss));
      this.Map((Expression<Func<TestOrder, object>>) (rt => rt.StatusType));
      this.Map((Expression<Func<TestOrder, object>>) (rt => rt.Generation));
      this.Map((Expression<Func<TestOrder, object>>) (rt => rt.Scenario));
      this.References<StructureNode>((Expression<Func<TestOrder, StructureNode>>) (m => m.StructureNode), "StructureNodeId");
      this.HasMany<RadioTestRun>((Expression<Func<TestOrder, IEnumerable<RadioTestRun>>>) (x => x.TestRunList)).KeyColumn("TestOrderId").Cascade.AllDeleteOrphan().Inverse();
      this.References<Country>((Expression<Func<TestOrder, Country>>) (m => m.Country), "CountryId").Not.LazyLoad();
    }
  }
}
