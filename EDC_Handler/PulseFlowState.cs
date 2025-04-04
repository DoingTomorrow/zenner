// Decompiled with JetBrains decompiler
// Type: EDC_Handler.PulseFlowState
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using System;

#nullable disable
namespace EDC_Handler
{
  [Flags]
  public enum PulseFlowState : byte
  {
    BLOCK = 1,
    LEAK = 2,
    BACK = 4,
  }
}
