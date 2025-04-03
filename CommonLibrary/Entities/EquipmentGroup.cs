// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.Entities.EquipmentGroup
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;
using System.Windows.Media.Imaging;

#nullable disable
namespace ZENNER.CommonLibrary.Entities
{
  [Serializable]
  public sealed class EquipmentGroup : IConnectionItem
  {
    public Func<int, BitmapImage> PreLoadImage;

    public int EquipmentGroupID { get; set; }

    public int ID => this.EquipmentGroupID;

    public int GroupID => this.EquipmentGroupID;

    public string Name { get; set; }

    public string Description { get; set; }

    public BitmapImage Image500x500
    {
      get => this.PreLoadImage == null ? (BitmapImage) null : this.PreLoadImage(this.ImageID);
    }

    public int ImageID { get; set; }

    public override string ToString()
    {
      return string.IsNullOrEmpty(this.Name) ? base.ToString() : this.Name;
    }
  }
}
