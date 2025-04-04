// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.NewsAndUpdates.NewsMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.NewsAndUpdates
{
  public class NewsMap : ClassMap<MSS.Core.Model.News.News>
  {
    public NewsMap()
    {
      this.Table("t_NewsAndUpdates");
      this.Id((Expression<Func<MSS.Core.Model.News.News, object>>) (n => (object) n.Id)).GeneratedBy.Assigned();
      this.Map((Expression<Func<MSS.Core.Model.News.News, object>>) (c => c.Subject));
      this.Map((Expression<Func<MSS.Core.Model.News.News, object>>) (c => (object) c.StartDate));
      this.Map((Expression<Func<MSS.Core.Model.News.News, object>>) (c => (object) c.EndDate));
      this.Map((Expression<Func<MSS.Core.Model.News.News, object>>) (c => c.Description)).Length(5000);
      this.Map((Expression<Func<MSS.Core.Model.News.News, object>>) (c => (object) c.IsNew));
    }
  }
}
