// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Structures.Helpers.StructureHelpers.StructureBehaviour
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS_Client.ViewModel.Structures.DeviceViewModels;
using MSS_Client.ViewModel.Structures.Helpers.DeviceHelpers;

#nullable disable
namespace MSS_Client.ViewModel.Structures.Helpers.StructureHelpers
{
  public abstract class StructureBehaviour
  {
    public abstract void InitializeDeviceViewModel(
      DeviceViewModel deviceVM,
      DeviceStateEnum deviceState);

    public abstract void UpdateName(DeviceViewModel deviceVM);
  }
}
