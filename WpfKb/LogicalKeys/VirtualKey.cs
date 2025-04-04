// Decompiled with JetBrains decompiler
// Type: WpfKb.LogicalKeys.VirtualKey
// Assembly: WpfKb, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B294CC70-CB21-4202-BD7A-A4E6693370B9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\WpfKb.dll

using WindowsInput;

#nullable disable
namespace WpfKb.LogicalKeys
{
  public class VirtualKey : LogicalKeyBase
  {
    private VirtualKeyCode _keyCode;

    public virtual VirtualKeyCode KeyCode
    {
      get => this._keyCode;
      set
      {
        if (value == this._keyCode)
          return;
        this._keyCode = value;
        this.OnPropertyChanged(nameof (KeyCode));
      }
    }

    public VirtualKey(VirtualKeyCode keyCode, string displayName)
    {
      this.DisplayName = displayName;
      this.KeyCode = keyCode;
    }

    public VirtualKey(VirtualKeyCode keyCode) => this.KeyCode = keyCode;

    public VirtualKey()
    {
    }

    public override void Press()
    {
      if (this._keyCode != VirtualKeyCode.MODECHANGE)
      {
        InputSimulator.SimulateKeyPress(this._keyCode);
        base.Press();
      }
      else
        base.Press();
    }
  }
}
