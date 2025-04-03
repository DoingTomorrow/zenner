// Decompiled with JetBrains decompiler
// Type: HandlerLib.ParameterType
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System.Xml.Serialization;

#nullable disable
namespace HandlerLib
{
  public class ParameterType
  {
    [XmlElement("ParameterTypeSaved")]
    public string ParameterTypeSaved { get; set; }

    [XmlElement("ParameterTypeColPreset")]
    public int ParameterTypeColPreset { get; set; }
  }
}
