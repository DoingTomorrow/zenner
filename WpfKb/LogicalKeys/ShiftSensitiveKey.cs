// Decompiled with JetBrains decompiler
// Type: WpfKb.LogicalKeys.ShiftSensitiveKey
// Assembly: WpfKb, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B294CC70-CB21-4202-BD7A-A4E6693370B9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\WpfKb.dll

using System.Collections.Generic;
using WindowsInput;

#nullable disable
namespace WpfKb.LogicalKeys
{
  public class ShiftSensitiveKey(VirtualKeyCode keyCode, IList<string> keyDisplays) : 
    MultiCharacterKey(keyCode, keyDisplays)
  {
  }
}
