// Decompiled with JetBrains decompiler
// Type: MSS.Utils.Controls.CheckableComboBox.CheckableComboBoxItem
// Assembly: MSS.Utils, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8365EDE-890D-4A42-AEA4-3B8FCE5E7B93
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Utils.dll

#nullable disable
namespace MSS.Utils.Controls.CheckableComboBox
{
  public class CheckableComboBoxItem : NotifyPropertyChanged
  {
    private string text;
    private bool isChecked;

    public bool IsChecked
    {
      get => this.isChecked;
      set
      {
        if (this.isChecked == value)
          return;
        this.isChecked = value;
        this.OnPropertyChanged(nameof (IsChecked));
      }
    }

    public string Text
    {
      get => this.text;
      set
      {
        if (!(this.text != value))
          return;
        this.text = value;
        this.OnPropertyChanged(nameof (Text));
      }
    }
  }
}
