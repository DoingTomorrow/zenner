// Decompiled with JetBrains decompiler
// Type: WpfKb.LogicalKeys.ModifierKeyBase
// Assembly: WpfKb, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B294CC70-CB21-4202-BD7A-A4E6693370B9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\WpfKb.dll

using WindowsInput;

#nullable disable
namespace WpfKb.LogicalKeys
{
  public abstract class ModifierKeyBase(VirtualKeyCode keyCode) : VirtualKey(keyCode)
  {
    private bool _isInEffect;

    public bool IsInEffect
    {
      get => this._isInEffect;
      set
      {
        if (value == this._isInEffect)
          return;
        this._isInEffect = value;
        this.OnPropertyChanged(nameof (IsInEffect));
      }
    }

    public abstract void SynchroniseKeyState();
  }
}
