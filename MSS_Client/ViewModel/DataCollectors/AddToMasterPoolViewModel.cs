// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.DataCollectors.AddToMasterPoolViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Interfaces;

#nullable disable
namespace MSS_Client.ViewModel.DataCollectors
{
  public class AddToMasterPoolViewModel : MasterPoolViewModelBase
  {
    public AddToMasterPoolViewModel(IRepositoryFactory repositoryFactory)
      : base(repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
    }
  }
}
