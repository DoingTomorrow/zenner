// Decompiled with JetBrains decompiler
// Type: WpfKb.LogicalKeys.MultiCharacterKey
// Assembly: WpfKb, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B294CC70-CB21-4202-BD7A-A4E6693370B9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\WpfKb.dll

using System;
using System.Collections.Generic;
using WindowsInput;

#nullable disable
namespace WpfKb.LogicalKeys
{
  public class MultiCharacterKey : VirtualKey
  {
    private int _selectedIndex;

    public IList<string> KeyDisplays { get; protected set; }

    public string SelectedKeyDisplay { get; protected set; }

    public int SelectedIndex
    {
      get => this._selectedIndex;
      set
      {
        if (value == this._selectedIndex)
          return;
        this._selectedIndex = value;
        this.SelectedKeyDisplay = this.KeyDisplays[value];
        this.DisplayName = this.SelectedKeyDisplay;
        this.OnPropertyChanged(nameof (SelectedIndex));
        this.OnPropertyChanged("SelectedKeyDisplay");
      }
    }

    public MultiCharacterKey(VirtualKeyCode keyCode, IList<string> keyDisplays)
      : base(keyCode)
    {
      if (keyDisplays == null)
        throw new ArgumentNullException(nameof (keyDisplays));
      this.KeyDisplays = keyDisplays.Count > 0 ? keyDisplays : throw new ArgumentException("Please provide a list of one or more keyDisplays", nameof (keyDisplays));
      this.DisplayName = keyDisplays[0];
    }
  }
}
