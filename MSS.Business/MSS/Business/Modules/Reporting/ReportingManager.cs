// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Reporting.ReportingManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using AutoMapper;
using Common.Library.NHibernate.Data;
using Microsoft.Win32;
using MSS.Business.DTO;
using MSS.Business.Errors;
using MSS.Business.Modules.StructuresManagement;
using MSS.Core.Model.Jobs;
using MSS.Core.Model.Meters;
using MSS.Core.Model.MSSClient;
using MSS.Core.Model.Orders;
using MSS.Core.Model.Reporting;
using MSS.Core.Model.Structures;
using MSS.Core.Model.UsersManagement;
using MSS.DTO.Meters;
using MSS.DTO.Orders;
using MSS.DTO.Reporting;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MSS.Localisation;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using Telerik.Windows.Data;
using ZR_ClassLibrary;

#nullable disable
namespace MSS.Business.Modules.Reporting
{
  public class ReportingManager
  {
    private readonly IRepository<Channel> _channelRepository;
    private readonly IRepository<ConnectedDeviceType> _connectedDeviceTypeRepository;
    private readonly IRepository<Location> _locationRepository;
    private readonly IRepository<MeasureUnit> _measureUniteRepository;
    private readonly IRepository<MSS.Core.Model.Meters.Meter> _meterRepository;
    private ISession _nhSession;
    private readonly IRepository<RoomType> _roomTypeRepository;
    private readonly IRepository<StructureNodeLinks> _structureNodeLinksRepository;
    private readonly IRepository<StructureNode> _structureNodeRepository;
    private readonly IRepository<MSS.Core.Model.Structures.StructureNodeType> _structureNodeTypeRepository;
    private readonly IRepository<Tenant> _tenantRepository;
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IRepository<Scenario> _repositoryScenario;
    private readonly IRepository<MeterReadingValue> _meterReadingValueRepository;
    private readonly IRepository<AutomatedExportJob> _automatedExportJobRepository;
    private static DateTime MinDateTime = new DateTime(1800, 1, 1);

    public ReportingManager(IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._meterRepository = repositoryFactory.GetRepository<MSS.Core.Model.Meters.Meter>();
      this._roomTypeRepository = repositoryFactory.GetRepository<RoomType>();
      this._measureUniteRepository = repositoryFactory.GetRepository<MeasureUnit>();
      this._channelRepository = repositoryFactory.GetRepository<Channel>();
      this._connectedDeviceTypeRepository = repositoryFactory.GetRepository<ConnectedDeviceType>();
      this._structureNodeRepository = repositoryFactory.GetRepository<StructureNode>();
      this._structureNodeTypeRepository = repositoryFactory.GetRepository<MSS.Core.Model.Structures.StructureNodeType>();
      this._structureNodeLinksRepository = repositoryFactory.GetRepository<StructureNodeLinks>();
      this._locationRepository = repositoryFactory.GetRepository<Location>();
      this._tenantRepository = repositoryFactory.GetRepository<Tenant>();
      this._orderRepository = this._repositoryFactory.GetRepository<Order>();
      this._repositoryScenario = repositoryFactory.GetRepository<Scenario>();
      this._meterReadingValueRepository = repositoryFactory.GetRepository<MeterReadingValue>();
      this._automatedExportJobRepository = repositoryFactory.GetRepository<AutomatedExportJob>();
      this._nhSession = repositoryFactory.GetSession();
      Mapper.CreateMap<MeterDTO, MSS.Core.Model.Meters.Meter>();
      Mapper.CreateMap<LocationDTO, Location>();
      Mapper.CreateMap<TenantDTO, Tenant>();
      Mapper.CreateMap<MeterReadingValue, MeterReadingValueDTO>();
      Mapper.CreateMap<StructureNodeDTO, StructureNode>().ForMember((Expression<Func<StructureNode, object>>) (strNode => strNode.Name), (Action<IMemberConfigurationExpression<StructureNodeDTO>>) (s => s.MapFrom<string>((Expression<Func<StructureNodeDTO, string>>) (strNodeDTO => strNodeDTO.Name)))).ForMember((Expression<Func<StructureNode, object>>) (strNode => strNode.Description), (Action<IMemberConfigurationExpression<StructureNodeDTO>>) (s => s.MapFrom<string>((Expression<Func<StructureNodeDTO, string>>) (strNodeDTO => strNodeDTO.Description)))).ForMember((Expression<Func<StructureNode, object>>) (strNode => (object) strNode.EntityId), (Action<IMemberConfigurationExpression<StructureNodeDTO>>) (s => s.ResolveUsing((Func<StructureNodeDTO, object>) (strNodeDTO => (object) this.GetEntityId(strNodeDTO))))).ForMember((Expression<Func<StructureNode, object>>) (strNode => strNode.EntityName), (Action<IMemberConfigurationExpression<StructureNodeDTO>>) (s => s.ResolveUsing((Func<StructureNodeDTO, object>) (strNodeDTO => (object) this.GetEntityName(strNodeDTO))))).ForMember((Expression<Func<StructureNode, object>>) (strNode => strNode.NodeType), (Action<IMemberConfigurationExpression<StructureNodeDTO>>) (s => s.MapFrom<MSS.Core.Model.Structures.StructureNodeType>((Expression<Func<StructureNodeDTO, MSS.Core.Model.Structures.StructureNodeType>>) (strNodeDTO => strNodeDTO.NodeType)))).ForMember((Expression<Func<StructureNode, object>>) (strNode => (object) strNode.StartDate), (Action<IMemberConfigurationExpression<StructureNodeDTO>>) (s => s.Ignore())).ForMember((Expression<Func<StructureNode, object>>) (strNode => (object) strNode.EndDate), (Action<IMemberConfigurationExpression<StructureNodeDTO>>) (s => s.Ignore()));
      Mapper.CreateMap<StructureNodeLinks, StructureNodeLinks>().ForMember((Expression<Func<StructureNodeLinks, object>>) (l => (object) l.Id), (Action<IMemberConfigurationExpression<StructureNodeLinks>>) (s => s.Ignore()));
    }

    private Guid GetEntityId(StructureNodeDTO structureNodeDTO)
    {
      return structureNodeDTO.Entity != null ? (Guid) structureNodeDTO.Entity.GetType().GetProperty("Id").GetValue(structureNodeDTO.Entity) : Guid.Empty;
    }

    private string GetEntityName(StructureNodeDTO structureNodeDTO)
    {
      return structureNodeDTO.Entity != null ? structureNodeDTO.Entity.GetType().Name : string.Empty;
    }

    public RadObservableCollection<Country> GetCountriesForExportJob(AutomatedExportJobDTO exportJob)
    {
      RadObservableCollection<Country> countriesForExportJob = new RadObservableCollection<Country>();
      AutomatedExportJob job = this._automatedExportJobRepository.FirstOrDefault((Expression<Func<AutomatedExportJob, bool>>) (j => j.Id == exportJob.Id));
      IRepository<AutomatedExportJobCountry> repository = this._repositoryFactory.GetRepository<AutomatedExportJobCountry>();
      Expression<Func<AutomatedExportJobCountry, bool>> predicate = (Expression<Func<AutomatedExportJobCountry, bool>>) (jc => jc.AutomatedExportJob == job);
      foreach (AutomatedExportJobCountry exportJobCountry in (IEnumerable<AutomatedExportJobCountry>) repository.SearchFor(predicate))
        countriesForExportJob.Add(exportJobCountry.Country);
      return countriesForExportJob;
    }

    public void SaveImportedStructure(List<string[]> nodesList)
    {
      try
      {
        List<StructureNodeDTO> structureNodeDtoList = new List<StructureNodeDTO>();
        List<StructureNodeEquipmentSettings> equipmentSettingsList = new List<StructureNodeEquipmentSettings>();
        object obj = new object();
        this._nhSession.FlushMode = FlushMode.Commit;
        ITransaction transaction = this._nhSession.BeginTransaction();
        foreach (string[] nodes in nodesList)
        {
          string[] node = nodes;
          if (node[0] == typeof (MeterDTO).Name)
          {
            MeterDTO meterDto = new MeterDTO();
            meterDto.SerialNumber = node[1];
            meterDto.ShortDeviceNo = node[2];
            meterDto.CompletDevice = node[3];
            meterDto.DeviceType = (DeviceTypeEnum) Enum.Parse(typeof (DeviceTypeEnum), node[4]);
            meterDto.Room = this._roomTypeRepository.FirstOrDefault((Expression<Func<RoomType, bool>>) (r => r.Code == node[5]));
            meterDto.StartValue = node[6] != string.Empty ? new double?(double.Parse(node[6])) : new double?();
            meterDto.ReadingUnit = this._measureUniteRepository.FirstOrDefault((Expression<Func<MeasureUnit, bool>>) (ru => ru.Code == node[7]));
            meterDto.DecimalPlaces = node[8] != string.Empty ? new double?(double.Parse(node[8])) : new double?();
            meterDto.Channel = this._channelRepository.FirstOrDefault((Expression<Func<Channel, bool>>) (c => c.Code == node[9]));
            meterDto.ConnectedDeviceType = this._connectedDeviceTypeRepository.FirstOrDefault((Expression<Func<ConnectedDeviceType, bool>>) (cdt => cdt.Code == node[10]));
            meterDto.ReadingEnabled = node.Length > 11 && Convert.ToBoolean(node[11]);
            MeterDTO meterDTO = meterDto;
            obj = (object) this.GetStructuresManagerInstance().TransactionalSaveOrUpdateMeter(meterDTO);
          }
          else if (node[0] == typeof (LocationDTO).Name)
          {
            LocationDTO locationDto = new LocationDTO();
            locationDto.City = node[1];
            locationDto.Street = node[2];
            locationDto.ZipCode = node[3];
            locationDto.BuildingNr = node[4];
            locationDto.Description = node[5];
            locationDto.Generation = (GenerationEnum) Enum.Parse(typeof (GenerationEnum), node[6]);
            locationDto.Scenario = this._repositoryScenario.FirstOrDefault((Expression<Func<Scenario, bool>>) (s => s.Code == int.Parse(node[7])));
            locationDto.DueDate = new DateTime?(DateTime.ParseExact(node[8], "dd-MM-yy hh:mm", (IFormatProvider) null));
            locationDto.HasMaster = new bool?(bool.Parse(node[9]));
            locationDto.Scale = (ScaleEnum) Enum.Parse(typeof (ScaleEnum), node[10]);
            LocationDTO locationDTO = locationDto;
            obj = (object) this.GetStructuresManagerInstance().TransactionalSaveOrUpdateLocation(locationDTO);
          }
          else if (node[0] == typeof (TenantDTO).Name)
          {
            TenantDTO tenantDTO = new TenantDTO()
            {
              TenantNr = int.Parse(node[1]),
              Name = node[2],
              FloorNr = node[3],
              FloorName = node[4],
              ApartmentNr = node[5],
              Description = node[6],
              CustomerTenantNo = node[7],
              Entrance = node[8]
            };
            obj = (object) this.GetStructuresManagerInstance().TransactionalSaveOrUpdateTenant(tenantDTO);
          }
          else if (node[0] == typeof (MinomatSerializableDTO).Name)
          {
            MinomatSerializableDTO minomatDto = new MinomatSerializableDTO()
            {
              AccessPoint = node[1],
              Challenge = node[2],
              CreatedBy = node[3],
              GsmId = node[4],
              HostAndPort = node[5],
              LastUpdatedBy = node[6],
              RadioId = node[7],
              ProviderName = node[8],
              SessionKey = node[9],
              SimPin = node[10],
              Status = node[11],
              Url = node[12],
              UserId = node[13],
              UserPassword = node[14],
              CreatedOn = new DateTime?(node[15] != string.Empty ? DateTime.Parse(node[15]) : DateTime.Now),
              EndDate = new DateTime?(node[16] != string.Empty ? DateTime.Parse(node[16]) : DateTime.Now),
              IsDeactivated = bool.Parse(node[17]),
              IsInMasterPool = bool.Parse(node[18]),
              IsMaster = bool.Parse(node[19]),
              LastChangedOn = new DateTime?(node[20] != string.Empty ? DateTime.Parse(node[20]) : DateTime.Now),
              Polling = int.Parse(node[21]),
              Registered = bool.Parse(node[22]),
              StartDate = new DateTime?(node[23] != string.Empty ? DateTime.Parse(node[23]) : DateTime.Now)
            };
            obj = (object) this.GetStructuresManagerInstance().TransactionalSaveOrUpdateMinomat(minomatDto);
          }
          else if (node[0] == typeof (MeterReadingValue).Name)
          {
            if (obj is MSS.Core.Model.Meters.Meter meter)
            {
              long result;
              this._meterReadingValueRepository.TransactionalInsert(new MeterReadingValue()
              {
                MeterId = meter.Id,
                MeterSerialNumber = node[1],
                StorageInterval = (long) int.Parse(node[2]),
                Date = DateTime.ParseExact(node[3], "dd.MM.yyyy", (IFormatProvider) null),
                Value = double.Parse(node[4]),
                ValueId = long.TryParse(node[6], out result) ? result : 0L,
                CreatedOn = DateTime.Now
              });
            }
          }
          else
          {
            StructureNodeDTO structureNodeDto1 = new StructureNodeDTO();
            structureNodeDto1.Id = Guid.Parse(node[0]);
            structureNodeDto1.Name = node[1];
            structureNodeDto1.NodeType = this._structureNodeTypeRepository.FirstOrDefault((Expression<Func<MSS.Core.Model.Structures.StructureNodeType, bool>>) (n => n.Name == node[2]));
            structureNodeDto1.Description = node[3];
            structureNodeDto1.Entity = node[5] != string.Empty ? obj : (object) null;
            structureNodeDto1.StructureType = new StructureTypeEnum?((StructureTypeEnum) Enum.Parse(typeof (StructureTypeEnum), node[6]));
            structureNodeDto1.ParentNode = node[7] != string.Empty ? structureNodeDtoList.FirstOrDefault<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (sn => sn.Id == Guid.Parse(node[7]))) : (StructureNodeDTO) null;
            structureNodeDto1.OrderNr = node[9] != string.Empty ? Convert.ToInt32(node[9]) : 0;
            StructureNodeDTO structureNodeDto2 = structureNodeDto1;
            if (structureNodeDto2.ParentNode != null)
              structureNodeDto2.ParentNode.SubNodes.Add(structureNodeDto2);
            structureNodeDtoList.Add(structureNodeDto2);
            structureNodeDto2.RootNode = structureNodeDtoList.FirstOrDefault<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (sn => sn.Id == Guid.Parse(node[8])));
            if (node[0] == node[8])
            {
              StructureNodeEquipmentSettings equipmentSettings = new StructureNodeEquipmentSettings()
              {
                EquipmentName = node.Length > 10 ? node[10] : (string) null,
                EquipmentParams = node.Length > 11 ? node[11] : (string) null,
                SystemName = node.Length > 12 ? node[12] : (string) null,
                ScanMode = node.Length > 13 ? node[13] : (string) null,
                ScanParams = node.Length > 14 ? node[14] : (string) null,
                ReadingProfileName = node.Length > 15 ? node[15] : (string) null,
                ReadingProfileParams = node.Length > 16 ? node[16] : (string) null,
                DeviceModelReadingParams = node.Length > 17 ? node[17] : (string) null,
                StructureNode = new StructureNode()
                {
                  Id = structureNodeDto2.Id
                }
              };
              equipmentSettingsList.Add(equipmentSettings);
            }
          }
        }
        List<Guid> idsBeforeInsert = structureNodeDtoList.Select<StructureNodeDTO, Guid>((Func<StructureNodeDTO, Guid>) (sn => sn.Id)).ToList<Guid>();
        foreach (StructureNodeDTO structureNodeDto in structureNodeDtoList)
          structureNodeDto.Id = Guid.Empty;
        StructureTypeEnum structureType = structureNodeDtoList[0].StructureType.Value;
        this.GetStructuresManagerInstance().TransactionalSaveNewStructure((IList<StructureNodeDTO>) structureNodeDtoList, structureType);
        List<Guid> list = structureNodeDtoList.Select<StructureNodeDTO, Guid>((Func<StructureNodeDTO, Guid>) (sn => sn.Id)).ToList<Guid>();
        int index = 0;
        Dictionary<Guid, Guid> idsBeforeAndAfterInsert = new Dictionary<Guid, Guid>();
        list.ForEach((Action<Guid>) (id =>
        {
          idsBeforeAndAfterInsert[idsBeforeInsert.ElementAt<Guid>(index)] = id;
          ++index;
        }));
        foreach (StructureNodeEquipmentSettings entity in equipmentSettingsList)
        {
          entity.StructureNode.Id = idsBeforeAndAfterInsert[entity.StructureNode.Id];
          this._repositoryFactory.GetRepository<StructureNodeEquipmentSettings>().TransactionalInsert(entity);
        }
        transaction.Commit();
      }
      catch (Exception ex)
      {
        this._nhSession.Transaction.Rollback();
        MessageHandler.LogException(ex);
        throw new BaseApplicationException(ErrorCodes.GetErrorMessage("MSSError_9"));
      }
    }

    private StructuresManager GetStructuresManagerInstance()
    {
      return new StructuresManager(this._repositoryFactory);
    }

    public void ExportAllReadingValues(
      bool dataExportLastDaysChoice,
      int numberOfDaysToExport,
      bool sasExportType,
      bool csvFileType,
      bool xmlFileType,
      bool excelFileType,
      bool commaDecimalSeparator,
      bool semicolonValueSeparator,
      BackgroundWorker backgroundWorkerExport,
      DoWorkEventArgs e,
      List<Country> countries)
    {
      backgroundWorkerExport?.ReportProgress(0);
      string str = string.Empty;
      if (csvFileType)
        str = !sasExportType ? "CSV Document for GMM|*.csv" : "CSV Document for SAS|*.csv";
      else if (xmlFileType)
        str = !sasExportType ? "XML Document for GMM|*.xml" : "XML Document for SAS|*.xml";
      else if (excelFileType)
        str = !sasExportType ? "Excel Document for GMM|*.xlsx" : "Excel Document for SAS|*.xlsx";
      SaveFileDialog saveFileDialog1 = new SaveFileDialog();
      saveFileDialog1.Filter = str;
      saveFileDialog1.Title = "Save reading values to file";
      SaveFileDialog saveFileDialog2 = saveFileDialog1;
      saveFileDialog2.ShowDialog();
      if (saveFileDialog2.FileName == string.Empty)
        return;
      this.ExportAllReadingValues(saveFileDialog2.FileName, dataExportLastDaysChoice, numberOfDaysToExport, sasExportType, csvFileType, xmlFileType, excelFileType, commaDecimalSeparator, semicolonValueSeparator, backgroundWorkerExport, e, countries);
    }

    public void ExportAllReadingValues(
      string fileName,
      bool dataExportLastDaysChoice,
      int numberOfDaysToExport,
      bool sasExportType,
      bool csvFileType,
      bool xmlFileType,
      bool excelFileType,
      bool commaDecimalSeparator,
      bool semicolonValueSeparator,
      BackgroundWorker backgroundWorkerExport,
      DoWorkEventArgs e,
      List<Country> countries)
    {
      backgroundWorkerExport?.ReportProgress(0);
      if (fileName == string.Empty)
        return;
      ISessionFactory sessionFactory = HibernateMultipleDatabasesManager.DataSessionFactory(ConfigurationManager.AppSettings["DatabaseEngine"]);
      this._nhSession = sessionFactory.OpenSession();
      this._repositoryFactory.SetSession(this._nhSession);
      int num1 = 0;
      int num2 = dataExportLastDaysChoice ? 1 : 0;
      int numberOfDaysToExport1 = numberOfDaysToExport;
      int num3 = sasExportType ? 1 : 0;
      int num4 = num1;
      int num5 = num4 + 1;
      int batchNumber = num4;
      for (List<MeterReadingValue> meterReadingValues = this.GetMeterReadingValues(num2 != 0, numberOfDaysToExport1, num3 != 0, batchNumber); meterReadingValues.Count > 0; meterReadingValues = this.GetMeterReadingValues(dataExportLastDaysChoice, numberOfDaysToExport, sasExportType, num5++))
      {
        ObservableCollection<MeterReadingValueDTO> meterReadingValueDtos = Mapper.Map<IEnumerable<MeterReadingValue>, ObservableCollection<MeterReadingValueDTO>>((IEnumerable<MeterReadingValue>) meterReadingValues);
        if (meterReadingValueDtos.Count > 0)
          this.ExportReadingValuesBase(sasExportType ? ReportingHelper.FilteredReadingValuesCollection(meterReadingValueDtos) : meterReadingValueDtos, sasExportType, backgroundWorkerExport, e, fileName, csvFileType, xmlFileType, excelFileType, commaDecimalSeparator, semicolonValueSeparator, countries);
        this.SetReadingValuesAsExported(meterReadingValues);
        ((IDisposable) this._repositoryFactory.GetSession()).Dispose();
        this._nhSession = sessionFactory.OpenSession();
        this._repositoryFactory.SetSession(this._nhSession);
      }
      backgroundWorkerExport?.ReportProgress(100);
    }

    private List<MeterReadingValue> GetMeterReadingValues(
      bool dataExportLastDaysChoice,
      int numberOfDaysToExport,
      bool sasExport,
      int batchNumber)
    {
      List<MeterReadingValue> meterReadingValueList = new List<MeterReadingValue>();
      ISession session = this._repositoryFactory.GetSession();
      int count = 500;
      List<MeterReadingValue> meterReadingValues;
      if (!dataExportLastDaysChoice)
      {
        List<MeterReadingValue> list;
        if (!sasExport)
          list = LinqExtensionMethods.Query<MeterReadingValue>(session).Where<MeterReadingValue>((Expression<Func<MeterReadingValue, bool>>) (mrv => mrv.ExportedOn == new DateTime?())).Take<MeterReadingValue>(count).ToList<MeterReadingValue>();
        else
          list = LinqExtensionMethods.Query<MeterReadingValue>(session).Where<MeterReadingValue>((Expression<Func<MeterReadingValue, bool>>) (mrv => mrv.ExportedOn == new DateTime?() && !(mrv.PhysicalQuantity == 18L || mrv.PhysicalQuantity == 1L && mrv.MeterType == 64L))).Take<MeterReadingValue>(count).ToList<MeterReadingValue>();
        meterReadingValues = list;
      }
      else
      {
        DateTime startDate = DateTime.Today.AddDays((double) -numberOfDaysToExport);
        List<MeterReadingValue> list;
        if (!sasExport)
          list = LinqExtensionMethods.Query<MeterReadingValue>(session).Where<MeterReadingValue>((Expression<Func<MeterReadingValue, bool>>) (mrv => mrv.CreatedOn >= startDate)).Skip<MeterReadingValue>(batchNumber * count).Take<MeterReadingValue>(count).ToList<MeterReadingValue>();
        else
          list = LinqExtensionMethods.Query<MeterReadingValue>(session).Where<MeterReadingValue>((Expression<Func<MeterReadingValue, bool>>) (mrv => mrv.CreatedOn >= startDate && !(mrv.PhysicalQuantity == 18L || mrv.PhysicalQuantity == 1L && mrv.MeterType == 64L))).Skip<MeterReadingValue>(batchNumber * count).Take<MeterReadingValue>(count).ToList<MeterReadingValue>();
        meterReadingValues = list;
      }
      return meterReadingValues;
    }

    public void ExportReadingValues(
      ObservableCollection<MeterReadingValueDTO> meterReadingValuesDtoUI,
      bool isSasExport,
      bool commaDecimalSeparator,
      string fileName,
      BackgroundWorker backgroundWorkerExport,
      DoWorkEventArgs e)
    {
      this.ExportReadingValuesBase(meterReadingValuesDtoUI, isSasExport, backgroundWorkerExport, e, fileName, true, false, false, commaDecimalSeparator, true, (List<Country>) null);
    }

    private void ExportReadingValuesBase(
      ObservableCollection<MeterReadingValueDTO> meterReadingValuesDtoUI,
      bool isSasExport,
      BackgroundWorker backgroundWorkerExport,
      DoWorkEventArgs e,
      string fileName,
      bool csvFileType,
      bool xmlFileType,
      bool excelFileType,
      bool commaDecimalSeparator,
      bool semicolonValueSeparator,
      List<Country> countriesFilterList)
    {
      int count1 = meterReadingValuesDtoUI.Count;
      int index = 0;
      int count2 = 500;
      if (count1 < count2)
        count2 = count1;
      List<MeterReadingValueDTO> list1 = meterReadingValuesDtoUI.ToList<MeterReadingValueDTO>();
      string name = commaDecimalSeparator ? "de-DE" : "en-GB";
      string valueSeparator = semicolonValueSeparator ? ";" : ",";
      while (index <= count1)
      {
        IList<MeterReadingValueDTO> meterReadingValueDtoList = index + count2 > count1 ? (IList<MeterReadingValueDTO>) list1.GetRange(index, count1 - index) : (IList<MeterReadingValueDTO>) list1.GetRange(index, count2);
        List<string[]> readingValueList = new List<string[]>();
        List<Guid> list2 = meterReadingValueDtoList.Select<MeterReadingValueDTO, Guid>((Func<MeterReadingValueDTO, Guid>) (x => x.Id)).ToList<Guid>();
        List<OrderReadingValues> orderReadingValuesList = this._repositoryFactory.GetReadingValuesRepository().LoadOrderReadingValues(list2);
        List<JobReadingValues> jobReadingValuesList = this._repositoryFactory.GetReadingValuesRepository().LoadJobReadingValues(list2);
        if (!isSasExport)
        {
          if (index == 0)
          {
            string[] strArray = new string[12]
            {
              "TimePoint",
              "SerialNr",
              "MeterName",
              "Device",
              "Value",
              "Unit",
              "Physical Quantity",
              "Calculation",
              "Storage Interval",
              "Calculation Start",
              "Creation",
              "Index"
            };
            readingValueList.Add(strArray);
          }
          Dictionary<Guid, List<MSS.Core.Model.Meters.Meter>> orderReadingValues = this.GetMetersForOrderReadingValues(meterReadingValueDtoList, orderReadingValuesList);
          List<MSS.Core.Model.Meters.Meter> jobReadingValues = this.GetMetersForJobReadingValues(meterReadingValueDtoList, jobReadingValuesList);
          foreach (MeterReadingValueDTO readingValue in (IEnumerable<MeterReadingValueDTO>) meterReadingValueDtoList)
          {
            MSS.Core.Model.Meters.Meter meterForReadingValue = ReportingManager.GetMeterForReadingValue(readingValue, orderReadingValuesList, jobReadingValuesList, orderReadingValues, jobReadingValues);
            string[] strArray = new string[12]
            {
              readingValue.Date.ToString("dd.MM.yyyy HH:mm:ss"),
              readingValue.MeterSerialNumber,
              readingValue.Name,
              meterForReadingValue != null ? meterForReadingValue.DeviceType.ToString() : string.Empty,
              readingValue.Value.ToString((IFormatProvider) CultureInfo.CreateSpecificCulture(name)),
              readingValue.UnitCode ?? (meterForReadingValue == null || meterForReadingValue.ReadingUnit == null ? string.Empty : meterForReadingValue.ReadingUnit.Code),
              readingValue.PhysicalQuantity.ToString(),
              readingValue.Calculation.ToString(),
              readingValue.StorageInterval == ValueIdent.ValueIdPart_StorageInterval.None ? "Actual" : readingValue.StorageInterval.ToString(),
              readingValue.CalculationStart.ToString(),
              readingValue.Creation.ToString(),
              readingValue.Index.ToString()
            };
            readingValueList.Add(strArray);
            if (backgroundWorkerExport != null)
            {
              if (!backgroundWorkerExport.CancellationPending)
              {
                int percentProgress = index * 100 / count1;
                backgroundWorkerExport.ReportProgress(percentProgress);
              }
              else
              {
                e.Cancel = true;
                break;
              }
            }
          }
          ReportingManager.SaveReadingValuesFile(fileName, csvFileType, xmlFileType, excelFileType, readingValueList, valueSeparator, index == 0, index == count1);
        }
        else
        {
          if (index == 0)
          {
            string[] strArray = new string[9]
            {
              "deConfigNr",
              "deLnr",
              "deMasterRadioId",
              "deRadioId",
              "deDate",
              "deValue",
              "deStatus",
              "deDetail",
              "deIdent"
            };
            readingValueList.Add(strArray);
          }
          Dictionary<Guid, Location> locationsDictionary = new Dictionary<Guid, Location>();
          this.GetMetersForOrderReadingValues(meterReadingValueDtoList, orderReadingValuesList, ref locationsDictionary);
          foreach (MeterReadingValueDTO meterReadingValueDto in (IEnumerable<MeterReadingValueDTO>) meterReadingValueDtoList)
          {
            MeterReadingValueDTO readingValue = meterReadingValueDto;
            Location location = (Location) null;
            location = locationsDictionary.ContainsKey(readingValue.Id) ? locationsDictionary[readingValue.Id] : (Location) null;
            if (location == null || countriesFilterList == null || location.Country == null || !countriesFilterList.All<Country>((Func<Country, bool>) (c => c.Id != location.Country.Id)))
            {
              OrderReadingValues orderReadingValues = orderReadingValuesList.LastOrDefault<OrderReadingValues>((Func<OrderReadingValues, bool>) (orv => orv.MeterReadingValue.Id == readingValue.Id));
              JobReadingValues jobReadingValues = jobReadingValuesList.LastOrDefault<JobReadingValues>((Func<JobReadingValues, bool>) (jrv => jrv.ReadingValue.Id == readingValue.Id));
              string[] strArray = new string[9]
              {
                orderReadingValues == null || orderReadingValues.OrderObj == null ? string.Empty : orderReadingValues.OrderObj.InstallationNumber,
                location != null ? location.BuildingNr : string.Empty,
                jobReadingValues == null || jobReadingValues.Job == null || jobReadingValues.Job.Minomat == null ? string.Empty : jobReadingValues.Job.Minomat.RadioId,
                readingValue.MeterSerialNumber,
                readingValue.Date.ToString("dd.MM.yyyy"),
                readingValue.Value.ToString((IFormatProvider) CultureInfo.CreateSpecificCulture(name)),
                readingValue.Status.ToString(),
                readingValue.StorageInterval == ValueIdent.ValueIdPart_StorageInterval.None ? "Actual" : readingValue.StorageInterval.ToString(),
                "M"
              };
              readingValueList.Add(strArray);
              if (backgroundWorkerExport != null)
              {
                if (!backgroundWorkerExport.CancellationPending)
                {
                  int percentProgress = index * 100 / count1;
                  backgroundWorkerExport.ReportProgress(percentProgress);
                }
                else
                {
                  e.Cancel = true;
                  break;
                }
              }
            }
          }
          if (backgroundWorkerExport != null && backgroundWorkerExport.CancellationPending)
          {
            e.Cancel = true;
            break;
          }
          ReportingManager.SaveReadingValuesFile(fileName, csvFileType, xmlFileType, excelFileType, readingValueList, valueSeparator, index == 0, index == count1);
        }
        if (index + count2 <= count1)
          index += count2;
        else if (index == count1)
        {
          index += count2;
          backgroundWorkerExport?.ReportProgress(100);
        }
        else
          index += count1 - index;
      }
    }

    private static MSS.Core.Model.Meters.Meter GetMeterForReadingValue(
      MeterReadingValueDTO readingValue,
      List<OrderReadingValues> orderReadingValues,
      List<JobReadingValues> jobReadingValues,
      Dictionary<Guid, List<MSS.Core.Model.Meters.Meter>> orderMetersDict,
      List<MSS.Core.Model.Meters.Meter> jobMeters)
    {
      List<MSS.Core.Model.Meters.Meter> source = new List<MSS.Core.Model.Meters.Meter>();
      OrderReadingValues orderReadingValues1 = orderReadingValues.LastOrDefault<OrderReadingValues>((Func<OrderReadingValues, bool>) (orv => orv.MeterReadingValue.Id == readingValue.Id));
      JobReadingValues jobReadingValues1 = jobReadingValues.LastOrDefault<JobReadingValues>((Func<JobReadingValues, bool>) (jrv => jrv.ReadingValue.Id == readingValue.Id));
      if (orderReadingValues1 != null)
      {
        Guid id = orderReadingValues1.OrderObj.Id;
        source = orderMetersDict[id];
      }
      if (jobReadingValues1 != null)
        source = jobMeters;
      return source.FirstOrDefault<MSS.Core.Model.Meters.Meter>((Func<MSS.Core.Model.Meters.Meter, bool>) (x => x.SerialNumber == readingValue.MeterSerialNumber));
    }

    private StructuresManager GetStructureManagerInstance()
    {
      return new StructuresManager(this._repositoryFactory);
    }

    private Dictionary<Guid, List<MSS.Core.Model.Meters.Meter>> GetMetersForOrderReadingValues(
      IList<MeterReadingValueDTO> meterReadingValuesDto,
      List<OrderReadingValues> orderReadingvalues)
    {
      Dictionary<Guid, List<MSS.Core.Model.Meters.Meter>> orderReadingValues1 = new Dictionary<Guid, List<MSS.Core.Model.Meters.Meter>>();
      List<string> readingValuesSerialNumbers = meterReadingValuesDto.Select<MeterReadingValueDTO, string>((Func<MeterReadingValueDTO, string>) (x => x.MeterSerialNumber)).Distinct<string>().ToList<string>();
      if (orderReadingvalues != null && orderReadingvalues.Any<OrderReadingValues>())
      {
        foreach (IEnumerable<OrderReadingValues> source in orderReadingvalues.GroupBy<OrderReadingValues, Guid>((Func<OrderReadingValues, Guid>) (o => o.OrderObj.Id)))
        {
          OrderReadingValues orderReadingValues2 = source.First<OrderReadingValues>();
          OrderSerializableStructure orderserializablestructure = StructuresHelper.DeserializeStructure(orderReadingValues2.OrderObj.StructureBytes);
          List<MSS.Core.Model.Meters.Meter> list = this.GetStructureManagerInstance().GetStructure(orderserializablestructure).Meters.Where<MSS.Core.Model.Meters.Meter>((Func<MSS.Core.Model.Meters.Meter, bool>) (x => readingValuesSerialNumbers.Contains(x.SerialNumber))).ToList<MSS.Core.Model.Meters.Meter>();
          orderReadingValues1.Add(orderReadingValues2.OrderObj.Id, list);
        }
      }
      return orderReadingValues1;
    }

    private List<MSS.Core.Model.Meters.Meter> GetMetersForJobReadingValues(
      IList<MeterReadingValueDTO> meterReadingValuesDto,
      List<JobReadingValues> jobReadingvalues)
    {
      List<MSS.Core.Model.Meters.Meter> jobReadingValues = new List<MSS.Core.Model.Meters.Meter>();
      List<string> readingValuesSerialNumbers = meterReadingValuesDto.Select<MeterReadingValueDTO, string>((Func<MeterReadingValueDTO, string>) (x => x.MeterSerialNumber)).Distinct<string>().ToList<string>();
      if (jobReadingvalues != null && jobReadingvalues.Any<JobReadingValues>())
        jobReadingValues = this._meterRepository.SearchFor((Expression<Func<MSS.Core.Model.Meters.Meter, bool>>) (x => readingValuesSerialNumbers.Contains(x.SerialNumber))).ToList<MSS.Core.Model.Meters.Meter>();
      return jobReadingValues;
    }

    private Dictionary<Guid, List<MSS.Core.Model.Meters.Meter>> GetMetersForOrderReadingValues(
      IList<MeterReadingValueDTO> meterReadingValuesDto,
      List<OrderReadingValues> orderReadingvalues,
      ref Dictionary<Guid, Location> locationsDictionary)
    {
      Dictionary<Guid, List<MSS.Core.Model.Meters.Meter>> orderReadingValues1 = new Dictionary<Guid, List<MSS.Core.Model.Meters.Meter>>();
      Dictionary<Guid, Location> dictionary = new Dictionary<Guid, Location>();
      List<string> readingValuesSerialNumbers = meterReadingValuesDto.Select<MeterReadingValueDTO, string>((Func<MeterReadingValueDTO, string>) (x => x.MeterSerialNumber)).Distinct<string>().ToList<string>();
      if (orderReadingvalues != null && orderReadingvalues.Any<OrderReadingValues>())
      {
        foreach (IEnumerable<OrderReadingValues> source in orderReadingvalues.GroupBy<OrderReadingValues, Guid>((Func<OrderReadingValues, Guid>) (o => o.OrderObj.Id)))
        {
          OrderReadingValues orderReadingValues2 = source.First<OrderReadingValues>();
          OrderSerializableStructure orderserializablestructure = StructuresHelper.DeserializeStructure(orderReadingValues2.OrderObj.StructureBytes);
          Structure structure = this.GetStructureManagerInstance().GetStructure(orderserializablestructure);
          List<MSS.Core.Model.Meters.Meter> list = structure.Meters.Where<MSS.Core.Model.Meters.Meter>((Func<MSS.Core.Model.Meters.Meter, bool>) (x => readingValuesSerialNumbers.Contains(x.SerialNumber))).ToList<MSS.Core.Model.Meters.Meter>();
          orderReadingValues1.Add(orderReadingValues2.OrderObj.Id, list);
          dictionary.Add(orderReadingValues2.OrderObj.Id, structure.Locations.FirstOrDefault<Location>());
        }
      }
      if (orderReadingvalues != null && orderReadingvalues.Any<OrderReadingValues>())
      {
        foreach (OrderReadingValues orderReadingvalue in orderReadingvalues)
        {
          Location location = dictionary[orderReadingvalue.OrderObj.Id];
          if (!locationsDictionary.ContainsKey(orderReadingvalue.MeterReadingValue.Id))
            locationsDictionary.Add(orderReadingvalue.MeterReadingValue.Id, location);
        }
      }
      return orderReadingValues1;
    }

    private List<MSS.Core.Model.Meters.Meter> GetMetersForJobReadingValues(
      IList<MeterReadingValueDTO> meterReadingValuesDto,
      List<JobReadingValues> jobReadingvalues,
      ref Dictionary<Guid, Location> locationsDictionary)
    {
      List<MSS.Core.Model.Meters.Meter> source = new List<MSS.Core.Model.Meters.Meter>();
      List<string> readingValuesSerialNumbers = meterReadingValuesDto.Select<MeterReadingValueDTO, string>((Func<MeterReadingValueDTO, string>) (x => x.MeterSerialNumber)).Distinct<string>().ToList<string>();
      if (jobReadingvalues != null && jobReadingvalues.Any<JobReadingValues>())
      {
        source = this._meterRepository.SearchFor((Expression<Func<MSS.Core.Model.Meters.Meter, bool>>) (x => readingValuesSerialNumbers.Contains(x.SerialNumber))).ToList<MSS.Core.Model.Meters.Meter>();
        Dictionary<Guid, Location> locationsForMeters = this._repositoryFactory.GetStructureNodeRepository().GetLocationsForMeters(source.Select<MSS.Core.Model.Meters.Meter, Guid>((Func<MSS.Core.Model.Meters.Meter, Guid>) (m => m.Id)).ToList<Guid>());
        foreach (JobReadingValues jobReadingvalue in jobReadingvalues)
        {
          JobReadingValues jobReadingValue = jobReadingvalue;
          MSS.Core.Model.Meters.Meter meter = source.FirstOrDefault<MSS.Core.Model.Meters.Meter>((Func<MSS.Core.Model.Meters.Meter, bool>) (m => m.SerialNumber == jobReadingValue.ReadingValue.MeterSerialNumber));
          if (meter != null && locationsForMeters.ContainsKey(meter.Id))
          {
            Location location = locationsForMeters[meter.Id];
            if (!locationsDictionary.ContainsKey(jobReadingValue.ReadingValue.Id))
              locationsDictionary.Add(jobReadingValue.ReadingValue.Id, location);
          }
        }
      }
      return source;
    }

    private static void SaveReadingValuesFile(
      string fileName,
      bool csvFileType,
      bool xmlFileType,
      bool excelFileType,
      List<string[]> readingValueList,
      string valueSeparator,
      bool writeStartElement,
      bool writeEndElement)
    {
      CSVManager csvManager = new CSVManager();
      XMLManager<MeterReadingValueDTO> xmlManager = new XMLManager<MeterReadingValueDTO>();
      XCellManager xcellManager = new XCellManager();
      if (csvFileType)
        csvManager.WriteToFileCultureDependent(fileName, readingValueList, valueSeparator);
      else if (xmlFileType)
      {
        xmlManager.WriteToFile(fileName, readingValueList, writeStartElement, writeEndElement);
      }
      else
      {
        if (!excelFileType)
          return;
        xcellManager.WriteToFile(fileName, readingValueList);
      }
    }

    public void SetReadingValuesAsExported(List<MeterReadingValue> readingValues)
    {
      try
      {
        foreach (MeterReadingValue readingValue in readingValues)
          readingValue.ExportedOn = new DateTime?(DateTime.Now);
        this._nhSession.FlushMode = FlushMode.Commit;
        ITransaction transaction = this._nhSession.BeginTransaction();
        this._repositoryFactory.GetRepository<MeterReadingValue>().TransactionalUpdateMany((IEnumerable<MeterReadingValue>) readingValues);
        transaction.Commit();
      }
      catch (Exception ex)
      {
        this._nhSession.Transaction.Rollback();
        throw;
      }
    }

    public List<string[]> CreateNodeList(List<StructureNodeDTO> nodeCollection)
    {
      List<string[]> nodeList = new List<string[]>();
      Stack<StructureNodeDTO> structureNodeDtoStack = new Stack<StructureNodeDTO>((IEnumerable<StructureNodeDTO>) nodeCollection);
      while (structureNodeDtoStack.Count > 0)
      {
        StructureNodeDTO next = structureNodeDtoStack.Pop();
        Guid guid;
        string empty1;
        if (next.ParentNode != null)
        {
          guid = next.ParentNode.Id;
          empty1 = guid.ToString();
        }
        else
          empty1 = string.Empty;
        string str1 = empty1;
        string empty2;
        if (next.RootNode != null)
        {
          guid = next.RootNode.Id;
          empty2 = guid.ToString();
        }
        else
          empty2 = string.Empty;
        string str2 = empty2;
        string str3;
        if (next.Entity == null)
        {
          guid = Guid.Empty;
          str3 = guid.ToString();
        }
        else
          str3 = next.Entity.GetType().GetProperty("Id").GetValue(next.Entity).ToString();
        string entityId = str3;
        string str4 = next.Entity != null ? next.Entity.GetType().Name : string.Empty;
        if (next.Entity != null)
        {
          nodeList.Add(new ReportingHelper().GetEntityStrings(next.Entity));
          if (next.Entity.GetType() == typeof (MeterDTO))
          {
            IList<MeterReadingValue> source = this._meterReadingValueRepository.SearchFor((Expression<Func<MeterReadingValue, bool>>) (mrv => mrv.MeterId == Guid.Parse(entityId)));
            if (source.Count > 0)
              nodeList.AddRange(source.Select<MeterReadingValue, string[]>((Func<MeterReadingValue, string[]>) (meterReadingValue => new ReportingHelper().GetReadingValueStrings(meterReadingValue))));
          }
        }
        StructureNodeEquipmentSettings equipmentSettings = (StructureNodeEquipmentSettings) null;
        guid = next.Id;
        if (guid.ToString() == str2)
          equipmentSettings = this._repositoryFactory.GetRepository<StructureNodeEquipmentSettings>().FirstOrDefault((Expression<Func<StructureNodeEquipmentSettings, bool>>) (es => es.StructureNode != default (object) && es.StructureNode.Id == next.Id));
        string str5 = equipmentSettings != null ? equipmentSettings.EquipmentName : string.Empty;
        string str6 = equipmentSettings != null ? equipmentSettings.EquipmentParams : string.Empty;
        string str7 = equipmentSettings != null ? equipmentSettings.SystemName : string.Empty;
        string str8 = equipmentSettings != null ? equipmentSettings.ScanMode : string.Empty;
        string str9 = equipmentSettings != null ? equipmentSettings.ScanParams : string.Empty;
        string str10 = equipmentSettings != null ? equipmentSettings.ReadingProfileName : string.Empty;
        string str11 = equipmentSettings != null ? equipmentSettings.ReadingProfileParams : string.Empty;
        string str12 = equipmentSettings != null ? equipmentSettings.DeviceModelReadingParams : string.Empty;
        string[] strArray1 = new string[18];
        guid = next.Id;
        strArray1[0] = guid.ToString();
        strArray1[1] = next.Name;
        strArray1[2] = next.NodeType.Name;
        strArray1[3] = next.Description;
        strArray1[4] = entityId;
        strArray1[5] = str4;
        strArray1[6] = next.StructureType.ToString();
        strArray1[7] = str1;
        strArray1[8] = str2;
        strArray1[9] = next.OrderNr.ToString();
        strArray1[10] = str5;
        strArray1[11] = str6;
        strArray1[12] = str7;
        strArray1[13] = str8;
        strArray1[14] = str9;
        strArray1[15] = str10;
        strArray1[16] = str11;
        strArray1[17] = str12;
        string[] strArray2 = strArray1;
        nodeList.Add(strArray2);
        foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) next.SubNodes)
          structureNodeDtoStack.Push(subNode);
      }
      return nodeList;
    }

    public List<string[]> CreateReadingValuesList(List<MeterReadingValueDTO> meterReadingValues)
    {
      List<string[]> readingValuesList = new List<string[]>();
      readingValuesList.Add(new string[9]
      {
        Resources.MSS_Client_ReadingValues_Date,
        Resources.MSS_Client_Structures_Header_SerialNumber,
        Resources.MSS_Client_ReadingValues_Value,
        Resources.MSS_Client_DataFilters_PhysicalQuantity,
        Resources.MSS_Client_DataFilters_MeterType,
        Resources.MSS_Client_DataFilters_Calculation,
        Resources.MSS_Client_DataFilters_CalculationStart,
        Resources.MSS_Client_DataFilters_StorageInterval,
        Resources.MSS_Client_DataFilters_Creation
      });
      foreach (MeterReadingValueDTO meterReadingValue in meterReadingValues)
        readingValuesList.Add(new string[9]
        {
          meterReadingValue.Date.ToString("dd.MM.yyyy"),
          meterReadingValue.MeterSerialNumber,
          meterReadingValue.Value.ToString((IFormatProvider) Thread.CurrentThread.CurrentCulture),
          meterReadingValue.PhysicalQuantity.ToString(),
          meterReadingValue.MeterType.ToString(),
          meterReadingValue.Calculation.ToString(),
          meterReadingValue.CalculationStart.ToString(),
          meterReadingValue.StorageInterval.ToString(),
          meterReadingValue.Creation.ToString()
        });
      return readingValuesList;
    }

    public List<string[]> CreateReadingValuesListForCsvOrPdfExport(
      List<MeterReadingValueDTO> meterReadingValues)
    {
      List<string[]> forCsvOrPdfExport = new List<string[]>();
      forCsvOrPdfExport.Add(new string[4]
      {
        Resources.MSS_Client_ReadingValues_Date,
        Resources.MSS_Client_ReadingValues_Value,
        Resources.MSS_Client_ReadingValues_Unit,
        Resources.MSS_Client_ReadingValues_Description
      });
      foreach (MeterReadingValueDTO meterReadingValue in meterReadingValues)
        forCsvOrPdfExport.Add(new string[4]
        {
          meterReadingValue.Date.ToString("dd.MM.yyyy HH:mm:ss"),
          meterReadingValue.Value.ToString((IFormatProvider) Thread.CurrentThread.CurrentCulture),
          meterReadingValue.UnitCode,
          meterReadingValue.PhysicalQuantity.ToString() + ", " + (object) meterReadingValue.Calculation
        });
      return forCsvOrPdfExport;
    }

    public List<string[]> CreateDeviceList(List<StructureNodeDTO> nodeCollection)
    {
      List<string[]> deviceList = new List<string[]>();
      Stack<StructureNodeDTO> structureNodeDtoStack = new Stack<StructureNodeDTO>((IEnumerable<StructureNodeDTO>) nodeCollection);
      string[] strArray1 = new string[11]
      {
        "dlLFNr",
        "dnBem",
        "dgZaehlerNr",
        "dgFunkId",
        "dgMessbereich",
        "dgErfasst",
        "dgAnfangsStand",
        "dgAnfStandExtZ",
        "dgRaumSchl",
        "dnWohnung",
        " dnStrAbw"
      };
      deviceList.Add(strArray1);
      while (structureNodeDtoStack.Count > 0)
      {
        StructureNodeDTO structureNodeDto = structureNodeDtoStack.Pop();
        if (structureNodeDto.Entity is MeterDTO)
        {
          MeterDTO entity = structureNodeDto.Entity as MeterDTO;
          MSS.Core.Model.Meters.Meter byId = this._repositoryFactory.GetRepository<MSS.Core.Model.Meters.Meter>().GetById((object) entity.Id);
          string[] strArray2 = new string[11]
          {
            structureNodeDto.RootNode == null || structureNodeDto.RootNode.Entity == null ? string.Empty : (structureNodeDto.RootNode.Entity as LocationDTO).BuildingNr,
            structureNodeDto.ParentNode == null || structureNodeDto.ParentNode.Entity == null ? string.Empty : (structureNodeDto.ParentNode.Entity as TenantDTO).Name,
            entity.DeviceType == DeviceTypeEnum.M7 || entity.DeviceType == DeviceTypeEnum.M6 ? entity.SerialNumber : entity.CompletDevice,
            byId.SerialNumber,
            byId == null || !byId.MeterRadioDetailsList.Any<MeterRadioDetails>() ? string.Empty : byId.MeterRadioDetailsList.First<MeterRadioDetails>().dgMessbereich,
            byId.LastChangedOn.HasValue ? byId.LastChangedOn.Value.ToString("yyyy-MM-dd") : string.Empty,
            byId.StartValue.HasValue ? byId.StartValue.ToString() : string.Empty,
            byId.StartValueImpulses,
            byId.Room != null ? byId.Room.Code : string.Empty,
            structureNodeDto.ParentNode == null || structureNodeDto.ParentNode.Entity == null ? string.Empty : (structureNodeDto.ParentNode.Entity as TenantDTO).CustomerTenantNo,
            structureNodeDto.ParentNode == null || structureNodeDto.ParentNode.Entity == null ? string.Empty : (structureNodeDto.ParentNode.Entity as TenantDTO).Description
          };
          deviceList.Add(strArray2);
        }
        foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) structureNodeDto.SubNodes)
          structureNodeDtoStack.Push(subNode);
      }
      return deviceList;
    }

    public void CreateExportJob(AutomatedExportJob automatedExportJob)
    {
      try
      {
        this._repositoryFactory.GetSession().BeginTransaction();
        automatedExportJob.CreatedOn = DateTime.Now;
        this._automatedExportJobRepository.TransactionalInsert(automatedExportJob);
        this._repositoryFactory.GetSession().Transaction.Commit();
      }
      catch (Exception ex)
      {
        this._repositoryFactory.GetSession().Transaction.Rollback();
        throw;
      }
    }

    public void CreateSasJobCountryConnections(
      AutomatedExportJob automatedExportJob,
      RadObservableCollection<Country> selectedCountries)
    {
      IRepository<AutomatedExportJobCountry> repository = this._repositoryFactory.GetRepository<AutomatedExportJobCountry>();
      foreach (Country selectedCountry in (Collection<Country>) selectedCountries)
        repository.Insert(new AutomatedExportJobCountry()
        {
          Country = selectedCountry,
          AutomatedExportJob = automatedExportJob
        });
    }

    public void UpdateExportJob(AutomatedExportJob automatedExportJob)
    {
      try
      {
        this._repositoryFactory.GetSession().BeginTransaction();
        this._automatedExportJobRepository.TransactionalUpdate(automatedExportJob);
        this._repositoryFactory.GetSession().Transaction.Commit();
      }
      catch (Exception ex)
      {
        this._repositoryFactory.GetSession().Transaction.Rollback();
        throw;
      }
    }

    public void UpdateSasJobCountryConnections(
      AutomatedExportJob automatedExportJob,
      RadObservableCollection<Country> selectedCountries)
    {
      IRepository<AutomatedExportJobCountry> repository1 = this._repositoryFactory.GetRepository<AutomatedExportJobCountry>();
      IRepository<AutomatedExportJobCountry> repository2 = repository1;
      Expression<Func<AutomatedExportJobCountry, bool>> predicate = (Expression<Func<AutomatedExportJobCountry, bool>>) (j => j.AutomatedExportJob == automatedExportJob);
      foreach (AutomatedExportJobCountry entity in (IEnumerable<AutomatedExportJobCountry>) repository2.SearchFor(predicate))
        repository1.Delete(entity);
      foreach (Country selectedCountry in (Collection<Country>) selectedCountries)
        repository1.Insert(new AutomatedExportJobCountry()
        {
          Country = selectedCountry,
          AutomatedExportJob = automatedExportJob
        });
    }

    public void DeleteExportJob(AutomatedExportJob automatedExportJob)
    {
      try
      {
        this._repositoryFactory.GetSession().BeginTransaction();
        this._automatedExportJobRepository.TransactionalDelete(automatedExportJob);
        this._repositoryFactory.GetSession().Transaction.Commit();
      }
      catch (Exception ex)
      {
        this._repositoryFactory.GetSession().Transaction.Rollback();
        throw;
      }
    }

    public bool ExistingRootNode(List<string[]> nodesList)
    {
      if (nodesList[0][0] == typeof (LocationDTO).Name)
      {
        string buildingNr = nodesList[0][4];
        List<Guid> entityIds = this._locationRepository.SearchFor((Expression<Func<Location, bool>>) (l => l.BuildingNr == buildingNr)).Select<Location, Guid>((Func<Location, Guid>) (l => l.Id)).ToList<Guid>();
        return this._structureNodeRepository.FirstOrDefault((Expression<Func<StructureNode, bool>>) (n => entityIds.Contains(n.EntityId) && n.EndDate == new DateTime?())) != null;
      }
      return this._structureNodeRepository.FirstOrDefault((Expression<Func<StructureNode, bool>>) (n => n.Name == nodesList[0][1] && n.EndDate == new DateTime?())) != null;
    }

    public List<string[]> ExistingMeters(List<string[]> nodesList)
    {
      return this.GetExistingMetersInDB(this.GetMetersInFile(nodesList));
    }

    private List<string[]> GetExistingMetersInDB(List<string[]> meterList)
    {
      List<string[]> existingMetersInDb = new List<string[]>();
      for (int i = 0; i < meterList.Count; i += 2)
      {
        List<Guid> meterEntityIds = this._meterRepository.SearchFor((Expression<Func<MSS.Core.Model.Meters.Meter, bool>>) (m => m.SerialNumber == meterList[i][1].ToString())).Select<MSS.Core.Model.Meters.Meter, Guid>((Func<MSS.Core.Model.Meters.Meter, Guid>) (m => m.Id)).ToList<Guid>();
        if (this._structureNodeRepository.FirstOrDefault((Expression<Func<StructureNode, bool>>) (n => meterEntityIds.Contains(n.EntityId) && n.EndDate == new DateTime?())) != null)
        {
          existingMetersInDb.Add(meterList[i]);
          existingMetersInDb.Add(meterList[i + 1]);
        }
      }
      return existingMetersInDb;
    }

    public List<string[]> GetMetersInFile(List<string[]> nodesList)
    {
      return nodesList.Where<string[]>((Func<string[], bool>) (nodeList => nodeList[0] == typeof (MeterDTO).Name || nodeList[5] == typeof (MeterDTO).Name)).ToList<string[]>();
    }
  }
}
