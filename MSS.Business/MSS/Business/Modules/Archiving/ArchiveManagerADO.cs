// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Archiving.ArchiveManagerADO
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Utils;
using MSS.Core.Model.Archiving;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Meters;
using MSS.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

#nullable disable
namespace MSS.Business.Modules.Archiving
{
  public class ArchiveManagerADO
  {
    private IRepositoryFactory RepositoryFactory { get; set; }

    private GenericArchiver Archiver { get; set; }

    public ArchiveManagerADO(IRepositoryFactory repositoryFactory)
    {
      this.RepositoryFactory = repositoryFactory;
      DatabaseEngineEnum databaseEngineEnum = (DatabaseEngineEnum) Enum.Parse(typeof (DatabaseEngineEnum), ConfigurationManager.AppSettings["DatabaseEngine"]);
      string propertyValue1 = NHibernateConfigurationHelper.GetPropertyValue(ConfigurationManager.AppSettings["DatabaseEngine"], "connection.connection_string");
      string propertyValue2 = NHibernateConfigurationHelper.GetPropertyValue(ConfigurationManager.AppSettings["ArhiveDatabaseEngine"], "connection.connection_string");
      switch (databaseEngineEnum)
      {
        case DatabaseEngineEnum.MSSQLDatabase:
          this.Archiver = (GenericArchiver) new MSSQLDatabaseArchiver(propertyValue1, propertyValue2, repositoryFactory.GetSession().SessionFactory);
          break;
        case DatabaseEngineEnum.SQLiteDatabase:
          this.Archiver = (GenericArchiver) new SQLiteDatabaseArchiver(propertyValue1, propertyValue2, repositoryFactory.GetSession().SessionFactory);
          break;
        default:
          throw new Exception("No database engine provided");
      }
    }

    public int Archive(ArchiveJob archiveJob)
    {
      List<ArchiveEntity> source = ArchivingHelper.DeserializeArchivedEntities(archiveJob.ArchivedEntities);
      bool flag1 = source.Any<ArchiveEntity>((Func<ArchiveEntity, bool>) (e => e.ArchivedEntityEnum == ArchivedEntitiesEnum.ReadingData && e.IsChecked));
      bool flag2 = source.Any<ArchiveEntity>((Func<ArchiveEntity, bool>) (e => e.ArchivedEntityEnum == ArchivedEntitiesEnum.OrderAndStructure && e.IsChecked));
      bool flag3 = source.Any<ArchiveEntity>((Func<ArchiveEntity, bool>) (e => e.ArchivedEntityEnum == ArchivedEntitiesEnum.Structure && e.IsChecked));
      bool flag4 = source.Any<ArchiveEntity>((Func<ArchiveEntity, bool>) (e => e.ArchivedEntityEnum == ArchivedEntitiesEnum.Jobs && e.IsChecked));
      bool flag5 = source.Any<ArchiveEntity>((Func<ArchiveEntity, bool>) (e => e.ArchivedEntityEnum == ArchivedEntitiesEnum.Logs && e.IsChecked));
      this.Archiver.Initialize();
      int num1 = 0;
      try
      {
        if (flag1)
        {
          int num2 = this.RepositoryFactory.GetSession().QueryOver<MeterReadingValue>().RowCount();
          num1 += num2;
          if (num2 > 0)
            this.Archiver.ArchiveReadingData(archiveJob);
        }
        if (flag2)
          this.Archiver.ArchiveOrders(archiveJob);
        if (flag4)
          this.Archiver.ArchiveJobs(archiveJob);
        if (flag3)
          this.Archiver.ArchiveStructures(archiveJob);
        if (flag5)
        {
          int num3 = this.RepositoryFactory.GetSession().QueryOver<MinomatConnectionLog>().RowCount();
          num1 += num3;
          if (num3 > 0)
            this.Archiver.ArchiveLogs(archiveJob);
        }
      }
      catch (Exception ex)
      {
        this.Archiver.Error();
        throw;
      }
      finally
      {
        this.Archiver.Commit();
      }
      return num1;
    }
  }
}
