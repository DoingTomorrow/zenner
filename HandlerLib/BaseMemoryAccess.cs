// Decompiled with JetBrains decompiler
// Type: HandlerLib.BaseMemoryAccess
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib
{
  public class BaseMemoryAccess
  {
    public virtual async Task ReadMemoryAsync(
      AddressRange addressRange,
      DeviceMemory deviceMemory,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      throw new NotImplementedException(nameof (ReadMemoryAsync));
    }

    public virtual async Task WriteMemoryAsync(
      AddressRange addressRange,
      DeviceMemory deviceMemory,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      throw new NotImplementedException(nameof (WriteMemoryAsync));
    }

    public virtual async Task<DeviceIdentification> ReadVersionAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      throw new NotImplementedException(nameof (ReadVersionAsync));
    }

    public virtual async Task InterruptConnection(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
    }
  }
}
