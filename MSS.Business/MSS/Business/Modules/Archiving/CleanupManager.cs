// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Archiving.CleanupManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Core.Model.Archiving;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Meters;
using MSS.Interfaces;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace MSS.Business.Modules.Archiving
{
  public class CleanupManager
  {
    private IRepositoryFactory RepositoryFactory { get; set; }

    private ITransaction Transaction { get; set; }

    public CleanupManager(IRepositoryFactory repositoryFactory)
    {
      this.RepositoryFactory = repositoryFactory;
    }

    public int Cleanup(ArchiveJob archiveJob)
    {
      List<ArchiveEntity> source = ArchivingHelper.DeserializeArchivedEntities(archiveJob.ArchivedEntities);
      bool flag1 = source.Any<ArchiveEntity>((Func<ArchiveEntity, bool>) (e => e.ArchivedEntityEnum == ArchivedEntitiesEnum.ReadingData && e.IsChecked));
      bool flag2 = source.Any<ArchiveEntity>((Func<ArchiveEntity, bool>) (e => e.ArchivedEntityEnum == ArchivedEntitiesEnum.OrderAndStructure && e.IsChecked));
      bool flag3 = source.Any<ArchiveEntity>((Func<ArchiveEntity, bool>) (e => e.ArchivedEntityEnum == ArchivedEntitiesEnum.Jobs && e.IsChecked));
      bool flag4 = source.Any<ArchiveEntity>((Func<ArchiveEntity, bool>) (e => e.ArchivedEntityEnum == ArchivedEntitiesEnum.Logs && e.IsChecked));
      int num1 = 0;
      try
      {
        if (flag1)
        {
          int num2 = this.RepositoryFactory.GetSession().QueryOver<MeterReadingValue>().RowCount();
          num1 += num2;
          if (num2 > 0)
            this.CleanupReadingData();
        }
        if (flag2)
          this.CleanupOrders();
        if (flag3)
          this.CleanupJobs();
        if (flag4)
        {
          int num3 = this.RepositoryFactory.GetSession().QueryOver<MinomatConnectionLog>().RowCount();
          num1 += num3;
          if (num3 > 0)
            this.CleanupLogs();
        }
      }
      catch (Exception ex)
      {
        this.RollbackTransaction();
        throw;
      }
      return num1;
    }

    public void InitializeTransaction()
    {
      this.Transaction = this.RepositoryFactory.GetSession().BeginTransaction();
    }

    public void CommitTransaction() => this.Transaction.Commit();

    public void RollbackTransaction() => this.Transaction.Rollback();

    public void CleanupReadingData()
    {
      ISession session = this.RepositoryFactory.GetSession();
      try
      {
        this.InitializeTransaction();
        session.CreateSQLQuery("exec sp_ArchiveDisableTriggers").ExecuteUpdate();
        this.CommitTransaction();
        this.InitializeTransaction();
        session.CreateSQLQuery("delete from t_OrderReadingValues where EXISTS (select id FROM temp_processing_items where MeterReadingValueId = temp_processing_items.id)").ExecuteUpdate();
        session.CreateSQLQuery("delete from t_JobReadingValues where EXISTS (select id FROM temp_processing_items where ReadingValueId = temp_processing_items.id)").ExecuteUpdate();
        session.CreateSQLQuery("DELETE FROM view_ArchiveReadingValues where EXISTS (select id FROM temp_processing_items where view_ArchiveReadingValues.Id = temp_processing_items.id)").ExecuteUpdate();
        session.CreateSQLQuery("truncate table temp_processing_items").ExecuteUpdate();
        this.CommitTransaction();
      }
      finally
      {
        this.InitializeTransaction();
        session.CreateSQLQuery("exec sp_ArchiveEnableTriggers").ExecuteUpdate();
        this.CommitTransaction();
      }
    }

    public void CleanupLogs()
    {
      ISession session = this.RepositoryFactory.GetSession();
      this.InitializeTransaction();
      session.CreateSQLQuery("DELETE FROM view_ArchiveLogs where EXISTS (select id FROM temp_processing_items where view_ArchiveLogs.Id = temp_processing_items.id)").ExecuteUpdate();
      session.CreateSQLQuery("truncate table temp_processing_items").ExecuteUpdate();
      this.CommitTransaction();
    }

    public void CleanupOrders()
    {
    }

    public void CleanupJobs()
    {
    }

    public void TransactionalInsert(Archive archive)
    {
      MSS.Core.Model.Archiving.Cleanup entity = new MSS.Core.Model.Archiving.Cleanup()
      {
        CleanupDate = DateTime.Now,
        Archive = archive
      };
      this.RepositoryFactory.GetRepository<MSS.Core.Model.Archiving.Cleanup>().TransactionalInsert(entity);
    }
  }
}
