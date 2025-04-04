// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.UpgradeAction
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

#nullable disable
namespace ZR_ClassLibrary
{
  public enum UpgradeAction
  {
    VerifyDatabase,
    CreateCopyOfSetupDB,
    UpgradeFilter,
    UpgradeFilterValue,
    UpgradeGMM_User,
    UpgradeMeterType,
    UpgradeMeterInfo,
    UpgradeMTypeZelsius,
    UpgradeMeter,
    UpgradeNodeList,
    UpgradeNodeList_1_to_2,
    UpgradeSubdevices_1_to_2,
    UpgradeNodeLayers,
    UpgradeNodeReferences,
    UpgradeNodeReferences_1_to_2,
    UpgradeMeterValues,
    UpgradeMeterValues_1_to_2,
    BackupOldDatabase,
    UseSetupDatabase,
    UseWorkDatabase,
    MergeData,
    DeleteTempDatabase,
    UpgradeUserPermissions,
    UpgradeSoftwareUsers,
  }
}
