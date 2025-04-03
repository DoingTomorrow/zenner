// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Structures.Helpers.StructureHelpers.FixedStructureBehavior
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS_Client.ViewModel.Structures.DeviceViewModels;
using MSS_Client.ViewModel.Structures.Helpers.DeviceHelpers;

#nullable disable
namespace MSS_Client.ViewModel.Structures.Helpers.StructureHelpers
{
  public class FixedStructureBehavior : StructureBehaviour
  {
    public static FixedStructureBehavior GetFixedStructureBehaviour(bool isEdit)
    {
      return !isEdit ? (FixedStructureBehavior) new CreateFixedStructureBehavior() : (FixedStructureBehavior) new EditFixedStructureBehavior();
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
          deviceVM.IsSpecificTabSelected = true;
          deviceVM.IsSaveAndCreateNewVisible = true;
          break;
        case DeviceStateEnum.Edit:
          deviceVM.IsSerialNumberEnabled = deviceVM.SerialNo == string.Empty;
          deviceVM.IsDeviceGroupEnabled = true;
          deviceVM.IsDeviceModelEnabled = true;
          deviceVM.IsSpecificTabSelected = true;
          deviceVM.IsSaveAndCreateNewVisible = true;
          break;
        case DeviceStateEnum.Replace:
          deviceVM.IsSerialNumberEnabled = true;
          deviceVM.IsDeviceGroupEnabled = true;
          deviceVM.IsDeviceModelEnabled = true;
          deviceVM.IsSpecificTabSelected = true;
          deviceVM.IsSaveAndCreateNewVisible = false;
          break;
      }
    }

    public override void UpdateName(DeviceViewModel deviceVM) => deviceVM.Name = deviceVM.SerialNo;
  }
}
