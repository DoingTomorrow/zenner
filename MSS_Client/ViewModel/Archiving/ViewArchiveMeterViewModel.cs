// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Archiving.ViewArchiveMeterViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Core.Model.Meters;
using MSS.DTO.Archive;
using MSS.Localisation;
using Ninject;
using System;
using System.Collections.Generic;

#nullable disable
namespace MSS_Client.ViewModel.Archiving
{
  internal class ViewArchiveMeterViewModel : ViewArchiveEntityViewModel
  {
    private ArchiveMeterDTO _Entity { get; }

    [Inject]
    public ViewArchiveMeterViewModel(ArchiveMeterDTO entity)
    {
      this._Entity = entity;
      this.ArchiveViewEntityDialogTitle = CultureResources.GetValue("MSS_Client_Archiving_ViewMeter");
      this.InitializeData(this._Entity);
    }

    private void InitializeData(ArchiveMeterDTO entity)
    {
      this.SerialNo = entity.SerialNumber;
      this.ShortDeviceNo = entity.ShortDeviceNo;
      this.CompleteDevice = entity.CompletDevice;
      this.DecimalPlaces = entity.DecimalPlaces;
      this.DeviceType = entity.DeviceType;
      this.ChannelCode = entity.ChannelCode;
      this.ConnectedDeviceTypeCode = entity.ConnectedDeviceTypeCode;
      this.ReadingUnitCode = entity.ReadingUnitCode;
      this.RoomTypeCode = entity.RoomTypeCode;
      this.StartValue = entity.StartValue;
    }

    public string SerialNo { get; set; }

    public string AnteriorSerialNumber { get; set; }

    public List<string> SerialNumberList { get; set; }

    public string ShortDeviceNo { get; set; }

    public string CompleteDevice { get; set; }

    public DeviceTypeEnum DeviceType { get; set; }

    public string RoomTypeCode { get; set; }

    public string ReadingUnitCode { get; set; }

    public double? StartValue { get; set; }

    public double? DecimalPlaces { get; set; }

    public Guid SelectedImpulsUnitId { get; set; }

    public string ChannelCode { get; set; }

    public string ConnectedDeviceTypeCode { get; set; }
  }
}
