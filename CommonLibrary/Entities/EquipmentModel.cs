// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.Entities.EquipmentModel
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
  public sealed class EquipmentModel : IConnectionDeviceItem, IConnectionItem
  {
    public Func<int, BitmapImage> PreLoadImage;

    public int EquipmentModelID { get; set; }

    public int ID => this.EquipmentModelID;

    public int GroupID => this.EquipmentGroup.EquipmentGroupID;

    public BitmapImage Image500x500
    {
      get => this.PreLoadImage == null ? (BitmapImage) null : this.PreLoadImage(this.ImageID);
    }

    public int ImageID { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public EquipmentGroup EquipmentGroup { get; set; }

    public SortedList<ConnectionProfileParameter, string> Parameters { get; set; }

    public List<ChangeableParameter> ChangeableParameters { get; set; }

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

    public EquipmentModel DeepCopy()
    {
      List<ChangeableParameter> changeableParameterList = (List<ChangeableParameter>) null;
      if (this.ChangeableParameters != null && this.ChangeableParameters.Count > 0)
      {
        changeableParameterList = new List<ChangeableParameter>(this.ChangeableParameters.Count);
        foreach (ChangeableParameter changeableParameter in this.ChangeableParameters)
          changeableParameterList.Add(changeableParameter.DeepCopy());
      }
      return new EquipmentModel()
      {
        ChangeableParameters = changeableParameterList,
        Description = this.Description,
        EquipmentGroup = this.EquipmentGroup,
        EquipmentModelID = this.EquipmentModelID,
        ImageID = this.ImageID,
        PreLoadImage = this.PreLoadImage,
        Name = this.Name,
        Parameters = this.Parameters
      };
    }

    public EquipmentModel Create(string port)
    {
      EquipmentModel equipmentModel = new EquipmentModel()
      {
        ChangeableParameters = new List<ChangeableParameter>()
      };
      equipmentModel.ChangeableParameters.Add(new ChangeableParameter()
      {
        Key = "Port",
        Value = port
      });
      equipmentModel.EquipmentModelID = 28;
      return equipmentModel;
    }
  }
}
