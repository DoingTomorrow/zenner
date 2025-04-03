// Decompiled with JetBrains decompiler
// Type: MSS_Client.Utils.MSSUIHelper
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using CA.Cryptography;
using Common.Library.NHibernate.Data;
using MSS.Business.Errors;
using MSS.Business.Languages;
using MSS.Business.Modules.GMM;
using MSS.Business.Modules.LicenseManagement;
using MSS.Business.Modules.UsersManagement;
using MSS.Business.Utils;
using MSS.Core.Model.Jobs;
using MSS.Core.Model.TechnicalParameters;
using MSS.Core.Model.UsersManagement;
using MSS.DIConfiguration;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MSS_Client.Properties;
using MSS_Client.ViewModel;
using MSS_Client.ViewModel.ExceptionMessageBox;
using MSS_Client.ViewModel.Startup;
using MSSWeb.Common.Helpers;
using NHibernate;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Threading;
using ZENNER;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS_Client.Utils
{
  public static class MSSUIHelper
  {
    private static void ShowLoginOrMainWindow(IWindowFactory windowFactory)
    {
      if (MSS.Business.Utils.AppContext.Current.LoggedUser == null)
      {
        if (MSSUIHelper.IsUserAllowedToLogin())
        {
          MSSLoginWindowViewModel loginWindowViewModel = DIConfigurator.GetConfigurator().Get<MSSLoginWindowViewModel>();
          windowFactory.CreateNewNonModalDialog((IViewModel) loginWindowViewModel);
        }
        else
          MSSUIHelper.ShowWarningDialog(windowFactory, MessageCodes.Warning.GetStringValue(), MSS.Localisation.Resources.MSS_WrongPassword, false);
      }
      else
      {
        MSSViewModel mssViewModel = DIConfigurator.GetConfigurator().Get<MSSViewModel>((IParameter) new ConstructorArgument("logoImage", (object) LicenseHelper.GetLogoImage()));
        windowFactory.CreateNewNonModalDialog((IViewModel) mssViewModel);
      }
    }

    private static bool ShowLicenseWindow(IWindowFactory windowFactory)
    {
      LicenseProblemTypeEnum? nullable = LicenseHelper.DetectLicenseProblemClient(MSS.Business.Utils.AppContext.Current.TechnicalParameters);
      if (!nullable.HasValue)
        return false;
      MSSLicenseCustomerWindowViewModel customerWindowViewModel = DIConfigurator.GetConfigurator().Get<MSSLicenseCustomerWindowViewModel>((IParameter) new ConstructorArgument("licenseProblemType", (object) nullable.Value));
      windowFactory.CreateNewNonModalDialog((IViewModel) customerWindowViewModel);
      return true;
    }

    public static void ShowErrorWindow(IWindowFactory windowFactory, string errorMessage)
    {
      ExceptionMessageBoxViewModel messageBoxViewModel = DIConfigurator.GetConfigurator().Get<ExceptionMessageBoxViewModel>((IParameter) new ConstructorArgument(nameof (errorMessage), (object) CultureResources.GetValue(errorMessage)), (IParameter) new ConstructorArgument("isUnhandledException", (object) true));
      windowFactory.CreateNewNonModalDialog((IViewModel) messageBoxViewModel);
    }

    private static bool ShowLicenseCustomerWindow(IWindowFactory windowFactory)
    {
      if (LicenseHelper.IsCustomerNumberFilled(MSS.Business.Utils.AppContext.Current.TechnicalParameters))
        return false;
      MSSLicenseCustomerWindowViewModel customerWindowViewModel = DIConfigurator.GetConfigurator().Get<MSSLicenseCustomerWindowViewModel>();
      windowFactory.CreateNewNonModalDialog((IViewModel) customerWindowViewModel);
      return true;
    }

    public static MSSSplashScreenViewModel ShowSplashScreen(IWindowFactory _windowFactory)
    {
      MSSSplashScreenViewModel splashScreenViewModel = DIConfigurator.GetConfigurator().Get<MSSSplashScreenViewModel>();
      _windowFactory.CreateNewNonModalDialog((IViewModel) splashScreenViewModel);
      return splashScreenViewModel;
    }

    public static void ShowApplicationStartWindow(
      IRepositoryFactory _repositoryFactory,
      IWindowFactory _windowFactory)
    {
      if (MSSUIHelper.ShowLicenseWindow(_windowFactory))
        return;
      MSSUIHelper.ShowLoginOrMainWindow(_windowFactory);
    }

    public static void CreateDefaultUser()
    {
      IRepositoryFactory repositoryFactory = DIConfigurator.GetConfigurator().Get<IRepositoryFactory>();
      repositoryFactory.GetSession().BeginTransaction();
      try
      {
        User localUser = repositoryFactory.GetRepository<User>().FirstOrDefault((Expression<Func<User, bool>>) (x => x.Username == "default" + LicenseHelper.GetValidHardwareKey()));
        Role role = repositoryFactory.GetRepository<Role>().FirstOrDefault((Expression<Func<Role, bool>>) (x => x.Name == "default" + LicenseHelper.GetValidHardwareKey()));
        if (localUser == null)
        {
          string hashAndSalt;
          new PasswordManager().GetPasswordHashAndSaltConcatenatedString("", out hashAndSalt);
          localUser = new User()
          {
            FirstName = "Default",
            LastName = "Default",
            Username = "default" + LicenseHelper.GetValidHardwareKey(),
            Password = hashAndSalt,
            Language = LangEnum.English.ToString()
          };
          repositoryFactory.GetRepository<User>().TransactionalInsert(localUser);
          if (role == null)
          {
            role = new Role()
            {
              Name = "default" + LicenseHelper.GetValidHardwareKey(),
              IsStandard = true
            };
            repositoryFactory.GetRepository<Role>().TransactionalInsert(role);
            foreach (object obj in Enum.GetValues(typeof (OperationEnum)))
            {
              object op = obj;
              Operation operation = repositoryFactory.GetRepository<Operation>().FirstOrDefault((Expression<Func<Operation, bool>>) (o => o.Name == op.ToString()));
              if (operation == null)
              {
                Operation entity1 = new Operation()
                {
                  Name = op.ToString()
                };
                repositoryFactory.GetRepository<Operation>().TransactionalInsert(entity1);
                RoleOperation entity2 = new RoleOperation()
                {
                  Operation = entity1,
                  Role = role
                };
                repositoryFactory.GetRepository<RoleOperation>().TransactionalInsert(entity2);
              }
              else
              {
                RoleOperation entity = new RoleOperation()
                {
                  Operation = operation,
                  Role = role
                };
                repositoryFactory.GetRepository<RoleOperation>().TransactionalInsert(entity);
              }
            }
          }
        }
        if (role == null)
        {
          role = new Role()
          {
            Name = "default" + LicenseHelper.GetValidHardwareKey(),
            IsStandard = true
          };
          repositoryFactory.GetRepository<Role>().TransactionalInsert(role);
          if (repositoryFactory.GetRepository<UserRole>().FirstOrDefault((Expression<Func<UserRole, bool>>) (x => x.Role.Id == role.Id && x.User.Id == localUser.Id)) == null)
          {
            UserRole entity = new UserRole()
            {
              Role = role,
              User = localUser
            };
            repositoryFactory.GetRepository<UserRole>().TransactionalInsert(entity);
          }
          foreach (object obj in Enum.GetValues(typeof (OperationEnum)))
          {
            Operation entity3 = new Operation()
            {
              Name = obj.ToString()
            };
            repositoryFactory.GetRepository<Operation>().TransactionalInsert(entity3);
            RoleOperation entity4 = new RoleOperation()
            {
              Operation = entity3,
              Role = role
            };
            repositoryFactory.GetRepository<RoleOperation>().TransactionalInsert(entity4);
          }
        }
        if (repositoryFactory.GetRepository<UserRole>().FirstOrDefault((Expression<Func<UserRole, bool>>) (x => x.Role.Id == role.Id && x.User.Id == localUser.Id)) == null)
        {
          UserRole entity5 = new UserRole()
          {
            Role = role,
            User = localUser
          };
          repositoryFactory.GetRepository<UserRole>().TransactionalInsert(entity5);
          if (repositoryFactory.GetRepository<RoleOperation>().GetAll().FirstOrDefault<RoleOperation>() == null)
          {
            foreach (Operation operation in (IEnumerable<Operation>) repositoryFactory.GetRepository<Operation>().GetAll())
            {
              RoleOperation entity6 = new RoleOperation()
              {
                Operation = operation,
                Role = role
              };
              repositoryFactory.GetRepository<RoleOperation>().TransactionalInsert(entity6);
            }
          }
        }
        repositoryFactory.GetSession().Transaction.Commit();
      }
      catch (Exception ex)
      {
        ISession session = repositoryFactory.GetSession();
        if (session.IsOpen && session.Transaction.IsActive)
          session.Transaction.Rollback();
        throw;
      }
    }

    public static void NotifyUserIfLicenseInvalidOffline(IRepositoryFactory _repositoryFactory)
    {
      IRepository<TechnicalParameter> repository = _repositoryFactory.GetRepository<TechnicalParameter>();
      TechnicalParameter technicalParameter = repository.FirstOrDefault((Expression<Func<TechnicalParameter, bool>>) (tp => true));
      LicenseNotification licenseNotification = LicenseHelper.ShouldNotifyUserAboutOfflineValidity(repository, technicalParameter, LicenseHelper.GetValidHardwareKey());
      if (!licenseNotification.ShowNotification)
        return;
      int num = (int) MessageBox.Show(string.Format(MSS.Localisation.Resources.MSS_Client_LicenseExpiresOffline_UserInformation_Message, (object) DateTime.Now.AddDays((double) (licenseNotification.NumberOfDays + 1)).ToShortDateString()), MSS.Localisation.Resources.MSS_Client_LicenseExpiresOffline_UserInformation_Title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
      _repositoryFactory.GetRepository<TechnicalParameter>().Update(technicalParameter);
    }

    public static void NotifyUserIfLicenseExpires(IRepositoryFactory _repositoryFactory)
    {
      IRepository<TechnicalParameter> repository = _repositoryFactory.GetRepository<TechnicalParameter>();
      TechnicalParameter technicalParameter = repository.FirstOrDefault((Expression<Func<TechnicalParameter, bool>>) (tp => true));
      LicenseNotification licenseNotification = LicenseHelper.ShouldNotifyUserAboutLicenseExpire(repository, technicalParameter, LicenseHelper.GetValidHardwareKey());
      if (!licenseNotification.ShowNotification)
        return;
      int num = (int) MessageBox.Show(string.Format(MSS.Localisation.Resources.MSS_Client_LicenseExpires_UserInformation_Message, (object) licenseNotification.NumberOfDays), MSS.Localisation.Resources.MSS_Client_LicenseExpires_UserInformation_Title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
      technicalParameter.LastReminderForLicenseExpire = new DateTime?(DateTime.Today);
      repository.Update(technicalParameter);
    }

    public static void UpdateLicense(this IRepositoryFactory _repositoryFactory)
    {
      DownloadLicenseResponse downloadLicenseResponse = new DownloadLicenseResponse();
      try
      {
        downloadLicenseResponse = LicenseWebApiHandler.DownloadDocument(MSS.Business.Utils.AppContext.Current.TechnicalParameters.CustomerNumber, LicenseHelper.GetValidHardwareKey());
      }
      catch (Exception ex)
      {
        MessageHandler.LogException(ex);
      }
      if (!_repositoryFactory.GetSession().IsOpen)
        _repositoryFactory = DIConfigurator.GetConfigurator().Get<IRepositoryFactory>();
      byte[] licenseAsByteArray = LicenseHelper.GetLicenseAsByteArray();
      if (!downloadLicenseResponse.IsSuccessfullyDownloaded)
        return;
      if (downloadLicenseResponse.FullLicenseBytes.Length != 0)
      {
        if (licenseAsByteArray.Length != 0)
        {
          if (!StructuralComparisons.StructuralEqualityComparer.Equals((object) downloadLicenseResponse.FullLicenseBytes, (object) licenseAsByteArray))
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Delegate) (() =>
            {
              if (MessageBox.Show(Application.Current.Windows[0], MSS.Localisation.Resources.MSS_Client_License_UserConfirmation_Message, MSS.Localisation.Resources.MSS_Client_License_UserConfirmation_Title, MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;
              MSSUIHelper.ReplaceCurrentLicenseWithTheOneFromTheServer(downloadLicenseResponse.FullLicenseBytes);
              MSSUIHelper.UpdateTheLastConnectionToServerDate(_repositoryFactory);
            }));
          else
            MSSUIHelper.UpdateTheLastConnectionToServerDate(_repositoryFactory);
        }
        else
        {
          MSSUIHelper.ReplaceCurrentLicenseWithTheOneFromTheServer(downloadLicenseResponse.FullLicenseBytes);
          MSSUIHelper.UpdateTheLastConnectionToServerDate(_repositoryFactory);
        }
      }
      else
      {
        MSSUIHelper.DeleteCurrentLicense();
        MSSUIHelper.UpdateTheLastConnectionToServerDate(_repositoryFactory);
      }
    }

    public static void UpdateTheApplicationVersionInformation()
    {
      try
      {
        LicenseWebApiHandler.UpdateTheApplicationVersionInformation(MSS.Business.Utils.AppContext.Current.TechnicalParameters.CustomerNumber, LicenseHelper.GetValidHardwareKey(), ConfigurationManager.AppSettings["ApplicationVersion"]);
      }
      catch (Exception ex)
      {
        MessageHandler.LogException(ex);
      }
    }

    private static void DeleteCurrentLicense()
    {
      File.Delete(Path.Combine(AppDataFolderHelper.GetUserAppDataPath(), LicenseHelper.GetLicenseFileName(LicenseHelper.GetValidHardwareKey())));
    }

    private static void ReplaceCurrentLicenseWithTheOneFromTheServer(byte[] licenseFileAsByteArray)
    {
      licenseFileAsByteArray.FromByteArrayToMemoryStream().SaveToDisk(Path.Combine(AppDataFolderHelper.GetUserAppDataPath(), LicenseHelper.GetLicenseFileName(LicenseHelper.GetValidHardwareKey())));
    }

    private static void UpdateTheLastConnectionToServerDate(IRepositoryFactory _repositoryFactory)
    {
      if (!_repositoryFactory.GetSession().IsOpen)
        _repositoryFactory = DIConfigurator.GetConfigurator().Get<IRepositoryFactory>();
      IRepository<TechnicalParameter> repository = _repositoryFactory.GetRepository<TechnicalParameter>();
      TechnicalParameter entity = repository.FirstOrDefault((Expression<Func<TechnicalParameter, bool>>) (tp => true));
      entity.LastConnectionToLicenseServer = new DateTime?(DateTime.Today);
      repository.Update(entity);
    }

    public static bool ShowSendChangesWarningDialog_AtLogout()
    {
      bool flag = true;
      if (!MSS.Business.Utils.AppContext.Current.IsClientUpToDateSend)
        flag = MessageHandlingManager.ShowCustomMessageDialog(CultureResources.GetValue("MSS_Client_Message_ChangesNotCommited_Title"), CultureResources.GetValue("MSS_Client_Message_ChangesNotCommited_LogOutMessage"));
      return flag;
    }

    public static bool ShowSendChangesWarningDialog_AtApplicationClose(
      IRepositoryFactory repositoryFactory,
      bool isLogOutOk)
    {
      if (isLogOutOk)
        return true;
      bool flag = true;
      if (GmmInterface.JobManager.Jobs == null || GmmInterface.JobManager.Jobs.Count == 0)
        return flag;
      if (!MessageHandlingManager.ShowCustomMessageDialog(CultureResources.GetValue("MSS_Client_Message_Jobs_Started_Title"), CultureResources.GetValue("MSS_Client_Message_Jobs_Started_Message")))
        return false;
      List<MssReadingJob> jobs = new List<MssReadingJob>();
      GmmInterface.JobManager.Jobs.ForEach((Action<Job>) (x =>
      {
        MssReadingJob byId = repositoryFactory.GetRepository<MssReadingJob>().GetById((object) x.JobID);
        byId.Status = JobStatusEnum.Inactive;
        jobs.Add(byId);
      }));
      GMMJobsManager gmmJobsManager = GMMJobsManager.Instance(DIConfigurator.GetConfigurator().Get<IRepositoryFactoryCreator>(), false);
      foreach (MssReadingJob mssReadingJob in jobs)
      {
        HibernateMultipleDatabasesManager.Update((object) mssReadingJob, repositoryFactory.GetSession());
        gmmJobsManager.RemoveJob(mssReadingJob);
      }
      return true;
    }

    public static bool ShowSendChangesWarningDialog_AtApplicationClose(
      IRepositoryFactory repositoryFactory)
    {
      bool flag = true;
      if (GmmInterface.JobManager.Jobs == null || GmmInterface.JobManager.Jobs.Count == 0)
        return flag;
      if (!MessageHandlingManager.ShowCustomMessageDialog(CultureResources.GetValue("MSS_Client_Message_Jobs_Started_Title"), CultureResources.GetValue("MSS_Client_Message_Jobs_Started_Message")))
        return false;
      List<MssReadingJob> jobs = new List<MssReadingJob>();
      GmmInterface.JobManager.Jobs.ForEach((Action<Job>) (x =>
      {
        MssReadingJob byId = repositoryFactory.GetRepository<MssReadingJob>().GetById((object) x.JobID);
        byId.Status = JobStatusEnum.Inactive;
        jobs.Add(byId);
      }));
      GMMJobsManager.Instance(DIConfigurator.GetConfigurator().Get<IRepositoryFactoryCreator>(), false).FinalizeJobs((IEnumerable<MssReadingJob>) jobs);
      return true;
    }

    public static bool? ShowWarningDialog(
      IWindowFactory windowFactory,
      string warningTitle,
      string warningMessage,
      bool isCancelButtonVisible)
    {
      GenericMessageViewModel messageViewModel = DIConfigurator.GetConfigurator().Get<GenericMessageViewModel>((IParameter) new ConstructorArgument("title", (object) warningTitle), (IParameter) new ConstructorArgument("message", (object) warningMessage), (IParameter) new ConstructorArgument(nameof (isCancelButtonVisible), (object) isCancelButtonVisible));
      return windowFactory.CreateNewModalDialog((IViewModel) messageViewModel);
    }

    public static bool IsUserAllowedToLogin()
    {
      return Settings.Default.NoOfLoginAttempts < (short) 3 || DateTime.Now.Ticks - Settings.Default.LastLoginAttempt.Ticks >= 3000000000L;
    }
  }
}
