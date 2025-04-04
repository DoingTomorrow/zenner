// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.S4_ChecksumCheckResults
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using System;

#nullable disable
namespace S4_Handler.Functions
{
  [Flags]
  public enum S4_ChecksumCheckResults : ushort
  {
    IdentityChecksumNotOk = 1,
    ParameterChecksumNotOk = 2,
    FirmwareMetrologicalPartNotOk = 4,
    FirmwareCommunicationPartNotOk = 8,
    FirmwareCompleteNotOk = 16, // 0x0010
  }
}
