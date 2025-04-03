// Decompiled with JetBrains decompiler
// Type: S3_Handler.WR4_VMCP_Diagnostic
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System;
using System.Collections.Generic;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class WR4_VMCP_Diagnostic
  {
    private S3_HandlerFunctions myFunctions;
    private string[] VMCP_DiagnosticParameters = new string[7]
    {
      S3_ParameterNames.VolReceiveErrorCounter.ToString(),
      S3_ParameterNames.VolReceiveWaitQuietCounter.ToString(),
      S3_ParameterNames.VolInputState.ToString(),
      S3_ParameterNames.VMCP_CycleCalibrationPossible.ToString(),
      S3_ParameterNames.RequestedEnergyCycleTime.ToString(),
      S3_ParameterNames.VolumeMeterIdentification.ToString(),
      S3_ParameterNames.VolumeInputValueBefore.ToString()
    };

    internal WR4_VMCP_Diagnostic(S3_HandlerFunctions myFunctions) => this.myFunctions = myFunctions;

    internal VMCP_Diagnostic ReadVMCP_Diagnostic()
    {
      S3_Meter connectedMeter = this.myFunctions.MyMeters.ConnectedMeter;
      if (connectedMeter == null)
        throw new Exception("Connected meter not availabl.");
      SortedList<string, S3_Parameter> parameterByName = connectedMeter.MyParameters.ParameterByName;
      VMCP_Diagnostic vmcpDiagnostic = new VMCP_Diagnostic();
      if (connectedMeter.MyIdentification.IsWR4)
      {
        SortedList<string, S3_Parameter> parameterList = new SortedList<string, S3_Parameter>();
        for (int index = 0; index < this.VMCP_DiagnosticParameters.Length; ++index)
        {
          if (parameterByName.ContainsKey(this.VMCP_DiagnosticParameters[index]))
            parameterList.Add(this.VMCP_DiagnosticParameters[index], parameterByName[this.VMCP_DiagnosticParameters[index]]);
        }
        if (parameterList.Count == 7)
        {
          AddressRange parameterAddressRange = S3_ParameterLoader.GetParameterAddressRange(parameterList);
          ByteField MemoryData;
          if (!this.myFunctions.MyCommands.ReadMemory(MemoryLocation.RAM, (int) parameterAddressRange.StartAddress, (int) parameterAddressRange.ByteSize, out MemoryData))
            throw new InvalidOperationException("Read error on VMCP ReadFlyingTestData");
          vmcpDiagnostic.VolInputState = (VMCP_State) MemoryData.Data[(IntPtr) ((long) parameterList[S3_ParameterNames.VolInputState.ToString()].BlockStartAddress - (long) parameterAddressRange.StartAddress)];
          vmcpDiagnostic.VMCP_CycleCalibrationPossible = MemoryData.Data[(IntPtr) ((long) parameterList[S3_ParameterNames.VMCP_CycleCalibrationPossible.ToString()].BlockStartAddress - (long) parameterAddressRange.StartAddress)];
          vmcpDiagnostic.VolReceiveErrorCounter = MemoryData.Data[(IntPtr) ((long) parameterList[S3_ParameterNames.VolReceiveErrorCounter.ToString()].BlockStartAddress - (long) parameterAddressRange.StartAddress)];
          vmcpDiagnostic.VolReceiveWaitQuietCounter = MemoryData.Data[(IntPtr) ((long) parameterList[S3_ParameterNames.VolReceiveWaitQuietCounter.ToString()].BlockStartAddress - (long) parameterAddressRange.StartAddress)];
          vmcpDiagnostic.RequestedEnergyCycleTime = MemoryData.Data[(IntPtr) ((long) parameterList[S3_ParameterNames.RequestedEnergyCycleTime.ToString()].BlockStartAddress - (long) parameterAddressRange.StartAddress)];
          vmcpDiagnostic.VolumeMeterIdentification = BitConverter.ToUInt32(MemoryData.Data, (int) ((long) parameterList[S3_ParameterNames.VolumeMeterIdentification.ToString()].BlockStartAddress - (long) parameterAddressRange.StartAddress));
          vmcpDiagnostic.VolumeInputValueBefore = BitConverter.ToUInt64(MemoryData.Data, (int) ((long) parameterList[S3_ParameterNames.VolumeInputValueBefore.ToString()].BlockStartAddress - (long) parameterAddressRange.StartAddress));
        }
      }
      return vmcpDiagnostic;
    }

    internal ulong ReadVMCP_Volume()
    {
      int index1 = this.myFunctions.MyMeters.ConnectedMeter.theMap.FindIndex((Predicate<KeyValuePair<string, int>>) (x => x.Key == "VolumeInputReceiveBuffer"));
      if (index1 < 0)
        throw new Exception("VolumeInputReceiveBuffer variable not found");
      AddressRange addressRange = new AddressRange((uint) this.myFunctions.MyMeters.ConnectedMeter.theMap[index1].Value, 7U);
      ByteField MemoryData;
      if (!this.myFunctions.MyCommands.ReadMemory(MemoryLocation.RAM, (int) addressRange.StartAddress, (int) addressRange.ByteSize, out MemoryData))
        throw new InvalidOperationException("Read error on VolumeInputReceiveBuffer");
      ulong num1 = 0;
      ulong num2 = 1;
      for (int index2 = 0; index2 < 7; ++index2)
      {
        ulong num3 = num1 + (ulong) ((int) MemoryData.Data[index2] & 15) * num2;
        ulong num4 = num2 * 10UL;
        num1 = num3 + (ulong) (((int) MemoryData.Data[index2] & 240) >> 4) * num4;
        num2 = num4 * 10UL;
      }
      return num1;
    }
  }
}
