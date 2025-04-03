// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.Entities.DeviceModel
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

#nullable disable
namespace ZENNER.CommonLibrary.Entities
{
  [Serializable]
  public sealed class DeviceModel : IConnectionDeviceItem, IConnectionItem
  {
    public Func<int, BitmapImage> PreLoadImage;

    public int DeviceModelID { get; set; }

    public int ID => this.DeviceModelID;

    public int GroupID => this.DeviceGroup.DeviceGroupID;

    public string Name { get; set; }

    public string Description { get; set; }

    public string ProfileTypeName { get; set; }

    public BitmapImage Image500x500
    {
      get => this.PreLoadImage == null ? (BitmapImage) null : this.PreLoadImage(this.ImageID);
    }

    public int ImageID { get; set; }

    public DeviceGroup DeviceGroup { get; set; }

    public SortedList<ConnectionProfileParameter, string> Parameters { get; set; }

    public List<ChangeableParameter> ChangeableParameters { get; set; }

    public string Manufacturer { get; set; }

    public string Medium { get; set; }

    public string Generation { get; set; }

    public override string ToString()
    {
      if (string.IsNullOrEmpty(this.Name))
        return base.ToString();
      if (this.ChangeableParameters == null || this.ChangeableParameters.Count == 0)
        return this.Name;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this.Name).Append(" {");
      foreach (ChangeableParameter changeableParameter in this.ChangeableParameters)
        stringBuilder.Append(changeableParameter.Key).Append("=").Append(changeableParameter.Value).Append(", ");
      stringBuilder.Remove(stringBuilder.Length - 2, 2);
      stringBuilder.Append("}");
      return stringBuilder.ToString();
    }

    public DeviceModel DeepCopy()
    {
      List<ChangeableParameter> changeableParameterList = (List<ChangeableParameter>) null;
      if (this.ChangeableParameters != null && this.ChangeableParameters.Count > 0)
      {
        changeableParameterList = new List<ChangeableParameter>(this.ChangeableParameters.Count);
        foreach (ChangeableParameter changeableParameter in this.ChangeableParameters)
          changeableParameterList.Add(changeableParameter.DeepCopy());
      }
      return new DeviceModel()
      {
        ChangeableParameters = changeableParameterList,
        Description = this.Description,
        DeviceGroup = this.DeviceGroup,
        DeviceModelID = this.DeviceModelID,
        Generation = this.Generation,
        ImageID = this.ImageID,
        PreLoadImage = this.PreLoadImage,
        Manufacturer = this.Manufacturer,
        Medium = this.Medium,
        Name = this.Name,
        Parameters = this.Parameters,
        ProfileTypeName = this.ProfileTypeName
      };
    }
  }
}
