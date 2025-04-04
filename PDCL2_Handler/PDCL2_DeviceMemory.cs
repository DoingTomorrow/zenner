// Decompiled with JetBrains decompiler
// Type: PDCL2_Handler.PDCL2_DeviceMemory
// Assembly: PDCL2_Handler, Version=2.22.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 03BA4C2D-69FE-4DA6-9C3F-B3D5471C4058
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDCL2_Handler.dll

using HandlerLib;
using HandlerLib.MapManagement;
using System;
using System.Collections.Generic;
using System.Reflection;
using ZENNER.CommonLibrary;

#nullable disable
namespace PDCL2_Handler
{
  internal class PDCL2_DeviceMemory : DeviceMemory
  {
    internal static PDCL2_Params[] BasicConfiguration = new PDCL2_Params[0];
    internal static PDCL2_Params[] IdentData = new PDCL2_Params[24]
    {
      PDCL2_Params.MeterId,
      PDCL2_Params.MeterInfoId,
      PDCL2_Params.HardwareTypeId,
      PDCL2_Params.BaseTypeId,
      PDCL2_Params.MeterTypeId,
      PDCL2_Params.SAP_MaterialNumber,
      PDCL2_Params.SAP_ProductionOrderNumber,
      PDCL2_Params.cfg_lora_deveui,
      PDCL2_Params.cfg_lora_appeui,
      PDCL2_Params.cfb_lora_AppKey,
      PDCL2_Params.Printed_serialnumber,
      PDCL2_Params.Mbus_ID,
      PDCL2_Params.Mbus_Manufacturer,
      PDCL2_Params.Mbus_Medium,
      PDCL2_Params.Mbus_Obis_code,
      PDCL2_Params.Mbus_Generation,
      PDCL2_Params.FD_Lora_DevEUI,
      PDCL2_Params.FD_LoRa_AppEUI,
      PDCL2_Params.FD_LoRa_AppKey,
      PDCL2_Params.FD_Mbus_Generation,
      PDCL2_Params.FD_Mbus_ID,
      PDCL2_Params.FD_Mbus_Manufacturer,
      PDCL2_Params.FD_Mbus_Medium,
      PDCL2_Params.FD_Obis_code
    };
    public static SortedList<CommonOverwriteGroups, string[]> ParameterGroups;
    private static string[] IGNORE_PARAMETER = new string[42]
    {
      "preambel_time_counter",
      "LMIC",
      "irda_buffer",
      "AESKEY",
      "hspi1",
      "packet_trx",
      "hirda1",
      "htim2",
      "htim21",
      "htim22",
      "hlptim1",
      "hcomp2",
      "hcrc",
      "ntc",
      "randbuf",
      "commBuffer",
      "Power_of_10",
      "hadc",
      "hrtc",
      "SENSITIVITY",
      "DR2HSYM_osticks",
      "iniChannelFreq",
      "TXPOWLEVELS",
      "AES_RCON",
      "AES_S",
      "AES_E1",
      "rxlorairqmask",
      "LORA_RXDONE_FIXUP",
      "dayInMonth",
      "w",
      "cidcb",
      "specialcommandcb",
      "HexToASCII",
      "AES_E2",
      "AES_E3",
      "AES_E4",
      "GPIO_InitStruct",
      "AESAUX",
      "save_data_to_send",
      "save_data_AP",
      "reportjob",
      "bcd"
    };
    internal AddressRange ArmIdRange;

    static PDCL2_DeviceMemory()
    {
      PDCL2_DeviceMemory.ParameterGroups = new SortedList<CommonOverwriteGroups, string[]>();
      PDCL2_DeviceMemory.AddParameterGroupContent(CommonOverwriteGroups.BasicConfiguration, PDCL2_DeviceMemory.BasicConfiguration);
      PDCL2_DeviceMemory.AddParameterGroupContent(CommonOverwriteGroups.IdentData, PDCL2_DeviceMemory.IdentData);
    }

    private static void AddParameterGroupContent(
      CommonOverwriteGroups theGroup,
      PDCL2_Params[] parameterList)
    {
      string[] strArray = new string[parameterList.Length];
      for (int index = 0; index < parameterList.Length; ++index)
        strArray[index] = parameterList[index].ToString();
      PDCL2_DeviceMemory.ParameterGroups.Add(theGroup, strArray);
    }

    internal PDCL2_DeviceMemory(uint version)
      : base(version, Assembly.GetExecutingAssembly())
    {
      this.DefineMapAndRanges();
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.ArmIdRange, 0U);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetSectionRanges("VERSION_SECTION"));
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetAddressRangeOfDefinedParameters(new string[1]
      {
        PDCL2_Params.cfg_AccessRadioKey.ToString()
      }));
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetParameter(PDCL2_Params.protocol_lora_version_major.ToString()).AddressRange);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetParameter(PDCL2_Params.protocol_lora_version_minor.ToString()).AddressRange);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetParameter(PDCL2_Params.protocol_lorawan_version_major.ToString()).AddressRange);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetParameter(PDCL2_Params.protocol_lorawan_version_middle.ToString()).AddressRange);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetParameter(PDCL2_Params.protocol_lorawan_version_minor.ToString()).AddressRange);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetParameter(PDCL2_Params.protocol_radio_version_major.ToString()).AddressRange);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetParameter(PDCL2_Params.protocol_radio_version_minor.ToString()).AddressRange);
      foreach (AddressRange usedSubAddressRange in this.MapDef.GetUsedSubAddressRanges(new AddressRange(536870912U)
      {
        EndAddress = 536891391U
      }, 4096))
        this.AddMemoryBlock(DeviceMemoryType.DataRAM, usedSubAddressRange, 0U);
    }

    private PDCL2_DeviceMemory(PDCL2_DeviceMemory PDCL2_DeviceMemoryToClone)
      : base((DeviceMemory) PDCL2_DeviceMemoryToClone)
    {
      this.ArmIdRange = PDCL2_DeviceMemoryToClone.ArmIdRange;
    }

    internal PDCL2_DeviceMemory(byte[] compressedData)
      : base(compressedData)
    {
      this.MapDef = MapDefClassBase.GetMapObjectFromVersion(Assembly.GetExecutingAssembly(), this.FirmwareVersion);
      if (this.MapDef == null)
        return;
      this.DefineMapAndRanges();
    }

    private void DefineMapAndRanges()
    {
      this.UsedParametersByName = this.MapDef.GetUsedParametersList(Enum.GetNames(typeof (PDCL2_Params)));
      this.ArmIdRange = new AddressRange(536346704U, 24U);
    }

    internal PDCL2_DeviceMemory Clone() => new PDCL2_DeviceMemory(this);

    internal List<AddressRange> GetRangesForRead(bool includeLogger, bool isDump = false)
    {
      List<AddressRange> addressRangeList = new List<AddressRange>();
      if (isDump)
        return this.GetRangesForRead(new List<string>(), 10);
      List<string> ignoreParameter = new List<string>((IEnumerable<string>) PDCL2_DeviceMemory.IGNORE_PARAMETER);
      if (!includeLogger)
        ignoreParameter.Add(PDCL2_Params.meter_reading.ToString());
      return this.GetRangesForRead(ignoreParameter, 10);
    }

    internal void SetParameterValue<T>(PDCL2_Params parameterName, T theValue)
    {
      this.SetParameterValue<T>(parameterName.ToString(), theValue);
    }

    internal T GetParameterValue<T>(PDCL2_Params parameterName)
    {
      return this.GetParameterValue<T>(parameterName.ToString());
    }

    internal bool IsParameterAvailable(PDCL2_Params parameterName)
    {
      return this.IsParameterAvailable(parameterName.ToString());
    }

    internal byte[] GetData(PDCL2_Params parameterName) => this.GetData(parameterName.ToString());

    internal void SetData(PDCL2_Params parameterName, byte[] buffer)
    {
      this.SetData(parameterName.ToString(), buffer);
    }
  }
}
