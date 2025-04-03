// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Orders.OrderMessagesViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Core.Model.Orders;
using MVVM.ViewModel;
using System.Collections.Generic;

#nullable disable
namespace MSS_Client.ViewModel.Orders
{
  public class OrderMessagesViewModel : ViewModelBase
  {
    private List<OrderMessage> _orderMessages;

    public OrderMessagesViewModel(List<OrderMessage> orderMessages)
    {
      this._orderMessages = orderMessages;
    }

    public List<OrderMessage> OrderMessages
    {
      get => this._orderMessages;
      set
      {
        this._orderMessages = value;
        this.OnPropertyChanged(nameof (OrderMessages));
      }
    }
  }
}
