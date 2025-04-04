// Decompiled with JetBrains decompiler
// Type: MinomatHandler.SCGiErrorType
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

#nullable disable
namespace MinomatHandler
{
  public enum SCGiErrorType
  {
    UnknownResponce,
    AuthenticationFailed,
    UnknownMessageType,
    SystemTimeIsNotConfigured,
    DeviceIsNotInDeploymentPhase,
    TestReceptionAlreadyRunningOrWasExitWithoutCommit,
    TestReceptionWasNotStarted,
    DeviceIsNotInAppropriateMode,
    TestReceptionIsNotYetCompleted,
    Communication,
    InvalidResponce,
    InvalidSCGiPacket,
    ParseError,
    CanNotRegisterMessUnit,
    UnknownErrorOccured,
    WrongPhaseOrWrongDeviceType,
    NoSlaveRegistered,
    WrongDeviceType,
    InvalidNetworkState,
    SlaveAlreadyRegistered,
    NotUsed,
    MaxNumberOfSlavesReached,
    InvalidMinolID,
    IsNotMaster,
    NoResourceAvailable,
    WrongCRC,
    MissingMD5Parameter,
    UnknownMessUnit,
    ItIsNotPossableCurrently,
    InvalidTimeInterval,
    CanNotChangeScenario,
    SavingError,
    NoRestorePossible,
    NoSwitchPossible,
    NotChangeable,
    InvalidScenario,
    MeterAlreadyExists,
    MaxNumberOfRegisteredMeterReached,
    NoMemorySpaceInEepromAvailable,
  }
}
