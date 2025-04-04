// Decompiled with JetBrains decompiler
// Type: S4_Handler.S4_CommunicationManager
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using S4_Handler.Functions;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace S4_Handler
{
  internal class S4_CommunicationManager
  {
    internal S4_DeviceMemory DeviceMemory;
    internal _UNIT_SCALE_ DefaultUnitScale = _UNIT_SCALE_.NotSet;
    internal _MBUS_INFO_TYPE_ DefaultVolumeScale;
    internal _MBUS_INFO_TYPE_ DefaultFlowScale;
    private uint MBusInfoLength;
    private uint ManagedParameterLength = 20;
    private List<_AVAILABLE_PARAMETER_TYPE_> ManagedParameters;
    private List<_SELECT_PARAMETER_TYPE_> SelectedParameters;

    internal S4_CommunicationManager(S4_DeviceMemory deviceMemory)
    {
      this.DeviceMemory = deviceMemory;
      try
      {
        this.DefaultUnitScale = (_UNIT_SCALE_) this.DeviceMemory.GetParameterValue<ushort>(S4_Params.DeviceDefaultUnitScale);
        this.MBusInfoLength = this.DeviceMemory.GetParameterAddressRange(S4_Params.DeviceDefaultMBusInfo).ByteSize / 2U;
        uint parameterAddress1 = deviceMemory.GetParameterAddress(S4_Params.DeviceDefaultMBusInfo);
        byte[] data1 = deviceMemory.GetData(parameterAddress1, 2U * this.MBusInfoLength);
        int offset = 0;
        this.DefaultVolumeScale = new _MBUS_INFO_TYPE_();
        this.DefaultVolumeScale.LoadFromBytes(data1, this.MBusInfoLength, ref offset);
        this.DefaultFlowScale = new _MBUS_INFO_TYPE_();
        this.DefaultFlowScale.LoadFromBytes(data1, this.MBusInfoLength, ref offset);
        uint parameterValue1 = (uint) deviceMemory.GetParameterValue<ushort>(S4_Params.manParamCount);
        uint parameterAddress2 = deviceMemory.GetParameterAddress(S4_Params.managedParam);
        this.ManagedParameterLength = this.DeviceMemory.GetParameterAddressRange(S4_Params.managedParam).ByteSize / parameterValue1;
        byte[] data2 = deviceMemory.GetData(parameterAddress2, parameterValue1 * this.ManagedParameterLength);
        this.ManagedParameters = this.GetManagedParameters(data2);
        byte[] managedParameterBytes = this.GetManagedParameterBytes(this.ManagedParameters);
        for (int index = 0; index < managedParameterBytes.Length; ++index)
        {
          if ((int) managedParameterBytes[index] != (int) data2[index])
            throw new Exception("ManagedParameters conversion error");
        }
        uint parameterValue2 = (uint) deviceMemory.GetParameterValue<ushort>(S4_Params.selParamCount);
        byte[] data3 = deviceMemory.GetData(deviceMemory.SelectedReadoutParametersRange.StartAddress, parameterValue2 * 20U);
        if (data3 == null)
          return;
        this.SelectedParameters = this.GetSelectedParameters(data3, parameterValue2);
        foreach (_SELECT_PARAMETER_TYPE_ selectedParameter in this.SelectedParameters)
        {
          _SELECT_PARAMETER_TYPE_ param = selectedParameter;
          List<_AVAILABLE_PARAMETER_TYPE_> all = this.ManagedParameters.FindAll((Predicate<_AVAILABLE_PARAMETER_TYPE_>) (x => (int) x.mbusInfo.storeNum == (int) param.mbusInfo.storeNum && x.mbusInfo.destType == param.mbusInfo.destType && (int) x.mbusInfo.tarif == (int) param.mbusInfo.tarif && x.mbusInfo.difFunction == param.mbusInfo.difFunction && S4_DifVif_Parameter.GetPhysicalBase(x.mbusInfo) == S4_DifVif_Parameter.GetPhysicalBase(param.mbusInfo)));
          if (all.Count < 1)
          {
            all = this.ManagedParameters.FindAll((Predicate<_AVAILABLE_PARAMETER_TYPE_>) (x => (int) x.mbusInfo.storeNum == (int) param.mbusInfo.storeNum && (int) x.mbusInfo.tarif == (int) param.mbusInfo.tarif && x.mbusInfo.difFunction == param.mbusInfo.difFunction && S4_DifVif_Parameter.GetPhysicalBase(x.mbusInfo) == S4_DifVif_Parameter.GetPhysicalBase(param.mbusInfo)));
            if (all.Count < 1)
              throw new Exception("Managed parameter not found for selected parameter: " + param.ToString());
          }
          foreach (_AVAILABLE_PARAMETER_TYPE_ availableParameterType in all)
          {
            if (availableParameterType != this.ManagedParameters[(int) param.paramIndex])
              throw new Exception("Illegal managed parameter index in selected parameter: " + param.ToString());
          }
        }
        byte[] selectedParameterBytes = this.GetSelectedParameterBytes(this.SelectedParameters);
        for (int index = 0; index < selectedParameterBytes.Length; ++index)
        {
          if ((int) selectedParameterBytes[index] != (int) data3[index])
            throw new Exception("SelectedParameters conversion error");
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Cannot create the communication manager", ex);
      }
    }

    private List<_AVAILABLE_PARAMETER_TYPE_> GetManagedParameters(byte[] managedParamBytes)
    {
      List<_AVAILABLE_PARAMETER_TYPE_> managedParameters = new List<_AVAILABLE_PARAMETER_TYPE_>();
      byte num = 0;
      int offset = 0;
      while (offset < managedParamBytes.Length)
      {
        _AVAILABLE_PARAMETER_TYPE_ availableParameterType = new _AVAILABLE_PARAMETER_TYPE_();
        availableParameterType.index = num++;
        availableParameterType.LoadFromBytes(managedParamBytes, this.MBusInfoLength, ref offset);
        managedParameters.Add(availableParameterType);
      }
      return managedParameters;
    }

    private List<_SELECT_PARAMETER_TYPE_> GetSelectedParameters(
      byte[] selectedParamBytes,
      uint selParamCount)
    {
      List<_SELECT_PARAMETER_TYPE_> selectedParameters = new List<_SELECT_PARAMETER_TYPE_>();
      int offset = 0;
      for (int index = 0; (long) index < (long) selParamCount; ++index)
      {
        _SELECT_PARAMETER_TYPE_ selectParameterType = new _SELECT_PARAMETER_TYPE_();
        selectParameterType.LoadFromBytes(selectedParamBytes, this.MBusInfoLength, ref offset);
        selectedParameters.Add(selectParameterType);
      }
      return selectedParameters;
    }

    private byte[] GetManagedParameterBytes(List<_AVAILABLE_PARAMETER_TYPE_> managedParameters)
    {
      List<byte> byteDestination = new List<byte>();
      foreach (_AVAILABLE_PARAMETER_TYPE_ managedParameter in managedParameters)
        managedParameter.GetBytes(byteDestination);
      return byteDestination.ToArray();
    }

    private byte[] GetSelectedParameterBytes(List<_SELECT_PARAMETER_TYPE_> selectedParameters)
    {
      List<byte> byteDestination = new List<byte>();
      foreach (_SELECT_PARAMETER_TYPE_ selectedParameter in selectedParameters)
        selectedParameter.GetBytes(byteDestination);
      return byteDestination.ToArray();
    }

    public string ToString(string leftSpaces)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(leftSpaces + "Default scales");
      _MBUS_INFO_TYPE_ completeParameter1 = (_MBUS_INFO_TYPE_) null;
      if (!this.DefaultVolumeScale.GetCompletedParameter(ref completeParameter1))
        S4_DifVif_Parameter.AllVolumeUnitBaseDefines[this.DefaultUnitScale].volumeInfo.GetCompletedParameter(ref completeParameter1);
      stringBuilder.AppendLine(leftSpaces + "   Volume scale: " + S4_DifVif_Parameter.BaseUnitScale[completeParameter1.scaleUnit].DisplayString + "; type: " + completeParameter1.destType.ToString());
      _MBUS_INFO_TYPE_ completeParameter2 = (_MBUS_INFO_TYPE_) null;
      if (!this.DefaultFlowScale.GetCompletedParameter(ref completeParameter2))
        S4_DifVif_Parameter.AllVolumeUnitBaseDefines[this.DefaultUnitScale].flowInfo.GetCompletedParameter(ref completeParameter2);
      stringBuilder.AppendLine(leftSpaces + "   Flow scale: " + S4_DifVif_Parameter.BaseUnitScale[completeParameter2.scaleUnit].DisplayString + "; type: " + completeParameter2.destType.ToString());
      stringBuilder.AppendLine();
      stringBuilder.AppendLine(leftSpaces + "Managed parameters");
      foreach (_AVAILABLE_PARAMETER_TYPE_ managedParameter in this.ManagedParameters)
      {
        _MBUS_INFO_TYPE_ completeParameter3 = (_MBUS_INFO_TYPE_) null;
        switch (S4_DifVif_Parameter.GetPhysicalBase(managedParameter.mbusInfo))
        {
          case VIF_PhyicalBase.Volume:
            if (!managedParameter.mbusInfo.GetCompletedParameter(ref completeParameter3) && !this.DefaultVolumeScale.GetCompletedParameter(ref completeParameter3))
            {
              S4_DifVif_Parameter.AllVolumeUnitBaseDefines[this.DefaultUnitScale].volumeInfo.GetCompletedParameter(ref completeParameter3);
              break;
            }
            break;
          case VIF_PhyicalBase.Flow:
            if (!managedParameter.mbusInfo.GetCompletedParameter(ref completeParameter3) && !this.DefaultFlowScale.GetCompletedParameter(ref completeParameter3))
            {
              S4_DifVif_Parameter.AllVolumeUnitBaseDefines[this.DefaultUnitScale].flowInfo.GetCompletedParameter(ref completeParameter3);
              break;
            }
            break;
          default:
            completeParameter3 = managedParameter.mbusInfo;
            break;
        }
        stringBuilder.AppendLine(leftSpaces + "   " + completeParameter3.GetParameterName() + "; Scale: " + S4_DifVif_Parameter.BaseUnitScale[completeParameter3.scaleUnit].DisplayString + "; Type: " + completeParameter3.destType.ToString());
      }
      stringBuilder.AppendLine();
      stringBuilder.AppendLine(leftSpaces + "Selected parameters");
      if (this.SelectedParameters != null)
      {
        int num = -1;
        foreach (_SELECT_PARAMETER_TYPE_ selectedParameter in this.SelectedParameters)
        {
          if ((int) selectedParameter.group != num)
          {
            if (num != -1)
              stringBuilder.AppendLine();
            stringBuilder.AppendLine(leftSpaces + "   Group: " + selectedParameter.group.ToString());
            num = (int) selectedParameter.group;
          }
          _MBUS_INFO_TYPE_ completeParameter4 = (_MBUS_INFO_TYPE_) null;
          switch (S4_DifVif_Parameter.GetPhysicalBase(selectedParameter.mbusInfo))
          {
            case VIF_PhyicalBase.Volume:
              if (!selectedParameter.mbusInfo.GetCompletedParameter(ref completeParameter4) && !this.ManagedParameters[(int) selectedParameter.paramIndex].mbusInfo.GetCompletedParameter(ref completeParameter4) && !this.DefaultVolumeScale.GetCompletedParameter(ref completeParameter4))
              {
                S4_DifVif_Parameter.AllVolumeUnitBaseDefines[this.DefaultUnitScale].volumeInfo.GetCompletedParameter(ref completeParameter4);
                break;
              }
              break;
            case VIF_PhyicalBase.Flow:
              if (!selectedParameter.mbusInfo.GetCompletedParameter(ref completeParameter4) && !this.ManagedParameters[(int) selectedParameter.paramIndex].mbusInfo.GetCompletedParameter(ref completeParameter4) && !this.DefaultFlowScale.GetCompletedParameter(ref completeParameter4))
              {
                S4_DifVif_Parameter.AllVolumeUnitBaseDefines[this.DefaultUnitScale].flowInfo.GetCompletedParameter(ref completeParameter4);
                break;
              }
              break;
            default:
              completeParameter4 = selectedParameter.mbusInfo;
              break;
          }
          stringBuilder.AppendLine(leftSpaces + "      " + completeParameter4.GetParameterName() + "; Scale: " + S4_DifVif_Parameter.BaseUnitScale[completeParameter4.scaleUnit].DisplayString + "; Type: " + completeParameter4.destType.ToString());
        }
      }
      return stringBuilder.ToString();
    }

    public override string ToString() => this.ToString("");

    internal enum _PARAM_INDEX_
    {
      SYSDATE,
      SYSTIME,
      VOLUME,
      FLOW_0,
      FLOW_1,
    }

    internal enum ParamPermission
    {
      ReadOnly = 0,
      ReadWrite = 128, // 0x00000080
    }
  }
}
