// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Structures.DeviceViewModels.ReplaceDeviceViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.DTO;
using MSS.Business.Modules.GMM;
using MSS.DTO.Meters;
using MSS.Interfaces;
using MSS.Localisation;
using MSS_Client.ViewModel.Structures.Helpers.DeviceHelpers;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS_Client.ViewModel.Structures.DeviceViewModels
{
  public class ReplaceDeviceViewModel : DeviceViewModel
  {
    [Inject]
    public ReplaceDeviceViewModel(
      DeviceStateEnum deviceState,
      StructureNodeDTO node,
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory,
      List<string> serialNumberList)
      : base(deviceState, node, repositoryFactory, windowFactory, serialNumberList)
    {
      this.MeterDialogTitle = CultureResources.GetValue("MSS_Client_Structures_ReplaceDevice_Title");
      DeviceGroup deviceGroup = new DeviceGroup();
      DeviceModel deviceModel = new DeviceModel();
      if (node.Entity == null)
        return;
      MeterDTO entity = node.Entity as MeterDTO;
      GMMHelper.GetDeviceGroupAndModelBasedOnDeviceType(entity.DeviceType, ref deviceGroup, ref deviceModel);
      this.SelectedDeviceGroup = deviceGroup;
      this.SelectedDeviceType = this.DeviceTypeCollection.FirstOrDefault<DeviceModel>((Func<DeviceModel, bool>) (item => item.Name == deviceModel.Name));
      this.SelectedRoomTypeId = new Guid?(entity.Room != null ? entity.Room.Id : Guid.Empty);
      this.Name = "Meter";
      this.IsReplaceMeterButtonVisible = true;
      this.structureBehaviour.InitializeDeviceViewModel((DeviceViewModel) this, deviceState);
    }
  }
}
