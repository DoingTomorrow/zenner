// Decompiled with JetBrains decompiler
// Type: MBusLib.Wildcard
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

#nullable disable
namespace MBusLib
{
  public class Wildcard
  {
    public uint ID_BCD { get; set; }

    public ushort Manufacturer { get; set; }

    public byte Version { get; set; }

    public byte Medium { get; set; }

    public Wildcard()
      : this(uint.MaxValue)
    {
    }

    public Wildcard(uint id_bcd)
    {
      this.ID_BCD = id_bcd;
      this.Manufacturer = ushort.MaxValue;
      this.Version = byte.MaxValue;
      this.Medium = byte.MaxValue;
    }
  }
}
