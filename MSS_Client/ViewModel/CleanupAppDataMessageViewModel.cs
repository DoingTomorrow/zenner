// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.CleanupAppDataMessageViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Modules.Cleanup;
using MSS.Business.Utils;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.PartialSync.PartialSync.Managers;

#nullable disable
namespace MSS_Client.ViewModel
{
  internal class CleanupAppDataMessageViewModel : GenericMessageConfirmationViewModel
  {
    private IRepositoryFactory _repositoryFactory;

    public CleanupAppDataMessageViewModel(IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this.DialogTitle = CultureResources.GetValue("MSS_DIALOG_TITLE_CLEANUP");
      this.DialogMessage = CultureResources.GetValue("MSS_DIALOG_MESSAGE_CleanupSyncDataOnChangingUser");
      this.OKButtonLabel = CultureResources.GetValue("MSS_MessageCodes_OK");
      this.CancelButtonLabel = CultureResources.GetValue("MSS_Cancel");
    }

    public override bool ExecuteOkButtonAction()
    {
      if (!new ClientPartialSyncManager().Send(AppContext.Current.LoggedUser.Id))
        return false;
      new DatabaseCleanupManager(this._repositoryFactory).CleanupDatabaseOnChangingLoggedUser();
      return true;
    }
  }
}
