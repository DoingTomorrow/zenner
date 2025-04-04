// Decompiled with JetBrains decompiler
// Type: SmartFunctionCompiler.FunctionRamHeader
// Assembly: SmartFunctionCompiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: E49EBEEE-4E03-4F25-A9DE-0F245CFB9A90
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmartFunctionCompiler.exe

using System;

#nullable disable
namespace SmartFunctionCompiler
{
  internal class FunctionRamHeader
  {
    internal ushort StorageOffset;

    internal FunctionRamHeader(ushort storageOffset) => this.StorageOffset = storageOffset;

    internal uint NextEventTime
    {
      get => BitConverter.ToUInt32(FunctionLoader.RamStorage, (int) this.StorageOffset);
      set
      {
        Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) FunctionLoader.RamStorage, (int) this.StorageOffset, 4);
      }
    }

    internal uint FunctionCallCounter
    {
      get => BitConverter.ToUInt32(FunctionLoader.RamStorage, (int) this.StorageOffset + 4);
      set
      {
        Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) FunctionLoader.RamStorage, (int) this.StorageOffset + 4, 4);
      }
    }
  }
}
