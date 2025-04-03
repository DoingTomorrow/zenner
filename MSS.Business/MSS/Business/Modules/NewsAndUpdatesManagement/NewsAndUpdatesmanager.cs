// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.NewsAndUpdatesManagement.NewsAndUpdatesmanager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using AutoMapper;
using MSS.Interfaces;
using MSSWeb.Common.WebApiUtils;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Business.Modules.NewsAndUpdatesManagement
{
  public class NewsAndUpdatesmanager
  {
    private readonly IRepositoryFactory _repositoryFactory;

    public NewsAndUpdatesmanager(IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
    }

    public void InsertNews(
      IEnumerable<NewsAndUpdatesSerializable> newsAndUpdatesList)
    {
      foreach (NewsAndUpdatesSerializable newsAndUpdates in newsAndUpdatesList)
      {
        Mapper.CreateMap<NewsAndUpdatesSerializable, MSS.Core.Model.News.News>();
        NewsAndUpdatesSerializable newsClone = newsAndUpdates;
        MSS.Core.Model.News.News news1 = this._repositoryFactory.GetRepository<MSS.Core.Model.News.News>().FirstOrDefault((Expression<Func<MSS.Core.Model.News.News, bool>>) (x => x.Id == newsClone.Id));
        if (news1 != null)
        {
          Mapper.Map<NewsAndUpdatesSerializable, MSS.Core.Model.News.News>(newsClone, news1);
          this._repositoryFactory.GetRepository<MSS.Core.Model.News.News>().Update(news1);
        }
        else
        {
          MSS.Core.Model.News.News news2 = new MSS.Core.Model.News.News();
          Mapper.Map<NewsAndUpdatesSerializable, MSS.Core.Model.News.News>(newsClone, news2);
          news2.IsNew = true;
          this._repositoryFactory.GetRepository<MSS.Core.Model.News.News>().Insert(news2);
        }
      }
    }
  }
}
