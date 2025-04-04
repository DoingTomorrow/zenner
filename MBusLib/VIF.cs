// Decompiled with JetBrains decompiler
// Type: MBusLib.VIF
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

using MBusLib.Utility;
using ZENNER.CommonLibrary;

#nullable disable
namespace MBusLib
{
  public sealed class VIF : IPrintable
  {
    public byte Value { get; set; }

    public bool HasExtension => ((int) this.Value & 128) == 128;

    public byte UnitAndMultiplier => (byte) ((uint) this.Value & (uint) sbyte.MaxValue);

    public override string ToString() => "0x" + this.Value.ToString("X2");

    public string Print(int spaces = 0) => Util.PrintObject((object) this);

    public static VIF Parse(byte value)
    {
      return new VIF() { Value = value };
    }
  }
}
