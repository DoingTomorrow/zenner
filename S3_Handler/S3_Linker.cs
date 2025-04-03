// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3_Linker
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using NLog;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal sealed class S3_Linker
  {
    private static Logger S3_LinkerLogger = LogManager.GetLogger(nameof (S3_Linker));
    internal S3_HandlerFunctions MyFunctions;
    internal S3_Meter MyMeter;
    internal SortedList<string, int> MyLabelAddresses;

    public S3_Linker(S3_HandlerFunctions MyFunctions, S3_Meter MyMeter)
    {
      this.MyFunctions = MyFunctions;
      this.MyMeter = MyMeter;
      this.MyLabelAddresses = new SortedList<string, int>();
    }

    public static IEnumerable<S3_MemoryBlock> ForEachMemoryBlock(S3_MemoryBlock root)
    {
      if (root != null)
      {
        yield return root;
        foreach (S3_MemoryBlock child in S3_Linker.ForEachChildMemoryBlock(root))
          yield return child;
      }
    }

    public static IEnumerable<S3_MemoryBlock> ForEachChildMemoryBlock(S3_MemoryBlock root)
    {
      if (root != null && root.childMemoryBlocks != null)
      {
        foreach (S3_MemoryBlock block in root.childMemoryBlocks)
        {
          yield return block;
          foreach (S3_MemoryBlock child in S3_Linker.ForEachChildMemoryBlock(block))
            yield return child;
        }
      }
    }

    internal bool Link(S3_MemoryBlock root)
    {
      int num1 = root.Alignment - 1;
      int blockStartAddress1;
      if ((root.BlockStartAddress & num1) != 0)
      {
        if (!this.MyFunctions.baseTypeEditMode && root.IsHardLinkedAddress)
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Can not align hard linked address: 0x" + root.BlockStartAddress.ToString("x04"), S3_Linker.S3_LinkerLogger);
        int num2 = (root.BlockStartAddress & ~num1) + root.Alignment;
        if (S3_Linker.S3_LinkerLogger.IsTraceEnabled)
        {
          Logger s3LinkerLogger = S3_Linker.S3_LinkerLogger;
          string[] strArray = new string[8];
          strArray[0] = "Align block from 0x";
          blockStartAddress1 = root.BlockStartAddress;
          strArray[1] = blockStartAddress1.ToString("x04");
          strArray[2] = " to 0x";
          strArray[3] = num2.ToString("x04");
          strArray[4] = " Alignment: ";
          strArray[5] = root.Alignment.ToString();
          strArray[6] = " ";
          strArray[7] = root.ToString();
          string message = string.Concat(strArray);
          s3LinkerLogger.Trace(message);
        }
        root.BlockStartAddress = num2;
      }
      if (root.childMemoryBlocks != null)
      {
        if (root.childMemoryBlocks.Count == 0)
        {
          root.childMemoryBlocks = (List<S3_MemoryBlock>) null;
          root.ByteSize = root.firstChildMemoryBlockOffset;
        }
        else
        {
          int num3 = 0;
          S3_MemoryBlock s3MemoryBlock = (S3_MemoryBlock) null;
          for (int index = 0; index < root.childMemoryBlocks.Count; ++index)
          {
            S3_MemoryBlock childMemoryBlock = root.childMemoryBlocks[index];
            if (!childMemoryBlock.IsNotLinked)
            {
              int num4 = s3MemoryBlock != null ? s3MemoryBlock.StartAddressOfNextBlock : root.BlockStartAddress + root.firstChildMemoryBlockOffset;
              s3MemoryBlock = childMemoryBlock;
              if (!childMemoryBlock.IsHardLinkedAddress)
              {
                if (num4 != childMemoryBlock.BlockStartAddress)
                  childMemoryBlock.BlockStartAddress = num4;
              }
              else if (childMemoryBlock.BlockStartAddress < num4)
              {
                if (this.MyFunctions.baseTypeEditMode)
                {
                  childMemoryBlock.BlockStartAddress = num4;
                }
                else
                {
                  blockStartAddress1 = childMemoryBlock.BlockStartAddress;
                  return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Can not move hard linked address: 0x" + blockStartAddress1.ToString("x04"), S3_Linker.S3_LinkerLogger);
                }
              }
              int blockStartAddress2 = childMemoryBlock.BlockStartAddress;
              if (!this.Link(childMemoryBlock))
                return false;
              int num5 = childMemoryBlock.BlockStartAddress - blockStartAddress2;
              num3 += childMemoryBlock.ByteSize + num5;
            }
          }
          if (!root.IsFixSize)
          {
            int byteSize = root.ByteSize;
            int num6 = num3 + root.firstChildMemoryBlockOffset;
            if (byteSize != num6)
              root.ByteSize = num6;
          }
        }
      }
      return true;
    }

    internal bool MoveEmtyBlocks() => this.MoveEmtyBlocks(this.MyMeter.MyDeviceMemory.meterMemory);

    private bool MoveEmtyBlocks(S3_MemoryBlock root)
    {
      if (root.childMemoryBlocks != null && root.childMemoryBlocks.Count > 0)
      {
        int num = root.BlockStartAddress;
        for (int index = 0; index < root.childMemoryBlocks.Count; ++index)
        {
          if (root.childMemoryBlocks[index].BlockStartAddress == 0)
            root.childMemoryBlocks[index].BlockStartAddress = num;
          num = root.childMemoryBlocks[index].StartAddressOfNextBlock;
          if (!this.MoveEmtyBlocks(root.childMemoryBlocks[index]))
            return false;
        }
      }
      return true;
    }

    internal S3_Linker Clone(S3_Meter theCloneMeter)
    {
      theCloneMeter.MyLinker = new S3_Linker(this.MyFunctions, theCloneMeter);
      foreach (KeyValuePair<string, int> labelAddress in this.MyLabelAddresses)
        theCloneMeter.MyLinker.MyLabelAddresses.Add(labelAddress.Key, labelAddress.Value);
      return theCloneMeter.MyLinker;
    }

    internal bool ResetLabels()
    {
      this.MyLabelAddresses = this.MyMeter.MyLoggerManager.GetLoggerChanalsByNames();
      foreach (KeyValuePair<string, S3_Parameter> keyValuePair in this.MyMeter.MyParameters.ParameterByName)
        this.AddLabel(keyValuePair.Key, keyValuePair.Value.BlockStartAddress);
      foreach (KeyValuePair<string, int> addressLable in this.MyMeter.MyParameters.AddressLables)
        this.AddLabel(addressLable.Key, addressLable.Value);
      return true;
    }

    internal void AddLabel(string labelName, int address)
    {
      if (this.MyLabelAddresses.ContainsKey(labelName))
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Add lable error. Lable exists! LableName: " + labelName + " Old address: 0x" + this.MyLabelAddresses[labelName].ToString("X4") + " New address: 0x" + address.ToString("X4"), S3_Linker.S3_LinkerLogger);
      else
        this.MyLabelAddresses.Add(labelName, address);
    }
  }
}
