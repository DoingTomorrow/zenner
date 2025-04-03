// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3_Function
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal sealed class S3_Function : S3_MemoryBlock
  {
    internal int LayerNr;
    internal string FrunctionFlags = string.Empty;
    internal ushort FunctionTableEntry;
    internal bool PredefinedRemoveFunctionRequest = false;

    internal Function DatabaseData { get; set; }

    internal S3_DisplayFunction DisplayCode { get; set; }

    internal S3_RuntimeCode ResetRuntimeCode { get; set; }

    internal S3_RuntimeCode CycleRuntimeCode { get; set; }

    internal S3_RuntimeCode MesurementRuntimeCode { get; set; }

    internal S3_RuntimeCode MBusRuntimeCode { get; set; }

    internal SortedList<string, S3_Parameter> RuntimeVars { get; set; }

    internal string Name
    {
      get => this.DatabaseData != null ? this.DatabaseData.FunctionName : string.Empty;
    }

    internal List<FunctionPrecompiled> Precompiled
    {
      get
      {
        return this.DatabaseData != null ? this.DatabaseData.Precompiled : (List<FunctionPrecompiled>) null;
      }
    }

    internal bool IsProtected
    {
      get => ((int) this.FunctionTableEntry & 32768) == 32768;
      set
      {
        if (value)
          this.FunctionTableEntry |= (ushort) 32768;
        else
          this.FunctionTableEntry &= (ushort) short.MaxValue;
      }
    }

    internal ushort FunctionNumber
    {
      get => (ushort) ((uint) this.FunctionTableEntry & (uint) short.MaxValue);
      set
      {
        this.FunctionTableEntry = (ushort) ((uint) this.FunctionTableEntry & 32768U | (uint) value);
      }
    }

    public S3_Function(S3_Meter MyMeter, S3_MemoryBlock parentMemoryBlock)
      : base(MyMeter, S3_MemorySegment.Function, parentMemoryBlock)
    {
      this.ByteSize = 2;
    }

    public S3_Function(S3_Meter MyMeter, S3_MemoryBlock parentMemoryBlock, int insertIndex)
      : base(MyMeter, S3_MemorySegment.Function, parentMemoryBlock, insertIndex)
    {
      this.ByteSize = 2;
    }

    internal bool CreateStructureObjectsOfFunction(int LayerNr)
    {
      if (this.Precompiled == null)
        return false;
      this.LayerNr = LayerNr;
      bool flag = false;
      S3_MemoryBlock s3MemoryBlock = (S3_MemoryBlock) null;
      S3_DisplayMenu MyMenu = (S3_DisplayMenu) null;
      S3_DisplayMenuBrunches displayMenuBrunches = (S3_DisplayMenuBrunches) null;
      for (int index1 = 0; index1 < this.Precompiled.Count; ++index1)
      {
        FunctionPrecompiled funcFromDB = this.Precompiled[index1];
        try
        {
          if (funcFromDB.RecordType == FunctionRecordType.DisplayCode)
          {
            if (!flag)
            {
              this.DisplayCode = !this.IsProtected ? new S3_DisplayFunction(this.MyMeter, this, this.MyMeter.MyDeviceMemory.BlockDisplayCode) : new S3_DisplayFunction(this.MyMeter, this, this.MyMeter.MyDeviceMemory.BlockProtectedDisplayCode);
              flag = true;
            }
            MyMenu = new S3_DisplayMenu(this.MyMeter, this, funcFromDB, (S3_MemoryBlock) this.DisplayCode);
            MyMenu.MenuName = this.Name + "." + funcFromDB.Name;
            s3MemoryBlock = (S3_MemoryBlock) MyMenu;
            int? variableOrEditFramecode = funcFromDB.OffsetOfVariableOrEditFramecode;
            if (variableOrEditFramecode.HasValue)
            {
              MeterScaling meterScaling = this.MyMeter.MyMeterScaling;
              ref byte[] local1 = ref MyMenu.Codes;
              variableOrEditFramecode = funcFromDB.OffsetOfVariableOrEditFramecode;
              int frameCodeStartOffset1 = variableOrEditFramecode.Value;
              string frunctionFlags = this.FrunctionFlags;
              string str1;
              ref string local2 = ref str1;
              meterScaling.FrameCodeAdjustUnit(ref local1, frameCodeStartOffset1, frunctionFlags, out local2);
              ref byte[] local3 = ref MyMenu.Codes;
              variableOrEditFramecode = funcFromDB.OffsetOfVariableOrEditFramecode;
              int frameCodeStartOffset2 = variableOrEditFramecode.Value;
              int layer = LayerNr;
              string str2;
              ref string local4 = ref str2;
              LcdUnitsC2.FrameCodeAdjustLayer(ref local3, frameCodeStartOffset2, layer, out local4);
            }
            displayMenuBrunches = (S3_DisplayMenuBrunches) null;
          }
          else if (funcFromDB.RecordType == FunctionRecordType.ResetRuntimeCode)
          {
            this.ResetRuntimeCode = new S3_RuntimeCode(funcFromDB, this.MyMeter, this.MyMeter.MyDeviceMemory.BlockResetRuntimeCode);
            s3MemoryBlock = (S3_MemoryBlock) this.ResetRuntimeCode;
          }
          else if (funcFromDB.RecordType == FunctionRecordType.CycleRuntimeCode)
          {
            this.CycleRuntimeCode = new S3_RuntimeCode(funcFromDB, this.MyMeter, this.MyMeter.MyDeviceMemory.BlockCycleRuntimeCode);
            s3MemoryBlock = (S3_MemoryBlock) this.CycleRuntimeCode;
          }
          else if (funcFromDB.RecordType == FunctionRecordType.MesurementRuntimeCode)
          {
            this.MesurementRuntimeCode = new S3_RuntimeCode(funcFromDB, this.MyMeter, this.MyMeter.MyDeviceMemory.BlockMesurementRuntimeCode);
            s3MemoryBlock = (S3_MemoryBlock) this.MesurementRuntimeCode;
          }
          else if (funcFromDB.RecordType == FunctionRecordType.MBusRuntimeCode)
          {
            this.MBusRuntimeCode = new S3_RuntimeCode(funcFromDB, this.MyMeter, this.MyMeter.MyDeviceMemory.BlockMBusRuntimeCode);
            s3MemoryBlock = (S3_MemoryBlock) this.MBusRuntimeCode;
          }
          else if (funcFromDB.RecordType == FunctionRecordType.RuntimeVars)
          {
            if (!this.MyMeter.MyParameters.ParameterByName.ContainsKey(funcFromDB.Name))
              this.MyMeter.MyParameters.AddNewHeapParameterByName(funcFromDB.Name, this.MyMeter.MyDeviceMemory.BlockHeap);
            if (this.RuntimeVars == null)
              this.RuntimeVars = new SortedList<string, S3_Parameter>();
            if (!this.RuntimeVars.ContainsKey(funcFromDB.Name))
            {
              S3_Parameter s3Parameter = this.MyMeter.MyParameters.ParameterByName[funcFromDB.Name];
              this.RuntimeVars.Add(s3Parameter.Name, s3Parameter);
            }
          }
          else if (funcFromDB.RecordType == FunctionRecordType.BackupRuntimeVars)
          {
            if (!this.MyMeter.MyParameters.ParameterByName.ContainsKey(funcFromDB.Name))
              this.MyMeter.MyParameters.AddNewHeapParameterByName(funcFromDB.Name, this.MyMeter.MyDeviceMemory.BlockBackupRuntimeVars);
            if (this.RuntimeVars == null)
              this.RuntimeVars = new SortedList<string, S3_Parameter>();
            if (!this.RuntimeVars.ContainsKey(funcFromDB.Name))
            {
              S3_Parameter s3Parameter = this.MyMeter.MyParameters.ParameterByName[funcFromDB.Name];
              this.RuntimeVars.Add(s3Parameter.Name, s3Parameter);
            }
          }
          else if (funcFromDB.RecordType == FunctionRecordType.Event_Click)
          {
            if (displayMenuBrunches == null)
              displayMenuBrunches = new S3_DisplayMenuBrunches(this.MyMeter, MyMenu, (S3_MemoryBlock) this.DisplayCode);
            displayMenuBrunches.ClickPointerName = this.GetPointerName(funcFromDB.Name);
          }
          else if (funcFromDB.RecordType == FunctionRecordType.Event_Press)
          {
            if (displayMenuBrunches == null)
              displayMenuBrunches = new S3_DisplayMenuBrunches(this.MyMeter, MyMenu, (S3_MemoryBlock) this.DisplayCode);
            displayMenuBrunches.PressPointerName = this.GetPointerName(funcFromDB.Name);
          }
          else if (funcFromDB.RecordType == FunctionRecordType.Event_Hold)
          {
            if (displayMenuBrunches == null)
              displayMenuBrunches = new S3_DisplayMenuBrunches(this.MyMeter, MyMenu, (S3_MemoryBlock) this.DisplayCode);
            displayMenuBrunches.HoldPointerName = this.GetPointerName(funcFromDB.Name);
          }
          else if (funcFromDB.RecordType == FunctionRecordType.Event_Timeout)
          {
            if (displayMenuBrunches == null)
              displayMenuBrunches = new S3_DisplayMenuBrunches(this.MyMeter, MyMenu, (S3_MemoryBlock) this.DisplayCode);
            displayMenuBrunches.TimeoutPointerName = this.GetPointerName(funcFromDB.Name);
          }
          else if (funcFromDB.RecordType == FunctionRecordType.Pointer)
          {
            int num1 = 0;
            byte[] numArray = (byte[]) null;
            switch (s3MemoryBlock)
            {
              case S3_DisplayMenu _:
                numArray = MyMenu.Codes;
                MyMenu.Codes = new byte[funcFromDB.Offset];
                for (int index2 = 0; index2 < MyMenu.Codes.Length; ++index2)
                  MyMenu.Codes[index2] = numArray[index2];
                s3MemoryBlock.ByteSize = MyMenu.Codes.Length;
                break;
              case S3_RuntimeCode _:
                S3_RuntimeCode s3RuntimeCode = (S3_RuntimeCode) s3MemoryBlock;
                numArray = s3RuntimeCode.Codes;
                s3RuntimeCode.Codes = new byte[funcFromDB.Offset];
                for (int index3 = 0; index3 < s3RuntimeCode.Codes.Length; ++index3)
                  s3RuntimeCode.Codes[index3] = numArray[index3];
                s3MemoryBlock.ByteSize = s3RuntimeCode.Codes.Length;
                break;
              case S3_DataBlock _:
                S3_DataBlock s3DataBlock = (S3_DataBlock) s3MemoryBlock;
                numArray = s3DataBlock.Data;
                int length1 = funcFromDB.Offset - s3DataBlock.NumberOfPreviesBytes;
                if (length1 < 0)
                {
                  ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Precompiled function is invalid! Please check function number" + funcFromDB.FunctionNumber.ToString() + " Type: " + funcFromDB.RecordType.ToString() + " Precompiled Order Number: " + funcFromDB.RecordOrder.ToString(), FunctionManager.logger);
                  return false;
                }
                s3DataBlock.Data = new byte[length1];
                for (int index4 = 0; index4 < s3DataBlock.Data.Length; ++index4)
                  s3DataBlock.Data[index4] = numArray[index4];
                s3MemoryBlock.ByteSize = s3DataBlock.Data.Length;
                num1 = s3DataBlock.NumberOfPreviesBytes;
                break;
            }
            S3_Pointer s3Pointer = new S3_Pointer(funcFromDB.Name, this.MyMeter, s3MemoryBlock.parentMemoryBlock);
            int num2 = funcFromDB.Offset + 2 - num1;
            int length2 = num1 + numArray.Length - funcFromDB.Offset - 2;
            if (length2 > 0)
            {
              byte[] Data = new byte[length2];
              for (int index5 = 0; index5 < length2; ++index5)
                Data[index5] = numArray[index5 + num2];
              s3MemoryBlock = (S3_MemoryBlock) new S3_DataBlock(Data, this.MyMeter, s3MemoryBlock.parentMemoryBlock)
              {
                NumberOfPreviesBytes = (funcFromDB.Offset + 2)
              };
            }
          }
        }
        catch (Exception ex)
        {
          throw new Exception("Precompiled exception. Funktion: " + funcFromDB.Name + ";row: " + index1.ToString() + ";Codes: " + funcFromDB.CodesAsHex, ex);
        }
      }
      return true;
    }

    private string GetPointerName(string rowName)
    {
      return rowName == "MAIN" || rowName == "FIRST" || rowName == "NEXT" || rowName == "RIGHT" ? rowName : this.Name + "." + rowName;
    }

    internal bool AdjustBrunchObjects()
    {
      if (this.DisplayCode.childMemoryBlocks == null)
        return false;
      foreach (object childMemoryBlock in this.DisplayCode.childMemoryBlocks)
      {
        if (childMemoryBlock is S3_DisplayMenuBrunches && !((S3_DisplayMenuBrunches) childMemoryBlock).AdjustPointersAndSize())
          return false;
      }
      return true;
    }

    internal bool CreateStructureObjectsOfFunctionTableFromMemory(S3_Meter meter)
    {
      this.FunctionTableEntry = meter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress);
      this.DatabaseData = meter.MyFunctions.MyDatabase.GetFunction(this.FunctionNumber);
      if (this.DatabaseData == null)
        return false;
      if (this.DatabaseData.Precompiled[0].RecordType == FunctionRecordType.FunctionFlags)
        this.FrunctionFlags = ";" + this.DatabaseData.Precompiled[0].Name + ";";
      return this.DatabaseData != null;
    }

    internal bool InsertData()
    {
      return this.MyMeter.MyDeviceMemory.SetUshortValue(this.BlockStartAddress, this.FunctionTableEntry);
    }

    internal S3_Function Clone(S3_FunctionLayer clone_S3_FunctionLayer)
    {
      S3_Meter meter = clone_S3_FunctionLayer.MyMeter;
      S3_Function theCloneFunction = new S3_Function(meter, (S3_MemoryBlock) clone_S3_FunctionLayer);
      theCloneFunction.FunctionTableEntry = this.FunctionTableEntry;
      theCloneFunction.DatabaseData = this.DatabaseData;
      theCloneFunction.FrunctionFlags = this.FrunctionFlags;
      theCloneFunction.LayerNr = this.LayerNr;
      if (this.DisplayCode != null)
        theCloneFunction.DisplayCode = !this.IsProtected ? this.DisplayCode.Clone(meter, theCloneFunction, meter.MyDeviceMemory.BlockDisplayCode) : this.DisplayCode.Clone(meter, theCloneFunction, meter.MyDeviceMemory.BlockProtectedDisplayCode);
      if (this.ResetRuntimeCode != null)
        theCloneFunction.ResetRuntimeCode = this.ResetRuntimeCode.Clone(meter, meter.MyDeviceMemory.BlockResetRuntimeCode);
      if (this.CycleRuntimeCode != null)
        theCloneFunction.CycleRuntimeCode = this.CycleRuntimeCode.Clone(meter, meter.MyDeviceMemory.BlockCycleRuntimeCode);
      if (this.MesurementRuntimeCode != null)
        theCloneFunction.MesurementRuntimeCode = this.MesurementRuntimeCode.Clone(meter, meter.MyDeviceMemory.BlockMesurementRuntimeCode);
      if (this.MBusRuntimeCode != null)
        theCloneFunction.MBusRuntimeCode = this.MBusRuntimeCode.Clone(meter, meter.MyDeviceMemory.BlockMBusRuntimeCode);
      if (this.RuntimeVars != null)
      {
        theCloneFunction.RuntimeVars = new SortedList<string, S3_Parameter>();
        foreach (KeyValuePair<string, S3_Parameter> runtimeVar in this.RuntimeVars)
        {
          try
          {
            S3_Parameter s3Parameter = theCloneFunction.MyMeter.MyParameters.ParameterByName[runtimeVar.Key];
            theCloneFunction.RuntimeVars.Add(s3Parameter.Name, s3Parameter);
          }
          catch (Exception ex)
          {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("HeapParameter not found: " + runtimeVar.Key);
            stringBuilder.AppendLine("Exception:");
            stringBuilder.AppendLine(ex.ToString());
            ZR_ClassLibMessages.AddErrorDescriptionAndException(ZR_ClassLibMessages.LastErrors.IllegalData, stringBuilder.ToString(), S3_FunctionLayer.logger);
          }
        }
      }
      return theCloneFunction;
    }

    public override string ToString()
    {
      return base.ToString() + " # " + this.FunctionNumber.ToString() + " " + this.Name + (this.IsProtected ? " protected" : "");
    }
  }
}
