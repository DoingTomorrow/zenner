// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Archiving.ArchiveManagerNHibernate
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Common.Library.NHibernate.Data;
using MSS.Core.Model.Archiving;
using MSS.Interfaces;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

#nullable disable
namespace MSS.Business.Modules.Archiving
{
  public class ArchiveManagerNHibernate
  {
    private IRepositoryFactory RepositoryFactory { get; set; }

    public ArchiveManagerNHibernate(IRepositoryFactory repositoryFactory)
    {
      this.RepositoryFactory = repositoryFactory;
    }

    public void Archive(ArchiveDetailsNHibernate archiveDetails)
    {
      List<ArchiveEntity> archivedEntities = archiveDetails.ArchivedEntities;
      bool flag1 = archivedEntities.Any<ArchiveEntity>((Func<ArchiveEntity, bool>) (e => e.ArchivedEntityEnum == ArchivedEntitiesEnum.ReadingData && e.IsChecked));
      bool flag2 = archivedEntities.Any<ArchiveEntity>((Func<ArchiveEntity, bool>) (e => e.ArchivedEntityEnum == ArchivedEntitiesEnum.OrderAndStructure && e.IsChecked));
      bool flag3 = archivedEntities.Any<ArchiveEntity>((Func<ArchiveEntity, bool>) (e => e.ArchivedEntityEnum == ArchivedEntitiesEnum.Jobs && e.IsChecked));
      bool flag4 = archivedEntities.Any<ArchiveEntity>((Func<ArchiveEntity, bool>) (e => e.ArchivedEntityEnum == ArchivedEntitiesEnum.Logs && e.IsChecked));
      if (!flag1)
        ;
      if (!flag2)
        ;
      if (!flag3)
        ;
      if (!flag4)
        ;
    }

    public static void TransferObjects(
      IRepositoryFactory repositoryFactory,
      ArchiveDetailsNHibernate archiveDetails)
    {
    }

    public static ISessionFactory GetSessionFactoryMSSArchive()
    {
      string appSetting = ConfigurationManager.AppSettings["ArhiveDatabaseEngine"];
      HibernateMultipleDatabasesManager.Initialize(appSetting);
      return HibernateMultipleDatabasesManager.DataSessionFactory(appSetting);
    }
  }
}
