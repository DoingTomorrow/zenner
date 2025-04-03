// Decompiled with JetBrains decompiler
// Type: S3_Handler.LoggerChanalDataBase
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System.Data;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal abstract class LoggerChanalDataBase : S3_MemoryBlock
  {
    internal LoggerChanalDataBase(
      S3_Meter MyMeter,
      S3_MemorySegment SegmentType,
      S3_MemoryBlock parentMemoryBlock)
      : base(MyMeter, SegmentType, parentMemoryBlock)
    {
    }

    internal LoggerChanalDataBase(
      S3_Meter MyMeter,
      S3_MemorySegment SegmentType,
      S3_MemoryBlock parentMemoryBlock,
      int insertIndex)
      : base(MyMeter, SegmentType, parentMemoryBlock, insertIndex)
    {
    }

    internal LoggerChanalDataBase(
      S3_Meter MyMeter,
      S3_MemoryBlock parentMemoryBlock,
      S3_MemoryBlock sourceMemoryBlock)
      : base(MyMeter, parentMemoryBlock, sourceMemoryBlock, false)
    {
    }

    internal abstract void Clone(
      S3_Meter cloneMeter,
      LoggerChanal cloneChanalInfo,
      S3_MemoryBlock cloneParentMemoryBlock);

    internal abstract bool RemoveChanalDataObjects();

    internal abstract bool CacheChanalData();

    internal abstract DataTable GetChanalData(bool physicalView);

    internal abstract bool ChangeChanalData();

    internal abstract bool InsertChanalData();

    internal abstract bool ResetChanalData();

    internal abstract bool ResetChanalDataToOneStoredValue();

    internal abstract bool FillChanalTestData(ref ulong testValue);
  }
}
