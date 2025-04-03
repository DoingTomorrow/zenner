// Decompiled with JetBrains decompiler
// Type: HandlerLib.MapManagement.MapSectionParameters
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib.MapManagement
{
  public class MapSectionParameters
  {
    public string SectionName { get; set; }

    public uint StartAddress { get; set; }

    public uint Size { get; set; }

    public override string ToString()
    {
      return string.Format("0x{0:X8} {1} {2} bytes", (object) this.StartAddress, (object) this.SectionName, (object) this.Size);
    }
  }
}
