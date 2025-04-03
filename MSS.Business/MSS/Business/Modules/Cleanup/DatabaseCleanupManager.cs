// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Cleanup.DatabaseCleanupManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Core.Model.ApplicationParamenters;
using MSS.Core.Model.COMServers;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.DataFilters;
using MSS.Core.Model.Jobs;
using MSS.Core.Model.MDM;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Orders;
using MSS.Core.Model.Reporting;
using MSS.Core.Model.Structures;
using MSS.Core.Model.UsersManagement;
using MSS.Interfaces;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace MSS.Business.Modules.Cleanup
{
  public class DatabaseCleanupManager
  {
    private IRepositoryFactory RepositoryFactory { get; }

    private CleanupModel CleanupModel { get; }

    public DatabaseCleanupManager(IRepositoryFactory _repositoryFactory, CleanupModel _cleanupModel = null)
    {
      this.RepositoryFactory = _repositoryFactory;
      this.CleanupModel = _cleanupModel;
    }

    private void CleanupJobs()
    {
      this.RepositoryFactory.GetRepository<AutomatedExportJobCountry>().TransactionalDeleteAll();
      this.RepositoryFactory.GetRepository<AutomatedExportJob>().TransactionalDeleteAll();
      this.RepositoryFactory.GetRepository<JobReadingValues>().TransactionalDeleteAll();
      this.RepositoryFactory.GetRepository<JobLogs>().TransactionalDeleteAll();
      this.RepositoryFactory.GetRepository<MssReadingJob>().TransactionalDeleteAll();
    }

    private void CleanupApplicationScope()
    {
      this.CleanupOrder();
      this.CleanupStructure();
    }

    private void CleanupOrder()
    {
      this.DeleteAllEntitiesWithCondition<OrderUser>();
      this.DeleteAllEntitiesWithCondition<OrderMessage>();
      this.DeleteAllEntitiesWithCondition<OrderReadingValues>();
      this.DeleteAllEntitiesWithCondition<Order>();
    }

    private void CleanupStructure()
    {
      this.DeleteAllEntitiesWithCondition<StructureNodeLinks>();
      this.DeleteAllEntitiesWithCondition<StructureNodeEquipmentSettings>();
      this.DeleteAllEntitiesWithCondition<StructureNode>();
      this.DeleteAllEntitiesWithCondition<Note>();
      this.DeleteAllEntitiesWithCondition<MeterRadioDetails>();
      this.DeleteAllEntitiesWithCondition<MeterReplacementHistory>();
      this.DeleteAllEntitiesWithCondition<MbusRadioMeter>();
      this.DeleteAllEntitiesWithCondition<MinomatMeter>();
      this.DeleteAllEntitiesWithCondition<Meter>();
      this.DeleteAllEntitiesWithCondition<MinomatRadioDetails>();
      this.DeleteAllEntitiesWithCondition<Minomat>();
      this.DeleteAllEntitiesWithCondition<Tenant>();
      this.DeleteAllEntitiesWithCondition<Location>();
    }

    private void CleanupUsersScope()
    {
      this.RepositoryFactory.GetRepository<UserDeviceTypeSettings>().TransactionalDeleteAll();
      this.RepositoryFactory.GetRepository<UserRole>().TransactionalDeleteAll();
      this.RepositoryFactory.GetRepository<RoleOperation>().TransactionalDeleteAll();
      this.RepositoryFactory.GetRepository<User>().TransactionalDeleteAll();
      this.RepositoryFactory.GetRepository<Role>().TransactionalDeleteAll();
      this.RepositoryFactory.GetRepository<Operation>().TransactionalDeleteAll();
    }

    private void CleanupConfigurationScope()
    {
      this.RepositoryFactory.GetRepository<StructureNodeType>().TransactionalDeleteAll();
      this.RepositoryFactory.GetRepository<ScenarioJobDefinition>().TransactionalDeleteAll();
      this.RepositoryFactory.GetRepository<JobDefinition>().TransactionalDeleteAll();
      this.RepositoryFactory.GetRepository<Scenario>().TransactionalDeleteAll();
      this.RepositoryFactory.GetRepository<CelestaReadingDeviceTypes>().TransactionalDeleteAll();
      this.RepositoryFactory.GetRepository<RoomType>().TransactionalDeleteAll();
      this.RepositoryFactory.GetRepository<MeasureUnit>().TransactionalDeleteAll();
      this.RepositoryFactory.GetRepository<ConnectedDeviceType>().TransactionalDeleteAll();
      this.RepositoryFactory.GetRepository<Channel>().TransactionalDeleteAll();
      this.RepositoryFactory.GetRepository<Rules>().TransactionalDeleteAll();
      this.RepositoryFactory.GetRepository<Provider>().TransactionalDeleteAll();
      this.RepositoryFactory.GetRepository<COMServer>().TransactionalDeleteAll();
      this.RepositoryFactory.GetRepository<MDMConfigs>().TransactionalDeleteAll();
      this.RepositoryFactory.GetRepository<NoteType>().TransactionalDeleteAll();
      this.RepositoryFactory.GetRepository<MSS.Core.Model.DataFilters.Filter>().TransactionalDeleteAll();
      this.RepositoryFactory.GetRepository<Country>().TransactionalDeleteAll();
    }

    private void CleanupReadingValuesScope()
    {
      this.DeleteAllEntitiesWithCondition<MeterReadingValue>();
    }

    private ITransaction InitializeCleanupOperation()
    {
      ISession session = this.RepositoryFactory.GetSession();
      session.FlushMode = FlushMode.Commit;
      return session.BeginTransaction();
    }

    private string GetWhereCondition(IEnumerable<Guid> ids)
    {
      if (ids == null || !ids.Any<Guid>())
        return string.Empty;
      StringBuilder builder = new StringBuilder("WHERE Id IN (");
      TypeHelperExtensionMethods.ForEach<Guid>(ids.Take<Guid>(ids.Count<Guid>() - 1), (Action<Guid>) (x => builder.Append("'" + (object) x + "',")));
      builder.Append("'" + (object) ids.LastOrDefault<Guid>() + "'");
      builder.Append(")");
      return builder.ToString();
    }

    private void DeleteAllEntitiesWithCondition<T>() where T : class
    {
      string empty = string.Empty;
      if (this.CleanupModel != null)
      {
        string name = "IDs" + typeof (T).Name;
        List<Guid> guidList = (List<Guid>) this.CleanupModel.GetType().GetProperty(name).GetValue((object) this.CleanupModel);
        if (guidList == null || !guidList.Any<Guid>())
          return;
        if (guidList.Count > 50)
        {
          int count = guidList.Count;
          while (count > 0)
          {
            if (count > 50)
            {
              string whereCondition = this.GetWhereCondition(guidList.Skip<Guid>(guidList.Count - count).Take<Guid>(50));
              this.RepositoryFactory.GetRepository<T>().TransactionalDeleteAll(whereCondition);
              count -= 50;
            }
            else
            {
              string whereCondition = this.GetWhereCondition(guidList.Skip<Guid>(guidList.Count - count).Take<Guid>(count));
              this.RepositoryFactory.GetRepository<T>().TransactionalDeleteAll(whereCondition);
              count -= count;
            }
          }
        }
        else
        {
          string whereCondition = this.GetWhereCondition((IEnumerable<Guid>) guidList);
          this.RepositoryFactory.GetRepository<T>().TransactionalDeleteAll(whereCondition);
        }
      }
      else
        this.RepositoryFactory.GetRepository<T>().TransactionalDeleteAll(empty);
    }

    public void CleanupDatabaseOnChangingServer(bool isPartialSync)
    {
      ITransaction transaction = this.InitializeCleanupOperation();
      try
      {
        this.CleanupJobs();
        this.CleanupApplicationScope();
        this.CleanupUsersScope();
        this.CleanupReadingValuesScope();
        this.CleanupConfigurationScope();
        transaction.Commit();
        if (!isPartialSync)
          return;
        this.ResetSyncParams();
      }
      catch (Exception ex)
      {
        transaction.Rollback();
      }
    }

    public void CleanupDatabaseOfOldClosedOrders()
    {
      ITransaction transaction = this.InitializeCleanupOperation();
      try
      {
        this.CleanupApplicationScope();
        this.CleanupReadingValuesScope();
        transaction.Commit();
      }
      catch (Exception ex)
      {
        transaction.Rollback();
      }
    }

    public void CleanupDatabaseOnChangingLoggedUser()
    {
      ITransaction transaction = this.InitializeCleanupOperation();
      try
      {
        this.CleanupApplicationScope();
        this.CleanupReadingValuesScope();
        transaction.Commit();
        this.ResetSyncParams();
      }
      catch (Exception ex)
      {
        transaction.Rollback();
      }
    }

    private void ResetSyncParams()
    {
      MSS.Business.Modules.AppParametersManagement.AppParametersManagement parametersManagement = new MSS.Business.Modules.AppParametersManagement.AppParametersManagement(this.RepositoryFactory);
      ApplicationParameter appParam1 = parametersManagement.GetAppParam("LastSuccesfullDownload");
      appParam1.Value = "";
      parametersManagement.Update(appParam1);
      ApplicationParameter appParam2 = parametersManagement.GetAppParam("LastSuccesfullUpload");
      appParam2.Value = "";
      parametersManagement.Update(appParam2);
    }
  }
}
