// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.LicenseManagement.LicenseProblemTypeEnum
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

#nullable disable
namespace MSS.Business.Modules.LicenseManagement
{
  public enum LicenseProblemTypeEnum
  {
    CustomerNumberDoesNotExist,
    LicenseNotActiveYet,
    LicenseExpired,
    LicenseDoesNotExist,
    LicenseInvalidSignature,
    LicenseInvalidHardwareKey,
    LicenseInvalidCustomerNumber,
    LicenseAvailabilityOfflineExpired,
    IsNotServerLicense,
    IsMinoConnectNeeded,
  }
}
