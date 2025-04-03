// Decompiled with JetBrains decompiler
// Type: PDC_Handler.OverwritePart
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using System;

#nullable disable
namespace PDC_Handler
{
  [Flags]
  public enum OverwritePart : byte
  {
    TypeIdentification = 1,
    RadioSettings = 2,
    DeviceSettings = 4,
    Constants = 8,
  }
}
