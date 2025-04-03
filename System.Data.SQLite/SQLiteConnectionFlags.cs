// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteConnectionFlags
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

#nullable disable
namespace System.Data.SQLite
{
  [Flags]
  public enum SQLiteConnectionFlags : long
  {
    None = 0,
    LogPrepare = 1,
    LogPreBind = 2,
    LogBind = 4,
    LogCallbackException = 8,
    LogBackup = 16, // 0x0000000000000010
    NoExtensionFunctions = 32, // 0x0000000000000020
    BindUInt32AsInt64 = 64, // 0x0000000000000040
    BindAllAsText = 128, // 0x0000000000000080
    GetAllAsText = 256, // 0x0000000000000100
    NoLoadExtension = 512, // 0x0000000000000200
    NoCreateModule = 1024, // 0x0000000000000400
    NoBindFunctions = 2048, // 0x0000000000000800
    NoLogModule = 4096, // 0x0000000000001000
    LogModuleError = 8192, // 0x0000000000002000
    LogModuleException = 16384, // 0x0000000000004000
    TraceWarning = 32768, // 0x0000000000008000
    ConvertInvariantText = 65536, // 0x0000000000010000
    BindInvariantText = 131072, // 0x0000000000020000
    NoConnectionPool = 262144, // 0x0000000000040000
    UseConnectionPool = 524288, // 0x0000000000080000
    UseConnectionTypes = 1048576, // 0x0000000000100000
    NoGlobalTypes = 2097152, // 0x0000000000200000
    StickyHasRows = 4194304, // 0x0000000000400000
    StrictEnlistment = 8388608, // 0x0000000000800000
    MapIsolationLevels = 16777216, // 0x0000000001000000
    DetectTextAffinity = 33554432, // 0x0000000002000000
    DetectStringType = 67108864, // 0x0000000004000000
    NoConvertSettings = 134217728, // 0x0000000008000000
    BindDateTimeWithKind = 268435456, // 0x0000000010000000
    RollbackOnException = 536870912, // 0x0000000020000000
    DenyOnException = 1073741824, // 0x0000000040000000
    InterruptOnException = 2147483648, // 0x0000000080000000
    UnbindFunctionsOnClose = 4294967296, // 0x0000000100000000
    NoVerifyTextAffinity = 8589934592, // 0x0000000200000000
    UseConnectionBindValueCallbacks = 17179869184, // 0x0000000400000000
    UseConnectionReadValueCallbacks = 34359738368, // 0x0000000800000000
    UseParameterNameForTypeName = 68719476736, // 0x0000001000000000
    UseParameterDbTypeForTypeName = 137438953472, // 0x0000002000000000
    NoVerifyTypeAffinity = 274877906944, // 0x0000004000000000
    BindAndGetAllAsText = GetAllAsText | BindAllAsText, // 0x0000000000000180
    ConvertAndBindInvariantText = BindInvariantText | ConvertInvariantText, // 0x0000000000030000
    BindAndGetAllAsInvariantText = BindAndGetAllAsText | BindInvariantText, // 0x0000000000020180
    ConvertAndBindAndGetAllAsInvariantText = BindAndGetAllAsInvariantText | ConvertInvariantText, // 0x0000000000030180
    UseConnectionAllValueCallbacks = UseConnectionReadValueCallbacks | UseConnectionBindValueCallbacks, // 0x0000000C00000000
    UseParameterAnythingForTypeName = UseParameterDbTypeForTypeName | UseParameterNameForTypeName, // 0x0000003000000000
    LogAll = LogModuleException | LogModuleError | LogBackup | LogCallbackException | LogBind | LogPreBind | LogPrepare, // 0x000000000000601F
    Default = LogModuleException | LogCallbackException, // 0x0000000000004008
    DefaultAndLogAll = Default | LogModuleError | LogBackup | LogBind | LogPreBind | LogPrepare, // 0x000000000000601F
  }
}
