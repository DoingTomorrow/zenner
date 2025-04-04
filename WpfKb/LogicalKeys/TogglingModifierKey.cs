// Decompiled with JetBrains decompiler
// Type: WpfKb.LogicalKeys.TogglingModifierKey
// Assembly: WpfKb, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B294CC70-CB21-4202-BD7A-A4E6693370B9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\WpfKb.dll

using WindowsInput;

#nullable disable
namespace WpfKb.LogicalKeys
{
  public class TogglingModifierKey : ModifierKeyBase
  {
    public TogglingModifierKey(string displayName, VirtualKeyCode keyCode)
      : base(keyCode)
    {
      this.DisplayName = displayName;
    }

    public override void Press()
    {
      this.IsInEffect = !InputSimulator.IsTogglingKeyInEffect(this.KeyCode);
      base.Press();
    }

    public override void SynchroniseKeyState()
    {
      this.IsInEffect = InputSimulator.IsTogglingKeyInEffect(this.KeyCode);
    }
  }
}
