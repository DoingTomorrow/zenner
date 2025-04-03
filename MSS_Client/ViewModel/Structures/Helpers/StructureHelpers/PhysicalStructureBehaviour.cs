// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Structures.Helpers.StructureHelpers.PhysicalStructureBehaviour
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Core.Model.Structures;
using MSS_Client.ViewModel.Structures.DeviceViewModels;
using MSS_Client.ViewModel.Structures.Helpers.DeviceHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace MSS_Client.ViewModel.Structures.Helpers.StructureHelpers
{
  public class PhysicalStructureBehaviour : StructureBehaviour
  {
    public static PhysicalStructureBehaviour GetPhysicalStructureBehaviour(bool isEdit)
    {
      return !isEdit ? (PhysicalStructureBehaviour) new CreatePhysicalStructureBehavior() : (PhysicalStructureBehaviour) new EditPhysicalStructureBehavior();
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
          deviceVM.IsSaveAndCreateNewVisible = true;
          break;
        case DeviceStateEnum.Edit:
          deviceVM.IsDeviceGroupEnabled = false;
          deviceVM.IsDeviceModelEnabled = false;
          deviceVM.IsMeterGeneralTabVisible = deviceVM.SelectedNodeType == StructureNodeTypeEnum.Meter;
          deviceVM.IsMeterGeneralTabSelected = deviceVM.IsMeterGeneralTabVisible;
          deviceVM.IsRadioMeterGeneralTabVisible = deviceVM.SelectedNodeType == StructureNodeTypeEnum.RadioMeter;
          deviceVM.IsRadioMeterGeneralTabSelected = deviceVM.IsRadioMeterGeneralTabVisible;
          deviceVM.IsSaveAndCreateNewVisible = true;
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
      deviceVM.Name = this.UpdateDeviceNameForLogicalAndPhysicalStr(deviceVM);
    }

    private string UpdateDeviceNameForLogicalAndPhysicalStr(DeviceViewModel deviceVM)
    {
      string str1 = string.Empty;
      if (deviceVM.Name != null)
      {
        string[] source = deviceVM.Name.Split(new string[1]
        {
          " - "
        }, 2, StringSplitOptions.None);
        string str2 = source[source.Length - 1];
        if (str2 == deviceVM.AnteriorSerialNumber)
        {
          if (deviceVM.SerialNo == string.Empty || str2 != deviceVM.SerialNo)
          {
            source[source.Length - 1] = deviceVM.SerialNo;
            str1 = ((IEnumerable<string>) source).Aggregate<string, string>(string.Empty, (Func<string, string, string>) ((current, s) => !(current == string.Empty) ? current + " - " + s : s));
            if (deviceVM.SerialNo == string.Empty)
              str1 = str1.Remove(str1.Length - 2, 2);
          }
        }
        else
          str1 = deviceVM.SerialNo != string.Empty ? string.Format("{0} - {1}", (object) deviceVM.Name, (object) deviceVM.SerialNo) : deviceVM.Name;
      }
      else if (deviceVM.SerialNo != string.Empty)
        str1 = string.Format("{0} - {1}", (object) string.Empty, (object) deviceVM.SerialNo);
      return str1;
    }
  }
}
