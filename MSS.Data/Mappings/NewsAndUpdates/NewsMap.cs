// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.NewsAndUpdates.NewsMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.NewsAndUpdates
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
