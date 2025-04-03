// Decompiled with JetBrains decompiler
// Type: S3_Handler.BackupRuntimeVarsManager
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

#nullable disable
namespace S3_Handler
{
  internal sealed class BackupRuntimeVarsManager
  {
    private S3_HandlerFunctions MyFunctions;
    private S3_Meter MyMeter;

    internal BackupRuntimeVarsManager(S3_HandlerFunctions MyFunctions, S3_Meter MyMeter)
    {
      this.MyFunctions = MyFunctions;
      this.MyMeter = MyMeter;
    }

    internal bool Clone(S3_Meter theCloneMeter)
    {
      theCloneMeter.MyBackupRuntimeVarsManager = new BackupRuntimeVarsManager(this.MyFunctions, theCloneMeter);
      if (this.MyMeter.MyDeviceMemory.BlockBackupRuntimeVars.childMemoryBlocks != null)
      {
        foreach (S3_Parameter childMemoryBlock in this.MyMeter.MyDeviceMemory.BlockBackupRuntimeVars.childMemoryBlocks)
        {
          if (!theCloneMeter.MyParameters.ParameterByName.ContainsKey(childMemoryBlock.Name))
          {
            theCloneMeter.MyParameters.AddNewHeapParameterByName(childMemoryBlock.Name, theCloneMeter.MyDeviceMemory.BlockBackupRuntimeVars).sourceMemoryBlock = (S3_MemoryBlock) childMemoryBlock;
          }
          else
          {
            S3_Parameter s3Parameter = theCloneMeter.MyParameters.ParameterByName[childMemoryBlock.Name];
            if (s3Parameter.parentMemoryBlock.SegmentType != childMemoryBlock.parentMemoryBlock.SegmentType)
            {
              S3_MemoryBlock sourceMemoryBlock = s3Parameter.sourceMemoryBlock;
              theCloneMeter.MyParameters.ParameterByName.Remove(childMemoryBlock.Name);
              s3Parameter.parentMemoryBlock.childMemoryBlocks.Remove((S3_MemoryBlock) s3Parameter);
              theCloneMeter.MyParameters.AddNewHeapParameterByName(childMemoryBlock.Name, theCloneMeter.MyDeviceMemory.BlockBackupRuntimeVars).sourceMemoryBlock = sourceMemoryBlock;
            }
          }
        }
      }
      return true;
    }
  }
}
