// Decompiled with JetBrains decompiler
// Type: GMMToMSSMigrator.GMMDataProvider
// Assembly: GMMToMSSMigrator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3ACF3C29-B99D-4830-8DFE-AD2278C0306B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMMToMSSMigrator.dll

using AutoMapper;
using MSS.Business.DTO;
using MSS.Business.Modules.StructuresManagement;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Structures;
using MSS.DTO.Meters;
using MSS.Interfaces;
using MSS.Localisation;
using ReadoutConfiguration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Linq.Expressions;
using ZR_ClassLibrary;

#nullable disable
namespace GMMToMSSMigrator
{
  public class GMMDataProvider
  {
    private readonly string _connectionString;
    private readonly IRepositoryFactory _repositoryFactory;
    private IRepository<MeasureUnit> _measureUnitRepository;
    private List<MSS.Core.Model.Structures.StructureNodeType> _mssEntityTypesList;
    private List<MeasureUnit> _measureUnits;
    private Dictionary<Tuple<byte, byte, byte, byte, byte, byte, byte>, long> _calculatedValueIdentDictionary;
    private Dictionary<string, Guid> _serialNumberWithMeterDictionary;
    private List<string> _importedMeterSerialNumbers;

    public GMMDataProvider(string gmmDbPath, IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._measureUnitRepository = repositoryFactory.GetRepository<MeasureUnit>();
      this._measureUnits = this._measureUnitRepository.GetAll().ToList<MeasureUnit>();
      this._mssEntityTypesList = this._repositoryFactory.GetRepository<MSS.Core.Model.Structures.StructureNodeType>().GetAll().ToList<MSS.Core.Model.Structures.StructureNodeType>();
      this._serialNumberWithMeterDictionary = new Dictionary<string, Guid>();
      this._calculatedValueIdentDictionary = new Dictionary<Tuple<byte, byte, byte, byte, byte, byte, byte>, long>();
      this._importedMeterSerialNumbers = new List<string>();
      this._connectionString = string.Format("data source={0};Password=meterdbpass;", (object) gmmDbPath);
      Mapper.CreateMap<MSS.Core.Model.Meters.Meter, MeterDTO>();
    }

    public List<GMMRowValidationResult> GetStructuresAndMeters(
      out List<StructureNodeDTO> structureNodeDtos)
    {
      List<GMMRowValidationResult> structuresAndMeters1 = new List<GMMRowValidationResult>();
      List<GMMStructureDTO> gmmStructureDtos = this.GetGmmStructureDtos();
      if (gmmStructureDtos == null || gmmStructureDtos.Count == 0)
      {
        structuresAndMeters1.Add(new GMMRowValidationResult()
        {
          IsSuccess = false,
          Message = Resources.MSS_Client_GMMMigration_NoData
        });
        structureNodeDtos = (List<StructureNodeDTO>) null;
        return structuresAndMeters1;
      }
      structureNodeDtos = new List<StructureNodeDTO>();
      List<GMMRowValidationResult> structuresAndMeters2 = this.ValidateStructuresAndMeters(gmmStructureDtos);
      foreach (GMMStructureDTO gmmNode in gmmStructureDtos)
      {
        StructureNodeDTO structureNodeDto = this.BuildMssNodeWithoutLinks(gmmNode);
        structureNodeDtos.Add(structureNodeDto);
      }
      int index1 = -1;
      foreach (StructureNodeDTO structureNodeDto in structureNodeDtos)
      {
        ++index1;
        GMMStructureDTO currentGmmNode = gmmStructureDtos.ElementAt<GMMStructureDTO>(index1);
        if (currentGmmNode.ParentID != 0)
        {
          GMMStructureDTO gmmParentNode = gmmStructureDtos.First<GMMStructureDTO>((System.Func<GMMStructureDTO, bool>) (item => item.NodeID == currentGmmNode.ParentID));
          int index2 = gmmStructureDtos.FindIndex((Predicate<GMMStructureDTO>) (item => item == gmmParentNode));
          structureNodeDto.ParentNode = structureNodeDtos.ElementAt<StructureNodeDTO>(index2);
          GMMStructureDTO gmmRootNode = currentGmmNode;
          do
          {
            gmmRootNode = gmmStructureDtos.First<GMMStructureDTO>((System.Func<GMMStructureDTO, bool>) (item => item.NodeID == gmmRootNode.ParentID));
          }
          while (gmmRootNode.ParentID != 0);
          int index3 = gmmStructureDtos.FindIndex((Predicate<GMMStructureDTO>) (item => item == gmmRootNode));
          structureNodeDto.RootNode = structureNodeDtos.ElementAt<StructureNodeDTO>(index3);
        }
        else
          structureNodeDto.RootNode = structureNodeDto;
        structureNodeDto.Entity = this.BuildEntity(currentGmmNode, structureNodeDto.NodeType.Name);
      }
      return structuresAndMeters2;
    }

    private List<GMMStructureDTO> GetGmmStructureDtos()
    {
      DataSet dataSet = this.GetDataSet("SELECT\r\n                                      NodeReferences.NodeID,\r\n                                      NodeReferences.ParentID,\r\n                                      NodeReferences.NodeOrder,\r\n                                      NodeReferences.LayerID,\r\n                                      NodeList.NodeTypeID,\r\n                                      NodeList.NodeName,\r\n                                      NodeList.NodeDescription,\r\n                                      NodeList.NodeSettings,\r\n                                      NodeList.ValidFrom,\r\n                                      NodeList.ValidTo,\r\n                                      Meter.SerialNr,\r\n                                      Meter.ProductionDate\r\n                                FROM\r\n                                      NodeReferences\r\n                                      INNER JOIN NodeList ON(NodeReferences.NodeID = NodeList.NodeID)\r\n                                      LEFT OUTER JOIN Meter ON(Meter.MeterID = NodeList.MeterID)");
      return dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0 ? dataSet.Tables[0].AsEnumerable().Select<DataRow, GMMStructureDTO>((System.Func<DataRow, GMMStructureDTO>) (row => new GMMStructureDTO()
      {
        NodeID = row.Field<int>("NodeID"),
        ParentID = row.Field<int>("ParentID"),
        NodeOrder = row.Field<int>("NodeOrder"),
        LayerID = row.Field<int>("LayerID"),
        NodeTypeID = row.Field<int>("NodeTypeID"),
        NodeName = row.Field<string>("NodeName"),
        NodeDescription = row.Field<string>("NodeDescription"),
        NodeSettings = row.Field<string>("NodeSettings"),
        ValidFrom = row.Field<DateTime?>("ValidFrom"),
        ValidTo = row.Field<DateTime?>("ValidTo"),
        SerialNr = row.Field<string>("SerialNr"),
        ProductionDate = row.Field<DateTime?>("ProductionDate")
      })).ToList<GMMStructureDTO>() : (List<GMMStructureDTO>) null;
    }

    public List<MeterReadingValue> GetReadingValuesBatch(ulong startIndex, ulong batchSize)
    {
      List<MeterReadingValue> meterReadingValueList = new List<MeterReadingValue>();
      List<GMMReadingValuesDTO> readingValuesBatch1 = this.GetGMMReadingValuesBatch(startIndex, batchSize);
      List<MeterReadingValue> readingValuesBatch2 = new List<MeterReadingValue>();
      foreach (GMMReadingValuesDTO gmmReadingValue in readingValuesBatch1)
        readingValuesBatch2.Add(this.BuildMeterReadingValue(gmmReadingValue));
      return readingValuesBatch2;
    }

    private List<GMMReadingValuesDTO> GetGMMReadingValuesBatch(ulong startIndex, ulong batchSize)
    {
      DataSet dataSet = this.GetDataSet("SELECT\tMeter.SerialNr,\r\n                                            MeterValues.TimePoint,\r\n                                            MeterValues.Value,\r\n                                            MeterValues.PhysicalQuantity,\r\n                                            MeterValues.MeterType,\r\n                                            MeterValues.Calculation,\r\n                                            MeterValues.CalculationStart,\r\n                                            MeterValues.StorageInterval,\r\n                                            MeterValues.Creation,\r\n                                            MeterValues.ValueIdentIndex\r\n                                FROM  MeterValues\r\n                                      LEFT OUTER JOIN Meter on (Meter.MeterID = MeterValues.MeterId) \r\n                                LIMIT " + startIndex.ToString() + ", " + batchSize.ToString());
      return dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0 ? dataSet.Tables[0].AsEnumerable().Select<DataRow, GMMReadingValuesDTO>((System.Func<DataRow, GMMReadingValuesDTO>) (row => new GMMReadingValuesDTO()
      {
        SerialNr = row.Field<string>("SerialNr"),
        TimePoint = row.Field<DateTime?>("TimePoint"),
        Value = row.Field<double>("Value"),
        PhysicalQuantity = row.Field<int>("PhysicalQuantity"),
        MeterType = row.Field<int>("MeterType"),
        Calculation = row.Field<int>("Calculation"),
        CalculationStart = row.Field<int>("CalculationStart"),
        StorageInterval = row.Field<int>("StorageInterval"),
        Creation = row.Field<int>("Creation"),
        ValueIdentIndex = row.Field<int>("ValueIdentIndex")
      })).ToList<GMMReadingValuesDTO>() : (List<GMMReadingValuesDTO>) null;
    }

    public ulong? GetNumberOfReadingValues()
    {
      ulong result;
      return ulong.TryParse(this.GetScalar("SELECT COUNT(*) \r\n                                  FROM MeterValues").ToString(), out result) ? new ulong?(result) : new ulong?();
    }

    public void SetMssGuidsForImportedMeters()
    {
      this._importedMeterSerialNumbers.Remove((string) null);
      foreach (string meterSerialNumber in this._importedMeterSerialNumbers)
      {
        string serialNumber = meterSerialNumber;
        MSS.Core.Model.Meters.Meter meter = this._repositoryFactory.GetRepository<MSS.Core.Model.Meters.Meter>().FirstOrDefault((Expression<System.Func<MSS.Core.Model.Meters.Meter, bool>>) (item => item.SerialNumber == serialNumber));
        if (meter != null)
          this._serialNumberWithMeterDictionary.Add(serialNumber, meter.Id);
      }
    }

    private DataSet GetDataSet(string sqlCommand)
    {
      DataSet dataSet = new DataSet();
      using (SQLiteConnection sqLiteConnection = new SQLiteConnection(this._connectionString))
      {
        sqLiteConnection.Open();
        SQLiteCommand command = sqLiteConnection.CreateCommand();
        command.CommandText = sqlCommand;
        DataTable table = new DataTable();
        table.Load((IDataReader) command.ExecuteReader());
        dataSet.Tables.Add(table);
      }
      return dataSet;
    }

    private object GetScalar(string sqlCommand)
    {
      object scalar;
      using (SQLiteConnection sqLiteConnection = new SQLiteConnection(this._connectionString))
      {
        sqLiteConnection.Open();
        SQLiteCommand command = sqLiteConnection.CreateCommand();
        command.CommandText = sqlCommand;
        scalar = command.ExecuteScalar();
      }
      return scalar;
    }

    private List<GMMRowValidationResult> ValidateStructuresAndMeters(
      List<GMMStructureDTO> gmmStructureDtos)
    {
      List<GMMRowValidationResult> validationResultList = new List<GMMRowValidationResult>();
      foreach (GMMStructureDTO gmmStructureDto in gmmStructureDtos)
      {
        GMMRowValidationResult validationResult = new GMMRowValidationResult()
        {
          IsSuccess = true,
          Message = ""
        };
        if (!Enum.IsDefined(typeof (GMMNodeTypeEnum), (object) gmmStructureDto.NodeTypeID))
        {
          validationResult.IsSuccess = false;
          validationResult.Message += "Invalid node type! ";
        }
        if (!validationResult.IsSuccess)
          validationResult.Message = "GMM Node ID: " + (object) gmmStructureDto.NodeID + "   " + validationResult.Message;
        validationResultList.Add(validationResult);
      }
      return validationResultList;
    }

    private StructureNodeDTO BuildMssNodeWithoutLinks(GMMStructureDTO gmmNode)
    {
      StructureNodeDTO structureNodeDto = new StructureNodeDTO();
      structureNodeDto.Name = gmmNode.NodeName;
      GMMNodeTypeEnum nodeTypeId = (GMMNodeTypeEnum) gmmNode.NodeTypeID;
      StructureNodeTypeEnum mssEntityType;
      structureNodeDto.NodeType = !Enum.TryParse<StructureNodeTypeEnum>(nodeTypeId.ToString(), out mssEntityType) ? (MSS.Core.Model.Structures.StructureNodeType) null : this._mssEntityTypesList.First<MSS.Core.Model.Structures.StructureNodeType>((System.Func<MSS.Core.Model.Structures.StructureNodeType, bool>) (item => item.Name == mssEntityType.ToString()));
      structureNodeDto.Description = gmmNode.NodeDescription;
      structureNodeDto.IsNewNode = false;
      structureNodeDto.StructureType = gmmNode.LayerID != 0 ? (gmmNode.LayerID != 1 ? new StructureTypeEnum?() : new StructureTypeEnum?(StructureTypeEnum.Logical)) : new StructureTypeEnum?(StructureTypeEnum.Physical);
      structureNodeDto.OrderNr = gmmNode.NodeOrder;
      structureNodeDto.StartDate = gmmNode.ValidFrom;
      structureNodeDto.EndDate = gmmNode.ValidTo;
      structureNodeDto.StructureType = gmmNode.LayerID == 0 ? new StructureTypeEnum?(StructureTypeEnum.Physical) : (gmmNode.LayerID == 1 ? new StructureTypeEnum?(StructureTypeEnum.Logical) : new StructureTypeEnum?());
      structureNodeDto.IsNewNode = true;
      return structureNodeDto;
    }

    private object BuildEntity(GMMStructureDTO gmmNode, string mssNodeTypeName)
    {
      object obj = (object) null;
      if (mssNodeTypeName == "Meter" && !string.IsNullOrEmpty(gmmNode.SerialNr))
      {
        MSS.Core.Model.Meters.Meter source = this._repositoryFactory.GetRepository<MSS.Core.Model.Meters.Meter>().FirstOrDefault((Expression<System.Func<MSS.Core.Model.Meters.Meter, bool>>) (item => item.SerialNumber == gmmNode.SerialNr));
        MeterDTO destination = new MeterDTO();
        if (source != null)
          Mapper.Map<MSS.Core.Model.Meters.Meter, MeterDTO>(source, destination);
        else
          destination.SerialNumber = gmmNode.SerialNr;
        string parameter1 = ParameterService.GetParameter(gmmNode.NodeSettings, "SID");
        string parameter2 = ParameterService.GetParameter(gmmNode.NodeSettings, "MAN");
        string parameter3 = ParameterService.GetParameter(gmmNode.NodeSettings, "GEN");
        bool isRadio = gmmNode.NodeSettings.Contains("RSSI;");
        destination.Medium = new DeviceMediumEnum?((DeviceMediumEnum) Enum.Parse(typeof (DeviceMediumEnum), ParameterService.GetParameter(gmmNode.NodeSettings, "MED").ToUpper()));
        destination.Manufacturer = parameter2;
        destination.Generation = parameter3;
        DeviceTypeEnum? byDeviceModelName = StructuresHelper.GetDeviceTypeEnumByDeviceModelName(new DeviceManager().DetermineDeviceModel(parameter1, parameter2, parameter3, isRadio).Name);
        if (byDeviceModelName.HasValue)
          destination.DeviceType = byDeviceModelName.Value;
        string parameter4 = ParameterService.GetParameter(gmmNode.NodeSettings, "RADR");
        if (!string.IsNullOrEmpty(parameter4))
          destination.PrimaryAddress = new int?(int.Parse(parameter4));
        destination.ConfigDate = gmmNode.ProductionDate;
        obj = (object) destination;
        if (!this._importedMeterSerialNumbers.Contains(gmmNode.SerialNr))
          this._importedMeterSerialNumbers.Add(gmmNode.SerialNr);
      }
      return obj;
    }

    private MeterReadingValue BuildMeterReadingValue(GMMReadingValuesDTO gmmReadingValue)
    {
      MeterReadingValue meterReadingValue = new MeterReadingValue();
      meterReadingValue.MeterSerialNumber = gmmReadingValue.SerialNr;
      if (gmmReadingValue.TimePoint.HasValue)
        meterReadingValue.Date = gmmReadingValue.TimePoint.Value;
      meterReadingValue.Value = gmmReadingValue.Value;
      meterReadingValue.PhysicalQuantity = (long) gmmReadingValue.PhysicalQuantity;
      meterReadingValue.MeterType = (long) gmmReadingValue.MeterType;
      meterReadingValue.Calculation = (long) gmmReadingValue.Calculation;
      meterReadingValue.CalculationStart = (long) gmmReadingValue.CalculationStart;
      meterReadingValue.StorageInterval = (long) gmmReadingValue.StorageInterval;
      meterReadingValue.Creation = (long) gmmReadingValue.Creation;
      long num;
      if (this._calculatedValueIdentDictionary.TryGetValue(new Tuple<byte, byte, byte, byte, byte, byte, byte>((byte) gmmReadingValue.ValueIdentIndex, (byte) gmmReadingValue.PhysicalQuantity, (byte) gmmReadingValue.MeterType, (byte) gmmReadingValue.Calculation, (byte) gmmReadingValue.CalculationStart, (byte) gmmReadingValue.StorageInterval, (byte) gmmReadingValue.Creation), out num))
      {
        meterReadingValue.ValueId = num;
      }
      else
      {
        meterReadingValue.ValueId = Convert.ToInt64(ValueIdent.GetValueIdent((byte) gmmReadingValue.ValueIdentIndex, (byte) gmmReadingValue.PhysicalQuantity, (byte) gmmReadingValue.MeterType, (byte) gmmReadingValue.Calculation, (byte) gmmReadingValue.CalculationStart, (byte) gmmReadingValue.StorageInterval, (byte) gmmReadingValue.Creation));
        this._calculatedValueIdentDictionary.Add(new Tuple<byte, byte, byte, byte, byte, byte, byte>((byte) gmmReadingValue.ValueIdentIndex, (byte) gmmReadingValue.PhysicalQuantity, (byte) gmmReadingValue.MeterType, (byte) gmmReadingValue.Calculation, (byte) gmmReadingValue.CalculationStart, (byte) gmmReadingValue.StorageInterval, (byte) gmmReadingValue.Creation), meterReadingValue.ValueId);
      }
      meterReadingValue.CreatedOn = DateTime.Now;
      meterReadingValue.MeterId = this._serialNumberWithMeterDictionary.FirstOrDefault<KeyValuePair<string, Guid>>((System.Func<KeyValuePair<string, Guid>, bool>) (item => item.Key == meterReadingValue.MeterSerialNumber)).Value;
      string unitName = ValueIdent.GetUnit(meterReadingValue.ValueId);
      MeasureUnit measureUnit = this._measureUnits.FirstOrDefault<MeasureUnit>((System.Func<MeasureUnit, bool>) (m => m.Code == unitName));
      if (measureUnit == null && !string.IsNullOrEmpty(unitName))
      {
        this._measureUnitRepository.TransactionalInsert(new MeasureUnit()
        {
          Code = unitName
        });
        this._measureUnits = this._measureUnitRepository.GetAll().ToList<MeasureUnit>();
        measureUnit = this._measureUnits.FirstOrDefault<MeasureUnit>((System.Func<MeasureUnit, bool>) (m => m.Code == unitName));
      }
      meterReadingValue.Unit = measureUnit;
      return meterReadingValue;
    }
  }
}
