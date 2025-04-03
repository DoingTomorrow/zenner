// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.NodeList
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;

#nullable disable
namespace ZR_ClassLibrary
{
  public sealed class NodeList
  {
    public int NodeID;
    public int MeterID;
    public int NodeTypeID;
    public string NodeName;
    public string NodeDescription;
    public string NodeSettings;
    public DateTime? ValidFrom;
    public DateTime? ValidTo;
    public string NodeAdditionalInfos;

    public override string ToString()
    {
      return string.Format("NodeID: {0}, MeterID: {1}, NodeName: {2}", (object) this.NodeID, (object) this.MeterID, (object) this.NodeName);
    }
  }
}
