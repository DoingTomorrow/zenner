// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.ReadingValues.ReadingValuesInitializer
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.DTO;
using MSS.Business.Utils;
using MSS.DTO.Meters;
using MSS.DTO.Orders;
using MSS.Interfaces;
using MSS.Localisation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#nullable disable
namespace MSS.Business.Modules.ReadingValues
{
  public abstract class ReadingValuesInitializer
  {
    protected readonly IRepositoryFactory _repositoryFactory;
    private ObservableCollection<MeterReadingValueDTO> meterReadingValuesDto;

    protected ReadingValuesInitializer(
      StructureNodeDTO structureNode,
      IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this.StructureNode = structureNode;
      MappingsManager.MeterReadingValue_to_MeterReadingValueDTO();
    }

    public StructureNodeDTO StructureNode { get; set; }

    public OrderDTO SelectedOrder { get; set; }

    public string SetTitle(string suffix)
    {
      return Resources.MSS_Client_Structures_ReadingValues + " - " + suffix;
    }

    public abstract List<Guid> GetReplacedMeters();

    public abstract List<MeterReadingValueDTO> GetReadingValues();

    public ObservableCollection<MeterReadingValueDTO> GetReadingValuesDTO()
    {
      List<Guid> replacedMeterIds = this.GetReplacedMeters();
      List<MeterReadingValueDTO> readingValues = this.GetReadingValues();
      foreach (MeterReadingValueDTO meterReadingValueDto in readingValues.Where<MeterReadingValueDTO>((Func<MeterReadingValueDTO, bool>) (meterReadingValueDto => replacedMeterIds.Contains(meterReadingValueDto.MeterId))))
        meterReadingValueDto.IsReplacedMeter = true;
      ObservableCollection<MeterReadingValueDTO> readingValuesDto = new ObservableCollection<MeterReadingValueDTO>();
      foreach (MeterReadingValueDTO meterReadingValueDto in readingValues)
        readingValuesDto.Add(meterReadingValueDto);
      return readingValuesDto;
    }
  }
}
