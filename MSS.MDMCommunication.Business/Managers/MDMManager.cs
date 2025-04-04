// Decompiled with JetBrains decompiler
// Type: MSS.MDMCommunication.Business.Managers.MDMManager
// Assembly: MSS.MDMCommunication.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBA4B3BD-8D82-4E93-946D-7969F81D07F7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.MDMCommunication.Business.dll

using GmmDbLib;
using GmmDbLib.DataSets;
using MinomatHandler;
using MSS.Business.Modules.OrdersManagement;
using MSS.Core.Model.ApplicationParamenters;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Jobs;
using MSS.Core.Model.MDM;
using MSS.Core.Model.Meters;
using MSS.Core.Model.RadioTest;
using MSS.Core.Model.Structures;
using MSS.Core.Model.UsersManagement;
using MSS.Core.Utils;
using MSS.DTO.Minomat;
using MSS.Interfaces;
using MSS.MDMCommunication.Business.MDMService;
using MSS.MDMCommunication.Business.Modules;
using MSS.MDMCommunication.Business.Utils;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using ZENNER;

#nullable disable
namespace MSS.MDMCommunication.Business.Managers
{
  [Guid("4E76720C-4160-4D52-8A3C-B9E62C58982E")]
  public class MDMManager
  {
    private static readonly DateTime MinDateTime = new DateTime(1800, 1, 1);
    private static readonly DateTime ValidToDate = new DateTime(2079, 6, 6);
    private IRepository<StructureNode> _structureNodeRepository;
    private IRepository<StructureNodeLinks> _structureNodeLinksRepository;
    private IRepository<StructureNodeType> _structureNodeTypeRepository;
    private IRepository<MeterReadingValue> _meterReadingValueRepository;
    private IRepository<MSS.Core.Model.Structures.Location> _locationRepository;
    private IRepository<Tenant> _tenantRepository;
    private IRepository<Meter> _meterRepository;
    private IRepository<ApplicationParameter> _appParamRepository;
    private IRepository<MDMConfigs> _mdmConfigsRepository;
    private IRepository<MSS.Core.Model.DataCollectors.Minomat> _minomatRepository;
    private IRepository<MinomatRadioDetails> _minomatRadioDetailsRepository;
    private IRepository<MSS.Core.Model.Orders.Order> _orderRepository;
    private IRepository<User> _userRepository;
    private IRepository<RadioTestRun> _radioTestRunRepository;
    private IRepository<RadioTestRunDevice> _radioTestRunDeviceRepository;
    private IRepository<TestOrder> _testOrderRepository;
    private IRepositoryFactory _repositoryFactory;
    private string batchSize;
    private static Logger mdmLogger;
    private const int READ_BATCH_SIZE = 500;

    public MDMManager(IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this.InitializeRepositories(repositoryFactory);
      this.batchSize = this._appParamRepository.FirstOrDefault((Expression<System.Func<ApplicationParameter, bool>>) (p => p.Parameter == "BatchSize")).Value;
      MDMManager.mdmLogger = MDMLogger.GetLogger();
    }

    public void InitializeRepositories(IRepositoryFactory repositoryFactory)
    {
      this._structureNodeRepository = repositoryFactory.GetRepository<StructureNode>();
      this._structureNodeLinksRepository = repositoryFactory.GetRepository<StructureNodeLinks>();
      this._structureNodeTypeRepository = repositoryFactory.GetRepository<StructureNodeType>();
      this._meterReadingValueRepository = repositoryFactory.GetRepository<MeterReadingValue>();
      this._locationRepository = repositoryFactory.GetRepository<MSS.Core.Model.Structures.Location>();
      this._tenantRepository = repositoryFactory.GetRepository<Tenant>();
      this._meterRepository = repositoryFactory.GetRepository<Meter>();
      this._appParamRepository = repositoryFactory.GetRepository<ApplicationParameter>();
      this._mdmConfigsRepository = repositoryFactory.GetRepository<MDMConfigs>();
      this._minomatRepository = repositoryFactory.GetRepository<MSS.Core.Model.DataCollectors.Minomat>();
      this._minomatRadioDetailsRepository = repositoryFactory.GetRepository<MinomatRadioDetails>();
      this._orderRepository = repositoryFactory.GetRepository<MSS.Core.Model.Orders.Order>();
      this._userRepository = repositoryFactory.GetRepository<User>();
      this._radioTestRunRepository = repositoryFactory.GetRepository<RadioTestRun>();
      this._radioTestRunDeviceRepository = repositoryFactory.GetRepository<RadioTestRunDevice>();
      this._testOrderRepository = repositoryFactory.GetRepository<TestOrder>();
    }

    public void SavePortfolioRecord()
    {
      try
      {
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "-------------------------------Begin Save Portfolio Record ------------------------------------");
        List<MSS.Core.Model.Structures.Location> list = this._locationRepository.SearchFor((Expression<System.Func<MSS.Core.Model.Structures.Location, bool>>) (l => l.LastPortfolioMDMExportOn == new DateTime?() || l.LastPortfolioMDMExportOn != new DateTime?() && l.LastUpdateBuildingNo >= l.LastPortfolioMDMExportOn)).ToList<MSS.Core.Model.Structures.Location>();
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "Number of locations: " + (object) list.Count);
        foreach (IGrouping<Country, MSS.Core.Model.Structures.Location> source in list.GroupBy<MSS.Core.Model.Structures.Location, Country>((System.Func<MSS.Core.Model.Structures.Location, Country>) (l => l.Country)))
        {
          Guid countryID = source.Key.Id;
          MDMConfigs mdmConfig = this._mdmConfigsRepository.FirstOrDefault((Expression<System.Func<MDMConfigs, bool>>) (m => m.Country.Id == countryID));
          if (mdmConfig != null)
          {
            MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, "Portfolio Record( Enterprise_Id:" + (object) mdmConfig.Company + ")");
            foreach (IGrouping<int, MSS.Core.Model.Structures.Location> grouping in source.Select((x, index) => new
            {
              x = x,
              index = index
            }).GroupBy(x => x.index / Convert.ToInt32(this.batchSize), y => y.x))
            {
              List<PortfolioRecord> data = new List<PortfolioRecord>();
              foreach (MSS.Core.Model.Structures.Location location1 in (IEnumerable<MSS.Core.Model.Structures.Location>) grouping)
              {
                MSS.Core.Model.Structures.Location location = location1;
                MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "Location Building Number: " + location.BuildingNr);
                PortfolioRecord portfolioRecord = new PortfolioRecord();
                portfolioRecord.Portfolio_ID = location.BuildingNr;
                portfolioRecord.Create_Date = this._structureNodeRepository.FirstOrDefault((Expression<System.Func<StructureNode, bool>>) (s => s.EntityId == location.Id)).StartDate;
                portfolioRecord.Create_User = string.IsNullOrEmpty(location.CreatedBy) ? string.Empty : this._userRepository.GetById((object) new Guid(location.CreatedBy)).Username;
                portfolioRecord.Name = string.Format("{0}, {1}, {2}", (object) location.ZipCode.Trim(), (object) location.City.Trim(), (object) location.Street.Trim());
                portfolioRecord.Address_ID = location.BuildingNr;
                portfolioRecord.Country = location.Country.Code;
                portfolioRecord.Office = location.Office;
                portfolioRecord.IsActive = !location.IsDeactivated;
                PortfolioRecord recordObject = portfolioRecord;
                data.Add(recordObject);
                string propNameColumns;
                string propValuesColumns;
                this.GetPropertiesNameAndValue<PortfolioRecord>(recordObject, out propNameColumns, out propValuesColumns);
                MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, propNameColumns);
                MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, propValuesColumns);
              }
              DataTable dataTable = data.ToDataTable<PortfolioRecord>();
              dataTable.TableName = "PortfolioRecordDTName";
              Service_MDMS_DataClient serviceMdmsDataClient = this.ServiceMdmsDataClient(mdmConfig);
              string msg;
              serviceMdmsDataClient.SavePortfolioRecord(mdmConfig.MDMUser, mdmConfig.MDMPassword, mdmConfig.Company.ToString(), dataTable, out msg);
              MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, msg);
              serviceMdmsDataClient.Close();
              foreach (MSS.Core.Model.Structures.Location entity in (IEnumerable<MSS.Core.Model.Structures.Location>) grouping)
              {
                entity.LastPortfolioMDMExportOn = new DateTime?(DateTime.Now);
                this._locationRepository.Update(entity);
              }
            }
          }
          else
            MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, source.First<MSS.Core.Model.Structures.Location>().Country.Name + "missing from MDM config. Locations for this country were not exported!");
        }
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "-------------------------------End Save Portfolio Record ------------------------------------");
      }
      catch (Exception ex)
      {
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, ex.ToString());
      }
    }

    public void SaveBuildingRecord()
    {
      try
      {
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "-------------------------------Begin Save Building Record ------------------------------------");
        List<MSS.Core.Model.Structures.Location> list = this._locationRepository.SearchFor((Expression<System.Func<MSS.Core.Model.Structures.Location, bool>>) (l => l.LastBuildingMDMExportOn == new DateTime?() || l.LastBuildingMDMExportOn != new DateTime?() && l.LastUpdateBuildingNo >= l.LastBuildingMDMExportOn)).ToList<MSS.Core.Model.Structures.Location>();
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "Number of locations: " + (object) list.Count);
        Guid id = this._structureNodeTypeRepository.FirstOrDefault((Expression<System.Func<StructureNodeType, bool>>) (t => t.Name == StructureNodeTypeEnum.Tenant.ToString())).Id;
        foreach (IGrouping<Country, MSS.Core.Model.Structures.Location> source in list.GroupBy<MSS.Core.Model.Structures.Location, Country>((System.Func<MSS.Core.Model.Structures.Location, Country>) (l => l.Country)))
        {
          Guid countryID = source.Key.Id;
          MDMConfigs mdmConfig = this._mdmConfigsRepository.FirstOrDefault((Expression<System.Func<MDMConfigs, bool>>) (m => m.Country.Id == countryID));
          if (mdmConfig != null)
          {
            MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, "Portfolio Record( Enterprise_Id:" + (object) mdmConfig.Company + ")");
            foreach (IGrouping<int, MSS.Core.Model.Structures.Location> grouping in list.Select((x, index) => new
            {
              x = x,
              index = index
            }).GroupBy(x => x.index / Convert.ToInt32(this.batchSize), y => y.x))
            {
              try
              {
                List<BuildingRecord> data = new List<BuildingRecord>();
                foreach (MSS.Core.Model.Structures.Location location1 in (IEnumerable<MSS.Core.Model.Structures.Location>) grouping)
                {
                  MSS.Core.Model.Structures.Location location = location1;
                  int nrOfTenants = this.GetNrOfTenants(this._structureNodeRepository.FirstOrDefault((Expression<System.Func<StructureNode, bool>>) (n => n.EntityId == location.Id)), id);
                  BuildingRecord buildingRecord1 = new BuildingRecord();
                  buildingRecord1.Portfolio_ID = location.BuildingNr;
                  buildingRecord1.Project_ID = location.BuildingNr;
                  buildingRecord1.Address_ID = location.BuildingNr;
                  buildingRecord1.Create_Date = this._structureNodeRepository.FirstOrDefault((Expression<System.Func<StructureNode, bool>>) (s => s.EntityId == location.Id)).StartDate;
                  buildingRecord1.Create_User = string.IsNullOrEmpty(location.CreatedBy) ? string.Empty : this._userRepository.GetById((object) new Guid(location.CreatedBy)).Username;
                  BuildingRecord buildingRecord2 = buildingRecord1;
                  DateTime? nullable1 = location.DueDate;
                  DateTime minValue;
                  if (!nullable1.HasValue)
                  {
                    minValue = DateTime.MinValue;
                  }
                  else
                  {
                    nullable1 = location.DueDate;
                    minValue = nullable1.Value;
                  }
                  buildingRecord2.CurrDueDate = minValue;
                  BuildingRecord buildingRecord3 = buildingRecord1;
                  nullable1 = location.LastUpdateBuildingNo;
                  DateTime minDateTime = MDMManager.MinDateTime;
                  DateTime? nullable2;
                  if ((nullable1.HasValue ? (nullable1.HasValue ? (nullable1.GetValueOrDefault() != minDateTime ? 1 : 0) : 0) : 1) == 0)
                  {
                    nullable1 = new DateTime?();
                    nullable2 = nullable1;
                  }
                  else
                    nullable2 = location.LastUpdateBuildingNo;
                  buildingRecord3.Update_Date = nullable2;
                  buildingRecord1.Update_User = string.IsNullOrEmpty(location.UpdatedBy) ? string.Empty : this._userRepository.GetById((object) new Guid(location.UpdatedBy)).Username;
                  buildingRecord1.MgmtAddr_ID = location.BuildingNr;
                  buildingRecord1.NbrUnits = nrOfTenants;
                  buildingRecord1.OwnerAddr_ID = location.BuildingNr;
                  buildingRecord1.Country = location.Country.Code;
                  buildingRecord1.Office = location.Office;
                  buildingRecord1.Factory = string.Empty;
                  buildingRecord1.Scenario = location.Scenario.CelestaCode.ToCharArray()[0];
                  buildingRecord1.Generation = Convert.ToChar((object) location.Generation);
                  buildingRecord1.IsConfig = false;
                  buildingRecord1.IsActive = !location.IsDeactivated;
                  BuildingRecord recordObject = buildingRecord1;
                  data.Add(recordObject);
                  string propNameColumns;
                  string propValuesColumns;
                  this.GetPropertiesNameAndValue<BuildingRecord>(recordObject, out propNameColumns, out propValuesColumns);
                  MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, propNameColumns);
                  MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, propValuesColumns);
                }
                DataTable dataTable = data.ToDataTable<BuildingRecord>();
                dataTable.TableName = "BuildingRecordDTName";
                Service_MDMS_DataClient serviceMdmsDataClient = this.ServiceMdmsDataClient(mdmConfig);
                string msg;
                serviceMdmsDataClient.SaveBuildingRecord(mdmConfig.MDMUser, mdmConfig.MDMPassword, mdmConfig.Company.ToString(), "000001000", dataTable, out msg);
                MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, msg);
                serviceMdmsDataClient.Close();
                foreach (MSS.Core.Model.Structures.Location entity in (IEnumerable<MSS.Core.Model.Structures.Location>) grouping)
                {
                  entity.LastBuildingMDMExportOn = new DateTime?(DateTime.Now);
                  this._locationRepository.Update(entity);
                }
              }
              catch (Exception ex)
              {
                MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, ex.ToString());
              }
            }
          }
          else
            MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, source.First<MSS.Core.Model.Structures.Location>().Country.Name + "missing from MDM config. Locations for this country were not exported!");
        }
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "-------------------------------End Save Portfolio Record ------------------------------------");
      }
      catch (Exception ex)
      {
      }
    }

    public void SaveTenantInfoRecord()
    {
      try
      {
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "-------------------------------Begin Save Tenant Info Record ------------------------------------");
        List<Tenant> list = this._tenantRepository.SearchFor((Expression<System.Func<Tenant, bool>>) (l => l.LastTenantInfoMDMExportOn == new DateTime?() || l.LastTenantInfoMDMExportOn != new DateTime?() && l.LastChangedOn >= l.LastTenantInfoMDMExportOn)).ToList<Tenant>();
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "Number of tenants: " + (object) list.Count);
        foreach (KeyValuePair<Country, List<Tenant>> keyValuePair in this.GroupTenantsByCountry(list))
        {
          Guid countryID = keyValuePair.Key.Id;
          MDMConfigs mdmConfig = this._mdmConfigsRepository.FirstOrDefault((Expression<System.Func<MDMConfigs, bool>>) (m => m.Country.Id == countryID));
          if (mdmConfig != null)
          {
            MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, "Tenant Info Record( Enterprise_Id:" + (object) mdmConfig.Company + ")");
            foreach (IGrouping<int, Tenant> grouping in keyValuePair.Value.Select((x, index) => new
            {
              x = x,
              index = index
            }).GroupBy(x => x.index / Convert.ToInt32(this.batchSize), y => y.x))
            {
              try
              {
                List<TenantInfoRecord> data = new List<TenantInfoRecord>();
                foreach (Tenant tenant1 in (IEnumerable<Tenant>) grouping)
                {
                  Tenant tenant = tenant1;
                  MSS.Core.Model.Structures.Location locationForTenant = this.GetLocationForTenant(tenant);
                  string tenantNrForUnitNbr = this.GetTenantNrForUnitNbr(tenant.TenantNr);
                  TenantInfoRecord tenantInfoRecord1 = new TenantInfoRecord();
                  tenantInfoRecord1.Project_ID = locationForTenant?.BuildingNr;
                  tenantInfoRecord1.UnitNbr = tenantNrForUnitNbr;
                  tenantInfoRecord1.Tenant_ID = tenant.TenantNr.ToString();
                  tenantInfoRecord1.Valid_From = this._structureNodeRepository.FirstOrDefault((Expression<System.Func<StructureNode, bool>>) (s => s.EntityId == tenant.Id)).StartDate;
                  TenantInfoRecord tenantInfoRecord2 = tenantInfoRecord1;
                  DateTime? nullable1;
                  if (!tenant.IsDeactivated)
                  {
                    nullable1 = new DateTime?(MDMManager.ValidToDate);
                  }
                  else
                  {
                    DateTime? nullable2 = tenant.LastChangedOn;
                    DateTime minDateTime = MDMManager.MinDateTime;
                    if ((nullable2.HasValue ? (nullable2.HasValue ? (nullable2.GetValueOrDefault() == minDateTime ? 1 : 0) : 1) : 0) == 0)
                    {
                      nullable1 = tenant.LastChangedOn;
                    }
                    else
                    {
                      nullable2 = new DateTime?();
                      nullable1 = nullable2;
                    }
                  }
                  tenantInfoRecord2.Valid_To = nullable1;
                  tenantInfoRecord1.ReferenceDate = new DateTime?();
                  tenantInfoRecord1.Tenant_Name = tenant.Name;
                  tenantInfoRecord1.IsEmpty = false;
                  tenantInfoRecord1.IsInWork = false;
                  tenantInfoRecord1.TenantChange = false;
                  tenantInfoRecord1.Cust_TenantID = tenant.CustomerTenantNo;
                  tenantInfoRecord1.NextTenant_ID = (string) null;
                  tenantInfoRecord1.Print = false;
                  tenantInfoRecord1.Language = (string) null;
                  tenantInfoRecord1.CostForHeat = "false";
                  tenantInfoRecord1.VATForm = (string) null;
                  tenantInfoRecord1.OwnerAddr_ID = (string) null;
                  tenantInfoRecord1.SvcSendAddr_ID = (string) null;
                  tenantInfoRecord1.SvcRecAddr_ID = (string) null;
                  tenantInfoRecord1.NumTenants = 0;
                  tenantInfoRecord1.Tax_ID = (string) null;
                  tenantInfoRecord1.AutoBill = false;
                  tenantInfoRecord1.BillNo = (string) null;
                  tenantInfoRecord1.KZU_Law = false;
                  tenantInfoRecord1.ChangeCharge = false;
                  tenantInfoRecord1.DriveCharge = false;
                  tenantInfoRecord1.BigLetter = false;
                  tenantInfoRecord1.BankAccount = (string) null;
                  tenantInfoRecord1.Account_Entity = (string) null;
                  tenantInfoRecord1.Bill_Unit = (string) null;
                  tenantInfoRecord1.SortField = (string) null;
                  tenantInfoRecord1.OwnerNum = (string) null;
                  tenantInfoRecord1.Create_Date = this._structureNodeRepository.FirstOrDefault((Expression<System.Func<StructureNode, bool>>) (s => s.EntityId == tenant.Id)).StartDate;
                  tenantInfoRecord1.Create_User = string.IsNullOrEmpty(tenant.CreatedBy) ? string.Empty : this._userRepository.GetById((object) new Guid(tenant.CreatedBy)).Username;
                  TenantInfoRecord tenantInfoRecord3 = tenantInfoRecord1;
                  DateTime? nullable3 = tenant.LastChangedOn;
                  DateTime minDateTime1 = MDMManager.MinDateTime;
                  DateTime? nullable4;
                  if ((nullable3.HasValue ? (nullable3.HasValue ? (nullable3.GetValueOrDefault() == minDateTime1 ? 1 : 0) : 1) : 0) == 0)
                  {
                    nullable4 = tenant.LastChangedOn;
                  }
                  else
                  {
                    nullable3 = new DateTime?();
                    nullable4 = nullable3;
                  }
                  tenantInfoRecord3.Update_Date = nullable4;
                  tenantInfoRecord1.Update_User = string.IsNullOrEmpty(tenant.UpdatedBy) ? string.Empty : this._userRepository.GetById((object) new Guid(tenant.UpdatedBy)).Username;
                  tenantInfoRecord1.IsConfig = false;
                  tenantInfoRecord1.IsActive = !tenant.IsDeactivated;
                  TenantInfoRecord recordObject = tenantInfoRecord1;
                  data.Add(recordObject);
                  MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "Tenant Info PK: " + recordObject.Project_ID + ", " + recordObject.UnitNbr + ", " + recordObject.Tenant_ID + ", " + (object) recordObject.Valid_From);
                  string propNameColumns;
                  string propValuesColumns;
                  this.GetPropertiesNameAndValue<TenantInfoRecord>(recordObject, out propNameColumns, out propValuesColumns);
                  MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, propNameColumns);
                  MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, propValuesColumns);
                }
                DataTable dataTable = data.ToDataTable<TenantInfoRecord>();
                dataTable.TableName = "TenantInfoRecordDTName";
                Service_MDMS_DataClient serviceMdmsDataClient = this.ServiceMdmsDataClient(mdmConfig);
                string msg;
                serviceMdmsDataClient.SaveTenantInfoRecord(mdmConfig.MDMUser, mdmConfig.MDMPassword, mdmConfig.Company.ToString(), mdmConfig.CustomerNumber, dataTable, out msg);
                MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, msg);
                serviceMdmsDataClient.Close();
                foreach (Tenant entity in (IEnumerable<Tenant>) grouping)
                {
                  entity.LastTenantInfoMDMExportOn = new DateTime?(DateTime.Now);
                  this._tenantRepository.Update(entity);
                }
              }
              catch (Exception ex)
              {
                MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, ex.ToString());
              }
            }
          }
          else
            MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, keyValuePair.Key.Name + "missing from MDM config. Tenants for this country were not exported!");
        }
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "-------------------------------End Save Tenant Info Record ------------------------------------");
      }
      catch (Exception ex)
      {
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, ex.ToString());
      }
    }

    private string GetTenantNrForUnitNbr(int tenantNr)
    {
      int length = tenantNr.ToString().Length;
      return "000000".Remove(6 - length, length) + (object) tenantNr;
    }

    public void SaveTenantFlatRecord()
    {
      try
      {
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "-------------------------------Begin Save Tenant Flat Record ------------------------------------");
        List<Tenant> list = this._tenantRepository.SearchFor((Expression<System.Func<Tenant, bool>>) (l => l.LastTenantFlatMDMExportOn == new DateTime?() || l.LastTenantFlatMDMExportOn != new DateTime?() && l.LastChangedOn >= l.LastTenantFlatMDMExportOn)).ToList<Tenant>();
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "Number of tenants: " + (object) list.Count);
        foreach (KeyValuePair<Country, List<Tenant>> keyValuePair in this.GroupTenantsByCountry(list))
        {
          Guid countryID = keyValuePair.Key.Id;
          MDMConfigs mdmConfig = this._mdmConfigsRepository.FirstOrDefault((Expression<System.Func<MDMConfigs, bool>>) (m => m.Country.Id == countryID));
          if (mdmConfig != null)
          {
            MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, "Tenant Flat Record( Enterprise_Id:" + (object) mdmConfig.Company + ")");
            foreach (IGrouping<int, Tenant> grouping in keyValuePair.Value.Select((x, index) => new
            {
              x = x,
              index = index
            }).GroupBy(x => x.index / Convert.ToInt32(this.batchSize), y => y.x))
            {
              try
              {
                List<TenantFlatRecord> data = new List<TenantFlatRecord>();
                foreach (Tenant tenant1 in (IEnumerable<Tenant>) grouping)
                {
                  Tenant tenant = tenant1;
                  MSS.Core.Model.Structures.Location locationForTenant = this.GetLocationForTenant(tenant);
                  string tenantNrForUnitNbr = this.GetTenantNrForUnitNbr(tenant.TenantNr);
                  TenantFlatRecord tenantFlatRecord1 = new TenantFlatRecord();
                  tenantFlatRecord1.Project_ID = locationForTenant?.BuildingNr;
                  tenantFlatRecord1.Valid_From = this._structureNodeRepository.FirstOrDefault((Expression<System.Func<StructureNode, bool>>) (s => s.EntityId == tenant.Id)).StartDate;
                  TenantFlatRecord tenantFlatRecord2 = tenantFlatRecord1;
                  DateTime? nullable1;
                  if (!tenant.IsDeactivated)
                  {
                    nullable1 = new DateTime?(MDMManager.ValidToDate);
                  }
                  else
                  {
                    DateTime? nullable2 = tenant.LastChangedOn;
                    DateTime minDateTime = MDMManager.MinDateTime;
                    if ((nullable2.HasValue ? (nullable2.HasValue ? (nullable2.GetValueOrDefault() == minDateTime ? 1 : 0) : 1) : 0) == 0)
                    {
                      nullable1 = tenant.LastChangedOn;
                    }
                    else
                    {
                      nullable2 = new DateTime?();
                      nullable1 = nullable2;
                    }
                  }
                  tenantFlatRecord2.Valid_To = nullable1;
                  tenantFlatRecord1.UnitNbr = tenantNrForUnitNbr;
                  tenantFlatRecord1.ResidentId = tenant.TenantNr.ToString();
                  tenantFlatRecord1.Cust_UnitID = tenant.CustomerTenantNo;
                  tenantFlatRecord1.MasterCustId = (string) null;
                  tenantFlatRecord1.User7 = (string) null;
                  tenantFlatRecord1.NextUnit = (string) null;
                  tenantFlatRecord1.UpperUnit = (string) null;
                  tenantFlatRecord1.Floor = tenant.FloorNr;
                  tenantFlatRecord1.Floor_Pos = tenant.ApartmentNr;
                  tenantFlatRecord1.FloorHost = string.Format("{0}/{1}/{2}/{3}", (object) tenant.FloorNr, (object) tenant.FloorName, (object) tenant.ApartmentNr, (object) tenant.Direction);
                  tenantFlatRecord1.Address_ID = (string) null;
                  tenantFlatRecord1.AreaSize = 0.0;
                  tenantFlatRecord1.AreaSize2_Warm = 0.0;
                  tenantFlatRecord1.AreaSize_B = 0.0;
                  tenantFlatRecord1.AreaSize2 = 0.0;
                  tenantFlatRecord1.AreaSize_Warm = 0.0;
                  tenantFlatRecord1.AreaSize_Indu = 0.0;
                  tenantFlatRecord1.VolumeSize = 0.0;
                  tenantFlatRecord1.VolumeSize_Warm = 0.0;
                  tenantFlatRecord1.AirFlow = 0.0;
                  tenantFlatRecord1.NumTenants = 0.0;
                  tenantFlatRecord1.OwnerShare = 0.0;
                  tenantFlatRecord1.Percent = 0.0;
                  tenantFlatRecord1.Garbage = 0.0;
                  tenantFlatRecord1.Interest = 0.0;
                  tenantFlatRecord1.GigaCalorie = 0.0;
                  tenantFlatRecord1.Joule = 0.0;
                  tenantFlatRecord1.Watt_KW = 0.0;
                  tenantFlatRecord1.Watt_MW = 0.0;
                  tenantFlatRecord1.Watt = 0.0;
                  tenantFlatRecord1.Scale = 0.0;
                  tenantFlatRecord1.SvcRec_AddrID = (string) null;
                  tenantFlatRecord1.SvcSend_AddrID = (string) null;
                  tenantFlatRecord1.InComplete = (byte) 0;
                  tenantFlatRecord1.IsOpen = (byte) 0;
                  tenantFlatRecord1.IsAllow = (byte) 0;
                  tenantFlatRecord1.Analyze = (byte) 0;
                  tenantFlatRecord1.DisKey1 = 0.0;
                  tenantFlatRecord1.DisKey2 = 0.0;
                  tenantFlatRecord1.DisKey3 = 0.0;
                  tenantFlatRecord1.DisKey4 = 0.0;
                  tenantFlatRecord1.DisKey5 = 0.0;
                  tenantFlatRecord1.DisKey6 = 0.0;
                  tenantFlatRecord1.DisKey7 = 0.0;
                  tenantFlatRecord1.DisKey8 = 0.0;
                  tenantFlatRecord1.Create_Date = this._structureNodeRepository.FirstOrDefault((Expression<System.Func<StructureNode, bool>>) (s => s.EntityId == tenant.Id)).StartDate;
                  tenantFlatRecord1.Create_User = string.IsNullOrEmpty(tenant.CreatedBy) ? string.Empty : this._userRepository.GetById((object) new Guid(tenant.CreatedBy)).Username;
                  TenantFlatRecord tenantFlatRecord3 = tenantFlatRecord1;
                  DateTime? nullable3 = tenant.LastChangedOn;
                  DateTime minDateTime1 = MDMManager.MinDateTime;
                  DateTime? nullable4;
                  if ((nullable3.HasValue ? (nullable3.HasValue ? (nullable3.GetValueOrDefault() == minDateTime1 ? 1 : 0) : 1) : 0) == 0)
                  {
                    nullable4 = tenant.LastChangedOn;
                  }
                  else
                  {
                    nullable3 = new DateTime?();
                    nullable4 = nullable3;
                  }
                  tenantFlatRecord3.Update_Date = nullable4;
                  tenantFlatRecord1.Update_User = string.IsNullOrEmpty(tenant.UpdatedBy) ? string.Empty : this._userRepository.GetById((object) new Guid(tenant.UpdatedBy)).Username;
                  tenantFlatRecord1.IsConfig = false;
                  tenantFlatRecord1.IsActive = !tenant.IsDeactivated;
                  TenantFlatRecord recordObject = tenantFlatRecord1;
                  data.Add(recordObject);
                  MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "Tenant Flat PK: " + recordObject.UnitNbr + ", " + (object) recordObject.Valid_From);
                  string propNameColumns;
                  string propValuesColumns;
                  this.GetPropertiesNameAndValue<TenantFlatRecord>(recordObject, out propNameColumns, out propValuesColumns);
                  MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, propNameColumns);
                  MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, propValuesColumns);
                }
                DataTable dataTable = data.ToDataTable<TenantFlatRecord>();
                dataTable.TableName = "TenantFlatRecordDTName";
                Service_MDMS_DataClient serviceMdmsDataClient = this.ServiceMdmsDataClient(mdmConfig);
                string msg;
                serviceMdmsDataClient.SaveTenantFlatRecord(mdmConfig.MDMUser, mdmConfig.MDMPassword, mdmConfig.Company.ToString(), mdmConfig.CustomerNumber, dataTable, out msg);
                MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, msg);
                serviceMdmsDataClient.Close();
                foreach (Tenant entity in (IEnumerable<Tenant>) grouping)
                {
                  entity.LastTenantFlatMDMExportOn = new DateTime?(DateTime.Now);
                  this._tenantRepository.Update(entity);
                }
              }
              catch (Exception ex)
              {
                MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, ex.ToString());
              }
            }
          }
          else
            MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, keyValuePair.Key.Name + "missing from MDM config. Tenants for this country were not exported!");
        }
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "-------------------------------End Save Tenant Flat Record ------------------------------------");
      }
      catch (Exception ex)
      {
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, ex.ToString());
      }
    }

    public void SaveDeviceInfoRecord()
    {
      try
      {
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "-------------------------------Begin Save Device Info Record ------------------------------------");
        for (IList<Meter> inFixedStructure = this.GetMeterInFixedStructure(Convert.ToInt32(this.batchSize)); inFixedStructure.Count > 0; inFixedStructure = this.GetMeterInFixedStructure(Convert.ToInt32(this.batchSize)))
        {
          MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "Number of devices: " + (object) inFixedStructure.Count);
          Dictionary<Country, List<Meter>> dictionary = this.GroupMetersByCountry(inFixedStructure);
          SortedList<int, string> roomTable = this.CreateRoomTable();
          foreach (KeyValuePair<Country, List<Meter>> keyValuePair in dictionary)
          {
            Guid countryID = keyValuePair.Key.Id;
            MDMConfigs mdmConfig = this._mdmConfigsRepository.FirstOrDefault((Expression<System.Func<MDMConfigs, bool>>) (m => m.Country.Id == countryID));
            if (mdmConfig != null)
            {
              MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, "Device Info Record( Enterprise_Id:" + (object) mdmConfig.Company + ")");
              foreach (IGrouping<int, Meter> source in keyValuePair.Value.Select((x, index) => new
              {
                x = x,
                index = index
              }).GroupBy(x => x.index / Convert.ToInt32(this.batchSize), y => y.x))
              {
                try
                {
                  List<DeviceInfoRecord> data = new List<DeviceInfoRecord>();
                  Dictionary<Guid, MSS.Core.Model.Structures.Location> locationsForMeters = this._repositoryFactory.GetStructureNodeRepository().GetLocationsForMeters(source.Select<Meter, Guid>((System.Func<Meter, Guid>) (m => m.Id)).ToList<Guid>());
                  foreach (Meter meter1 in (IEnumerable<Meter>) source)
                  {
                    Meter meter = meter1;
                    object tenantGuid = this.GetParentEntity(meter.Id);
                    Tenant tenant = this._tenantRepository.FirstOrDefault((Expression<System.Func<Tenant, bool>>) (t => t.Id == (Guid) tenantGuid));
                    if (tenant != null)
                    {
                      MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "Device info for serial: " + meter.SerialNumber);
                      MSS.Core.Model.Structures.Location location = locationsForMeters.ContainsKey(meter.Id) ? locationsForMeters[meter.Id] : (MSS.Core.Model.Structures.Location) null;
                      DeviceInfoRecord deviceInfoRecord1 = new DeviceInfoRecord();
                      deviceInfoRecord1.Project_ID = location?.BuildingNr;
                      deviceInfoRecord1.MTU_ID = meter.SerialNumber;
                      deviceInfoRecord1.Valid_From = this._structureNodeRepository.FirstOrDefault((Expression<System.Func<StructureNode, bool>>) (s => s.EntityId == meter.Id)).StartDate;
                      DeviceInfoRecord deviceInfoRecord2 = deviceInfoRecord1;
                      DateTime? nullable1;
                      if (!meter.IsDeactivated)
                      {
                        nullable1 = new DateTime?(MDMManager.ValidToDate);
                      }
                      else
                      {
                        DateTime? nullable2 = meter.LastChangedOn;
                        DateTime minDateTime = MDMManager.MinDateTime;
                        if ((nullable2.HasValue ? (nullable2.HasValue ? (nullable2.GetValueOrDefault() == minDateTime ? 1 : 0) : 1) : 0) == 0)
                        {
                          nullable1 = meter.LastChangedOn;
                        }
                        else
                        {
                          nullable2 = new DateTime?();
                          nullable1 = nullable2;
                        }
                      }
                      deviceInfoRecord2.Valid_To = nullable1;
                      deviceInfoRecord1.Meter_ID = meter.SerialNumber;
                      DeviceInfoRecord deviceInfoRecord3 = deviceInfoRecord1;
                      int num1 = tenant.TenantNr;
                      string str1 = num1.ToString();
                      deviceInfoRecord3.UnitNbr = str1;
                      DeviceInfoRecord deviceInfoRecord4 = deviceInfoRecord1;
                      num1 = this.SetKeyRoom(meter, roomTable);
                      string str2 = num1.ToString();
                      deviceInfoRecord4.KeyRoom = str2;
                      deviceInfoRecord1.Location = (string) null;
                      deviceInfoRecord1.InstallPosition = (string) null;
                      deviceInfoRecord1.InstallPoint = (string) null;
                      deviceInfoRecord1.MeaPoint_Minol = meter.SerialNumber;
                      deviceInfoRecord1.Channel = meter.DeviceType == DeviceTypeEnum.MinotelContactRadio3 ? (meter.Channel != null ? meter.Channel.Code : (string) null) : (string) null;
                      deviceInfoRecord1.ImpulseValue = meter.StartValueImpulses;
                      deviceInfoRecord1.EstimateCode = (string) null;
                      deviceInfoRecord1.EstimateValue = 0.0;
                      deviceInfoRecord1.CartridgeHeater = false;
                      deviceInfoRecord1.Factor = 0.0f;
                      deviceInfoRecord1.Factor2 = 0.0f;
                      deviceInfoRecord1.DeviceState = (string) null;
                      deviceInfoRecord1.InstallDate = new DateTime?();
                      deviceInfoRecord1.DeviceID_Rep = (string) null;
                      deviceInfoRecord1.FinalScore_Rep = 0.0;
                      deviceInfoRecord1.Contract_No = (string) null;
                      deviceInfoRecord1.Contract_Position = 0;
                      deviceInfoRecord1.Rent_Contract = (string) null;
                      deviceInfoRecord1.Rent_Contract_Pos = 0;
                      deviceInfoRecord1.Type_Host = '0';
                      deviceInfoRecord1.User1 = (string) null;
                      deviceInfoRecord1.User2 = (string) null;
                      deviceInfoRecord1.User3 = (string) null;
                      deviceInfoRecord1.User4 = (string) null;
                      deviceInfoRecord1.User5 = (string) null;
                      deviceInfoRecord1.RadioData = 0.0;
                      deviceInfoRecord1.UnityImpulse = meter.ImpulsUnit == null || string.IsNullOrEmpty(meter.ImpulsUnit.CelestaCode) ? 0 : Convert.ToInt32(meter.ImpulsUnit.CelestaCode.Substring(2));
                      deviceInfoRecord1.ImpulseNum = meter.NrOfImpulses;
                      DeviceInfoRecord deviceInfoRecord5 = deviceInfoRecord1;
                      double? startValue = meter.StartValue;
                      double num2;
                      if (!startValue.HasValue)
                      {
                        num2 = 0.0;
                      }
                      else
                      {
                        startValue = meter.StartValue;
                        num2 = startValue.Value;
                      }
                      deviceInfoRecord5.StartImpulse = num2;
                      deviceInfoRecord1.ModuleUsed = false;
                      deviceInfoRecord1.Module_ID = (string) null;
                      deviceInfoRecord1.IsHeatMeter = false;
                      deviceInfoRecord1.RegMode1 = meter.MeterRadioDetailsList.Count > 0 ? this.GetReg(meter.MeterRadioDetailsList[0].dgReg1Mode) : string.Empty;
                      deviceInfoRecord1.RegMode2 = meter.MeterRadioDetailsList.Count > 0 ? this.GetReg(meter.MeterRadioDetailsList[0].dgReg2Mode) : string.Empty;
                      deviceInfoRecord1.RegMode3 = meter.MeterRadioDetailsList.Count > 0 ? this.GetReg(meter.MeterRadioDetailsList[0].dgReg3Mode) : string.Empty;
                      deviceInfoRecord1.DgReg1 = meter.MeterRadioDetailsList.Count <= 0 || string.IsNullOrWhiteSpace(meter.MeterRadioDetailsList[0].dgReg1Flag) ? 0 : Convert.ToInt32(this.GetReg(meter.MeterRadioDetailsList[0].dgReg1Flag));
                      deviceInfoRecord1.DgReg2 = meter.MeterRadioDetailsList.Count <= 0 || string.IsNullOrWhiteSpace(meter.MeterRadioDetailsList[0].dgReg2Flag) ? 0 : Convert.ToInt32(this.GetReg(meter.MeterRadioDetailsList[0].dgReg2Flag));
                      deviceInfoRecord1.DgReg3 = meter.MeterRadioDetailsList.Count <= 0 || string.IsNullOrWhiteSpace(meter.MeterRadioDetailsList[0].dgReg3Flag) ? 0 : Convert.ToInt32(this.GetReg(meter.MeterRadioDetailsList[0].dgReg3Flag));
                      deviceInfoRecord1.RegSignal1 = meter.MeterRadioDetailsList.Count <= 0 || string.IsNullOrWhiteSpace(meter.MeterRadioDetailsList[0].dgReg1Signal) ? 0 : Convert.ToInt32(meter.MeterRadioDetailsList[0].dgReg1Signal);
                      deviceInfoRecord1.RegSignal2 = meter.MeterRadioDetailsList.Count <= 0 || string.IsNullOrWhiteSpace(meter.MeterRadioDetailsList[0].dgReg2Signal) ? 0 : Convert.ToInt32(meter.MeterRadioDetailsList[0].dgReg2Signal);
                      deviceInfoRecord1.RegSignal3 = meter.MeterRadioDetailsList.Count <= 0 || string.IsNullOrWhiteSpace(meter.MeterRadioDetailsList[0].dgReg3Signal) ? 0 : Convert.ToInt32(meter.MeterRadioDetailsList[0].dgReg3Signal);
                      deviceInfoRecord1.DCU_ID1 = meter.MeterRadioDetailsList.Count > 0 ? meter.MeterRadioDetailsList[0].dgReg1DakonSerNr : (string) null;
                      deviceInfoRecord1.DCU_ID2 = meter.MeterRadioDetailsList.Count > 0 ? meter.MeterRadioDetailsList[0].dgReg2DakonSerNr : (string) null;
                      deviceInfoRecord1.DCU_ID3 = meter.MeterRadioDetailsList.Count > 0 ? meter.MeterRadioDetailsList[0].dgReg3DakonSernr : (string) null;
                      deviceInfoRecord1.DiagnosticMsg = location != null ? location.Street : string.Empty;
                      deviceInfoRecord1.ConfigFlag = ((int) meter.IsConfigured ?? 0) != 0 ? 1 : 0;
                      deviceInfoRecord1.DgScenario = location != null ? location.Scenario.Code : 0;
                      deviceInfoRecord1.DgMeasureArea = meter.MeterRadioDetailsList.Count > 0 ? meter.MeterRadioDetailsList[0].dgMessbereich : string.Empty;
                      deviceInfoRecord1.Full_DeviceID = meter.MeterRadioDetailsList.Count > 0 ? meter.MeterRadioDetailsList[0].dgZaehlerNr : string.Empty;
                      deviceInfoRecord1.Short_ID = meter.ShortDeviceNo;
                      deviceInfoRecord1.Create_Date = this._structureNodeRepository.FirstOrDefault((Expression<System.Func<StructureNode, bool>>) (s => s.EntityId == meter.Id)).StartDate;
                      deviceInfoRecord1.Create_User = string.IsNullOrEmpty(meter.CreatedBy) ? string.Empty : this._userRepository.GetById((object) new Guid(meter.CreatedBy)).Username;
                      deviceInfoRecord1.Update_Date = meter.LastChangedOn;
                      deviceInfoRecord1.Update_User = string.IsNullOrEmpty(meter.UpdatedBy) ? string.Empty : this._userRepository.GetById((object) new Guid(meter.UpdatedBy)).Username;
                      deviceInfoRecord1.IsConfig = false;
                      deviceInfoRecord1.IsActive = !meter.IsDeactivated;
                      deviceInfoRecord1.Device_Type = meter.DeviceType.GetMDMCodes();
                      deviceInfoRecord1.Device_Product = (string) null;
                      DeviceInfoRecord recordObject = deviceInfoRecord1;
                      data.Add(recordObject);
                      MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "Device PK: " + recordObject.Project_ID + ", " + recordObject.MTU_ID + ", " + (object) recordObject.Valid_From);
                      string propNameColumns;
                      string propValuesColumns;
                      this.GetPropertiesNameAndValue<DeviceInfoRecord>(recordObject, out propNameColumns, out propValuesColumns);
                      MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, propNameColumns);
                      MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, propValuesColumns);
                    }
                  }
                  DataTable dataTable = data.ToDataTable<DeviceInfoRecord>();
                  dataTable.TableName = "DeviceInfoRecordDTName";
                  Service_MDMS_DataClient serviceMdmsDataClient = this.ServiceMdmsDataClient(mdmConfig);
                  string msg;
                  serviceMdmsDataClient.SaveDeviceInfoRecord(mdmConfig.MDMUser, mdmConfig.MDMPassword, mdmConfig.Company.ToString(), mdmConfig.CustomerNumber, dataTable, out msg);
                  MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, msg);
                  serviceMdmsDataClient.Close();
                  this._repositoryFactory.GetSession().FlushMode = FlushMode.Commit;
                  this._repositoryFactory.GetSession().BeginTransaction();
                  foreach (Meter entity in (IEnumerable<Meter>) source)
                  {
                    entity.LastMDMExportOn = new DateTime?(DateTime.Now);
                    this._meterRepository.TransactionalUpdate(entity);
                  }
                  this._repositoryFactory.GetSession().Transaction.Commit();
                }
                catch (Exception ex)
                {
                  MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, ex.ToString());
                }
              }
            }
            else
              MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, keyValuePair.Key.Name + "missing from MDM config. Meters for this country were not exported!");
          }
          MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "-------------------------------End Save Device Info Record ------------------------------------");
        }
      }
      catch (Exception ex)
      {
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, ex.ToString());
      }
    }

    public void SaveReadDataRecord()
    {
      try
      {
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "-------------------------------Begin Save Read Data Record ------------------------------------");
        bool flag1 = this._repositoryFactory.GetRepository<MeterReadingValue>().Exists((Expression<System.Func<MeterReadingValue, bool>>) (r => (r.MDMExportedOn == new DateTime?() || r.MDMExportedOn != new DateTime?() && (DateTime?) r.CreatedOn >= r.MDMExportedOn && r.MDMExportedOn > (DateTime?) MDMManager.MinDateTime) && !(r.PhysicalQuantity == 18L || r.PhysicalQuantity == 1L && r.MeterType == 64L)));
        bool flag2 = false;
        IList<Country> all = this._repositoryFactory.GetRepository<Country>().GetAll();
        if (all.Count > 1)
          flag2 = true;
        IRepository<MeterReadingValue> repository;
        Expression<System.Func<MeterReadingValue, bool>> predicate;
        for (; flag1; flag1 = repository.Exists(predicate))
        {
          NHibernate.ISession session = this._repositoryFactory.GetSession();
          session.FlushMode = FlushMode.Commit;
          session.BeginTransaction();
          List<MeterReadingValue> list = session.Query<MeterReadingValue>().Where<MeterReadingValue>((Expression<System.Func<MeterReadingValue, bool>>) (r => (r.MDMExportedOn == new DateTime?() || r.MDMExportedOn != new DateTime?() && (DateTime?) r.CreatedOn >= r.MDMExportedOn && r.MDMExportedOn > (DateTime?) MDMManager.MinDateTime) && !(r.PhysicalQuantity == 18L || r.PhysicalQuantity == 1L && r.MeterType == 64L))).Take<MeterReadingValue>(500).ToList<MeterReadingValue>();
          MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "Number of read data records: " + (object) list.Count);
          Dictionary<Country, List<MeterReadingValue>> dictionary;
          if (!flag2)
            dictionary = new Dictionary<Country, List<MeterReadingValue>>()
            {
              {
                all[0],
                list
              }
            };
          else
            dictionary = this.GroupMeterReadingValuesByCountry(list);
          foreach (KeyValuePair<Country, List<MeterReadingValue>> keyValuePair in dictionary)
          {
            Guid countryID = keyValuePair.Key.Id;
            MDMConfigs mdmConfig = this._mdmConfigsRepository.FirstOrDefault((Expression<System.Func<MDMConfigs, bool>>) (m => m.Country.Id == countryID));
            if (mdmConfig != null)
            {
              MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, "Read Data Record( Enterprise_Id:" + (object) mdmConfig.Company + ")");
              foreach (IGrouping<int, MeterReadingValue> grouping in keyValuePair.Value.Select((x, index) => new
              {
                x = x,
                index = index
              }).GroupBy(x => x.index / Convert.ToInt32(this.batchSize), y => y.x))
              {
                try
                {
                  List<ReadDataRecord> data = new List<ReadDataRecord>();
                  foreach (MeterReadingValue entity in (IEnumerable<MeterReadingValue>) grouping)
                  {
                    ReadDataRecord recordObject = new ReadDataRecord()
                    {
                      Meter_ID = entity.MeterSerialNumber,
                      Read_Type = 4264650L.ToString((IFormatProvider) CultureInfo.InvariantCulture),
                      Read_Time = entity.Date,
                      MRead = entity.Value,
                      User1 = "ok",
                      User2 = (string) null,
                      User3 = (string) null,
                      User4 = (string) null,
                      User5 = (string) null,
                      User6 = (string) null,
                      Create_Date = entity.CreatedOn,
                      Create_Prog = "M"
                    };
                    data.Add(recordObject);
                    MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "Meter Reading Value PK: " + recordObject.Meter_ID);
                    string propNameColumns;
                    string propValuesColumns;
                    this.GetPropertiesNameAndValue<ReadDataRecord>(recordObject, out propNameColumns, out propValuesColumns);
                    MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, propNameColumns);
                    MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, propValuesColumns);
                    entity.MDMExportedOn = new DateTime?(DateTime.Now);
                    this._meterReadingValueRepository.TransactionalUpdate(entity);
                  }
                  DataTable dataTable = data.ToDataTable<ReadDataRecord>();
                  dataTable.TableName = "ReadDataRecordDTName";
                  Service_MDMS_DataClient serviceMdmsDataClient = this.ServiceMdmsDataClient(mdmConfig);
                  string msg;
                  serviceMdmsDataClient.SaveReadDataRecord(mdmConfig.MDMUser, mdmConfig.MDMPassword, mdmConfig.Company.ToString(), mdmConfig.CustomerNumber, dataTable, out msg);
                  MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, msg);
                  serviceMdmsDataClient.Close();
                }
                catch (Exception ex)
                {
                  MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, ex.ToString());
                }
              }
            }
            else
              MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, keyValuePair.Key.Name + "missing from MDM config. Tenants for this country were not exported!");
          }
          session.Transaction.Commit();
          repository = this._repositoryFactory.GetRepository<MeterReadingValue>();
          predicate = (Expression<System.Func<MeterReadingValue, bool>>) (r => r.MDMExportedOn == new DateTime?() || r.MDMExportedOn != new DateTime?() && (DateTime?) r.CreatedOn >= r.MDMExportedOn);
        }
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "-------------------------------End Save Read Data Record ------------------------------------");
      }
      catch (Exception ex)
      {
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, ex.ToString());
      }
    }

    public void SaveAddressRecord()
    {
      try
      {
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "-------------------------------Begin Save Address Record ------------------------------------");
        List<MSS.Core.Model.Structures.Location> list = this._locationRepository.SearchFor((Expression<System.Func<MSS.Core.Model.Structures.Location, bool>>) (l => l.LastAddressMDMExportOn == new DateTime?() || l.LastAddressMDMExportOn != new DateTime?() && l.LastUpdateBuildingNo >= l.LastAddressMDMExportOn)).ToList<MSS.Core.Model.Structures.Location>();
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "Number of locations: " + (object) list.Count);
        foreach (IGrouping<Country, MSS.Core.Model.Structures.Location> source in list.GroupBy<MSS.Core.Model.Structures.Location, Country>((System.Func<MSS.Core.Model.Structures.Location, Country>) (l => l.Country)))
        {
          Guid countryID = source.Key.Id;
          MDMConfigs mdmConfig = this._mdmConfigsRepository.FirstOrDefault((Expression<System.Func<MDMConfigs, bool>>) (m => m.Country.Id == countryID));
          if (mdmConfig != null)
          {
            MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, "Address Record( Enterprise_Id:" + (object) mdmConfig.Company + ")");
            foreach (IGrouping<int, MSS.Core.Model.Structures.Location> grouping in source.Select((x, index) => new
            {
              x = x,
              index = index
            }).GroupBy(x => x.index / Convert.ToInt32(this.batchSize), y => y.x))
            {
              try
              {
                List<AddressRecord> data = new List<AddressRecord>();
                foreach (MSS.Core.Model.Structures.Location location1 in (IEnumerable<MSS.Core.Model.Structures.Location>) grouping)
                {
                  MSS.Core.Model.Structures.Location location = location1;
                  AddressRecord addressRecord1 = new AddressRecord();
                  addressRecord1.Address_ID = location.BuildingNr;
                  addressRecord1.Valid_From = this._structureNodeRepository.FirstOrDefault((Expression<System.Func<StructureNode, bool>>) (s => s.EntityId == location.Id)).StartDate;
                  AddressRecord addressRecord2 = addressRecord1;
                  DateTime? nullable1;
                  if (!location.IsDeactivated)
                  {
                    nullable1 = new DateTime?(MDMManager.ValidToDate);
                  }
                  else
                  {
                    DateTime? nullable2 = location.LastUpdateBuildingNo;
                    DateTime minDateTime = MDMManager.MinDateTime;
                    if ((nullable2.HasValue ? (nullable2.HasValue ? (nullable2.GetValueOrDefault() == minDateTime ? 1 : 0) : 1) : 0) == 0)
                    {
                      nullable1 = location.LastUpdateBuildingNo;
                    }
                    else
                    {
                      nullable2 = new DateTime?();
                      nullable1 = nullable2;
                    }
                  }
                  addressRecord2.Valid_To = nullable1;
                  addressRecord1.Version = '0';
                  addressRecord1.Title = (string) null;
                  addressRecord1.Name1 = string.Format("{0} {1}", (object) location.Street.Trim(), (object) location.City.Trim());
                  addressRecord1.Name2 = (string) null;
                  addressRecord1.Name3 = (string) null;
                  addressRecord1.Name4 = (string) null;
                  addressRecord1.Name_Text = (string) null;
                  addressRecord1.Name_Co = (string) null;
                  addressRecord1.City1 = location.City;
                  addressRecord1.City2 = (string) null;
                  addressRecord1.Home_City = (string) null;
                  addressRecord1.Post_Code1 = location.ZipCode;
                  addressRecord1.Post_Code2 = (string) null;
                  addressRecord1.Post_Code3 = (string) null;
                  addressRecord1.PO_Box = (string) null;
                  addressRecord1.Street = location.Street;
                  addressRecord1.Street_Abbr = (string) null;
                  addressRecord1.House_Num1 = (string) null;
                  addressRecord1.House_Num2 = (string) null;
                  addressRecord1.House_Num3 = (string) null;
                  addressRecord1.Str_Suppl1 = (string) null;
                  addressRecord1.Str_Suppl2 = (string) null;
                  addressRecord1.Str_Suppl3 = (string) null;
                  addressRecord1.Location = (string) null;
                  addressRecord1.Building = (string) null;
                  addressRecord1.Room_Num = (string) null;
                  addressRecord1.Floor = (string) null;
                  addressRecord1.Region = (string) null;
                  addressRecord1.Country = (string) null;
                  addressRecord1.Language = (string) null;
                  addressRecord1.Phone = (string) null;
                  addressRecord1.Fax = (string) null;
                  addressRecord1.Email = (string) null;
                  addressRecord1.Time_Zone = (string) null;
                  AddressRecord recordObject = addressRecord1;
                  data.Add(recordObject);
                  MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "Address PK: " + recordObject.Address_ID);
                  string propNameColumns;
                  string propValuesColumns;
                  this.GetPropertiesNameAndValue<AddressRecord>(recordObject, out propNameColumns, out propValuesColumns);
                  MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, propNameColumns);
                  MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, propValuesColumns);
                }
                DataTable dataTable = data.ToDataTable<AddressRecord>();
                dataTable.TableName = "AddressRecordDTName";
                Service_MDMS_DataClient serviceMdmsDataClient = this.ServiceMdmsDataClient(mdmConfig);
                string msg;
                serviceMdmsDataClient.SaveAddressRecord(mdmConfig.MDMUser, mdmConfig.MDMPassword, mdmConfig.Company.ToString(), mdmConfig.CustomerNumber, dataTable, out msg);
                MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, msg);
                serviceMdmsDataClient.Close();
                foreach (MSS.Core.Model.Structures.Location entity in (IEnumerable<MSS.Core.Model.Structures.Location>) grouping)
                {
                  entity.LastAddressMDMExportOn = new DateTime?(DateTime.Now);
                  this._locationRepository.Update(entity);
                }
              }
              catch (Exception ex)
              {
                MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, ex.ToString());
              }
            }
          }
          else
            MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, source.Key.Name + "missing from MDM config. Tenants for this country were not exported!");
        }
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "-------------------------------End Save Address Record ------------------------------------");
      }
      catch (Exception ex)
      {
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, ex.ToString());
      }
    }

    public void SaveDCUInfoRecord()
    {
      try
      {
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "-------------------------------Begin Save DCU Info Record ------------------------------------");
        List<MSS.Core.Model.DataCollectors.Minomat> list = this._minomatRepository.SearchFor((Expression<System.Func<MSS.Core.Model.DataCollectors.Minomat, bool>>) (l => l.LastDCUInfoMDMExportOn == new DateTime?() || l.LastDCUInfoMDMExportOn != new DateTime?() && l.LastChangedOn >= l.LastDCUInfoMDMExportOn)).ToList<MSS.Core.Model.DataCollectors.Minomat>();
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "Number of minomats: " + (object) list.Count);
        foreach (KeyValuePair<Country, List<MSS.Core.Model.DataCollectors.Minomat>> keyValuePair in this.GroupMinomatsByCountry(list))
        {
          Guid countryID = keyValuePair.Key.Id;
          MDMConfigs mdmConfig = this._mdmConfigsRepository.FirstOrDefault((Expression<System.Func<MDMConfigs, bool>>) (m => m.Country.Id == countryID));
          if (mdmConfig != null)
          {
            MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, "DCU Info Record( Enterprise_Id:" + (object) mdmConfig.Company + ")");
            foreach (IGrouping<int, MSS.Core.Model.DataCollectors.Minomat> source in keyValuePair.Value.Select((x, index) => new
            {
              x = x,
              index = index
            }).GroupBy(x => x.index / Convert.ToInt32(this.batchSize), y => y.x))
            {
              try
              {
                List<Guid?> masterGuids = source.Where<MSS.Core.Model.DataCollectors.Minomat>((System.Func<MSS.Core.Model.DataCollectors.Minomat, bool>) (item => !item.IsMaster)).Select<MSS.Core.Model.DataCollectors.Minomat, Guid?>((System.Func<MSS.Core.Model.DataCollectors.Minomat, Guid?>) (item => item.MinomatMasterId)).Distinct<Guid?>().ToList<Guid?>();
                Dictionary<Guid, string> dictionary = this._minomatRepository.Where((Expression<System.Func<MSS.Core.Model.DataCollectors.Minomat, bool>>) (item => masterGuids.Contains((Guid?) item.Id))).ToDictionary<MSS.Core.Model.DataCollectors.Minomat, Guid, string>((System.Func<MSS.Core.Model.DataCollectors.Minomat, Guid>) (item1 => item1.Id), (System.Func<MSS.Core.Model.DataCollectors.Minomat, string>) (item2 => item2.RadioId));
                List<DCUInfoRecord> data = new List<DCUInfoRecord>();
                foreach (MSS.Core.Model.DataCollectors.Minomat minomat1 in (IEnumerable<MSS.Core.Model.DataCollectors.Minomat>) source)
                {
                  MSS.Core.Model.DataCollectors.Minomat minomat = minomat1;
                  MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "Minomat Number: " + minomat.RadioId);
                  MinomatRadioDetails minomatRadioDetails = this._minomatRadioDetailsRepository.FirstOrDefault((Expression<System.Func<MinomatRadioDetails, bool>>) (m => m.Minomat.Id == minomat.Id));
                  MSS.Core.Model.Structures.Location locationForMinomat = this.GetLocationForMinomat(minomat);
                  MSS.Core.Model.Orders.Order orderForLocation = locationForMinomat != null ? this.GetOrderForLocation(locationForMinomat) : (MSS.Core.Model.Orders.Order) null;
                  long int64 = orderForLocation != null ? Convert.ToInt64(orderForLocation.InstallationNumber.Trim()) : 0L;
                  DateTime dateTime = new DateTime(1800, 1, 1);
                  DCUInfoRecord recordObject = new DCUInfoRecord()
                  {
                    Project_ID = locationForMinomat != null ? locationForMinomat.BuildingNr : string.Empty,
                    DCU_ID = minomat.RadioId,
                    Name = minomatRadioDetails != null ? minomatRadioDetails.Description : string.Empty,
                    Type = minomat.IsMaster ? "M" : "S",
                    SWVersion = string.Empty,
                    ModemVersion = string.Empty,
                    Location = minomatRadioDetails != null ? minomatRadioDetails.Location : string.Empty,
                    Entrance = minomatRadioDetails != null ? minomatRadioDetails.Entrance : string.Empty,
                    DistReg = minomatRadioDetails != null ? (Convert.ToBoolean(minomat.Registered) ? "1" : "0") : "0",
                    LastCall_In = minomatRadioDetails == null || !minomatRadioDetails.LastConnection.HasValue ? dateTime : minomatRadioDetails.LastConnection.Value,
                    NextCall_In = minomatRadioDetails == null || !minomatRadioDetails.LastConnection.HasValue ? dateTime : minomatRadioDetails.LastConnection.Value.AddDays(1.0),
                    PrimaryIP = minomat.HostAndPort,
                    SecondIP = minomat.Url,
                    Net_ID = minomatRadioDetails != null ? (string.IsNullOrEmpty(minomatRadioDetails.NetId) ? 0 : Convert.ToInt32(minomatRadioDetails.NetId.Trim())) : 0,
                    Node_ID = minomatRadioDetails != null ? (string.IsNullOrEmpty(minomatRadioDetails.NodeId) ? 0 : Convert.ToInt32(minomatRadioDetails.NodeId.Trim())) : 0,
                    Channel = minomatRadioDetails != null ? (string.IsNullOrEmpty(minomatRadioDetails.Channel) ? 0 : Convert.ToInt32(minomatRadioDetails.Channel.Trim())) : 0,
                    NumRegDevices = minomatRadioDetails != null ? (string.IsNullOrEmpty(minomatRadioDetails.NrOfRegisteredDevices) ? 0 : Convert.ToInt32(minomatRadioDetails.NrOfRegisteredDevices.Trim())) : 0,
                    NumRecDevices = minomatRadioDetails != null ? (string.IsNullOrEmpty(minomatRadioDetails.NrOfReceivedDevices) ? 0 : Convert.ToInt32(minomatRadioDetails.NrOfReceivedDevices.Trim())) : 0,
                    NumAssignDevices = minomatRadioDetails != null ? (string.IsNullOrEmpty(minomatRadioDetails.NrOfAssignedDevices) ? 0 : Convert.ToInt32(minomatRadioDetails.NrOfAssignedDevices.Trim())) : 0,
                    NumReservDevices = minomatRadioDetails != null ? minomatRadioDetails.ReservedPlaces : 0,
                    Polling = minomat.Polling,
                    DDConfig = int64,
                    IsActive = !minomat.IsDeactivated
                  };
                  string empty = string.Empty;
                  if (minomatRadioDetails != null)
                    dictionary.TryGetValue(minomatRadioDetails.Minomat.MinomatMasterId.Value, out empty);
                  recordObject.Reg_MasterID = empty ?? string.Empty;
                  data.Add(recordObject);
                  string propNameColumns;
                  string propValuesColumns;
                  this.GetPropertiesNameAndValue<DCUInfoRecord>(recordObject, out propNameColumns, out propValuesColumns);
                  MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, propNameColumns);
                  MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, propValuesColumns);
                }
                DataTable dataTable = data.ToDataTable<DCUInfoRecord>();
                dataTable.TableName = "DCUInfoRecordDTName";
                Service_MDMS_DataClient serviceMdmsDataClient = this.ServiceMdmsDataClient(mdmConfig);
                string msg;
                serviceMdmsDataClient.SaveDCUInfoRecord(mdmConfig.MDMUser, mdmConfig.MDMPassword, mdmConfig.Company.ToString(), mdmConfig.CustomerNumber, dataTable, out msg);
                MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, msg);
                serviceMdmsDataClient.Close();
                foreach (MSS.Core.Model.DataCollectors.Minomat entity in (IEnumerable<MSS.Core.Model.DataCollectors.Minomat>) source)
                {
                  entity.LastDCUInfoMDMExportOn = new DateTime?(DateTime.Now);
                  this._minomatRepository.Update(entity);
                }
              }
              catch (Exception ex)
              {
                MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, ex.ToString());
              }
            }
          }
          else
            MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, keyValuePair.Key.Name + "missing from MDM config. Minomats for this country were not exported!");
        }
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "-------------------------------End Save DCU Info Record ------------------------------------");
      }
      catch (Exception ex)
      {
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, ex.ToString());
      }
    }

    public void SaveDCUConnectionRecord()
    {
      try
      {
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "-------------------------------Begin Save DCU Connection Record ------------------------------------");
        MSS.Core.Model.DataCollectors.Minomat min = (MSS.Core.Model.DataCollectors.Minomat) null;
        MinomatForMDMDTO minomatforMDM = (MinomatForMDMDTO) null;
        while (this._repositoryFactory.GetRepository<MinomatConnectionLog>().GetAll().Any<MinomatConnectionLog>((System.Func<MinomatConnectionLog, bool>) (x => !x.LastDCUConnectionMDMExportOn.HasValue)))
        {
          List<MinomatConnectionLog> list1 = this._repositoryFactory.GetRepository<MinomatConnectionLog>().SearchFor((Expression<System.Func<MinomatConnectionLog, bool>>) (l => l.LastDCUConnectionMDMExportOn == new DateTime?())).Take<MinomatConnectionLog>(500).ToList<MinomatConnectionLog>();
          Guid?[] array = list1.Select<MinomatConnectionLog, Guid?>((System.Func<MinomatConnectionLog, Guid?>) (x => x.MinomatId)).Distinct<Guid?>().ToArray<Guid?>();
          List<MinomatForMDMDTO> list2 = this._repositoryFactory.GetSession().QueryOver<MSS.Core.Model.DataCollectors.Minomat>((Expression<Func<MSS.Core.Model.DataCollectors.Minomat>>) (() => min)).WhereRestrictionOn((Expression<System.Func<MSS.Core.Model.DataCollectors.Minomat, object>>) (val => (object) val.Id)).IsIn((ICollection) array).Select((IProjection) Projections.ProjectionList().Add(Projections.Property((Expression<Func<object>>) (() => (object) min.Id)).WithAlias((Expression<Func<object>>) (() => (object) minomatforMDM.Id))).Add(Projections.Property((Expression<Func<object>>) (() => min.GsmId)).WithAlias((Expression<Func<object>>) (() => minomatforMDM.GsmId))).Add(Projections.Property((Expression<Func<object>>) (() => (object) min.Polling)).WithAlias((Expression<Func<object>>) (() => (object) minomatforMDM.Polling))).Add(Projections.Property((Expression<Func<object>>) (() => min.Country)).WithAlias((Expression<Func<object>>) (() => minomatforMDM.Country)))).TransformUsing(Transformers.AliasToBean<MinomatForMDMDTO>()).List<MinomatForMDMDTO>().ToList<MinomatForMDMDTO>();
          MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "Number of Logs: " + (object) list1.Count);
          IList<MDMConfigs> all = this._mdmConfigsRepository.GetAll();
          if (all.Count == 1)
          {
            if (list2.Any<MinomatForMDMDTO>((System.Func<MinomatForMDMDTO, bool>) (x => x.Country == null)))
            {
              MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, "Minomats without country identified. They won`t be exported to MDM");
              list2 = list2.Where<MinomatForMDMDTO>((System.Func<MinomatForMDMDTO, bool>) (x => x.Country != null)).ToList<MinomatForMDMDTO>();
            }
            this.SendConnectionLogsToMDM(all[0], list1, list2);
          }
          else
          {
            foreach (IGrouping<Country, MinomatForMDMDTO> grouping in list2.GroupBy<MinomatForMDMDTO, Country>((System.Func<MinomatForMDMDTO, Country>) (x => x.Country)).ToList<IGrouping<Country, MinomatForMDMDTO>>())
            {
              IGrouping<Country, MinomatForMDMDTO> minomatsInCountry = grouping;
              if (minomatsInCountry.Key == null)
              {
                MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, "Minomats without country identified. They won`t be exported to MDM");
              }
              else
              {
                MDMConfigs mdmConfig = all.FirstOrDefault<MDMConfigs>((System.Func<MDMConfigs, bool>) (x => x.Country == minomatsInCountry.Key));
                if (mdmConfig != null)
                {
                  List<Guid> minomatsWithConfigIds = minomatsInCountry.Select<MinomatForMDMDTO, Guid>((System.Func<MinomatForMDMDTO, Guid>) (x => x.Id)).ToList<Guid>();
                  List<MinomatConnectionLog> list3 = list1.Where<MinomatConnectionLog>((System.Func<MinomatConnectionLog, bool>) (x => x.MinomatId.HasValue && minomatsWithConfigIds.Contains(x.MinomatId.Value))).ToList<MinomatConnectionLog>();
                  this.SendConnectionLogsToMDM(mdmConfig, list3, new List<MinomatForMDMDTO>((IEnumerable<MinomatForMDMDTO>) minomatsInCountry));
                }
                else
                  MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, minomatsInCountry.Key.Name + " missing from MDM config. Minomat connection log for this country were not exported!");
              }
            }
            MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "-------------------------------End Save DCU Connection Record ------------------------------------");
          }
        }
      }
      catch (Exception ex)
      {
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, ex.ToString());
      }
    }

    private void SendConnectionLogsToMDM(
      MDMConfigs mdmConfig,
      List<MinomatConnectionLog> connectionLogs,
      List<MinomatForMDMDTO> minomats)
    {
      MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, "DCU Connection Record( Enterprise_Id:" + (object) mdmConfig.Company + ")");
      foreach (IGrouping<int, MinomatConnectionLog> grouping in connectionLogs.Select((x, index) => new
      {
        x = x,
        index = index
      }).GroupBy(x => x.index / Convert.ToInt32(this.batchSize), y => y.x))
      {
        try
        {
          List<DCUConnectionRecord> data = new List<DCUConnectionRecord>();
          foreach (MinomatConnectionLog minomatConnectionLog in (IEnumerable<MinomatConnectionLog>) grouping)
          {
            MinomatConnectionLog minomatDataLog = minomatConnectionLog;
            Encoding utF8 = Encoding.UTF8;
            DCUConnectionRecord recordObject = new DCUConnectionRecord()
            {
              DCU_ID = minomatDataLog.GsmID,
              HttpRequest = 0,
              Load_Dttm = minomatDataLog.TimePoint,
              GSMConnection = minomats.FirstOrDefault<MinomatForMDMDTO>((System.Func<MinomatForMDMDTO, bool>) (x =>
              {
                Guid id = x.Id;
                Guid? minomatId = minomatDataLog.MinomatId;
                return minomatId.HasValue && id == minomatId.GetValueOrDefault();
              })).Polling,
              HttpIn = 0,
              HttpOut = 0
            };
            data.Add(recordObject);
            string propNameColumns;
            string propValuesColumns;
            this.GetPropertiesNameAndValue<DCUConnectionRecord>(recordObject, out propNameColumns, out propValuesColumns);
            MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, propNameColumns);
            MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, propValuesColumns);
          }
          DataTable dataTable = data.ToDataTable<DCUConnectionRecord>();
          dataTable.TableName = "DCUConnectionRecordDTName";
          Service_MDMS_DataClient serviceMdmsDataClient = this.ServiceMdmsDataClient(mdmConfig);
          string msg;
          serviceMdmsDataClient.SaveDCUConnectionRecord(mdmConfig.MDMUser, mdmConfig.MDMPassword, mdmConfig.Company.ToString(), mdmConfig.CustomerNumber, dataTable, out msg);
          MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, msg);
          serviceMdmsDataClient.Close();
        }
        catch (Exception ex)
        {
          MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, ex.ToString());
        }
      }
      foreach (MinomatConnectionLog connectionLog in connectionLogs)
      {
        connectionLog.LastDCUConnectionMDMExportOn = new DateTime?(DateTime.Now);
        this._repositoryFactory.GetRepository<MinomatConnectionLog>().Update(connectionLog);
      }
    }

    public void SaveAMRRouteRecord()
    {
      try
      {
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "-------------------------------Begin Save AMR Route Record ------------------------------------");
        IList<MSS.Core.Model.DataCollectors.Minomat> all = this._repositoryFactory.GetRepository<MSS.Core.Model.DataCollectors.Minomat>().GetAll();
        Dictionary<Country, List<DriverTables.ServiceTaskResultRow>> dictionary = new Dictionary<Country, List<DriverTables.ServiceTaskResultRow>>();
        List<DriverTables.ServiceTaskResultRow> source = ServiceTaskResult.LoadServiceTaskResultByMethodName(GmmInterface.Database.BaseDbConnection, "MinomatHandler.RoutingTable GetRoutingTable()");
        if (source == null)
          return;
        foreach (DriverTables.ServiceTaskResultRow serviceTaskResultRow1 in source)
        {
          DriverTables.ServiceTaskResultRow serviceTaskResultRow = serviceTaskResultRow1;
          MSS.Core.Model.DataCollectors.Minomat minomat = all.FirstOrDefault<MSS.Core.Model.DataCollectors.Minomat>((System.Func<MSS.Core.Model.DataCollectors.Minomat, bool>) (m => m.GsmId == serviceTaskResultRow.SerialNumber));
          if (minomat != null)
          {
            Country country = minomat.Country;
            if (dictionary.ContainsKey(country))
              dictionary[country].Add(serviceTaskResultRow);
            else
              dictionary.Add(country, new List<DriverTables.ServiceTaskResultRow>()
              {
                serviceTaskResultRow
              });
          }
        }
        foreach (KeyValuePair<Country, List<DriverTables.ServiceTaskResultRow>> keyValuePair in dictionary)
        {
          Guid countryID = keyValuePair.Key.Id;
          MDMConfigs mdmConfig = this._mdmConfigsRepository.FirstOrDefault((Expression<System.Func<MDMConfigs, bool>>) (m => m.Country.Id == countryID));
          if (mdmConfig != null)
          {
            MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, "AMR Route Record( Enterprise_Id:" + (object) mdmConfig.Company + ")");
            IEnumerable<IGrouping<int, DriverTables.ServiceTaskResultRow>> groupings = source.Select((x, index) => new
            {
              x = x,
              index = index
            }).GroupBy(x => x.index / Convert.ToInt32(this.batchSize), y => y.x);
            Encoding utF8 = Encoding.UTF8;
            foreach (IGrouping<int, DriverTables.ServiceTaskResultRow> grouping in groupings)
            {
              List<AMRRouteRecord> data = new List<AMRRouteRecord>();
              foreach (DriverTables.ServiceTaskResultRow serviceTaskResultRow in (IEnumerable<DriverTables.ServiceTaskResultRow>) grouping)
              {
                string serialNumer = serviceTaskResultRow.SerialNumber;
                MSS.Core.Model.DataCollectors.Minomat minomat = all.FirstOrDefault<MSS.Core.Model.DataCollectors.Minomat>((System.Func<MSS.Core.Model.DataCollectors.Minomat, bool>) (m => m.GsmId == serialNumer));
                if (minomat != null)
                {
                  byte[] byteArray = MDMManager.StringToByteArray(serviceTaskResultRow.RawData);
                  AMRRouteRecord recordObject = new AMRRouteRecord()
                  {
                    DCU_ID = minomat.RadioId,
                    DateOfTable = serviceTaskResultRow.TimePoint,
                    Route_Data = MDMManager.ByteArrayToString(MinomatV4.RemoveSCGiFrame(byteArray))
                  };
                  data.Add(recordObject);
                  string propNameColumns;
                  string propValuesColumns;
                  this.GetPropertiesNameAndValue<AMRRouteRecord>(recordObject, out propNameColumns, out propValuesColumns);
                  MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, propNameColumns);
                  MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, propValuesColumns);
                }
              }
              DataTable dataTable = data.ToDataTable<AMRRouteRecord>();
              dataTable.TableName = "AMRRouteRecordDTName";
              Service_MDMS_DataClient serviceMdmsDataClient = this.ServiceMdmsDataClient(mdmConfig);
              string msg;
              serviceMdmsDataClient.SaveAMRRouteRecord(mdmConfig.MDMUser, mdmConfig.MDMPassword, mdmConfig.Company.ToString(), mdmConfig.CustomerNumber, dataTable, out msg);
              MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, msg);
              serviceMdmsDataClient.Close();
            }
          }
          else
            MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, keyValuePair.Key.Name + "missing from MDM config. AMR Route for this country were not exported!");
        }
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "-------------------------------End Save AMR Route Record ------------------------------------");
      }
      catch (Exception ex)
      {
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, ex.ToString());
      }
    }

    public static byte[] StringToByteArray(string hex)
    {
      return Enumerable.Range(0, hex.Length).Where<int>((System.Func<int, bool>) (x => x % 2 == 0)).Select<int, byte>((System.Func<int, byte>) (x => Convert.ToByte(hex.Substring(x, 2), 16))).ToArray<byte>();
    }

    public static string ByteArrayToString(byte[] ba)
    {
      StringBuilder stringBuilder = new StringBuilder(ba.Length * 2);
      foreach (byte num in ba)
        stringBuilder.AppendFormat("{0:x2}", (object) num);
      return stringBuilder.ToString();
    }

    public void SaveTestConfigRunRecord()
    {
      try
      {
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "-------------------------------Begin Save Test Config Run Record ------------------------------------");
        List<RadioTestRun> list = this._radioTestRunRepository.SearchFor((Expression<System.Func<RadioTestRun, bool>>) (l => l.LastRadioTestRunMDMExportOn == new DateTime?() || l.LastRadioTestRunMDMExportOn != new DateTime?() && l.UpdateDate >= l.LastRadioTestRunMDMExportOn)).ToList<RadioTestRun>();
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "Number of radio test run items: " + (object) list.Count);
        foreach (KeyValuePair<Country, List<RadioTestRun>> keyValuePair in this.GroupRadioTestRunByCountry(list))
        {
          Guid countryID = keyValuePair.Key.Id;
          MDMConfigs mdmConfig = this._mdmConfigsRepository.FirstOrDefault((Expression<System.Func<MDMConfigs, bool>>) (m => m.Country.Id == countryID));
          if (mdmConfig != null)
          {
            MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, "Test Config Runt Record( Enterprise_Id:" + (object) mdmConfig.Company + ")");
            foreach (IGrouping<int, RadioTestRun> grouping in keyValuePair.Value.Select((x, index) => new
            {
              x = x,
              index = index
            }).GroupBy(x => x.index / Convert.ToInt32(this.batchSize), y => y.x))
            {
              try
              {
                List<TestconfigRunRecord> data = new List<TestconfigRunRecord>();
                foreach (RadioTestRun radioTestRun in (IEnumerable<RadioTestRun>) grouping)
                {
                  TestconfigRunRecord recordObject = new TestconfigRunRecord()
                  {
                    Config_ID = radioTestRun.TrKonfig,
                    TestRun_No = radioTestRun.TrNumber,
                    Project_ID = radioTestRun.TestOrder.BuildingNumber,
                    TestRun_Type = radioTestRun.TrType,
                    UserLocation = radioTestRun.TrUserLocation,
                    DeviceLocation = radioTestRun.TrDeviceLocation,
                    Comment = radioTestRun.TrComment,
                    BestChoice = radioTestRun.TrBestChoice.ToString()
                  };
                  data.Add(recordObject);
                  MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "Radio test run :" + recordObject.Config_ID + ", " + recordObject.Project_ID);
                  string propNameColumns;
                  string propValuesColumns;
                  this.GetPropertiesNameAndValue<TestconfigRunRecord>(recordObject, out propNameColumns, out propValuesColumns);
                  MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, propNameColumns);
                  MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, propValuesColumns);
                }
                DataTable dataTable = data.ToDataTable<TestconfigRunRecord>();
                dataTable.TableName = "TestConfigRunRecordDTName";
                Service_MDMS_DataClient serviceMdmsDataClient = this.ServiceMdmsDataClient(mdmConfig);
                string msg;
                serviceMdmsDataClient.SaveTestconfigRunRecord(mdmConfig.MDMUser, mdmConfig.MDMPassword, mdmConfig.Company.ToString(), mdmConfig.CustomerNumber, dataTable, out msg);
                MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, msg);
                serviceMdmsDataClient.Close();
                foreach (RadioTestRun entity in (IEnumerable<RadioTestRun>) grouping)
                {
                  entity.LastRadioTestRunMDMExportOn = new DateTime?(DateTime.Now);
                  this._radioTestRunRepository.Update(entity);
                }
              }
              catch (Exception ex)
              {
                MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, ex.ToString());
              }
            }
          }
          else
            MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, keyValuePair.Key.Name + "missing from MDM config. Radio test run for this country were not exported!");
        }
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "-------------------------------End Save Test Config Run Record ------------------------------------");
      }
      catch (Exception ex)
      {
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, ex.ToString());
      }
    }

    public void SaveTestConfigDeviceRecord()
    {
      try
      {
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "-------------------------------Begin Save Test Config Run Device Record ------------------------------------");
        List<RadioTestRunDevice> list = this._radioTestRunDeviceRepository.SearchFor((Expression<System.Func<RadioTestRunDevice, bool>>) (l => l.LastRadioTestRunDeviceMDMExportOn == new DateTime?() || l.LastRadioTestRunDeviceMDMExportOn != new DateTime?() && l.UpdateDate >= l.LastRadioTestRunDeviceMDMExportOn)).ToList<RadioTestRunDevice>();
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "Number of radio test run device items: " + (object) list.Count);
        foreach (KeyValuePair<Country, List<RadioTestRunDevice>> keyValuePair in this.GroupRadioTestRunDeviceByCountry(list))
        {
          Guid countryID = keyValuePair.Key.Id;
          MDMConfigs mdmConfig = this._mdmConfigsRepository.FirstOrDefault((Expression<System.Func<MDMConfigs, bool>>) (m => m.Country.Id == countryID));
          if (mdmConfig != null)
          {
            MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, "Test Config Run Device Record( Enterprise_Id:" + (object) mdmConfig.Company + ")");
            foreach (IGrouping<int, RadioTestRunDevice> grouping in keyValuePair.Value.Select((x, index) => new
            {
              x = x,
              index = index
            }).GroupBy(x => x.index / Convert.ToInt32(this.batchSize), y => y.x))
            {
              try
              {
                List<TestconfigDeviceRecord> data = new List<TestconfigDeviceRecord>();
                foreach (RadioTestRunDevice radioTestRunDevice in (IEnumerable<RadioTestRunDevice>) grouping)
                {
                  TestconfigDeviceRecord recordObject = new TestconfigDeviceRecord()
                  {
                    Config_ID = radioTestRunDevice.TgKonfig,
                    TestRun_No = radioTestRunDevice.TgNumber,
                    Project_ID = radioTestRunDevice.RadioTestRun.TestOrder.BuildingNumber,
                    Radio_ID = radioTestRunDevice.TgRadioId,
                    Last_RSSI = Convert.ToInt32(radioTestRunDevice.TgLastRssi),
                    AverageRSSI = Convert.ToInt32((object) radioTestRunDevice.TgAverage),
                    DeviceType = Convert.ToInt32(radioTestRunDevice.TgFabrikat)
                  };
                  data.Add(recordObject);
                  MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "Radio test run device :" + recordObject.Config_ID + ", " + recordObject.Project_ID);
                  string propNameColumns;
                  string propValuesColumns;
                  this.GetPropertiesNameAndValue<TestconfigDeviceRecord>(recordObject, out propNameColumns, out propValuesColumns);
                  MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, propNameColumns);
                  MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Debug, propValuesColumns);
                }
                DataTable dataTable = data.ToDataTable<TestconfigDeviceRecord>();
                dataTable.TableName = "TestConfigRunDeviceRecordDTName";
                Service_MDMS_DataClient serviceMdmsDataClient = this.ServiceMdmsDataClient(mdmConfig);
                string msg;
                serviceMdmsDataClient.SaveTestconfigDeviceRecord(mdmConfig.MDMUser, mdmConfig.MDMPassword, mdmConfig.Company.ToString(), mdmConfig.CustomerNumber, dataTable, out msg);
                MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, msg);
                serviceMdmsDataClient.Close();
                foreach (RadioTestRunDevice entity in (IEnumerable<RadioTestRunDevice>) grouping)
                {
                  entity.LastRadioTestRunDeviceMDMExportOn = new DateTime?(DateTime.Now);
                  this._radioTestRunDeviceRepository.Update(entity);
                }
              }
              catch (Exception ex)
              {
                MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, ex.ToString());
              }
            }
          }
          else
            MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, keyValuePair.Key.Name + "missing from MDM config. Radio test run device for this country were not exported!");
        }
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Info, "-------------------------------End Save Test Config Run Device Record ------------------------------------");
      }
      catch (Exception ex)
      {
        MDMManager.mdmLogger.WriteMessageToLogger(NLog.LogLevel.Error, ex.ToString());
      }
    }

    private List<MinomatConnectionLog> GetConnectionLog(
      string gsmId,
      DateTime? StartDate,
      DateTime EndDate)
    {
      if (!StartDate.HasValue)
        return this._repositoryFactory.GetSession().QueryOver<MinomatConnectionLog>().Where((Expression<System.Func<MinomatConnectionLog, bool>>) (x => x.GsmID == gsmId)).List<MinomatConnectionLog>().ToList<MinomatConnectionLog>();
      return this._repositoryFactory.GetSession().QueryOver<MinomatConnectionLog>().Where((Expression<System.Func<MinomatConnectionLog, bool>>) (x => x.GsmID == gsmId && (DateTime?) x.TimePoint > StartDate && x.TimePoint <= EndDate)).List<MinomatConnectionLog>().ToList<MinomatConnectionLog>();
    }

    private MSS.Core.Model.Orders.Order GetOrderForLocation(MSS.Core.Model.Structures.Location locationForMinomat)
    {
      StructureNode node = this._structureNodeRepository.FirstOrDefault((Expression<System.Func<StructureNode, bool>>) (n => n.EntityId == locationForMinomat.Id && n.EndDate == new DateTime?()));
      if (node != null)
      {
        MSS.Core.Model.Orders.Order orderForLocation = this._orderRepository.FirstOrDefault((Expression<System.Func<MSS.Core.Model.Orders.Order, bool>>) (o => o.RootStructureNodeId == node.Id));
        if (orderForLocation != null)
          return orderForLocation;
      }
      return (MSS.Core.Model.Orders.Order) null;
    }

    private IList<Meter> GetMeterInFixedStructure(int batchSize)
    {
      NHibernate.ISession session = this._repositoryFactory.GetSession();
      StructureNodeLinks linkAlias = (StructureNodeLinks) null;
      StructureNode nodeAlias = (StructureNode) null;
      Channel channelAlias = (Channel) null;
      MeterRadioDetails meterRadioDetailsAlias = (MeterRadioDetails) null;
      Meter meterAlias = (Meter) null;
      QueryOver<StructureNodeLinks, StructureNodeLinks> detachedCriteria = QueryOver.Of<StructureNodeLinks>((Expression<Func<StructureNodeLinks>>) (() => linkAlias)).JoinAlias((Expression<System.Func<StructureNodeLinks, object>>) (l => l.Node), (Expression<Func<object>>) (() => nodeAlias)).Where((Expression<System.Func<StructureNodeLinks, bool>>) (l => l.EndDate == new DateTime?() && (int) l.StructureType == 2)).Select((Expression<System.Func<StructureNodeLinks, object>>) (l => (object) nodeAlias.EntityId));
      return session.QueryOver<Meter>((Expression<Func<Meter>>) (() => meterAlias)).JoinAlias((Expression<System.Func<Meter, object>>) (m => m.MeterRadioDetailsList), (Expression<Func<object>>) (() => meterRadioDetailsAlias), JoinType.LeftOuterJoin).JoinAlias((Expression<System.Func<Meter, object>>) (m => m.Channel), (Expression<Func<object>>) (() => channelAlias), JoinType.LeftOuterJoin).WithSubquery.WhereProperty((Expression<System.Func<Meter, object>>) (m => (object) m.Id)).In<StructureNodeLinks>((QueryOver<StructureNodeLinks>) detachedCriteria).Where((Expression<System.Func<Meter, bool>>) (m => m.LastMDMExportOn == new DateTime?() || m.LastMDMExportOn != new DateTime?() && m.LastChangedOn >= m.LastMDMExportOn)).Take(batchSize).List<Meter>();
    }

    private string GetReg(string dgReg)
    {
      return string.IsNullOrEmpty(dgReg) || dgReg.Length < 2 ? string.Empty : dgReg.Substring(2, 1);
    }

    private MSS.Core.Model.Structures.Location GetLocationForTenant(Tenant tenant)
    {
      StructureNode tenantNode = this._structureNodeRepository.FirstOrDefault((Expression<System.Func<StructureNode, bool>>) (n => n.EntityId == tenant.Id && n.EndDate == new DateTime?()));
      if (tenantNode == null)
        return (MSS.Core.Model.Structures.Location) null;
      StructureNodeLinks tenantLink = this._structureNodeLinksRepository.FirstOrDefault((Expression<System.Func<StructureNodeLinks, bool>>) (l => l.Node.Id == tenantNode.Id && l.ParentNodeId == l.RootNode.Id && l.EndDate == new DateTime?()));
      if (tenantLink == null)
        return (MSS.Core.Model.Structures.Location) null;
      StructureNode locationNode = this._structureNodeRepository.FirstOrDefault((Expression<System.Func<StructureNode, bool>>) (l => l.Id == tenantLink.RootNode.Id));
      if (locationNode == null)
        return (MSS.Core.Model.Structures.Location) null;
      return this._locationRepository.FirstOrDefault((Expression<System.Func<MSS.Core.Model.Structures.Location, bool>>) (l => l.Id == locationNode.EntityId));
    }

    private MSS.Core.Model.Structures.Location GetLocationForMinomat(MSS.Core.Model.DataCollectors.Minomat minomat)
    {
      StructureNode minomatNode = this._structureNodeRepository.FirstOrDefault((Expression<System.Func<StructureNode, bool>>) (n => n.EntityId == minomat.Id && n.EndDate == new DateTime?()));
      if (minomatNode == null)
        return (MSS.Core.Model.Structures.Location) null;
      StructureNodeLinks minomatLink = this._structureNodeLinksRepository.FirstOrDefault((Expression<System.Func<StructureNodeLinks, bool>>) (l => l.Node.Id == minomatNode.Id && l.ParentNodeId == l.RootNode.Id && l.EndDate == new DateTime?()));
      if (minomatLink == null)
        return (MSS.Core.Model.Structures.Location) null;
      StructureNode locationNode = this._structureNodeRepository.FirstOrDefault((Expression<System.Func<StructureNode, bool>>) (l => l.Id == minomatLink.RootNode.Id));
      if (locationNode == null)
        return (MSS.Core.Model.Structures.Location) null;
      return this._locationRepository.FirstOrDefault((Expression<System.Func<MSS.Core.Model.Structures.Location, bool>>) (l => l.Id == locationNode.EntityId));
    }

    private object GetParentEntity(Guid entityId)
    {
      StructureNode node = this._structureNodeRepository.FirstOrDefault((Expression<System.Func<StructureNode, bool>>) (n => n.EntityId == entityId));
      if (node == null)
        return (object) Guid.Empty;
      StructureNodeLinks link = this._structureNodeLinksRepository.FirstOrDefault((Expression<System.Func<StructureNodeLinks, bool>>) (l => l.Node.Id == node.Id));
      if (link == null)
        return (object) Guid.Empty;
      StructureNode structureNode = this._structureNodeRepository.FirstOrDefault((Expression<System.Func<StructureNode, bool>>) (n => n.Id == link.ParentNodeId));
      return (object) (structureNode != null ? structureNode.EntityId : Guid.Empty);
    }

    private void GetPropertiesNameAndValue<T>(
      T recordObject,
      out string propNameColumns,
      out string propValuesColumns)
    {
      PropertyInfo[] properties = recordObject.GetType().GetProperties();
      List<string> list = ((IEnumerable<PropertyInfo>) properties).Select<PropertyInfo, string>((System.Func<PropertyInfo, string>) (property => property.Name)).ToList<string>();
      propNameColumns = string.Join("| ", list.ToArray());
      List<string> stringList = new List<string>();
      foreach (PropertyInfo propertyInfo in properties)
      {
        object obj = propertyInfo.GetValue((object) recordObject, (object[]) null);
        stringList.Add(obj != null ? obj.ToString() : string.Empty);
      }
      propValuesColumns = string.Join("| ", stringList.ToArray());
    }

    private Service_MDMS_DataClient ServiceMdmsDataClient(MDMConfigs mdmConfig)
    {
      BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
      basicHttpBinding.Name = "BasicHttpBinding_IService_MDMS_Data";
      EndpointAddress remoteAddress = new EndpointAddress(mdmConfig.MDMUrl);
      return new Service_MDMS_DataClient((Binding) basicHttpBinding, remoteAddress);
    }

    private Dictionary<Country, List<Tenant>> GroupTenantsByCountry(List<Tenant> tenants)
    {
      Dictionary<MSS.Core.Model.Structures.Location, List<Tenant>> source = new Dictionary<MSS.Core.Model.Structures.Location, List<Tenant>>();
      foreach (Tenant tenant in tenants)
      {
        MSS.Core.Model.Structures.Location locationForTenant = this.GetLocationForTenant(tenant);
        if (locationForTenant != null)
        {
          if (source.ContainsKey(locationForTenant))
            source[locationForTenant].Add(tenant);
          else
            source.Add(locationForTenant, new List<Tenant>()
            {
              tenant
            });
        }
      }
      Dictionary<Country, List<Tenant>> dictionary = new Dictionary<Country, List<Tenant>>();
      foreach (IGrouping<Country, MSS.Core.Model.Structures.Location> grouping in source.Select<KeyValuePair<MSS.Core.Model.Structures.Location, List<Tenant>>, MSS.Core.Model.Structures.Location>((System.Func<KeyValuePair<MSS.Core.Model.Structures.Location, List<Tenant>>, MSS.Core.Model.Structures.Location>) (entry => entry.Key)).ToList<MSS.Core.Model.Structures.Location>().GroupBy<MSS.Core.Model.Structures.Location, Country>((System.Func<MSS.Core.Model.Structures.Location, Country>) (l => l.Country)))
      {
        foreach (MSS.Core.Model.Structures.Location location in (IEnumerable<MSS.Core.Model.Structures.Location>) grouping)
        {
          foreach (KeyValuePair<MSS.Core.Model.Structures.Location, List<Tenant>> keyValuePair in source)
          {
            if (keyValuePair.Key == location)
            {
              if (dictionary.ContainsKey(grouping.Key))
              {
                foreach (Tenant tenant in keyValuePair.Value)
                  dictionary[grouping.Key].Add(tenant);
              }
              else
                dictionary.Add(grouping.Key, keyValuePair.Value);
            }
          }
        }
      }
      return dictionary;
    }

    private Dictionary<Country, List<MSS.Core.Model.DataCollectors.Minomat>> GroupMinomatsByCountry(
      List<MSS.Core.Model.DataCollectors.Minomat> minomats)
    {
      Dictionary<Country, List<MSS.Core.Model.DataCollectors.Minomat>> dictionary = new Dictionary<Country, List<MSS.Core.Model.DataCollectors.Minomat>>();
      foreach (MSS.Core.Model.DataCollectors.Minomat minomat in minomats)
      {
        Country country = minomat.Country;
        if (country == null)
          MDMManager.mdmLogger.Debug("Country is null for Minomat " + minomat.RadioId);
        else if (dictionary.ContainsKey(country))
          dictionary[country].Add(minomat);
        else
          dictionary.Add(country, new List<MSS.Core.Model.DataCollectors.Minomat>()
          {
            minomat
          });
      }
      return dictionary;
    }

    private Dictionary<Country, List<Meter>> GroupMetersByCountry(IList<Meter> meters)
    {
      Dictionary<Guid, MSS.Core.Model.Structures.Location> locationsForMeters = this._repositoryFactory.GetStructureNodeRepository().GetLocationsForMeters(meters.Select<Meter, Guid>((System.Func<Meter, Guid>) (m => m.Id)).ToList<Guid>());
      Dictionary<Country, List<Meter>> dictionary = new Dictionary<Country, List<Meter>>();
      foreach (IGrouping<Country, MSS.Core.Model.Structures.Location> grouping in locationsForMeters.Select<KeyValuePair<Guid, MSS.Core.Model.Structures.Location>, MSS.Core.Model.Structures.Location>((System.Func<KeyValuePair<Guid, MSS.Core.Model.Structures.Location>, MSS.Core.Model.Structures.Location>) (entry => entry.Value)).ToList<MSS.Core.Model.Structures.Location>().GroupBy<MSS.Core.Model.Structures.Location, Country>((System.Func<MSS.Core.Model.Structures.Location, Country>) (l => l.Country)))
      {
        foreach (MSS.Core.Model.Structures.Location location in (IEnumerable<MSS.Core.Model.Structures.Location>) grouping)
        {
          foreach (KeyValuePair<Guid, MSS.Core.Model.Structures.Location> keyValuePair in locationsForMeters)
          {
            KeyValuePair<Guid, MSS.Core.Model.Structures.Location> locationMetesEntry = keyValuePair;
            if (locationMetesEntry.Value == location)
            {
              if (dictionary.ContainsKey(grouping.Key))
                dictionary[grouping.Key].Add(meters.FirstOrDefault<Meter>((System.Func<Meter, bool>) (m => m.Id == locationMetesEntry.Key)));
              else
                dictionary.Add(grouping.Key, new List<Meter>()
                {
                  meters.FirstOrDefault<Meter>((System.Func<Meter, bool>) (m => m.Id == locationMetesEntry.Key))
                });
            }
          }
        }
      }
      return dictionary;
    }

    private Dictionary<Country, List<MeterReadingValue>> GroupMeterReadingValuesByCountry(
      List<MeterReadingValue> meterReadingValues)
    {
      Dictionary<Country, List<MeterReadingValue>> dictionary = new Dictionary<Country, List<MeterReadingValue>>();
      List<Guid> readingValuesIds = meterReadingValues.Select<MeterReadingValue, Guid>((System.Func<MeterReadingValue, Guid>) (rv => rv.Id)).ToList<Guid>();
      Dictionary<Guid, MSS.Core.Model.Structures.Location> locs = new ReadingValuesManager(this._repositoryFactory).GetLocationForOrderReadingValues(this._repositoryFactory.GetRepository<OrderReadingValues>().SearchFor((Expression<System.Func<OrderReadingValues, bool>>) (orv => readingValuesIds.Contains(orv.MeterReadingValue.Id))));
      IList<JobReadingValues> source = this._repositoryFactory.GetRepository<JobReadingValues>().SearchFor((Expression<System.Func<JobReadingValues, bool>>) (jrv => readingValuesIds.Contains(jrv.ReadingValue.Id)));
      foreach (MeterReadingValue meterReadingValue1 in meterReadingValues)
      {
        MeterReadingValue meterReadingValue = meterReadingValue1;
        Country key = (Country) null;
        if (locs.ContainsKey(meterReadingValue.Id))
        {
          key = locs[meterReadingValue.Id].Country;
          if (key == null)
          {
            MSS.Core.Model.Structures.Location location = this._repositoryFactory.GetRepository<MSS.Core.Model.Structures.Location>().FirstOrDefault((Expression<System.Func<MSS.Core.Model.Structures.Location, bool>>) (l => l.BuildingNr == locs[meterReadingValue.Id].BuildingNr));
            if (location != null)
              key = location.Country;
          }
        }
        else
        {
          JobReadingValues jobReadingValues = source.FirstOrDefault<JobReadingValues>((System.Func<JobReadingValues, bool>) (jrv => jrv.ReadingValue.Id == meterReadingValue.Id));
          if (jobReadingValues != null && jobReadingValues.Job.Minomat != null)
            key = jobReadingValues.Job.Minomat.Country;
        }
        if (key == null)
        {
          meterReadingValue.MDMExportedOn = new DateTime?(MDMManager.MinDateTime);
          this._repositoryFactory.GetRepository<MeterReadingValue>().TransactionalUpdate(meterReadingValue);
          MDMManager.mdmLogger.Debug("Error: No country found for serial number " + meterReadingValue.MeterSerialNumber);
        }
        else if (dictionary.ContainsKey(key))
          dictionary[key].Add(meterReadingValue);
        else
          dictionary.Add(key, new List<MeterReadingValue>()
          {
            meterReadingValue
          });
      }
      return dictionary;
    }

    private Dictionary<Country, List<RadioTestRun>> GroupRadioTestRunByCountry(
      List<RadioTestRun> radioTestRunItems)
    {
      Dictionary<Country, List<RadioTestRun>> dictionary = new Dictionary<Country, List<RadioTestRun>>();
      foreach (RadioTestRun radioTestRunItem1 in radioTestRunItems)
      {
        RadioTestRun radioTestRunItem = radioTestRunItem1;
        TestOrder testOrder = this._testOrderRepository.FirstOrDefault((Expression<System.Func<TestOrder, bool>>) (m => m.Id == radioTestRunItem.TestOrder.Id));
        if (testOrder != null)
        {
          Country country = testOrder.Country;
          if (dictionary.ContainsKey(country))
            dictionary[country].Add(radioTestRunItem);
          else
            dictionary.Add(country, new List<RadioTestRun>()
            {
              radioTestRunItem
            });
        }
      }
      return dictionary;
    }

    private Dictionary<Country, List<RadioTestRunDevice>> GroupRadioTestRunDeviceByCountry(
      List<RadioTestRunDevice> radioTestRunDeviceList)
    {
      Dictionary<Country, List<RadioTestRunDevice>> dictionary = new Dictionary<Country, List<RadioTestRunDevice>>();
      foreach (RadioTestRunDevice radioTestRunDevice in radioTestRunDeviceList)
      {
        RadioTestRunDevice radioTestRunDeviceItem = radioTestRunDevice;
        RadioTestRun radiotestRun = this._radioTestRunRepository.FirstOrDefault((Expression<System.Func<RadioTestRun, bool>>) (rt => rt.Id == radioTestRunDeviceItem.RadioTestRun.Id));
        if (radiotestRun != null)
        {
          TestOrder testOrder = this._testOrderRepository.FirstOrDefault((Expression<System.Func<TestOrder, bool>>) (m => m.Id == radiotestRun.TestOrder.Id));
          if (testOrder != null)
          {
            Country country = testOrder.Country;
            if (dictionary.ContainsKey(country))
              dictionary[country].Add(radioTestRunDeviceItem);
            else
              dictionary.Add(country, new List<RadioTestRunDevice>()
              {
                radioTestRunDeviceItem
              });
          }
        }
      }
      return dictionary;
    }

    private List<Meter> GetMeterListForMeterReadingValues(
      IEnumerable<MeterReadingValue> meterReadingValues)
    {
      List<Meter> meterReadingValues1 = new List<Meter>();
      foreach (MeterReadingValue meterReadingValue in meterReadingValues)
      {
        MeterReadingValue value = meterReadingValue;
        Meter meter = this._meterRepository.FirstOrDefault((Expression<System.Func<Meter, bool>>) (m => m.Id == value.MeterId));
        if (!meterReadingValues1.Contains(meter) && meter != null)
          meterReadingValues1.Add(meter);
      }
      return meterReadingValues1;
    }

    private SortedList<int, string> CreateRoomTable()
    {
      return new SortedList<int, string>()
      {
        {
          54,
          "AB"
        },
        {
          93,
          "AK"
        },
        {
          1,
          "AZ"
        },
        {
          44,
          "AR"
        },
        {
          82,
          "AM"
        },
        {
          55,
          "BS"
        },
        {
          2,
          "BD"
        },
        {
          41,
          "BZ"
        },
        {
          61,
          "BH"
        },
        {
          83,
          "BR"
        },
        {
          3,
          "BU"
        },
        {
          58,
          "DT"
        },
        {
          4,
          "DU"
        },
        {
          84,
          "ER"
        },
        {
          5,
          "EZ"
        },
        {
          86,
          "F1"
        },
        {
          87,
          "F2"
        },
        {
          88,
          "F3"
        },
        {
          89,
          "F4"
        },
        {
          90,
          "F5"
        },
        {
          91,
          "F6"
        },
        {
          70,
          "FI"
        },
        {
          109,
          "FW"
        },
        {
          6,
          "FL"
        },
        {
          7,
          "FZ"
        },
        {
          49,
          "FB"
        },
        {
          72,
          "FU"
        },
        {
          8,
          "GA"
        },
        {
          53,
          "GW"
        },
        {
          9,
          "GS"
        },
        {
          51,
          "GM"
        },
        {
          56,
          "GS"
        },
        {
          69,
          "GH"
        },
        {
          10,
          "GR"
        },
        {
          48,
          "HA"
        },
        {
          73,
          "HU"
        },
        {
          31,
          "HR"
        },
        {
          11,
          "HO"
        },
        {
          29,
          "KM"
        },
        {
          40,
          "KA"
        },
        {
          12,
          "KE"
        },
        {
          13,
          "KZ"
        },
        {
          30,
          "KI"
        },
        {
          14,
          "KU"
        },
        {
          35,
          "LB"
        },
        {
          15,
          "LD"
        },
        {
          16,
          "LG"
        },
        {
          68,
          "LO"
        },
        {
          46,
          "NR"
        },
        {
          32,
          "OI"
        },
        {
          17,
          "PX"
        },
        {
          18,
          "SA"
        },
        {
          38,
          "SF"
        },
        {
          20,
          "SZ"
        },
        {
          34,
          "SR"
        },
        {
          19,
          "SB"
        },
        {
          85,
          "SE"
        },
        {
          81,
          "SP"
        },
        {
          37,
          "TK"
        },
        {
          23,
          "WC"
        },
        {
          21,
          "TH"
        },
        {
          71,
          "TR"
        },
        {
          45,
          "UR"
        },
        {
          28,
          "UN"
        },
        {
          33,
          "UI"
        },
        {
          36,
          "VK"
        },
        {
          39,
          "VR"
        },
        {
          42,
          "WA"
        },
        {
          24,
          "WK"
        },
        {
          74,
          "WM"
        },
        {
          75,
          "WW"
        },
        {
          59,
          "WP"
        },
        {
          25,
          "WE"
        },
        {
          43,
          "WG"
        },
        {
          47,
          "WI"
        },
        {
          26,
          "WR"
        },
        {
          27,
          "WZ"
        },
        {
          50,
          "Z1"
        },
        {
          76,
          "Z2"
        },
        {
          98,
          "Z10"
        },
        {
          99,
          "Z11"
        },
        {
          100,
          "Z12"
        },
        {
          101,
          "Z13"
        },
        {
          102,
          "Z14"
        },
        {
          103,
          "Z15"
        },
        {
          104,
          "Z16"
        },
        {
          105,
          "Z17"
        },
        {
          106,
          "Z18"
        },
        {
          107,
          "Z19"
        },
        {
          77,
          "Z2"
        },
        {
          108,
          "Z20"
        },
        {
          78,
          "Z3"
        },
        {
          79,
          "Z4"
        },
        {
          80,
          "Z5"
        },
        {
          94,
          "Z6"
        },
        {
          95,
          "Z7"
        },
        {
          96,
          "Z8"
        },
        {
          97,
          "Z9"
        },
        {
          111,
          "ZM"
        },
        {
          110,
          "ZS"
        }
      };
    }

    private int SetKeyRoom(Meter meter, SortedList<int, string> roomConvertion)
    {
      int num = 28;
      if (meter.Room != null)
      {
        string code = meter.Room.Code;
        if (roomConvertion.ContainsValue(code))
        {
          int index = roomConvertion.IndexOfValue(code);
          num = roomConvertion.Keys[index];
        }
      }
      return num;
    }

    private int GetNrOfTenants(StructureNode locNode, Guid tenantNodeTypeId)
    {
      int nrOfTenants = 0;
      IList<StructureNodeLinks> query = this._structureNodeLinksRepository.SearchFor((Expression<System.Func<StructureNodeLinks, bool>>) (s => s.RootNode.Id == locNode.Id && s.ParentNodeId == locNode.Id && s.EndDate == new DateTime?()));
      List<Guid> childIds = new List<Guid>();
      query.ForEach<StructureNodeLinks>((Action<StructureNodeLinks>) (c => childIds.Add(c.Node.Id)));
      IList<StructureNode> source = this._structureNodeRepository.SearchFor((Expression<System.Func<StructureNode, bool>>) (s => childIds.Contains(s.Id)));
      foreach (StructureNodeLinks structureNodeLinks in (IEnumerable<StructureNodeLinks>) query)
      {
        StructureNodeLinks child = structureNodeLinks;
        if (source.FirstOrDefault<StructureNode>((System.Func<StructureNode, bool>) (n => n.Id == child.Node.Id)).NodeType.Id == tenantNodeTypeId)
          ++nrOfTenants;
      }
      return nrOfTenants;
    }
  }
}
