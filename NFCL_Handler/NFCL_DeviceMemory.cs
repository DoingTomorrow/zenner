// Decompiled with JetBrains decompiler
// Type: NFCL_Handler.NFCL_DeviceMemory
// Assembly: NFCL_Handler, Version=2.3.2.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 887E21A2-7448-48CC-AF3E-C39E4C7B3AFD
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NFCL_Handler.dll

using HandlerLib;
using HandlerLib.MapManagement;
using System;
using System.Collections.Generic;
using System.Reflection;
using ZENNER.CommonLibrary;

#nullable disable
namespace NFCL_Handler
{
  internal class NFCL_DeviceMemory : DeviceMemory
  {
    internal static NFCL_Params[] BasicConfiguration = new NFCL_Params[36]
    {
      NFCL_Params.MeterId,
      NFCL_Params.HardwareTypeId,
      NFCL_Params.MeterInfoId,
      NFCL_Params.BaseTypeId,
      NFCL_Params.MeterTypeId,
      NFCL_Params.SAP_MaterialNumber,
      NFCL_Params.SAP_ProductionOrderNumber,
      NFCL_Params.Printed_serialnumber,
      NFCL_Params.IdentificationChecksum,
      NFCL_Params.Bak_TimeZoneInQuarterHours,
      NFCL_Params.Mbus_ID,
      NFCL_Params.Mbus_Manufacturer,
      NFCL_Params.Mbus_Generation,
      NFCL_Params.Mbus_Medium,
      NFCL_Params.Mbus_Obis_code,
      NFCL_Params.cfg_battery_end_life_date,
      NFCL_Params.cfg_lora_basic_frequency1,
      NFCL_Params.cfg_lora_basic_frequency2,
      NFCL_Params.cfg_lora_basic_frequency3,
      NFCL_Params.cfg_lora_rx_freq_window2,
      NFCL_Params.radio_frequency_inc_dec,
      NFCL_Params.radio_center_frequency,
      NFCL_Params.radio_band_with,
      NFCL_Params.radio_afc_band_with,
      NFCL_Params.radio_tx_data_rate,
      NFCL_Params.radio_rx_data_rate,
      NFCL_Params.radio_power,
      NFCL_Params.radio_frequency_deviation,
      NFCL_Params.radio_TxBandwithLora,
      NFCL_Params.radio_carrier_mode,
      NFCL_Params.radio_transmit_sync1,
      NFCL_Params.radio_transmit_sync2,
      NFCL_Params.radio_resceive_sync1,
      NFCL_Params.radio_resceive_sync2,
      NFCL_Params.radio_reg_tx_bandwith_lora,
      NFCL_Params.cfg_ModuleCode
    };
    public static SortedList<CommonOverwriteGroups, string[]> ParameterGroups;
    private static string[] IGNORE_PARAMETER = new string[29]
    {
      "preambel_time_counter",
      "LMIC",
      "irda_buffer",
      "AESKEY",
      "hspi1",
      "packet_trx",
      "hlcd",
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
      "hca_cal_t",
      "bigBuffer",
      "Lora_packet",
      "Lora_parameter",
      "hi2c1",
      "hirda2",
      "Radio_parameter",
      "sen_ctx",
      "cr95Ctx",
      "nfcData",
      "buf",
      "hrtc"
    };
    internal AddressRange ArmIdRange;

    static NFCL_DeviceMemory()
    {
      NFCL_DeviceMemory.ParameterGroups = new SortedList<CommonOverwriteGroups, string[]>();
      NFCL_DeviceMemory.AddParameterGroupContent(CommonOverwriteGroups.BasicConfiguration, NFCL_DeviceMemory.BasicConfiguration);
    }

    private static void AddParameterGroupContent(
      CommonOverwriteGroups theGroup,
      NFCL_Params[] parameterList)
    {
      string[] strArray = new string[parameterList.Length];
      for (int index = 0; index < parameterList.Length; ++index)
        strArray[index] = parameterList[index].ToString();
      NFCL_DeviceMemory.ParameterGroups.Add(theGroup, strArray);
    }

    internal NFCL_DeviceMemory(uint version)
      : base(version, Assembly.GetExecutingAssembly())
    {
      this.DefineMapAndRanges();
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.ArmIdRange, 0U);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetSectionRanges("VERSION_SECTION"));
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetSectionRanges("METROLOGY_DATA_CONFIG"));
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetAddressRangeOfDefinedParameters(new string[1]
      {
        NFCL_Params.cfg_AccessRadioKey.ToString()
      }));
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetParameter(NFCL_Params.protocol_lora_version_major.ToString()).AddressRange);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetParameter(NFCL_Params.protocol_lora_version_minor.ToString()).AddressRange);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetParameter(NFCL_Params.protocol_lorawan_version_major.ToString()).AddressRange);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetParameter(NFCL_Params.protocol_lorawan_version_middle.ToString()).AddressRange);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetParameter(NFCL_Params.protocol_lorawan_version_minor.ToString()).AddressRange);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetParameter(NFCL_Params.protocol_radio_version_major.ToString()).AddressRange);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetParameter(NFCL_Params.protocol_radio_version_minor.ToString()).AddressRange);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetParameter(NFCL_Params.ndc_lib_version.ToString()).AddressRange);
      foreach (AddressRange usedSubAddressRange in this.MapDef.GetUsedSubAddressRanges(new AddressRange(536870912U)
      {
        EndAddress = 536891391U
      }, 4096))
        this.AddMemoryBlock(DeviceMemoryType.DataRAM, usedSubAddressRange, 0U);
    }

    private NFCL_DeviceMemory(NFCL_DeviceMemory NFCL_DeviceMemoryToClone)
      : base((DeviceMemory) NFCL_DeviceMemoryToClone)
    {
      this.ArmIdRange = NFCL_DeviceMemoryToClone.ArmIdRange.Clone();
    }

    internal NFCL_DeviceMemory(byte[] compressedData)
      : base(compressedData)
    {
      this.MapDef = MapDefClassBase.GetMapObjectFromVersion(Assembly.GetExecutingAssembly(), this.FirmwareVersion);
      if (this.MapDef == null)
        return;
      this.DefineMapAndRanges();
    }

    private void DefineMapAndRanges()
    {
      this.UsedParametersByName = this.MapDef.GetUsedParametersList(Enum.GetNames(typeof (NFCL_Params)));
      this.ArmIdRange = new AddressRange(536346704U, 24U);
    }

    internal NFCL_DeviceMemory Clone() => new NFCL_DeviceMemory(this);

    internal List<AddressRange> GetRangesForRead(bool includeLogger, bool isDump = false)
    {
      return isDump ? this.GetRangesForRead(new List<string>(), 10) : this.GetRangesForRead(new List<string>((IEnumerable<string>) NFCL_DeviceMemory.IGNORE_PARAMETER), 10);
    }

    internal void SetParameterValue<T>(NFCL_Params parameterName, T theValue)
    {
      this.SetParameterValue<T>(parameterName.ToString(), theValue);
    }

    internal T GetParameterValue<T>(NFCL_Params parameterName)
    {
      return this.GetParameterValue<T>(parameterName.ToString());
    }

    internal bool IsParameterAvailable(NFCL_Params parameterName)
    {
      return this.IsParameterAvailable(parameterName.ToString());
    }

    internal byte[] GetData(NFCL_Params parameterName) => this.GetData(parameterName.ToString());

    internal void SetData(NFCL_Params parameterName, byte[] buffer)
    {
      this.SetData(parameterName.ToString(), buffer);
    }
  }
}
