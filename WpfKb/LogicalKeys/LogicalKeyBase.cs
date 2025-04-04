// Decompiled with JetBrains decompiler
// Type: WpfKb.LogicalKeys.LogicalKeyBase
// Assembly: WpfKb, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B294CC70-CB21-4202-BD7A-A4E6693370B9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\WpfKb.dll

using System.ComponentModel;
using System.Windows;
using WindowsInput;
using WpfKb.Controls;

#nullable disable
namespace WpfKb.LogicalKeys
{
  public abstract class LogicalKeyBase : ILogicalKey, INotifyPropertyChanged
  {
    private string _displayName;

    public event LogicalKeyPressedEventHandler LogicalKeyPressed;

    public event PropertyChangedEventHandler PropertyChanged;

    public virtual string DisplayName
    {
      get => this._displayName;
      set
      {
        if (!(value != this._displayName))
          return;
        this._displayName = value;
        this.OnPropertyChanged(nameof (DisplayName));
      }
    }

    public virtual void Press() => this.OnKeyPressed();

    protected void OnKeyPressed()
    {
      if (this.LogicalKeyPressed == null)
        return;
      if (this.GetType() == typeof (VirtualKey) && ((VirtualKey) this).KeyCode == VirtualKeyCode.MODECHANGE && ((FrameworkElement) this.LogicalKeyPressed.Target).DataContext is TouchScreenKeyboardUserControl dataContext)
        dataContext.KeyBoardInputType = dataContext.KeyBoardInputType == "Numeric" ? "AlphaNumeric" : "Numeric";
      this.LogicalKeyPressed((object) this, new LogicalKeyEventArgs((ILogicalKey) this));
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
      propertyChanged((object) this, e);
    }
  }
}
