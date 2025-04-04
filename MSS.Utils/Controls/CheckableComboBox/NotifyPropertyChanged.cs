// Decompiled with JetBrains decompiler
// Type: MSS.Utils.Controls.CheckableComboBox.NotifyPropertyChanged
// Assembly: MSS.Utils, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8365EDE-890D-4A42-AEA4-3B8FCE5E7B93
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Utils.dll

using System.ComponentModel;

#nullable disable
namespace MSS.Utils.Controls.CheckableComboBox
{
  public class NotifyPropertyChanged : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
