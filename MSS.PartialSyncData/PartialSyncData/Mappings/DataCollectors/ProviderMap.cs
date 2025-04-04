// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.DataCollectors.ProviderMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.DataCollectors;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.DataCollectors
{
  public class ProviderMap : ClassMap<Provider>
  {
    private ProviderMap()
    {
      this.Table("t_Provider");
      this.Id((Expression<Func<Provider, object>>) (n => (object) n.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<Provider, object>>) (c => c.ProviderName));
      this.Map((Expression<Func<Provider, object>>) (c => c.SimPin));
      this.Map((Expression<Func<Provider, object>>) (c => c.AccessPoint));
      this.Map((Expression<Func<Provider, object>>) (c => c.UserId));
      this.Map((Expression<Func<Provider, object>>) (c => c.UserPassword));
      this.HasMany<Minomat>((Expression<Func<Provider, IEnumerable<Minomat>>>) (m => m.MinomatsList)).KeyColumn("ProviderId").Inverse();
    }
  }
}
