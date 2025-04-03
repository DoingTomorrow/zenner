// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.NodeLayer
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using GmmDbLib;
using System.Collections.Generic;

#nullable disable
namespace ZR_ClassLibrary
{
  public sealed class NodeLayer
  {
    private const string MODUL_NAME = "MeterInstaller";

    public int LayerID { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string TranslatedName
    {
      get
      {
        switch (this.LayerID)
        {
          case 0:
            return NodeLayer.GetTranslatedLanguageText("MeterInstaller", "PhysicalLayer");
          case 1:
            return NodeLayer.GetTranslatedLanguageText("MeterInstaller", "LogicLayer");
          default:
            return this.Name;
        }
      }
    }

    public bool IsPhysicalLayer => this.LayerID == 0;

    public static List<NodeLayer> GetStaticLayers()
    {
      return new List<NodeLayer>()
      {
        new NodeLayer()
        {
          LayerID = 0,
          Name = "PhysicalLayer",
          Description = NodeLayer.GetTranslatedLanguageText("MeterInstaller", "PhysicalLayerDesc")
        },
        new NodeLayer() { LayerID = 1, Name = "LogicLayer" }
      };
    }

    private static string GetTranslatedLanguageText(string GmmModule, string TextKey)
    {
      string str = GmmModule + TextKey;
      return Ot.Gtt(Tg.Common, str, str);
    }
  }
}
