// Decompiled with JetBrains decompiler
// Type: S3_Handler.WriteProtTableManager
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal sealed class WriteProtTableManager
  {
    private S3_HandlerFunctions MyFunctions;
    private S3_Meter MyMeter;
    internal bool[] ByteIsNotProtected;
    private bool isCreated = false;

    public WriteProtTableManager(S3_HandlerFunctions MyFunctions, S3_Meter MyMeter)
    {
      this.MyFunctions = MyFunctions;
      this.MyMeter = MyMeter;
    }

    public bool IsByteProtected(ushort address)
    {
      if (this.ByteIsNotProtected == null)
      {
        this.ByteIsNotProtected = new bool[this.MyMeter.MyDeviceMemory.ByteIsDefined.Length];
        foreach (S3_MemoryBlock childMemoryBlock in this.MyMeter.MyDeviceMemory.BlockWriteProtTable.childMemoryBlocks)
        {
          if (childMemoryBlock is NotProtectedRange)
          {
            ushort protectedAddress = ((NotProtectedRange) childMemoryBlock).NotProtectedAddress;
            ushort notProtectedLength = ((NotProtectedRange) childMemoryBlock).NotProtectedLength;
            for (int index = (int) protectedAddress; index < (int) protectedAddress + (int) notProtectedLength; ++index)
              this.ByteIsNotProtected[index] = true;
          }
        }
      }
      return !this.ByteIsNotProtected[(int) address];
    }

    internal bool CreateStructureObjects()
    {
      if (this.isCreated)
        throw new Exception("Die WriteProtectedTableManager.CreateStructureObjects darf nicht mehrmals aufgerufen werden!");
      if (!this.MyMeter.MyDeviceMemory.BlockWriteProtTable.CreateStructureObjects((ushort) this.MyMeter.MyParameters.ParameterByName["Con_WritePermissionTableFirstSize"].BlockStartAddress))
        return false;
      this.isCreated = true;
      return true;
    }

    internal List<KeyValuePair<int, int>> GetProtectionRanges()
    {
      List<KeyValuePair<int, int>> protectionRanges = new List<KeyValuePair<int, int>>();
      foreach (S3_MemoryBlock childMemoryBlock in this.MyMeter.MyDeviceMemory.BlockWriteProtTable.childMemoryBlocks)
      {
        if (childMemoryBlock is NotProtectedRange)
        {
          NotProtectedRange notProtectedRange = childMemoryBlock as NotProtectedRange;
          protectionRanges.Add(new KeyValuePair<int, int>((int) notProtectedRange.NotProtectedAddress, (int) notProtectedRange.NotProtectedAddress + (int) notProtectedRange.NotProtectedLength - 1));
        }
      }
      return protectionRanges;
    }

    internal bool SetProtectionRanges(List<KeyValuePair<int, int>> ranges)
    {
      this.MyMeter.MyDeviceMemory.BlockWriteProtTable.Clear();
      for (int index = 0; index < ranges.Count; ++index)
      {
        NotProtectedRange notProtectedRange = new NotProtectedRange(this.MyMeter, (S3_MemoryBlock) this.MyMeter.MyDeviceMemory.BlockWriteProtTable)
        {
          NotProtectedAddress = (ushort) ranges[index].Key
        };
        notProtectedRange.NotProtectedLength = (ushort) (ranges[index].Value - (int) notProtectedRange.NotProtectedAddress + 1);
      }
      S3_MemoryBlock s3MemoryBlock = new S3_MemoryBlock(this.MyMeter, 2, S3_MemorySegment.WriteProt, (S3_MemoryBlock) this.MyMeter.MyDeviceMemory.BlockWriteProtTable);
      return true;
    }

    internal WriteProtTableManager Clone(S3_Meter theCloneMeter)
    {
      this.MyMeter.MyDeviceMemory.BlockWriteProtTable.Clone(theCloneMeter);
      return new WriteProtTableManager(this.MyFunctions, theCloneMeter);
    }

    internal bool InsertData() => this.MyMeter.MyDeviceMemory.BlockWriteProtTable.InsertData();
  }
}
