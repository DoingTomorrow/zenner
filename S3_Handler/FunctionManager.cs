// Decompiled with JetBrains decompiler
// Type: S3_Handler.FunctionManager
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal sealed class FunctionManager
  {
    private S3_HandlerFunctions MyFunctions;
    private S3_Meter MyMeter;
    internal static Logger logger = LogManager.GetLogger("FunctionTableManager");
    private Dictionary<ushort, string> pointer;
    internal S3_FunctionTable MyFunctionTable;
    private bool mainIsDefined;

    public FunctionManager(S3_HandlerFunctions MyFunctions, S3_Meter MyMeter)
    {
      this.MyFunctions = MyFunctions;
      this.MyMeter = MyMeter;
      this.pointer = new Dictionary<ushort, string>();
    }

    internal bool CreateStructureObjects()
    {
      this.MyFunctionTable = new S3_FunctionTable(this.MyMeter, this.MyMeter.MyDeviceMemory.BlockFunctionTable);
      if (!this.MyFunctionTable.CreateStructureObjectsFromMemory())
        return false;
      if (this.MyMeter.MyDeviceMemory.BlockDisplayCode.BlockStartAddress == 0)
        this.MyMeter.MyDeviceMemory.BlockDisplayCode.BlockStartAddress = this.MyMeter.MyDeviceMemory.BlockFunctionTable.StartAddressOfNextBlock;
      return this.CreateStructureObjectsOfFunctions();
    }

    private bool CreateStructureObjectsOfFunctions()
    {
      this.ResetMemoryBlocksOfFunctions(this.MyMeter);
      this.MyMeter.MyDeviceMemory.BlockHeap.BlockStartAddress = this.MyMeter.MyDeviceMemory.BlockLoggerRamData.StartAddressOfNextBlock;
      foreach (S3_FunctionLayer childMemoryBlock1 in this.MyMeter.MyFunctionManager.MyFunctionTable.childMemoryBlocks)
      {
        if (childMemoryBlock1.childMemoryBlocks != null)
        {
          foreach (S3_Function childMemoryBlock2 in childMemoryBlock1.childMemoryBlocks)
          {
            if (!childMemoryBlock2.CreateStructureObjectsOfFunction(childMemoryBlock1.LayerNr))
              return false;
          }
        }
        else
          break;
      }
      if (this.MyMeter.MyDeviceMemory.BlockResetRuntimeCode.ByteSize > 0)
      {
        S3_DataBlock s3DataBlock1 = new S3_DataBlock(new byte[1], this.MyMeter, this.MyMeter.MyDeviceMemory.BlockResetRuntimeCode);
      }
      if (this.MyMeter.MyDeviceMemory.BlockCycleRuntimeCode.ByteSize > 0)
      {
        S3_DataBlock s3DataBlock2 = new S3_DataBlock(new byte[1], this.MyMeter, this.MyMeter.MyDeviceMemory.BlockCycleRuntimeCode);
      }
      if (this.MyMeter.MyDeviceMemory.BlockMesurementRuntimeCode.ByteSize > 0)
      {
        S3_DataBlock s3DataBlock3 = new S3_DataBlock(new byte[1], this.MyMeter, this.MyMeter.MyDeviceMemory.BlockMesurementRuntimeCode);
      }
      if (this.MyMeter.MyDeviceMemory.BlockMBusRuntimeCode.ByteSize > 0)
      {
        S3_DataBlock s3DataBlock4 = new S3_DataBlock(new byte[1], this.MyMeter, this.MyMeter.MyDeviceMemory.BlockMBusRuntimeCode);
      }
      this.RemoveEmptyLayers();
      foreach (S3_FunctionLayer childMemoryBlock3 in this.MyMeter.MyFunctionManager.MyFunctionTable.childMemoryBlocks)
      {
        if (childMemoryBlock3.childMemoryBlocks != null)
        {
          foreach (S3_Function childMemoryBlock4 in childMemoryBlock3.childMemoryBlocks)
          {
            if (childMemoryBlock4.DisplayCode != null && !childMemoryBlock4.AdjustBrunchObjects())
              return false;
          }
        }
        else
          break;
      }
      this.MyMeter.MyLinker.Link(this.MyMeter.MyDeviceMemory.BlockRAM);
      this.MyMeter.MyLinker.Link(this.MyMeter.MyDeviceMemory.BlockProtectedDisplayCode);
      this.MyMeter.MyLinker.Link(this.MyMeter.MyDeviceMemory.BlockFlashBlock1);
      this.MyMeter.MyParameters.RecreateParameterByAddress();
      return true;
    }

    internal bool InsertData() => this.MyFunctionTable.InsertData() && this.InsertDataOfFunction();

    internal bool InsertDataOfFunction()
    {
      this.mainIsDefined = false;
      this.pointer.Clear();
      foreach (S3_MemoryBlock childMemoryBlock in this.MyMeter.MyDeviceMemory.BlockProtectedDisplayCode.childMemoryBlocks)
      {
        if (!this.InsertDataInBlock(childMemoryBlock))
          return false;
      }
      if (this.MyMeter.MyDeviceMemory.BlockDisplayCode.childMemoryBlocks != null)
      {
        foreach (S3_MemoryBlock childMemoryBlock in this.MyMeter.MyDeviceMemory.BlockDisplayCode.childMemoryBlocks)
        {
          if (!this.InsertDataInBlock(childMemoryBlock))
            return false;
        }
      }
      return this.InsertDataInBlock(this.MyMeter.MyDeviceMemory.BlockResetRuntimeCode) && this.InsertDataInBlock(this.MyMeter.MyDeviceMemory.BlockCycleRuntimeCode) && this.InsertDataInBlock(this.MyMeter.MyDeviceMemory.BlockMesurementRuntimeCode) && this.InsertDataInBlock(this.MyMeter.MyDeviceMemory.BlockMBusRuntimeCode);
    }

    private bool InsertDataInBlock(S3_MemoryBlock parentBlock)
    {
      if (parentBlock.childMemoryBlocks == null)
        return true;
      foreach (S3_MemoryBlock childMemoryBlock in parentBlock.childMemoryBlocks)
      {
        switch (childMemoryBlock)
        {
          case S3_Pointer _:
            this.pointer.Add((ushort) childMemoryBlock.BlockStartAddress, ((S3_Pointer) childMemoryBlock).PointerName);
            break;
          case S3_DisplayMenu _:
            S3_DisplayMenu s3DisplayMenu = (S3_DisplayMenu) childMemoryBlock;
            if (!s3DisplayMenu.InsertData())
              return false;
            this.MyMeter.MyLinker.AddLabel(s3DisplayMenu.MenuName, childMemoryBlock.BlockStartAddress);
            if (!this.mainIsDefined && s3DisplayMenu.My_S3_Function.LayerNr == 1)
            {
              this.MyMeter.MyLinker.AddLabel("MAIN", childMemoryBlock.BlockStartAddress);
              this.mainIsDefined = true;
              break;
            }
            break;
          case S3_DataBlock _:
            if (!((S3_DataBlock) childMemoryBlock).InsertData())
              return false;
            break;
          case S3_DisplayMenuBrunches _:
            if (!((S3_DisplayMenuBrunches) childMemoryBlock).InsertData())
              return false;
            break;
          case S3_RuntimeCode _:
            if (!((S3_RuntimeCode) childMemoryBlock).InsertData())
              return false;
            break;
        }
      }
      return true;
    }

    private byte[] CreateFunctionTable(int address)
    {
      this.MyFunctionTable.BlockStartAddress = address;
      int num = address;
      List<byte> byteList = new List<byte>();
      foreach (S3_FunctionLayer childMemoryBlock1 in this.MyFunctionTable.childMemoryBlocks)
      {
        if (childMemoryBlock1.childMemoryBlocks.Count != 0)
        {
          byteList.Add((byte) childMemoryBlock1.childMemoryBlocks.Count);
          byteList.Add((byte) childMemoryBlock1.LayerNr);
          num += 2;
          foreach (S3_Function childMemoryBlock2 in childMemoryBlock1.childMemoryBlocks)
          {
            num += 2;
            if (childMemoryBlock2.IsProtected)
              byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes((ushort) ((uint) childMemoryBlock2.FunctionNumber + 32768U)));
            else
              byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(childMemoryBlock2.FunctionNumber));
          }
        }
        else
          break;
      }
      byteList.Add((byte) 0);
      byteList.Add((byte) 0);
      this.MyFunctionTable.ByteSize = byteList.Count;
      return byteList.ToArray();
    }

    internal bool Clone(S3_Meter theCloneMeter)
    {
      this.ResetMemoryBlocksOfFunctions(theCloneMeter);
      this.MyMeter.MyBackupRuntimeVarsManager.Clone(theCloneMeter);
      this.MyMeter.MyHeapManager.Clone(theCloneMeter);
      theCloneMeter.MyFunctionManager = new FunctionManager(this.MyFunctions, theCloneMeter);
      if (this.MyFunctionTable != null)
        theCloneMeter.MyFunctionManager.MyFunctionTable = this.MyFunctionTable.Clone(theCloneMeter);
      return true;
    }

    internal bool RemoveByResources()
    {
      for (int index1 = this.MyFunctionTable.childMemoryBlocks.Count - 2; index1 >= 0; --index1)
      {
        S3_FunctionLayer childMemoryBlock1 = (S3_FunctionLayer) this.MyFunctionTable.childMemoryBlocks[index1];
        if (childMemoryBlock1.childMemoryBlocks == null || childMemoryBlock1.childMemoryBlocks.Count == 0)
        {
          childMemoryBlock1.RemoveFromParentMemoryBlock();
        }
        else
        {
          for (int index2 = childMemoryBlock1.childMemoryBlocks.Count - 1; index2 >= 0; --index2)
          {
            S3_Function childMemoryBlock2 = (S3_Function) childMemoryBlock1.childMemoryBlocks[index2];
            if ((long) this.MyMeter.MyIdentification.FirmwareVersion > (long) childMemoryBlock2.DatabaseData.FirmwareVersionMax || (long) this.MyMeter.MyIdentification.FirmwareVersion < (long) childMemoryBlock2.DatabaseData.FirmwareVersionMin)
            {
              this.MyMeter.MyResources.ResourceEventDeleteObjectByMissedResource("Delete function: " + childMemoryBlock2.DatabaseData?.ToString(), "Not supported for current Firmware");
              if (!this.RemoveFunction(childMemoryBlock2.FunctionNumber))
                return false;
            }
            else if (childMemoryBlock2.PredefinedRemoveFunctionRequest)
            {
              this.MyMeter.MyResources.ResourceEventDeleteObjectByMissedResource("Delete function: " + childMemoryBlock2.DatabaseData?.ToString(), "PredefinedRemoveRequest");
              if (!this.RemoveFunction(childMemoryBlock2.FunctionNumber))
                return false;
            }
            else
            {
              string missedResource;
              if (!this.MyMeter.MyResources.AreAllResourcesAvailable(childMemoryBlock2.DatabaseData.NeedResources, out missedResource))
              {
                this.MyMeter.MyResources.ResourceEventDeleteObjectByMissedResource("Delete function: " + childMemoryBlock2.DatabaseData?.ToString(), missedResource);
                if (!this.RemoveFunction(childMemoryBlock2.FunctionNumber))
                  return false;
              }
              else
              {
                foreach (FunctionPrecompiled functionPrecompiled in childMemoryBlock2.DatabaseData.Precompiled)
                {
                  if (functionPrecompiled.RecordType == FunctionRecordType.Pointer)
                  {
                    int length = functionPrecompiled.Name.IndexOf('.');
                    if ((length < 0 || !(functionPrecompiled.Name.Substring(0, length) == childMemoryBlock2.Name)) && functionPrecompiled.Name != "MAIN" && functionPrecompiled.Name != "NEXT" && functionPrecompiled.Name != "RIGHT" && functionPrecompiled.Name != "FIRST" && !this.MyMeter.MyLinker.MyLabelAddresses.ContainsKey(functionPrecompiled.Name))
                    {
                      ZR_ClassLibMessages.AddWarning("The LCD function is deleted: " + childMemoryBlock2.Name + "; Label not found: " + functionPrecompiled.Name);
                      this.MyMeter.MyResources.ResourceEventDeleteObjectByMissedResource("Delete function: " + childMemoryBlock2.DatabaseData?.ToString() + ". Invalid pointer detected: ", functionPrecompiled.Name);
                      if (!this.RemoveFunction(childMemoryBlock2.FunctionNumber))
                        return false;
                      break;
                    }
                  }
                }
              }
            }
          }
        }
      }
      return true;
    }

    public S3_Function GetNextFunction(ushort functionNumber)
    {
      foreach (S3_FunctionLayer childMemoryBlock1 in this.MyFunctionTable.childMemoryBlocks)
      {
        if (childMemoryBlock1.childMemoryBlocks == null)
          return (S3_Function) null;
        S3_Function s3Function = (S3_Function) null;
        foreach (S3_Function childMemoryBlock2 in childMemoryBlock1.childMemoryBlocks)
        {
          if ((int) childMemoryBlock2.FunctionNumber == (int) functionNumber)
          {
            s3Function = childMemoryBlock2;
            break;
          }
        }
        if (s3Function != null)
        {
          int num1 = childMemoryBlock1.childMemoryBlocks.IndexOf((S3_MemoryBlock) s3Function);
          if (num1 + 1 <= childMemoryBlock1.childMemoryBlocks.Count - 1)
            return (S3_Function) childMemoryBlock1.childMemoryBlocks[num1 + 1];
          int num2 = this.MyFunctionTable.childMemoryBlocks.IndexOf((S3_MemoryBlock) childMemoryBlock1);
          if (this.MyFunctionTable.childMemoryBlocks.Count > num2 + 1)
          {
            S3_MemoryBlock childMemoryBlock3 = this.MyFunctionTable.childMemoryBlocks[num2 + 1];
            if (childMemoryBlock3.childMemoryBlocks != null && childMemoryBlock3.childMemoryBlocks.Count > 0)
              return (S3_Function) childMemoryBlock3.childMemoryBlocks[0];
          }
        }
      }
      return (S3_Function) null;
    }

    public S3_Function GetPrevFunction(ushort functionNumber)
    {
      S3_Function prevFunction = (S3_Function) null;
      foreach (S3_FunctionLayer childMemoryBlock1 in this.MyFunctionTable.childMemoryBlocks)
      {
        if (childMemoryBlock1.childMemoryBlocks != null)
        {
          S3_Function s3Function = (S3_Function) null;
          foreach (S3_Function childMemoryBlock2 in childMemoryBlock1.childMemoryBlocks)
          {
            if ((int) childMemoryBlock2.FunctionNumber == (int) functionNumber)
            {
              s3Function = childMemoryBlock2;
              break;
            }
          }
          if (s3Function != null)
          {
            int num1 = childMemoryBlock1.childMemoryBlocks.IndexOf((S3_MemoryBlock) s3Function);
            if (num1 > 0)
              return (S3_Function) childMemoryBlock1.childMemoryBlocks[num1 - 1];
            int num2 = this.MyFunctionTable.childMemoryBlocks.IndexOf((S3_MemoryBlock) childMemoryBlock1);
            if (num2 > 0)
            {
              S3_MemoryBlock childMemoryBlock3 = this.MyFunctionTable.childMemoryBlocks[num2 - 1];
              if (childMemoryBlock3.childMemoryBlocks.Count > 0)
                return (S3_Function) childMemoryBlock3.childMemoryBlocks[childMemoryBlock3.childMemoryBlocks.Count - 1];
            }
          }
        }
      }
      return prevFunction;
    }

    public S3_Function GetFunction(ushort functionNumber)
    {
      foreach (S3_FunctionLayer childMemoryBlock1 in this.MyFunctionTable.childMemoryBlocks)
      {
        if (childMemoryBlock1.childMemoryBlocks != null)
        {
          foreach (S3_Function childMemoryBlock2 in childMemoryBlock1.childMemoryBlocks)
          {
            if ((int) childMemoryBlock2.FunctionNumber == (int) functionNumber)
              return childMemoryBlock2;
          }
        }
      }
      return (S3_Function) null;
    }

    internal SortedList<ushort, S3_Function> GetAllFunctions()
    {
      SortedList<ushort, S3_Function> allFunctions = new SortedList<ushort, S3_Function>();
      foreach (S3_FunctionLayer childMemoryBlock1 in this.MyFunctionTable.childMemoryBlocks)
      {
        if (childMemoryBlock1.childMemoryBlocks != null)
        {
          foreach (S3_Function childMemoryBlock2 in childMemoryBlock1.childMemoryBlocks)
            allFunctions.Add(childMemoryBlock2.FunctionNumber, childMemoryBlock2);
        }
      }
      return allFunctions;
    }

    public int? GetLayerNumber(ushort functionNumber)
    {
      foreach (S3_FunctionLayer childMemoryBlock1 in this.MyFunctionTable.childMemoryBlocks)
      {
        foreach (S3_Function childMemoryBlock2 in childMemoryBlock1.childMemoryBlocks)
        {
          if ((int) childMemoryBlock2.FunctionNumber == (int) functionNumber)
            return new int?(childMemoryBlock1.LayerNr);
        }
      }
      return new int?();
    }

    internal bool SetAsProtected(ushort functionNumber)
    {
      S3_Function prevFunction = this.GetPrevFunction(functionNumber);
      if (prevFunction != null && !prevFunction.IsProtected)
        return false;
      S3_Function function = this.GetFunction(functionNumber);
      if (function == null)
        return false;
      function.IsProtected = true;
      return true;
    }

    internal bool SetAsNotProtected(ushort functionNumber)
    {
      S3_Function nextFunction = this.GetNextFunction(functionNumber);
      if (nextFunction != null && nextFunction.IsProtected)
        return false;
      S3_Function function = this.GetFunction(functionNumber);
      if (function == null)
        return false;
      function.IsProtected = false;
      return true;
    }

    private FunctionPrecompiled FindEventTarget(List<FunctionPrecompiled> rows, string eventTarget)
    {
      foreach (FunctionPrecompiled row in rows)
      {
        if (row.RecordType == FunctionRecordType.DisplayCode && row.Name == eventTarget)
          return row;
      }
      return (FunctionPrecompiled) null;
    }

    private List<FunctionPrecompiled> GetSortedEventList(
      List<FunctionPrecompiled> rows,
      int startIndex)
    {
      List<FunctionPrecompiled> functionPrecompiledList = new List<FunctionPrecompiled>();
      for (int index = startIndex; index < rows.Count; ++index)
      {
        FunctionRecordType recordType = rows[index].RecordType;
        int num;
        switch (recordType)
        {
          case FunctionRecordType.Event_Click:
          case FunctionRecordType.Event_Press:
          case FunctionRecordType.Event_Hold:
            num = 1;
            break;
          default:
            num = recordType == FunctionRecordType.Event_Timeout ? 1 : 0;
            break;
        }
        if (num != 0)
          functionPrecompiledList.Add(rows[index]);
        else
          break;
      }
      List<FunctionPrecompiled> sortedEventList = new List<FunctionPrecompiled>();
      FunctionPrecompiled functionPrecompiled1 = functionPrecompiledList.Find((Predicate<FunctionPrecompiled>) (e => e.RecordType == FunctionRecordType.Event_Click));
      if (functionPrecompiled1 != null)
        sortedEventList.Add(functionPrecompiled1);
      FunctionPrecompiled functionPrecompiled2 = functionPrecompiledList.Find((Predicate<FunctionPrecompiled>) (e => e.RecordType == FunctionRecordType.Event_Press));
      if (functionPrecompiled2 != null)
        sortedEventList.Add(functionPrecompiled2);
      FunctionPrecompiled functionPrecompiled3 = functionPrecompiledList.Find((Predicate<FunctionPrecompiled>) (e => e.RecordType == FunctionRecordType.Event_Hold));
      if (functionPrecompiled3 != null)
        sortedEventList.Add(functionPrecompiled3);
      FunctionPrecompiled functionPrecompiled4 = functionPrecompiledList.Find((Predicate<FunctionPrecompiled>) (e => e.RecordType == FunctionRecordType.Event_Timeout));
      if (functionPrecompiled4 != null)
        sortedEventList.Add(functionPrecompiled4);
      return sortedEventList;
    }

    internal S3_FunctionLayer AddLayer() => this.MyFunctionTable.AddLayer();

    internal S3_Function AddFunction(int layerNr, Function f, int pos)
    {
      if (this.GetFunction((ushort) f.FunctionNumber) != null || this.MyFunctionTable.childMemoryBlocks == null || layerNr == 0 || this.MyFunctionTable.childMemoryBlocks[layerNr - 1].childMemoryBlocks == null || this.MyFunctionTable.childMemoryBlocks[layerNr - 1].childMemoryBlocks.Count == 0)
        return (S3_Function) null;
      if (layerNr == 1 && pos == 0)
      {
        FunctionPrecompiled functionPrecompiled = f.Precompiled.Find((Predicate<FunctionPrecompiled>) (e => e.RecordType == FunctionRecordType.Event_Timeout));
        if (functionPrecompiled != null)
        {
          int num = f.Precompiled.IndexOf(functionPrecompiled);
          int index1 = f.Precompiled.FindIndex((Predicate<FunctionPrecompiled>) (e => e.Codes != null && e.Codes.Length != 0));
          if (index1 != -1 && index1 + 1 < f.Precompiled.Count - 1)
          {
            int index2 = f.Precompiled.FindIndex(index1 + 1, (Predicate<FunctionPrecompiled>) (e => e.Codes != null && e.Codes.Length != 0));
            if (index2 == -1)
            {
              if (index1 < num)
              {
                ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.OperationCancelled, "The function '" + f.FunctionName + "' can not use at first position in first layer!", FunctionManager.logger);
                return (S3_Function) null;
              }
            }
            else if (index1 < num && index2 > num)
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.OperationCancelled, "The function '" + f.FunctionName + "' can not use at first position in first layer!", FunctionManager.logger);
              return (S3_Function) null;
            }
          }
        }
      }
      S3_Function s3Function = this.MyFunctionTable.AddFunction(layerNr, f, pos);
      if (s3Function == null)
        return (S3_Function) null;
      if (this.ReCreateMemoryBlocks())
        return s3Function;
      this.RemoveFunction(s3Function.FunctionNumber);
      s3Function.RemoveFromParentMemoryBlock();
      return (S3_Function) null;
    }

    internal bool RemoveFunction(ushort functionNumber)
    {
      S3_Function function = this.GetFunction(functionNumber);
      if (function == null || !(function.parentMemoryBlock is S3_FunctionLayer parentMemoryBlock) || function.LayerNr == 0)
        return false;
      if (function.LayerNr == 1 && parentMemoryBlock.NumberOfFunctions == 1)
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.OperationCancelled, "Can not remove '" + function.Name + "' function. First layer must have at least one function.", FunctionManager.logger);
      if (function.DatabaseData.SetResources != null)
      {
        foreach (string setResource in function.DatabaseData.SetResources)
        {
          bool flag = true;
          foreach (S3_FunctionLayer childMemoryBlock1 in this.MyFunctionTable.childMemoryBlocks)
          {
            if (childMemoryBlock1.childMemoryBlocks != null)
            {
              foreach (S3_Function childMemoryBlock2 in childMemoryBlock1.childMemoryBlocks)
              {
                if ((int) function.FunctionNumber != (int) childMemoryBlock2.FunctionNumber && childMemoryBlock2.DatabaseData.SetResources != null && childMemoryBlock2.DatabaseData.SetResources.Contains(setResource))
                {
                  flag = false;
                  break;
                }
              }
              if (!flag)
                break;
            }
          }
          if (flag && this.MyMeter.MyResources.IsResourceAvailable(setResource))
            this.MyMeter.MyResources.DeleteResource(setResource);
        }
      }
      if (function.CycleRuntimeCode != null)
        function.CycleRuntimeCode.RemoveFromParentMemoryBlock();
      if (function.DisplayCode != null)
        function.DisplayCode.RemoveFromParentMemoryBlock();
      if (function.MBusRuntimeCode != null)
        function.MBusRuntimeCode.RemoveFromParentMemoryBlock();
      if (function.MesurementRuntimeCode != null)
        function.MesurementRuntimeCode.RemoveFromParentMemoryBlock();
      if (function.ResetRuntimeCode != null)
        function.ResetRuntimeCode.RemoveFromParentMemoryBlock();
      if (function.RuntimeVars != null)
      {
        foreach (KeyValuePair<string, S3_Parameter> runtimeVar in function.RuntimeVars)
        {
          List<S3_Function> usedHeapParameter = this.GetFunctionListUsedHeapParameter(runtimeVar.Key);
          if (usedHeapParameter != null && usedHeapParameter.Exists((Predicate<S3_Function>) (e => (int) e.FunctionNumber == (int) functionNumber)))
            usedHeapParameter.Remove(function);
          if (usedHeapParameter == null || usedHeapParameter.Count == 0)
          {
            S3_Parameter s3Parameter = runtimeVar.Value;
            this.MyMeter.MyParameters.ParameterByName.Remove(s3Parameter.Name);
            this.MyMeter.MyParameters.ParameterByAddress.Remove(s3Parameter.BlockStartAddress);
            runtimeVar.Value.RemoveFromParentMemoryBlock();
          }
        }
        function.RuntimeVars.Clear();
      }
      function.RemoveFromParentMemoryBlock();
      --parentMemoryBlock.NumberOfFunctions;
      if (parentMemoryBlock.NumberOfFunctions == 0)
      {
        parentMemoryBlock.RemoveFromParentMemoryBlock();
        for (int index = 0; index < this.MyFunctionTable.childMemoryBlocks.Count; ++index)
          (this.MyFunctionTable.childMemoryBlocks[index] as S3_FunctionLayer).LayerNr = index;
      }
      return true;
    }

    private List<S3_Function> GetFunctionListUsedHeapParameter(string heapParameterName)
    {
      List<S3_Function> usedHeapParameter = new List<S3_Function>();
      foreach (S3_FunctionLayer childMemoryBlock1 in this.MyFunctionTable.childMemoryBlocks)
      {
        if (childMemoryBlock1.childMemoryBlocks != null)
        {
          foreach (S3_Function childMemoryBlock2 in childMemoryBlock1.childMemoryBlocks)
          {
            if (childMemoryBlock2.RuntimeVars != null && childMemoryBlock2.RuntimeVars.ContainsKey(heapParameterName))
              usedHeapParameter.Add(childMemoryBlock2);
          }
        }
      }
      return usedHeapParameter;
    }

    internal bool LinkPointer()
    {
      try
      {
        foreach (KeyValuePair<ushort, string> keyValuePair in this.pointer)
        {
          ushort NewValue;
          if (this.MyMeter.MyLinker.MyLabelAddresses.ContainsKey(keyValuePair.Value))
          {
            NewValue = (ushort) this.MyMeter.MyLinker.MyLabelAddresses[keyValuePair.Value];
          }
          else
          {
            NewValue = this.LinkEventPointer(keyValuePair.Value, keyValuePair.Key);
            if (NewValue == (ushort) 0)
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Can not find the label for pointer: " + keyValuePair.Value + " [" + keyValuePair.Key.ToString("x04") + "]", FunctionManager.logger);
              continue;
            }
          }
          if (!this.MyMeter.MyDeviceMemory.SetUshortValue((int) keyValuePair.Key, NewValue))
            return false;
        }
        string[] strArray = new string[5]
        {
          "Con_DispCodeAdrFromProt_0",
          "Con_DispCodeAdrFromProt_1",
          "Con_DispCodeAdrFromProt_2",
          "Con_DispCodeAdrFromProt_3",
          "Con_DispCodeAdrFromProt_4"
        };
        List<string> outOfProtectionBrunches = new List<string>();
        if (this.MyFunctionTable.childMemoryBlocks.Count == 3)
          outOfProtectionBrunches.Add("MAIN");
        foreach (S3_MemoryBlock childMemoryBlock1 in this.MyMeter.MyDeviceMemory.BlockProtectedDisplayCode.childMemoryBlocks)
        {
          foreach (S3_MemoryBlock childMemoryBlock2 in childMemoryBlock1.childMemoryBlocks)
          {
            if (childMemoryBlock2 is S3_DisplayMenuBrunches && !((S3_DisplayMenuBrunches) childMemoryBlock2).InsertBrunches(outOfProtectionBrunches))
              return false;
          }
        }
        for (int index = 0; index < strArray.Length; ++index)
        {
          S3_Parameter s3Parameter = this.MyMeter.MyParameters.ParameterByName[strArray[index]];
          if (index >= outOfProtectionBrunches.Count)
          {
            if (!s3Parameter.SetUshortValue((ushort) 0))
              return false;
          }
          else
          {
            if (!this.MyMeter.MyLinker.MyLabelAddresses.ContainsKey(outOfProtectionBrunches[index]))
              return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Lable not found: " + outOfProtectionBrunches[index], FunctionManager.logger);
            ushort labelAddress = (ushort) this.MyMeter.MyLinker.MyLabelAddresses[outOfProtectionBrunches[index]];
            if (!s3Parameter.SetUshortValue(labelAddress))
              return false;
          }
        }
        if (this.MyMeter.MyDeviceMemory.BlockDisplayCode.childMemoryBlocks != null)
        {
          foreach (S3_MemoryBlock childMemoryBlock3 in this.MyMeter.MyDeviceMemory.BlockDisplayCode.childMemoryBlocks)
          {
            foreach (S3_MemoryBlock childMemoryBlock4 in childMemoryBlock3.childMemoryBlocks)
            {
              if (childMemoryBlock4 is S3_DisplayMenuBrunches && !((S3_DisplayMenuBrunches) childMemoryBlock4).InsertBrunches((List<string>) null))
                return false;
            }
          }
        }
        this.MyMeter.MyParameters.ParameterByName["Con_FunctionTableAdr"].SetUshortValue((ushort) this.MyMeter.MyDeviceMemory.BlockFunctionTable.BlockStartAddress);
        S3_Parameter s3Parameter1 = this.MyMeter.MyParameters.ParameterByName["Con_CycleRuntimeCodeAdr"];
        if (this.MyMeter.MyDeviceMemory.BlockCycleRuntimeCode.ByteSize > 0)
          s3Parameter1.SetUshortValue((ushort) this.MyMeter.MyDeviceMemory.BlockCycleRuntimeCode.BlockStartAddress);
        else
          s3Parameter1.SetUshortValue((ushort) 0);
        S3_Parameter s3Parameter2 = this.MyMeter.MyParameters.ParameterByName["Con_MBusRuntimeCodeAdr"];
        if (this.MyMeter.MyDeviceMemory.BlockMBusRuntimeCode.ByteSize > 0)
          s3Parameter2.SetUshortValue((ushort) this.MyMeter.MyDeviceMemory.BlockMBusRuntimeCode.BlockStartAddress);
        else
          s3Parameter2.SetUshortValue((ushort) 0);
        S3_Parameter s3Parameter3 = this.MyMeter.MyParameters.ParameterByName["Con_MesurementRuntimeCodeAdr"];
        if (this.MyMeter.MyDeviceMemory.BlockMesurementRuntimeCode.ByteSize > 0)
          s3Parameter3.SetUshortValue((ushort) this.MyMeter.MyDeviceMemory.BlockMesurementRuntimeCode.BlockStartAddress);
        else
          s3Parameter3.SetUshortValue((ushort) 0);
        S3_Parameter s3Parameter4 = this.MyMeter.MyParameters.ParameterByName["Con_ResetRuntimeCodeAdr"];
        if (this.MyMeter.MyDeviceMemory.BlockResetRuntimeCode.ByteSize > 0)
          s3Parameter4.SetUshortValue((ushort) this.MyMeter.MyDeviceMemory.BlockResetRuntimeCode.BlockStartAddress);
        else
          s3Parameter4.SetUshortValue((ushort) 0);
        ushort labelAddress1 = (ushort) this.MyMeter.MyLinker.MyLabelAddresses["MAIN"];
        this.MyMeter.MyParameters.ParameterByName["Con_ProtectedDispCodeAdrHead"].SetUshortValue(labelAddress1);
        S3_Parameter s3Parameter5 = this.MyMeter.MyParameters.ParameterByName["Con_ProtectedDispCodeAdrCool"];
        if (this.MyMeter.MyLinker.MyLabelAddresses.ContainsKey("S3_CoolEnergy.S3_CoolEnergy"))
          s3Parameter5.SetUshortValue((ushort) this.MyMeter.MyLinker.MyLabelAddresses["S3_CoolEnergy.S3_CoolEnergy"]);
        else
          s3Parameter5.SetUshortValue(labelAddress1);
        S3_Parameter s3Parameter6 = this.MyMeter.MyParameters.ParameterByName["Con_ProtectedDispCodeAdrSegTest"];
        if (this.MyMeter.MyLinker.MyLabelAddresses.ContainsKey("S3_SegmentTest.S3_SegmentTest"))
          s3Parameter6.SetUshortValue((ushort) this.MyMeter.MyLinker.MyLabelAddresses["S3_SegmentTest.S3_SegmentTest"]);
        else
          s3Parameter6.SetUshortValue(labelAddress1);
        S3_Parameter s3Parameter7 = this.MyMeter.MyParameters.ParameterByName["Con_ProtectedDispCodeAdrTest"];
        if (this.MyMeter.MyLinker.MyLabelAddresses.ContainsKey("S3_TestState.S3_TestState"))
          s3Parameter7.SetUshortValue((ushort) this.MyMeter.MyLinker.MyLabelAddresses["S3_TestState.S3_TestState"]);
        else
          s3Parameter7.SetUshortValue(labelAddress1);
        S3_Parameter s3Parameter8 = this.MyMeter.MyParameters.ParameterByName["Con_ProtectedDispCodeAdrSleep"];
        if (this.MyMeter.MyLinker.MyLabelAddresses.ContainsKey("S3_Sleep.S3_Sleep"))
          s3Parameter8.SetUshortValue((ushort) this.MyMeter.MyLinker.MyLabelAddresses["S3_Sleep.S3_Sleep"]);
        else
          s3Parameter8.SetUshortValue(labelAddress1);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ex.ToString(), FunctionManager.logger);
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Link function error", FunctionManager.logger);
      }
      return true;
    }

    private ushort LinkEventPointer(string PointerName, ushort PointerAddress)
    {
      if (PointerName != "MAIN" && PointerName != "FIRST" && PointerName != "NEXT" && PointerName != "RIGHT")
        return 0;
      S3_Function s3Function1 = (S3_Function) null;
      foreach (S3_MemoryBlock childMemoryBlock in this.MyMeter.MyDeviceMemory.BlockProtectedDisplayCode.childMemoryBlocks)
      {
        if ((int) PointerAddress < childMemoryBlock.StartAddressOfNextBlock)
        {
          s3Function1 = ((S3_DisplayFunction) childMemoryBlock).MyFunction;
          break;
        }
      }
      if (s3Function1 == null)
      {
        foreach (S3_MemoryBlock childMemoryBlock in this.MyMeter.MyDeviceMemory.BlockDisplayCode.childMemoryBlocks)
        {
          if ((int) PointerAddress < childMemoryBlock.StartAddressOfNextBlock)
          {
            s3Function1 = ((S3_DisplayFunction) childMemoryBlock).MyFunction;
            break;
          }
        }
      }
      if (s3Function1 == null)
        throw new Exception("source-function not found for the event-pointer: " + PointerName);
      ushort blockStartAddress;
      switch (PointerName)
      {
        case "MAIN":
          S3_Function function1 = this.GetFunction(((S3_Function) this.MyFunctionTable.childMemoryBlocks[1].childMemoryBlocks[0]).FunctionNumber);
          if (function1.DisplayCode == null)
          {
            if (this.MyFunctionTable.childMemoryBlocks[1].childMemoryBlocks.Count <= 1)
              throw new NotImplementedException();
            function1 = this.GetFunction(((S3_Function) this.MyFunctionTable.childMemoryBlocks[1].childMemoryBlocks[1]).FunctionNumber);
          }
          blockStartAddress = (ushort) function1.DisplayCode.BlockStartAddress;
          break;
        case "FIRST":
          S3_Function function2 = this.GetFunction(((S3_Function) this.MyFunctionTable.childMemoryBlocks[s3Function1.LayerNr].childMemoryBlocks[0]).FunctionNumber);
          if (function2.DisplayCode == null)
          {
            if (this.MyFunctionTable.childMemoryBlocks[s3Function1.LayerNr].childMemoryBlocks.Count <= 1)
              throw new NotImplementedException();
            function2 = this.GetFunction(((S3_Function) this.MyFunctionTable.childMemoryBlocks[s3Function1.LayerNr].childMemoryBlocks[1]).FunctionNumber);
          }
          blockStartAddress = (ushort) function2.DisplayCode.BlockStartAddress;
          break;
        case "NEXT":
          S3_MemoryBlock childMemoryBlock1 = this.MyFunctionTable.childMemoryBlocks[s3Function1.LayerNr];
          int num1 = childMemoryBlock1.childMemoryBlocks.IndexOf((S3_MemoryBlock) s3Function1);
          if (childMemoryBlock1.childMemoryBlocks.Count > num1 + 1)
          {
            S3_Function s3Function2 = (S3_Function) childMemoryBlock1.childMemoryBlocks[num1 + 1];
            if (s3Function2.DisplayCode == null)
              s3Function2 = childMemoryBlock1.childMemoryBlocks.Count <= num1 + 2 ? (S3_Function) childMemoryBlock1.childMemoryBlocks[0] : (S3_Function) childMemoryBlock1.childMemoryBlocks[num1 + 2];
            blockStartAddress = (ushort) s3Function2.DisplayCode.BlockStartAddress;
            break;
          }
          S3_Function childMemoryBlock2 = (S3_Function) childMemoryBlock1.childMemoryBlocks[0];
          if (childMemoryBlock2.DisplayCode == null)
          {
            if (childMemoryBlock1.childMemoryBlocks.Count <= 1)
              throw new NotImplementedException();
            childMemoryBlock2 = (S3_Function) childMemoryBlock1.childMemoryBlocks[1];
          }
          blockStartAddress = (ushort) childMemoryBlock2.DisplayCode.BlockStartAddress;
          break;
        case "RIGHT":
          S3_MemoryBlock childMemoryBlock3 = this.MyFunctionTable.childMemoryBlocks[s3Function1.LayerNr];
          int num2 = childMemoryBlock3.parentMemoryBlock.childMemoryBlocks.IndexOf(childMemoryBlock3);
          if (childMemoryBlock3.parentMemoryBlock.childMemoryBlocks.Count > num2 + 2)
          {
            S3_MemoryBlock childMemoryBlock4 = childMemoryBlock3.parentMemoryBlock.childMemoryBlocks[num2 + 1];
            if (childMemoryBlock4.childMemoryBlocks == null || childMemoryBlock4.childMemoryBlocks.Count == 0)
            {
              S3_Function function3 = this.GetFunction(((S3_Function) this.MyFunctionTable.childMemoryBlocks[1].childMemoryBlocks[0]).FunctionNumber);
              if (function3.DisplayCode == null)
              {
                if (this.MyFunctionTable.childMemoryBlocks[1].childMemoryBlocks.Count <= 1)
                  throw new NotImplementedException();
                function3 = this.GetFunction(((S3_Function) this.MyFunctionTable.childMemoryBlocks[1].childMemoryBlocks[1]).FunctionNumber);
              }
              blockStartAddress = (ushort) function3.DisplayCode.BlockStartAddress;
              break;
            }
            S3_Function childMemoryBlock5 = (S3_Function) childMemoryBlock3.parentMemoryBlock.childMemoryBlocks[num2 + 1].childMemoryBlocks[0];
            if (childMemoryBlock5.DisplayCode == null)
            {
              if (childMemoryBlock3.parentMemoryBlock.childMemoryBlocks[num2 + 1].childMemoryBlocks.Count <= 1)
                throw new NotImplementedException();
              childMemoryBlock5 = (S3_Function) childMemoryBlock3.parentMemoryBlock.childMemoryBlocks[num2 + 1].childMemoryBlocks[1];
            }
            blockStartAddress = (ushort) childMemoryBlock5.DisplayCode.BlockStartAddress;
            break;
          }
          S3_Function function4 = this.GetFunction(((S3_Function) this.MyFunctionTable.childMemoryBlocks[1].childMemoryBlocks[0]).FunctionNumber);
          if (function4.DisplayCode == null)
          {
            if (this.MyFunctionTable.childMemoryBlocks[1].childMemoryBlocks.Count <= 1)
              throw new NotImplementedException();
            function4 = this.GetFunction(((S3_Function) this.MyFunctionTable.childMemoryBlocks[1].childMemoryBlocks[1]).FunctionNumber);
          }
          blockStartAddress = (ushort) function4.DisplayCode.BlockStartAddress;
          break;
        default:
          S3_Function function5 = this.GetFunction(((S3_Function) this.MyFunctionTable.childMemoryBlocks[1].childMemoryBlocks[0]).FunctionNumber);
          if (function5.DisplayCode == null)
          {
            if (this.MyFunctionTable.childMemoryBlocks[1].childMemoryBlocks.Count <= 1)
              throw new NotImplementedException();
            function5 = this.GetFunction(((S3_Function) this.MyFunctionTable.childMemoryBlocks[1].childMemoryBlocks[1]).FunctionNumber);
          }
          blockStartAddress = (ushort) function5.DisplayCode.BlockStartAddress;
          break;
      }
      return blockStartAddress;
    }

    internal bool ReCreateMemoryBlocks()
    {
      this.pointer.Clear();
      return this.CreateStructureObjectsOfFunctions();
    }

    private void ResetMemoryBlocksOfFunctions(S3_Meter meter)
    {
      meter.MyDeviceMemory.BlockProtectedDisplayCode.Clear();
      meter.MyDeviceMemory.BlockDisplayCode.Clear();
      meter.MyDeviceMemory.BlockCycleRuntimeCode.Clear();
      meter.MyDeviceMemory.BlockMBusRuntimeCode.Clear();
      meter.MyDeviceMemory.BlockMesurementRuntimeCode.Clear();
      meter.MyDeviceMemory.BlockResetRuntimeCode.Clear();
      meter.MyDeviceMemory.BlockRuntimeConstants.Clear();
    }

    internal void RemoveEmptyLayers()
    {
      for (int index = this.MyFunctionTable.childMemoryBlocks.Count - 1; index >= 0; --index)
      {
        S3_FunctionLayer childMemoryBlock = this.MyFunctionTable.childMemoryBlocks[index] as S3_FunctionLayer;
        if (childMemoryBlock.childMemoryBlocks == null)
          break;
        if (childMemoryBlock.NumberOfFunctions == 0)
          childMemoryBlock.RemoveFromParentMemoryBlock();
      }
    }

    public void PrintMenu(StringBuilder printText)
    {
      int num = 0;
      int ushortValue = (int) this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.Device_Setup_2.ToString()].GetUshortValue();
      foreach (S3_FunctionLayer childMemoryBlock1 in this.MyFunctionTable.childMemoryBlocks)
      {
        if (num == 0)
          ++num;
        else if (childMemoryBlock1.childMemoryBlocks != null)
        {
          printText.AppendLine("Menu layer: " + num.ToString());
          ++num;
          foreach (S3_Function childMemoryBlock2 in childMemoryBlock1.childMemoryBlocks)
          {
            if ((ushortValue & 8) != 0 || !(childMemoryBlock2.Name == "S3_FlowLine") && !(childMemoryBlock2.Name == "S3_ShowFlowLineSelectedByUser"))
              printText.AppendLine("   " + childMemoryBlock2.Name);
          }
        }
      }
    }

    internal List<string> GetSetResources()
    {
      List<string> setResources = new List<string>();
      if (this.MyFunctionTable.childMemoryBlocks == null)
        return setResources;
      foreach (S3_FunctionLayer childMemoryBlock1 in this.MyFunctionTable.childMemoryBlocks)
      {
        if (childMemoryBlock1.childMemoryBlocks != null)
        {
          foreach (S3_Function childMemoryBlock2 in childMemoryBlock1.childMemoryBlocks)
          {
            if (childMemoryBlock2.DatabaseData.SetResources != null)
            {
              foreach (string setResource in childMemoryBlock2.DatabaseData.SetResources)
              {
                if (!setResources.Contains(setResource))
                  setResources.Add(setResource);
              }
            }
          }
        }
      }
      return setResources;
    }
  }
}
