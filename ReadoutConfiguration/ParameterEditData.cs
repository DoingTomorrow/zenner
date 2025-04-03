// Decompiled with JetBrains decompiler
// Type: ReadoutConfiguration.ParameterEditData
// Assembly: ReadoutConfiguration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 1BD19DC4-A290-473A-8451-94ED3EF61361
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ReadoutConfiguration.dll

using System.ComponentModel;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace ReadoutConfiguration
{
  public class ParameterEditData : INotifyPropertyChanged
  {
    internal ParameterEditData.GroupFunctionChanged GroupFunctionChangedCall = (ParameterEditData.GroupFunctionChanged) null;
    private string selectedGroupFunction = ConnectionProfileFilterGroupFunctions.OR.ToString();

    public string SelectedGroup { get; set; }

    public string SelectedType { get; set; }

    public string ParameterValue { get; set; }

    public string SelectedGroupFunction
    {
      get => this.selectedGroupFunction;
      set
      {
        this.selectedGroupFunction = value;
        if (this.GroupFunctionChangedCall != null)
          this.GroupFunctionChangedCall(this);
        this.NotifyPropertyChanged(nameof (SelectedGroupFunction));
      }
    }

    public ParameterEditData(string selectedType, string parameterValue)
    {
      this.SelectedGroup = "0";
      this.SelectedType = selectedType;
      this.ParameterValue = parameterValue;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void NotifyPropertyChanged(string propName)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(propName));
    }

    public override string ToString()
    {
      return this.SelectedGroup + ";" + this.selectedGroupFunction + ";" + this.SelectedType + ";" + this.ParameterValue;
    }

    internal delegate void GroupFunctionChanged(ParameterEditData changedParam);
  }
}
