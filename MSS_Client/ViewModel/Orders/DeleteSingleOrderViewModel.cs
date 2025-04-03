// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Orders.DeleteSingleOrderViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Modules.OrdersManagement;
using MSS.DTO.Orders;
using MSS.Interfaces;
using MVVM.Commands;
using MVVM.ViewModel;
using Ninject;
using System;
using System.Collections.Generic;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Orders
{
  public class DeleteSingleOrderViewModel : ViewModelBase
  {
    private readonly List<OrderDTO> _selectedOrders;
    private readonly IRepositoryFactory _repositoryFactory;
    private List<string> _installationNumberValue = new List<string>();
    private List<string> _installationNumberValueSkiped = new List<string>();
    private bool _isdeleteSkiped;
    private string _windowHeight;
    private string _gridHeight;

    public string DeleteOrderTitle { get; set; }

    [Inject]
    public DeleteSingleOrderViewModel(
      List<OrderDTO> selectedOrders,
      string deleteOrderTitle,
      IRepositoryFactory repositoryFactory)
    {
      this.DeleteOrderTitle = deleteOrderTitle;
      this._repositoryFactory = repositoryFactory;
      this._selectedOrders = selectedOrders;
      this.IsDeleteSkiped = false;
      this._selectedOrders.ForEach((Action<OrderDTO>) (x =>
      {
        Guid? lockedBy = x.LockedBy;
        Guid empty = Guid.Empty;
        int num;
        if ((lockedBy.HasValue ? (lockedBy.HasValue ? (lockedBy.GetValueOrDefault() == empty ? 1 : 0) : 1) : 0) == 0)
        {
          lockedBy = x.LockedBy;
          num = !lockedBy.HasValue ? 1 : 0;
        }
        else
          num = 1;
        if (num != 0)
        {
          this.InstallationNumberValue.Add(x.InstallationNumber);
        }
        else
        {
          this.IsDeleteSkiped = true;
          this.InstallationNumberValueSkiped.Add(x.InstallationNumber);
        }
      }));
      if (this.InstallationNumberValueSkiped.Count == 0)
      {
        this.WindowHeight = "250";
        this.GridHeight = "0";
      }
      else
      {
        this.WindowHeight = "380";
        this.GridHeight = "140";
      }
    }

    private OrdersManager GetOrderManagerInstance() => new OrdersManager(this._repositoryFactory);

    public List<string> InstallationNumberValue
    {
      get => this._installationNumberValue;
      set => this._installationNumberValue = value;
    }

    public List<string> InstallationNumberValueSkiped
    {
      get => this._installationNumberValueSkiped;
      set => this._installationNumberValueSkiped = value;
    }

    public bool IsDeleteSkiped
    {
      get => this._isdeleteSkiped;
      set
      {
        this._isdeleteSkiped = value;
        this.OnPropertyChanged(nameof (IsDeleteSkiped));
      }
    }

    public string WindowHeight
    {
      get => this._windowHeight;
      set
      {
        this._windowHeight = value;
        this.OnPropertyChanged(nameof (WindowHeight));
      }
    }

    public string GridHeight
    {
      get => this._gridHeight;
      set
      {
        this._gridHeight = value;
        this.OnPropertyChanged(nameof (GridHeight));
      }
    }

    public ICommand DeleteOrderCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          OrdersManager orderManager = this.GetOrderManagerInstance();
          this._selectedOrders.ForEach((Action<OrderDTO>) (x =>
          {
            Guid? lockedBy = x.LockedBy;
            Guid empty = Guid.Empty;
            int num;
            if ((lockedBy.HasValue ? (lockedBy.HasValue ? (lockedBy.GetValueOrDefault() == empty ? 1 : 0) : 1) : 0) == 0)
            {
              lockedBy = x.LockedBy;
              num = !lockedBy.HasValue ? 1 : 0;
            }
            else
              num = 1;
            if (num == 0)
              return;
            orderManager.DeleteOrder(x.Id);
          }));
          this.OnRequestClose(true);
        });
      }
    }
  }
}
