// Decompiled with JetBrains decompiler
// Type: HandlerLib.Double2ulong
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System.Runtime.InteropServices;

#nullable disable
namespace HandlerLib
{
  [StructLayout(LayoutKind.Explicit)]
  internal struct Double2ulong
  {
    [FieldOffset(0)]
    public double d;
    [FieldOffset(0)]
    public ulong ul;
  }
}
