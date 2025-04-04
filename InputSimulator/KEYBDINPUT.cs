// Decompiled with JetBrains decompiler
// Type: WindowsInput.KEYBDINPUT
// Assembly: InputSimulator, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 21845CD4-46CC-4FE2-BD83-49CF4563A54D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InputSimulator.dll

using System;

#nullable disable
namespace WindowsInput
{
  internal struct KEYBDINPUT
  {
    public ushort Vk;
    public ushort Scan;
    public uint Flags;
    public uint Time;
    public IntPtr ExtraInfo;
  }
}
