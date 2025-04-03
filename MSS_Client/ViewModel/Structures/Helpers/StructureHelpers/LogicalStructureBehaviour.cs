// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Structures.Helpers.StructureHelpers.LogicalStructureBehaviour
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Core.Model.Structures;
using MSS_Client.ViewModel.Structures.DeviceViewModels;
using MSS_Client.ViewModel.Structures.Helpers.DeviceHelpers;

#nullable disable
namespace MSS_Client.ViewModel.Structures.Helpers.StructureHelpers
{
  public class LogicalStructureBehaviour : StructureBehaviour
  {
    public static LogicalStructureBehaviour GetLogicalStructureBehaviour(bool isEdit)
    {
      return !isEdit ? (LogicalStructureBehaviour) new CreateLogicalStructureBehavior() : (LogicalStructureBehaviour) new EditLogicalStructureBehavior();
    }

    public override void InitializeDeviceViewModel(
      DeviceViewModel deviceVM,
      DeviceStateEnum deviceState)
    {
      switch (deviceState)
      {
        case DeviceStateEnum.Create:
          deviceVM.IsSerialNumberEnabled = true;
          deviceVM.IsDeviceGroupEnabled = true;
          deviceVM.IsDeviceModelEnabled = true;
          deviceVM.IsMeterGeneralTabVisible = deviceVM.SelectedNodeType == StructureNodeTypeEnum.Meter;
          deviceVM.IsRadioMeterGeneralTabVisible = deviceVM.SelectedNodeType == StructureNodeTypeEnum.RadioMeter;
          deviceVM.IsSpecificTabSelected = true;
          deviceVM.IsSaveAndCreateNewVisible = false;
          break;
        case DeviceStateEnum.Edit:
          deviceVM.IsSerialNumberEnabled = deviceVM.SerialNo == string.Empty;
          deviceVM.IsDeviceGroupEnabled = false;
          deviceVM.IsDeviceModelEnabled = false;
          deviceVM.IsMeterGeneralTabVisible = deviceVM.SelectedNodeType == StructureNodeTypeEnum.Meter;
          deviceVM.IsMeterGeneralTabSelected = deviceVM.IsMeterGeneralTabVisible;
          deviceVM.IsRadioMeterGeneralTabVisible = deviceVM.SelectedNodeType == StructureNodeTypeEnum.RadioMeter;
          deviceVM.IsRadioMeterGeneralTabSelected = deviceVM.IsRadioMeterGeneralTabVisible;
          deviceVM.IsSaveAndCreateNewVisible = false;
          break;
        case DeviceStateEnum.Replace:
          deviceVM.IsSerialNumberEnabled = true;
          deviceVM.IsDeviceGroupEnabled = true;
          deviceVM.IsDeviceModelEnabled = true;
          deviceVM.IsMeterGeneralTabVisible = deviceVM.SelectedNodeType == StructureNodeTypeEnum.Meter;
          deviceVM.IsRadioMeterGeneralTabVisible = deviceVM.SelectedNodeType == StructureNodeTypeEnum.RadioMeter;
          deviceVM.IsSpecificTabSelected = true;
          deviceVM.IsSaveAndCreateNewVisible = false;
          break;
      }
    }

    public override void UpdateName(DeviceViewModel deviceVM)
    {
    }
  }
}
