// Decompiled with JetBrains decompiler
// Type: HandlerLib.MapManagement.MapParameterInfo
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib.MapManagement
{
  public class MapParameterInfo
  {
    public string FirmwareName { get; set; }

    public uint MapAddress { get; set; }

    public uint ByteSize { get; set; }

    public string Section { get; set; }

    public string Typ { get; set; }

    public bool ShowInMAP { get; set; }

    public MapParameterInfo Clone()
    {
      return new MapParameterInfo()
      {
        FirmwareName = this.FirmwareName,
        MapAddress = this.MapAddress,
        Section = this.Section,
        ByteSize = this.ByteSize,
        Typ = this.Typ,
        ShowInMAP = this.ShowInMAP
      };
    }
  }
}
