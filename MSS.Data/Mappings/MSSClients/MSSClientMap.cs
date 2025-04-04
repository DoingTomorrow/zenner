// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.MSSClients.MSSClientMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.MSSClients
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
