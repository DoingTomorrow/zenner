// Decompiled with JetBrains decompiler
// Type: HandlerLib.MBusChannelConfiguration
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib
{
  public sealed class MBusChannelConfiguration : ReturnValue
  {
    public byte Channel { get; set; }

    public byte[] Mantissa { get; set; }

    public sbyte Exponent { get; set; }

    public byte VIF { get; set; }
  }
}
