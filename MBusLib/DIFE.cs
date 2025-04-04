// Decompiled with JetBrains decompiler
// Type: MBusLib.DIFE
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

using MBusLib.Utility;
using ZENNER.CommonLibrary;

#nullable disable
namespace MBusLib
{
  public sealed class DIFE : IPrintable
  {
    public byte Value { get; set; }

    public bool HasExtension => ((int) this.Value & 128) == 128;

    public int SubUnit => ((int) this.Value & 64) >> 6;

    public long Tariff => (long) (((int) this.Value & 48) >> 4);

    public long StorageNumber => (long) ((int) this.Value & 15);

    public override string ToString() => "0x" + this.Value.ToString("X2");

    public string Print(int spaces = 0) => Util.PrintObject((object) this);

    public static DIFE Parse(byte value)
    {
      return new DIFE() { Value = value };
    }
  }
}
