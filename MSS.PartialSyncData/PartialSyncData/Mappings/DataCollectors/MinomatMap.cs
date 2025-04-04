// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.DataCollectors.MinomatMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Structures;
using MSS.Core.Model.UsersManagement;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.DataCollectors
{
  public class MinomatMap : ClassMap<Minomat>
  {
    public MinomatMap()
    {
      this.Table("t_Minomat");
      this.Id((Expression<Func<Minomat, object>>) (n => (object) n.Id)).GeneratedBy.Assigned();
      this.Map((Expression<Func<Minomat, object>>) (c => c.RadioId));
      this.Map((Expression<Func<Minomat, object>>) (c => c.Status));
      this.Map((Expression<Func<Minomat, object>>) (c => (object) c.Registered));
      this.Map((Expression<Func<Minomat, object>>) (c => c.HostAndPort));
      this.Map((Expression<Func<Minomat, object>>) (c => c.Url));
      this.Map((Expression<Func<Minomat, object>>) (c => c.CreatedBy));
      this.Map((Expression<Func<Minomat, object>>) (c => (object) c.CreatedOn));
      this.Map((Expression<Func<Minomat, object>>) (c => c.LastUpdatedBy));
      this.Map((Expression<Func<Minomat, object>>) (c => (object) c.LastChangedOn)).Nullable();
      this.Map((Expression<Func<Minomat, object>>) (c => c.Challenge));
      this.Map((Expression<Func<Minomat, object>>) (c => c.GsmId));
      this.Map((Expression<Func<Minomat, object>>) (c => (object) c.IsDeactivated));
      this.Map((Expression<Func<Minomat, object>>) (c => (object) c.StartDate));
      this.Map((Expression<Func<Minomat, object>>) (c => (object) c.EndDate));
      this.Map((Expression<Func<Minomat, object>>) (c => (object) c.Polling));
      this.Map((Expression<Func<Minomat, object>>) (c => (object) c.IsMaster));
      this.Map((Expression<Func<Minomat, object>>) (c => (object) c.IsInMasterPool));
      this.Map((Expression<Func<Minomat, object>>) (c => c.ProviderName));
      this.Map((Expression<Func<Minomat, object>>) (c => c.SimPin));
      this.Map((Expression<Func<Minomat, object>>) (c => c.AccessPoint));
      this.Map((Expression<Func<Minomat, object>>) (c => c.UserId));
      this.Map((Expression<Func<Minomat, object>>) (c => c.UserPassword));
      this.Map((Expression<Func<Minomat, object>>) (c => c.SessionKey));
      this.Map((Expression<Func<Minomat, object>>) (c => c.CommParameter));
      this.Map((Expression<Func<Minomat, object>>) (c => c.CreatedByName));
      this.Map((Expression<Func<Minomat, object>>) (c => (object) c.MinomatMasterId));
      this.Map((Expression<Func<Minomat, object>>) (c => (object) c.LoggingEnabled));
      this.Map((Expression<Func<Minomat, object>>) (c => c.SimCardNumber));
      this.HasOne<MinomatRadioDetails>((Expression<Func<Minomat, MinomatRadioDetails>>) (m => m.RadioDetails)).PropertyRef((Expression<Func<MinomatRadioDetails, object>>) (x => x.Minomat)).Cascade.Delete();
      this.References<Scenario>((Expression<Func<Minomat, Scenario>>) (ur => ur.Scenario), "ScenarioId").Class<Scenario>();
      this.References<Provider>((Expression<Func<Minomat, Provider>>) (m => m.Provider), "ProviderId");
      this.References<Country>((Expression<Func<Minomat, Country>>) (m => m.Country), "CountryId");
    }
  }
}
