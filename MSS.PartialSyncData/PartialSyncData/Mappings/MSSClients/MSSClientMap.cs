// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.MSSClients.MSSClientMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.MSSClients
{
  public class MSSClientMap : ClassMap<MSS.Core.Model.MSSClient.MSSClient>
  {
    public MSSClientMap()
    {
      this.Table("t_MSSClient");
      this.Id((Expression<Func<MSS.Core.Model.MSSClient.MSSClient, object>>) (x => (object) x.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<MSS.Core.Model.MSSClient.MSSClient, object>>) (client => client.UniqueClientRequest)).Length(200).Not.Nullable();
      this.Map((Expression<Func<MSS.Core.Model.MSSClient.MSSClient, object>>) (client => (object) client.Status)).Not.Nullable();
      this.Map((Expression<Func<MSS.Core.Model.MSSClient.MSSClient, object>>) (client => (object) client.ApprovedOn)).Not.Nullable();
      this.Map((Expression<Func<MSS.Core.Model.MSSClient.MSSClient, object>>) (client => (object) client.UserId)).Not.Nullable();
      this.Map((Expression<Func<MSS.Core.Model.MSSClient.MSSClient, object>>) (client => client.UserName)).Not.Nullable();
    }
  }
}
