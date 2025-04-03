// Decompiled with JetBrains decompiler
// Type: S3_Handler.TransmitParameterManager
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal sealed class TransmitParameterManager
  {
    private S3_HandlerFunctions MyFunctions;
    private S3_Meter MyMeter;

    internal DataTransmitter Transmitter { get; private set; }

    internal TransmitParameterManager(S3_HandlerFunctions MyFunctions, S3_Meter MyMeter)
    {
      this.MyFunctions = MyFunctions;
      this.MyMeter = MyMeter;
    }

    internal bool CreateMbusTableFromMemory()
    {
      this.Transmitter = new DataTransmitter(this.MyMeter, this.MyMeter.MyDeviceMemory.BlockMBusTable);
      return this.Transmitter.CreateMbusTableFromMemory();
    }

    internal bool Clone(S3_Meter theCloneMeter)
    {
      if (theCloneMeter == null || theCloneMeter.MyDeviceMemory == null || theCloneMeter.MyDeviceMemory.BlockMBusTable == null || this.Transmitter == null)
        return false;
      if (theCloneMeter.MyDeviceMemory.BlockMBusTable.childMemoryBlocks != null)
        theCloneMeter.MyDeviceMemory.BlockMBusTable.childMemoryBlocks.Clear();
      theCloneMeter.MyTransmitParameterManager = new TransmitParameterManager(this.MyFunctions, theCloneMeter);
      theCloneMeter.MyTransmitParameterManager.Transmitter = this.Transmitter.Clone(theCloneMeter, theCloneMeter.MyDeviceMemory.BlockMBusTable);
      return true;
    }

    internal bool SetMbusPrimAddressesFromSerialNumber()
    {
      uint uintValue = this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_SerialNumber.ToString()].GetUintValue();
      byte mbusAddress = (byte) (((int) uintValue & 15) + ((int) (uintValue >> 4) & 15) * 10);
      if (mbusAddress >= (byte) 50)
        mbusAddress -= (byte) 50;
      return (this.MyMeter.IsWriteProtected || this.SetMbusPrimAddress(S3_ParameterNames.Con_SelectedList_PrimaryAddress, mbusAddress)) && this.SetMbusPrimAddress(S3_ParameterNames.SerDev0_SelectedList_PrimaryAddress, mbusAddress) && (!this.MyMeter.MyIdentification.IsInput1Available || this.SetMbusPrimAddress(S3_ParameterNames.SerDev1_SelectedList_PrimaryAddress, (byte) ((uint) mbusAddress + 50U))) && (!this.MyMeter.MyIdentification.IsInput2Available || this.SetMbusPrimAddress(S3_ParameterNames.SerDev2_SelectedList_PrimaryAddress, (byte) ((uint) mbusAddress + 100U))) && (!this.MyMeter.MyIdentification.IsInput3Available || this.SetMbusPrimAddress(S3_ParameterNames.SerDev3_SelectedList_PrimaryAddress, (byte) ((uint) mbusAddress + 150U)));
    }

    internal bool IsMbusPrimAddressesFromSerialNumber()
    {
      uint uintValue = this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_SerialNumber.ToString()].GetUintValue();
      byte num = (byte) (((int) uintValue & 15) + ((int) (uintValue >> 4) & 15) * 10);
      if (num >= (byte) 50)
        num -= (byte) 50;
      return (!this.MyMeter.MyIdentification.IsInput1Available || (int) this.GetMbusPrimAddress(S3_ParameterNames.SerDev1_SelectedList_PrimaryAddress) == (int) (byte) ((uint) num + 50U)) && (!this.MyMeter.MyIdentification.IsInput2Available || (int) this.GetMbusPrimAddress(S3_ParameterNames.SerDev2_SelectedList_PrimaryAddress) == (int) (byte) ((uint) num + 100U)) && (!this.MyMeter.MyIdentification.IsInput3Available || (int) this.GetMbusPrimAddress(S3_ParameterNames.SerDev3_SelectedList_PrimaryAddress) == (int) (byte) ((uint) num + 150U));
    }

    internal bool AreVirtualDevicesUsed()
    {
      return this.MyMeter.MyIdentification.IsInput1Available && (int) this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.SerDev1_IdentNo.ToString()].GetUintValue() != (int) S3_DeviceIdentification.virtualDeviceOffSerialNumber[0];
    }

    internal bool SetMbusPrimAddress(
      S3_ParameterNames SelectedList_PrimaryAddress,
      byte mbusAddress)
    {
      S3_Parameter s3Parameter = this.MyMeter.MyParameters.ParameterByName[SelectedList_PrimaryAddress.ToString()];
      ushort NewValue = (ushort) ((uint) (ushort) ((uint) s3Parameter.GetUshortValue() & 65280U) + (uint) mbusAddress);
      return s3Parameter.SetUshortValue(NewValue);
    }

    internal byte GetMbusPrimAddress(S3_ParameterNames SelectedList_PrimaryAddress)
    {
      return (byte) this.MyMeter.MyParameters.ParameterByName[SelectedList_PrimaryAddress.ToString()].GetUshortValue();
    }

    internal bool RemoveByResources()
    {
      if (this.Transmitter.childMemoryBlocks == null || !this.RemoveByResources(this.Transmitter.P2P))
        return false;
      if (this.Transmitter.Heat.childMemoryBlocks != null)
      {
        for (int index = this.Transmitter.Heat.childMemoryBlocks.Count - 1; index >= 0; --index)
        {
          if (!this.RemoveByResources(this.Transmitter.Heat.childMemoryBlocks[index] as MBusList))
            return false;
        }
      }
      if (this.Transmitter.Input1 != null && this.Transmitter.Input1.childMemoryBlocks != null)
      {
        for (int index = this.Transmitter.Input1.childMemoryBlocks.Count - 1; index >= 0; --index)
        {
          if (!this.RemoveByResources(this.Transmitter.Input1.childMemoryBlocks[index] as MBusList))
            return false;
        }
      }
      if (this.Transmitter.Input2 != null && this.Transmitter.Input2.childMemoryBlocks != null)
      {
        for (int index = this.Transmitter.Input2.childMemoryBlocks.Count - 1; index >= 0; --index)
        {
          if (!this.RemoveByResources(this.Transmitter.Input2.childMemoryBlocks[index] as MBusList))
            return false;
        }
      }
      if (this.Transmitter.Input3 != null && this.Transmitter.Input3.childMemoryBlocks != null)
      {
        for (int index = this.Transmitter.Input3.childMemoryBlocks.Count - 1; index >= 0; --index)
        {
          if (!this.RemoveByResources(this.Transmitter.Input3.childMemoryBlocks[index] as MBusList))
            return false;
        }
      }
      if (this.Transmitter.Radio != null && this.Transmitter.Radio.childMemoryBlocks != null)
      {
        for (int index = this.Transmitter.Radio.childMemoryBlocks.Count - 1; index >= 0; --index)
        {
          if (!this.RemoveByResources(this.Transmitter.Radio.childMemoryBlocks[index]))
            return false;
        }
      }
      return true;
    }

    private bool RemoveByResources(MBusList mbusList)
    {
      if (mbusList.childMemoryBlocks == null)
        return true;
      for (int index = mbusList.childMemoryBlocks.Count - 1; index >= 0; --index)
      {
        S3_MemoryBlock childMemoryBlock = mbusList.childMemoryBlocks[index];
        switch (childMemoryBlock)
        {
          case MBusParameter _:
            MBusParameter block1 = (MBusParameter) childMemoryBlock;
            if (block1.IsLogger)
            {
              if (!string.IsNullOrEmpty(block1.Name) && !this.MyMeter.MyLinker.MyLabelAddresses.ContainsKey(block1.Name))
              {
                this.MyMeter.MyResources.ResourceEventDeleteObjectByMissedResource("Delete transmit parameter logger: " + block1.Name, string.Empty);
                this.Remove((S3_MemoryBlock) block1);
                break;
              }
              break;
            }
            if (!this.MyMeter.MyParameters.ParameterByName.ContainsKey(block1.Name))
            {
              this.MyMeter.MyResources.ResourceEventDeleteObjectByMissedResource("Delete transmit parameter: " + block1.Name, string.Empty);
              this.Remove((S3_MemoryBlock) block1);
              break;
            }
            string needResource = this.MyMeter.MyParameters.ParameterByName[block1.Name].Statics.NeedResource;
            if (!this.MyMeter.MyResources.IsResourceAvailable(needResource))
            {
              this.MyMeter.MyResources.ResourceEventDeleteObjectByMissedResource("Delete transmit parameter: " + block1.Name, needResource);
              this.Remove((S3_MemoryBlock) block1);
              break;
            }
            goto default;
          case ListLink _:
            ListLink block2 = (ListLink) childMemoryBlock;
            if (this.Transmitter.FindListByName((S3_MemoryBlock) this.Transmitter, block2.Name) == null && this.Remove((S3_MemoryBlock) block2))
            {
              this.MyMeter.MyResources.ResourceEventDeleteObjectByMissedResource("Delete link to transmit parameter list: " + block2.Name, string.Empty);
              break;
            }
            goto default;
          case MBusParameterGroup _:
            this.RemoveFromParameterGroup(childMemoryBlock as MBusParameterGroup);
            goto default;
        }
      }
      this.Transmitter.RemoveEmptyList();
      return true;
    }

    private bool RemoveByResources(S3_MemoryBlock memoryItem)
    {
      if (memoryItem == null)
        return false;
      if (memoryItem.childMemoryBlocks == null)
        return true;
      if (!this.MyMeter.MyLinker.MyMeter.MyResources.IsResourceAvailable("Radio"))
      {
        if (memoryItem is RadioListHeader)
          this.Remove((S3_MemoryBlock) (memoryItem as RadioListHeader));
        return true;
      }
      if (memoryItem is RadioList)
      {
        RadioList radioList = memoryItem as RadioList;
        for (int index = radioList.childMemoryBlocks.Count - 1; index >= 0; --index)
        {
          S3_MemoryBlock childMemoryBlock = radioList.childMemoryBlocks[index];
          if (childMemoryBlock is MBusParameter)
          {
            MBusParameter block = (MBusParameter) childMemoryBlock;
            if (block.IsLogger)
            {
              if (!string.IsNullOrEmpty(block.Name) && !this.MyMeter.MyLinker.MyLabelAddresses.ContainsKey(block.Name))
              {
                this.MyMeter.MyResources.ResourceEventDeleteObjectByMissedResource("Delete transmit parameter logger: " + block.Name, string.Empty);
                this.Remove((S3_MemoryBlock) block);
              }
            }
            else if (!this.MyMeter.MyParameters.ParameterByName.ContainsKey(block.Name))
            {
              this.MyMeter.MyResources.ResourceEventDeleteObjectByMissedResource("Delete transmit parameter: " + block.Name, string.Empty);
              this.Remove((S3_MemoryBlock) block);
            }
            else
            {
              string needResource = this.MyMeter.MyParameters.ParameterByName[block.Name].Statics.NeedResource;
              if (!this.MyMeter.MyResources.IsResourceAvailable(needResource))
              {
                this.MyMeter.MyResources.ResourceEventDeleteObjectByMissedResource("Delete transmit parameter: " + block.Name, needResource);
                this.Remove((S3_MemoryBlock) block);
              }
            }
          }
          else if (childMemoryBlock is MBusParameterGroup)
            this.RemoveFromParameterGroup(childMemoryBlock as MBusParameterGroup);
        }
      }
      this.Transmitter.RemoveEmptyList();
      return true;
    }

    private void RemoveFromParameterGroup(MBusParameterGroup paramGroup)
    {
      if (paramGroup.childMemoryBlocks == null)
        return;
      for (int index = paramGroup.childMemoryBlocks.Count - 1; index >= 0; --index)
      {
        if (paramGroup.childMemoryBlocks[index] is MBusParameter)
        {
          MBusParameter childMemoryBlock1 = (MBusParameter) paramGroup.childMemoryBlocks[index];
          if (childMemoryBlock1.IsLogger)
          {
            if (!string.IsNullOrEmpty(childMemoryBlock1.Name))
            {
              if (!this.MyMeter.MyLinker.MyLabelAddresses.ContainsKey(childMemoryBlock1.Name))
              {
                this.MyMeter.MyResources.ResourceEventDeleteObjectByMissedResource("Delete transmit parameter logger: " + childMemoryBlock1.Name, string.Empty);
                this.Remove((S3_MemoryBlock) childMemoryBlock1);
              }
              if (index == 0 && paramGroup.childMemoryBlocks.Count > 0)
              {
                if ((paramGroup.childMemoryBlocks.Count & 1) != 0)
                  throw new Exception("Count of logger parameters not ok");
                (paramGroup.childMemoryBlocks[0] as MBusParameter).ControlWord0.ControlLogger = ControlLogger.LoggerReset;
                MBusParameter childMemoryBlock2 = paramGroup.childMemoryBlocks[paramGroup.childMemoryBlocks.Count / 2] as MBusParameter;
                childMemoryBlock2.ControlWord0.ItFollowsNextControlWord = true;
                childMemoryBlock2.ControlWord0.ControlLogger = ControlLogger.LoggerNextChangeChanal;
                if (childMemoryBlock2.ControlWord1 == null)
                {
                  childMemoryBlock2.ControlWord1 = new ControlWord1();
                  childMemoryBlock2.ByteSize += 2;
                  childMemoryBlock2.ControlWord1.ItFollowsNextControlWord = true;
                }
                if (childMemoryBlock2.ControlWord2 == null)
                {
                  childMemoryBlock2.ControlWord2 = new ControlWord2();
                  childMemoryBlock2.ByteSize += 2;
                }
                childMemoryBlock2.ControlWord2.IsSavePtr = true;
                MBusParameter childMemoryBlock3 = paramGroup.childMemoryBlocks[paramGroup.childMemoryBlocks.Count - 1] as MBusParameter;
                childMemoryBlock3.ControlWord0.ItFollowsNextControlWord = true;
                childMemoryBlock3.ControlWord0.ControlLogger = ControlLogger.LoggerChangeChanal;
                if (childMemoryBlock3.ControlWord1 == null)
                {
                  childMemoryBlock3.ControlWord1 = new ControlWord1();
                  childMemoryBlock3.ByteSize += 2;
                }
                childMemoryBlock3.ControlWord1.ItFollowsNextControlWord = true;
                childMemoryBlock3.ControlWord1.LoggerCycleCount = paramGroup.LoggerCycleCount;
                if (childMemoryBlock3.ControlWord2 == null)
                {
                  childMemoryBlock3.ControlWord2 = new ControlWord2();
                  childMemoryBlock3.ByteSize += 2;
                }
                childMemoryBlock3.ControlWord2.ListMaxCount = paramGroup.ListMaxCount;
              }
            }
          }
          else if (!this.MyMeter.MyParameters.ParameterByName.ContainsKey(childMemoryBlock1.Name))
          {
            this.MyMeter.MyResources.ResourceEventDeleteObjectByMissedResource("Delete transmit parameter: " + childMemoryBlock1.Name, string.Empty);
            this.Remove((S3_MemoryBlock) childMemoryBlock1);
          }
          else
          {
            string needResource = this.MyMeter.MyParameters.ParameterByName[childMemoryBlock1.Name].Statics.NeedResource;
            if (!this.MyMeter.MyResources.IsResourceAvailable(needResource))
            {
              this.MyMeter.MyResources.ResourceEventDeleteObjectByMissedResource("Delete transmit parameter: " + childMemoryBlock1.Name, needResource);
              this.Remove((S3_MemoryBlock) childMemoryBlock1);
            }
          }
        }
      }
      if (paramGroup.childMemoryBlocks.Count == 0)
        this.Remove((S3_MemoryBlock) paramGroup);
    }

    private void RemoveTarif1fromDIV(List<S3_MemoryBlock> blockList, bool isLogger = false)
    {
      if (blockList == null)
        return;
      foreach (S3_MemoryBlock block in blockList)
      {
        switch (block)
        {
          case MBusParameter _:
            ((MBusParameter) block).RemoveTarif1fromDIV(isLogger);
            break;
          case MBusParameterGroup _:
            isLogger = true;
            break;
        }
        this.RemoveTarif1fromDIV(block.childMemoryBlocks, isLogger);
      }
    }

    internal bool LinkPointer()
    {
      try
      {
        return this.Transmitter.LinkPointer();
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Failed link pointer! " + ZR_Constants.SystemNewLine + ex.Message);
        return false;
      }
    }

    internal bool Remove(S3_MemoryBlock block) => this.Transmitter.Remove(block);

    internal bool GetMBusThirdPartySupportState()
    {
      return this.MyMeter.MyIdentification.FirmwareVersion > 67309573U || this.Transmitter.P2P.childMemoryBlocks.Find((Predicate<S3_MemoryBlock>) (e => e is MBusParameter)) != null && this.GetMBusThirdPartySupportState(this.Transmitter.Heat) && this.GetMBusThirdPartySupportState(this.Transmitter.Input1) && this.GetMBusThirdPartySupportState(this.Transmitter.Input2) && this.GetMBusThirdPartySupportState(this.Transmitter.Input3);
    }

    private bool GetMBusThirdPartySupportState(MBusTransmitter mBusTransmitter)
    {
      if (mBusTransmitter == null || mBusTransmitter.childMemoryBlocks == null)
        return true;
      S3_MemoryBlock s3MemoryBlock = mBusTransmitter.childMemoryBlocks.Find((Predicate<S3_MemoryBlock>) (e => e is MBusList && ((MBusList) e).IsSelected));
      if (s3MemoryBlock == null || s3MemoryBlock.childMemoryBlocks == null)
        return false;
      List<S3_MemoryBlock> all = s3MemoryBlock.childMemoryBlocks.FindAll((Predicate<S3_MemoryBlock>) (e => e is MBusParameter));
      return all != null && all.Count == 1 && !(((MBusParameter) all[0]).Name != "Con_MeterId");
    }

    internal bool SetMBusThirdPartySupportState(bool enable)
    {
      if (this.MyMeter.MyIdentification.FirmwareVersion != 67309573U)
        return false;
      return enable ? this.EnableMBusThirdPartySupport() : this.DisableMBusThirdPartySupport();
    }

    private bool DisableMBusThirdPartySupport()
    {
      S3_MemoryBlock block = this.Transmitter.P2P.childMemoryBlocks.Find((Predicate<S3_MemoryBlock>) (e => e is MBusParameter));
      return (block == null || !(((MBusParameter) block).Name == "Con_MeterId") || this.Transmitter.Remove(block)) && this.DisableMBusThirdPartySupport(this.Transmitter.Heat) && this.DisableMBusThirdPartySupport(this.Transmitter.Input1) && this.DisableMBusThirdPartySupport(this.Transmitter.Input2) && this.DisableMBusThirdPartySupport(this.Transmitter.Input3);
    }

    private bool DisableMBusThirdPartySupport(MBusTransmitter mBusTransmitter)
    {
      if (mBusTransmitter == null || mBusTransmitter.childMemoryBlocks == null)
        return true;
      S3_MemoryBlock block = mBusTransmitter.childMemoryBlocks.Find((Predicate<S3_MemoryBlock>) (e => e is MBusList && ((MBusList) e).IsSelected));
      if (block != null && block.childMemoryBlocks != null)
      {
        List<S3_MemoryBlock> all = block.childMemoryBlocks.FindAll((Predicate<S3_MemoryBlock>) (e => e is MBusParameter));
        if (all != null && all.Count == 1 && ((MBusParameter) all[0]).Name != "Con_MeterId")
          return this.Transmitter.Remove(block);
      }
      return true;
    }

    private bool EnableMBusThirdPartySupport()
    {
      if (this.Transmitter.P2P.childMemoryBlocks.Find((Predicate<S3_MemoryBlock>) (e => e is MBusParameter)) == null)
        this.Transmitter.P2P.AddParameter("Con_MeterId");
      return this.EnableMBusThirdPartySupport(this.Transmitter.Heat) && this.EnableMBusThirdPartySupport(this.Transmitter.Input1) && this.EnableMBusThirdPartySupport(this.Transmitter.Input2) && this.EnableMBusThirdPartySupport(this.Transmitter.Input3);
    }

    private bool EnableMBusThirdPartySupport(MBusTransmitter mBusTransmitter)
    {
      if (mBusTransmitter.childMemoryBlocks != null)
      {
        S3_MemoryBlock s3MemoryBlock = mBusTransmitter.childMemoryBlocks.Find((Predicate<S3_MemoryBlock>) (e => e is MBusList && ((MBusList) e).IsSelected));
        if (s3MemoryBlock != null && s3MemoryBlock.childMemoryBlocks != null)
        {
          MBusList mbusList = s3MemoryBlock as MBusList;
          MBusList list = mBusTransmitter.InsertNewMBusList(0);
          if (list == null || list.AddParameter("Con_MeterId") == null || mbusList.AddLink(list.Name) == null)
            return false;
          foreach (string linkName in mbusList.LinkNames)
          {
            if (list.AddLink(linkName) == null)
              return false;
          }
          mBusTransmitter.SetAsActive(list);
        }
      }
      return true;
    }
  }
}
