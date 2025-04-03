// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Reporting.AutomatedReportingManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using AutoMapper;
using MSS.Business.Utils;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Reporting;
using MSS.Core.Model.UsersManagement;
using MSS.DTO.Meters;
using MSS.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

#nullable disable
namespace MSS.Business.Modules.Reporting
{
  public class AutomatedReportingManager
  {
    private readonly IRepositoryFactory _repositoryFactory;

    public AutomatedReportingManager(IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
    }

    public void Export(
      AutomatedExportJob automatedExportJob,
      string fileName,
      AutomatedReportingAdditionalInfo automatedReportingAdditionalInfo)
    {
      DataToExport dataToExport = JsonConvert.DeserializeObject<DataToExport>(automatedExportJob.DataToExport);
      bool sasExportType = automatedExportJob.ExportFor == "SAS";
      bool dataExportLastDaysChoice = dataToExport.Definition == "FromTheLastXDays";
      bool csvFileType = automatedExportJob.ExportedFileType == "CSV";
      bool commaDecimalSeparator = automatedExportJob.DecimalSeparator.ToString() == ",";
      bool semicolonValueSeparator = automatedExportJob.ValueSeparator.ToString() == ";";
      new ReportingManager(this._repositoryFactory).ExportAllReadingValues(fileName, dataExportLastDaysChoice, dataToExport.NumberOfDays, sasExportType, csvFileType, false, false, commaDecimalSeparator, semicolonValueSeparator, (BackgroundWorker) null, (DoWorkEventArgs) null, automatedExportJob.JobCountries.Select<AutomatedExportJobCountry, Country>((Func<AutomatedExportJobCountry, Country>) (j => j.Country)).Distinct<Country>().ToList<Country>());
    }

    private void ExportForGMM_CSV(
      AutomatedExportJob automatedExportJob,
      string fileName,
      AutomatedReportingAdditionalInfo automatedReportingAdditionalInfo)
    {
      List<string[]> nodeList = new List<string[]>()
      {
        new string[12]
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
        }
      };
      MappingsManager.MeterReadingValue_to_MeterReadingValueDTO();
      foreach (MeterReadingValueDTO meterReadingValueDto in Mapper.Map<IEnumerable<MeterReadingValue>, IEnumerable<MeterReadingValueDTO>>(automatedReportingAdditionalInfo.MeterReadingValues))
      {
        MeterReadingValueDTO readingValue = meterReadingValueDto;
        Meter meter = automatedReportingAdditionalInfo.Meters.FirstOrDefault<Meter>((Func<Meter, bool>) (m => m.Id == readingValue.MeterId));
        string[] strArray = new string[12]
        {
          readingValue.Date.ToString("dd.MM.yyyy"),
          readingValue.MeterSerialNumber,
          readingValue.Name,
          meter == null ? string.Empty : meter.DeviceType.ToString(),
          readingValue.Value.ToString((IFormatProvider) CultureInfo.InvariantCulture),
          meter == null || meter.ReadingUnit == null ? string.Empty : meter.ReadingUnit.Code,
          readingValue.PhysicalQuantity.ToString(),
          readingValue.Calculation.ToString(),
          readingValue.StorageInterval.ToString(),
          readingValue.CalculationStart.ToString(),
          readingValue.Creation.ToString(),
          readingValue.Index.ToString()
        };
        nodeList.Add(strArray);
      }
      new CSVManager().WriteToFileCultureDependent(fileName, nodeList, automatedExportJob.ValueSeparator.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }
  }
}
