// Decompiled with JetBrains decompiler
// Type: EDCL_Handler.EDCL_DeviceMemory
// Assembly: EDCL_Handler, Version=2.2.10.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F3010E47-8885-4BE8-8551-D37B09710D3C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDCL_Handler.dll

using HandlerLib;
using HandlerLib.MapManagement;
using System;
using System.Collections.Generic;
using System.Reflection;
using ZENNER.CommonLibrary;

#nullable disable
namespace EDCL_Handler
{
  internal class EDCL_DeviceMemory : DeviceMemory
  {
    internal static EDCL_Params[] BasicConfiguration = new EDCL_Params[0];
    internal static EDCL_Params[] IdentData = new EDCL_Params[36]
    {
      EDCL_Params.MeterId,
      EDCL_Params.MeterInfoId,
      EDCL_Params.HardwareTypeId,
      EDCL_Params.BaseTypeId,
      EDCL_Params.MeterTypeId,
      EDCL_Params.SAP_MaterialNumber,
      EDCL_Params.SAP_ProductionOrderNumber,
      EDCL_Params.cfg_lora_deveui,
      EDCL_Params.cfg_lora_appeui,
      EDCL_Params.cfb_lora_AppKey,
      EDCL_Params.fullserialnumber,
      EDCL_Params.Printed_serialnumber,
      EDCL_Params.Mbus_ID,
      EDCL_Params.Mbus_Manufacturer,
      EDCL_Params.Mbus_Medium,
      EDCL_Params.Mbus_Obis_code,
      EDCL_Params.Mbus_Generation,
      EDCL_Params.Mbus_ID_meter,
      EDCL_Params.Mbus_Manufacturer_meter,
      EDCL_Params.Mbus_Medium_meter,
      EDCL_Params.Mbus_Obis_code_meter,
      EDCL_Params.Mbus_Generation_meter,
      EDCL_Params.Mbus_aes_key,
      EDCL_Params.FD_Lora_DevEUI,
      EDCL_Params.FD_LoRa_AppEUI,
      EDCL_Params.FD_LoRa_AppKey,
      EDCL_Params.FD_Mbus_Generation,
      EDCL_Params.FD_Mbus_Generation_meter,
      EDCL_Params.FD_Mbus_ID,
      EDCL_Params.FD_Mbus_ID_meter,
      EDCL_Params.FD_Mbus_Manufacturer,
      EDCL_Params.FD_Mbus_Manufacturer_meter,
      EDCL_Params.FD_Mbus_Medium,
      EDCL_Params.FD_Mbus_Medium_meter,
      EDCL_Params.FD_Obis_code,
      EDCL_Params.FD_Obis_code_meter
    };
    internal static EDCL_Params[] TypeIdentification = new EDCL_Params[6]
    {
      EDCL_Params.MeterInfoId,
      EDCL_Params.HardwareTypeId,
      EDCL_Params.BaseTypeId,
      EDCL_Params.MeterTypeId,
      EDCL_Params.SAP_MaterialNumber,
      EDCL_Params.SAP_ProductionOrderNumber
    };
    public static SortedList<CommonOverwriteGroups, string[]> ParameterGroups;
    private static string[] IGNORE_PARAMETER = new string[35]
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
      "AES_E4"
    };
    internal AddressRange ArmIdRange;

    static EDCL_DeviceMemory()
    {
      EDCL_DeviceMemory.ParameterGroups = new SortedList<CommonOverwriteGroups, string[]>();
      EDCL_DeviceMemory.AddParameterGroupContent(CommonOverwriteGroups.BasicConfiguration, EDCL_DeviceMemory.BasicConfiguration);
      EDCL_DeviceMemory.AddParameterGroupContent(CommonOverwriteGroups.IdentData, EDCL_DeviceMemory.IdentData);
      EDCL_DeviceMemory.AddParameterGroupContent(CommonOverwriteGroups.TypeIdentification, EDCL_DeviceMemory.TypeIdentification);
    }

    private static void AddParameterGroupContent(
      CommonOverwriteGroups theGroup,
      EDCL_Params[] parameterList)
    {
      string[] strArray = new string[parameterList.Length];
      for (int index = 0; index < parameterList.Length; ++index)
        strArray[index] = parameterList[index].ToString();
      EDCL_DeviceMemory.ParameterGroups.Add(theGroup, strArray);
    }

    internal EDCL_DeviceMemory(uint version)
      : base(version, Assembly.GetExecutingAssembly())
    {
      this.DefineMapAndRanges();
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.ArmIdRange, 0U);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetSectionRanges("VERSION_SECTION"));
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetAddressRangeOfDefinedParameters(new string[1]
      {
        EDCL_Params.cfg_AccessRadioKey.ToString()
      }));
      foreach (AddressRange usedSubAddressRange in this.MapDef.GetUsedSubAddressRanges(new AddressRange(536870912U)
      {
        EndAddress = 536891391U
      }, 4096))
        this.AddMemoryBlock(DeviceMemoryType.DataRAM, usedSubAddressRange, 0U);
    }

    private EDCL_DeviceMemory(EDCL_DeviceMemory EDCL_DeviceMemoryToClone)
      : base((DeviceMemory) EDCL_DeviceMemoryToClone)
    {
      this.ArmIdRange = EDCL_DeviceMemoryToClone.ArmIdRange;
    }

    internal EDCL_DeviceMemory(byte[] compressedData)
      : base(compressedData)
    {
      this.MapDef = MapDefClassBase.GetMapObjectFromVersion(Assembly.GetExecutingAssembly(), this.FirmwareVersion);
      if (this.MapDef == null)
        return;
      this.DefineMapAndRanges();
    }

    private void DefineMapAndRanges()
    {
      this.UsedParametersByName = this.MapDef.GetUsedParametersList(Enum.GetNames(typeof (EDCL_Params)));
      this.ArmIdRange = new AddressRange(536346704U, 24U);
    }

    internal EDCL_DeviceMemory Clone() => new EDCL_DeviceMemory(this);

    internal List<AddressRange> GetRangesForRead(bool includeLogger, bool isDump = false)
    {
      List<AddressRange> addressRangeList = new List<AddressRange>();
      if (isDump)
        return this.GetRangesForRead(new List<string>(), 10);
      List<string> ignoreParameter = new List<string>((IEnumerable<string>) EDCL_DeviceMemory.IGNORE_PARAMETER);
      if (!includeLogger)
        ignoreParameter.Add(EDCL_Params.meter_reading.ToString());
      return this.GetRangesForRead(ignoreParameter, 10);
    }

    internal void SetParameterValue<T>(EDCL_Params parameterName, T theValue)
    {
      this.SetParameterValue<T>(parameterName.ToString(), theValue);
    }

    internal T GetParameterValue<T>(EDCL_Params parameterName)
    {
      return this.GetParameterValue<T>(parameterName.ToString());
    }

    internal bool IsParameterAvailable(EDCL_Params parameterName)
    {
      return this.IsParameterAvailable(parameterName.ToString());
    }

    internal byte[] GetData(EDCL_Params parameterName) => this.GetData(parameterName.ToString());

    internal void SetData(EDCL_Params parameterName, byte[] buffer)
    {
      this.SetData(parameterName.ToString(), buffer);
    }
  }
}
