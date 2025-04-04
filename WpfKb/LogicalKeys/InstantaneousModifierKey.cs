// Decompiled with JetBrains decompiler
// Type: WpfKb.LogicalKeys.InstantaneousModifierKey
// Assembly: WpfKb, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B294CC70-CB21-4202-BD7A-A4E6693370B9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\WpfKb.dll

using WindowsInput;

#nullable disable
namespace WpfKb.LogicalKeys
{
  public class InstantaneousModifierKey : ModifierKeyBase
  {
    public InstantaneousModifierKey(string displayName, VirtualKeyCode keyCode)
      : base(keyCode)
    {
      this.DisplayName = displayName;
    }

    public override void Press()
    {
      if (this.IsInEffect)
        InputSimulator.SimulateKeyUp(this.KeyCode);
      else
        InputSimulator.SimulateKeyDown(this.KeyCode);
      this.IsInEffect = InputSimulator.IsKeyDownAsync(this.KeyCode);
      this.OnKeyPressed();
    }

    public override void SynchroniseKeyState()
    {
      this.IsInEffect = InputSimulator.IsKeyDownAsync(this.KeyCode);
    }
  }
}
