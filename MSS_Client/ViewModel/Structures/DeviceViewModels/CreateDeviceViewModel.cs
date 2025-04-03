// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Structures.DeviceViewModels.CreateDeviceViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.DTO;
using MSS.Business.Modules.Configuration;
using MSS.Core.Model.Structures;
using MSS.Interfaces;
using MSS.Localisation;
using MSS_Client.ViewModel.Structures.Helpers.DeviceHelpers;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace MSS_Client.ViewModel.Structures.DeviceViewModels
{
  public class CreateDeviceViewModel : DeviceViewModel
  {
    public CreateDeviceViewModel(
      DeviceStateEnum deviceState,
      StructureNodeDTO node,
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory,
      List<string> serialNumberList)
      : base(deviceState, node, repositoryFactory, windowFactory, serialNumberList)
    {
      this.MeterDialogTitle = node.NodeType.Name == "Meter" ? CultureResources.GetValue("MSS_Client_Structures_CreateDevice_Title") : CultureResources.GetValue("MSS_Client_Structures_CreateRadioDevice_Title");
      if (node.StructureType.HasValue && node.StructureType.Value == StructureTypeEnum.Fixed)
        this.MeterDialogTitle += this.GetTenantTitleString();
      this.IsAddMeterButtonVisible = true;
      this.IsEditMeterButtonVisible = false;
      this.IsReplaceMeterButtonVisible = false;
      this.GetPreviousDeviceGroupAndModel();
      this.Name = node.Name;
      this.Description = node.Description;
      this.IsConfigValuesVisible = this.ConfigValuesCollection != null && this.ConfigValuesCollection.Any<ConfigurationPerChannel>();
      this.isSaveAndCreateNewFirstTimePressed = true;
      this.structureBehaviour.InitializeDeviceViewModel((DeviceViewModel) this, deviceState);
      this.IsMeterReplacementVisible = false;
      StructureTypeEnum? structureType = this._structureType;
      StructureTypeEnum structureTypeEnum = StructureTypeEnum.Fixed;
      this.IsReadingEnabled = structureType.GetValueOrDefault() == structureTypeEnum && structureType.HasValue;
    }
  }
}
