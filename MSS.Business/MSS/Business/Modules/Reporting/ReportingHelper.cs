// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Reporting.ReportingHelper
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Utils;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Reporting;
using MSS.DTO.Meters;
using MSS.DTO.Structures;
using MSS.Localisation;
using MSSArchive.Core.Model.Jobs;
using MSSArchive.Core.Model.Meters;
using MSSArchive.Core.Model.Orders;
using MSSArchive.Core.Model.Reporting;
using MSSArchive.Core.Model.Structures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using ZR_ClassLibrary;

#nullable disable
namespace MSS.Business.Modules.Reporting
{
  public class ReportingHelper
  {
    public string[] GetEntityStrings(object entity)
    {
      string str1 = entity != null ? entity.GetType().Name : string.Empty;
      switch (str1)
      {
        case "MeterDTO":
          if (entity is MeterDTO meterDto)
          {
            string str2 = meterDto.Room == null ? string.Empty : meterDto.Room.Code;
            string str3 = meterDto.ReadingUnit == null ? string.Empty : meterDto.ReadingUnit.Code;
            string str4 = meterDto.Channel == null ? string.Empty : meterDto.Channel.Code;
            string str5 = meterDto.ConnectedDeviceType == null ? string.Empty : meterDto.ConnectedDeviceType.Code;
            string[] entityStrings = new string[12]
            {
              str1,
              meterDto.SerialNumber,
              meterDto.ShortDeviceNo,
              meterDto.CompletDevice,
              meterDto.DeviceType.ToString(),
              str2,
              null,
              null,
              null,
              null,
              null,
              null
            };
            double? nullable = meterDto.StartValue;
            entityStrings[6] = nullable.ToString();
            entityStrings[7] = str3;
            nullable = meterDto.DecimalPlaces;
            entityStrings[8] = nullable.ToString();
            entityStrings[9] = str4;
            entityStrings[10] = str5;
            entityStrings[11] = meterDto.ReadingEnabled.ToString();
            return entityStrings;
          }
          break;
        case "LocationDTO":
          if (entity is LocationDTO locationDto)
          {
            string str6 = locationDto.Scenario == null ? string.Empty : locationDto.Scenario.Code.ToString((IFormatProvider) CultureInfo.InvariantCulture);
            string[] entityStrings = new string[11]
            {
              str1,
              locationDto.City,
              locationDto.Street,
              locationDto.ZipCode,
              locationDto.BuildingNr,
              locationDto.Description,
              locationDto.Generation.ToString(),
              str6,
              null,
              null,
              null
            };
            DateTime? dueDate = locationDto.DueDate;
            string str7;
            if (!dueDate.HasValue)
            {
              str7 = "";
            }
            else
            {
              dueDate = locationDto.DueDate;
              str7 = dueDate.Value.ToString("dd-MM-yy hh:mm");
            }
            entityStrings[8] = str7;
            entityStrings[9] = locationDto.HasMaster.ToString();
            entityStrings[10] = locationDto.Scale.ToString();
            return entityStrings;
          }
          break;
        case "TenantDTO":
          if (entity is TenantDTO tenantDto)
            return new string[9]
            {
              str1,
              tenantDto.TenantNr.ToString((IFormatProvider) CultureInfo.InvariantCulture),
              tenantDto.Name,
              tenantDto.FloorNr,
              tenantDto.FloorName,
              tenantDto.ApartmentNr,
              tenantDto.Description,
              tenantDto.CustomerTenantNo,
              tenantDto.Entrance
            };
          break;
        case "MinomatSesializableDTO":
          if (entity is MinomatSerializableDTO minomatSerializableDto)
            return new string[25]
            {
              str1,
              minomatSerializableDto.AccessPoint,
              minomatSerializableDto.CelestaId,
              minomatSerializableDto.Challenge,
              minomatSerializableDto.CreatedBy,
              minomatSerializableDto.GsmId,
              minomatSerializableDto.HostAndPort,
              minomatSerializableDto.LastUpdatedBy,
              minomatSerializableDto.RadioId,
              minomatSerializableDto.ProviderName,
              minomatSerializableDto.SessionKey,
              minomatSerializableDto.SimPin,
              minomatSerializableDto.Status,
              minomatSerializableDto.Url,
              minomatSerializableDto.UserId,
              minomatSerializableDto.UserPassword,
              minomatSerializableDto.CreatedOn.ToString(),
              minomatSerializableDto.EndDate.ToString(),
              minomatSerializableDto.IsDeactivated.ToString(),
              minomatSerializableDto.IsInMasterPool.ToString(),
              minomatSerializableDto.IsMaster.ToString(),
              minomatSerializableDto.LastChangedOn.ToString(),
              minomatSerializableDto.Polling.ToString((IFormatProvider) CultureInfo.InvariantCulture),
              minomatSerializableDto.Registered.ToString(),
              minomatSerializableDto.StartDate.ToString()
            };
          break;
      }
      return (string[]) null;
    }

    public string[] GetReadingValueStrings(MeterReadingValue meterReadingValue)
    {
      return new string[7]
      {
        typeof (MeterReadingValue).Name,
        meterReadingValue.MeterSerialNumber,
        meterReadingValue.StorageInterval.ToString(),
        meterReadingValue.Date.ToString("dd.MM.yyyy"),
        meterReadingValue.Value.ToString(),
        string.Empty,
        meterReadingValue.ValueId.ToString((IFormatProvider) CultureInfo.InvariantCulture)
      };
    }

    public static string GetLocalizedCharacterName(char c)
    {
      switch (c)
      {
        case ',':
          return Resources.MSS_Client_Comma;
        case '.':
          return Resources.MSS_Client_Dot;
        case ';':
          return Resources.MSS_Client_Semicolon;
        default:
          return string.Empty;
      }
    }

    public static string GetLocalizedDataToExport(string dataToExportJson)
    {
      DataToExport dataToExport = JsonConvert.DeserializeObject<DataToExport>(dataToExportJson);
      if (dataToExport.Definition == "NotYetExported")
        return Resources.MSS_Client_Reporting_AutomatedJobCreateDialog_NotYetExported;
      return dataToExport.Definition == "FromTheLastXDays" ? Resources.MSS_Client_Reporting_AutomatedJobCreateDialog_FromTheLast + (object) dataToExport.NumberOfDays : string.Empty;
    }

    public List<string[]> GetArchiveListRows<T1>(IEnumerable<T1> archiveList)
    {
      List<string[]> archiveListRows = new List<string[]>();
      foreach (T1 archive in archiveList)
      {
        Type type = archive.GetType();
        DateTime? nullable;
        bool flag;
        if (type == typeof (ArchiveJobLogs) && archive is ArchiveJobLogs archiveJobLogs)
        {
          string[] strArray1 = new string[7];
          strArray1[0] = "\"" + archiveJobLogs.JobEntityNumber + "\"";
          strArray1[1] = archiveJobLogs.CreatedOn.ToShortDateString();
          nullable = archiveJobLogs.StartDate;
          strArray1[2] = nullable.ToString();
          nullable = archiveJobLogs.EndDate;
          strArray1[3] = nullable.ToString();
          flag = archiveJobLogs.Active;
          strArray1[4] = flag.ToString();
          strArray1[5] = archiveJobLogs.Status.ToString();
          strArray1[6] = "\"" + archiveJobLogs.Message + "\"";
          string[] strArray2 = strArray1;
          archiveListRows.Add(strArray2);
        }
        if (type == typeof (ArchiveMssReadingJob) && archive is ArchiveMssReadingJob archiveMssReadingJob)
        {
          string[] strArray3 = new string[21];
          nullable = archiveMssReadingJob.StartDate;
          strArray3[0] = nullable.ToString();
          nullable = archiveMssReadingJob.EndDate;
          strArray3[1] = nullable.ToString();
          flag = archiveMssReadingJob.IsDeactivated;
          strArray3[2] = flag.ToString();
          strArray3[3] = "\"" + archiveMssReadingJob.JobDefinitionName + "\"";
          strArray3[4] = "\"" + archiveMssReadingJob.JobDefinitionEquipmentModel + "\"";
          strArray3[5] = "\"" + archiveMssReadingJob.JobDefinitionServiceJob + "\"";
          strArray3[6] = "\"" + archiveMssReadingJob.JobDefinitionEquipmentParams + "\"";
          strArray3[7] = "\"" + archiveMssReadingJob.JobDefinitionSystem + "\"";
          strArray3[8] = "\"" + archiveMssReadingJob.JobDefinitionProfileType + "\"";
          nullable = archiveMssReadingJob.JobDefinitionStartDate;
          strArray3[9] = nullable.ToString();
          nullable = archiveMssReadingJob.JobDefinitionEndDate;
          strArray3[10] = nullable.ToString();
          flag = archiveMssReadingJob.JobDefinitionIsDeactivated;
          strArray3[11] = flag.ToString();
          strArray3[12] = archiveMssReadingJob.Scenario;
          strArray3[13] = archiveMssReadingJob.Minomat;
          strArray3[14] = archiveMssReadingJob.RootNode;
          flag = archiveMssReadingJob.IsUpdate;
          strArray3[15] = flag.ToString();
          nullable = archiveMssReadingJob.LastExecutionDate;
          strArray3[16] = nullable.ToString();
          strArray3[17] = archiveMssReadingJob.ErrorMessage;
          strArray3[18] = archiveMssReadingJob.Status.ToString();
          strArray3[19] = archiveMssReadingJob.CreatedOn.ToShortDateString();
          nullable = archiveMssReadingJob.LastUpdatedOn;
          strArray3[20] = nullable.ToString();
          string[] strArray4 = strArray3;
          archiveListRows.Add(strArray4);
        }
        if (type == typeof (ArchiveOrder) && archive is ArchiveOrder archiveOrder)
        {
          string[] strArray5 = new string[14];
          strArray5[0] = "\"" + archiveOrder.InstallationNumber + "\"";
          strArray5[1] = "\"" + archiveOrder.Details + "\"";
          flag = archiveOrder.Exported;
          strArray5[2] = flag.ToString();
          strArray5[3] = archiveOrder.Status.ToString();
          strArray5[4] = "\"" + archiveOrder.DeviceNumber + "\"";
          strArray5[5] = archiveOrder.DueDate.ToShortDateString();
          flag = archiveOrder.IsDeactivated;
          strArray5[6] = flag.ToString();
          strArray5[7] = archiveOrder.OrderType.ToString();
          strArray5[8] = archiveOrder.CloseOrderReason.ToString();
          strArray5[9] = archiveOrder.StructureType.ToString();
          strArray5[10] = archiveOrder.CreatedOn.ToShortDateString();
          nullable = archiveOrder.LastUpdatedOn;
          strArray5[11] = nullable.ToString();
          strArray5[12] = "\"" + archiveOrder.OrderRules + "\"";
          strArray5[13] = "\"" + archiveOrder.OrderUsers + "\"";
          string[] strArray6 = strArray5;
          archiveListRows.Add(strArray6);
        }
        if (type == typeof (ArchiveMeterReadingValue) && archive is ArchiveMeterReadingValue meterReadingValue)
        {
          string[] strArray7 = new string[11];
          strArray7[0] = "\"" + meterReadingValue.MeterSerialNumber + "\"";
          strArray7[1] = meterReadingValue.Date.ToShortDateString();
          strArray7[2] = meterReadingValue.Value.ToString();
          strArray7[3] = meterReadingValue.CreatedOn.ToShortDateString();
          nullable = meterReadingValue.ExportedOn;
          strArray7[4] = nullable.ToString();
          strArray7[5] = meterReadingValue.PhysicalQuantity.ToString();
          strArray7[6] = meterReadingValue.MeterType.ToString();
          strArray7[7] = meterReadingValue.CalculationStart.ToString();
          strArray7[8] = meterReadingValue.Creation.ToString();
          strArray7[9] = meterReadingValue.Calculation.ToString();
          strArray7[10] = meterReadingValue.StorageInterval.ToString();
          string[] strArray8 = strArray7;
          archiveListRows.Add(strArray8);
        }
        if (type == typeof (ArchiveStructureNode) && archive is ArchiveStructureNode archiveStructureNode)
        {
          string[] strArray9 = new string[5]
          {
            "\"" + archiveStructureNode.EntityName + "\"",
            "\"" + archiveStructureNode.Name + "\"",
            "\"" + archiveStructureNode.Description + "\"",
            null,
            null
          };
          nullable = archiveStructureNode.StartDate;
          strArray9[3] = nullable.ToString();
          nullable = archiveStructureNode.EndDate;
          strArray9[4] = nullable.ToString();
          string[] strArray10 = strArray9;
          archiveListRows.Add(strArray10);
        }
      }
      return archiveListRows;
    }

    public string WriteArchiveListHeader<T1>()
    {
      string str = string.Empty;
      if (typeof (T1) == typeof (ArchiveJobLogs))
        str = "JobEntityNumber, CreatedOn, StartDate, EndDate, Active, Status, Message";
      if (typeof (T1) == typeof (ArchiveMssReadingJob))
        str = "StartDate, EndDate, IsDeactivated, JobDefinitionName, JobDefinitionEquipmentModel, JobDefinitionServiceJob, JobDefinitionEquipmentParams, JobDefinitionSystem, JobDefinitionProfileType, JobDefinitionStartDate, JobDefinitionEndDate, JobDefinitionIsDeactivated, Scenario, Minomat, RootNode, IsUpdate, LastExecutionDate, ErrorMessage, Status, CreatedOn, LastUpdatedOn";
      if (typeof (T1) == typeof (ArchiveOrder))
        str = "InstallationNumber, Details, Exported, Status, DeviceNumber, DueDate, IsDeactivated, OrderType, CloseOrderReason, StructureType, CreatedOn, LastUpdatedOn, OrderRules, OrderUsers";
      if (typeof (T1) == typeof (ArchiveMeterReadingValue))
        str = "MeterSerialNumber, Date, Value, CreatedOn, ExportedOn, PhysicalQuantity., MeterType, CalculationStart, Creation, Calculation, StorageInterval";
      if (typeof (T1) == typeof (ArchiveStructureNode))
        str = "EntityName, Name, Description, StartDate, EndDate";
      return str;
    }

    public static ObservableCollection<MeterReadingValueDTO> FilteredReadingValuesCollection(
      ObservableCollection<MeterReadingValueDTO> meterReadingValueDtos)
    {
      return MSSHelper.ListToObsCollection<MeterReadingValueDTO>(MSSHelper.DistinctBy(meterReadingValueDtos.Where<MeterReadingValueDTO>((Func<MeterReadingValueDTO, bool>) (rv => rv.PhysicalQuantity != ValueIdent.ValueIdPart_PhysicalQuantity.SignalStrength)), p => new
      {
        MeterSerialNumber = p.MeterSerialNumber,
        Date = p.Date,
        StorageInterval = p.StorageInterval
      }));
    }
  }
}
