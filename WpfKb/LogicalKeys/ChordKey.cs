// Decompiled with JetBrains decompiler
// Type: WpfKb.LogicalKeys.ChordKey
// Assembly: WpfKb, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B294CC70-CB21-4202-BD7A-A4E6693370B9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\WpfKb.dll

using System.Collections.Generic;
using WindowsInput;

#nullable disable
namespace WpfKb.LogicalKeys
{
  public class ChordKey : LogicalKeyBase
  {
    public IList<VirtualKeyCode> ModifierKeys { get; private set; }

    public IList<VirtualKeyCode> Keys { get; private set; }

    public ChordKey(string displayName, VirtualKeyCode modifierKey, VirtualKeyCode key)
      : this(displayName, (IList<VirtualKeyCode>) new List<VirtualKeyCode>()
      {
        modifierKey
      }, (IList<VirtualKeyCode>) new List<VirtualKeyCode>()
      {
        key
      })
    {
    }

    public ChordKey(string displayName, IList<VirtualKeyCode> modifierKeys, VirtualKeyCode key)
      : this(displayName, modifierKeys, (IList<VirtualKeyCode>) new List<VirtualKeyCode>()
      {
        key
      })
    {
    }

    public ChordKey(string displayName, VirtualKeyCode modifierKey, IList<VirtualKeyCode> keys)
      : this(displayName, (IList<VirtualKeyCode>) new List<VirtualKeyCode>()
      {
        modifierKey
      }, keys)
    {
    }

    public ChordKey(
      string displayName,
      IList<VirtualKeyCode> modifierKeys,
      IList<VirtualKeyCode> keys)
    {
      this.DisplayName = displayName;
      this.ModifierKeys = modifierKeys;
      this.Keys = keys;
    }

    public override void Press()
    {
      InputSimulator.SimulateModifiedKeyStroke((IEnumerable<VirtualKeyCode>) this.ModifierKeys, (IEnumerable<VirtualKeyCode>) this.Keys);
      base.Press();
    }
  }
}
