// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.ObexHeader
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

#nullable disable
namespace InTheHand.Net
{
  internal enum ObexHeader : byte
  {
    None = 0,
    Name = 1,
    Description = 5,
    Type = 66, // 0x42
    TimeIso8601 = 68, // 0x44
    Target = 70, // 0x46
    Http = 71, // 0x47
    Body = 72, // 0x48
    EndOfBody = 73, // 0x49
    Who = 74, // 0x4A
    ApplicationParameter = 76, // 0x4C
    AuthenticationChallenge = 77, // 0x4D
    AuthenticationResponse = 78, // 0x4E
    ObjectClass = 79, // 0x4F
    WanUuid = 80, // 0x50
    SessionParamters = 82, // 0x52
    SessionSequenceNumber = 147, // 0x93
    Count = 192, // 0xC0
    Length = 195, // 0xC3
    Time4Byte = 196, // 0xC4
    ConnectionID = 203, // 0xCB
    CreatorID = 207, // 0xCF
  }
}
