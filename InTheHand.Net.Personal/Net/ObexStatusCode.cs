// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.ObexStatusCode
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;

#nullable disable
namespace InTheHand.Net
{
  [Flags]
  public enum ObexStatusCode : byte
  {
    Final = 128, // 0x80
    Continue = 16, // 0x10
    OK = 32, // 0x20
    Created = 33, // 0x21
    Accepted = 34, // 0x22
    NonAuthorativeInformation = 35, // 0x23
    NoContent = 36, // 0x24
    ResetContent = 37, // 0x25
    PartialContent = 38, // 0x26
    MultipleChoices = OK | Continue, // 0x30
    MovedPermanently = 49, // 0x31
    MovedTemporarily = 50, // 0x32
    SeeOther = 51, // 0x33
    NotModified = 52, // 0x34
    UseProxy = 53, // 0x35
    BadRequest = 64, // 0x40
    Unauthorized = 65, // 0x41
    PaymentRequired = 66, // 0x42
    Forbidden = 67, // 0x43
    NotFound = 68, // 0x44
    MethodNotAllowed = 69, // 0x45
    NotAcceptable = 70, // 0x46
    ProxyAuthenticationRequired = 71, // 0x47
    RequestTimeout = 72, // 0x48
    Conflict = 73, // 0x49
    Gone = 74, // 0x4A
    LengthRequired = 75, // 0x4B
    PreconditionFailed = 76, // 0x4C
    RequestedEntityTooLarge = 77, // 0x4D
    RequestedUrlTooLarge = 78, // 0x4E
    UnsupportedMediaType = 79, // 0x4F
    InternalServerError = BadRequest | Continue, // 0x50
    NotImplemented = 81, // 0x51
    BadGateway = 82, // 0x52
    ServiceUnavailable = 83, // 0x53
    GatewayTimeout = 84, // 0x54
    HttpVersionNotSupported = 85, // 0x55
    DatabaseFull = BadRequest | OK, // 0x60
    DatabaseLocked = 97, // 0x61
  }
}
