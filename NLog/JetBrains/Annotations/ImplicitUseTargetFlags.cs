// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.ImplicitUseTargetFlags
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;

#nullable disable
namespace JetBrains.Annotations
{
  [Flags]
  internal enum ImplicitUseTargetFlags
  {
    Default = 1,
    Itself = Default, // 0x00000001
    Members = 2,
    WithMembers = Members | Itself, // 0x00000003
  }
}
