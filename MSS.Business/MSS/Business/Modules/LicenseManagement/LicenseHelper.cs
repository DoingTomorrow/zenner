// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.LicenseManagement.LicenseHelper
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Errors;
using MSS.Business.Modules.UsersManagement;
using MSS.Business.Utils;
using MSS.Core.Model.TechnicalParameters;
using MSS.Core.Model.UsersManagement;
using MSS.Interfaces;
using MSSWeb.Common.Helpers;
using PlugInLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace MSS.Business.Modules.LicenseManagement
{
  public static class LicenseHelper
  {
    private static string HardwareKey = HardwareKeyGenerator.GetHardwareUniqueKey();
    private static string HardwareKeyBackwardsCompatible = HardwareKeyGenerator.GetHardwareUniqueKeyBackwardsCompatible();

    public static string ValidHardwareKey { get; private set; }

    private static LicenseInfo GetLicense(string hardwareKey)
    {
      LicenseInfo license = new LicenseInfo();
      string currentLicenseFileName = LicenseHelper.GetCurrentLicenseFileName(hardwareKey);
      if (!(currentLicenseFileName != string.Empty))
        return license;
      license.FullLicenseText = File.ReadAllText(currentLicenseFileName);
      try
      {
        license.SignedLicense = LicenseManager.LicensFromXML(license.FullLicenseText);
        return license;
      }
      catch (Exception ex)
      {
        MessageHandler.LogException(ex);
        return license;
      }
    }

    public static string GetCurrentLicenseFileName(string hardwareKey)
    {
      string[] files = Directory.GetFiles(AppDataFolderHelper.GetUserAppDataPath(), LicenseHelper.GetLicenseFileName(hardwareKey));
      return ((IEnumerable<string>) files).Any<string>() ? files[0] : string.Empty;
    }

    public static byte[] GetLicenseAsByteArray()
    {
      string currentLicenseFileName = LicenseHelper.GetCurrentLicenseFileName(LicenseHelper.GetValidHardwareKey());
      return currentLicenseFileName != string.Empty ? FileHelper.OpenFile(currentLicenseFileName).ToArray() : new byte[0];
    }

    public static bool LicenseExistsAndIsValid()
    {
      if (LicenseHelper.LicenseExistsAndIsValid(LicenseHelper.HardwareKey, MSS.Business.Utils.AppContext.Current.TechnicalParameters))
      {
        LicenseHelper.ValidHardwareKey = LicenseHelper.HardwareKey;
        return true;
      }
      if (!LicenseHelper.LicenseExistsAndIsValid(LicenseHelper.HardwareKeyBackwardsCompatible, MSS.Business.Utils.AppContext.Current.TechnicalParameters))
        return false;
      LicenseHelper.ValidHardwareKey = LicenseHelper.HardwareKeyBackwardsCompatible;
      return true;
    }

    private static bool LicenseExistsAndIsValid(
      string hardwareKey,
      TechnicalParameter technicalParameters)
    {
      return LicenseHelper.LicenseExistsAndIsValid(LicenseHelper.GetLicense(hardwareKey), hardwareKey, technicalParameters);
    }

    private static bool LicenseExistsAndIsValid(
      LicenseInfo licenseInfo,
      string hardwareKey,
      TechnicalParameter technicalParameters)
    {
      return licenseInfo.SignedLicense != null && licenseInfo.IsValid(technicalParameters, hardwareKey);
    }

    public static string GetValidHardwareKey()
    {
      if (string.IsNullOrEmpty(LicenseHelper.ValidHardwareKey) && !LicenseHelper.LicenseExistsAndIsValid())
        LicenseHelper.ValidHardwareKey = LicenseHelper.HardwareKey;
      return LicenseHelper.ValidHardwareKey;
    }

    public static string GetValidHardwareKeyForOpenFile(string fileName)
    {
      string str = "";
      string XMLString = File.ReadAllText(fileName);
      try
      {
        str = LicenseManager.LicensFromXML(XMLString).License.HardwareKey;
      }
      catch (Exception ex)
      {
        MessageHandler.LogException(ex);
      }
      if (str == LicenseHelper.HardwareKey)
        return LicenseHelper.HardwareKey;
      return str == LicenseHelper.HardwareKeyBackwardsCompatible ? LicenseHelper.HardwareKeyBackwardsCompatible : "";
    }

    public static string GetCurrentLicenseType(string hardwareKey)
    {
      return LicenseHelper.GetLicense(hardwareKey).SignedLicense.License.CurrentLicenseType;
    }

    public static string GetCurrentLicenseAboutText(string hardwareKey)
    {
      return LicenseHelper.GetLicense(hardwareKey).SignedLicense.License.AboutText;
    }

    private static bool LicenseExists(string hardwareKey, out LicenseInfo licenseInfo)
    {
      licenseInfo = LicenseHelper.GetLicense(hardwareKey);
      return licenseInfo.SignedLicense != null;
    }

    private static bool LicenseExists(this LicenseInfo licenseInfo)
    {
      return licenseInfo != null && licenseInfo.SignedLicense != null;
    }

    private static bool IsMinoConnectNeededAndIsNotConnected(this LicenseInfo licenseInfo)
    {
      return licenseInfo != null && licenseInfo.SignedLicense.License.IsMinoConnectedNeeded && !MSS.Business.Utils.AppContext.Current.IsMinoConnectConnected;
    }

    public static bool IsMinoConnectNeeded(string hardwareKey)
    {
      return LicenseHelper.GetLicense(hardwareKey).SignedLicense.License.IsMinoConnectedNeeded;
    }

    private static bool LicenseIsNotActiveYet(this LicenseInfo licenseInfo)
    {
      return licenseInfo != null && licenseInfo.IsNotActiveYet();
    }

    private static bool LicenseIsExpired(this LicenseInfo licenseInfo)
    {
      return licenseInfo != null && licenseInfo.IsExpired();
    }

    private static bool IsServerLicense(this LicenseInfo licenseInfo, string hardwareKey)
    {
      return licenseInfo != null && licenseInfo.SignedLicense.License.IsServerLicense;
    }

    public static bool IsStandaloneClientLicense(string hardwareKey)
    {
      return LicenseHelper.GetLicense(hardwareKey).SignedLicense.License.IsStandaloneClient;
    }

    private static bool LicenseAvailabilityOfflineExpired(
      this LicenseInfo licenseInfo,
      TechnicalParameter technicalParameter)
    {
      return licenseInfo != null && LicenseHelper.DaysSinceTheLicenseIsUsedOffline(licenseInfo.SignedLicense, technicalParameter) > licenseInfo.SignedLicense.License.ServerConnectionInterval + licenseInfo.SignedLicense.License.ServerConnectionTolerance;
    }

    private static bool LicenseHasValidSignature(this LicenseInfo licenseInfo)
    {
      return licenseInfo != null && licenseInfo.FullLicenseText != string.Empty && LicenseManager.VerifySignature(licenseInfo.FullLicenseText, licenseInfo.SignedLicense);
    }

    private static bool IsHardwareValid(this LicenseInfo licenseInfo, string hardwareKey)
    {
      return licenseInfo != null && licenseInfo.SignedLicense.License.HardwareKey == hardwareKey;
    }

    private static bool IsCustomerNumberValid(
      this LicenseInfo licenseInfo,
      TechnicalParameter technicalParameters)
    {
      return licenseInfo != null && licenseInfo.SignedLicense.License.Customer == technicalParameters.CustomerNumber;
    }

    private static bool IsValid(
      this LicenseInfo licenseInfo,
      TechnicalParameter technicalParameters,
      string hardwareKey)
    {
      return LicenseHelper.IsCustomerNumberFilled(technicalParameters) && licenseInfo.LicenseExists() && licenseInfo.LicenseHasValidSignature() && licenseInfo.IsHardwareValid(hardwareKey) && licenseInfo.IsCustomerNumberValid(technicalParameters) && !licenseInfo.LicenseIsExpired() && !licenseInfo.LicenseIsNotActiveYet();
    }

    private static bool IsNotActiveYet(this LicenseInfo licenseInfo)
    {
      bool flag = true;
      if (licenseInfo.SignedLicense.GetStartDate() > DateTime.Today)
        flag = false;
      return !flag;
    }

    private static bool IsExpired(this LicenseInfo licenseInfo)
    {
      bool flag = true;
      DateTime? endDate = licenseInfo.SignedLicense.GetEndDate();
      if (endDate.HasValue)
      {
        DateTime? nullable = endDate;
        DateTime today = DateTime.Today;
        if (nullable.HasValue && nullable.GetValueOrDefault() < today)
          flag = false;
      }
      return !flag;
    }

    public static bool IsCustomerNumberFilled(TechnicalParameter technicalParameters)
    {
      return !string.IsNullOrEmpty(technicalParameters.CustomerNumber);
    }

    public static LicenseNotification ShouldNotifyUserAboutOfflineValidity(
      IRepository<TechnicalParameter> technicalParametersRepository,
      TechnicalParameter technicalParameter,
      string hardwareKey)
    {
      LicenseInfo license = LicenseHelper.GetLicense(hardwareKey);
      int num = LicenseHelper.DaysSinceTheLicenseIsUsedOffline(license.SignedLicense, technicalParameter);
      if (num >= license.SignedLicense.License.ServerConnectionInterval && num < license.SignedLicense.License.ServerConnectionInterval + license.SignedLicense.License.ServerConnectionTolerance)
        return new LicenseNotification()
        {
          ShowNotification = true,
          NumberOfDays = license.SignedLicense.License.ServerConnectionInterval + license.SignedLicense.License.ServerConnectionTolerance - num
        };
      return new LicenseNotification()
      {
        ShowNotification = false
      };
    }

    private static int DaysSinceTheLicenseIsUsedOffline(
      SignedLicense signedLicense,
      TechnicalParameter technicalParameter)
    {
      return !technicalParameter.LastConnectionToLicenseServer.HasValue ? DateTime.Today.Subtract(signedLicense.License.ValidityStartDate.Value).Days : DateTime.Today.Subtract(technicalParameter.LastConnectionToLicenseServer.Value).Days;
    }

    public static int DaysSinceTheLicenseIsUsedOffline(TechnicalParameter technicalParameter)
    {
      return LicenseHelper.DaysSinceTheLicenseIsUsedOffline(LicenseHelper.GetLicense(LicenseHelper.GetValidHardwareKey()).SignedLicense, technicalParameter);
    }

    private static DateTime GetStartDate(this SignedLicense signedLicense)
    {
      return signedLicense.License.ValidityStartDate.Value;
    }

    private static DateTime? GetEndDate(this SignedLicense signedLicense)
    {
      int result;
      return !string.IsNullOrEmpty(signedLicense.License.ValidityPeriod) && int.TryParse(signedLicense.License.ValidityPeriod, out result) ? new DateTime?(signedLicense.License.ValidityStartDate.Value.AddMonths(result)) : new DateTime?();
    }

    public static IEnumerable<string> GetOperations(
      User loggedUser,
      List<Operation> operationsForRole)
    {
      List<string> operations = new List<string>();
      if (!LicenseHelper.LicenseExistsAndIsValid() || loggedUser == null)
        return (IEnumerable<string>) operations;
      LicenseInfo licenseInfo = LicenseHelper.GetLicense(LicenseHelper.GetValidHardwareKey());
      operations.AddRange(operationsForRole.Where<Operation>((Func<Operation, bool>) (operation => licenseInfo.SignedLicense.License.Rights.Count<RightInfo>((Func<RightInfo, bool>) (right => right.Right.Substring(right.Right.LastIndexOf("\\") + 1) == operation.Name && right.Enable)) > 0)).Select<Operation, string>((Func<Operation, string>) (operation => operation.Name)));
      return (IEnumerable<string>) operations;
    }

    public static IEnumerable<string> GetOperationsInLicense(TechnicalParameter technicalParameter)
    {
      List<string> operationsInLicense = new List<string>();
      string validHardwareKey = LicenseHelper.GetValidHardwareKey();
      if (!LicenseHelper.LicenseExistsAndIsValid(validHardwareKey, technicalParameter))
        return (IEnumerable<string>) operationsInLicense;
      LicenseInfo license = LicenseHelper.GetLicense(validHardwareKey);
      operationsInLicense.AddRange(license.SignedLicense.License.Rights.Select<RightInfo, string>((Func<RightInfo, string>) (operation => operation.Right)));
      return (IEnumerable<string>) operationsInLicense;
    }

    public static byte[] GetLogoImage()
    {
      string validHardwareKey = LicenseHelper.GetValidHardwareKey();
      LicenseInfo license = LicenseHelper.GetLicense(validHardwareKey);
      return !LicenseHelper.LicenseExistsAndIsValid(license, validHardwareKey, MSS.Business.Utils.AppContext.Current.TechnicalParameters) ? (byte[]) null : license.SignedLicense.License.LogoImage;
    }

    public static IEnumerable<string> GetDeviceTypes()
    {
      List<string> deviceTypes = new List<string>();
      string validHardwareKey = LicenseHelper.GetValidHardwareKey();
      LicenseInfo license = LicenseHelper.GetLicense(validHardwareKey);
      if (!LicenseHelper.LicenseExistsAndIsValid(license, validHardwareKey, MSS.Business.Utils.AppContext.Current.TechnicalParameters))
        return (IEnumerable<string>) deviceTypes;
      deviceTypes.AddRange(license.SignedLicense.License.DeviceTypes.Where<DeviceTypeInfo>((Func<DeviceTypeInfo, bool>) (deviceType => deviceType.Enable)).Select<DeviceTypeInfo, string>((Func<DeviceTypeInfo, string>) (deviceType => deviceType.DeviceType)));
      return (IEnumerable<string>) deviceTypes;
    }

    public static IEnumerable<string> GetOperations(
      User loggedUser,
      IRepositoryFactory repositoryFactory)
    {
      List<Operation> operations = repositoryFactory.GetUserRepository().GetOperations(MSS.Business.Utils.AppContext.Current.LoggedUser);
      return LicenseHelper.GetOperations(loggedUser, operations);
    }

    public static LicenseProblemTypeEnum? DetectLicenseProblemForServer(
      TechnicalParameter technicalParameter)
    {
      string validHardwareKey = LicenseHelper.GetValidHardwareKey();
      LicenseProblemTypeEnum? nullable = new LicenseProblemTypeEnum?();
      LicenseInfo licenseInfo = (LicenseInfo) null;
      if (!LicenseHelper.IsCustomerNumberFilled(technicalParameter))
        nullable = new LicenseProblemTypeEnum?(LicenseProblemTypeEnum.CustomerNumberDoesNotExist);
      else if (!LicenseHelper.LicenseExists(validHardwareKey, out licenseInfo))
        nullable = new LicenseProblemTypeEnum?(LicenseProblemTypeEnum.LicenseDoesNotExist);
      else if (!licenseInfo.IsServerLicense(validHardwareKey))
        nullable = new LicenseProblemTypeEnum?(LicenseProblemTypeEnum.IsNotServerLicense);
      else if (!licenseInfo.LicenseHasValidSignature())
        nullable = new LicenseProblemTypeEnum?(LicenseProblemTypeEnum.LicenseInvalidSignature);
      else if (!licenseInfo.IsHardwareValid(validHardwareKey))
        nullable = new LicenseProblemTypeEnum?(LicenseProblemTypeEnum.LicenseInvalidHardwareKey);
      else if (!licenseInfo.IsCustomerNumberValid(technicalParameter))
        nullable = new LicenseProblemTypeEnum?(LicenseProblemTypeEnum.LicenseInvalidCustomerNumber);
      else if (licenseInfo.LicenseIsNotActiveYet())
        nullable = new LicenseProblemTypeEnum?(LicenseProblemTypeEnum.LicenseNotActiveYet);
      else if (licenseInfo.LicenseIsExpired())
        nullable = new LicenseProblemTypeEnum?(LicenseProblemTypeEnum.LicenseExpired);
      else if (licenseInfo.LicenseAvailabilityOfflineExpired(technicalParameter))
        nullable = new LicenseProblemTypeEnum?(LicenseProblemTypeEnum.LicenseAvailabilityOfflineExpired);
      return nullable;
    }

    public static LicenseProblemTypeEnum? DetectLicenseProblemClient(
      TechnicalParameter technicalParameter)
    {
      string validHardwareKey = LicenseHelper.GetValidHardwareKey();
      LicenseProblemTypeEnum? nullable = new LicenseProblemTypeEnum?();
      LicenseInfo licenseInfo = (LicenseInfo) null;
      if (!LicenseHelper.IsCustomerNumberFilled(technicalParameter))
        nullable = new LicenseProblemTypeEnum?(LicenseProblemTypeEnum.CustomerNumberDoesNotExist);
      else if (!LicenseHelper.LicenseExists(validHardwareKey, out licenseInfo))
        nullable = new LicenseProblemTypeEnum?(LicenseProblemTypeEnum.LicenseDoesNotExist);
      else if (!licenseInfo.LicenseHasValidSignature())
        nullable = new LicenseProblemTypeEnum?(LicenseProblemTypeEnum.LicenseInvalidSignature);
      else if (!licenseInfo.IsHardwareValid(validHardwareKey))
        nullable = new LicenseProblemTypeEnum?(LicenseProblemTypeEnum.LicenseInvalidHardwareKey);
      else if (!licenseInfo.IsCustomerNumberValid(technicalParameter))
        nullable = new LicenseProblemTypeEnum?(LicenseProblemTypeEnum.LicenseInvalidCustomerNumber);
      else if (licenseInfo.IsMinoConnectNeededAndIsNotConnected())
        nullable = new LicenseProblemTypeEnum?(LicenseProblemTypeEnum.IsMinoConnectNeeded);
      else if (licenseInfo.LicenseIsNotActiveYet())
        nullable = new LicenseProblemTypeEnum?(LicenseProblemTypeEnum.LicenseNotActiveYet);
      else if (licenseInfo.LicenseIsExpired())
        nullable = new LicenseProblemTypeEnum?(LicenseProblemTypeEnum.LicenseExpired);
      else if (licenseInfo.LicenseAvailabilityOfflineExpired(technicalParameter))
        nullable = new LicenseProblemTypeEnum?(LicenseProblemTypeEnum.LicenseAvailabilityOfflineExpired);
      return nullable;
    }

    public static LicenseNotification ShouldNotifyUserAboutLicenseExpire(
      IRepository<TechnicalParameter> technicalParametersRepository,
      TechnicalParameter technicalParameter,
      string hardwareKey)
    {
      LicenseInfo license = LicenseHelper.GetLicense(hardwareKey);
      int num1;
      if (technicalParameter.LastLicenseUniqueId.HasValue)
      {
        Guid? lastLicenseUniqueId = technicalParameter.LastLicenseUniqueId;
        Guid id = license.SignedLicense.License.Id;
        num1 = lastLicenseUniqueId.HasValue ? (lastLicenseUniqueId.HasValue ? (lastLicenseUniqueId.GetValueOrDefault() != id ? 1 : 0) : 0) : 1;
      }
      else
        num1 = 1;
      if (num1 != 0)
      {
        technicalParameter.LastReminderForLicenseExpire = new DateTime?();
        technicalParameter.LastLicenseUniqueId = new Guid?(license.SignedLicense.License.Id);
        technicalParametersRepository.Update(technicalParameter);
      }
      DateTime? endDate = license.SignedLicense.GetEndDate();
      if (endDate.HasValue)
      {
        int days = endDate.Value.Subtract(DateTime.Today).Days;
        if (days <= technicalParameter.DaysNotifyUserAboutLicenseExpire)
        {
          DateTime? forLicenseExpire = technicalParameter.LastReminderForLicenseExpire;
          int num2;
          if (forLicenseExpire.HasValue)
          {
            DateTime today1 = DateTime.Today;
            ref DateTime local1 = ref today1;
            forLicenseExpire = technicalParameter.LastReminderForLicenseExpire;
            DateTime dateTime1 = forLicenseExpire.Value;
            if (local1.Subtract(dateTime1).Days <= technicalParameter.IntervalNotifyUserAboutLicenseExpire)
            {
              DateTime today2 = DateTime.Today;
              ref DateTime local2 = ref today2;
              forLicenseExpire = technicalParameter.LastReminderForLicenseExpire;
              DateTime dateTime2 = forLicenseExpire.Value;
              num2 = local2.Subtract(dateTime2).Days > 0 ? 1 : 0;
            }
            else
              num2 = 0;
          }
          else
            num2 = 1;
          if (num2 != 0)
            return new LicenseNotification()
            {
              ShowNotification = true,
              NumberOfDays = days
            };
        }
      }
      return new LicenseNotification()
      {
        ShowNotification = false
      };
    }

    public static string GetLicenseFileName(string hardwareKey)
    {
      return MLSHelper.GetLicenseFileName(hardwareKey);
    }

    public static bool LicenseIsDisplayEvaluationFactor()
    {
      LicenseInfo license = LicenseHelper.GetLicense(LicenseHelper.GetValidHardwareKey());
      return license.SignedLicense != null && license.SignedLicense.License.DisplayEvaluationFactor;
    }

    public static bool HasRightOnLicense(string hardwareKey, OperationEnum operation)
    {
      return LicenseHelper.GetLicense(hardwareKey).SignedLicense.License.Rights.Count<RightInfo>((Func<RightInfo, bool>) (right => right.Right.Substring(right.Right.LastIndexOf("\\") + 1) == operation.ToString() && right.Enable)) > 0;
    }
  }
}
