// Decompiled with JetBrains decompiler
// Type: THL_Handler.THL_DeviceMemory
// Assembly: THL_Handler, Version=1.0.5.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: C9669406-A704-45DE-B726-D8A41F27FFB8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\THL_Handler.dll

using HandlerLib;
using HandlerLib.MapManagement;
using System;
using System.Collections.Generic;
using System.Reflection;
using ZENNER.CommonLibrary;

#nullable disable
namespace THL_Handler
{
  internal class THL_DeviceMemory : DeviceMemory
  {
    internal static THL_Params[] BasicConfiguration = new THL_Params[0];
    public static SortedList<CommonOverwriteGroups, string[]> ParameterGroups;
    private static string[] IGNORE_PARAMETER = new string[25]
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
      "th_reading",
      "Lora_packet",
      "Lora_parameter",
      "hi2c1",
      "hirda2",
      "Radio_parameter"
    };
    internal AddressRange ArmIdRange;

    static THL_DeviceMemory()
    {
      THL_DeviceMemory.ParameterGroups = new SortedList<CommonOverwriteGroups, string[]>();
      THL_DeviceMemory.AddParameterGroupContent(CommonOverwriteGroups.BasicConfiguration, THL_DeviceMemory.BasicConfiguration);
    }

    private static void AddParameterGroupContent(
      CommonOverwriteGroups theGroup,
      THL_Params[] parameterList)
    {
      string[] strArray = new string[parameterList.Length];
      for (int index = 0; index < parameterList.Length; ++index)
        strArray[index] = parameterList[index].ToString();
      THL_DeviceMemory.ParameterGroups.Add(theGroup, strArray);
    }

    internal THL_DeviceMemory(uint version)
      : base(version, Assembly.GetExecutingAssembly())
    {
      this.DefineMapAndRanges();
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.ArmIdRange, 0U);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetSectionRanges("VERSION_SECTION"));
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetSectionRanges("METROLOGY_DATA_CONFIG"));
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetParameter(THL_Params.protocol_lora_version_major.ToString()).AddressRange);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetParameter(THL_Params.protocol_lora_version_minor.ToString()).AddressRange);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetParameter(THL_Params.protocol_lorawan_version_major.ToString()).AddressRange);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetParameter(THL_Params.protocol_lorawan_version_middle.ToString()).AddressRange);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetParameter(THL_Params.protocol_lorawan_version_minor.ToString()).AddressRange);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetParameter(THL_Params.protocol_radio_version_major.ToString()).AddressRange);
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.MapDef.GetParameter(THL_Params.protocol_radio_version_minor.ToString()).AddressRange);
      foreach (AddressRange usedSubAddressRange in this.MapDef.GetUsedSubAddressRanges(new AddressRange(536870912U)
      {
        EndAddress = 536891391U
      }, 4096))
        this.AddMemoryBlock(DeviceMemoryType.DataRAM, usedSubAddressRange, 0U);
    }

    private THL_DeviceMemory(THL_DeviceMemory THL_DeviceMemoryToClone)
      : base((DeviceMemory) THL_DeviceMemoryToClone)
    {
      this.ArmIdRange = THL_DeviceMemoryToClone.ArmIdRange.Clone();
    }

    internal THL_DeviceMemory(byte[] compressedData)
      : base(compressedData)
    {
      this.MapDef = MapDefClassBase.GetMapObjectFromVersion(Assembly.GetExecutingAssembly(), this.FirmwareVersion);
      if (this.MapDef == null)
        return;
      this.DefineMapAndRanges();
    }

    private void DefineMapAndRanges()
    {
      this.UsedParametersByName = this.MapDef.GetUsedParametersList(Enum.GetNames(typeof (THL_Params)));
      this.ArmIdRange = new AddressRange(536346704U, 24U);
    }

    internal THL_DeviceMemory Clone() => new THL_DeviceMemory(this);

    internal List<AddressRange> GetRangesForRead(bool includeLogger, bool isDump = false)
    {
      return isDump ? this.GetRangesForRead(new List<string>(), 10) : this.GetRangesForRead(new List<string>((IEnumerable<string>) THL_DeviceMemory.IGNORE_PARAMETER), 10);
    }

    internal void SetParameterValue<T>(THL_Params parameterName, T theValue)
    {
      this.SetParameterValue<T>(parameterName.ToString(), theValue);
    }

    internal T GetParameterValue<T>(THL_Params parameterName)
    {
      return this.GetParameterValue<T>(parameterName.ToString());
    }

    internal bool IsParameterAvailable(THL_Params parameterName)
    {
      return this.IsParameterAvailable(parameterName.ToString());
    }

    internal byte[] GetData(THL_Params parameterName) => this.GetData(parameterName.ToString());

    internal void SetData(THL_Params parameterName, byte[] buffer)
    {
      this.SetData(parameterName.ToString(), buffer);
    }
  }
}
