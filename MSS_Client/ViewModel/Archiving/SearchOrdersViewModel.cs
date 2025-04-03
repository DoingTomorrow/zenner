// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Archiving.SearchOrdersViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Errors;
using MSS.Business.Utils;
using MSS.DTO.MessageHandler;
using MSS.Utils.Utils;
using MSSArchive.Core.Model.Archiving;
using MSSArchive.Core.Model.Orders;
using MVVM.Commands;
using MVVM.ViewModel;
using Ninject;
using System;
using System.Linq.Expressions;
using System.Windows.Input;
using Telerik.Windows.Data;

#nullable disable
namespace MSS_Client.ViewModel.Archiving
{
  public class SearchOrdersViewModel : ViewModelBase
  {
    public VirtualQueryableCollectionView<ArchiveOrder> _archiveOrdersDTO;

    private ArchiveInformation ArchiveInformation { get; set; }

    [Inject]
    public SearchOrdersViewModel(ArchiveInformation archiveInformation)
    {
      this.ArchiveInformation = archiveInformation;
    }

    private void IntializeCollection()
    {
      this.ArchiveOrdersDTO = MSSHelper.LoadCollection<ArchiveOrder>();
    }

    public ICommand SearchOrderByInstallationNumberCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          string searchText = parameter as string;
          this.ArchiveOrdersDTO = MSSHelper.LoadCollection<ArchiveOrder>((Expression<Func<ArchiveOrder, bool>>) (mrv => mrv.InstallationNumber.Contains(searchText)));
          if (this.ArchiveOrdersDTO.Count != 0)
            return;
          MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
          {
            MessageType = MessageTypeEnum.Warning,
            MessageText = MessageCodes.No_Item_found.GetStringValue()
          };
        }));
      }
    }

    public VirtualQueryableCollectionView<ArchiveOrder> ArchiveOrdersDTO
    {
      get => this._archiveOrdersDTO;
      set
      {
        this._archiveOrdersDTO = value;
        this.OnPropertyChanged(nameof (ArchiveOrdersDTO));
      }
    }
  }
}
