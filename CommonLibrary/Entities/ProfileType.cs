// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.Entities.ProfileType
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

#nullable disable
namespace ZENNER.CommonLibrary.Entities
{
  [Serializable]
  public sealed class ProfileType : IConnectionDeviceItem, IConnectionItem
  {
    public Func<int, BitmapImage> PreLoadImage;

    public int ProfileTypeID { get; set; }

    public int ID => this.ProfileTypeID;

    public int GroupID => this.ProfileTypeGroup.ProfileTypeGroupID;

    public BitmapImage Image500x500
    {
      get => this.PreLoadImage == null ? (BitmapImage) null : this.PreLoadImage(this.ImageID);
    }

    public int ImageID { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public ProfileTypeGroup ProfileTypeGroup { get; set; }

    public SortedList<ConnectionProfileParameter, string> Parameters { get; set; }

    public List<ChangeableParameter> ChangeableParameters { get; set; }

    public override string ToString()
    {
      return string.IsNullOrEmpty(this.Name) ? base.ToString() : this.Name;
    }

    public ProfileType DeepCopy()
    {
      List<ChangeableParameter> changeableParameterList = (List<ChangeableParameter>) null;
      if (this.ChangeableParameters != null && this.ChangeableParameters.Count > 0)
      {
        changeableParameterList = new List<ChangeableParameter>(this.ChangeableParameters.Count);
        foreach (ChangeableParameter changeableParameter in this.ChangeableParameters)
          changeableParameterList.Add(changeableParameter.DeepCopy());
      }
      return new ProfileType()
      {
        ChangeableParameters = changeableParameterList,
        Description = this.Description,
        ProfileTypeGroup = this.ProfileTypeGroup,
        ProfileTypeID = this.ProfileTypeID,
        ImageID = this.ImageID,
        PreLoadImage = this.PreLoadImage,
        Name = this.Name,
        Parameters = this.Parameters
      };
    }

    public ProfileType Create(string name)
    {
      return new ProfileType() { Name = name };
    }
  }
}
