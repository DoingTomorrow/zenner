// Decompiled with JetBrains decompiler
// Type: WindowsInput.MouseFlag
// Assembly: InputSimulator, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 21845CD4-46CC-4FE2-BD83-49CF4563A54D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InputSimulator.dll

#nullable disable
namespace WindowsInput
{
  public enum MouseFlag : uint
  {
    MOVE = 1,
    LEFTDOWN = 2,
    LEFTUP = 4,
    RIGHTDOWN = 8,
    RIGHTUP = 16, // 0x00000010
    MIDDLEDOWN = 32, // 0x00000020
    MIDDLEUP = 64, // 0x00000040
    XDOWN = 128, // 0x00000080
    XUP = 256, // 0x00000100
    WHEEL = 2048, // 0x00000800
    VIRTUALDESK = 16384, // 0x00004000
    ABSOLUTE = 32768, // 0x00008000
  }
}
