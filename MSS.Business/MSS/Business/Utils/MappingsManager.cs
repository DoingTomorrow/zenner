// Decompiled with JetBrains decompiler
// Type: MSS.Business.Utils.MappingsManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using AutoMapper;
using MSS.Business.Modules.Reporting;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Reporting;
using MSS.DTO.Meters;
using MSS.DTO.Reporting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ZR_ClassLibrary;

#nullable disable
namespace MSS.Business.Utils
{
  public static class MappingsManager
  {
    public static void AutomatedExportJob_to_AutomatedExportJobDTO()
    {
      Mapper.CreateMap<AutomatedExportJob, AutomatedExportJobDTO>().ForMember((Expression<Func<AutomatedExportJobDTO, object>>) (m => (object) m.DataToExport), (Action<IMemberConfigurationExpression<AutomatedExportJob>>) (action => action.ResolveUsing((Func<AutomatedExportJob, object>) (j => (object) JsonConvert.DeserializeObject<DataToExport>(j.DataToExport))))).ForMember((Expression<Func<AutomatedExportJobDTO, object>>) (m => m.DecimalSeparator), (Action<IMemberConfigurationExpression<AutomatedExportJob>>) (action => action.ResolveUsing((Func<AutomatedExportJob, object>) (j => (object) string.Format("{0} (" + ReportingHelper.GetLocalizedCharacterName(j.DecimalSeparator) + ")", (object) j.DecimalSeparator))))).ForMember((Expression<Func<AutomatedExportJobDTO, object>>) (m => m.ValueSeparator), (Action<IMemberConfigurationExpression<AutomatedExportJob>>) (action => action.ResolveUsing((Func<AutomatedExportJob, object>) (j => (object) string.Format("{0} (" + ReportingHelper.GetLocalizedCharacterName(j.ValueSeparator) + ")", (object) j.ValueSeparator))))).ForMember((Expression<Func<AutomatedExportJobDTO, object>>) (m => m.ExportedDataFormatted), (Action<IMemberConfigurationExpression<AutomatedExportJob>>) (action => action.ResolveUsing((Func<AutomatedExportJob, object>) (j => (object) ReportingHelper.GetLocalizedDataToExport(j.DataToExport))))).ForMember((Expression<Func<AutomatedExportJobDTO, object>>) (m => m.ExportPath), (Action<IMemberConfigurationExpression<AutomatedExportJob>>) (action => action.MapFrom<string>((Expression<Func<AutomatedExportJob, string>>) (j => j.Path))));
    }

    public static void MeterReadingValue_to_MeterReadingValueDTO()
    {
      Mapper.CreateMap<MeterReadingValue, MeterReadingValueDTO>().ForMember((Expression<Func<MeterReadingValueDTO, object>>) (m => (object) m.Index), (Action<IMemberConfigurationExpression<MeterReadingValue>>) (action => action.ResolveUsing((Func<MeterReadingValue, object>) (j => (object) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Index>(j.ValueId)))));
    }

    public static void SetIsReplacedForMeterReadingValueDTO(List<Guid> replacedMeterIds)
    {
      Mapper.CreateMap<MeterReadingValue, MeterReadingValueDTO>().ForMember((Expression<Func<MeterReadingValueDTO, object>>) (m => (object) m.IsReplacedMeter), (Action<IMemberConfigurationExpression<MeterReadingValue>>) (action => action.ResolveUsing((Func<MeterReadingValue, object>) (j => (object) replacedMeterIds.Contains(j.MeterId)))));
    }
  }
}
