// Decompiled with JetBrains decompiler
// Type: GMM_Handler.EpromHeader
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using ZR_ClassLibrary;

#nullable disable
namespace GMM_Handler
{
  internal class EpromHeader : LinkBlock
  {
    internal EpromHeader(Meter MyMeterIn)
      : base(MyMeterIn, LinkBlockTypes.EpromHeader)
    {
      this.BlockStartAddress = 0;
      Parameter NewParameter1 = new Parameter("EEP_HEADER_TestByte", 1, LinkBlockTypes.EpromHeader);
      NewParameter1.ExistOnEprom = true;
      this.LinkObjList.Add((object) NewParameter1);
      this.AddToAllParameters(NewParameter1);
      Parameter NewParameter2 = new Parameter("EEP_HEADER_FillByte", 1, LinkBlockTypes.EpromHeader);
      NewParameter2.ExistOnEprom = true;
      this.LinkObjList.Add((object) NewParameter2);
      this.AddToAllParameters(NewParameter2);
      Parameter NewParameter3 = new Parameter("EEP_HEADER_MeterKey", 4, LinkBlockTypes.EpromHeader);
      NewParameter3.ExistOnEprom = true;
      NewParameter3.GroupMember[1] = true;
      this.LinkObjList.Add((object) NewParameter3);
      this.AddToAllParameters(NewParameter3);
      Parameter NewParameter4 = new Parameter("EEP_HEADER_SerialNr", 4, LinkBlockTypes.EpromHeader);
      NewParameter4.ExistOnEprom = true;
      NewParameter4.ParameterFormat = Parameter.BaseParameterFormat.BCD;
      NewParameter4.GroupMember[1] = true;
      this.LinkObjList.Add((object) NewParameter4);
      this.AddToAllParameters(NewParameter4);
      Parameter NewParameter5 = new Parameter("EEP_HEADER_MBusSerialNr", 4, LinkBlockTypes.EpromHeader);
      NewParameter5.ExistOnEprom = true;
      NewParameter5.ParameterFormat = Parameter.BaseParameterFormat.BCD;
      NewParameter5.GroupMember[5] = true;
      this.LinkObjList.Add((object) NewParameter5);
      this.AddToAllParameters(NewParameter5);
      Parameter NewParameter6 = new Parameter("EEP_HEADER_MBusManufacturer", 2, LinkBlockTypes.EpromHeader);
      NewParameter6.ExistOnEprom = true;
      NewParameter6.GroupMember[1] = true;
      this.LinkObjList.Add((object) NewParameter6);
      this.AddToAllParameters(NewParameter6);
      Parameter NewParameter7 = new Parameter("EEP_HEADER_MBusMeterType", 1, LinkBlockTypes.EpromHeader);
      NewParameter7.ExistOnEprom = true;
      NewParameter7.GroupMember[1] = true;
      this.LinkObjList.Add((object) NewParameter7);
      this.AddToAllParameters(NewParameter7);
      Parameter NewParameter8 = new Parameter("EEP_HEADER_MBusMedium", 1, LinkBlockTypes.EpromHeader);
      NewParameter8.ExistOnEprom = true;
      NewParameter8.GroupMember[1] = true;
      this.LinkObjList.Add((object) NewParameter8);
      this.AddToAllParameters(NewParameter8);
      Parameter NewParameter9 = new Parameter("EEP_HEADER_MeterID", 4, LinkBlockTypes.EpromHeader);
      NewParameter9.ExistOnEprom = true;
      NewParameter9.GroupMember[1] = true;
      this.LinkObjList.Add((object) NewParameter9);
      this.AddToAllParameters(NewParameter9);
      Parameter NewParameter10 = new Parameter("EEP_HEADER_MeterInfoID", 4, LinkBlockTypes.EpromHeader);
      NewParameter10.ExistOnEprom = true;
      NewParameter10.GroupMember[1] = true;
      this.LinkObjList.Add((object) NewParameter10);
      this.AddToAllParameters(NewParameter10);
      Parameter NewParameter11 = new Parameter("EEP_HEADER_MeterTypeID", 4, LinkBlockTypes.EpromHeader);
      NewParameter11.ExistOnEprom = true;
      NewParameter11.GroupMember[1] = true;
      this.LinkObjList.Add((object) NewParameter11);
      this.AddToAllParameters(NewParameter11);
      Parameter NewParameter12 = new Parameter("EEP_HEADER_RamParamBlockAdr", 2, LinkBlockTypes.EpromHeader);
      NewParameter12.ExistOnEprom = true;
      this.LinkObjList.Add((object) NewParameter12);
      this.AddToAllParameters(NewParameter12);
      Parameter NewParameter13 = new Parameter("EEP_HEADER_BackupBlockAdr", 2, LinkBlockTypes.EpromHeader);
      NewParameter13.ExistOnEprom = true;
      this.LinkObjList.Add((object) NewParameter13);
      this.AddToAllParameters(NewParameter13);
      Parameter NewParameter14 = new Parameter("EEP_HEADER_FixedParamAdr", 2, LinkBlockTypes.EpromHeader);
      NewParameter14.ExistOnEprom = true;
      this.LinkObjList.Add((object) NewParameter14);
      this.AddToAllParameters(NewParameter14);
      Parameter NewParameter15 = new Parameter("EEP_HEADER_WritePermTableAdr", 2, LinkBlockTypes.EpromHeader);
      NewParameter15.ExistOnEprom = true;
      this.LinkObjList.Add((object) NewParameter15);
      this.AddToAllParameters(NewParameter15);
      Parameter NewParameter16 = new Parameter("EEP_HEADER_DispBlockAdr", 2, LinkBlockTypes.EpromHeader);
      NewParameter16.ExistOnEprom = true;
      this.LinkObjList.Add((object) NewParameter16);
      this.AddToAllParameters(NewParameter16);
      Parameter NewParameter17 = new Parameter("EEP_HEADER_RuntimeVarsAdr", 2, LinkBlockTypes.EpromHeader);
      NewParameter17.ExistOnEprom = true;
      this.LinkObjList.Add((object) NewParameter17);
      this.AddToAllParameters(NewParameter17);
      Parameter NewParameter18 = new Parameter("EEP_HEADER_RuntimeCodeAdr", 2, LinkBlockTypes.EpromHeader);
      NewParameter18.ExistOnEprom = true;
      this.LinkObjList.Add((object) NewParameter18);
      this.AddToAllParameters(NewParameter18);
      Parameter NewParameter19 = new Parameter("EEP_HEADER_EpromVarsAdr", 2, LinkBlockTypes.EpromHeader);
      NewParameter19.ExistOnEprom = true;
      this.LinkObjList.Add((object) NewParameter19);
      this.AddToAllParameters(NewParameter19);
      Parameter NewParameter20 = new Parameter("EEP_HEADER_ParamBlockAdr", 2, LinkBlockTypes.EpromHeader);
      NewParameter20.ExistOnEprom = true;
      this.LinkObjList.Add((object) NewParameter20);
      this.AddToAllParameters(NewParameter20);
      Parameter NewParameter21 = new Parameter("EEP_HEADER_EpromRuntimeAdr", 2, LinkBlockTypes.EpromHeader);
      NewParameter21.ExistOnEprom = true;
      this.LinkObjList.Add((object) NewParameter21);
      this.AddToAllParameters(NewParameter21);
      Parameter NewParameter22 = new Parameter("EEP_HEADER_MBusBlockAdr", 2, LinkBlockTypes.EpromHeader);
      NewParameter22.ExistOnEprom = true;
      this.LinkObjList.Add((object) NewParameter22);
      this.AddToAllParameters(NewParameter22);
      Parameter NewParameter23 = new Parameter("EEP_HEADER_FunctionTableAdr", 2, LinkBlockTypes.EpromHeader);
      NewParameter23.ExistOnEprom = true;
      this.LinkObjList.Add((object) NewParameter23);
      this.AddToAllParameters(NewParameter23);
      Parameter NewParameter24 = new Parameter("EEP_HEADER_StaticChecksum", 2, LinkBlockTypes.EpromHeader);
      NewParameter24.ExistOnEprom = true;
      this.LinkObjList.Add((object) NewParameter24);
      this.AddToAllParameters(NewParameter24);
      Parameter NewParameter25 = new Parameter("EEP_HEADER_HeaderChecksum", 2, LinkBlockTypes.EpromHeader);
      NewParameter25.ExistOnEprom = true;
      this.LinkObjList.Add((object) NewParameter25);
      this.AddToAllParameters(NewParameter25);
      Parameter NewParameter26 = new Parameter("EEP_HEADER_BackupChecksum", 2, LinkBlockTypes.EpromHeader);
      NewParameter26.ExistOnEprom = true;
      this.LinkObjList.Add((object) NewParameter26);
      this.AddToAllParameters(NewParameter26);
      Parameter NewParameter27 = new Parameter("EEP_HEADER_EditStr", 16, LinkBlockTypes.EpromHeader);
      NewParameter27.ExistOnEprom = true;
      this.LinkObjList.Add((object) NewParameter27);
      this.AddToAllParameters(NewParameter27);
    }

    private void AddToAllParameters(Parameter NewParameter)
    {
      string key = "EEP_Header." + NewParameter.Name;
      NewParameter.FullName = key;
      this.MyMeter.AllParameters.Add((object) key, (object) NewParameter);
    }

    internal bool ReadHeaderFromConnectedDevice()
    {
      int address1 = ((LinkObj) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_SerialNr"]).Address;
      Parameter allParameter = (Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_EditStr"];
      int num = allParameter.Address + allParameter.Size - address1;
      ByteField MemoryData = new ByteField(num);
      if (!this.MyMeter.MyCommunication.MyBus.ReadMemory(MemoryLocation.EEPROM, address1, num, out MemoryData))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "");
        return false;
      }
      this.MyMeter.Eprom = new byte[address1 + num];
      for (int index = 0; index < num; ++index)
        this.MyMeter.Eprom[index + address1] = MemoryData.Data[index];
      int address2 = ((LinkObj) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_HeaderChecksum"]).Address;
      if ((int) this.GenerateChecksum(this.MyMeter.Eprom, address1, address2 - address1, (ushort) 0) != (int) ParameterService.GetFromByteArray_ushort(this.MyMeter.Eprom, ref address2))
      {
        if (this.MyMeter.MyHandler.checksumErrorsAsWarning)
        {
          ZR_ClassLibMessages.AddWarning("Header checksum error");
        }
        else
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Header checksum error");
          return false;
        }
      }
      return true;
    }

    internal bool GenerateChecksums()
    {
      Parameter allParameter1 = (Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_StaticChecksum"];
      Parameter allParameter2 = (Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_HeaderChecksum"];
      Parameter allParameter3 = (Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_BackupChecksum"];
      int blockStartAddress1 = this.MyMeter.MyRamParameter.BlockStartAddress;
      int NumberOfBytes1 = this.MyMeter.MyBackup.BlockStartAddress - blockStartAddress1;
      ushort checksum1 = this.GenerateChecksum(this.MyMeter.Eprom, blockStartAddress1, NumberOfBytes1, (ushort) 0);
      int blockStartAddress2 = this.MyMeter.MyFixedParameter.BlockStartAddress;
      int NumberOfBytes2 = this.MyMeter.MyRuntimeVars.BlockStartAddress - blockStartAddress2;
      ushort checksum2 = this.GenerateChecksum(this.MyMeter.Eprom, blockStartAddress2, NumberOfBytes2, checksum1);
      int blockStartAddress3 = this.MyMeter.MyRuntimeCode.BlockStartAddress;
      int NumberOfBytes3 = this.MyMeter.MyEpromVars.BlockStartAddress - blockStartAddress3;
      ushort checksum3 = this.GenerateChecksum(this.MyMeter.Eprom, blockStartAddress3, NumberOfBytes3, checksum2);
      int blockStartAddress4 = this.MyMeter.MyEpromRuntime.BlockStartAddress;
      int NumberOfBytes4 = this.MyMeter.MyFunctionTable.BlockStartAddress - blockStartAddress4;
      ushort checksum4 = this.GenerateChecksum(this.MyMeter.Eprom, blockStartAddress4, NumberOfBytes4, checksum3);
      allParameter1.ValueEprom = (long) checksum4;
      allParameter1.UpdateByteList();
      allParameter1.CopyToEprom(this.MyMeter.Eprom);
      int address = ((LinkObj) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_SerialNr"]).Address;
      int NumberOfBytes5 = allParameter2.Address - address;
      ushort checksum5 = this.GenerateChecksum(this.MyMeter.Eprom, address, NumberOfBytes5, (ushort) 0);
      allParameter2.ValueEprom = (long) checksum5;
      allParameter2.UpdateByteList();
      allParameter2.CopyToEprom(this.MyMeter.Eprom);
      int blockStartAddress5 = this.MyMeter.MyBackup.BlockStartAddress;
      int NumberOfBytes6 = this.MyMeter.MyFixedParameter.BlockStartAddress - blockStartAddress5;
      ushort checksum6 = this.GenerateChecksum(this.MyMeter.Eprom, blockStartAddress5, NumberOfBytes6, (ushort) 0);
      int blockStartAddress6 = this.MyMeter.MyRuntimeVars.BlockStartAddress;
      int NumberOfBytes7 = this.MyMeter.MyRuntimeCode.BlockStartAddress - blockStartAddress6;
      ushort checksum7 = this.GenerateChecksum(this.MyMeter.Eprom, blockStartAddress6, NumberOfBytes7, checksum6);
      allParameter3.ValueEprom = (long) checksum7;
      allParameter3.UpdateByteList();
      allParameter3.CopyToEprom(this.MyMeter.Eprom);
      return true;
    }

    internal bool AllChecksumsOk()
    {
      Parameter allParameter1 = (Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_StaticChecksum"];
      Parameter allParameter2 = (Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_BackupChecksum"];
      int valueEprom1 = (int) ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_RamParamBlockAdr"]).ValueEprom;
      int NumberOfBytes1 = (int) ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_BackupBlockAdr"]).ValueEprom - valueEprom1;
      ushort checksum1 = this.GenerateChecksum(this.MyMeter.Eprom, valueEprom1, NumberOfBytes1, (ushort) 0);
      Parameter allParameter3 = (Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_FixedParamAdr"];
      int valueEprom2 = (int) allParameter3.ValueEprom;
      Parameter allParameter4 = (Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_RuntimeVarsAdr"];
      int NumberOfBytes2 = (int) allParameter4.ValueEprom - valueEprom2;
      ushort checksum2 = this.GenerateChecksum(this.MyMeter.Eprom, valueEprom2, NumberOfBytes2, checksum1);
      Parameter allParameter5 = (Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_RuntimeCodeAdr"];
      int valueEprom3 = (int) allParameter5.ValueEprom;
      int NumberOfBytes3 = (int) ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_EpromVarsAdr"]).ValueEprom - valueEprom3;
      ushort checksum3 = this.GenerateChecksum(this.MyMeter.Eprom, valueEprom3, NumberOfBytes3, checksum2);
      int valueEprom4 = (int) ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_ParamBlockAdr"]).ValueEprom;
      int NumberOfBytes4 = (int) ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_FunctionTableAdr"]).ValueEprom - valueEprom4;
      ushort checksum4 = this.GenerateChecksum(this.MyMeter.Eprom, valueEprom4, NumberOfBytes4, checksum3);
      if (allParameter1.ValueEprom != (long) checksum4)
      {
        if (this.MyMeter.MyHandler.checksumErrorsAsWarning)
        {
          ZR_ClassLibMessages.AddWarning("Static checksum error");
        }
        else
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Static checksum error");
          return false;
        }
      }
      int valueEprom5 = (int) ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_BackupBlockAdr"]).ValueEprom;
      int NumberOfBytes5 = (int) allParameter3.ValueEprom - valueEprom5;
      ushort checksum5 = this.GenerateChecksum(this.MyMeter.Eprom, valueEprom5, NumberOfBytes5, (ushort) 0);
      int valueEprom6 = (int) allParameter4.ValueEprom;
      int NumberOfBytes6 = (int) allParameter5.ValueEprom - valueEprom6;
      ushort checksum6 = this.GenerateChecksum(this.MyMeter.Eprom, valueEprom6, NumberOfBytes6, checksum5);
      if (allParameter2.ValueEprom != (long) checksum6)
        ZR_ClassLibMessages.AddWarning("Backup checksum error");
      return true;
    }

    internal ushort GenerateChecksum(
      byte[] Data,
      int StartOffset,
      int NumberOfBytes,
      ushort ChecksumIn)
    {
      for (int index = 0; index < NumberOfBytes; ++index)
        ChecksumIn ^= (ushort) ((int) Data[index + StartOffset] + ((int) ChecksumIn << 1) + 1);
      return ChecksumIn;
    }

    internal bool UpdateIdentData()
    {
      this.MyMeter.MyIdent.MeterID = (int) ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_MeterID"]).ValueEprom;
      this.MyMeter.MyIdent.MeterInfoID = (int) ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_MeterTypeID"]).ValueEprom;
      this.MyMeter.MyIdent.MeterInfoBaseID = (int) ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_MeterInfoID"]).ValueEprom;
      this.MyMeter.MyIdent.SerialNr = ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_SerialNr"]).ValueEprom.ToString("X8");
      this.MyMeter.MyIdent.MBusSerialNr = ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_MBusSerialNr"]).ValueEprom.ToString("X8");
      return this.MyMeter.MyIdent.MeterInfoID != 0 && this.MyMeter.MyIdent.MeterInfoID != this.MyMeter.MyIdent.MeterInfoBaseID || this.MyMeter.MyHandler.MyDataBaseAccess.GetLastMeterInfoID(this.MyMeter.MyIdent);
    }

    internal bool UpdateMBusHeaderData()
    {
      Parameter allParameter1 = (Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_MBusMeterType"];
      if (allParameter1.ValueEprom < 129L || allParameter1.ValueEprom > 131L)
      {
        ZR_ClassLibMessages.AddWarning("Meter generation not found! Set to 130 !!!");
        allParameter1.ValueEprom = 130L;
      }
      Parameter allParameter2 = (Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_MBusManufacturer"];
      if (allParameter2.ValueEprom == 0L)
      {
        ZR_ClassLibMessages.AddWarning("Manufacturer not found! Set to 'ZRM' !!!");
        allParameter2.ValueEprom = 27213L;
      }
      if (this.MyMeter.WriteEnable)
      {
        int index1 = this.MyMeter.MyFunctionTable.OverridesList.IndexOfKey((object) OverrideID.WarmerPipe);
        int index2 = this.MyMeter.MyFunctionTable.OverridesList.IndexOfKey((object) OverrideID.BaseConfig);
        if (index1 >= 0 && index2 >= 0)
        {
          OverrideParameter byIndex = (OverrideParameter) this.MyMeter.MyFunctionTable.OverridesList.GetByIndex(index1);
          OverrideParameter.BaseConfigStruct baseConfigStruct = OverrideParameter.GetBaseConfigStruct(((ConfigurationParameter) this.MyMeter.MyFunctionTable.OverridesList.GetByIndex(index2)).GetStringValueWin());
          if (baseConfigStruct != null)
          {
            Parameter allParameter3 = (Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_MBusMedium"];
            if (baseConfigStruct.HeatAndCooling)
              allParameter3.ValueEprom = 13L;
            else if (!baseConfigStruct.EnergyOff)
              allParameter3.ValueEprom = !baseConfigStruct.Cooling ? (byIndex.Value != 0UL ? 12L : 4L) : (byIndex.Value != 0UL ? 10L : 11L);
          }
        }
      }
      return true;
    }

    internal void ClearChecksumsAndAddresses()
    {
      ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_RamParamBlockAdr"]).ValueEprom = 0L;
      ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_BackupBlockAdr"]).ValueEprom = 0L;
      ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_FixedParamAdr"]).ValueEprom = 0L;
      ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_WritePermTableAdr"]).ValueEprom = 0L;
      ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_DispBlockAdr"]).ValueEprom = 0L;
      ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_RuntimeVarsAdr"]).ValueEprom = 0L;
      ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_RuntimeCodeAdr"]).ValueEprom = 0L;
      ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_EpromVarsAdr"]).ValueEprom = 0L;
      ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_ParamBlockAdr"]).ValueEprom = 0L;
      ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_EpromRuntimeAdr"]).ValueEprom = 0L;
      ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_MBusBlockAdr"]).ValueEprom = 0L;
      ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_FunctionTableAdr"]).ValueEprom = 0L;
      ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_StaticChecksum"]).ValueEprom = 0L;
      ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_HeaderChecksum"]).ValueEprom = 0L;
      ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_BackupChecksum"]).ValueEprom = 0L;
    }
  }
}
