// Decompiled with JetBrains decompiler
// Type: WindowsInput.MOUSEKEYBDHARDWAREINPUT
// Assembly: InputSimulator, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 21845CD4-46CC-4FE2-BD83-49CF4563A54D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InputSimulator.dll

using System.Runtime.InteropServices;

#nullable disable
namespace WindowsInput
{
  [StructLayout(LayoutKind.Explicit)]
  internal struct MOUSEKEYBDHARDWAREINPUT
  {
    [FieldOffset(0)]
    public MOUSEINPUT Mouse;
    [FieldOffset(0)]
    public KEYBDINPUT Keyboard;
    [FieldOffset(0)]
    public HARDWAREINPUT Hardware;
  }
}
