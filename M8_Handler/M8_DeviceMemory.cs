// Decompiled with JetBrains decompiler
// Type: M8_Handler.M8_DeviceMemory
// Assembly: M8_Handler, Version=2.0.6.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 582F1296-F274-42DF-B72B-4C0B4D92AA72
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\M8_Handler.dll

using HandlerLib;
using HandlerLib.MapManagement;
using System;
using System.Collections.Generic;
using System.Reflection;
using ZENNER.CommonLibrary;

#nullable disable
namespace M8_Handler
{
  internal class M8_DeviceMemory : DeviceMemory
  {
    internal static M8_Params[] BasicConfiguration = new M8_Params[0];
    public static SortedList<CommonOverwriteGroups, string[]> ParameterGroups;
    private static string[] IGNORE_PARAMETER = new string[19]
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
      "bigBuffer"
    };
    internal AddressRange ArmIdRange;

    static M8_DeviceMemory()
    {
      M8_DeviceMemory.ParameterGroups = new SortedList<CommonOverwriteGroups, string[]>();
      M8_DeviceMemory.AddParameterGroupContent(CommonOverwriteGroups.BasicConfiguration, M8_DeviceMemory.BasicConfiguration);
    }

    private static void AddParameterGroupContent(
      CommonOverwriteGroups theGroup,
      M8_Params[] parameterList)
    {
      string[] strArray = new string[parameterList.Length];
      for (int index = 0; index < parameterList.Length; ++index)
        strArray[index] = parameterList[index].ToString();
      M8_DeviceMemory.ParameterGroups.Add(theGroup, strArray);
    }

    internal M8_DeviceMemory(uint version)
      : base(version, Assembly.GetExecutingAssembly())
    {
      this.DefineMapAndRanges();
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.ArmIdRange, 0U);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetSectionRanges("VERSION_SECTION"));
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetSectionRanges("METROLOGICAL_SECTION"));
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetSectionRanges("MY_ROM_SECTION"), 250U);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetParameter(M8_Params.protocol_lora_version_major.ToString()).AddressRange);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetParameter(M8_Params.protocol_lora_version_minor.ToString()).AddressRange);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetParameter(M8_Params.protocol_lorawan_version_major.ToString()).AddressRange);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetParameter(M8_Params.protocol_lorawan_version_middle.ToString()).AddressRange);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetParameter(M8_Params.protocol_lorawan_version_minor.ToString()).AddressRange);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetParameter(M8_Params.protocol_radio_version_major.ToString()).AddressRange);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetParameter(M8_Params.protocol_radio_version_minor.ToString()).AddressRange);
      foreach (AddressRange usedSubAddressRange in this.MapDef.GetUsedSubAddressRanges(new AddressRange(536870912U)
      {
        EndAddress = 536891391U
      }, 4096))
        this.AddMemoryBlock(DeviceMemoryType.DataRAM, usedSubAddressRange, 0U);
    }

    private M8_DeviceMemory(M8_DeviceMemory M8_DeviceMemoryToClone)
      : base((DeviceMemory) M8_DeviceMemoryToClone)
    {
      this.ArmIdRange = M8_DeviceMemoryToClone.ArmIdRange.Clone();
    }

    internal M8_DeviceMemory(byte[] compressedData)
      : base(compressedData)
    {
      this.MapDef = MapDefClassBase.GetMapObjectFromVersion(Assembly.GetExecutingAssembly(), this.FirmwareVersion);
      if (this.MapDef == null)
        return;
      this.DefineMapAndRanges();
    }

    private void DefineMapAndRanges()
    {
      this.UsedParametersByName = this.MapDef.GetUsedParametersList(Enum.GetNames(typeof (M8_Params)));
      this.ArmIdRange = new AddressRange(536346704U, 24U);
    }

    internal M8_DeviceMemory Clone() => new M8_DeviceMemory(this);

    internal List<AddressRange> GetRangesForRead(bool includeLogger, bool isDump = false)
    {
      if (isDump)
        return this.GetRangesForRead(new List<string>(), 10);
      SortedList<string, Parameter32bit> allParametersList = this.MapDef.GetAllParametersList();
      List<string> ignoreParameter = new List<string>((IEnumerable<string>) M8_DeviceMemory.IGNORE_PARAMETER);
      if (!includeLogger)
        ignoreParameter.Add(M8_Params.hca_reading.ToString());
      List<AddressRange> rangesForRead = this.GetRangesForRead(ignoreParameter, 10);
      Parameter32bit parameter32bit = allParametersList[M8_Params.hca_reading.ToString()];
      rangesForRead.Add(new AddressRange(parameter32bit.Address, 4U));
      return rangesForRead;
    }

    internal void SetParameterValue<T>(M8_Params parameterName, T theValue)
    {
      this.SetParameterValue<T>(parameterName.ToString(), theValue);
    }

    internal T GetParameterValue<T>(M8_Params parameterName)
    {
      return this.GetParameterValue<T>(parameterName.ToString());
    }

    internal bool IsParameterAvailable(M8_Params parameterName)
    {
      return this.IsParameterAvailable(parameterName.ToString());
    }

    internal byte[] GetData(M8_Params parameterName) => this.GetData(parameterName.ToString());

    internal void SetData(M8_Params parameterName, byte[] buffer)
    {
      this.SetData(parameterName.ToString(), buffer);
    }
  }
}
